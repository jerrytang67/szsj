using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Values;
using Castle.Core.Internal;
using Newtonsoft.Json;

namespace TtWork.ProjectName.Entities.Place
{
    [Table("T_Places")]
    public class Place : ValueObject, IEntity<string>
    {
        public bool IsTransient()
        {
            return !Id.IsNullOrEmpty();
        }

        [NotMapped]
        public string Id
        {
            get => Uid;
            set => Uid = value;
        }

        private Location _location;

        [Key] [StringLength(48)] public string Uid { get; set; }

        [StringLength(48)] public string Name { get; set; }
        [StringLength(24)] public string City { get; set; }
        public int CityId { get; set; }
        [StringLength(24)] public string District { get; set; }
        [StringLength(24)] public string Province { get; set; }

        public double Lat { get; set; }
        public double Lng { get; set; }

        [NotMapped]
        public Location Location
        {
            get => _location;
            set
            {
                _location = value;
                Lng = value.Lng;
                Lat = value.Lat;
            }
        }


        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Uid;
        }
    }

    public class Location
    {
        public Location(double lat, double lng)
        {
            Lat = lat;
            Lng = lng;
        }

        public double Lat { get; set; }
        public double Lng { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(new {lat = Lat, lng = Lng});
        }
    }
}