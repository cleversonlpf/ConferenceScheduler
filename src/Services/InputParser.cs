using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ConferenceScheduler.Model;
using CsvHelper;
using CsvHelper.Configuration;

namespace ConferenceScheduler.Services
{
    public class InputParser
    {
        public List<Talk> Parse(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Arquivo de entrada não encontrado: {filePath}");

            var talks = new List<Talk>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                MissingFieldFound = null,
                BadDataFound = null // ignora linhas mal formadas;
            };

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);

            while (csv.Read())
            {
                string title = csv.GetField(0)?.Trim() ?? string.Empty;
                string durationStr = csv.GetField(1)?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(durationStr))
                    continue; // pula linhas vazias ou incompletas

                int minutes = ParseDuration(durationStr);
                talks.Add(new Talk { Title = title, DurationMinutes = minutes });
            }

            if (talks.Count == 0)
                throw new InvalidDataException("Nenhuma palestra válida encontrada no arquivo.");

            return talks;
        }

        private int ParseDuration(string input)
        {
            if (input.Equals("relâmpago", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("lightning", StringComparison.OrdinalIgnoreCase)) // Adicionado suporte para "lightning" em inglês;
                return 5;

            // Remove "min" (case insensitive) e converte
            string numberPart = input.Replace("min", "", StringComparison.OrdinalIgnoreCase).Trim();
            if (int.TryParse(numberPart, out int minutes) && minutes > 0)
                return minutes;

            throw new FormatException($"Duração inválida: '{input}'. Esperado 'XXmin' ou 'relâmpago'.");
        }
    }
}