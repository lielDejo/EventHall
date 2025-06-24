using EventModels;
using System.Data;

namespace HallWebService
{
    // מחלקה זו אחראית לניהול יצירת המודלים השונים. 
    // כל מחלקה (כמו EventHallCreator, CityCreator וכו') אחראית ליצירת מודל מסוג מסוים.

    public class ModelFactory
    {
        EventHallCreator eventHallCreator;
        CityCreator cityCreator;
        RatingCreator ratingCreator;
        MeetCreator meetCreator;
        ImageCreator imageCreator;
        UserCreator userCreator;

        public EventHallCreator EventHallCreator
        {
            get
            {
                if (this.eventHallCreator == null)
                    this.eventHallCreator = new EventHallCreator();
                return this.eventHallCreator;
            }
        }
        public CityCreator CityCreator
        {
            get
            {
                if (this.cityCreator == null)
                    this.cityCreator = new CityCreator();
                return this.cityCreator;
            }
        }
        public RatingCreator RatingCreator
        {
            get
            {
                if (this.ratingCreator == null)
                    this.ratingCreator = new RatingCreator();
                return this.ratingCreator;
            }
        }
        public MeetCreator MeetCreator
        {
            get
            {
                if (this.meetCreator == null)
                    this.meetCreator = new MeetCreator();
                return this.meetCreator;
            }
        }
        public ImageCreator ImageCreator
        {
            get
            {
                if (this.imageCreator == null)
                    this.imageCreator = new ImageCreator();
                return this.imageCreator;
            }
        }
        public UserCreator UserCreator
        {
            get
            {
                if (this.userCreator == null)
                    this.userCreator = new UserCreator();
                return this.userCreator;
            }
        }

        
    }
}