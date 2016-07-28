using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TechnicalTest.CrossLayer
{
    /// <summary>
    /// Class ObjectsAddedToCollectionProperties
    /// </summary>
    [CollectionDataContract(Name = "ObjectsAddedToCollectionProperties",
            ItemName = "AddedObjectsForProperty", KeyName = "CollectionPropertyName", ValueName = "AddedObjects")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ObjectsAddedToCollectionProperties : Dictionary<string, ObjectList> { }
}