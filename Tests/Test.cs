using Xunit;
using Moq;
using ZooERP.Interfaces;
using ZooERP.Models;
using ZooERP.Services;
using System.Linq;

namespace ZooERPTests
{
    /// <summary>
    /// Набор юнит-тестов для класса Zoo.
    /// Демонстрирует тестирование приёмки животных, учёта еды и фильтрации контактных животных.
    /// </summary>
    public class ZooTests
    {
        /// <summary>
        /// Тест: здоровое животное должно быть добавлено в зоопарк (при условии, 
        /// что ветеринарная клиника возвращает true).
        /// </summary>
        [Fact]
        public void AddAnimal_HealthyAnimal_ShouldBeAccepted()
        {
            // Arrange: создаём mock для IVetClinic, всегда возвращающий true
            var vetClinicMock = new Mock<IVetClinic>();
            vetClinicMock
                .Setup(clinic => clinic.CheckHealth(It.IsAny<Animal>()))
                .Returns(true);

            var zoo = new Zoo(vetClinicMock.Object);

            var monkey = new Monkey
            {
                Name = "TestMonkey",
                IsHealthy = true,
                Food = 2,
                Number = 1,
                Kindness = 7
            };

            // Act
            zoo.AddAnimal(monkey);

            // Assert
            // Проверяем, что животное действительно оказалось в зоопарке.
            // Т.к. это травоядное с Kindness=7 (>5), оно и в списке контактных.
            var animalsForContactZoo = zoo.GetContactZooAnimals();
            Assert.Contains(monkey, animalsForContactZoo);
        }

        /// <summary>
        /// Тест: больное животное не должно быть добавлено в зоопарк 
        /// (ветклиника возвращает false).
        /// </summary>
        [Fact]
        public void AddAnimal_SickAnimal_ShouldBeRejected()
        {
            // Arrange: клиника возвращает false, т.е. животное больное
            var vetClinicMock = new Mock<IVetClinic>();
            vetClinicMock
                .Setup(clinic => clinic.CheckHealth(It.IsAny<Animal>()))
                .Returns(false);

            var zoo = new Zoo(vetClinicMock.Object);

            var tiger = new Tiger
            {
                Name = "SickTiger",
                IsHealthy = false,  // Больное животное
                Food = 5,
                Number = 2
            };

            // Act
            zoo.AddAnimal(tiger);

            // Assert
            // Убеждаемся, что тигра нет среди инвентаря (животных),
            // так как он не прошёл проверку здоровья и не был добавлен
            var allInventory = zoo.GetAllInventoryItems();
            Assert.DoesNotContain(tiger, allInventory.Cast<Animal>());
        }

        /// <summary>
        /// Тест: Проверяем корректность подсчёта общего количества еды для всех животных.
        /// </summary>
        [Fact]
        public void GetTotalFoodPerDay_ShouldSumCorrectly()
        {
            // Arrange
            var vetClinicMock = new Mock<IVetClinic>();
            // Допустим, все животные здоровые
            vetClinicMock
                .Setup(clinic => clinic.CheckHealth(It.IsAny<Animal>()))
                .Returns(true);

            var zoo = new Zoo(vetClinicMock.Object);

            zoo.AddAnimal(new Monkey { Name = "Monkey1", Food = 2, IsHealthy = true });
            zoo.AddAnimal(new Rabbit { Name = "Rabbit1", Food = 1, IsHealthy = true });
            zoo.AddAnimal(new Tiger { Name = "Tiger1", Food = 5, IsHealthy = true });

            // Act
            var totalFood = zoo.GetTotalFoodPerDay();

            // Assert
            // 2 + 1 + 5 = 8
            Assert.Equal(8, totalFood);
        }

        /// <summary>
        /// Тест: Проверяем, что в список контактных животных (GetContactZooAnimals)
        /// попадают только травоядные (Herbo) с Kindness > 5.
        /// </summary>
        [Fact]
        public void GetContactZooAnimals_OnlyHerboWithKindnessMoreThan5()
        {
            // Arrange
            var vetClinicMock = new Mock<IVetClinic>();
            vetClinicMock
                .Setup(clinic => clinic.CheckHealth(It.IsAny<Animal>()))
                .Returns(true);

            var zoo = new Zoo(vetClinicMock.Object);

            var goodRabbit = new Rabbit
            {
                Name = "GoodRabbit",
                Kindness = 8,   // >5
                IsHealthy = true
            };
            var averageRabbit = new Rabbit
            {
                Name = "AverageRabbit",
                Kindness = 5,   // Ровно 5 (не проходит)
                IsHealthy = true
            };
            var monkey = new Monkey
            {
                Name = "FunnyMonkey",
                Kindness = 9,   // >5
                IsHealthy = true
            };
            var tiger = new Tiger
            {
                Name = "AngryTiger",
                IsHealthy = true
                // Kindness здесь нет, т.к. Tiger -> Predator, не Herbo
            };

            // Добавляем всех в зоопарк
            zoo.AddAnimal(goodRabbit);
            zoo.AddAnimal(averageRabbit);
            zoo.AddAnimal(monkey);
            zoo.AddAnimal(tiger);

            // Act
            var contactAnimals = zoo.GetContactZooAnimals(); // List<Herbo>

            // Assert
            // goodRabbit и monkey (оба Herbo, Kindness>5) должны быть в списке
            Assert.Contains(goodRabbit, contactAnimals);
            Assert.Contains(monkey, contactAnimals);

            // averageRabbit (Kindness=5) не должен попасть
            Assert.DoesNotContain(averageRabbit, contactAnimals);

            // Tiger – хищник, не Herbo -> не должен быть в списке контактных
            // Из-за несоответствия типов Tiger vs Herbo используем перегрузку с предикатом:
            Assert.DoesNotContain(contactAnimals, x => x.Name == tiger.Name);
        }
    }
}