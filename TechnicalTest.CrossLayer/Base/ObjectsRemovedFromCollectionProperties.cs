using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TechnicalTest.CrossLayer
{
    /// <summary>
    /// Class ObjectsRemovedFromCollectionProperties
    /// </summary>
    [CollectionDataContract(Name = "ObjectsRemovedFromCollectionProperties",
            ItemName = "DeletedObjectsForProperty", KeyName = "CollectionPropertyName", ValueName = "DeletedObjects")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ObjectsRemovedFromCollectionProperties : Dictionary<string, ObjectList> { }
}