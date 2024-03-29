﻿using Microsoft.Extensions.DependencyInjection;

namespace Craft.Components.Notifications;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNotifications(this IServiceCollection services)
    {
        return services.AddScoped<INotificationService, NotificationService>();
    }
}
