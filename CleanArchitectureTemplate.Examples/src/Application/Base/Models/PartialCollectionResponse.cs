using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SP.SampleCleanArchitectureTemplate.Application.Extensions.TaskExtensions;

namespace SP.SampleCleanArchitectureTemplate.Application.Base.Models
{
    /// <summary>
    ///     A collection of entities
    /// </summary>
    /// <typeparam name="TEntity">The entity in collection values</typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class PartialCollectionResponse<TEntity> : IPartialCollection<TEntity>
    {
        /// <summary>
        ///     The entities with pagination
        /// </summary>
        public IReadOnlyCollection<TEntity> Values { get; set; }

        /// <summary>
        ///     The current offset used for query the values
        /// </summary>
        public int? Offset { get; set; }

        /// <summary>
        ///     The current limit used for query the values
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        ///     The total of entities without pagination
        /// </summary>
        public long Count { get; set; }
    }
}

