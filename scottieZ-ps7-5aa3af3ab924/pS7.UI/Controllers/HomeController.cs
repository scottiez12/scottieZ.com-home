using System.Web.Mvc;
using System;
using System.Linq;
using System.Net.Mail;
using System.Net;
using pS7.Data.EF;
using pS7.UI.Models;


namespace IdentitySample.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel contact)
        {
            if (ModelState.IsValid)
            {
                //Create a body for the email (words)
                string body = string.Format($"Name: {contact.Name}<br />Email: {contact.Email}" +
                    $"<br />Subject: {contact.Subject}<br /><br />{contact.Message}");

                //Create and configure the Mail message (letter)
                MailMessage msg = new MailMessage
                    ("admin@scottiez.com", //from
                    "ziggish@att.net", //where we are sending to
                    contact.Subject, //subject of the message
                    body);

                msg.IsBodyHtml = true; //body of the message is HTML
                                       //msg.cc.Add("ziggish@att.net");  sends a carbon copy
                                       //msg.Bcc.Add("ziggish@att.net"); //send a blind carbon copy so that no one knows that you got a CC
                msg.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient("mail.scottiez.com");  
                client.Credentials = new NetworkCredential("Admin@scottiez.com", "P@ssw0rd");
                client.Port = 8889;
                //the original is 25, but in case that port is being blocked, you can change the port number here
                //send the email
                using (client)
                {
                    try
                    {
                        client.Send(msg); //sending the MailMessage object
                    }
                    catch
                    {
                        ViewBag.ErrorMessage = "There was an error sending your message, please email admin@scottiez.com";
                        return View();
                    }
                }//using            
                //send the user to the ContactConfirmation View
                ViewBag.Status = "Email Sent Successfully";
                return View("_Layout", "Shared");
                //pass the contact object with it                 
            }//if
            else
            {
                return View(); //the model isn't valid, return to form 
            }//else


        }







    }
}
