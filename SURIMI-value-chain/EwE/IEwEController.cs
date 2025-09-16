using Grpc.Surimi;

namespace SURIMI_value_chain.EwE
{
    public interface IEwEController
    {
        EwEController.RunStates RunState { get; }
        bool IsWaiting { get; }
        Task<int> StartAsync(int timeoutMs = 60000);
        Task<bool> ContinueAsync(int timeoutMs = 60000);
        Task<bool> StopAsync(int timeoutMs = 10000);
        Task<bool> UpdateSales();
        //Task<Biomass> GetBiomassAsync();
        Task<List<SalesSummary>> UpdateSalesSummariesAsync();
        Task<CatchDispositionSummary> GetCatchDispositionSummaryAsync(DateTime start, DateTime end);
    }
}
