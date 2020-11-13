using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using KeyVaultWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Web.WebPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace KeyVaultWeb.Controllers
{
    public class KeyController : Controller
    {
        private string _baseadres = "https://localhost:44399/api/Key/";
        private static string sessionUserid = "userid";
        private static string userid;
        static HttpClient client = new HttpClient();
        public KeyController()
        {
            if (client.BaseAddress == null)
            {
                client.BaseAddress = new Uri(_baseadres);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        [HttpGet]
        [Route("Keys")]
        public IActionResult Keys()
        {
            IEnumerable<KeyViewModel> keys = null;
            UserLogViewModel userModel = null;
            userid = HttpContext.Session.GetString(key: sessionUserid);
            var responseTask = client.GetAsync($"GetKeys/{userid}");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<KeyViewModel>>();
                readTask.Wait();
                keys = readTask.Result;
            }
            else
            {
                keys = Enumerable.Empty<KeyViewModel>();
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            var responseTask2 = client.GetAsync($"GetUserLog/{userid}");
            responseTask2.Wait();
            var result2 = responseTask2.Result;
            var read2 = result2.Content.ReadAsAsync<UserLogViewModel>();
            read2.Wait();
            userModel = read2.Result;

            var tupleData = new Tuple<IEnumerable<KeyViewModel>, UserLogViewModel>(keys, userModel);
            return View(tupleData);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KeyViewModel model)
        {

            userid = HttpContext.Session.GetString(key: sessionUserid);
            model.UserId = userid;
            //HTTP POST
            var postTask = client.PostAsJsonAsync<KeyViewModel>("CreateAsync", model);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Keys", "Key");
            }

            ModelState.AddModelError(string.Empty, "An error occured. Please try again.");
            return View(model);
        }

        [HttpGet]
        [Route("GetKey/{id}")]
        public ActionResult GetKey(string id)
        {
            KeyViewModel key = null;

            var responseTask = client.GetAsync($"GetKey/{id}");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<KeyViewModel>();
                readTask.Wait();

                key = readTask.Result;
            }
            else
            {

                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }

            return View(key);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateKey(KeyViewModel model)
        {

            var postTask = client.PostAsJsonAsync<KeyViewModel>("UpdateKey", model);
            postTask.Wait();
            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Keys", "Key");
            }

            return RedirectToAction($"GetKey/{model.Id}");
        }
        public ActionResult DeleteKey(string id)
        {
            KeyViewModel key = null;
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "Record could not found");
            }

            var responseTask = client.GetAsync($"GetKey/{id}");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<KeyViewModel>();
                readTask.Wait();

                key = readTask.Result;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }

            return View(key);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {

            var responseTask = client.DeleteAsync($"DeleteKey/{id}");
            responseTask.Wait();
            var result = responseTask.Result;
            if (!result.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }

            return RedirectToAction("Keys");
        }

        [HttpGet]
        public PartialViewResult ShareKey(string id)
        {
            IEnumerable<UserListViewModel> users = null;
            var userModel = new UsernameViewModel();
            userModel.keyId = id;
            var list = new List<SelectListItem>();

            var responseTask = client.GetAsync("GetUsers");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<UserListViewModel>>();
                readTask.Wait();
                users = readTask.Result;
                foreach (var item in users)
                {
                    var t = new SelectListItem()
                    {
                        Value = item.UserId,
                        Text = item.UserName
                    };
                    list.Add(t);
                }
                userModel.allUsers = list;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }

            return PartialView(userModel);
        }
        [HttpPost, ActionName("ShareKey")]
        public ActionResult ShareToUser(UsernameViewModel model)
        {
            KeyViewModel key = null;

            var responseTask = client.GetAsync($"GetKey/{model.keyId}");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<KeyViewModel>();
                readTask.Wait();

                key = readTask.Result;
            }
            else
            {

                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            //duplicate the key for new user
            key.UserId = model.SelectedUser;
            var postTask2 = client.PostAsJsonAsync<KeyViewModel>("CreateAsync", key);
            postTask2.Wait();
            var result2 = postTask2.Result;
            if (result2.IsSuccessStatusCode)
            {
                return RedirectToAction("Keys", "Key");
            }

            return RedirectToAction("ShareKey", key.Id);
        }

        [HttpGet, ActionName("Filter")]
        public ActionResult Filter(string filter,string searchString, string model)
        {
            IEnumerable<KeyViewModel> sortModel;
            var DeserializedModel = JsonConvert.DeserializeObject<IEnumerable<KeyViewModel>>(model);
            sortModel = DeserializedModel;
            if (searchString != null)
            {
                searchString = null;
            }
            switch (filter)
            {
                case "username":
                    sortModel = sortModel.OrderBy(x => x.Username).ToList();
                    break;
                case "expdate":
                    sortModel = sortModel.OrderBy(x => x.ExpirationDate).ToList();
                    break;
                case "crtdate":
                    sortModel = sortModel.OrderBy(x => x.CreateDate).ToList();
                    break;
                default:
                    break;
            }

            return PartialView("_PartialKeyList", sortModel);
        }
    }
}