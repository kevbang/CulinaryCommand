namespace CulinaryCommand.Models
{
    public class LocationModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

        public int CompanyId { get; set; }
        public string MarginEdgeKey { get; set; } = string.Empty;
    }
}