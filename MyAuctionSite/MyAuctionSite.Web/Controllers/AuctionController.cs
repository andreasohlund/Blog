namespace MyAuctionSite.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Commands;

    public class AuctionController : Controller
	{
		public ActionResult Index()
		{
			return View("MyAuctions");
		}

        public ActionResult Register()
		{
			return View();
		}
        public ActionResult Confirmation()
        {
            return View();
        }

		[HttpPost]
		public ActionResult Register(FormCollection collection)
		{
			MyAuctionApplication.Bus.Send(new RegisterAuctionCommand
			                              	{
			                              		AuctionId = Guid.NewGuid(),
			                              		Description = collection["Description"],
			                              		EndsAt = DateTime.Now.AddHours(1),
												UserId = MyAuctionApplication.UserId
										});

			return RedirectToAction("Confirmation");
		}


	}
}
