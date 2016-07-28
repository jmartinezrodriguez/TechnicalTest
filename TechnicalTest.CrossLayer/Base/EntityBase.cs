using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace TechnicalTest.CrossLayer
{
    /// <summary>
    /// Class EntityBase
    /// </summary>
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class EntityBase : IObjectWithChangeTracker, INotifyPropertyChanged, IDataErrorInfo
    {
        /// <summary>
        /// The property getters
        /// </summary>
        private readonly Dictionary<String, PropertyInfo> propertyGetters;

        /// <summary>
        /// The validators
        /// </summary>
        private readonly Dictionary<String, ValidationAttribute[]> validators;

        /// <summary>
        /// The _is ready to validate
        /// </summary>
        private Boolean _isReadyToValidate = false;

        /// <summary>
        /// The metadata type
        /// </summary>
        private Type metadataType;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBase"/> class.
        /// </summary>
        public EntityBase()
        {
            propertyGetters = new Dictionary<String, PropertyInfo>();
            validators = new Dictionary<String, ValidationAttribute[]>();
            object[] meta = this.GetType().GetCustomAttributes(typeof(MetadataTypeAttribute), false);

            if (meta != null && meta.Length > 0)
            {
                metadataType = ((MetadataTypeAttribute)meta[0]).MetadataClassType;
                TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(this.GetType(), metadataType), this.GetType());
                var info = metadataType.GetProperties().Where(p => GetValidations(p).Length != 0).Cast<PropertyInfo>();

                foreach (PropertyInfo item in info)
                {
                    propertyGetters.Add(item.Name, item);
                    validators.Add(item.Name, GetValidations(item));
                }
            }
        }

        #region ChangeTracking

        /// <summary>
        /// The _change tracker
        /// </summary>
        private ObjectChangeTracker _changeTracker;

        /// <summary>
        /// Gets or sets the change tracker.
        /// </summary>
        /// <value>The change tracker.</value>
        [DataMember]
        public ObjectChangeTracker ChangeTracker
        {
            get
            {
                if (_changeTracker == null)
                {
                    _changeTracker = new ObjectChangeTracker();
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
                return _changeTracker;
            }
            set
            {
                if (_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging -= HandleObjectStateChanging;
                }
                _changeTracker = value;
                if (_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is deserializing.
        /// </summary>
        /// <value><c>true</c> if this instance is deserializing; otherwise, <c>false</c>.</value>
        protected bool IsDeserializing { get; private set; }

        /// <summary>
        /// Clears the navigation properties.
        /// </summary>
        protected virtual void ClearNavigationProperties()
        {
        }

        /// <summary>
        /// Handles the object state changing.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ObjectStateChangingEventArgs"/> instance containing the event data.</param>
        private void HandleObjectStateChanging(object sender, ObjectStateChangingEventArgs e)
        {
            if (e.NewState == ObjectState.Deleted)
            {
                ClearNavigationProperties();
            }
        }

        /// <summary>
        /// Called when [deserialized method].
        /// </summary>
        /// <param name="context">The context.</param>
        [OnDeserialized]
        private void OnDeserializedMethod(StreamingContext context)
        {
            IsDeserializing = false;
            ChangeTracker.ChangeTrackingEnabled = true;
        }

        /// <summary>
        /// Called when [deserializing method].
        /// </summary>
        /// <param name="context">The context.</param>
        [OnDeserializing]
        private void OnDeserializingMethod(StreamingContext context)
        {
            IsDeserializing = true;
        }

        #endregion ChangeTracking

        #region Notify Property Changed

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged { add { _propertyChanged += value; } remove { _propertyChanged -= value; } }

        /// <summary>
        /// Occurs when [_property changed].
        /// </summary>
        private event PropertyChangedEventHandler _propertyChanged;

        /// <summary>
        /// Called when [navigation property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnNavigationPropertyChanged(String propertyName)
        {
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(String propertyName)
        {
            if (ChangeTracker.State != ObjectState.Added && ChangeTracker.State != ObjectState.Deleted)
            {
                ChangeTracker.State = ObjectState.Modified;
            }
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Notify Property Changed

        #region ChangeTracking

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <value>The error.</value>
        /// <returns>An error message indicating what is wrong with this object. The default is an empty string ("").</returns>
        public string Error
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is ready to validate.
        /// </summary>
        /// <value><c>true</c> if this instance is ready to validate; otherwise, <c>false</c>.</value>
        public bool IsReadyToValidate
        {
            get { return _isReadyToValidate; }
            set { _isReadyToValidate = value; }
        }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>System.String.</returns>
        public string this[string columnName]
        {
            get
            {
                string errorMessage = null;
                if (_isReadyToValidate)
                {
                    dynamic value = this.GetType().GetProperty(columnName).GetValue(this, null);
                    List<String> errors = (from v in validators[columnName]
                                           where !v.IsValid(value)
                                           select Convert.ToString((string.IsNullOrEmpty(v.ErrorMessage) ? GetMessage(v) : v.ErrorMessage))).ToList();
                    if (errors != null && errors.Count > 0)
                    {
                        errorMessage = String.Join(Environment.NewLine, errors);
                    }
                }
                return errorMessage;
            }
        }

        /// <summary>
        /// Validates the specified validation results.
        /// </summary>
        /// <param name="validationResults">The validation results.</param>
        /// <returns>Boolean.</returns>
        public Boolean Validate(List<ValidationResult> validationResults)
        {
            dynamic vc = new ValidationContext(this, null, null);
            Boolean result = Validator.TryValidateObject(this, vc, validationResults, true);
            if (!result)
            {
                foreach (PropertyInfo prop in this.GetType().GetProperties())
                {
                    OnPropertyChanged(prop.Name);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns>System.String.</returns>
        private string GetMessage(ValidationAttribute v)
        {
            dynamic p = v.ErrorMessageResourceType.GetProperty(v.ErrorMessageResourceName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
            return p.GetValue(null, null);
        }

        /// <summary>
        /// Gets the validations.
        /// </summary>
        /// <param name="propertyLocal">The property local.</param>
        /// <returns>ValidationAttribute[][].</returns>
        private ValidationAttribute[] GetValidations(PropertyInfo propertyLocal)
        {
            return (ValidationAttribute[])propertyLocal.GetCustomAttributes(typeof(ValidationAttribute), true);
        }

        #endregion ChangeTracking
    }
}