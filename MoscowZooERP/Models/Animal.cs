using ZooERP.Interfaces;

namespace ZooERP.Models
{
    /// <summary>
    /// Базовый класс для всех животных. Реализует интерфейсы IAlive и IInventory.
    /// Содержит общие для всех животных свойства, такие как потребление пищи и инвентаризационный номер.
    /// </summary>
    public abstract class Animal : IAlive, IInventory
    {
        /// <summary>
        /// Количество пищи (в килограммах), которое животное потребляет ежедневно.
        /// </summary>
        public int Food { get; set; }

        /// <summary>
        /// Инвентаризационный номер животного.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Свойство, указывающее на здоровье животного.
        /// </summary>
        public bool IsHealthy { get; set; }

        /// <summary>
        /// Имя животного.
        /// </summary>
        public string Name { get; set; }
    }
}