using System.Linq;
using FoodDelivery.BLL.DTOs;
using FoodDelivery.DAL.Entities;

namespace FoodDelivery.BLL.Mapping
{
    public static class ObjectMapper
    {
        public static DishDto ToDto(Dish dish)
        {
            return new DishDto
            {
                Id = dish.Id,
                Name = dish.Name,
                Price = dish.Price,
                CategoryName = dish.Category?.Name ?? "Без категорії"
            };
        }

        public static DailyMenuDto ToDto(DailyMenu dailyMenu)
        {
            return new DailyMenuDto
            {
                Id = dailyMenu.Id,
                DayOfWeek = dailyMenu.Day.ToString(),
                Dishes = dailyMenu.MenuItems
                    .Where(mi => mi.Dish != null)
                    .Select(mi => ToDto(mi.Dish!))
                    .ToList()
            };
        }

        public static ComboMealDto ToDto(ComboMeal comboMeal)
        {
            return new ComboMealDto
            {
                Id = comboMeal.Id,
                Name = comboMeal.Name,
                Price = comboMeal.Price,
                Dishes = comboMeal.Items
                    .Where(ci => ci.Dish != null)
                    .Select(ci => ToDto(ci.Dish!))
                    .ToList()
            };
        }

        public static OrderDto ToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                ItemNames = order.OrderItems.Select(oi => 
                    oi.Dish != null 
                        ? $"{oi.Dish.Name} x{oi.Quantity}" 
                        : (oi.ComboMeal != null ? $"{oi.ComboMeal.Name} (Комплекс) x{oi.Quantity}" : "Невідома позиція")
                ).ToList()
            };
        }
    }
}