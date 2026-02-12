using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Domain.Entities.SharedValueObjects
{
    public class GeoLocation
    {
        public decimal Latitude { get; private set; }
        public decimal Longitude { get; private set; }

        
        private GeoLocation() { }

        public GeoLocation(decimal latitude, decimal longitude)
        {
            if (latitude < -90 || latitude > 90)
                throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90");

            if (longitude < -180 || longitude > 180)
                throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180");

            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
