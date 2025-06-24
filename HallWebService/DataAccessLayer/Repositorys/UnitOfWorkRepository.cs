namespace HallWebService
{
    public class UnitOfWorkRepository
    {
        UserRepository _userRepository;
        HallRepository _eventHallRepository;
        CityRepository _cityRepository;
        ImageRepository _imageRepository;
        MeetRepository _meetRepository;
        RatingRepository _ratingRepository;
        DBContext dbContext;
        public UnitOfWorkRepository(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public UserRepository UserRepository
        {
            get
            {
                if (this._userRepository == null)
                {
                    this._userRepository = new UserRepository(DBContext.GetInstance());
                }
                return _userRepository;
            }

        }
        public HallRepository EventHallRepository
        {
            get
            {
                if (_eventHallRepository == null)
                {
                    _eventHallRepository = new HallRepository(DBContext.GetInstance());
                }
                return _eventHallRepository;
            }
        }

        public CityRepository CityRepository
        {
            get
            {
                if (_cityRepository == null)
                {
                    _cityRepository = new CityRepository(DBContext.GetInstance());
                }
                return _cityRepository;
            }
        }

       
        public ImageRepository ImageRepository
        {
            get
            {
                if (_imageRepository == null)
                {
                    _imageRepository = new ImageRepository(DBContext.GetInstance());
                }
                return _imageRepository;
            }
        }

      
        public MeetRepository MeetRepository
        {
            get
            {
                if (_meetRepository == null)
                {
                    _meetRepository = new MeetRepository(DBContext.GetInstance());
                }
                return _meetRepository;
            }
        }

   
        public RatingRepository RatingRepository
        {
            get
            {
                if (_ratingRepository == null)
                {
                    _ratingRepository = new RatingRepository(DBContext.GetInstance());
                }
                return _ratingRepository;
            }
        }
    }
}