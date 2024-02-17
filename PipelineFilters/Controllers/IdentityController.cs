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
            var myDto = new PrepareActivationStepDto { Message = "Initial Input" };

            var pipeline = new Pipeline<PrepareActivationStepDto>(_context);

            pipeline.AddStep(new CheckAppUserStep(isBypassed: true));

            pipeline.AddStep(new GenerateActivationGuidStep());

            pipeline.AddStep(new CheckUserStateStep());

            //pipeline.AddStep(_serviceProvider.GetRequiredService<GetUserLoginInfoStep>());

            var result = await pipeline.ExecuteAsync(myDto);

            //if (result.Error is not null)
            //{
            //    // Koþullara baðlý olarak dinamik adýmlar ekleniyor

            //    pipeline.AddRangeStep(ConditionalStepGroup.GetCentralLoginSteps(result.Data));

            //    pipeline.AddStep(new CheckUserStaticIPAddressStep());
            //    pipeline.AddStep(new GetActivationStep());
            //    pipeline.AddStep(new CompletePrepareActivationStep());
            //}

            //var result2 = await pipeline.ExecuteAsync(myDto);

            //Mapping
        }
    }
}
