using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace SP.SampleCleanArchitectureTemplate.Application.Base
{
    [ExcludeFromCodeCoverage]
    public abstract class Service
    {
        protected Service(IMapper mapper,
                          ILogger logger)
        {
            Logger = logger;
        }

        protected ILogger Logger { get; }
    }
}
