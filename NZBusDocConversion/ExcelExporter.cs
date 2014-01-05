using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZBusDocConversion
{
    class ExcelExporter
    {
        public void ExportToCSV(List<ShiftInfo> infos)
        {
            var csvVals = "Shift; Sign on; Sign off; Route; Direction; Variation; Trip; DestinationSign; Stop; RouteDescription; Departure Time; Arrival Time; Ends with runoff" + System.Environment.NewLine;
            foreach (var shift in infos)
            {
                foreach (var trip in shift.Trips)
                {
                    csvVals += string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13}", shift.Shift, shift.SignOn, shift.SignOff, trip.Route, trip.Direction, trip.Variation, trip.Trip, trip.DestinationSign, trip.Stop, trip.RouteDescription, trip.DepartureTime, trip.ArrivalTime, trip.EndsWithRunOff, System.Environment.NewLine );
                }
            }
            File.WriteAllText("NZBusShiftExport.csv", csvVals);
        }

    }
}
