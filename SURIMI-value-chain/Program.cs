
using Eii.BlobStore;
using Eii.BlobStore.S3;
using SURIMI.Common.gRPC;
using SURIMI.Common.gRPC.Services;
using SURIMI.Common.Services;
using SURIMI_value_chain.EwE;
using SURIMI_value_chain.Services;

namespace SURIMI_value_chain
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<IBlobStore>(sp =>
            {
                // if AWS_ACCESS_KEY_ID is set, use MinIO
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID")))
                {
                    return new S3BlobStore(
                        Environment.GetEnvironmentVariable("AWS_S3_ENDPOINT"),
                        Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"),
                        Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY"),
                        Environment.GetEnvironmentVariable("AWS_BUCKET_NAME"), inputBasePrefix: "value_chain", outputBasePrefix: "value_chain", localInputRoot: "Includes", localOutputRoot: "Output");
                }

                // Default local Filesystem
                return new LocalBlobStore(inputRoot: "Includes", outputRoot: "Output");
            });

            builder.AddServiceDefaults();

            // Add services to the container.

            // Add services to the container.
            builder.Services.AddGrpc(options =>
            {
                options.Interceptors.Add<VersionMetadataInterceptor>();
                options.Interceptors.Add<ExceptionMetadataInterceptor>();
            });
            builder.Services.AddGrpcReflection();

            builder.Services.AddGrpcReflection();
            builder.Services.AddSingleton<IValueChainController, ValueChainController>();
            builder.Services.AddSingleton<ProtocolVersionService>();

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
            app.MapGrpcService<ValueChainService>();
            app.MapGrpcReflectionService();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            // Retrieve the logger
            var logger = app.Services.GetRequiredService<ILogger<Program>>();

            app.Services.GetRequiredService<VaultService>().LoadVaultSecretsInEnvironmentVariables();

            logger.LogInformation("============================= Logging All Environment Variables =============================");
            foreach (System.Collections.DictionaryEntry envVar in Environment.GetEnvironmentVariables())
            {
                logger.LogInformation("{Key}: {Value}", envVar.Key, envVar.Value);
            }
            logger.LogInformation("============================= End of Environment Variables =============================");

            logger.LogInformation("Value-chain running.......................");
            app.Run();
            logger.LogInformation("Value-chain shutting down.......................");
        }
    }
}
