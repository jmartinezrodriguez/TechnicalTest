using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TechnicalTest.CrossLayer
{
    internal class ChangeTrackerIterator : IDisposable, IEnumerable<object>
    {
        private List<IObjectWithChangeTracker> _items = new List<IObjectWithChangeTracker>();

        private ChangeTrackerIterator()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "entity")]
        public static ChangeTrackerIterator Create<TEntity>(TEntity entity) where TEntity : IObjectWithChangeTracker
        {
            var iterator = new ChangeTrackerIterator();
            return iterator;
        }

        #region Public Methods

        internal void Execute(Action<IObjectWithChangeTracker> action)
        {
            _items.ForEach(action);
        }

        #endregion Public Methods

        #region Visit and Traverse Methods

        private void Visit(IObjectWithChangeTracker entity)
        {
            if (entity != null)
            {
                if (!_items.Contains(entity))
                {
                    _items.Add(entity);
                    Traverse(entity);
                }
            }
        }

        private void Traverse(IObjectWithChangeTracker entity)
        {
            Type type = entity.GetType();

            IEnumerable<PropertyInfo> properties = (from pInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                    where typeof(IObjectWithChangeTracker).IsAssignableFrom(pInfo.PropertyType)
                                                    select pInfo);

            properties.ToList().ForEach(t => Visit(t.GetValue(entity, null) as IObjectWithChangeTracker));

            properties = (from pInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                          where typeof(ICollection).IsAssignableFrom(pInfo.PropertyType)
                          select pInfo);

            properties.ToList().ForEach(p =>
                {
                    IEnumerable enumerable = (IEnumerable)p.GetValue(entity, null);
                    if (enumerable == null)
                    {
                        enumerable.OfType<IObjectWithChangeTracker>().ToList().ForEach(e => Visit(e));
                    }
                }
                    );
        }

        #endregion Visit and Traverse Methods

        public void Dispose()
        {
            _items.Clear();
        }

        public IEnumerator<object> GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._items.GetEnumerator();
        }
    }
}