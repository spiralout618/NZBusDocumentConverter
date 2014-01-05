using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZBusDocConversion
{
    public class ShiftInfo
    {
        public string TotalHours { get; set; }
        public string Shift { get; set; }
        public string SignOn { get; set; }
        public string SignOff { get; set; }
        public string DepotFinish { get; set; }
        public List<TripInfo> Trips { get; set; }

        public ShiftInfo()
        {
            Trips = new List<TripInfo>();
        }

        public string ToString()
        {
            var trips = "";
            var count = 1;
            foreach (var trip in Trips)
            {
                trips += string.Format("Trip {0}:{1}{2}{3}", count, System.Environment.NewLine, trip.ToString(), System.Environment.NewLine);
                count++;
            }
            return string.Format(@"Shift:{0}, Sign on:{1}, Sign off:{2},{3}{4}", Shift, SignOn, SignOff, System.Environment.NewLine, trips);
        }

    }

    public class TripInfo
    {
        public string Route { get; set; }
        public string Direction { get; set; }
        public string Variation { get; set; }
        public string Trip { get; set; }
        public string DestinationSign { get; set; }
        public string Stop { get; set; }
        public string RouteDescription { get; set; }
        public bool EndsWithRunOff { get; set; }
        public string ArrivalTime { get; set; }
        public string DepartureTime { get; set; }

        public string ToString()
        {
            return string.Format("Route:{0}, Direction:{1}, Variation:{2}, Trip:{3}, DestinationSign:{4}, Stop:{5}, RouteDescription:{6}, Departure Time:{7}, Arrival Time:{8}, Ends with runoff:{9}",
                Route, Direction, Variation, Trip, DestinationSign, Stop, RouteDescription, DepartureTime, ArrivalTime, EndsWithRunOff);
        }
    }
}
