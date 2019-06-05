using EmployeeDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPIDemo.Controllers
{
    public class EmployeesController : ApiController
    {
        public IEnumerable<Employee> Get()
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
               return  entities.Employees.ToList();
            }
        }

        public HttpResponseMessage Get(int Id)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var emp= entities.Employees.FirstOrDefault(e => e.Id == Id);
                    if (emp != null)
                        return Request.CreateResponse(HttpStatusCode.OK,emp);
                    else
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,"Employee with Id "+Id.ToString()+" Not Found");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.Id.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,ex);
            }
        }

        public HttpResponseMessage Delete(int Id)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var emp = entities.Employees.FirstOrDefault(e => e.Id == Id);
                    if (emp == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id = " + Id.ToString() + " Not found");
                    }
                    else
                    {
                        entities.Employees.Remove(emp);
                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(int Id,[FromBody] Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var emp = entities.Employees.FirstOrDefault(e => e.Id == Id);
                    if (emp == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id = " + Id.ToString() + " Not found");
                    }
                    else
                    {
                        emp.FirstName = employee.FirstName;
                        emp.LastName = employee.LastName;
                        emp.Gender = employee.Gender;
                        emp.Salary = employee.Salary;

                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK,emp);
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
