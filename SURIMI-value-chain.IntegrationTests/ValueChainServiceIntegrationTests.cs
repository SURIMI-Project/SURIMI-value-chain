using FluentAssertions;
using Grpc.Net.Client;
using Grpc.Surimi;
using Xunit;

namespace SURIMI_value_chain_IntegrationTests;

public class ValueChainServiceIntegrationTests : IClassFixture<GrpcTestFixture>
{
    private readonly ValueChainService.ValueChainServiceClient m_client;

    public ValueChainServiceIntegrationTests(GrpcTestFixture fixture)
    {
        GrpcChannel channel = fixture.CreateGrpcChannel();
        m_client = new ValueChainService.ValueChainServiceClient(channel);
    }

    [Fact]
    public async Task InitialiseExperiment_ValidRequest_ReturnsMatchingExperimentId()
    {
        InitialiseExperimentRequest request =
            GrpcMessageLoader.Load<InitialiseExperimentRequest>("InitialiseExperiment", "InitialiseExperimentRequest.json");

        InitialiseExperimentResponse response = await m_client.InitialiseExperimentAsync(request);

        response.ExperimentId.Should().Be(request.ExperimentId);
    }

    [Fact]
    public async Task FinaliseExperiment_ValidRequest_ReturnsMatchingExperimentId()
    {
        FinaliseExperimentRequest request =
            GrpcMessageLoader.Load<FinaliseExperimentRequest>("FinaliseExperiment", "FinaliseExperimentRequest.json");

        FinaliseExperimentResponse response = await m_client.FinaliseExperimentAsync(request);

        response.ExperimentId.Should().Be(request.ExperimentId);
    }

    [Fact]
    public async Task CancelExperiment_ValidRequest_ReturnsMatchingExperimentId()
    {
        CancelExperimentRequest request =
            GrpcMessageLoader.Load<CancelExperimentRequest>("CancelExperiment", "CancelExperimentRequest.json");

        CancelExperimentResponse response = await m_client.CancelExperimentAsync(request);

        response.ExperimentId.Should().Be(request.ExperimentId);
    }

    [Fact]
    public async Task ExperimentStep_ValidRequest_ReturnsMatchingExperimentId()
    {
        ExperimentStepRequest request =
            GrpcMessageLoader.Load<ExperimentStepRequest>("ExperimentStep", "ExperimentStepRequest.json");

        ExperimentStepResponse response = await m_client.ExperimentStepAsync(request);

        response.ExperimentId.Should().Be(request.ExperimentId);
    }

    [Fact]
    public async Task GetProtocolVersion_ReturnsNonEmptyVersion()
    {
        GetProtocolVersionRequest request =
            GrpcMessageLoader.Load<GetProtocolVersionRequest>("GetProtocol", "GetProtocolVersionRequest.json");

        GetProtocolVersionResponse response = await m_client.GetProtocolVersionAsync(request);

        response.ProtocolVersion.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task UpdateSalesStatistics_ValidRequest_ReturnsMatchingExperimentId()
    {
        UpdateSalesStatisticsRequest request =
            GrpcMessageLoader.Load<UpdateSalesStatisticsRequest>("UpdateSalesStatistics", "UpdateSalesStatisticsRequest.json");

        UpdateSalesStatisticsResponse response = await m_client.UpdateSalesStatisticsAsync(request);

        response.ExperimentId.Should().Be(request.ExperimentId);
    }
}
