using ZooERP.Models;

namespace ZooERP.Interfaces
{
    /// <summary>
    /// Интерфейс для ветеринарной клиники, которая проверяет здоровье животных.
    /// </summary>
    public interface IVetClinic
    {
        /// <summary>
        /// Проверяет здоровье животного. Если оно здорово, животное принимается в зоопарк.
        /// </summary>
        /// <param name="animal">Животное, которое нужно осмотреть.</param>
        /// <returns>True, если животное здорово; false — если нет.</returns>
        bool CheckHealth(Animal animal);
    }
}