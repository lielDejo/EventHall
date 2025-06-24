using EventModels;
using System.Data;

namespace HallWebService
{
    // מחלקה זו אחראית ליצור אובייקט Image מתוך נתוני מקור נתונים.
    public class ImageCreator : IModelCreator<Image_>
    {
        /// <summary>
        /// יוצר מופע של אובייקט Image מתוך נתוני מקור נתונים (IDataReader).
        /// </summary>
        /// <param name="src">אובייקט IDataReader המכיל את פרטי התמונה ממסד הנתונים.</param>
        /// <returns>אובייקט Image המייצג את התמונה שנקלטה ממסד הנתונים, כולל השדות הרלוונטיים.</returns>
        public Image_ CreateModel(IDataReader src)
        {
            Image_ image = new Image_()
            {
                Id = Convert.ToString(src["ImageId"]),
                HallId = Convert.ToString(src["HallId"]),
                ImageAddress = "http://localhost:5232/images/"  + Convert.ToString(src["ImageAddress"]),
                ImageName = Convert.ToString(src["ImageName"])
            };
            Console.WriteLine(image);
            return image;
        }
    }
}
