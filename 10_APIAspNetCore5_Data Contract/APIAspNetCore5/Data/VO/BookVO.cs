using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace APIAspNetCore5.Data.VO
{
    [MetadataType(typeof(BookVO))]
    public class BookVO
    {
        [Display(Order = 1, Name = "codigo")]
        public long? Id { get; set; }

        [Display(Order = 2)]
        public string Title { get; set; }

        [Display(Order = 3)]
        public string Author { get; set; }

        [Display(Order = 5)]
        public decimal Price { get; set; }

        [Display(Order = 4)]
        public DateTime LaunchDate { get; set; }
    }
}
