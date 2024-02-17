namespace PipelineFilters
{
    public class Pipeline<T> where T : class
    {
        public List<IStep<T>> Steps { get; } = new();
        public IStep<T>? CurrentStep { get; set; }
        public int CurrentStepIndex { get; set; } = 0;

        private readonly PipelineContext _context;

        public Pipeline(PipelineContext context)
        {
            _context = context;
        }
        public void AddStep(IStep<T> step)
        {
            step.AddContext(_context);
            step.AddPipeline(this);

            Steps.Add(step);
        }

        public void AddRangeStep(List<IStep<T>> steps)
        {
            foreach (IStep<T> step in steps) 
            {
                step.AddContext(_context);

                Steps.Add(step);
            }
        }

        public async Task<(T Data, StepError? Error)> ExecuteAsync(T input)
        {
            T current = input;

            if (CurrentStep?.Error is not null)
            {
                return (current, CurrentStep?.Error);
            }

            for (int i = CurrentStepIndex; i < Steps.Count; i++)
            {
                current = await Steps[i].ExecuteAsync(current);

                CurrentStep = Steps[i];

                if (Steps[i].Error is not null)
                {
                    return (current, Steps[i].Error);
                }
                
                CurrentStepIndex++;

            }

            return (current, null);
        }
    }

}
