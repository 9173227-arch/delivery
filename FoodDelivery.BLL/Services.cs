using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodDelivery.BLL.DTOs;
using FoodDelivery.BLL.Mapping;
using FoodDelivery.DAL.Entities;
using FoodDelivery.DAL.Interfaces;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IMenuService
    {
        Task<DailyMenuDto?> GetMenuByDayAsync(DayOfWeek day);
        Task<IEnumerable<DishDto>> GetDishesByCategoryAsync(int categoryId);
        Task<IEnumerable<ComboMealDto>> GetComboMealsAsync();
    }

    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto);
        Task<OrderDto?> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
    }
}

namespace FoodDelivery.BLL.Services
{
    using FoodDelivery.BLL.Interfaces;

    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MenuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DailyMenuDto?> GetMenuByDayAsync(DayOfWeek day)
        {
            var menus = await _unitOfWork.Repository<DailyMenu>().FindAsync(m => m.Day == day);
            var menu = menus.FirstOrDefault();
            return menu != null ? ObjectMapper.ToDto(menu) : null;
        }

        public async Task<IEnumerable<DishDto>> GetDishesByCategoryAsync(int categoryId)
        {
            var dishes = await _unitOfWork.Repository<Dish>().FindAsync(d => d.CategoryId == categoryId);
            return dishes.Select(ObjectMapper.ToDto);
        }

        public async Task<IEnumerable<ComboMealDto>> GetComboMealsAsync()
        {
            var combos = await _unitOfWork.Repository<ComboMeal>().GetAllAsync();
            return combos.Select(ObjectMapper.ToDto);
        }
    }

    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            var order = new Order
            {
                OrderDate = DateTime.Now,
                TotalAmount = 0
            };

            decimal total = 0;

            foreach (var itemDto in createOrderDto.Items)
            {
                decimal unitPrice = 0;

                if (itemDto.DishId.HasValue)
                {
                    var dish = await _unitOfWork.Repository<Dish>().GetByIdAsync(itemDto.DishId.Value);
                    if (dish != null) unitPrice = dish.Price;
                }
                else if (itemDto.ComboMealId.HasValue)
                {
                    var combo = await _unitOfWork.Repository<ComboMeal>().GetByIdAsync(itemDto.ComboMealId.Value);
                    if (combo != null) unitPrice = combo.Price;
                }

                var orderItem = new OrderItem
                {
                    DishId = itemDto.DishId,
                    ComboMealId = itemDto.ComboMealId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = unitPrice
                };

                total += unitPrice * itemDto.Quantity;
                order.OrderItems.Add(orderItem);
            }

            order.TotalAmount = total;

            await _unitOfWork.Repository<Order>().AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return ObjectMapper.ToDto(order);
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(id);
            return order != null ? ObjectMapper.ToDto(order) : null;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Repository<Order>().GetAllAsync();
            return orders.Select(ObjectMapper.ToDto);
        }
    }
}