using System;
using System.Collections.Generic;

namespace FoodDelivery.DAL.Entities
{
    public class DishCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
    }

    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public DishCategory? Category { get; set; }
    }

    public class DailyMenu
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }
        public ICollection<DailyMenuItem> MenuItems { get; set; } = new List<DailyMenuItem>();
    }

    public class DailyMenuItem
    {
        public int Id { get; set; }
        public int DailyMenuId { get; set; }
        public DailyMenu? DailyMenu { get; set; }
        public int DishId { get; set; }
        public Dish? Dish { get; set; }
    }

    public class ComboMeal
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public ICollection<ComboMealItem> Items { get; set; } = new List<ComboMealItem>();
    }

    public class ComboMealItem
    {
        public int Id { get; set; }
        public int ComboMealId { get; set; }
        public ComboMeal? ComboMeal { get; set; }
        public int DishId { get; set; }
        public Dish? Dish { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int? DishId { get; set; }
        public Dish? Dish { get; set; }
        public int? ComboMealId { get; set; }
        public ComboMeal? ComboMeal { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}