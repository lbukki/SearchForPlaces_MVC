using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using GooglePlaces.Models;
using GooglePlaces.Models.API;

namespace GooglePlaces.Controllers
{
    
    public class HomeController : Controller
    {
        private static Dictionary<string, string> ListOfAvailablePlaces = new Dictionary<string, string>() {
            { "airport" , "repülőtér" },
            { "atm", "bankautomata" },
            { "bank", "bankfiók" },
            { "bar", "bár" },
            { "bus_station", "autóbuszállomás" },
            { "cafe", "kávézó" },
            { "car_repair", "autószerviz" },
            { "cemetery", "temető" },
            { "church", "templom" },
            { "dentist", "fogorvos" },
            { "doctor", "orvosi rendelő" },
            { "florist", "virágbolt" },
            { "gas_station", "benzinkút" },
            { "hair_care", "fodrász" },
            { "hospital", "kórház" },
            { "library", "könyvtár" },
            { "local_government_office", "polgármesteri hivatal" },
            { "movie_theater", "mozi" },
            { "museum", "múzeum" },
            { "park", "közpark" },
            { "parking", "parkoló" },
            { "pharmacy", "gyógyszertár" },
            { "police", "rendőrség" },
            { "post_office", "posta" },
            { "restaurant", "étterem" },
            { "school", "iskola" },
            { "shopping_mall", "bevásárlóközpont" },
            { "spa", "fürdő" },
            { "stadium", "stadion" },
            { "subway_station", "metróállomás" },
            { "supermarket", "szupermarket" },
            { "taxi_stand", "taxiállomás" },
            { "train_station", "vasútállomás" },
            { "veterinary_care", "állatorvosi rendelő" },
            { "zoo", "állatkert" },
        };
        private static Places ModelPlaces { get; set; }
        private static string myKeyForPlaces = "AIzaSyBcU_1cdrHpe5g8xZlPLqE67UufPFpwtSI";
        private static string myKey = "AIzaSyBPkJo4BrHtBx9DFA62MVuSQA32XA-Rte4";
        private static string latitude = "47,561462";
        private static string longitude = "19,084184";
        private static int radius = 500;
        private static string typeOfPlace = "restaurant";

        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();

            model.Latitude = latitude;
            model.Longitude = longitude;
            model.Radius = radius;
            model.TypeOfPlace = typeOfPlace;
            model.TypesOfPlaces = ListOfAvailablePlaces;           
            model.NumberOfplaces = 20;
            model.APIKey = myKey;
            GetPlaces(model.Latitude, model.Longitude, model.Radius, model.TypeOfPlace);
            model.ModelPlaces = ModelPlaces;

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateByCoordinates(IndexViewModel model)
        {
            latitude = model.Latitude;
            longitude = model.Longitude;
            radius = model.Radius;
            typeOfPlace = model.TypeOfPlace;
            return RedirectToAction("Index", "Home");
        }

        public void GetPlaces(string latitude, string longitude, int radius, string typeOfPlace)
        {

            WebClient client = new WebClient();

            client.BaseAddress = "https://maps.googleapis.com/maps/api/place/nearbysearch/json";
            client.Encoding = System.Text.Encoding.UTF8;
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("location", latitude.Replace(",", ".") + "," + longitude.Replace(",", "."));
            parameters.Add("radius", radius.ToString());
            parameters.Add("type", typeOfPlace);
            parameters.Add("key", myKeyForPlaces);
            client.QueryString = parameters;

            try
            {
                string data = client.DownloadString("https://maps.googleapis.com/maps/api/place/nearbysearch/json");
                JavaScriptSerializer JSserializer = new JavaScriptSerializer();
                ModelPlaces = JSserializer.Deserialize<Places>(data);
                foreach (var result in ModelPlaces.results)
                {
                    WebClient imageClient = new WebClient();
                    imageClient.BaseAddress = "https://maps.googleapis.com/maps/api/place/photo";
                    NameValueCollection imageParameters = new NameValueCollection();
                    imageParameters.Add("maxheight", "150");
                    if (result.photos != null && result.photos[0] != null && result.photos[0].photo_reference != null && result.photos[0].photo_reference != "")
                    {
                        imageParameters.Add("photoreference", result.photos[0].photo_reference);
                    }
                    else continue;
                    imageParameters.Add("key", myKeyForPlaces);
                    imageClient.QueryString = imageParameters;
                    result.image = imageClient.DownloadData("https://maps.googleapis.com/maps/api/place/photo");
                }
            }
            catch (WebException webex)
            {
                HttpWebResponse webResp = (HttpWebResponse)webex.Response;
                ViewBag.Message = webResp.StatusCode;
            }
            
        }
    }
}