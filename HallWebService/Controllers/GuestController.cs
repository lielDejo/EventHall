using EventModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection.PortableExecutable;

namespace HallWebService
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        DBContext dbContext;
        UnitOfWorkRepository unitOfWorkRepository;

        public GuestController()
        {
            this.dbContext = DBContext.GetInstance();
            this.unitOfWorkRepository = new UnitOfWorkRepository(this.dbContext);


        }
        [HttpGet]
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
                foreach(EventHall eventHall in eventHallViewModel.Halls)
                {
                    eventHall.HallImage = unitOfWorkRepository.ImageRepository.GetLsPicByHallId(eventHall.Id);

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
       
        [HttpGet]
        public EventHall GetEventHall(string id = "")
        {
            try
            {
                EventHall eventHall = new EventHall();
                this.dbContext.OpenConection();
                eventHall = unitOfWorkRepository.EventHallRepository.GetById(id);
                eventHall.HallImage = unitOfWorkRepository.ImageRepository.GetLsPicByHallId(id);
                return eventHall;
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
        public string AddNewUser(string userName, string password, string phoneNumber, string email, bool isManage)
        {
            try
            {
                this.dbContext.OpenConection();
                return unitOfWorkRepository.UserRepository.Create(userName,  password,  phoneNumber,  email,  isManage);
            }
            catch(Exception ex) 
            {
                string message = ex.Message;    
                return "";
            }
            finally
            {
                this.dbContext.CloseConection();
            }
        }
        [HttpPost]
        public bool UpdateEventHall(EventHall eventHall)
        {
            try
            {
                this.dbContext.OpenConection();
                return unitOfWorkRepository.EventHallRepository.UpDate(eventHall);
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
        [HttpGet]
        public string LoginUser(string userName, string password)
        {
            string id = "";
            try
            {
                this.dbContext.OpenConection();
                id = unitOfWorkRepository.UserRepository.GetIDByUserNameAndPassword(userName, password);
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
            return id;
        }
      
        [HttpGet]
        public string LoginAdmin(string userName, string password)
        {
            string id = "";
            try
            {
                this.dbContext.OpenConection();
                id = unitOfWorkRepository.UserRepository.GetIDByAdminNameAndPassword(userName, password);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return "";
            }
            finally
            {
                this.dbContext.CloseConection();
            }
            return id;
        }

        [HttpGet]
        public string RegisterUser(string userName, string password, string phoneNumber, string email)
        {
            string id = "";
            try
            {
                this.dbContext.OpenConection();
                id = unitOfWorkRepository.UserRepository.Create(userName, password, phoneNumber,  email, false);
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
            return id;
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
        
    }
}

  