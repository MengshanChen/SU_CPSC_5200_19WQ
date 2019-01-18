using System;
using System.Globalization;
using Newtonsoft.Json;

namespace restapi.Models
{
    public class TimecardLine
    {
        public int Week { get; set; }

        public int Year { get; set; }

        public DayOfWeek Day { get; set; }

        public float Hours { get; set; }

        public string Project { get; set; }
    }

    public class AnnotatedTimecardLine : TimecardLine
    {
        private DateTime workDate;
        private DateTime? periodFrom;
        private DateTime? periodTo;

        public AnnotatedTimecardLine(TimecardLine line)
        {
            Week = line.Week;
            Year = line.Year;
            Day = line.Day;
            Hours = line.Hours;
            Project = line.Project;

            Recorded = DateTime.UtcNow;
            workDate = FirstDateOfWeekISO8601(line.Year, line.Week).AddDays((int)line.Day - 1);
            UniqueIdentifier = Guid.NewGuid();
        }

        public void replaceLine(TimecardLine line){
            Week = line.Week;
            Year = line.Year;
            Day = line.Day;
            Hours = line.Hours;
            Project = line.Project;

            Recorded = DateTime.UtcNow;
            workDate = FirstDateOfWeekISO8601(line.Year, line.Week).AddDays((int)line.Day - 1);
        }

        public void updateLine(Nullable<int> week, Nullable<int> year, Nullable<DayOfWeek> day, Nullable<float> hours, string project){
            bool isUpdate = false;

            //assume that there is no zero week 
            if(week.HasValue && week != 0){
                this.Week = week.Value; 
                isUpdate = true; 
            }

            //assume that there is no zero year
            if(year.HasValue && year != 0){
                this.Year = year.Value; 
                isUpdate = true;
            }

            //only differnt day can update it 
            if(day.HasValue && !day.Equals(this.Day)){
                this.Day = day.Value; 
                isUpdate = true; 
            }

            //assume that default zero has meaning 
            if(hours.HasValue){
                this.Hours = hours.Value;
            }

            //assume that default "string" has meaning 
            if(!project.Equals(this.Project)){
                this.Project = project;
            }
            Recorded = DateTime.UtcNow;
            if(isUpdate){
                workDate = FirstDateOfWeekISO8601(this.Year, this.Week).AddDays((int)this.Day - 1); 
            }
        }
        

        public DateTime Recorded { get; set; }

        public string WorkDate { get => workDate.ToString("yyyy-MM-dd"); }

        public float LineNumber { get; set; }

        [JsonProperty("recId")]
        public int RecordIdentity { get; set; } = 0;

        [JsonProperty("recVersion")]
        public int RecordVersion { get; set; } = 0;

        public Guid UniqueIdentifier { get; set; }

        public string PeriodFrom { get => periodFrom?.ToString("yyyy-MM-dd"); }

        public string PeriodTo { get => periodTo?.ToString("yyyy-MM-dd"); }

        public string Version { get; set; } = "line-0.1";

        private static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            var jan1 = new DateTime(year, 1, 1);
            var daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            var firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            var firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }

            var result = firstThursday.AddDays(weekNum * 7);

            return result.AddDays(-3);
        }        
    }
}