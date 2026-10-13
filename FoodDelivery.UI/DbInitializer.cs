using System;
using System.Linq;
using FoodDelivery.DAL.Context;
using FoodDelivery.DAL.Entities;

namespace FoodDelivery.UI
{
    public static class DbInitializer
    {
        public static void Seed(FoodDeliveryContext context)
        {
            if (context.Dishes.Any()) return; // Якщо дані вже є, повторно не додаємо

            // 1. Категорії
            var catFirst = new DishCategory { Name = "Перші страви" };
            var catSecond = new DishCategory { Name = "Другі страви" };
            var catDrink = new DishCategory { Name = "Напої" };

            context.DishCategories.AddRange(catFirst, catSecond, catDrink);

            // 2. Страви
            var borscht = new Dish { Name = "Борщ український", Price = 120, Category = catFirst };
            var soup = new Dish { Name = "Суп курячий", Price = 95, Category = catFirst };
            var steak = new Dish { Name = "Стейк зі свинини", Price = 210, Category = catSecond };
            var puree = new Dish { Name = "Картопляне пюре", Price = 60, Category = catSecond };
            var compote = new Dish { Name = "Узвар", Price = 35, Category = catDrink };

            context.Dishes.AddRange(borscht, soup, steak, puree, compote);

            // 3. Денне меню (Понеділок та Вівторок)
            var mondayMenu = new DailyMenu { Day = DayOfWeek.Monday };
            mondayMenu.MenuItems.Add(new DailyMenuItem { Dish = borscht });
            mondayMenu.MenuItems.Add(new DailyMenuItem { Dish = puree });
            mondayMenu.MenuItems.Add(new DailyMenuItem { Dish = compote });

            var tuesdayMenu = new DailyMenu { Day = DayOfWeek.Tuesday };
            tuesdayMenu.MenuItems.Add(new DailyMenuItem { Dish = soup });
            tuesdayMenu.MenuItems.Add(new DailyMenuItem { Dish = steak });
            tuesdayMenu.MenuItems.Add(new DailyMenuItem { Dish = compote }); // Страва повторюється в інший день

            context.DailyMenus.AddRange(mondayMenu, tuesdayMenu);

            // 4. Комплексний обід
            var combo = new ComboMeal { Name = "Бізнес-ланч 'Класичний'", Price = 180 };
            combo.Items.Add(new ComboMealItem { Dish = borscht });
            combo.Items.Add(new ComboMealItem { Dish = puree });
            combo.Items.Add(new ComboMealItem { Dish = compote });

            context.ComboMeals.Add(combo);

            context.SaveChanges();
        }
    }
}