using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace MyProject;

class Program
{
    private const string _clientId = "bf086018-f1eb-48a3-873f-a50ab641a0d8";
    private const string _tenantId = "80294c2e-7d09-469f-95e7-d0e75197f067";

    public static async Task Main(string[] args)
    {
        var app = PublicClientApplicationBuilder
            .Create(_clientId)
            .WithAuthority(AzureCloudInstance.AzurePublic, _tenantId)
            .WithRedirectUri("http://localhost")
            .Build();
        
        string[] scopes = {"user.read"};
        AuthenticationResult result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();
        Console.WriteLine($"Token:\t{result.AccessToken}");
    }
}