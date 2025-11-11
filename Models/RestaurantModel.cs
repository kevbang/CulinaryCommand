namespace CulinaryCommand.Models
{
    public class RestaurantModel
    {
        public int? Id { get; set; } = null;
        public string Name { get; set; } = string.Empty;
        public string CuisineType { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
    }
}