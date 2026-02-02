namespace CulinaryCommandApp.AIDashboard.Services.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class AIAnalysisResultDTO
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("summary")]
        public string? Summary { get; set; }

        [JsonPropertyName("metrics")]
        public List<Metric>? Metrics { get; set; }

        [JsonPropertyName("sections")]
        public List<Section>? Sections { get; set; }
        
        [JsonPropertyName("anomalies")]
        public List<Anomaly>? Anomalies { get; set; }

        [JsonPropertyName("recommendations")]
        public List<string>? Recommendations { get; set; }

        [JsonPropertyName("generatedAt")]
        public DateTimeOffset? GeneratedAt { get; set; }

        [JsonPropertyName("confidence")]
        public double? Confidence { get; set; }
    }

    public class Metric
    {
        [JsonPropertyName("label")]
        public string? Label { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }
        
        [JsonPropertyName("unit")]
        public string? Unit { get; set; }
    }
    public class Section
    {
        [JsonPropertyName("heading")]
        public string? Heading { get; set; }

        [JsonPropertyName("body")]
        public string? Body { get; set; }
    }

    public class Anomaly
    {
        [JsonPropertyName("row")]
        public string? Row { get; set; } 

        [JsonPropertyName("reason")]
        public string? Reason { get; set; }
 
    }
}