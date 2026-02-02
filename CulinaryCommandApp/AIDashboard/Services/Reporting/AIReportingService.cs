namespace CulinaryCommandApp.AIDashboard.Services.Reporting
{
    using Google.GenAI;
    using Google.GenAI.Types;
    using System;
    using System.Threading.Tasks;
    using System.Linq;
    using System.Text;
    using System.Globalization;
    using System.Text.Json.Serialization;
    using System.Text.Json;
    using CulinaryCommandApp.AIDashboard.Services.DTOs;

    public static class AIReportingService
    {
        public static async Task<string> AnalyzeCsvAsync(string? csvPath = null)
        {
            const string PayloadSchema = @" You are an expert restaurant data analyst.
                                            Analyze the CSV I give you and return ONLY a JSON object
                                            that exactly follows this SCHEMA:
                                            {
                                                ""title"": string,
                                                ""summary"": string,
                                                ""metrics"": [ { ""label"": string, ""value"": string, ""unit"": string|null } ],
                                                ""sections"": [ { ""heading"": string, ""body"": string } ],
                                                ""anomalies"": [ { ""row"": string, ""reason"": string } ],
                                                ""recommendations"": [ string ],
                                                ""generatedAt"": ""ISO-8601 datetime string"",
                                                ""confidence"": ""number"" (0.0 to 1.0)
                                            }
                                            Constraints:
                                            - Return only JSON (no surrounding text, no markdown fences).
                                            - For long text blocks, try to be concise in the ""body"" fields.
                                            - Use ISO-8601 for generatedAt.
                                            - Provide a numeric confidence between 0 and 1.

                                            Analysis requirements:
                                            - Use business/user friendly tone.
                                            - If there are anomalies, prompt the user to investigate.
                                            - Include top seller focus analysis.
                                            - Provide quick action items if appropriate.
                                            Now analyze the CSV content below and return the JSON.
                                            ";


            var client = new Client();

            if (!System.IO.File.Exists(csvPath))
            {
                Console.WriteLine($"CSV not found at: {csvPath}");
                return $"CSV not found at: {csvPath}";
            }

            var lines = System.IO.File.ReadAllLines(csvPath).ToList();

            if (lines.Count <= 1)
            {
                Console.WriteLine("CSV empty or only header.");
                return "CSV empty or only header.";
            }

            var header = lines[0];
            var rows = lines.Skip(1).ToList();

            /****DOING STATIC ANALYSIS TO BE DISPLAYED ON DASHBOARD****/
            // Extract price values from CSV file.
            var priceValues = rows
                .Select(r => r.Split(","))
                .Where(cols => cols.Length >=3 && decimal.TryParse(cols[2], NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                .Select(cols => decimal.Parse(cols[2], NumberStyles.Any, CultureInfo.InvariantCulture))
                .ToList();

            decimal? minPrice = priceValues.Any() ? priceValues.Min() : (decimal?)null;
            decimal? maxPrice = priceValues.Any() ? priceValues.Max() : (decimal?)null;
            double? averagePrice = priceValues.Any() ? (double?)priceValues.Average() : null;

            // Try to look for any "anomalies/outliers". AI Generated:
            var anomalies = rows.Where(row =>
            {
                var cols = row.Split(",");
                if (cols.Length < 3) return true;
                if (!DateTime.TryParse(cols[0], out var d)) return true;
                if (!decimal.TryParse(cols[2], NumberStyles.Any, CultureInfo.InvariantCulture, out var p)) return true;
                if (p > 100m) return true;
                if (d.Year < 2000 || d.Year > DateTime.UtcNow.Year + 1) return true;
                return false;
            }).ToList();

            /**** Build payload to send to Gemini model****/
            var geminiPayload = new StringBuilder();

            geminiPayload.AppendLine(PayloadSchema);
            geminiPayload.AppendLine(header);
            foreach (var r in rows) geminiPayload.AppendLine(r);

            Console.WriteLine(geminiPayload);

            /***** Make API call to Gemini with payload ******/
            var response = await client.Models.GenerateContentAsync(
                model: "gemini-3-flash-preview",
                contents: geminiPayload.ToString()
            );

            Console.WriteLine(response.Candidates[0].Content.Parts[0].Text);

            var AIAnalysisResponse = response?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;

            /*** Deserialize ***/

            if (!string.IsNullOrWhiteSpace(AIAnalysisResponse))
            {
                try
                {
                    var options = new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };

                    var result = System.Text.Json.JsonSerializer.Deserialize<AIAnalysisResultDTO>(AIAnalysisResponse, options);

                    if (result != null)
                    {
                        return System.Text.Json.JsonSerializer.Serialize(result, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                    }
                }
                catch (JsonException)
                {
                    // just catch the exception and move on
                }
            }

            return AIAnalysisResponse ?? "(no analysis returned)";
        }

    }
}