using Microsoft.Extensions.DependencyInjection;
using ZooERP.Interfaces;
using ZooERP.Services;

namespace ZooERP.DI
{
    /// <summary>
    /// Класс для регистрации сервисов в DI контейнере.
    /// Добавляет необходимые сервисы (ветеринарную клинику и зоопарк) в контейнер.
    /// </summary>
    public static class ServiceRegistrations
    {
        /// <summary>
        /// Метод для добавления сервисов в контейнер.
        /// </summary>
        public static IServiceCollection AddZooServices(this IServiceCollection services)
        {
            services.AddSingleton<IVetClinic, VetClinic>();
            services.AddSingleton<Zoo>();
            return services;
        }
    }
}