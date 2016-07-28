using System;
using System.Linq;

namespace TechnicalTest.CrossLayer
{
    /// <summary>
    /// Class ChangeTrackerExtensions
    /// </summary>
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public static class ChangeTrackerExtensions
    {
        /// <summary>
        /// Accepts all changes.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public static void AcceptAllChanges(this IObjectWithChangeTracker entity)
        {
            using (ChangeTrackerIterator iterator = ChangeTrackerIterator.Create(entity))
            {
                iterator.Execute(t => t.AcceptChanges());
            }
        }

        /// <summary>
        /// Merges the with.
        /// </summary>
        /// <typeparam name="TEntity">The type of the T entity.</typeparam>
        /// <typeparam name="TGraph">The type of the T graph.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="graph">The graph.</param>
        /// <param name="KeyCompare">The key compare.</param>
        /// <returns>``0.</returns>
        public static TEntity MergeWith<TEntity, TGraph>(TEntity entity, TGraph graph, Func<TEntity, TEntity, bool> KeyCompare)
            where TEntity : class ,IObjectWithChangeTracker
            where TGraph : class ,IObjectWithChangeTracker
        {
            using (ChangeTrackerIterator iterator = ChangeTrackerIterator.Create(entity))
            {
                return iterator.OfType<TEntity>().SingleOrDefault(e => KeyCompare(entity, e)) ?? entity;
            }
        }

        /// <summary>
        /// Starts the tracking all.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public static void StartTrackingAll(this IObjectWithChangeTracker entity)
        {
            using (ChangeTrackerIterator iterator = ChangeTrackerIterator.Create(entity))
            {
                iterator.Execute(t => t.StartTracking());
            }
        }

        /// <summary>
        /// Stops the tracking all.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public static void StopTrackingAll(this IObjectWithChangeTracker entity)
        {
            using (ChangeTrackerIterator iterator = ChangeTrackerIterator.Create(entity))
            {
                iterator.Execute(t => t.StopTracking());
            }
        }
    }
}