using Grpc.Core;
using Grpc.Surimi;
using SURIMI_value_chain.EwE;

namespace SURIMI_value_chain.Services
{
    public class ValueChainWorkflowService : WorkflowService.WorkflowServiceBase
    {
        private readonly ILogger<ValueChainWorkflowService> m_logger;
        private readonly IEwEController m_controller;

        public ValueChainWorkflowService(ILogger<ValueChainWorkflowService> logger, IEwEController controller)
        {
            m_logger = logger;
            m_controller = controller;
        }

        public override async Task<InitialiseResponse> Initialise(InitialiseRequest request, ServerCallContext context)
        {
            GrpcValidation.ArgumentNotNullOrEmpty(request.ScenarioId);


            m_logger.LogInformation($"Initializing simulation {request.SimulationId}, scenario {request.ScenarioId}...");

            try
            {
                var result = await m_controller.StartAsync();
                if (result != 1)
                {
                    throw new RpcException(new Status(StatusCode.Internal, "Failed to initialise Value Chain"));
                }
                return new InitialiseResponse() { SimulationId = request.SimulationId };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override async Task<FinaliseResponse> Finalise(FinaliseRequest request, ServerCallContext context)
        {
            m_logger.LogInformation($"Finalizing simulation {request.SimulationId}");

            try
            {
                var result = await m_controller.StopAsync();
                if (result == false)
                {
                    throw new RpcException(new Status(StatusCode.Internal, "Failed to finalise Value Chain"));
                }
                return new FinaliseResponse() { SimulationId = request.SimulationId };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override async Task<CancelResponse> Cancel(CancelRequest request, ServerCallContext context)
        {
            m_logger.LogInformation($"Cancel simulation {request.SimulationId}");

            try
            {
                var result = await m_controller.StopAsync();
                if (result == false)
                {
                    throw new RpcException(new Status(StatusCode.Internal, "Failed to cancel Value Chain"));
                }
                return new CancelResponse() { SimulationId = request.SimulationId };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override async Task<SimulateStepResponse> SimulateStep(SimulateStepRequest request, ServerCallContext context)
        {
            m_logger.LogInformation($"Simulate step for simulation {request.SimulationId}");

            var res = await m_controller.ContinueAsync();

            return new SimulateStepResponse() { SimulationId = request.SimulationId };
        }
    }
}
