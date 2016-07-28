using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;
using TechnicalTest.CrossLayer;

namespace TechnicalTest.DataLayer
{
    internal class EntityIndex
    {
        private readonly ObjectContext _context;

        // Set of all entities
        private readonly HashSet<IObjectWithChangeTracker> _allEntities;

        // Index of the final key that will be used in the context (could be real for non-added, could be temporary for added)
        // to the initial temporary key
        private readonly Dictionary<EntityKey, EntityKey> _temporaryKeyMap;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public EntityIndex(ObjectContext context)
        {
            _context = context;

            _allEntities = new HashSet<IObjectWithChangeTracker>();
            _temporaryKeyMap = new Dictionary<EntityKey, EntityKey>();
        }

        public void Add(ObjectStateEntry entry, ObjectChangeTracker changeTracker)
        {
            EntityKey temporaryKey = entry.EntityKey;
            EntityKey finalKey;

            if (!_allEntities.Contains(entry.Entity))
            {
                // Track that this Apply will be handling this entity
                _allEntities.Add(entry.Entity as IObjectWithChangeTracker);
            }

            if (changeTracker.State == ObjectState.Added)
            {
                finalKey = temporaryKey;
            }
            else
            {
                finalKey = _context.CreateEntityKey(temporaryKey.EntityContainerName + "." + temporaryKey.EntitySetName, entry.Entity);
            }
            if (!_temporaryKeyMap.ContainsKey(finalKey))
            {
                _temporaryKeyMap.Add(finalKey, temporaryKey);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public bool Contains(object entity)
        {
            return _allEntities.Contains(entity);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public IEnumerable<IObjectWithChangeTracker> AllEntities
        {
            get { return _allEntities; }
        }

        // Converts the passed in EntityKey to the EntityKey that is usable by the current state of ApplyChanges
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public EntityKey ConvertEntityKey(EntityKey targetKey)
        {
            ObjectStateEntry targetEntry;
            if (!_context.ObjectStateManager.TryGetObjectStateEntry(targetKey, out targetEntry))
            {
                // If no entry exists, then either:
                // 1. This is an EntityKey that is not represented in the set of entities being dealt with during the Apply
                // 2. This is an EntityKey that will represent one of the yet-to-be-processed Added entries, so look it up
                EntityKey temporaryKey;
                if (_temporaryKeyMap.TryGetValue(targetKey, out temporaryKey))
                {
                    targetKey = temporaryKey;
                }
            }
            return targetKey;
        }
    }
}