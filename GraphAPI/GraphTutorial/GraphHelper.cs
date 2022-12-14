using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;

class GraphHelper
{
    // Settings object
    private static Settings? _settings;
    // User auth token credential
    private static DeviceCodeCredential? _deviceCodeCredential;
    // Client configured with user authentication
    private static GraphServiceClient? _userClient;

    public static void InitialiseGraphForUserAuth(Settings settings, Func<DeviceCodeInfo, CancellationToken, Task> deviceCodePrompt)
    {
        _settings = settings;
        _deviceCodeCredential = new DeviceCodeCredential(deviceCodePrompt, settings.AuthTenant, settings.ClientId);
        _userClient = new GraphServiceClient(_deviceCodeCredential, settings.GraphUserScopes);
    }

    public static async Task<string> GetUserTokenAsync()
    {
        // Ensure credential isn't null
        if (_deviceCodeCredential is null)
        {
            throw new System.NullReferenceException("Graph has not been initialised for user auth");
        }
        // Ensure scope isn't null
        if (_settings?.GraphUserScopes is null)
        {
            throw new System.ArgumentNullException("Argument 'scopes' cannot be null");
        }
        // Request token with given scopes
        var context = new TokenRequestContext(_settings.GraphUserScopes);
        var response = await _deviceCodeCredential.GetTokenAsync(context);
        return response.Token;
    }

    public static async Task<User> GetUserAsync()
    {
        // Ensure client isn't null
        if (_userClient is null)
        {
            throw new System.NullReferenceException("Graph has not been initialised for user auth.");
        }
        return await _userClient.Me
            .Request()
            .Select(u => new
            {
                // Only request specific properties
                u.DisplayName,
                u.Mail,
                u.UserPrincipalName
            })
            .GetAsync();
    }

    public static Task<IMailFolderMessagesCollectionPage> GetInboxAsync()
    {
        if (_userClient is null)
        {
            throw new System.NullReferenceException("Graph has not been initialised for user auth");
        }
        return _userClient.Me
            // Only messages from Inbox folder
            .MailFolders["Inbox"]
            .Messages
            .Request()
            .Select(m => new
            {
                // Only request specific properties
                m.From,
                m.IsRead,
                m.ReceivedDateTime,
                m.Subject
            })
            // Get at most 25 results
            .Top(25)
            // Sort by received time, newest first
            .OrderBy("ReceivedDateTime DESC")
            .GetAsync();
    }

    public static async Task SendMailAsync(string subject, string body, string recipient)
    {
        if (_userClient is null)
        {
            throw new System.NullReferenceException("Graph has not been initialised for user auth");
        }
        // Create a new message
        var message = new Message
        {
            Subject = subject,
            Body = new ItemBody
            {
                Content = body,
                ContentType = BodyType.Text
            },
            ToRecipients = new Recipient[]
            {
                new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = recipient
                    }
                }
            }
        };
        // Send the message
        await _userClient.Me
            .SendMail(message)
            .Request()
            .PostAsync();
    }

}