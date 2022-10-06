using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using WebAppKVDemo.Models;

namespace WebAppKVDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                // Read secret1 from Azure Key Vault
                string kvUri = "https://kz-az204.vault.azure.net/";
                SecretClient client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

                string secret = client.GetSecretAsync("secretColour", "f06aad4b58894fff9928586921fc3d42").Result.Value.Value;
                ViewBag.secretColour = secret;
            }
            catch (Exception exception)
            {
                ViewBag.error = exception.Message;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}