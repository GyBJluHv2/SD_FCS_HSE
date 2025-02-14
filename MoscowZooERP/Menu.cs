using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Media;
using ZooERP.DI;
using ZooERP.Models;
using ZooERP.Services;

namespace ZooERP
{
    internal class Menu
    {
        /// <summary>
        /// Главная точка входа в приложение.
        /// </summary>
        /// <param name="args">Аргументы командной строки.</param>
        static void Main(string[] args)
        {
            PlayIntroMusic(); // Воспроизведение вступительной музыки

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
       М О С К О В С К И Й   З О О П А Р К  
");
            Console.ResetColor();

            // Настройка DI-контейнера
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddZooServices(); // Добавление сервисов зоопарка
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var zoo = serviceProvider.GetRequiredService<Zoo>(); // Получение экземпляра Zoo

            // Добавление начальных объектов в инвентарь
            zoo.AddThing(new Table { Name = "Стол для кормления", Number = 1000 });
            zoo.AddThing(new Computer { Name = "Компьютер главного офиса", Number = 1001 });

            // Основной цикл меню
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ЗООПАРКА ===");
                Console.ResetColor();

                Console.WriteLine("1. Добавить новое животное");
                Console.WriteLine("2. Посмотреть суммарное количество еды (в кг) для всех животных");
                Console.WriteLine("3. Список животных для контактного зоопарка");
                Console.WriteLine("4. Вывести все инвентарные объекты (животные и вещи)");
                Console.WriteLine("5. Выход");
                Console.Write("Выберите пункт меню: ");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        AddNewAnimal(zoo); // Добавление нового животного
                        break;
                    case "2":
                        ShowTotalFood(zoo); // Показать суммарное количество еды
                        break;
                    case "3":
                        ShowContactZooAnimals(zoo); // Показать животных для контактного зоопарка
                        break;
                    case "4":
                        ShowAllInventoryItems(zoo); // Показать все инвентарные объекты
                        break;
                    case "5":
                        Console.WriteLine("Спасибо за использование системы ZooERP! До свидания!");
                        return; // Выход из программы
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Неверный пункт меню. Повторите ввод.");
                        Console.ResetColor();
                        break;
                }
            }
        }

        /// <summary>
        /// Воспроизводит вступительную музыку из WAV-файла.
        /// </summary>
        static void PlayIntroMusic()
        {
            try
            {
                // Путь к файлу music.wav в папке Resources
                string musicFilePath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Resources",
                    "music.wav"
                );

                using var player = new SoundPlayer(musicFilePath);
                player.Play(); // Воспроизведение музыки
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Не удалось воспроизвести музыку при запуске: " + ex.Message);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Добавляет новое животное в зоопарк.
        /// </summary>
        /// <param name="zoo">Экземпляр зоопарка.</param>
        static void AddNewAnimal(Zoo zoo)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n=== Добавление нового животного ===");
            Console.ResetColor();

            Console.Write("Введите тип животного (Monkey, Rabbit, Tiger, Wolf): ");
            var animalType = Console.ReadLine()?.Trim().ToLower();

            Animal newAnimal = null;
            switch (animalType)
            {
                case "monkey": newAnimal = new Monkey(); break;
                case "rabbit": newAnimal = new Rabbit(); break;
                case "tiger": newAnimal = new Tiger(); break;
                case "wolf": newAnimal = new Wolf(); break;
                default:
                    Console.WriteLine("Неизвестный тип животного. Операция отменена.");
                    return;
            }

            Console.Write("Введите имя животного: ");
            newAnimal.Name = Console.ReadLine();

            Console.Write("Здорово ли животное? (да/нет): ");
            var isHealthyInput = Console.ReadLine();
            newAnimal.IsHealthy = (isHealthyInput?.Trim().ToLower() == "да");

            // Ввод количества еды с проверкой
            newAnimal.Food = ReadIntegerInput("Сколько килограммов еды в сутки потребляет животное?: ", 0);

            // Ввод инвентарного номера с проверкой
            newAnimal.Number = ReadIntegerInput("Введите инвентарный номер животного: ", 0);

            // Если животное травоядное, запрашиваем уровень доброты
            if (newAnimal is Herbo herbo)
            {
                herbo.Kindness = ReadIntegerInput("Введите уровень доброты (0-10): ", 0, 10);
            }

            zoo.AddAnimal(newAnimal); // Добавление животного в зоопарк
        }

        /// <summary>
        /// Читает целое число из ввода пользователя с проверкой на корректность.
        /// </summary>
        /// <param name="prompt">Сообщение для пользователя.</param>
        /// <param name="minValue">Минимальное допустимое значение.</param>
        /// <param name="maxValue">Максимальное допустимое значение.</param>
        /// <returns>Корректное целое число, введенное пользователем.</returns>
        static int ReadIntegerInput(string prompt, int minValue = int.MinValue, int maxValue = int.MaxValue)
        {
            int result;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out result))
                {
                    if (result >= minValue && result <= maxValue)
                    {
                        return result;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Введите число от {minValue} до {maxValue}.");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неверный формат числа. Повторите ввод.");
                    Console.ResetColor();
                }
            }
        }

        /// <summary>
        /// Показывает суммарное количество еды, необходимое для всех животных в зоопарке.
        /// </summary>
        /// <param name="zoo">Экземпляр зоопарка.</param>
        static void ShowTotalFood(Zoo zoo)
        {
            var totalFood = zoo.GetTotalFoodPerDay();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Суммарное количество еды в день (кг): {totalFood}");
            Console.ResetColor();
        }

        /// <summary>
        /// Показывает список животных, подходящих для контактного зоопарка.
        /// </summary>
        /// <param name="zoo">Экземпляр зоопарка.</param>
        static void ShowContactZooAnimals(Zoo zoo)
        {
            var contactAnimals = zoo.GetContactZooAnimals();
            if (contactAnimals.Count == 0)
            {
                Console.WriteLine("Нет животных, пригодных для контактного зоопарка.");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nЖивотные для контактного зоопарка:");
            Console.ResetColor();

            foreach (var animal in contactAnimals)
            {
                Console.WriteLine($" - [{animal.Name}], доброта = {animal.Kindness}, инв.№ {animal.Number}");
            }
        }

        /// <summary>
        /// Показывает все инвентарные объекты (животные и вещи) в зоопарке.
        /// </summary>
        /// <param name="zoo">Экземпляр зоопарка.</param>
        static void ShowAllInventoryItems(Zoo zoo)
        {
            var allInventory = zoo.GetAllInventoryItems();
            if (allInventory.Count == 0)
            {
                Console.WriteLine("Нет инвентаризируемых объектов в зоопарке.");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nВсе инвентаризируемые объекты:");
            Console.ResetColor();

            foreach (var item in allInventory)
            {
                if (item is Animal animalItem)
                {
                    Console.WriteLine($"Животное: [{animalItem.Name}] - Инв.№ {animalItem.Number}");
                }
                else if (item is Thing thingItem)
                {
                    Console.WriteLine($"Вещь: [{thingItem.Name}] - Инв.№ {thingItem.Number}");
                }
            }
        }
    }
}