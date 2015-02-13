namespace DataBaseAccess.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MultimediaModel : DbContext
    {
        public MultimediaModel()
            : base("name=MultimediaConnection")
        {
        }

        public virtual DbSet<Audio> Audios { get; set; }
        public virtual DbSet<FileType> FileTypes { get; set; }
        public virtual DbSet<Video> Videos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
