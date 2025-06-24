using EventModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Drawing;
using System.Net;
using WebApiClient;

namespace EventHallWebApplication.Controllers
{
    public class CatalogController : Controller
    {
        public IActionResult Index()
        {
            WebClient<EventHallViewModel> webClient = new WebClient<EventHallViewModel>();
            webClient.Schema = "http";
            webClient.Port = 5232;
            webClient.Host = "localHost";
            webClient.Path = "api/Guest/GetEventHallViewModel";
            EventHallViewModel eventHallViewModel = webClient.Get().Result;

            return View(eventHallViewModel);
        }
        public IActionResult GetHallCatalog(string city, string type, string grade, string minCap, string maxCap)
        {
            if (HttpContext.Session.Get("userId") != null)
            {
                ViewBag.UserId = HttpContext.Session.Get("userId");
            }
            WebClient<EventHallViewModel> webClient = new WebClient<EventHallViewModel>();
            webClient.Schema = "http";
            webClient.Port = 5232;
            webClient.Host = "localHost"; 
            webClient.Path = "api/Guest/GetEventHallViewModel";
            if (city != null)
                webClient.AddParam("city", city);
            if (type != null)
                webClient.AddParam("type", type);
            if (grade != null)
                webClient.AddParam("grade", grade);
            if (minCap != null && maxCap != null)
            {
                webClient.AddParam("minCap", minCap);
                webClient.AddParam("maxCap", maxCap);
            }
            EventHallViewModel eventHallViewModel = webClient.Get().Result;

			return View(eventHallViewModel);
        }
        public IActionResult GetHallList(string city, string type, string grade, string minCap, string maxCap)
        {
            if (HttpContext.Session.Get("userId") != null)
            {
                ViewBag.UserId = HttpContext.Session.Get("userId");
            }
            WebClient<EventHallViewModel> webClient = new WebClient<EventHallViewModel>();
            webClient.Schema = "http";
            webClient.Port = 5232;
            webClient.Host = "localHost"; 
            webClient.Path = "api/Guest/GetEventHallViewModel";
            if (city != null)
                webClient.AddParam("city", city);
            if (type != null)
                webClient.AddParam("type", type);
            if (grade != null)
                webClient.AddParam("grade", grade);
            if (minCap != null && maxCap != null)
            {
                webClient.AddParam("minCap", minCap);
                webClient.AddParam("maxCap", maxCap);
            }
            EventHallViewModel eventHallViewModel = webClient.Get().Result;

			return PartialView(eventHallViewModel);
        }

        public IActionResult HallItem(string id)
        {
            if (HttpContext.Session.Get("userId") != null)
            {
                ViewBag.UserId = HttpContext.Session.Get("userId");
            }
            WebClient<EventHall> webClient = new WebClient<EventHall>();
            webClient.Schema = "http";
            webClient.Port = 5232;
            webClient.Host = "localHost";
            webClient.Path = "api/Guest/GetEventHall";
            webClient.AddParam("id", id);

            EventHall eventHall = webClient.Get().Result;

            return View(eventHall);
        }
        [HttpPost]
        public IActionResult yourMeets(Meet meet)
        {
            ViewBag.Error = null;
            meet.UserId = HttpContext.Session.GetString("userId");

            // יצירת פגישה אם אין כפילות
            WebClient<Meet> webClient = new WebClient<Meet>();
            webClient.Schema = "http";
            webClient.Port = 5232;
            webClient.Host = "localHost";
            webClient.Path = "api/ragister/CreateMeet";

            bool isWork = webClient.Post(meet).Result;
            if (!isWork)
            {
                ViewBag.Error = "Failed to create the meeting. Please try again.";
                @ViewBag.HallId = meet.HallId;
                return View("viewOrderMeet", meet.HallId);
            }

            // קבלת רשימת הפגישות המעודכנת
            WebClient<List<Meet>> webClient1 = new WebClient<List<Meet>>();
            webClient1.Schema = "http";
            webClient1.Port = 5232;
            webClient1.Host = "localHost";
            webClient1.Path = "api/ragister/GetListOfMeets";
            webClient1.AddParam("UserID", meet.UserId);

            List<Meet> listM = webClient1.Get().Result;

            // קבלת שם האולם
            List<string> hallNames = GetHallNameById(listM);  // כאן קוראים לפונקציה מהקונטרולר
            ViewBag.HallName = hallNames;  // שליחת שם האולם ל-View

            return View("yourMeets", listM);
        }

        public IActionResult viewOrderMeet(string HallId)
        {
            ViewBag.HallId = HallId;
            return View();
        }

        public IActionResult viewLoginForm(string id)
        {
            return View();
        }



        [HttpPost]
        public IActionResult Login(string userName, string password)
        {
            WebClient<string> webClient = new WebClient<string>();
            webClient.Schema = "http";
            webClient.Port = 5232;
            webClient.Host = "localHost";
            webClient.Path = "api/Guest/LoginUser";
            webClient.AddParam("userName", userName);
            webClient.AddParam("password", password);

            string userId = webClient.Get().Result;
            if (userId == "" || userId == null) {
                ViewBag.Error = true;
                return View("viewLoginForm");
            }
            HttpContext.Session.SetString("userId", userId);//אזור בזיכרון שמחזיק מידע על הבראוזר שפתח את הפניה לשרת (פותח את הסאשין עבור על בראוזר)מוסיפים שבתוך הרשימה הזו ישמר התז של המשתמש

            return RedirectToAction("GetHallCatalog", "Catalog");
        }

        public IActionResult viewRegistration(string id)
        {
            return View();
        }
		[HttpPost]
		public IActionResult Registration(User user)
		{
			WebClient<string> webClient = new WebClient<string>();
			webClient.Schema = "http";
			webClient.Port = 5232;
			webClient.Host = "localHost";
			webClient.Path = "api/Guest/RegisterUser";
			webClient.AddParam("userName", user.UserName);
			webClient.AddParam("password", user.Password);
			webClient.AddParam("phoneNumber", user.PhoneNumber);
			webClient.AddParam("email", user.Email);

			string response = webClient.Get().Result;

			if (string.IsNullOrEmpty(response) || response.ToLower() == "error")
			{
				ViewBag.Error = "Registration failed. Please try again.";
				return View("viewRegistration");
			}
            HttpContext.Session.SetString("userId", response);//אזור בזיכרון שמחזיק מידע על הבראוזר שפתח את הפניה לשרת (פותח את הסאשין עבור על בראוזר)מוסיפים שבתוך הרשימה הזו ישמר התז של המשתמש

            return RedirectToAction("GetHallCatalog", "Catalog");
		}
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("userId"); // מוחק את מזהה המשתמש מה-Session
            return RedirectToAction("GetHallCatalog", "Catalog"); // חזרה לעמוד הבית
        }
        public IActionResult lstMeets()
        {
            if (HttpContext.Session.Get("userId") != null)
            {
                ViewBag.UserId = HttpContext.Session.Get("userId");
            }
            string userId = HttpContext.Session.GetString("userId");
            // קבלת רשימת הפגישות המעודכנת
            WebClient<List<Meet>> webClient1 = new WebClient<List<Meet>>();
            webClient1.Schema = "http";
            webClient1.Port = 5232;
            webClient1.Host = "localHost";
            webClient1.Path = "api/ragister/GetListOfMeets";
            webClient1.AddParam("UserID", userId);

            List<Meet> listM = webClient1.Get().Result;

            // קבלת שם האולם
            List<string> hallNames = GetHallNameById(listM);  // כאן קוראים לפונקציה מהקונטרולר
            ViewBag.HallName = hallNames;  // שליחת שם האולם ל-View

            return View("yourMeets", listM);
        }
      
        public IActionResult DeleteMeet(int id)
        {
            WebClient<bool> webClient = new WebClient<bool>();
            webClient.Schema = "http";
            webClient.Port = 5232;
            webClient.Host = "localHost";
            webClient.Path = "api/ragister/DeleteMeet";
            webClient.AddParam("id", id.ToString());

            bool isDeleted = webClient.Get().Result;

            if (!isDeleted)
            {
                ViewBag.Error = "Failed to delete the meeting.";
            }

            string UserId = HttpContext.Session.GetString("userId");
            WebClient<List<Meet>> webClient1 = new WebClient<List<Meet>>();
            webClient1.Schema = "http";
            webClient1.Port = 5232;
            webClient1.Host = "localHost";
            webClient1.Path = "api/ragister/GetListOfMeets";
            webClient1.AddParam("UserID", UserId);

            List<Meet> listM = webClient1.Get().Result;

            // קבלת שם האולם
            List<string> hallNames = GetHallNameById(listM);  // כאן קוראים לפונקציה מהקונטרולר
            ViewBag.HallName = hallNames;  // שליחת שם האולם ל-View

            return View("yourMeets", listM);
        }
        public List<string> GetHallNameById(List<Meet> meets)
        {
            List<string> hallNames = new List<string>();

            foreach (var meet in meets)
            {
                // נקרא למערכת או דאטהבייס כדי לקבל את שם האולם לפי ה-HallId
                WebClient<string> webClient = new WebClient<string>();
                webClient.Schema = "http";
                webClient.Port = 5232;
                webClient.Host = "localHost";
                webClient.Path = "api/ragister/getNameHallById";
                webClient.AddParam("id", meet.HallId);

                // הוספת שם האולם לרשימה
                string hallName = webClient.Get().Result;
                hallNames.Add(hallName);
            }

            return hallNames;
        }
        [HttpPost]
        public IActionResult SubmitRating(int hallId, int rating)
        {
            // קבלת האולם לפי ID
            WebClient<EventHall> webClient = new WebClient<EventHall>();
            webClient.Schema = "http";
            webClient.Port = 5232;
            webClient.Host = "localHost";
            webClient.Path = "api/Guest/GetEventHall";
            webClient.AddParam("id", hallId.ToString());

            EventHall eventHall = webClient.Get().Result;

            if (eventHall == null)
            {
                return BadRequest("Invalid hall ID");
            }

            // חישוב ממוצע הדירוגים: אם יש דירוגים קודמים
            int totalRating = eventHall.Rating + rating;
            int newAverageRating = totalRating / 2;

            // עדכון הדירוג החדש
            eventHall.Rating = newAverageRating;

            WebClient<EventHall> webClientUpdate = new WebClient<EventHall>();
            webClientUpdate.Schema = "http";
            webClientUpdate.Port = 5232;
            webClientUpdate.Host = "localHost";
            webClientUpdate.Path = "api/Guest/UpdateEventHall";

            bool result = webClientUpdate.Post(eventHall).Result;

            if (result)
            {
                return RedirectToAction("HallItem", new { id = hallId });  // חזרה לדף האולם
            }

            return StatusCode(500, "Failed to update rating");
        }


    }
}
