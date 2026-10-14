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
            GrpcMessageLoader.Load<InitialiseExperimentRequest>("InitialiseExperiment", "InitialiseExperimentRequest.request.yaml");

        InitialiseExperimentResponse response = await m_client.InitialiseExperimentAsync(request);

        response.ExperimentId.Should().Be(request.ExperimentId);
    }

    [Fact]
    public async Task FinaliseExperiment_ValidRequest_ReturnsMatchingExperimentId()
    {
        FinaliseExperimentRequest request =
            GrpcMessageLoader.Load<FinaliseExperimentRequest>("FinaliseExperiment", "FinaliseExperimentRequest.request.yaml");

        FinaliseExperimentResponse response = await m_client.FinaliseExperimentAsync(request);

        response.ExperimentId.Should().Be(request.ExperimentId);
    }

    [Fact]
    public async Task CancelExperiment_ValidRequest_ReturnsMatchingExperimentId()
    {
        CancelExperimentRequest request =
            GrpcMessageLoader.Load<CancelExperimentRequest>("CancelExperiment", "CancelExperimentRequest.request.yaml");

        CancelExperimentResponse response = await m_client.CancelExperimentAsync(request);

        response.ExperimentId.Should().Be(request.ExperimentId);
    }

    [Fact]
    public async Task ExperimentStep_ValidRequest_ReturnsMatchingExperimentId()
    {
        ExperimentStepRequest request =
            GrpcMessageLoader.Load<ExperimentStepRequest>("ExperimentStep", "ExperimentStep.request.yaml");

        ExperimentStepResponse response = await m_client.ExperimentStepAsync(request);

        response.ExperimentId.Should().Be(request.ExperimentId);
    }

    [Fact]
    public async Task GetProtocolVersion_ReturnsNonEmptyVersion()
    {
        GetProtocolVersionRequest request =
            GrpcMessageLoader.Load<GetProtocolVersionRequest>("GetProtocol", "GetProtocolVersionRequest.request.yaml");

        GetProtocolVersionResponse response = await m_client.GetProtocolVersionAsync(request);

        response.ProtocolVersion.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task UpdateSalesStatistics_ValidRequest_ReturnsMatchingExperimentId()
    {
        UpdateSalesStatisticsRequest request =
            GrpcMessageLoader.Load<UpdateSalesStatisticsRequest>("UpdateSalesStatistics", "UpdateSalesStatisticsRequest.request.yaml");

        UpdateSalesStatisticsResponse response = await m_client.UpdateSalesStatisticsAsync(request);

        response.ExperimentId.Should().Be(request.ExperimentId);
    }
}

