namespace PipelineFilters
{
    public class Pipeline<T> where T : class, new()
    {
        public List<IStep<T>> Steps { get; } = new();
        public IStep<T>? CurrentStep { get; set; }
        public int CurrentStepIndex { get; set; } = 0;

        private readonly PipelineContext _context;

        private T _currentDto;

        public Pipeline(PipelineContext context)
        {
            _context = context;
            _currentDto = new T();
        }
        public void AddStep(IStep<T> step)
        {
            step.AddContext(_context);
            step.AddPipeline(this);

            Steps.Add(step);
        }

        public async Task<(bool IsSucess, T Data, StepError? Error)> AddStepAndExecute(IStep<T> step)
        {
            step.AddContext(_context);
            step.AddPipeline(this);

            Steps.Add(step);

            return await ExecuteAsync();
        }

        public Pipeline<T> AddStep2(IStep<T> step)
        {
            step.AddContext(_context);
            step.AddPipeline(this);

            Steps.Add(step);

            return this;
        }

        public void AddRangeStep(List<IStep<T>> steps)
        {
            foreach (IStep<T> step in steps) 
            {
                step.AddContext(_context);

                Steps.Add(step);
            }
        }

        public Pipeline<T> AddRangeStep2 (List<IStep<T>> steps)
        {
            foreach (IStep<T> step in steps)
            {
                step.AddContext(_context);

                Steps.Add(step);
            }

            return this;
        }

        public async Task<(bool IsSucess, T Data, StepError? Error)> ExecuteAsync()
        {
            if (CurrentStep?.Error is not null)
            {
                return (false, _currentDto, CurrentStep?.Error);
            }

            for (int i = CurrentStepIndex; i < Steps.Count; i++)
            {
                _currentDto = await Steps[i].ExecuteAsync(_currentDto);

                CurrentStep = Steps[i];

                if (Steps[i].Error is not null)
                {
                    return (false, _currentDto, Steps[i].Error);
                }
                
                CurrentStepIndex++;

            }

            return (true, _currentDto, null);
        }
    }

}
