using System;
using System.Runtime.Serialization;

namespace CommonClasses
{
    [DataContract]
    public sealed class ImdbInformation
    {
        [DataMember(Name = "Title")]
        public string Title { get; set; }
        [DataMember(Name = "Year")]
        public string Year { get; set; }
        [DataMember(Name = "Runtime")]
        public string Runtime { get; set; }
        [DataMember(Name = "Actors")]
        public string Actors { get; set; }
        [DataMember(Name = "Plot")]
        public string Plot { get; set; }
        [DataMember(Name = "Poster")]
        public string PosterUrl { get; set; }
        [DataMember(Name = "Response")]
        public string Response { get; set; }
        [DataMember(Name = "Director")]
        public string Director { get; set; }
        [DataMember(Name = "Released")]
        public string Released { get; set; }

        public string YearOfRelease
        {
            get
            {
                return DateTime.Parse(Released).Year.ToString();
            }
        }
    }
}
