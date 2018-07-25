using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PicturesGallery.Models;

namespace PicturesGallery.Controllers
{
    public class HomeController : Controller
    {
        private const int GOOGLE_MAPS_COORDINATE_PRECISION = 6;

        #region Public Methods

        //
        // GET: /Home/

        public ActionResult Index(int? id)
        {
            List<PictureDbEntity> pictures;
            using (PicturesGalleryEntities context = new PicturesGalleryEntities())
            {
                pictures = context.PictureDbEntities.ToList();
            }

            List<PictureViewModel> viewModels = MapPicturesToPictureViewModels(pictures).ToList();

            if (id.HasValue)
            {
                PictureViewModel selectedViewModel = viewModels.Single(m => m.Id == id);
                selectedViewModel.IsSelected = true;
                selectedViewModel.MetaData = new ImageMetaData();
                FillImageMetadata(id.Value, selectedViewModel.MetaData);
            }
            
            return View(viewModels);
        }

        public ActionResult UploadPicture(HttpPostedFileBase file, int? selectedId)
        {
            if (file == null)
            {
                return RedirectToAction("Index", new { id = selectedId });
            }

            using (PicturesGalleryEntities context = new PicturesGalleryEntities())
            {         
                PictureDbEntity newPicture = new PictureDbEntity
                {
                    Name = file.FileName,
                    Description = "Sample picture description"
                };
                context.PictureDbEntities.Add(newPicture);
                context.SaveChanges();

                // Use Id from database as a file name because it's unique for each picture
                string path = Path.Combine(Server.MapPath("~/Images"), newPicture.Id + Path.GetExtension(file.FileName));
                file.SaveAs(path);
            }

            return RedirectToAction("Index", new {id = selectedId});
        }

        public ActionResult SaveDescription(int id, string description)
        {
            using (PicturesGalleryEntities context = new PicturesGalleryEntities())
            {
                context.PictureDbEntities.Single(p => p.Id == id).Description = description;
                context.SaveChanges();
            }

            return RedirectToAction("Index", new {id});
        }

        public JsonResult GetPictureMetadata(int id)
        {
            ImageMetaData metaData = new ImageMetaData();
            FillImageMetadata(id, metaData);
            return Json(metaData, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private Methods

        private void FillImageMetadata(int id, ImageMetaData metaData)
        {
            using (PicturesGalleryEntities context = new PicturesGalleryEntities())
            {
                PictureDbEntity picture = context.PictureDbEntities.Single(p => p.Id == id);

                string filePath = Path.Combine(Server.MapPath("~/Images"), picture.Id + Path.GetExtension(picture.Name));
                Image image = Image.FromFile(filePath, false);

                PropertyItem[] propertyItems = image.PropertyItems;

                List<int> ids = new List<int> { 0x010F, 0x0110, 0x829A, 0x0132, 0x0103, 0x9000 };
                if (!propertyItems.Select(i => i.Id).Intersect(ids).Any())
                {
                    return;                  
                }

                metaData.MetadataExists = "true";

                PropertyItem manufacturerPropertyItem = propertyItems.SingleOrDefault(i => i.Id == 0x010F);
                if (manufacturerPropertyItem != null)
                {
                    metaData.EquipmentManufacturer = new ASCIIEncoding().GetString(manufacturerPropertyItem.Value);
                }

                PropertyItem modelPropertyItem = propertyItems.SingleOrDefault(i => i.Id == 0x0110);
                if (modelPropertyItem != null)
                {
                    metaData.EquipmentModel = new ASCIIEncoding().GetString(modelPropertyItem.Value);
                }

                PropertyItem exposureTimePropertyItem = propertyItems.SingleOrDefault(i => i.Id == 0x829A);
                if (exposureTimePropertyItem != null)
                {
                    metaData.ExposureTime = GetExposureTimeFromBytes(exposureTimePropertyItem.Value);
                }

                PropertyItem dateTakenPropertyItem = propertyItems.SingleOrDefault(i => i.Id == 0x0132);
                if (dateTakenPropertyItem != null)
                {
                    metaData.DateTaken = new ASCIIEncoding().GetString(dateTakenPropertyItem.Value);
                }

                PropertyItem exifVersionPropertyItem = propertyItems.SingleOrDefault(i => i.Id == 0x9000);
                if (exifVersionPropertyItem != null)
                {
                    metaData.ExifVersion = new ASCIIEncoding().GetString(exifVersionPropertyItem.Value).Insert(2, ".").Trim('0');
                }

                PropertyItem latitudePropertyItem = propertyItems.SingleOrDefault(i => i.Id == 0x0002);
                PropertyItem latitudeRefPropertyItem = propertyItems.SingleOrDefault(i => i.Id == 0x0001);

                PropertyItem longitudePropertyItem = propertyItems.SingleOrDefault(i => i.Id == 0x0004);
                PropertyItem longitudeRefPropertyItem = propertyItems.SingleOrDefault(i => i.Id == 0x0003);

                if (latitudePropertyItem != null && latitudeRefPropertyItem != null && longitudePropertyItem != null &&
                    longitudeRefPropertyItem != null)
                {
                    metaData.LatitudeLongitude = GetLatitudeLongitude(latitudePropertyItem.Value,
                        longitudePropertyItem.Value, latitudeRefPropertyItem.Value, longitudeRefPropertyItem.Value);
                }
            }
        }

        private string GetLatitudeLongitude(byte[] latitudeBytes, byte[] longitudeBytes, byte[] latitudeRefBytes,
            byte[] longitudeRefBytes)
        {
            string latitudeRef = new ASCIIEncoding().GetString(latitudeRefBytes);
            string longitudeRef = new ASCIIEncoding().GetString(longitudeRefBytes);

            string latitude = GetCoordinate(latitudeBytes);
            if (latitudeRef[0] == 'S')
            {
                latitude = latitude.Insert(0, "-");
            }

            string longitude = GetCoordinate(longitudeBytes);
            if (longitudeRef[0] == 'W')
            {
                longitude = longitude.Insert(0, "-");
            }

            return latitude + "," + longitude;
        }

        private string GetCoordinate(byte[] coordinateBytes)
        {
            long degreesNumerator = GetLongNumberFromBytesArray(coordinateBytes, 0);
            long degreesDenominator = GetLongNumberFromBytesArray(coordinateBytes, 4);

            long minutesNumerator = GetLongNumberFromBytesArray(coordinateBytes, 8);
            long minutesDenominator = GetLongNumberFromBytesArray(coordinateBytes, 12);

            long secondsNumerator = GetLongNumberFromBytesArray(coordinateBytes, 16);
            long secondsDenominator = GetLongNumberFromBytesArray(coordinateBytes, 20);

            double coordinateInDegrees = ((double) degreesNumerator) / degreesDenominator +
                                         ((double) minutesNumerator) / minutesDenominator / 60 +
                                         ((double)secondsNumerator) / secondsDenominator / 3600;

            return
                Math.Round(coordinateInDegrees, GOOGLE_MAPS_COORDINATE_PRECISION, MidpointRounding.AwayFromZero)
                    .ToString(CultureInfo.InvariantCulture);
        }

        /// <param name="bytes">Bytes array which holds 4 bytes for every number.</param>
        private long GetLongNumberFromBytesArray(byte[] bytes, int startIndex)
        {
            long result = 0;
            long weight = 1;
            for (int i = 0; i < 4; i++)
            {
                result += bytes[startIndex + i] * weight;
                weight *= 256;
            }

            return result;
        }

        private string GetExposureTimeFromBytes(byte[] bytes)
        {
            // bytes is a 8-byte array, first 4 of them represent numerator,
            // next 4 bytes represent denominator

            long numerator = GetLongNumberFromBytesArray(bytes, 0);
            long denominator = GetLongNumberFromBytesArray(bytes, 4);

            return numerator + " / " + denominator + " sec";
        }

        private IEnumerable<PictureViewModel> MapPicturesToPictureViewModels(IEnumerable<PictureDbEntity> pictures)
        {
            return pictures.Select(p => new PictureViewModel
                {
                    Id = p.Id,
                    FileName = p.Id + Path.GetExtension(p.Name),
                    Description = p.Description
                });
        }

        #endregion
    }
}
