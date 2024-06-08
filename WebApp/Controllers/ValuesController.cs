using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        PineWoodTestEntities pineWoodTestEntities = new PineWoodTestEntities();
        public IEnumerable<Customer> Get()
        {
            return pineWoodTestEntities.Customers.ToList();
        }

        // GET api/values/5
        public Customer Get(int id)
        {
            return pineWoodTestEntities.Customers.Where(a => a.Id == id).FirstOrDefault(); ;
        }

        // POST api/values
        public IHttpActionResult Post(Customer customer)
        {
            var checkEmailExists = pineWoodTestEntities.Customers.Where(a => a.Email == customer.Email).FirstOrDefault();
            if (checkEmailExists == null)
            {
                pineWoodTestEntities.Customers.Add(customer);
                try
                {
                    pineWoodTestEntities.SaveChanges();
                }
                catch (Exception ex)
                {
                    return Content(HttpStatusCode.InternalServerError, $"Error saving customer: {ex.Message}");
                }


            }
            else
            {
                return Content(HttpStatusCode.BadRequest, "A customer with the same email already exists.");
            }
            return Ok(customer);
        }

        // PUT api/values/5
        public IHttpActionResult Put(int id, Customer customer)
        {
            var checkEmailExists = pineWoodTestEntities.Customers.Where(a => a.Id == customer.Id).FirstOrDefault();
            if (checkEmailExists != null)
            {
                checkEmailExists.Email = customer.Email;
                checkEmailExists.Address = customer.Address;
                checkEmailExists.Name = customer.Name;
                pineWoodTestEntities.Entry(checkEmailExists).State = System.Data.Entity.EntityState.Modified;
                try
                {
                    pineWoodTestEntities.SaveChanges();
                }
                catch (Exception ex)
                {
                    return Content(HttpStatusCode.InternalServerError, $"Error updating customer: {ex.Message}");
                }
            }
            else
            {
                return Content(HttpStatusCode.NotFound, "Customer not found.");
            }
            return Ok(customer);
        }

        // DELETE api/values/5
        public IHttpActionResult Delete(int id)
        {
            var customerToBeDeleted = pineWoodTestEntities.Customers.Where(a => a.Id == id).FirstOrDefault();
            if (customerToBeDeleted != null)
            {
                pineWoodTestEntities.Customers.Remove(customerToBeDeleted);
                pineWoodTestEntities.SaveChanges();
                try
                {
                    pineWoodTestEntities.SaveChanges();
                }
                catch (Exception ex)
                {
                    return Content(HttpStatusCode.InternalServerError, $"Error deleting customer: {ex.Message}");
                }
            }
            else
            {
                return Content(HttpStatusCode.NotFound, "Customer not found.");
            }
            return Ok();
        }
    }
}
