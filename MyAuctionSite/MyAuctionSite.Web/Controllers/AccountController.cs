using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using MyAuctionSite.Web.Models;

namespace MyAuctionSite.Web.Controllers
{
	using Commands;
	using Events;
	using Operations.Messages;

	[HandleError]
	public class AccountController : Controller
	{

		public IFormsAuthenticationService FormsService { get; set; }
		public IMembershipService MembershipService { get; set; }

		protected override void Initialize(RequestContext requestContext)
		{
			if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
			if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

			base.Initialize(requestContext);
		}

		// **************************************
		// URL: /Account/LogOn
		// **************************************

		public ActionResult LogOn()
		{
			return View();
		}

		[HttpPost]
		public ActionResult LogOn(LogOnModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				if (MembershipService.ValidateUser(model.UserName, model.Password))
				{
					FormsService.SignIn(model.UserName, model.RememberMe);
					if (!String.IsNullOrEmpty(returnUrl))
					{
						return Redirect(returnUrl);
					}
					else
					{
						return RedirectToAction("Index", "Home");
					}
				}
				else
				{
					ModelState.AddModelError("", "The user name or password provided is incorrect.");
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		// **************************************
		// URL: /Account/LogOff
		// **************************************

		public ActionResult LogOff()
		{
			FormsService.SignOut();

			return RedirectToAction("Index", "Home");
		}

		// **************************************
		// URL: /Account/Register
		// **************************************

		public ActionResult Register()
		{
			ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
			return View();
		}

		[HttpPost]
		public ActionResult Register(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				MyAuctionApplication.Bus.Send<UserSignupRequested>(c => c.EmailAddress = model.Email);
				return RedirectToAction("SignupReceived");
			}

			return View(model);
		}

		public ActionResult SignupReceived()
		{
			return View();
		}

		public ActionResult Verify(Guid id)
		{
			var model = new VerifyViewModel();

			model.RegistrationId = id;

			return View(model);
		}

		[HttpPost]
		public ActionResult Verify(VerifyViewModel model)
		{
			if (ModelState.IsValid)
			{
				MyAuctionApplication.Bus.Send<UserVerified>(c =>
				{
					c.VerificationId = model.RegistrationId;
					c.SelectedPassword = model.Password;
				});
				return RedirectToAction("LogOn");
			}

			return View(model);
		}

		public ActionResult FinalizeAccount()
		{
			return View();
		}
	}

	public class VerifyViewModel
	{
		public Guid RegistrationId { get; set; }

		public string Password { get; set; }

		public string ConfirmPassword { get; set; }
	}
}
