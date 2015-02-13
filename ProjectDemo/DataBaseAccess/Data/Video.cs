using CommonClasses;
using CommonClasses.CommonClasses;

namespace DataBaseAccess.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    
    [Table("Video")]
    public partial class Video:PropertyChangedNotification
    {

        public int Id
        {
            get
            {
                return GetValue(() => Id);
            }
            set
            {
                SetValue(()=>Id,value);
            }
        }

        [Required(ErrorMessage = "Title field is required")]
        [StringLength(512)]
        public string Title 
        {
            get { return GetValue(() => Title); }
            set {  SetValue(()=>Title,value); }
        }

        [StringLength(512)]
        public string ProducedBy
        {
            get { return GetValue(() => ProducedBy); }
            set { SetValue(()=>ProducedBy,value);}
        }

        [StringLength(4000)]
        public string Location
        {
            get { return GetValue(() => Location); }
            set {SetValue(()=>Location,value); }
        }

        public TimeSpan? Duration { get; set; }

        [Column(TypeName = "image")]
        public byte[] Poster { get; set; }

        [StringLength(4000)]
        public string Comments
        {
            get { return GetValue(() => Comments); } 
            set{SetValue(()=>Comments,value);}
        }

        [Column(TypeName = "datetime2")]
        public DateTime CreationDate
        {
            get { return GetValue(() => CreationDate); } 
            set{SetValue(()=>CreationDate,value);}
        }
        [RegularExpression(@"\d{4}",ErrorMessage = "Incorrect year. Example: 2010")]
        [StringLength(4,MinimumLength = 4,ErrorMessage = "Year can`t consist of less or more than 4 characters")]
        public string YearOfRelease
        {
            get { return GetValue(() => YearOfRelease); } 
            set{ SetValue(()=>YearOfRelease,value);}
        }
    }
}
