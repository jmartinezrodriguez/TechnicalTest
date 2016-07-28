
using System;

namespace TechnicalTest.CrossLayer
{
    /// <summary>
    /// Class ObjectWithChangeTrackerExtensions
    /// </summary>
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public static class ObjectWithChangeTrackerExtensions
    {
        /// <summary>
        /// Accepts the changes.
        /// </summary>
        /// <param name="trackingItem">The tracking item.</param>
        /// <exception cref="System.ArgumentNullException">trackingItem</exception>
        public static void AcceptChanges(this IObjectWithChangeTracker trackingItem)
        {
            if (trackingItem == null)
            {
                throw new ArgumentNullException("trackingItem");
            }

            trackingItem.ChangeTracker.AcceptChanges();
        }

        /// <summary>
        /// Marks as added.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="trackingItem">The tracking item.</param>
        /// <returns>``0.</returns>
        /// <exception cref="System.ArgumentNullException">trackingItem</exception>
        public static T MarkAsAdded<T>(this T trackingItem) where T : IObjectWithChangeTracker
        {
            if (trackingItem == null)
            {
                throw new ArgumentNullException("trackingItem");
            }

            trackingItem.ChangeTracker.ChangeTrackingEnabled = true;
            trackingItem.ChangeTracker.State = ObjectState.Added;
            return trackingItem;
        }

        /// <summary>
        /// Marks as deleted.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="trackingItem">The tracking item.</param>
        /// <returns>``0.</returns>
        /// <exception cref="System.ArgumentNullException">trackingItem</exception>
        public static T MarkAsDeleted<T>(this T trackingItem) where T : IObjectWithChangeTracker
        {
            if (trackingItem == null)
            {
                throw new ArgumentNullException("trackingItem");
            }

            trackingItem.ChangeTracker.ChangeTrackingEnabled = true;
            trackingItem.ChangeTracker.State = ObjectState.Deleted;
            return trackingItem;
        }

        /// <summary>
        /// Marks as modified.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="trackingItem">The tracking item.</param>
        /// <returns>``0.</returns>
        /// <exception cref="System.ArgumentNullException">trackingItem</exception>
        public static T MarkAsModified<T>(this T trackingItem) where T : IObjectWithChangeTracker
        {
            if (trackingItem == null)
            {
                throw new ArgumentNullException("trackingItem");
            }

            trackingItem.ChangeTracker.ChangeTrackingEnabled = true;
            trackingItem.ChangeTracker.State = ObjectState.Modified;
            return trackingItem;
        }

        /// <summary>
        /// Marks as unchanged.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="trackingItem">The tracking item.</param>
        /// <returns>``0.</returns>
        /// <exception cref="System.ArgumentNullException">trackingItem</exception>
        public static T MarkAsUnchanged<T>(this T trackingItem) where T : IObjectWithChangeTracker
        {
            if (trackingItem == null)
            {
                throw new ArgumentNullException("trackingItem");
            }

            trackingItem.ChangeTracker.ChangeTrackingEnabled = true;
            trackingItem.ChangeTracker.State = ObjectState.Unchanged;
            return trackingItem;
        }

        /// <summary>
        /// Starts the tracking.
        /// </summary>
        /// <param name="trackingItem">The tracking item.</param>
        /// <exception cref="System.ArgumentNullException">trackingItem</exception>
        public static void StartTracking(this IObjectWithChangeTracker trackingItem)
        {
            if (trackingItem == null)
            {
                throw new ArgumentNullException("trackingItem");
            }

            trackingItem.ChangeTracker.ChangeTrackingEnabled = true;
        }

        /// <summary>
        /// Stops the tracking.
        /// </summary>
        /// <param name="trackingItem">The tracking item.</param>
        /// <exception cref="System.ArgumentNullException">trackingItem</exception>
        public static void StopTracking(this IObjectWithChangeTracker trackingItem)
        {
            if (trackingItem == null)
            {
                throw new ArgumentNullException("trackingItem");
            }

            trackingItem.ChangeTracker.ChangeTrackingEnabled = false;
        }
    }
}