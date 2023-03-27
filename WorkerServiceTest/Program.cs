using MassTransit;
using WorkerServiceTest;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options =>
    {
        options.ServiceName = ".NET Sample Service";
    });
builder.Services.AddMassTransit(x =>
    {
        x.SetKebabCaseEndpointNameFormatter();

        x.AddConsumer<ServiceManagerConsumer>();

        x.UsingGrpc((context, cfg) =>
        {
            cfg.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500) ));
            cfg.Host(h =>
            {
                h.Host = "127.0.0.1";
                h.Port = 19796;
            });

            cfg.ConfigureEndpoints(context);
        });
    });

builder.Services.AddHostedService<Worker>();

var host  = builder.Build();
host.Run();
