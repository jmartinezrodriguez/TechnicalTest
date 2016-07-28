
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace TechnicalTest.CrossLayer
{
    // An System.Collections.ObjectModel.ObservableCollection that raises
    // individual item removal notifications on clear and prevents adding duplicates.
    /// <summary>
    /// Class TrackableCollection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [CollectionDataContract]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class TrackableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        /// Clears the items.
        /// </summary>
        protected override void ClearItems()
        {
            new List<T>(this).ForEach(t => Remove(t));
        }

        /// <summary>
        /// Inserts the item.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        protected override void InsertItem(int index, T item)
        {
            if (!this.Contains(item))
            {
                base.InsertItem(index, item);
            }
        }
    }
}