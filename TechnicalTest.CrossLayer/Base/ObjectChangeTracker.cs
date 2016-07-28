using System;
using System.Runtime.Serialization;

namespace TechnicalTest.CrossLayer
{
    // Helper class that captures most of the change tracking work that needs to be done
    // for self tracking entities.
    /// <summary>
    /// Class ObjectChangeTracker
    /// </summary>
    [DataContract(IsReference = true)]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ObjectChangeTracker
    {
        #region Fields

        /// <summary>
        /// The _change tracking enabled
        /// </summary>
        private bool _changeTrackingEnabled;

        /// <summary>
        /// The _extended properties
        /// </summary>
        private ExtendedPropertiesDictionary _extendedProperties;

        /// <summary>
        /// The _is deserializing
        /// </summary>
        private bool _isDeserializing;

        /// <summary>
        /// The _objects added to collections
        /// </summary>
        private ObjectsAddedToCollectionProperties _objectsAddedToCollections = new ObjectsAddedToCollectionProperties();

        /// <summary>
        /// The _objects removed from collections
        /// </summary>
        private ObjectsRemovedFromCollectionProperties _objectsRemovedFromCollections = new ObjectsRemovedFromCollectionProperties();

        /// <summary>
        /// The _object state
        /// </summary>
        private ObjectState _objectState = ObjectState.Added;

        /// <summary>
        /// The _original values
        /// </summary>
        private OriginalValuesDictionary _originalValues;

        #endregion Fields

        #region Events

        /// <summary>
        /// Occurs when [object state changing].
        /// </summary>
        public event EventHandler<ObjectStateChangingEventArgs> ObjectStateChanging;

        #endregion Events

        /// <summary>
        /// Gets or sets a value indicating whether [change tracking enabled].
        /// </summary>
        /// <value><c>true</c> if [change tracking enabled]; otherwise, <c>false</c>.</value>
        public bool ChangeTrackingEnabled
        {
            get { return _changeTrackingEnabled; }
            set { _changeTrackingEnabled = value; }
        }

        // Returns the extended property values.
        // This includes key values for independent associations that are needed for the
        // concurrency model in the Entity Framework
        /// <summary>
        /// Gets the extended properties.
        /// </summary>
        /// <value>The extended properties.</value>
        [DataMember]
        public ExtendedPropertiesDictionary ExtendedProperties
        {
            get
            {
                if (_extendedProperties == null)
                {
                    _extendedProperties = new ExtendedPropertiesDictionary();
                }
                return _extendedProperties;
            }
        }

        // Returns the added objects to collection valued properties that were changed.
        /// <summary>
        /// Gets the objects added to collection properties.
        /// </summary>
        /// <value>The objects added to collection properties.</value>
        [DataMember]
        public ObjectsAddedToCollectionProperties ObjectsAddedToCollectionProperties
        {
            get
            {
                if (_objectsAddedToCollections == null)
                {
                    _objectsAddedToCollections = new ObjectsAddedToCollectionProperties();
                }
                return _objectsAddedToCollections;
            }
        }

        // Returns the removed objects to collection valued properties that were changed.
        /// <summary>
        /// Gets the objects removed from collection properties.
        /// </summary>
        /// <value>The objects removed from collection properties.</value>
        [DataMember]
        public ObjectsRemovedFromCollectionProperties ObjectsRemovedFromCollectionProperties
        {
            get
            {
                if (_objectsRemovedFromCollections == null)
                {
                    _objectsRemovedFromCollections = new ObjectsRemovedFromCollectionProperties();
                }
                return _objectsRemovedFromCollections;
            }
        }

        // Returns the original values for properties that were changed.
        /// <summary>
        /// Gets the original values.
        /// </summary>
        /// <value>The original values.</value>
        [DataMember]
        public OriginalValuesDictionary OriginalValues
        {
            get
            {
                if (_originalValues == null)
                {
                    _originalValues = new OriginalValuesDictionary();
                }
                return _originalValues;
            }
        }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        [DataMember]
        public ObjectState State
        {
            get { return _objectState; }
            set
            {
                if (_isDeserializing || _changeTrackingEnabled)
                {
                    OnObjectStateChanging(value);
                    _objectState = value;
                }
            }
        }

        /// <summary>
        /// Called when [object state changing].
        /// </summary>
        /// <param name="newState">The new state.</param>
        protected virtual void OnObjectStateChanging(ObjectState newState)
        {
            if (ObjectStateChanging != null)
            {
                ObjectStateChanging(this, new ObjectStateChangingEventArgs() { NewState = newState });
            }
        }

        #region MethodsForChangeTrackingOnClient

        // Resets the ObjectChangeTracker to the Unchanged state and
        // clears the original values as well as the record of changes
        // to collection properties
        /// <summary>
        /// Accepts the changes.
        /// </summary>
        public void AcceptChanges()
        {
            OnObjectStateChanging(ObjectState.Unchanged);
            OriginalValues.Clear();
            ObjectsAddedToCollectionProperties.Clear();
            ObjectsRemovedFromCollectionProperties.Clear();
            ChangeTrackingEnabled = true;
            _objectState = ObjectState.Unchanged;
        }

        // Records an addition to collection valued properties on SelfTracking Entities.
        /// <summary>
        /// Records the addition to collection properties.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        internal void RecordAdditionToCollectionProperties(string propertyName, object value)
        {
            if (_changeTrackingEnabled)
            {
                // Add the entity back after deleting it, we should do nothing here then
                if (ObjectsRemovedFromCollectionProperties.ContainsKey(propertyName)
                    && ObjectsRemovedFromCollectionProperties[propertyName].Contains(value))
                {
                    ObjectsRemovedFromCollectionProperties[propertyName].Remove(value);
                    if (ObjectsRemovedFromCollectionProperties[propertyName].Count == 0)
                    {
                        ObjectsRemovedFromCollectionProperties.Remove(propertyName);
                    }
                    return;
                }

                if (!ObjectsAddedToCollectionProperties.ContainsKey(propertyName))
                {
                    ObjectsAddedToCollectionProperties[propertyName] = new ObjectList();
                    ObjectsAddedToCollectionProperties[propertyName].Add(value);
                }
                else
                {
                    ObjectsAddedToCollectionProperties[propertyName].Add(value);
                }
            }
        }

        // Captures the original value for a property that is changing.
        /// <summary>
        /// Records the original value.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        internal void RecordOriginalValue(string propertyName, object value)
        {
            if (_changeTrackingEnabled && _objectState != ObjectState.Added)
            {
                if (!OriginalValues.ContainsKey(propertyName))
                {
                    OriginalValues[propertyName] = value;
                }
            }
        }

        // Records a removal to collection valued properties on SelfTracking Entities.
        /// <summary>
        /// Records the removal from collection properties.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        internal void RecordRemovalFromCollectionProperties(string propertyName, object value)
        {
            if (_changeTrackingEnabled)
            {
                // Delete the entity back after adding it, we should do nothing here then
                if (ObjectsAddedToCollectionProperties.ContainsKey(propertyName)
                    && ObjectsAddedToCollectionProperties[propertyName].Contains(value))
                {
                    ObjectsAddedToCollectionProperties[propertyName].Remove(value);
                    if (ObjectsAddedToCollectionProperties[propertyName].Count == 0)
                    {
                        ObjectsAddedToCollectionProperties.Remove(propertyName);
                    }
                    return;
                }

                if (!ObjectsRemovedFromCollectionProperties.ContainsKey(propertyName))
                {
                    ObjectsRemovedFromCollectionProperties[propertyName] = new ObjectList();
                    ObjectsRemovedFromCollectionProperties[propertyName].Add(value);
                }
                else
                {
                    if (!ObjectsRemovedFromCollectionProperties[propertyName].Contains(value))
                    {
                        ObjectsRemovedFromCollectionProperties[propertyName].Add(value);
                    }
                }
            }
        }

        /// <summary>
        /// Called when [deserialized method].
        /// </summary>
        /// <param name="context">The context.</param>
        [OnDeserialized]
        private void OnDeserializedMethod(StreamingContext context)
        {
            _isDeserializing = false;
        }

        /// <summary>
        /// Called when [deserializing method].
        /// </summary>
        /// <param name="context">The context.</param>
        [OnDeserializing]
        private void OnDeserializingMethod(StreamingContext context)
        {
            _isDeserializing = true;
        }

        #endregion MethodsForChangeTrackingOnClient
    }
}