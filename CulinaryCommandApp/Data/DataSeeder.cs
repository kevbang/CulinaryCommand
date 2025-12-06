
// public static class DataSeeder
// {
//     public static List<Location> GetLocations()
//     {
//         var locations = new List<Location>
//         {
//             new Location
//             {
//                 ID = 1,
//                 Name = "Culinary Command Downtown",
//                 Address = "123 Main St",
//                 City = "Ames",
//                 State = "IA",
//                 ZipCode = "50010",
//             },
//             new Location
//             {
//                 ID = 2,
//                 Name = "Culinary Command North",
//                 Address = "456 Oak Ave",
//                 City = "Des Moines",
//                 State = "IA",
//                 ZipCode = "50309",
//             }
//         };

//         // assign users after creating them so references match
//         var users = GetUsers();
//         locations[0].Users = users.Where(u => u.LocationId == 1).ToList();
//         locations[1].Users = users.Where(u => u.LocationId == 2).ToList();

//         return locations;
//     }

//     public static List<User> GetUsers()
//     {
//         return new List<User>
//         {
//             new User
//             {
//                 Id = 1,
//                 Name = "Wyatt Hunter",
//                 Phone = "5151234567",
//                 Email = "wyatt@example.com",
//                 Role = Roles.Manager,
//                 StationsWorked = new List<string> { Station.Grill, Station.Expo },
//                 LocationId = 1
//             },
//             new User
//             {
//                 Id = 2,
//                 Name = "Matayas Lopez",
//                 Phone = "5157654321",
//                 Email = "matayas@example.com",
//                 Role = Roles.Employee,
//                 StationsWorked = new List<string> { Station.Saute, Station.Fry },
//                 LocationId = 1
//             },
//             new User
//             {
//                 Id = 3,
//                 Name = "Ryan Lee",
//                 Phone = "6415559988",
//                 Email = "ryan@example.com",
//                 Role = Roles.Employee,
//                 StationsWorked = new List<string> { Station.DishWasher },
//                 LocationId = 2
//             }
//         };
//     }

//     public static List<Task> GetTasks()
//     {
//         return new List<Task>
//         {
//             new Task
//             {
//                 Id = 1,
//                 Name = "Clean Grill Area",
//                 Station = Station.Grill,
//                 Status = TaskStatus.Pending,
//                 Assigner = "Wyatt Hunter",
//                 Date = DateTime.Today,
//                 UserId = 2, // Matayas
//                 CreatedAt = DateTime.UtcNow,
//                 UpdatedAt = DateTime.UtcNow
//             },
//             new Task
//             {
//                 Id = 2,
//                 Name = "Prep Salad Ingredients",
//                 Station = Station.Salads,
//                 Status = TaskStatus.InProgress,
//                 Assigner = "Wyatt Hunter",
//                 Date = DateTime.Today.AddDays(-1),
//                 UserId = 3, // Ryan
//                 CreatedAt = DateTime.UtcNow,
//                 UpdatedAt = DateTime.UtcNow
//             },
//             new Task
//             {
//                 Id = 3,
//                 Name = "Run Dishwasher",
//                 Station = Station.DishWasher,
//                 Status = TaskStatus.Completed,
//                 Assigner = "Ryan Lee",
//                 Date = DateTime.Today,
//                 UserId = 3,
//                 CreatedAt = DateTime.UtcNow,
//                 UpdatedAt = DateTime.UtcNow
//             }
//         };
//     }
// }
