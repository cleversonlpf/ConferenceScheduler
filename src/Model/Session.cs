using System;
using System.Collections.Generic;
using System.Linq;

namespace ConferenceScheduler.Model
{
    public enum SessionType
    {
        Morning,
        Afternoon
    }

    public class Session
    {
        public SessionType Type { get; set; }
        public List<Talk> Talks { get; set; } = new List<Talk>();
        public TimeSpan StartTime { get; set; }

        public TimeSpan TotalDuration => TimeSpan.FromMinutes(Talks.Sum(t => t.DurationMinutes));

        public bool IsValid
        {
            get
            {
                int minutes = (int)TotalDuration.TotalMinutes;
                if (Type == SessionType.Morning)
                    return minutes == 180; // exatamente 3h
                else
                    return minutes >= 180 && minutes <= 240; // entre 3h e 4h
            }
        }
    }
}