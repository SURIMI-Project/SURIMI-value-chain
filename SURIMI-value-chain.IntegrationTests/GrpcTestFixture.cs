using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SURIMI_value_chain_IntegrationTests;

public class GrpcTestFixture : WebApplicationFactory<SURIMI_value_chain.Program>
{
    public GrpcChannel CreateGrpcChannel()
    {
        HttpClient httpClient = CreateClient();
        return GrpcChannel.ForAddress(httpClient.BaseAddress!, new GrpcChannelOptions
        {
            HttpClient = httpClient
        });
    }
}
