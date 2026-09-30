using Grpc.Core;
using Grpc.Surimi;
using SURIMI.Common.gRPC;
using SURIMI.Common.gRPC.Services;
using SURIMI_value_chain.EwE;

namespace SURIMI_value_chain.Services
{
    public class ValueChainService : Grpc.Surimi.ValueChainService.ValueChainServiceBase
    {
        private readonly ILogger<ValueChainService> m_logger;
        private readonly IEwEController m_controller;
        private readonly string m_version;

        public ValueChainService(ILogger<ValueChainService> logger, IEwEController controller, ProtocolVersionService protocolVersionService)
        {
            m_logger = logger;
            m_controller = controller;
            m_version = protocolVersionService.LoadVersion();
        }

        public override async Task<InitialiseExperimentResponse> InitialiseExperiment(InitialiseExperimentRequest request, ServerCallContext context)
        {
            GrpcValidation.ArgumentNotNullOrEmpty(request.ExperimentId);
            GrpcValidation.ArgumentNotNullOrEmpty(request.ScenarioName);

            m_logger.LogInformation($"Initializing experiment {request.ExperimentId}, scenario {request.ScenarioName}...");

            try
            {
                SURIMI.Datamodel.SurimiContract surimiContract = GetSurimiContract(request.Simulation);

                int result = await m_controller.StartAsync();
                if (result != 1)
                {
                    throw new RpcException(new Status(StatusCode.Internal, "Failed to initialise Value Chain"));
                }

                return new InitialiseExperimentResponse() { ExperimentId = request.ExperimentId };
            }
            catch (Exception ex)
            {
                m_logger.LogError(ex, "Error during InitialiseExperiment");
                throw;
            }
        }

        public override async Task<FinaliseExperimentResponse> FinaliseExperiment(FinaliseExperimentRequest request, ServerCallContext context)
        {
            m_logger.LogInformation($"Finalizing experiment {request.ExperimentId}");

            try
            {
                bool result = await m_controller.StopAsync();
                if (result == false)
                {
                    throw new RpcException(new Status(StatusCode.Internal, "Failed to finalise Value Chain"));
                }

                return new FinaliseExperimentResponse() { ExperimentId = request.ExperimentId };
            }
            catch (Exception ex)
            {
                m_logger.LogError(ex, "Error during FinaliseExperiment");
                throw;
            }
        }

        public override async Task<CancelExperimentResponse> CancelExperiment(CancelExperimentRequest request, ServerCallContext context)
        {
            m_logger.LogInformation($"Cancel experiment {request.ExperimentId}");

            try
            {
                bool result = await m_controller.StopAsync();
                if (result == false)
                {
                    throw new RpcException(new Status(StatusCode.Internal, "Failed to cancel Value Chain"));
                }

                return new CancelExperimentResponse() { ExperimentId = request.ExperimentId };
            }
            catch (Exception ex)
            {
                m_logger.LogError(ex, "Error during CancelExperiment");
                throw;
            }
        }

        public override async Task<ExperimentStepResponse> ExperimentStep(ExperimentStepRequest request, ServerCallContext context)
        {
            m_logger.LogInformation($"Simulate step for experiment {request.ExperimentId}");

            bool result = await m_controller.ContinueAsync();
            if (result == false)
            {
                throw new RpcException(new Status(StatusCode.Internal, "Failed to simulate step for Value Chain"));
            }

            return new ExperimentStepResponse() { ExperimentId = request.ExperimentId };
        }

        public override Task<GetProtocolVersionResponse> GetProtocolVersion(GetProtocolVersionRequest request, ServerCallContext context)
        {
            return Task.FromResult(new GetProtocolVersionResponse() { ProtocolVersion = m_version });
        }

        public override async Task<UpdateSalesStatisticsResponse> UpdateSalesStatistics(UpdateSalesStatisticsRequest request, ServerCallContext context)
        {
            m_logger.LogInformation($"Updating sales statistics for experiment {request.ExperimentId}");

            bool result = await m_controller.UpdateSales();
            if (result == false)
            {
                throw new RpcException(new Status(StatusCode.Internal, "Failed to update sales statistics for Value Chain"));
            }

            return new UpdateSalesStatisticsResponse() { ExperimentId = request.ExperimentId };
        }

        /// <summary>
        /// Mapping method from gRPC Surimi Simulation to SURIMI Datamodel SurimiConfiguration
        /// </summary>
        /// <param name="simulation"></param>
        /// <returns></returns>
        private SURIMI.Datamodel.SurimiContract GetSurimiContract(Grpc.Surimi.Simulation simulation)
        {
            return new SURIMI.Datamodel.SurimiContract
            {
                Simulation = new SURIMI.Datamodel.Simulation()
                {
                    CaseStudyName = simulation.CaseStudyName,
                    StartDateTime = simulation.StartDateTime.ToDateTime(),
                    MaximumEndDateTime = simulation.MaximumEndDateTime.ToDateTime(),
                    TimeStep = simulation.TimeStep,
                    Geography = new SURIMI.Datamodel.Geography()
                    {
                        Crs = new SURIMI.Datamodel.CoordinateReferenceSystem()
                        {
                            Authority = simulation.Geography.Crs.Authority,
                            Code = simulation.Geography.Crs.Code,
                            Name = simulation.Geography.Crs.Name
                        },
                        RasterCellOrigin = Enum.Parse<SURIMI.Datamodel.RasterCellOrigin>(simulation.Geography.RasterCellOrigin.ToString()),
                        Xres = simulation.Geography.Xres,
                        Yres = simulation.Geography.Yres,
                        Ncol = simulation.Geography.Ncol,
                        Nrow = simulation.Geography.Nrow,
                        Xmin = simulation.Geography.Xmin,
                        Xmax = simulation.Geography.Xmax,
                        Ymin = simulation.Geography.Ymin,
                        Ymax = simulation.Geography.Ymax
                    }
                },
                Standards = new SURIMI.Datamodel.Standards()
                {
                    Currency = simulation.Standards.Currency,
                    CountryCode = simulation.Standards.CountryCode,
                    //CategoryCode = surimiContract.Standards?.CategoryCode ?? string.Empty,        TODO
                    DateAndTime = simulation.Standards.DateAndTime,
                    GearCode = simulation.Standards.GearCode,
                    LifeStage = simulation.Standards.LifeStage,
                    MarketCode = simulation.Standards.MarketCode,
                    SpeciesCode = simulation.Standards.SpeciesCode,
                    Measurements = new SURIMI.Datamodel.Measurement()
                    {
                        System = simulation.Standards.Measurements.System,
                        Units = simulation.Standards.Measurements.Units
                            .Select(u => new SURIMI.Datamodel.UnitType
                            {
                                Quantity = u.Quantity ?? string.Empty,
                                Unit = u.Unit_ ?? string.Empty,
                            })
                            .ToList()
                    }
                },
                Items = new SURIMI.Datamodel.Items()
                {
                    Species = simulation.Items.Species
                        .Select(s => new SURIMI.Datamodel.Species
                        {
                            SpeciesCode = s.SpeciesCode,
                            LengthClass = s.LengthClass,
                            Age = s.Age,
                            LifeStage = s.LifeStage
                        })
                        .ToList(),
                    FleetSegments = simulation.Items.FleetSegments
                        .Select(f => new SURIMI.Datamodel.FleetSegment
                        {
                            GearCode = f.GearCode,
                            VesselLengthClass = f.VesselLengthClass,
                            Scale = f.Scale,
                            CountryCode = f.CountryCode,
                        })
                        .ToList(),
                    Currencies = simulation.Items.Currencies
                        .Select(c => new SURIMI.Datamodel.Currency
                        {
                            CurrencyCode = c.Code
                        })
                        .ToList(),
                    Markets = simulation.Items.Markets
                        .Select(c => new SURIMI.Datamodel.Market
                        {
                            MarketCode = c.MarketCode,
                        })
                        .ToList(),
                    PriceCategories = simulation.Items.PriceCategories
                        .Select(c => new SURIMI.Datamodel.PriceCategory
                        {
                            CategoryCode = c.CategoryCode,
                        })
                        .ToList(),
                    ClimateScenarios = simulation.Items.ClimateScenarios
                    .Select(c => new SURIMI.Datamodel.ClimateScenario
                    {
                        ClimateScenarioCode = c.ClimateScenarioCode,
                    })
                    .ToList()
                }
            };
        }
    }
}
