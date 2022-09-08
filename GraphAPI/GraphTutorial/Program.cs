Console.WriteLine(".NET Graph Tutorial\n");

var settings = Settings.LoadSettings();

//Init graph
InitializeGraph(settings);

//Greet the user by name
await GreetUserAsync();

int choice -1;

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
    catch (System.FormatException)
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

void InitializeGraph()
{
    //TODO
}

async Task GreetUserAsync()
{
    //TODO
}

async Task DisplayAccessTokenAsync()
{
    //TODO
}
async Task ListInboxAsync()
{
    // TODO
}

async Task SendMailAsync()
{
    // TODO
}

async Task ListUsersAsync()
{
    // TODO
}

async Task MakeGraphCallAsync()
{
    // TODO
}
