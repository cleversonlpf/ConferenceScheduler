using System;
using System.Collections.Generic;
using System.Linq;
using ConferenceScheduler.Model;

#nullable enable annotations

namespace ConferenceScheduler.Services
{
    public class Scheduler
    {
        public List<Track> Schedule(List<Talk> talks)
        {
            // Cópia da lista para não modificar a original
            var remainingTalks = new List<Talk>(talks);
            var tracks = new List<Track>();

            while (remainingTalks.Count > 0)
            {
                var track = new Track();

                // Preenche manhã (exatos 180 min)
                var morningTalks = FindExactSum(remainingTalks, 180);
                if (morningTalks == null)
                    throw new InvalidOperationException(
                        "Não foi possível encaixar as palestras da manhã. Verifique os dados de entrada.");
                track.MorningSession.Talks = morningTalks;

                // Remove as talks usadas de manhã da lista restante
                foreach (var t in morningTalks)
                    remainingTalks.Remove(t);

                // Preenche tarde (entre 180 e 240 min)
                var afternoonTalks = FillAfternoon(remainingTalks);
                track.AfternoonSession.Talks = afternoonTalks;

                foreach (var t in afternoonTalks)
                    remainingTalks.Remove(t);

                // Define horários de início das sessões
                track.MorningSession.StartTime = new TimeSpan(9, 0, 0);   // 09:00
                track.AfternoonSession.StartTime = new TimeSpan(13, 0, 0); // 13:00

                tracks.Add(track);
            }

            return tracks;
        }

        // Backtracking para encontrar subconjunto com soma exata 'targetMinutes'
        private List<Talk>? FindExactSum(List<Talk> talks, int targetMinutes)
        {
            // Ordena decrescente para melhor desempenho (poda)
            var sorted = talks.OrderByDescending(t => t.DurationMinutes).ToList();
            List<Talk>? best = null;
            FindCombination(sorted, 0, targetMinutes, new List<Talk>(), ref best);
            return best;
        }

        private void FindCombination(List<Talk> talks, int index, int remaining,
            List<Talk> current, ref List<Talk>? best)
        {
            if (remaining == 0)
            {
                best = new List<Talk>(current);
                return;
            }
            if (remaining < 0 || index >= talks.Count || best != null)
                return;

            // Inclui a talk atual
            var talk = talks[index];
            current.Add(talk);
            FindCombination(talks, index + 1, remaining - talk.DurationMinutes, current, ref best);
            current.RemoveAt(current.Count - 1);

            // Pula a talk atual
            FindCombination(talks, index + 1, remaining, current, ref best);
        }

        // Preenche a tarde com um algoritmo guloso
        private List<Talk> FillAfternoon(List<Talk> availableTalks)
        {
            var selected = new List<Talk>();
            int total = 0;
            var ordered = availableTalks.OrderByDescending(t => t.DurationMinutes).ToList();

            // Adiciona talks enquanto não ultrapassar 240 min
            foreach (var talk in ordered)
            {
                if (total + talk.DurationMinutes <= 240)
                {
                    selected.Add(talk);
                    total += talk.DurationMinutes;
                }
            }

            // Se após o preenchimento o total for menor que 180, tenta adicionar mais talks (se possível)
            // Isso pode ocorrer se não houver talks suficientes; nesse caso a validação falhará.

            if (total < 180)
            {
                throw new InvalidOperationException(
                    "Não foi possível preencher a tarde com no mínimo 180 minutos. Verifique os dados de entrada.");
            }

            return selected;
        }
    }
}