using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8; 

        while (true)
        {
            Console.WriteLine("====================================");
            Console.WriteLine("        MENU PRINCIPAL ISI");
            Console.WriteLine("====================================");
            Console.WriteLine("1️ Calcular Estatísticas");
            Console.WriteLine("2️ Gerar Resumo Diário em XML");
            Console.WriteLine("0️ Sair");
            Console.WriteLine("------------------------------------");
            Console.Write("Escolha uma opção: ");

            string opcao = Console.ReadLine();

            if (opcao == "1")
            {
                string caminhoEntrada = @"C:\ISI\Resultados\api_resultados.csv";
                string caminhoSaida = @"C:\ISI\Resultados\estatisticas_completas.csv";

                Directory.CreateDirectory(Path.GetDirectoryName(caminhoSaida));

                var linhas = File.ReadAllLines(caminhoEntrada)
                                 .Skip(1)
                                 .Where(l => !string.IsNullOrWhiteSpace(l))
                                 .ToList();

                
                char separador = ';';

                
                List<double> LerColuna(List<string> linhas, int indice)
                {
                    return linhas.Select(l => l.Split(separador))
                                 .Where(p => p.Length > indice && double.TryParse(p[indice], NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                                 .Select(p => double.Parse(p[indice], CultureInfo.InvariantCulture))
                                 .ToList();
                }

                
                var temp_min_api = LerColuna(linhas, 1);
                var temp_api = LerColuna(linhas, 2);
                var hum_api = LerColuna(linhas, 3);
                var temp_max_api = LerColuna(linhas, 4);

              
                Console.WriteLine($"📊 Linhas lidas: {linhas.Count}");
                Console.WriteLine($"🌡️ temp_min_api: {temp_min_api.Count} valores");
                Console.WriteLine($"🔥 temp_api: {temp_api.Count} valores");
                Console.WriteLine($"💧 hum_api: {hum_api.Count} valores");
                Console.WriteLine($"☀️ temp_max_api: {temp_max_api.Count} valores");

                if (temp_api.Count == 0)
                {
                    Console.WriteLine("❌ Nenhum dado foi lido! Verifica o separador ou o caminho do ficheiro.");
                    return;
                }

                
                double Mediana(List<double> valores)
                {
                    var ordenados = valores.OrderBy(v => v).ToList();
                    int meio = ordenados.Count / 2;
                    return ordenados.Count % 2 == 0 ? (ordenados[meio - 1] + ordenados[meio]) / 2.0 : ordenados[meio];
                }

                double Moda(List<double> valores)
                {
                    return valores.GroupBy(v => v)
                                  .OrderByDescending(g => g.Count())
                                  .First().Key;
                }

                (double media, double mediana, double moda, double variancia, double amplitude, double desvio) Estatisticas(List<double> v)
                {
                    if (v == null || v.Count == 0)
                    {
                        return (0, 0, 0, 0, 0, 0);
                    }

                    double media = v.Average();
                    double mediana = Mediana(v);
                    double moda = Moda(v);
                    double variancia = v.Average(x => Math.Pow(x - media, 2));
                    double amplitude = v.Max() - v.Min();
                    double desvio = Math.Sqrt(variancia);
                    return (media, mediana, moda, variancia, amplitude, desvio);
                }

                var estTempMin = Estatisticas(temp_min_api);
                var estTemp = Estatisticas(temp_api);
                var estHum = Estatisticas(hum_api);
                var estTempMax = Estatisticas(temp_max_api);

                
                string resultado = "Variavel;Media;Mediana;Moda;Variancia;Amplitude;DesvioPadrao\n" +
                    $"temp_min_api;{estTempMin.media:F2};{estTempMin.mediana:F2};{estTempMin.moda:F2};{estTempMin.variancia:F2};{estTempMin.amplitude:F2};{estTempMin.desvio:F2}\n" +
                    $"temp_api;{estTemp.media:F2};{estTemp.mediana:F2};{estTemp.moda:F2};{estTemp.variancia:F2};{estTemp.amplitude:F2};{estTemp.desvio:F2}\n" +
                    $"hum_api;{estHum.media:F2};{estHum.mediana:F2};{estHum.moda:F2};{estHum.variancia:F2};{estHum.amplitude:F2};{estHum.desvio:F2}\n" +
                    $"temp_max_api;{estTempMax.media:F2};{estTempMax.mediana:F2};{estTempMax.moda:F2};{estTempMax.variancia:F2};{estTempMax.amplitude:F2};{estTempMax.desvio:F2}";

                File.WriteAllText(caminhoSaida, resultado);
                Console.WriteLine($"\n Estatísticas guardadas em: {caminhoSaida}");
            }

            else if (opcao == "2")
            {
                GerarXML.CriarResumoXML();
            }
            else if (opcao == "0")
            {
                Console.WriteLine("\n Encerrando o programa...");
                break;
            }
            else
            {
                Console.WriteLine("\n❌ Opção inválida. Tenta novamente.\n");
            }
        }
    }
}
