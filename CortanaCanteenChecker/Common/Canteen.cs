using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public sealed class Canteen
    {
        public int CanteenId { get; set; }
        public string Name { get; set; }
        public string Meal { get; set; }
        public double MealPrice { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public double AverageRating { get; set; }
        public double AverageWaitingTime { get; set; }
    }
}
