using System;
using System.IO;
using ConferenceScheduler.Services;

namespace ConferenceScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "sample_input.csv";
            string outputPath = "Agenda.txt";

            if (args.Length >= 1)
            {
                if (args[0].Equals("-h", StringComparison.OrdinalIgnoreCase) ||
                    args[0].Equals("-help", StringComparison.OrdinalIgnoreCase))
                {
                    ShowHelp();
                    return;
                }

                inputPath = args[0];
                if (args.Length >= 2)
                    outputPath = args[1];
            }

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Erro: Arquivo de entrada '{inputPath}' não encontrado.");

                if (args.Length == 0)
                {
                    ShowHelp();
                    return;
                }

                return;
            }

            try
            {
                var parser = new InputParser();
                var talks = parser.Parse(inputPath);
                Console.WriteLine($"{talks.Count} palestras carregadas.");

                var scheduler = new Scheduler();
                var tracks = scheduler.Schedule(talks);

                var formatter = new OutputFormatter();
                string agenda = formatter.Format(tracks);

                Console.WriteLine(agenda);
                File.WriteAllText(outputPath, agenda);
                Console.WriteLine($"\nAgenda salva em: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Uso: dotnet run -- <arquivo_entrada.csv> [arquivo_saida.txt]");
            Console.WriteLine("Exemplo: dotnet run -- data/sample_input.csv data/Agenda.txt");
        }
    }
}