
namespace ConferenceScheduler.Tests
{
    public class SchedulerTests
    {
        [Fact]
        public void Schedule_AllTalksFitInOneTrack_ReturnsSingleTrack()
        {
            // 3 talks de 60 min = 180 min (manhã) + 3 talks de 60 min = 180 min (tarde)
            var talks = new List<Talk>
            {
                new Talk { Title = "A", DurationMinutes = 60 },
                new Talk { Title = "B", DurationMinutes = 60 },
                new Talk { Title = "C", DurationMinutes = 60 },
                new Talk { Title = "D", DurationMinutes = 60 },
                new Talk { Title = "E", DurationMinutes = 60 },
                new Talk { Title = "F", DurationMinutes = 60 },
            };

            var scheduler = new Scheduler();
            var tracks = scheduler.Schedule(talks);

            Assert.Single(tracks);
            Assert.Equal(180, (int)tracks[0].MorningSession.TotalDuration.TotalMinutes);
            Assert.True(tracks[0].MorningSession.IsValid);
            Assert.Equal(180, (int)tracks[0].AfternoonSession.TotalDuration.TotalMinutes);
            Assert.True(tracks[0].AfternoonSession.IsValid);
        }

        [Fact]
        public void Schedule_EveningMustBeBetween180And240()
        {
            // Talks que forçam tarde a ter 181 min (mínimo +1)
            var talks = new List<Talk>
            {
                new Talk { Title = "M1", DurationMinutes = 60 },
                new Talk { Title = "M2", DurationMinutes = 60 },
                new Talk { Title = "M3", DurationMinutes = 60 }, // manhã 180
                new Talk { Title = "A1", DurationMinutes = 60 },
                new Talk { Title = "A2", DurationMinutes = 60 },
                new Talk { Title = "A3", DurationMinutes = 30 },
                new Talk { Title = "A4", DurationMinutes = 31 }, // total tarde = 181
            };

            var scheduler = new Scheduler();
            var tracks = scheduler.Schedule(talks);
            Assert.Single(tracks);
            Assert.InRange((int)tracks[0].AfternoonSession.TotalDuration.TotalMinutes, 180, 240);
        }

        [Fact]
        public void Schedule_RequiresMultipleTracks_WhenTalksExceedDay()
        {
            // Talks suficientes para 2 tracks
            var talks = new List<Talk>();
            for (int i = 0; i < 12; i++) // 12 * 60 min = 720 min = 4 tracks de 180 min cada.
                talks.Add(new Talk { Title = $"T{i}", DurationMinutes = 60 });

            // Cada track: manhã 3 talks (180), tarde 3 talks (180) -> 6 talks por track
            var scheduler = new Scheduler();
            var tracks = scheduler.Schedule(talks);
            Assert.Equal(2, tracks.Count);
            Assert.All(tracks, t => Assert.Equal(180, (int)t.MorningSession.TotalDuration.TotalMinutes));
            Assert.All(tracks, t => Assert.InRange((int)t.AfternoonSession.TotalDuration.TotalMinutes, 180, 240));
        }
    }
}