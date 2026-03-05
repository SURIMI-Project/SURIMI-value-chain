using Grpc.Core;
using Grpc.Surimi;
using SURIMI_value_chain.EwE;

namespace SURIMI_value_chain.Services
{
    public class ValueChainMarketService : MarketProviderService.MarketProviderServiceBase
    {
        private readonly ILogger<ValueChainMarketService> m_logger;
        private readonly IEwEController m_controller;

        public ValueChainMarketService(ILogger<ValueChainMarketService> logger, IEwEController controller)
        {
            m_logger = logger;
            m_controller = controller;
        }

        public override async Task<UpdateSalesResponse> UpdateSales(UpdateSalesRequest request, ServerCallContext context)
        {
            m_logger.LogInformation($"Updating sales for {request.SalesSummaries.Count} sales...");

            //var speciesPrices = request.SalesSummaries
            //    .Select(p => new Models.SpeciesPrice
            //    {
            //        // Note that the market does not distinguish species sizes, ages and lengths, and ignores gear specifics other than gearcode.
            //        // Although this is by design but may have to be revisited; the limitations seem like an oversight.
            //        SpeciesCode = p.Sales..Species.SpeciesCode,
            //        GearCode = p.GearCode,
            //        Price = p.Price,
            //        Currency = p.Currency,
            //        MeasurementUnit = p.MeasurementUnit,
            //        MarketCode = p.MarketCode,
            //        Timestamp = p.Timestamp.ToDateTime()
            //    })
            //    .ToList();

            var res = await m_controller.UpdateSales();

            if (res != true)
            {
                throw new RpcException(new Status(StatusCode.Internal, "Failed to Update Sales Value Chain"));
            }
            return new UpdateSalesResponse() { SimulationId = request.SimulationId };
        }
    }
}
