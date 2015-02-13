using CommonClasses.CommonClasses;

namespace DataBaseAccess.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FileType")]
    public partial class FileType:PropertyChangedNotification
    {
        public int id { get; set; }

        [RegularExpression(@"^\.\w+",ErrorMessage = "Wrong format of file type(Example .mp3)")]
        [Required(ErrorMessage="Type name required")]
        [StringLength(10)]
        public string Name { get { return GetValue(() => Name); } set { SetValue(() => Name, value); } }

        [Required]
        [StringLength(512)]
        public string Decription { get { return GetValue(() => Decription); } set { SetValue(() => Decription, value); } }
    }
}
