using RateSetterTestApp.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RateSetterTestApp.Models;

namespace RateSetterTestApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string url)
        {
            var shortUrl = ShortenUrl(url);
            ViewBag.ShortUrl = shortUrl;

            return View();
        }

        public ActionResult RedirectToUrl(string urlId)
        {
            string originalUrl = GetOriginalUrl(urlId);

            return Redirect(originalUrl);
        }

        public ActionResult ShowAllLinks()
        {
            ViewBag.Message = "Show All Links";

            var links = GetAllLinks();

            return View("Links", links);
        }

        private List<Link> GetAllLinks()
        {
            using (var dbContext = new RateSetterDbEntities())
            {
                return dbContext.Links.ToList();
            }
        }

        private string GetOriginalUrl(string urlId)
        {
            var originalUrl = "";
            using (var dbContext = new RateSetterDbEntities())
            {
                var link = dbContext.Links.SingleOrDefault(x => x.LinkId == urlId);
                if (link != null)
                {
                    originalUrl = link.OriginalUrl;
                }

                Log logEntry = new Log()
                {
                    RequestType = (short)RequestType.Retrieve,
                    CreateDate = DateTime.Now,
                    OriginalUrl = originalUrl,
                    ShortUrl = urlId
                };

                dbContext.Logs.Add(logEntry);
                dbContext.SaveChanges();
            }

            return originalUrl;
        }

        private string ShortenUrl(string url)
        {
            var shortUrl = Utils.Utils.GetNewShortUrl(out string guid);

            using (var dbContext = new RateSetterDbEntities())
            {
                Link link = new Link()
                {
                    LinkId = guid,
                    OriginalUrl = url,
                    ShortUrl = shortUrl,
                    CreateDate = DateTime.Now
                };

                Log logEntry = new Log()
                {
                    RequestType = (short)RequestType.Create,
                    CreateDate = DateTime.Now,
                    OriginalUrl = url,
                    ShortUrl = shortUrl
                };

                dbContext.Links.Add(link);
                dbContext.Logs.Add(logEntry);
                dbContext.SaveChanges();
            }

            return shortUrl;
        }
    }
}