namespace PicturesGallery.Models
{
    public class PictureViewModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public bool IsSelected { get; set; }
        public ImageMetaData MetaData { get; set; }
    }
}