using Grpc.Surimi;

namespace SURIMI_value_chain.ValueChain
{
    public interface IValueChainController
    {
        bool IsWaiting { get; }
        Task<int> StartAsync(int timeoutMs = 60000);
        Task<bool> ContinueAsync(int timeoutMs = 60000);
        Task<bool> StopAsync(int timeoutMs = 10000);
        Task<bool> UpdateSales();
    }
}
