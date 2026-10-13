using Grpc.Surimi;
using ValueChain;

namespace SURIMI_value_chain.EwE
{
    public class ValueChainController : IValueChainController
    {
        private readonly ILogger<ValueChainController> _logger;
        private readonly cValueChainData _valueChainData;
        private readonly cValueChain _valueChain;
        private readonly cValueChainResults _valueChainResults;

        public ValueChainController(ILogger<ValueChainController> logger)
        {
            _logger = logger;

            _valueChainData = new cValueChainData();
            _valueChain = new cValueChain(_valueChainData);
            _valueChainResults = new cValueChainResults(_valueChainData);

            // Todo:
            // - Load value chain sqlite data for scenario
            // - Load semantics registry JSON for scenario
            // - Wait for final data, feed the chain to mapped entities
            // - Run value chain when final data has arrived
            // - Convert output and spit it out
        }

        public Task<bool> ContinueAsync(int timeoutMs = 60000)
        {
            // This method can go
            return Task.FromResult(true);
        }

        public Task<CatchDispositionSummary> GetCatchDispositionSummaryAsync(DateTime start, DateTime end)
        {
            // Rik, let's discuss
            throw new NotImplementedException();
        }

        public Task<List<SalesSummary>> UpdateSalesSummariesAsync()
        {
            // Rik, let's discuss
            throw new NotImplementedException();
        }

        public Task<int> StartAsync(int timeoutMs = 60000)
        {
            return Task.FromResult(1);
        }

        public Task<bool> StopAsync(int timeoutMs = 10000)
        {
            return Task.FromResult(true);
        }

        public Task<bool> UpdateSales()
        {
            // This method can go
            return Task.FromResult(true);
        }
    }
}
