using Microsoft.EntityFrameworkCore;
using FoodDelivery.DAL.Entities;

namespace FoodDelivery.DAL.Context
{
    public class FoodDeliveryContext : DbContext
    {
        public FoodDeliveryContext(DbContextOptions<FoodDeliveryContext> options) : base(options) { }

        public DbSet<DishCategory> DishCategories => Set<DishCategory>();
        public DbSet<Dish> Dishes => Set<Dish>();
        public DbSet<DailyMenu> DailyMenus => Set<DailyMenu>();
        public DbSet<DailyMenuItem> DailyMenuItems => Set<DailyMenuItem>();
        public DbSet<ComboMeal> ComboMeals => Set<ComboMeal>();
        public DbSet<ComboMealItem> ComboMealItems => Set<ComboMealItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    }
}