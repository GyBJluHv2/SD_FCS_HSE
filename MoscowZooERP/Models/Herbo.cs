namespace ZooERP.Models
{
    /// <summary>
    /// Абстрактный класс для травоядных животных. Наследует от Animal.
    /// Добавляет свойство "Kindness" для определения доброты животного.
    /// </summary>
    public abstract class Herbo : Animal
    {
        /// <summary>
        /// Уровень доброты животного. 0 - наименьший, 10 - наибольший.
        /// Животные с добротой выше 5 могут быть в контактном зоопарке.
        /// </summary>
        public int Kindness { get; set; }
    }
}