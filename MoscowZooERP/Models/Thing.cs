using ZooERP.Interfaces;

namespace ZooERP.Models
{
    /// <summary>
    /// Базовый класс для объектов, которые подлежат инвентаризации.
    /// Реализует интерфейс IInventory.
    /// </summary>
    public abstract class Thing : IInventory
    {
        /// <summary>
        /// Инвентаризационный номер вещи.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Имя вещи.
        /// </summary>
        public string Name { get; set; }
    }
}