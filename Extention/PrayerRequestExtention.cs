using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PrayerRequest.Service.Models;

namespace PrayerRequest.Service.Extention
{
    public static class PrayerRequestExtention
    {
        public static bool IsModified(this PrayerRequestDetail prayerRequest)
        {
            if(prayerRequest.Id < 0)
            {
                return true;
            }
            return false;
        }
    }
}