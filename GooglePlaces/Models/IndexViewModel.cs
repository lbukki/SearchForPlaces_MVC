using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GooglePlaces.Models.API;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;

namespace GooglePlaces.Models
{
    public partial class IndexViewModel
    {
        public Places ModelPlaces { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int Radius { get; set; }
        public string TypeOfPlace { get; set; }
        public Dictionary<string, string> TypesOfPlaces { get; set; }
        public int NumberOfplaces { get; set; }
        public string APIKey { get; set; }
        public string Address { get; set; }
    }

    [MetadataType(typeof(IndexViewModelPartial))]
    public partial class IndexViewModel
    {
    }

    public class IndexViewModelPartial
    {
        [Required (ErrorMessage = "A földrajzi szélesség értékét meg kell adni!")]
        public string Latitude;
        [Required (ErrorMessage = "A földrajzi hosszúság értékét meg kell adni!")]
        public string Longitude;
        [Range(50, 2000, ErrorMessage = "A távolságnak 50 és 2000 m között kell lennie!")]
        public int Radius;
        [Required]
        public string TypeOfPlace;
    }
}