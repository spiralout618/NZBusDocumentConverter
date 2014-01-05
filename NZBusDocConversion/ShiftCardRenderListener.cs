using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace NZBusDocConversion
{
    public class ShiftCardRenderListener : IRenderListener
    {

        private ShiftInfo Info;
        private NextLine nextLine;
        private TripInfo CurrentTrip;
        private bool CurrentTripEndsWithStop;

        public ShiftCardRenderListener()
        {
            Info = new ShiftInfo();
            nextLine = NextLine.Unknown;
        }

        public void PrintShiftInfo()
        {
            Console.WriteLine(Info.ToString());
        }

        public void BeginTextBlock() { }

        public void EndTextBlock() { }

        public void RenderImage(ImageRenderInfo renderInfo) { }

        public void RenderText(TextRenderInfo renderInfo)
        {
            Console.WriteLine(string.Format("Text:{0}, AscentLine:{1}, DescentLine{2}",
                                            renderInfo.GetText(),
                                            renderInfo.GetAscentLine().GetStartPoint(),
                                            renderInfo.GetDescentLine().GetStartPoint()));

            ProcessText(renderInfo.GetText());

        }

        private void ProcessText(string text)
        {
            if (nextLine != NextLine.Unknown)
            {
                if (nextLine.Equals(NextLine.CheckForRunOff))
                {
                    if (text.StartsWith("NOT IN SERVICE"))
                        CurrentTrip.EndsWithRunOff = true;
                    Info.Trips.Add(CurrentTrip);
                    nextLine = NextLine.Unknown;
                }
                else
                {
                    if (nextLine.Equals(NextLine.SignOn))
                    {
                        Info.SignOn = text.Trim();
                        nextLine = NextLine.Unknown;
                    }
                    else if (nextLine.Equals(NextLine.SignOff))
                    {
                        Info.SignOff = text.Trim();
                        nextLine = NextLine.Unknown;
                    }
                    else if (nextLine.Equals(NextLine.TripLine1) || nextLine.Equals(NextLine.TripLine2))
                    {
                        //Note each line is a stop what we don't have is info about when the bus arrives
                        if (text.Contains("Stop"))
                            CurrentTrip.Stop = text;
                        else
                            CurrentTrip.RouteDescription = text;

                        nextLine = nextLine.Equals(NextLine.TripLine1) ? NextLine.TripLine1Time : NextLine.TripLine2Time;
                    }
                    else if (nextLine.Equals(NextLine.TripLine1Time))
                    {
                        CurrentTrip.DepartureTime = text;
                        nextLine = NextLine.TripLine2;

                    }
                    else if (nextLine.Equals(NextLine.TripLine2Time))
                    {
                        CurrentTrip.ArrivalTime = text;
                        nextLine = NextLine.CheckForRunOff;
                    }
                    return;
                }
            }

            if (text.StartsWith("SHIFT"))
                Info.Shift = text.Replace("SHIFT", "").Trim();

            if (text.StartsWith("SIGN ON"))
                nextLine = NextLine.SignOn;

            if (text.StartsWith("SIGN OFF"))
                nextLine = NextLine.SignOff;

            if (text.StartsWith("Route:"))
            {
                CurrentTrip = new TripInfo();
                var splitRouteInfo = text.Split('-');
                CurrentTrip.Route = splitRouteInfo[0].RemoveWordAndTrim("Route:");
                CurrentTrip.Variation = splitRouteInfo[1].RemoveWordAndTrim("V:");
                CurrentTrip.Direction = splitRouteInfo[2].RemoveWordAndTrim("D:");
                CurrentTrip.Trip = splitRouteInfo[3].RemoveWordAndTrim("Trip:");
                CurrentTrip.DestinationSign = splitRouteInfo[4].RemoveWordAndTrim("D:");
                nextLine = NextLine.TripLine1;
            }
        }

        public ShiftInfo GetShiftInfo()
        {
            return Info;
        }
    }

    public enum NextLine
    {
        SignOn,
        SignOff,
        TripLine1,
        TripLine1Time,
        TripLine2,
        TripLine2Time,
        CheckForRunOff,
        Unknown
    }
}
