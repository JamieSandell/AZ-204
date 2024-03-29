﻿internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine(".NET Graph Tutorial\n");

        var settings = Settings.LoadSettings();

        //Init graph
        InitializeGraph(settings);

        //Greet the user by name
        await GreetUserAsync();

        int choice = -1;

        while (choice != 0)
        {
            Console.WriteLine("Please choose one of the following options:");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Display access token");
            Console.WriteLine("2. List my inbox");
            Console.WriteLine("3. Send mail");
            Console.WriteLine("4. List users (requires app-only)");
            Console.WriteLine("5. Make a Graph call");

            try
            {
                choice = int.Parse(Console.ReadLine() ?? string.Empty);
            }
            catch (FormatException)
            {
                // Set to invalid value
                choice = -1;
            }

            switch (choice)
            {
                case 0:
                    //Exit the program
                    Console.WriteLine("Goodbye...");
                    break;
                case 1:
                    //Display access token
                    await DisplayAccessTokenAsync();
                    break;
                case 2:
                    // List emails from user's inbox
                    await ListInboxAsync();
                    break;
                case 3:
                    // Send an email message
                    await SendMailAsync();
                    break;
                case 4:
                    // List users
                    await ListUsersAsync();
                    break;
                case 5:
                    // Run any Graph code
                    await MakeGraphCallAsync();
                    break;
                default:
                    Console.WriteLine("Invalid choice! Please try again.");
                    break;
            }
        }

        void InitializeGraph(Settings settings)
        {
            GraphHelper.InitialiseGraphForUserAuth(settings,
                (info, cancel) =>
                {
                    /* Display the device code message to the user.
                    This tells them where to go to sign in and provides the code to use. */
                    Console.WriteLine(info.Message);
                    return Task.FromResult(0);
                });
        }

        async Task GreetUserAsync()
        {
            try
            {
                var user = await GraphHelper.GetUserAsync();
                Console.WriteLine($"Hello, {user?.DisplayName}!");
                /* For Work/school accounts, email is in Mail property
                Personal accounts, email is in UserPrincipalName*/
                Console.WriteLine($"Email: {user?.Mail ?? user?.UserPrincipalName ?? ""}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting user: {ex.Message}");
            }
        }

        async Task DisplayAccessTokenAsync()
        {
            try
            {
                var userToken = await GraphHelper.GetUserTokenAsync();
                Console.WriteLine($"User token: {userToken}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting user access token: {ex.Message}");
            }
        }
        async Task ListInboxAsync()
        {
            try
            {
                var messagePage = await GraphHelper.GetInboxAsync();
                // Output each message's details
                foreach (var message in messagePage.CurrentPage)
                {
                    Console.WriteLine($"Message: {message.Subject ?? "NO SUBJECT"}");
                    Console.WriteLine($"\tFrom: {message.From?.EmailAddress?.Name}");
                    Console.WriteLine($"\tStatus: {(message.IsRead!.Value ? "Read" : "Unread")}");
                    Console.WriteLine($"\tReceived: {message.ReceivedDateTime?.ToLocalTime().ToString()}");
                }
                /* If NextPageRequest is not null, there are more messages
                availabe on the server. Access the next page like:
                messagePage.NextPageRequest.Async(); */
                var moreAvailable = messagePage.NextPageRequest != null;
                Console.WriteLine($"\nMore messages available? {moreAvailable}");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error getting user's inbox: {exception.Message}");
            }
            
        }

        async Task SendMailAsync()
        {
            try
            {
                /* Send mail to the signed-in user
                Get the user for their email address */
                var user = await GraphHelper.GetUserAsync();
                var userEmail = user?.Mail ?? user?.UserPrincipalName;
                if (string.IsNullOrEmpty(userEmail))
                {
                    Console.WriteLine("Couldn't get your email address, cancelling.");
                    return;
                }
                await GraphHelper.SendMailAsync("Testing Microsoft Graph", "Hello World!", userEmail);
                Console.WriteLine("Mail sent.");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error sending mail: {exception.Message}");
            }
        }

        async Task ListUsersAsync()
        {
            // TODO
        }

        async Task MakeGraphCallAsync()
        {
            // TODO
        }
    }
}