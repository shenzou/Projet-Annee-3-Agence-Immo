using Newtonsoft.Json.Linq;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;




namespace PROJET_BDD
{
   
    public class Accomodation
    {
        public string host_id;
        public string room_id;
        [JsonIgnore] public string room_type;
        [JsonIgnore] public string borough;
        [JsonIgnore] public string neighborhood;
        [JsonIgnore] public string reviews;
        [JsonIgnore] public double overall_satisfaction;
        [JsonIgnore] public string accommodates;
        [JsonIgnore] public string bedrooms;
        [JsonIgnore] public string price;
        [JsonIgnore] public string minstay;
        [JsonIgnore] public string latitude;
        [JsonIgnore] public string longitude;
        public string week;
        public string availability;

        public Accomodation(string room_id, string room_type, string borough, string neighborhood, string reviews, double overall_satisfaction, string accommodates, string bedrooms, string price, string minstay, string latitude, string longitude, string week, string availability)
        {
            this.room_id = room_id;
            this.room_type = room_type;
            this.borough = borough;
            this.neighborhood = neighborhood;
            this.reviews = reviews;
            this.overall_satisfaction = overall_satisfaction;
            this.accommodates = accommodates;
            this.bedrooms = bedrooms;
            this.price = price;
            this.minstay = minstay;
            this.latitude = latitude;
            this.longitude = longitude;
            this.week = week;
            this.availability = availability;
        }
    }
}
