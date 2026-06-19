namespace ConferenceScheduler.Model
{
    public class Track
    {
        public Session MorningSession { get; set; } = new Session { Type = SessionType.Morning };
        public Session AfternoonSession { get; set; } = new Session { Type = SessionType.Afternoon };
    }
}