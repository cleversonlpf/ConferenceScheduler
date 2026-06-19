using System;
using System.Collections.Generic;
using System.Text;
using ConferenceScheduler.Model;

namespace ConferenceScheduler.Services
{
    public class OutputFormatter
    {
        public string Format(List<Track> tracks)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < tracks.Count; i++)
            {
                var track = tracks[i];
                sb.AppendLine($"Trilha {i + 1}:");

                // Sessão matutina
                TimeSpan currentTime = track.MorningSession.StartTime;
                foreach (var talk in track.MorningSession.Talks)
                {
                    sb.AppendLine($"{FormatTime(currentTime)} {talk.Title} {FormatDuration(talk.DurationMinutes)}");
                    currentTime += TimeSpan.FromMinutes(talk.DurationMinutes);
                }
                sb.AppendLine("12:00H Almoço");

                // Sessão vespertina
                currentTime = track.AfternoonSession.StartTime;
                foreach (var talk in track.AfternoonSession.Talks)
                {
                    sb.AppendLine($"{FormatTime(currentTime)} {talk.Title} {FormatDuration(talk.DurationMinutes)}");
                    currentTime += TimeSpan.FromMinutes(talk.DurationMinutes);
                }

                // Evento de Networking
                sb.AppendLine($"{FormatTime(currentTime)} Networking Event");
                sb.AppendLine();
            }
            return sb.ToString().TrimEnd();
        }

        private string FormatTime(TimeSpan time)
        {
            return $"{(int)time.TotalHours:D2}:{time.Minutes:D2}H";
        }

        private string FormatDuration(int minutes)
        {
            if (minutes == 5)
                return "relâmpago";
            else
                return $"{minutes}min";
        }
    }
}