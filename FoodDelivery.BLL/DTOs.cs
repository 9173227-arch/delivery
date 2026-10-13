using System;
using System.Collections.Generic;

namespace FoodDelivery.BLL.DTOs
{
    public class DishDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }

    public class DailyMenuDto
    {
        public int Id { get; set; }
        public string DayOfWeek { get; set; } = string.Empty;
        public List<DishDto> Dishes { get; set; } = new();
    }

    public class ComboMealDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public List<DishDto> Dishes { get; set; } = new();
    }

    public class CreateOrderItemDto
    {
        public int? DishId { get; set; }
        public int? ComboMealId { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateOrderDto
    {
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }

    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<string> ItemNames { get; set; } = new();
    }
}