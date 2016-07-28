
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using TechnicalTest.CrossLayer;

namespace TechnicalTest.DataLayer
{
    internal class AddHelper
    {
        private readonly ObjectContext _context;
        private readonly EntityIndex _entityIndex;

        // Used during add processing
        private readonly Queue<Tuple<string, IObjectWithChangeTracker>> _entitiesToAdd;

        private readonly Queue<Tuple<ObjectStateEntry, string, IEnumerable<object>>> _entitiesDuringAdd;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static EntityIndex AddAllEntities(ObjectContext context, string entitySetName, IObjectWithChangeTracker entity)
        {
            AddHelper addHelper = new AddHelper(context);

            try
            {
                // Include the root element to start the Apply
                addHelper.QueueAdd(entitySetName, entity);

                // Add everything
                while (addHelper.HasMore)
                {
                    Tuple<string, IObjectWithChangeTracker> entityInSet = addHelper.NextAdd();
                    // Only add the object if it's not already in the context
                    ObjectStateEntry entry = null;
                    if (!context.ObjectStateManager.TryGetObjectStateEntry(entityInSet.Item2, out entry))
                    {
                        context.AddObject(entityInSet.Item1, entityInSet.Item2);
                    }
                }
            }
            finally
            {
                addHelper.Detach();
            }
            return addHelper.EntityIndex;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private AddHelper(ObjectContext context)
        {
            _context = context;
            _context.ObjectStateManager.ObjectStateManagerChanged += this.HandleStateManagerChange;

            _entityIndex = new EntityIndex(context);
            _entitiesToAdd = new Queue<Tuple<string, IObjectWithChangeTracker>>();
            _entitiesDuringAdd = new Queue<Tuple<ObjectStateEntry, string, IEnumerable<object>>>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private void Detach()
        {
            _context.ObjectStateManager.ObjectStateManagerChanged -= this.HandleStateManagerChange;
        }

        private void HandleStateManagerChange(object sender, CollectionChangeEventArgs args)
        {
            if (args.Action == CollectionChangeAction.Add)
            {
                IObjectWithChangeTracker entity = args.Element as IObjectWithChangeTracker;
                ObjectStateEntry entry = _context.ObjectStateManager.GetObjectStateEntry(entity);
                ObjectChangeTracker changeTracker = entity.ChangeTracker;

                changeTracker.ChangeTrackingEnabled = false;
                _entityIndex.Add(entry, changeTracker);

                // Queue removed reference values
                var navPropNames = _context.MetadataWorkspace.GetCSpaceEntityType(entity.GetType()).NavigationProperties.Select(n => n.Name);
                var entityRefOriginalValues = changeTracker.OriginalValues.Where(kvp => navPropNames.Contains(kvp.Key));
                foreach (KeyValuePair<string, object> originalValueWithName in entityRefOriginalValues)
                {
                    if (originalValueWithName.Value != null)
                    {
                        _entitiesDuringAdd.Enqueue(new Tuple<ObjectStateEntry, string, IEnumerable<object>>(
                            entry,
                            originalValueWithName.Key,
                            new object[] { originalValueWithName.Value }));
                    }
                }

                // Queue removed collection values
                foreach (KeyValuePair<string, ObjectList> collectionPropertyChangesWithName in changeTracker.ObjectsRemovedFromCollectionProperties)
                {
                    _entitiesDuringAdd.Enqueue(new Tuple<ObjectStateEntry, string, IEnumerable<object>>(
                        entry,
                        collectionPropertyChangesWithName.Key,
                        collectionPropertyChangesWithName.Value));
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private EntityIndex EntityIndex
        {
            get { return _entityIndex; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private bool HasMore
        {
            get { ProcessNewAdds(); return _entitiesToAdd.Count > 0; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private void QueueAdd(string entitySetName, IObjectWithChangeTracker entity)
        {
            if (!_entityIndex.Contains(entity))
            {
                // Queue the entity so that we can add the 'removed collection' items
                _entitiesToAdd.Enqueue(new Tuple<string, IObjectWithChangeTracker>(entitySetName, entity));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private Tuple<string, IObjectWithChangeTracker> NextAdd()
        {
            ProcessNewAdds();
            return _entitiesToAdd.Dequeue();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private void ProcessNewAdds()
        {
            while (_entitiesDuringAdd.Count > 0)
            {
                Tuple<ObjectStateEntry, string, IEnumerable<object>> relatedEntities = _entitiesDuringAdd.Dequeue();
                RelatedEnd relatedEnd = relatedEntities.Item1.GetRelatedEnd(relatedEntities.Item2);
                string entitySetName = relatedEnd.GetEntitySetName();

                foreach (var targetEntity in relatedEntities.Item3)
                {
                    QueueAdd(entitySetName, targetEntity as IObjectWithChangeTracker);
                }
            }
        }
    }
}