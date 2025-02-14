namespace ZooERP.Interfaces
{
    /// <summary>
    /// Интерфейс для объектов, которые подлежат инвентаризации (например, вещи или животные).
    /// Включает свойство для учёта инвентаризационного номера.
    /// </summary>
    public interface IInventory
    {
        /// <summary>
        /// Уникальный инвентаризационный номер объекта.
        /// </summary>
        int Number { get; set; }
    }
}