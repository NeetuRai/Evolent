using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data.Entity;
using System.Web.Http;
using EmployeeDataAccess;

namespace WebAPIDemo.Controllers
{
    public class EmployeesController : ApiController
    {
        [HttpGet]
        public IEnumerable<ContactInfo> ListContacts()
        {
            using (ContactDBEntities entities = new ContactDBEntities())
            {
                return entities.ContactInfoes.ToList();
            }
        }

        [HttpGet]
        public HttpResponseMessage ListContactsById(int id)
        {
            using (ContactDBEntities entities = new ContactDBEntities())
            {
                var entity = entities.ContactInfoes.FirstOrDefault(e => e.Id == id);

                if(entity!= null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id = " + id + "is not found");
                }
            }
        }

       // [HttpPost]
        public HttpResponseMessage Post([FromBody] ContactInfo contact)
        {
            try
            {
                using (ContactDBEntities entities = new ContactDBEntities())
                {
                    entities.ContactInfoes.Add(contact);
                    entities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, contact);
                    message.Headers.Location = new Uri(Request.RequestUri + contact.Id.ToString());
                    return message;
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

       // [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
            using (ContactDBEntities entities = new ContactDBEntities())
            {
                var entity = entities.ContactInfoes.FirstOrDefault(e => e.Id == id);
                if (entity != null)
                {
                    entities.ContactInfoes.Remove(entity);
                    entities.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id = " + id + "is not found to delete");
                }
                
            }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

       // [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]ContactInfo contactInfo)
        {
            try
            {
                using (ContactDBEntities entities = new ContactDBEntities())
                {
                    var entity = entities.ContactInfoes.FirstOrDefault(e => e.Id == id);

                    if(entity !=null)
                    {
                        entity.FirstName = contactInfo.FirstName;
                        entity.LastName = contactInfo.LastName;
                        entity.PhoneNumber = contactInfo.PhoneNumber;
                        entity.ContactStatus = contactInfo.ContactStatus;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id = " + id + "is not found to update");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
