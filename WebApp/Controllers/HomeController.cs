using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        static HomeController()
        {
            //replace with current local host
            client.BaseAddress = new Uri("https://localhost:44386/api/");

        }
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult GetIndex()
        {
            List<Customer> customers = FetchCustomerDetailsAsync();
            return View(customers);
        }

        private List<Customer> FetchCustomerDetailsAsync()
        {
            List<Customer> customers = new List<Customer>();

            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri("https://localhost:44386/api/");

            //Called Member default GET All records  
            //GetAsync to send a GET request   
            // PutAsync to send a PUT request  
            var responseTask = client.GetAsync("values");
            responseTask.Wait();
            //To store result of web api response.   
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<Customer>>();
                readTask.Wait();

                customers = readTask.Result;
            }
            else
            {
                //Error response received   
                customers = null;
                ModelState.AddModelError(string.Empty, "Server error try after some time.");
            }
            //  }

            return customers;
        }
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            var responseTask = client.PostAsJsonAsync("values", customer);
            responseTask.Wait();
            //To store result of web api response.   
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("GetIndex");
            }
            else
            {
                string errorMessage = result.Content.ReadAsStringAsync().ToString();
                ModelState.AddModelError(string.Empty, errorMessage);
                return View();
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Customer customer = FetchCustomerByIdAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        [HttpPost]
        public ActionResult Edit(int id, Customer customer)
        {
            var responseTask = client.PutAsJsonAsync($"values/{id}", customer);
            responseTask.Wait();
            //To store result of web api response.   
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("GetIndex");
            }
            else
            {
                string errorMessage = result.Content.ReadAsStringAsync().ToString();
                ModelState.AddModelError(string.Empty, errorMessage);
                return View();
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Customer customer = FetchCustomerByIdAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteCusromer(int id)
        {
            var responseTask = client.DeleteAsync($"values/{id}");
            responseTask.Wait();
            //To store result of web api response.   
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("GetIndex");
            }
            else
            {
                string errorMessage = result.Content.ReadAsStringAsync().ToString();
                ModelState.AddModelError(string.Empty, errorMessage);
                return View();
            }
        }
        private Customer FetchCustomerByIdAsync(int id)
        {
            Customer customer = null;
            var responseTask = client.GetAsync($"values/{id}");
            responseTask.Wait();
            //To store result of web api response.   
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<Customer>();
                readTask.Wait();

                customer = readTask.Result;
            }
            return customer;
        }


    }
}
