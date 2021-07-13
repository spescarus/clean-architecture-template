using System;
using System.Diagnostics.CodeAnalysis;

namespace SP.SampleCleanArchitectureTemplate.Application.Base.Models
{
    [ExcludeFromCodeCoverage]
    public class TimeStampedModel
    {
        public DateTime  CreatedAt { get; set; }
        public DateTime  UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
