using System.Data.Entity;

namespace PicturesGallery
{
    public class PicturesGalleryEntities : DbContext
    {
        // Your context has been configured to use a 'PicturesGalleryEntities' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'PicturesGallery.PicturesGalleryEntities' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'PicturesGalleryEntities' 
        // connection string in the application configuration file.
        public PicturesGalleryEntities()
            : base("name=PicturesGalleryEntities")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<PictureDbEntity> PictureDbEntities { get; set; }
    }

    public class PictureDbEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}