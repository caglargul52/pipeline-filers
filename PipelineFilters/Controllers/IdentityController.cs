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

            var getUserLoginInfoStepResult = await pipeline
                .AddStep(new CheckAppUserStep(isBypassed: true))
                .AddStep(new CheckUserStateStep())
                .AddStep(new GetUserLoginInfoStep())
                .ExecuteAsync();


            var result = await pipeline
                .AddRangeStep(ConditionalStepGroup.GetCentralLoginSteps(getUserLoginInfoStepResult.Data))
                .AddStep(new CheckUserStaticIPAddressStep())
                .AddStep(new GetActivationStep())
                .AddStep(new CompletePrepareActivationStep())
                .ExecuteAsync();
        }
    }
}
