using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ST.Models
{
    public partial class Utilities
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Utilities()
        {

        }

        /// <summary>
        /// Convert to Ecuador TimeZone
        /// </summary>
        /// <param name="_dateTime"></param>
        /// <returns></returns>
        public DateTime convertToTimeZoneEcuador(DateTime _dateTime)
        {
            TimeZone curTimeZone = TimeZone.CurrentTimeZone;
            DateTime curUTC = curTimeZone.ToUniversalTime(_dateTime);
            TimeZoneInfo est = null;
            try
            {
                //Ecuador TimeZone
                est = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {


            }
            catch (InvalidTimeZoneException)
            {


            }
            DateTime targetTime = TimeZoneInfo.ConvertTime(curUTC, est);
            return targetTime;
        }

    }
}