using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrayerRequest.Service.Models
{
    public class PrayerRequestDetail
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Request { get; set; }

        public DateTime Date { get; set; }

        public bool IsCurrent { get; set; }
    }
}