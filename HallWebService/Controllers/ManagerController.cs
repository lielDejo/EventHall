using EventModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace HallWebService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        DBContext dbContext;
        UnitOfWorkRepository unitOfWorkRepository;

        public ManagerController()
        {
            this.dbContext = DBContext.GetInstance();
            this.unitOfWorkRepository = new UnitOfWorkRepository(this.dbContext);


        }
        [HttpGet]
        public List<Meet> GetListOfMeets(string hallID)
        {
            try
            {
                // פתיחת חיבור לבסיס הנתונים
                this.dbContext.OpenConection();

                // קריאה ל-Repository כדי לקבל את רשימת הפגישות
                return unitOfWorkRepository.MeetRepository.GetMeetByHallId(hallID);
            }
            catch (Exception ex)
            {
                // כתיבה ליומן שגיאות אם זמין, אחרת הדפסה לקונסולה
                Console.WriteLine($"Error in GetListOfMeets: {ex.Message}");
                return new List<Meet>(); // החזרת רשימה ריקה במקרה של שגיאה
            }
            finally
            {
                // סגירת חיבור לבסיס הנתונים בכל מקרה
                this.dbContext.CloseConection();
            }
        }


        [HttpPost]
        public bool CreateNewMeet(Meet meet)
        {
            try
            {
                this.dbContext.OpenConection();

                bool result = unitOfWorkRepository.MeetRepository.Create(meet);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating new meet: {ex.Message}");

                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                return false;
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }



        [HttpPost]
        public bool DelMeet(Meet meet)
        {
            try
            {
                this.dbContext.OpenConection();
                return unitOfWorkRepository.MeetRepository.Delete(meet.Id);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return false;
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }

        [HttpPost]
        public bool UpDateMeet(Meet meet)
        {
            try
            {
                this.dbContext.OpenConection();
                return unitOfWorkRepository.MeetRepository.UpDate(meet);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return false;
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }

        [HttpPost]
        public bool CreateNewHall()
        {
            string json = Request.Form["model"];
            EventHall model = JsonSerializer.Deserialize<EventHall>(json);
            IFormFile file = Request.Form.Files[0];
            try
            {
                this.dbContext.OpenConection();
                this.dbContext.beginTranzaction();
                bool cityAddOk = unitOfWorkRepository.CityRepository.Create(model.cityName);
                if (!cityAddOk) { return false; }
                int cityId = unitOfWorkRepository.CityRepository.GetCityIdByName(model.cityName);
                model.City = cityId;
                bool isr = unitOfWorkRepository.EventHallRepository.Create(model);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (isr)
                {
                    string hallId = unitOfWorkRepository.EventHallRepository.GetIDHallbyName(model.HallName);
                    string extension = Path.GetExtension(file.FileName);
                    Image_ image_ = new Image_();
                    image_.HallId = hallId;
                    image_.ImageAddress = model.HallName + model.HallImage[0].ImageAddress;
                    image_.ImageName = "1";
                    bool imageAddOk = unitOfWorkRepository.ImageRepository.Create(image_);
                    if (imageAddOk)
                    {
                        string filePath = Path.Combine(path, image_.ImageAddress);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }
                    this.dbContext.Commit();

                }
                return true;

            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return false;
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }

        [HttpPost]
        public bool UpDateHall(EventHall hall)
        {
            try
            {
                this.dbContext.OpenConection();
                return unitOfWorkRepository.EventHallRepository.UpDate(hall);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return false;
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }
        [HttpPost]
        public bool DeleteHall(EventHall hall)
        {
            try
            {
                this.dbContext.OpenConection();
                this.dbContext.beginTranzaction();
                foreach (Image_ i in hall.HallImage) {
                    bool isWork = unitOfWorkRepository.ImageRepository.Delete(i.Id);
                    }
                bool ok =  unitOfWorkRepository.EventHallRepository.Delete(hall.Id);
                this.dbContext.Commit();
                return ok;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return false;
                this.dbContext.RollBack();
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }
        [HttpGet]
        public List<EventHall> GetHallsByAdmin(string manageId)
        {
            try
            {
                this.dbContext.OpenConection();
                return unitOfWorkRepository.EventHallRepository.GetHallsByManager(manageId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetHallsByAdmin: {ex.Message}");
                return new List<EventHall>();
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }
        [HttpPost]
 
        [HttpGet]
        public int getIDbyCityName(string CityName)
        {
            try
            {
                this.dbContext.OpenConection();
                return unitOfWorkRepository.CityRepository.GetCityIdByName(CityName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetHallsByAdmin: {ex.Message}");
                return 0;
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }

        [HttpGet]
        public string getIDHallbyName(string hallName)
        {
            try
            {
                this.dbContext.OpenConection();
                return unitOfWorkRepository.EventHallRepository.GetIDHallbyName(hallName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetHallsByAdmin: {ex.Message}");
                return "";
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }

        [HttpGet]
                public string getNameUserbyId(string id)
        {
            try
            {
                this.dbContext.OpenConection();
                return unitOfWorkRepository.UserRepository.GetUserNameById(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetHallsByAdmin: {ex.Message}");
                return "";
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }

        [HttpPost]
        public bool addImages()
        {
            string json = Request.Form["model"];
            List<Image_> model = JsonSerializer.Deserialize<List<Image_>>(json);
            try
            {
                this.dbContext.OpenConection();
                bool isr = true;
                int ind = 0;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                foreach (Image_ image in model)
                {
                    if (!isr) { return false; }
                    else
                    {
                        IFormFile formFile = Request.Form.Files[ind];
                        string extension = Path.GetExtension(image.ImageAddress);
                        Image_ image_ = new Image_();
                        image_.ImageName =  DateTime.Now.ToString("yyyyMMddHHmmssfff")+ind.ToString();
                        image_.ImageAddress = image_.ImageName + extension;
                        image_.HallId = image.HallId; 

                        bool imageAddOk = unitOfWorkRepository.ImageRepository.Create(image_);
                        if (imageAddOk)
                        {
                            string filePath = Path.Combine(path, image_.ImageAddress);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                formFile.CopyTo(stream);
                            }
                        }
                        ind++;
                    }

                }
                return isr;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetHallsByAdmin: {ex.Message}");
                return false;
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }

    }
}


