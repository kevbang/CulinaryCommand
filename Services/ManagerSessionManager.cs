using CulinaryCommand.Models;

//possibly use this component to manage state for a logged in owner/manager
// this can be refactored to however the backend needs it -ryan
namespace CulinaryCommand.Services
{
    public class ManagerSessionManager
    {
        // properties for a logged-in manager
        public string? ManagerId { get; set; }
        public string? ManagerName { get; set; }

        //replace in the future with a call to the database to get associated restaurants
        public List<RestaurantModel> Restaurants = new()
        {
            new RestaurantModel
            {
                Id = 1,
                Name = "Downtown Bistro",
                CuisineType = "Modern American",
                Address = "123 Main Street, Downtown, IA 50010",
                Phone = "(515) 555-1234",
                Email = "info@downtownbistro.com",
                Description = "A modern eatery offering farm-to-table dishes and craft cocktails in a cozy downtown setting.",
                Website = "https://www.downtownbistro.com"
            },
            new RestaurantModel
            {
                Id = 2,
                Name = "Uptown Cafe",
                CuisineType = "Breakfast & Brunch",
                Address = "456 Elm Street, Uptown, IA 50011",
                Phone = "(515) 555-4567",
                Email = "contact@uptowncafe.com",
                Description = "Popular brunch spot known for fresh pastries, hearty breakfast plates, and artisan coffee.",
                Website = "https://www.uptowncafe.com"
            },
            new RestaurantModel
            {
                Id = 3,
                Name = "Light Deli",
                CuisineType = "Deli & Sandwiches",
                Address = "789 Oak Avenue, Midtown, IA 50012",
                Phone = "(515) 555-7890",
                Email = "hello@lightdeli.com",
                Description = "A bright and casual deli serving gourmet sandwiches, soups, and salads with local ingredients.",
                Website = "https://www.lightdeli.com"
            }
        };

        // clear session data (on logout)
        public void Clear()
        {
            ManagerId = null;
            ManagerName = null;
            Restaurants = new List<RestaurantModel>(); //empty list
        }
    }
}

