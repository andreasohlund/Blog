using System;
using System.Web.Mvc;
using System.Linq;

namespace MyAuctionSite.Web.Controllers
{
	using Commands;
	using PersistentViewModel;
	using Raven.Client.Document;

	public class AuctionController : Controller
	{
		//
		// GET: /Auction/

		public ActionResult Index()
		{
			return View("MyAuctions");
		}

		//
		// GET: /Auction/Details/5

		public ActionResult Details(int id)
		{
			return View();
		}

		public ActionResult Auctions()
		{
			var documentStore = new DocumentStore { Url = "http://localhost:8080" };

			documentStore.Initialize();

			using (var session = documentStore.OpenSession())
			{

				var d = session.Query<AuctionSummaryItem>().ToList();
				return Json(d, JsonRequestBehavior.AllowGet);
			}
		}

		//
		// GET: /Auction/Register

		public ActionResult Register()
		{
			return View();
		}

		//
		// POST: /Auction/Create

		[HttpPost]
		public ActionResult Register(FormCollection collection)
		{
			MyAuctionApplication.Bus.Send(new RegisterAuctionCommand
			                              	{
			                              		AuctionId = Guid.NewGuid(),
			                              		Description = "Todo",
			                              		EndsAt = DateTime.Now.AddHours(1),
												UserId = MyAuctionApplication.UserId
										});

			return RedirectToAction("Index");

		}


	}
}
