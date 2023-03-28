using Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace MauiAppTest
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.AddRequestClient<ServiceManagerRequest>(TimeSpan.FromSeconds(10));

                x.UsingGrpc((context, cfg) =>
                {
                    cfg.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500)));
                    cfg.Host(h =>
                    {
                        h.Host = "127.0.0.1";
                        h.Port = 19797;

                        h.AddServer(new Uri("http://127.0.0.1:19796"));
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            builder.Services.AddSingleton<MainPage>();

            return builder.Build();
        }
    }
}