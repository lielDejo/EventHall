using EventModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HallWebService
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ragisterController : ControllerBase
    {
        DBContext dbContext;
        UnitOfWorkRepository unitOfWorkRepository;

        public ragisterController()
        {
            this.dbContext = DBContext.GetInstance();
            this.unitOfWorkRepository = new UnitOfWorkRepository(this.dbContext);


        }
        [HttpPost]
        public EventHallViewModel GetEventHallViewModel(string city = "", string grade = "", string type = "", string minCap = "", string maxCap = "")
        {
            try
            {
                EventHallViewModel eventHallViewModel = new EventHallViewModel();
                this.dbContext.OpenConection();
                eventHallViewModel.Halls = unitOfWorkRepository.EventHallRepository.GetAll();
                if (city != "")
                {
                    eventHallViewModel.Halls = unitOfWorkRepository.EventHallRepository.SortByCity(city);
                }
                else if (grade != "")
                {
                    eventHallViewModel.Halls = unitOfWorkRepository.EventHallRepository.SortByRatings(grade);
                }
                else if (type != "")
                {
                    eventHallViewModel.Halls = unitOfWorkRepository.EventHallRepository.SortBytype(type);
                }
                else if (minCap != "" && maxCap != "")
                {
                    eventHallViewModel.Halls = unitOfWorkRepository.EventHallRepository.SortByContent(minCap, maxCap);
                }

                eventHallViewModel.Citys = unitOfWorkRepository.CityRepository.GetAll();
                return eventHallViewModel;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return null;
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }

        [HttpPost]
        public bool UpDateUser(User user)
        {
            try
            {
                this.dbContext.OpenConection();
                return unitOfWorkRepository.UserRepository.UpDate(user);
            }
            catch
            {
                return false;
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }

        [HttpPost]
        public bool PostRating(Rating rating)
        {
            try
            {
                this.dbContext.OpenConection();
                return unitOfWorkRepository.RatingRepository.Create(rating);
            }
            catch
            {
                return false;
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }

        [HttpPost]
        public bool UpDateRating(Rating rating)
        {
            try
            {
                this.dbContext.OpenConection();
                return unitOfWorkRepository.RatingRepository.UpDate(rating);
            }
            catch
            {
                return false;
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }
        [HttpGet]
        public bool DelRating(string id)
        {
            try
            {
                this.dbContext.OpenConection();
                return unitOfWorkRepository.RatingRepository.Delete(id);
            }
            catch
            {
                return false;
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }

        [HttpGet]
        public bool DeleteMeet(string id)
        {
           
            try
            {
                this.dbContext.OpenConection();
                return unitOfWorkRepository.MeetRepository.Delete(id);
            }
            catch
            {
                return false;
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }

        [HttpGet]
        public List<Meet> GetListOfMeets(string UserID)
        {
            try
            {
                // פתיחת חיבור לבסיס הנתונים
                this.dbContext.OpenConection();

                // קריאה ל-Repository כדי לקבל את רשימת הפגישות
                return unitOfWorkRepository.MeetRepository.GetMeetsByUserId(UserID);
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

        [HttpGet]
        public List<Rating> GetRatingsOfHall(string id)
        {
            try
            {
                this.dbContext.OpenConection();
                return unitOfWorkRepository.RatingRepository.GetRatingByHallId(id);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return new List<Rating>();
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }

        [HttpGet]
        public List<Image_> GetImagesOfHall(string id)
        {
            try
            {
                this.dbContext.OpenConection();
                return unitOfWorkRepository.ImageRepository.GetLsPicByHallId(id);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return new List<Image_>();
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }
        [HttpGet]
        public Image_ GetMainImagesOfHall(string id)
        {
            try
            {
                this.dbContext.OpenConection();
                return unitOfWorkRepository.ImageRepository.GetMainPicByHallId(id);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return new Image_();
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }

        [HttpPost]
        public bool CreateMeet(Meet meet)
        {

            try
            {
                this.dbContext.OpenConection();
                // בדיקה אם יש כבר פגישה לאולם הזה או למשתמש הזה באותו יום ושעה
                bool exists = unitOfWorkRepository.MeetRepository
                    .GetAll()
                    .Any(m => (m.HallId == meet.HallId || m.UserId == meet.UserId) &&
                              m.DateMeet == meet.DateMeet && m.Hour == meet.Hour);

                if (exists) return false;
                return unitOfWorkRepository.MeetRepository.Create(meet);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error in CheckExistingMeet: {ex.Message}");
                return false; // במקרה של שגיאה נניח שאין פגישה, אך ניתן לשנות זאת בהתאם לצורך
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }

        [HttpGet]
        public string getNameHallById(string id)
        {
            try
            {
                this.dbContext.OpenConection();
                return unitOfWorkRepository.EventHallRepository.GetNameById(id);

               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in getNameHallById: {ex.Message}");
                return ""; // במקרה של שגיאה נניח שאין פגישה, אך ניתן לשנות זאת בהתאם לצורך
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }


    }
}

