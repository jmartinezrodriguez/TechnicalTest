
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TechnicalTest.CrossLayer
{
    /// <summary>
    /// Class OriginalValuesDictionary
    /// </summary>
    [CollectionDataContract(Name = "OriginalValuesDictionary",
            ItemName = "OriginalValues", KeyName = "Name", ValueName = "OriginalValue")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class OriginalValuesDictionary : Dictionary<string, Object> { }
}