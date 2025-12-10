
using SURIMI_value_chain.EwE;
using SURIMI_value_chain.Services;

namespace SURIMI_value_chain
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Add services to the container.
            builder.Services.AddGrpc(options =>
            {
                options.Interceptors.Add<ExceptionMetadataInterceptor>();
            });

            //builder.Services.AddSingleton<CheckSimulationService>();
            //builder.Services.AddSingleton<IEwECore, EwE.Wrapper.EwECore>();
            //builder.Services.AddSingleton<IEwEConfiguration, EwEConfiguration>();
            builder.Services.AddSingleton<IEwEController, EwEController>();

            builder.Logging.ClearProviders();
            builder.Services.AddLogging(opt =>
            {
                opt.AddSimpleConsole(c =>
                {
                    c.TimestampFormat = "[HH:mm:ss] ";
                });
            });

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            app.MapGrpcService<ValueChainWorkflowService>();
            app.MapGrpcService<ValueChainMarketService>();

            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}
