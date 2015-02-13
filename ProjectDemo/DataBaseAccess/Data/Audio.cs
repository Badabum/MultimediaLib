using CommonClasses.CommonClasses;

namespace DataBaseAccess.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Audio")]
    public partial class Audio:PropertyChangedNotification
    {
        public int Id { get; set; }

        [Required]
        [StringLength(512)]
        public string Title { get { return GetValue(() => Title); } set{ SetValue(()=>Title,value);} }

        [StringLength(512)]
        public string Artist { get { return GetValue(() => Artist); } set { SetValue(() => Artist, value); } }

        [StringLength(512)]
        public string Album { get { return GetValue(() => Album); } set { SetValue(() => Album, value); } }

        [StringLength(512)]
        public string Location { get { return GetValue(() => Location); } set { SetValue(() => Location, value); } }

        public TimeSpan? Duration { get { return GetValue(() => Duration); } set { SetValue(() => Duration, value); } }

        [Column(TypeName = "datetime2")]
        public DateTime? CreationDate { get { return GetValue(() => CreationDate); } set { SetValue(() => CreationDate, value); } }
    }
}
