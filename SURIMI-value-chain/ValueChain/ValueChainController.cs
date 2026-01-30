using Grpc.Surimi;
using ValueChain;

namespace SURIMI_value_chain.ValueChain
{
    public class ValueChainController : IValueChainController
    {

        private cValueChainData? m_data;
        private cValueChain? m_chain;
        private cValueChainResults? m_results;
        private int m_iTimeStep = 1;


        public ValueChainController()
        {
        }

        public bool IsWaiting { get; set; }

        public Task<bool> UpdateSales()
        {
            return Task.FromResult(true);
        }

        public Task<int> StartAsync(int timeoutMs = 60000)
        {
            m_data =new cValueChainData();
            m_chain = new cValueChain(m_data);
            m_results = new cValueChainResults(m_data);

            m_data.InitRun();
            m_iTimeStep = 1;

            // ToDo: load the value chain from .sqlite file

            IsWaiting = true;

            return Task.FromResult(1);
        }

        public Task<bool> ContinueAsync(int timeoutMs = 60000)
        {
            bool success = false;

            if (m_data != null)
            {
                if (m_data.InitTimeStep())
                {
                    foreach (cProducerUnit unit in m_data.GetUnits(cUnitFactory.eUnitType.Producer))
                    {
                        unit.Process(m_results, m_iTimeStep, 0);
                    }
                    success = true;
                }
            }

            m_iTimeStep =+ 1;

            return Task.FromResult(success);
        }

        public Task<bool> StopAsync(int timeoutMs = 10000)
        {
            m_chain = null;
            m_data = null;
            m_results = null;

            IsWaiting = false;

            return Task.FromResult(true);
        }
    }
}
