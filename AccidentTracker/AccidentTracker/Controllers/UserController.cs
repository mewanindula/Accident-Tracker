using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccidentTracker.Models;
using System.Net.Mail;
using System.Net;
using System.Web.Security;

namespace AccidentTracker.Controllers
{
	public class UserController : Controller
	{
		//Registration Action
		[HttpGet]
		public ActionResult Registration()
		{

			return View();
		
		}

		//Registration POST Action
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Registration([Bind(Exclude="IsEmailVerified, ActivationCode")] User user)
		{

			bool Status = false;
			string message = "";

			//Model Validation
			if(ModelState.IsValid)
			{
				//#region //Email Is Already Exist
				//var isEmailExist = IsEmailExist(user.uEmail);

				//if(isEmailExist)
				//{
				//    ModelState.AddModelError("EmailExist", "Email Already Exist");

				//    return View(user);
				//}
				//#endregion


				#region //UserID Is Already Exist
				var isUserIDExist = IsUserIDExist(user.UserID);

				var isUserIDExistInGSM = IsUserIDExistInGSM(user.UserID);

				if(isUserIDExist)
				{
					ModelState.AddModelError("UserIDExist", "User ID Already Exist");

					return View(user);
				}

				if (!isUserIDExistInGSM)
				{
					ModelState.AddModelError("UserIDInvalid", "Invalid User ID"); 

					return View(user);
				}
				#endregion

				#region //Generate Activation Code
				user.ActivationCode = Guid.NewGuid();
				#endregion

				#region //Password Hashing
				user.Password = Crypto.Hash(user.Password);
				user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
				#endregion

				user.IsEmailVerified = false;

				#region //Save Data to DataBase
				using(AccidentTrackerEntities dc = new AccidentTrackerEntities())
				{
					dc.Users.Add(user);
					dc.SaveChanges();

					//Send Email To User
					SendVerificationLinkEmail(user.uEmail, user.ActivationCode.ToString());

					message = " Registration successfully done. Acount activation link" +
						" has been send to your email id : " + user.uEmail;

					Status = true;

				}
				#endregion
			}
			else
			{
				message = "Invalid Request";
			}

			ViewBag.Message = message;
			ViewBag.Status = Status;

			return View(user);
		
		}

		//Verify Account
		[HttpGet]
		public ActionResult VerifyAccount(string id)
		{
			bool Status = false;

			using(AccidentTrackerEntities dc = new AccidentTrackerEntities())
			{
				dc.Configuration.ValidateOnSaveEnabled = false;

				var v = dc.Users.Where(a=>a.ActivationCode == new Guid(id)).FirstOrDefault();

				if(v != null)
				{
					v.IsEmailVerified = true;
					dc.SaveChanges();
					Status = true;
				}
				else
				{
					ViewBag.Message = " Invalid Request";
				}
			}

			ViewBag.Status = Status;
			return View();
		}


		//Login
		[HttpGet]
		public ActionResult Login()
		{
			return View();
		}

		//Login POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(UserLogin login, string ReturnUrl="")
		{
			if (ModelState.IsValid)
			{
				string message = "";

				using (AccidentTrackerEntities dc = new AccidentTrackerEntities())
				{
					var v = dc.Users.Where(a => a.UserID == login.UserID).FirstOrDefault();

					if (v != null)
					{
						if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
						{
							int timeout = login.RememberMe ? 525600 : 20; //525600min = 1yr

							var ticket = new FormsAuthenticationTicket(v.FirstName, login.RememberMe, timeout);

							string encrypted = FormsAuthentication.Encrypt(ticket);

							var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);

							cookie.Expires = DateTime.Now.AddMinutes(timeout);

							cookie.HttpOnly = true;

							Response.Cookies.Add(cookie);


							if (Url.IsLocalUrl(ReturnUrl))
							{
								return Redirect(ReturnUrl);
							}
							else
							{
								Session["UserID"] = login.UserID.ToString();
								return RedirectToAction("Show", "Home");
							}
						}
						else
						{
							ModelState.AddModelError("UserInvalPass", "Invalid Password");
							message = "Invalid Password";
						}
					}
					else
					{
						ModelState.AddModelError("UserInval", "Invalid User");
						message = "Invalid User";
					}
				}

				ViewBag.Message = message;

				return View();
			}
			return View();
		}


		//Logout
		[Authorize]
		[HttpPost]
		public ActionResult Logout()
		{
			Session.Clear();
			FormsAuthentication.SignOut();

			return RedirectToAction("Login", "User");
		}



		[NonAction] //Email Exist Or Not
		public bool IsEmailExist(string emailID)
		{
			using(AccidentTrackerEntities dc = new AccidentTrackerEntities())
			{
				var v = dc.Users.Where(a => a.uEmail == emailID).FirstOrDefault();

				return v != null;
			}
		}


		[NonAction] //UserID Exist Or Not
		public bool IsUserIDExist(string userID)
		{
			using (AccidentTrackerEntities dc = new AccidentTrackerEntities())
			{
				var v = dc.Users.Where(a => a.UserID == userID).FirstOrDefault();

				return v != null;
			}
		}


		[NonAction] //UserID Exist Or Not In GSM
		public bool IsUserIDExistInGSM(string userID)
		{
			using (AccidentTrackerEntities dc = new AccidentTrackerEntities())
			{
				var v = dc.GSMs.Where(a => a.UserID == userID).FirstOrDefault();

				return v != null;
			}
		}


		[NonAction] //Send Verification Link To Email
		public void SendVerificationLinkEmail(string emailID, string activationCode)
		{
			var verifyUrl = "/User/VerifyAccount/" + activationCode;
			var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

			var fromEmail = new MailAddress("accidenttrackeratd9@gmail.com", "AccidentTracker");
			var fromEmailPassword = "accidenttracker";
			var toEmail = new MailAddress(emailID);

			string subject = "AccidentTracker Account Is Successfully Created!";
			string body = "<br><br>We Are Excited To Tell You That Your AccidentTracker Account Is" +
				" Successfully Created. Please Click On The Below Link To Verify Your Account" +
				" <br><br><a href = '" + link + "'>" + link + "</a>";

			var smtp = new SmtpClient
			{
				Host = "smtp.gmail.com",
				Port = 587,
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)

			};


			using (var message = new MailMessage(fromEmail, toEmail)
			{
				Subject = subject,
				Body = body,
				IsBodyHtml = true
			})
			smtp.Send(message);
		}
	}
}