using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using POE_MVC.Models;
using POE_MVC.Helpers;

namespace POE_MVC.Controllers
{
    public class DB_ForecastController : Controller
    {
        private WeatherForecastAppEntities db = new WeatherForecastAppEntities();

        // GET: DB_Forecast
        public ActionResult Index()
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "DB_User");

            var CityCodeDict = Helpers.CityUtilities.getCityCodeDict();

            //Data to populate top istbox
            var forecasts = db.DB_Forecast.ToList();
            var cityIds = forecasts.Select(f => f.CityID).Distinct().ToList();
            ViewBag.StartDate = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            ViewBag.EndDate = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            ViewBag.Cities = cityIds.Select(id => CityCodeDict[id]).ToList();
            ViewBag.Forecasts = TempData["Forecasts"];
            //Data to populate bottom listox
            bool gettingCities = TempData["GettingCities"] != null;
            if (gettingCities || ViewBag.Forecasts != null)
            {
                ViewBag.GettingCities = true;
                ViewBag.SelectedCityIDs = TempData["SelectedCityIDs"];
                ViewBag.SelectedCities = TempData["SelectedCities"];
                ViewBag.StartDate = TempData["StartDate"];
                ViewBag.EndDate = TempData["EndDate"];
                ViewBag.Forecasts = TempData["Forecasts"];
                ViewBag.CityName = TempData["CityName"];
                ViewBag.ForecastCityID = TempData["ForecastCityID"];
            }

            return View();
        }

        public ActionResult GetCities(string cityIds, DateTime startDate, DateTime endDate)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "DB_User");

            List<int> intCityIds = cityIds.Split(',').Select(id=> Convert.ToInt32(id)).ToList();
            Dictionary<int, City> cityCodeDict = CityUtilities.getCityCodeDict();
            List<City> selectedCities = intCityIds.Select(id => cityCodeDict[Convert.ToInt32(id)]).ToList();

            TempData["GettingCities"] = "1";
            TempData["SelectedCityIDs"] = intCityIds;
            TempData["SelectedCities"] = selectedCities;
            TempData["StartDate"] = startDate.ToString("yyyy'-'MM'-'dd"); ;
            TempData["EndDate"] = endDate.ToString("yyyy'-'MM'-'dd"); 
            return RedirectToAction("Index");
        }

        public ActionResult GetForecasts(string cityIds, int cityID, DateTime startDate, DateTime endDate)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "DB_User");

            List<int> intCityIds = cityIds.Split(',').Select(id => Convert.ToInt32(id)).ToList();
            Dictionary<int, City> cityCodeDict = CityUtilities.getCityCodeDict();
            List<City> selectedCities = intCityIds.Select(id => cityCodeDict[Convert.ToInt32(id)]).ToList();

            List<DB_Forecast> matchingForecasts = db.DB_Forecast.Where(f => f.ForecastDate.CompareTo(startDate) >= 0 && f.ForecastDate.CompareTo(endDate) <= 0 && f.CityID==cityID).OrderBy(f => f.ForecastDate).ToList();
            List<DateTime> datesWithNoForecasts = new List<DateTime>();

            DateTime dt = startDate;
            while (dt.Date.CompareTo(endDate.Date) <= 0)
            {
                if(!matchingForecasts.Any(f=> f.ForecastDate.Equals(dt)))
                {
                    datesWithNoForecasts.Add(dt);
                }
                 dt = dt.AddDays(1);
            }

            //Takes matched forecasts and makes blank forecasts with dates unused, to make a combined list of forecasts ordered by date
            List<DB_Forecast> allForecasts = matchingForecasts.Concat(datesWithNoForecasts.Select(f => new DB_Forecast { ForecastID = -1, ForecastDate = f.Date })).OrderBy(f => f.ForecastDate).ToList();

            TempData["Forecasts"] = allForecasts;
            TempData["StartDate"] = startDate.ToString("yyyy'-'MM'-'dd"); ;
            TempData["EndDate"] = endDate.ToString("yyyy'-'MM'-'dd"); ;
            TempData["CityName"] = CityUtilities.getCityCodeDict()[cityID];
            TempData["ForecastCityID"] = cityID;
            TempData["SelectedCityIDs"] = intCityIds;
            TempData["SelectedCities"] = selectedCities;

            return RedirectToAction("Index");
            
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
