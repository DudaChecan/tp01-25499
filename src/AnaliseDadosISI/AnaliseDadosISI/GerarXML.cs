using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Xml.Linq;

class GerarXML
{
    public static void CriarResumoXML()
    {
        Console.WriteLine("\n🟣 Iniciando geração do resumo diário em XML...\n");

        string caminhoEntrada = @"C:\ISI\Resultados\api_resultados.csv";
        string caminhoSaidaXml = @"C:\ISI\Resultados\resumo_diario.xml";

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(caminhoSaidaXml)!);

            var todas = File.ReadAllLines(caminhoEntrada)
                            .Where(l => !string.IsNullOrWhiteSpace(l))
                            .ToList();

            if (todas.Count < 2)
            {
                Console.WriteLine(" O ficheiro CSV não contém dados suficientes.");
                return;
            }

            
            char sep = ';';

            
            int IDX_TMIN = 1, IDX_TEMP = 2, IDX_HUM = 3, IDX_TMAX = 4, IDX_DATA = 5;

            
            string[] formatosData = {
                "yyyy-MM-dd HH:mm:ss",
                "yyyy-MM-dd HH:mm"
            };

            var culturaPT = new CultureInfo("pt-PT");

            
            var rows = todas.Skip(1)
                .Select(l => l.Trim().Replace("\"", "")) // remove aspas
                .Select(l => l.Split(sep))
                .Where(p => p.Length > IDX_DATA)
                .Select(p => new
                {
                    Tmin = ParseDouble(p[IDX_TMIN]),
                    Temp = ParseDouble(p[IDX_TEMP]),
                    Hum = ParseDouble(p[IDX_HUM]),
                    Tmax = ParseDouble(p[IDX_TMAX]),
                    Data = ParseData(p[IDX_DATA], formatosData, culturaPT)
                })
                .Where(r => r.Data != DateTime.MinValue)
                .ToList();

            if (rows.Count == 0)
            {
                Console.WriteLine(" Nenhuma linha válida foi lida. Verifica o separador e o formato da data.");
                Console.WriteLine($" Primeira linha lida (debug): {todas[1]}");
                return;
            }

            
            var porDia = rows
                .GroupBy(r => r.Data.Date)
                .OrderBy(g => g.Key);

            
            var xml = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("resumosDiarios",
                    new XAttribute("fonte", Path.GetFileName(caminhoEntrada)),
                    porDia.Select(g => new XElement("dia",
                        new XAttribute("data", g.Key.ToString("yyyy-MM-dd")),
                        new XAttribute("leituras", g.Count()),
                        BlocoMetricas("temp_api", g.Select(x => x.Temp)),
                        BlocoMetricas("temp_min_api", g.Select(x => x.Tmin)),
                        BlocoMetricas("temp_max_api", g.Select(x => x.Tmax)),
                        BlocoMetricas("hum_api", g.Select(x => x.Hum))
                    ))
                )
            );

            xml.Save(caminhoSaidaXml);
            Console.WriteLine($" XML diário gerado com sucesso!\n📁 Caminho: {caminhoSaidaXml}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Ocorreu um erro: {ex.Message}");
        }
    }

   
    static double ParseDouble(string valor)
    {
        valor = valor.Trim().Replace("\"", "");
        if (double.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            return result;
        return double.NaN;
    }

    static DateTime ParseData(string s, string[] formatos, CultureInfo cultura)
    {
        s = s.Trim().Replace("\"", "");
        if (DateTime.TryParseExact(s, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            return dt;
        return DateTime.MinValue;
    }

    static XElement BlocoMetricas(string nome, IEnumerable<double> valores)
    {
        var v = valores.Where(x => !double.IsNaN(x)).ToList();
        if (v.Count == 0)
            return new XElement(nome, new XAttribute("erro", "sem dados"));

        double media = v.Average();
        double min = v.Min();
        double max = v.Max();

        return new XElement(nome,
            new XElement("media", media.ToString("F2", CultureInfo.InvariantCulture)),
            new XElement("minimo", min.ToString("F2", CultureInfo.InvariantCulture)),
            new XElement("maximo", max.ToString("F2", CultureInfo.InvariantCulture)),
            new XElement("amostras", v.Count)
        );
    }
}
