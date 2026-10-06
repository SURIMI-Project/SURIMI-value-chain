using Grpc.Surimi;

namespace SURIMI_value_chain.EwE
{
    public interface IValueChainController
    {
        Task<int> StartAsync(int timeoutMs = 60000);
        Task<bool> ContinueAsync(int timeoutMs = 60000);
        Task<bool> StopAsync(int timeoutMs = 10000);
        Task<bool> UpdateSales();
        //Task<Biomass> GetBiomassAsync();
        Task<List<SalesSummary>> UpdateSalesSummariesAsync();
    }
}
