using Contracts;
using MassTransit;
using ServiceClient;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddRequestClient<ServiceManagerRequest>(TimeSpan.FromSeconds(10));

    x.UsingGrpc((context, cfg) =>
    {
        cfg.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500) ));
        cfg.Host(h =>
        {
            h.Host = "127.0.0.1";
            h.Port = 19797;

            h.AddServer(new Uri("http://127.0.0.1:19796"));
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<IScopedProcessingService, DefaultScopedProcessingService>();

var host = builder.Build();
host.Run();
