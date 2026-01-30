
using SURIMI_value_chain.ValueChain;
using SURIMI_value_chain.Services;

namespace SURIMI_value_chain
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddServiceDefaults();

            // Add services to the container.

            // Add services to the container.
            builder.Services.AddGrpc(options =>
            {
                options.Interceptors.Add<ExceptionMetadataInterceptor>();
            });

            //builder.Services.AddSingleton<CheckSimulationService>();
            builder.Services.AddSingleton<IValueChainController, ValueChainController>();

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
            // ToDo: value chain just needs to collect all sales summaries from SURIMI models; nothing else
            //app.MapGrpcService<ValueChainMarketService>();

            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}
