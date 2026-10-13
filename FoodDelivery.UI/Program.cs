using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FoodDelivery.DAL.Context;
using FoodDelivery.DAL.Repositories;
using FoodDelivery.BLL.Services;
using FoodDelivery.BLL.DTOs;
using FoodDelivery.UI;

class Program
{
    static async Task Main(string[] args)
    {
        var options = new DbContextOptionsBuilder<FoodDeliveryContext>()
            .UseInMemoryDatabase(databaseName: "FoodDeliveryDb")
            .Options;

        using var context = new FoodDeliveryContext(options);
        DbInitializer.Seed(context);

        using var unitOfWork = new UnitOfWork(context);
        var menuService = new MenuService(unitOfWork);
        var orderService = new OrderService(unitOfWork);

        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("======================================");
            Console.WriteLine("     СЕРВІС ДОСТАВКИ ЇЖІ (ВАpoint 7) ");
            Console.WriteLine("======================================");
            Console.WriteLine("1. Переглянути меню на день тижня");
            Console.WriteLine("2. Переглянути комплексні обіди");
            Console.WriteLine("3. Створити нове замовлення");
            Console.WriteLine("4. Переглянути історію замовлень");
            Console.WriteLine("0. Вихід");
            Console.Write("\nОберіть пункт: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ShowMenuByDay(menuService);
                    break;
                case "2":
                    await ShowComboMeals(menuService);
                    break;
                case "3":
                    await CreateOrder(menuService, orderService);
                    break;
                case "4":
                    await ShowOrders(orderService);
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Невірний вибір! Натисніть Enter...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private static async Task ShowMenuByDay(MenuService menuService)
    {
        Console.Clear();
        Console.WriteLine("Оберіть день (1 - Понеділок, 2 - Вівторок): ");
        var dayInput = Console.ReadLine();
        DayOfWeek day = dayInput == "2" ? DayOfWeek.Tuesday : DayOfWeek.Monday;

        var menu = await menuService.GetMenuByDayAsync(day);
        if (menu == null)
        {
            Console.WriteLine("Меню на цей день відсутнє.");
        }
        else
        {
            Console.WriteLine($"\n--- МЕНЮ НА {menu.DayOfWeek.ToUpper()} ---");
            foreach (var dish in menu.Dishes)
            {
                Console.WriteLine($"[{dish.Id}] {dish.Name} ({dish.CategoryName}) - {dish.Price} грн");
            }
        }
        Console.WriteLine("\nНатисніть Enter для повернення...");
        Console.ReadLine();
    }

    private static async Task ShowComboMeals(MenuService menuService)
    {
        Console.Clear();
        Console.WriteLine("--- ДОСТУПНІ КОМПЛЕКСНІ ОБІДИ ---");
        var combos = await menuService.GetComboMealsAsync();
        foreach (var combo in combos)
        {
            Console.WriteLine($"\n[Комплекс №{combo.Id}] {combo.Name} - Ціна: {combo.Price} грн");
            Console.WriteLine("Склад:");
            foreach (var dish in combo.Dishes)
            {
                Console.WriteLine($"  - {dish.Name}");
            }
        }
        Console.WriteLine("\nНатисніть Enter для повернення...");
        Console.ReadLine();
    }

    private static async Task CreateOrder(MenuService menuService, OrderService orderService)
    {
        Console.Clear();
        Console.WriteLine("--- ОФОРМЛЕННЯ ЗАМОВЛЕННЯ ---");
        var createOrderDto = new CreateOrderDto();

        Console.WriteLine("Бажаєте замовити комплексний обід? (1 - Так, 0 - Ні): ");
        if (Console.ReadLine() == "1")
        {
            Console.Write("Введіть ID комплексу (наприклад, 1): ");
            if (int.TryParse(Console.ReadLine(), out int comboId))
            {
                createOrderDto.Items.Add(new CreateOrderItemDto { ComboMealId = comboId, Quantity = 1 });
            }
        }

        Console.WriteLine("Бажаєте додати окрему страву? (1 - Так, 0 - Ні): ");
        if (Console.ReadLine() == "1")
        {
            Console.Write("Введіть ID страви (наприклад, 1): ");
            if (int.TryParse(Console.ReadLine(), out int dishId))
            {
                createOrderDto.Items.Add(new CreateOrderItemDto { DishId = dishId, Quantity = 1 });
            }
        }

        if (createOrderDto.Items.Any())
        {
            var resultOrder = await orderService.CreateOrderAsync(createOrderDto);
            Console.WriteLine($"\nЗамовлення успішно створено! ID: {resultOrder.Id}");
            Console.WriteLine($"Загальна сума до сплати: {resultOrder.TotalAmount} грн");
        }
        else
        {
            Console.WriteLine("\nЗамовлення порожнє.");
        }

        Console.WriteLine("\nНатисніть Enter для повернення...");
        Console.ReadLine();
    }

    private static async Task ShowOrders(OrderService orderService)
    {
        Console.Clear();
        Console.WriteLine("--- ІСТОРІЯ ЗАМОВЛЕНЬ ---");
        var orders = await orderService.GetAllOrdersAsync();
        foreach (var order in orders)
        {
            Console.WriteLine($"\nЗамовлення №{order.Id} від {order.OrderDate:g} | Сума: {order.TotalAmount} грн");
            Console.WriteLine("Позиції:");
            foreach (var item in order.ItemNames)
            {
                Console.WriteLine($"  - {item}");
            }
        }
        Console.WriteLine("\nНатисніть Enter для повернення...");
        Console.ReadLine();
    }
}