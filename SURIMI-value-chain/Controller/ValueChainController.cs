using Eii.ControlledVocabularies.Core;
using Grpc.Surimi;

namespace SURIMI_value_chain.EwE
{
    public class ValueChainController : IValueChainController
    {

        public ValueChainController()
        {
            // Just to test that ControlledVocabularies references are working
            var m_multiLevelKeyFactory = new MultiLevelKeyFactory();

        }

        public Task<bool> ContinueAsync(int timeoutMs = 60000)
        {
            return Task.FromResult(true);
        }


        public Task<List<SalesSummary>> UpdateSalesSummariesAsync()
        {
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
            return Task.FromResult(true);
        }

    }
}
