using SURIMI.Datamodel;
namespace SURIMI_value_chain.EwE
{
    public interface IEwEController
    {
        EwEController.RunStates RunState { get; }
        bool IsWaiting { get; }
        Task<int> StartAsync(int timeoutMs = 60000);
        Task<bool> ContinueAsync(int timeoutMs = 60000);
        Task<bool> StopAsync(int timeoutMs = 10000);
        Task<bool> UpdateSalesStatisticsAsync();
        Task<ValueChainSummary> GetValueChainAsync(string experimentId);
    }
}
