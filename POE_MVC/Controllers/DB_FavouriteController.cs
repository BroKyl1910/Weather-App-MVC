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
using System.Web.Script.Serialization;

namespace POE_MVC.Controllers
{
    public class DB_FavouriteController : Controller
    {
        private WeatherForecastAppEntities db = new WeatherForecastAppEntities();

        // GET: DB_Favourite
        public ActionResult Index()
        {

            if (Session["Username"] == null) return RedirectToAction("Login", "DB_User");

            Dictionary<int, City> cityCodeDict = CityUtilities.getCityCodeDict();
            Dictionary<string, City> cityNameDict = CityUtilities.getNameCityDict();


            string username = Session["Username"] as string;
            //List of ids of users favourite cities
            List<int> favouriteCityIds = db.DB_Favourite.Where(fav => fav.Username.Equals(username)).ToList().Select(fav => fav.CityID).ToList();

            //If no city is selected, i.e on first load, just get the first city's forecast. This is done as early as possible to avoid wasting processing time
            //Already a lot of time is wasted, therefore this can be optimised
            if (TempData["SelectedID"] == null)
            {
                if (favouriteCityIds.Count > 0)
                {
                    GetForecast(favouriteCityIds[0], true);
                }
                else
                {
                    ViewBag.HasFavourites = false;
                    ViewBag.Background = -1;
                    return View(favouriteCityIds);
                }
            }

            //List of city names of favourite cities
            List<string> favouriteCityNames = favouriteCityIds.Select(id => cityCodeDict[id]).ToList().Select(city => city.ToString()).ToList();

            //https://stackoverflow.com/questions/2434593/create-a-dictionary-using-2-lists-using-linq
            //Custom dictionary of city ids to city names for users favourite cities
            var dict = favouriteCityIds.Zip(favouriteCityNames, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);

            if (TempData["SelectedID"] == null)
            {
                ViewBag.SelectedID = favouriteCityIds[0];
            }
            else
            {
                ViewBag.SelectedID = TempData["SelectedID"];
            }
            if (TempData["UseAPI"] == null)
            {
                ViewBag.UseAPI = true;
            }
            else
            {
                ViewBag.UseAPI = TempData["UseAPI"];
            }

            ViewBag.HasFavourites = true;
            ViewBag.CityIdNameDict = dict;

            ViewBag.HasForecast = TempData["HasForecast"];
            ViewBag.City = TempData["City"];
            ViewBag.Desc = TempData["Desc"];
            ViewBag.Min = TempData["Min"];
            ViewBag.Max = TempData["Max"];

            ViewBag.Date = TempData["Date"];
            ViewBag.Wind = TempData["Wind"];
            ViewBag.Humidity = TempData["Humidity"];
            ViewBag.Precip = TempData["Precip"];

            ViewBag.Background = TempData["Background"];
            if (ViewBag.Background == null)
            {
                ViewBag.Background = -1;
            }

            return View(favouriteCityIds);
        }

        public ActionResult GetForecast(int id, bool useAPI)
        {
            TempData["SelectedID"] = id;
            TempData["UseAPI"] = useAPI;
            if (useAPI)
            {
                GetAPIWeather(id);
            }
            else
            {
                GetDBWeather(id);
            }
            return RedirectToAction("Index");
        }

        private void GetDBWeather(int id)
        {
            TempData["City"] = Helpers.CityUtilities.getCityCodeDict()[id].ToString();
            DB_Forecast forecast = db.DB_Forecast.Where(f => f.CityID == id).OrderByDescending(f => f.ForecastDate).FirstOrDefault();
            if (forecast == null)
            {
                TempData["HasForecast"] = false;
                return;
            }
            TempData["HasForecast"] = true;

            TempData["Desc"] = null;
            TempData["Min"] = forecast.MinTemp + " °C";
            TempData["Max"] = forecast.MaxTemp + " °C";

            TempData["Date"] = forecast.ForecastDate.ToLongDateString();
            TempData["Wind"] = forecast.WindSpeed + " km /h";
            TempData["Humidity"] = forecast.Humidity + " % ";
            TempData["Precip"] = forecast.Precipitation + " %";

            //TempData["Background"] = getBackground(currentWeather);
        }

        void GetAPIWeather(int id)
        {
            APICurrentWeather currentWeather = Helpers.APICurrentWeather.GetCurrentWeather(id + "");
            TempData["HasForecast"] = true;
            TempData["City"] = currentWeather.name + ", " + currentWeather.sys.country;
            TempData["Desc"] = currentWeather.weather[0].main + " - " + currentWeather.weather[0].description;
            TempData["Min"] = Math.Round(currentWeather.main.temp_min) + " °C";
            TempData["Max"] = Math.Round(currentWeather.main.temp_max) + " °C";

            TempData["Date"] = DateTime.Now.Date.ToLongDateString();
            TempData["Wind"] = currentWeather.wind.speed + " km /h";
            TempData["Humidity"] = currentWeather.main.humidity + " % ";
            TempData["Precip"] = currentWeather.GetRain() + " mm in last 3 hours";

            TempData["Background"] = getBackground(currentWeather);

        }

        string getBackground(APICurrentWeather currentWeather)
        {
            string desc = currentWeather.weather[0].description;
            if (desc.Contains("cloud") || desc.Contains("rain") || desc.Contains("shower") || desc.Contains("storm") || desc.Contains("drizzle"))
            {
                return "cloudy.jpg";
            }
            int randomIndex = new System.Random().Next(2);
            return new string[] { "sunny.png", "skyWithClouds.jpg" }[randomIndex];
        }

        public ActionResult RemoveFavourite(int id)
        {
            string username = Session["Username"] as string;
            DB_Favourite favToDelete = db.DB_Favourite.Where(f => f.Username.Equals(username) && f.CityID == id).First();
            db.DB_Favourite.Remove(favToDelete);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public string AutoComplete(string q)
        {
            var matchingCities = CityUtilities.SearchCities(q).ToArray();
            string[] result = matchingCities.Select(c => c.id + "_" + c.ToString()).Take(20).ToArray();
            return new JavaScriptSerializer().Serialize(result);
        }

        public ActionResult AddFavourite(int id)
        {
            string username = Session["Username"] as string;
            if (!db.DB_Favourite.Any(f => f.CityID == id && f.Username.Equals(username)))
            {
                db.DB_Favourite.Add(new DB_Favourite { CityID = id, Username = username });
                db.SaveChanges();
            }
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
