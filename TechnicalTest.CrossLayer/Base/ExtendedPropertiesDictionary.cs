using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TechnicalTest.CrossLayer
{
    /// <summary>
    /// Class ExtendedPropertiesDictionary
    /// </summary>
    [CollectionDataContract(Name = "ExtendedPropertiesDictionary",
            ItemName = "ExtendedProperties", KeyName = "Name", ValueName = "ExtendedProperty")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ExtendedPropertiesDictionary : Dictionary<string, Object> { }
}