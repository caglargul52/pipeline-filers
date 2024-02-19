using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PipelineFilters.Steps;

namespace PipelineFilters.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class IdentityController : ControllerBase
    {
        private readonly PipelineContext _context;
        private readonly ILogger<IdentityController> _logger;

        public IdentityController(ILogger<IdentityController> logger, PipelineContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost(Name = "PrepareActivation")]
        public async Task PrepareActivation()
        {
            var pipeline = new Pipeline<PrepareActivationStepDto>(_context);

            pipeline.AddStep(new CheckAppUserStep(isBypassed: true));

            var checkUserStateStepResult = await pipeline.AddStepAndExecute(new CheckUserStateStep());

            pipeline.AddRangeStep(ConditionalStepGroup.GetCentralLoginSteps(checkUserStateStepResult.Data));

            pipeline.AddStep(new CheckUserStaticIPAddressStep());

            pipeline.AddStep(new GetActivationStep());

            var completePrepareActivationStepResult = await pipeline.AddStepAndExecute(new CompletePrepareActivationStep());

            /*

            var result1 = await pipeline
                .AddStep2(new CheckAppUserStep())
                .AddStep2(new CheckUserStateStep())
                .ExecuteAsync();

            //pipeline.AddStep(_serviceProvider.GetRequiredService<GetUserLoginInfoStep>());

            //var result2 = await pipeline.ExecuteAsync();

            var result3 = await pipeline
                .AddRangeStep2(ConditionalStepGroup.GetCentralLoginSteps(result1.Data))
                .AddStep2(new CheckUserStaticIPAddressStep())
                .AddStep2(new GetActivationStep())
                .AddStep2(new CompletePrepareActivationStep())
                .ExecuteAsync();
            */
        }
    }
}
