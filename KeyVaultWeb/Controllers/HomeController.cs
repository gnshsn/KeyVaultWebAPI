using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KeyVaultWeb.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using KeyVaultWeb.Utility;
using Microsoft.AspNetCore.Http;

namespace KeyVaultWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private string _baseadres = "https://localhost:44399/api/Account/";
        private string sessionUsername = "userid";
        private string userid;
        static HttpClient client = new HttpClient();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            if (client.BaseAddress == null)
            {
                client.BaseAddress = new Uri(_baseadres);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        public IActionResult Index()
        {
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

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            //HTTP POST
            var postTask = client.PostAsJsonAsync<LoginViewModel>("Login", model);
            postTask.Wait();
            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                UserLogViewModel uLogin = new UserLogViewModel();
                var responseTask2 = client.GetAsync($"GetUserID/{model.Email}");
                responseTask2.Wait();
                var result2 = responseTask2.Result;
                var read2 = result2.Content.ReadAsAsync<string>();
                read2.Wait();
                userid = read2.Result;
                HttpContext.Session.SetString(sessionUsername, userid);
                uLogin.Username = model.Email;
                uLogin.UserId = userid;
                uLogin.Id = Guid.NewGuid().ToString();
                uLogin.LoginTime = DateTime.Now;
                var postTask3 = client.PostAsJsonAsync<UserLogViewModel>("SetUserLog", uLogin);
                postTask3.Wait();

                var result3 = postTask3.Result;
                if (!result3.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Could not register the user log");
                }
                _logger.LogInformation("User Logged in");
                return RedirectToAction("Keys", "Key");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Registiration()
        {
            return View("Registiration");
        }

        [HttpPost]
        [Route("Registiration")]
        [AllowAnonymous]
        public ActionResult Registiration(RegisterBindingModel model)
        {
            //HTTP POST
            var postTask = client.PostAsJsonAsync<RegisterBindingModel>("Register", model);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "Home");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            //HTTP POST
            var postTask = client.GetAsync("Logout");
            postTask.Wait();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Login), "Home");

        }
    }
}
