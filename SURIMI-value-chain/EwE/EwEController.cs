using Eii.ControlledVocabularies.Core;
using Eii.ControlledVocabularies.Utils;
using Grpc.Surimi;

namespace SURIMI_value_chain.EwE
{
    public class EwEController : IEwEController
    {

        private RunStates m_runstate = RunStates.idle;

        public EwEController()
        {
            // Just to test that ControlledVocabularies references are working
            var m_multiLevelKeyFactory = new MultiLevelKeyFactory();

        }

        public Task<bool> ContinueAsync(int timeoutMs = 60000)
        {
            return Task.FromResult(true);
        }

        //public Task<Biomass> GetBiomassAsync()
        //{
        //    throw new NotImplementedException();
        //}

        public Task<CatchDispositionSummary> GetCatchDispositionSummaryAsync(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
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

        #region EwE helpers

        /// <summary>
        /// Enumerated type, defining the possible run states of the EwEController.
        /// </summary>
        public enum RunStates : uint
        {
            /// <summary>Ready to be started.</summary>
            idle = 0,
            /// <summary>Starting up, not ready yet.</summary>
            starting,
            /// <summary>Waiting for external input.</summary>
            waiting,
            /// <summary>Busy running simulations.</summary>
            running,
            /// <summary>Busy stopping.</summary>
            stopping
        }

        /// <summary>
        /// The current EwE run state.
        /// </summary>
        public RunStates RunState
        {
            get => m_runstate;
            private set
            {
                if (m_runstate != value)
                {
                    //Console.WriteLine("Run state set to " + value.ToString());
                    m_runstate = value;
                    //OnRunStateChanged?.Invoke(m_runstate);
                }
            }
        }
        /// <summary>
        /// Helper method, returns if Ecospace is waiting for input.
        /// </summary>
        public bool IsWaiting { get { return RunState == RunStates.waiting; } }

        #endregion EwE helpers
    }
}
