namespace TechnicalTest.CrossLayer
{
    // The interface is implemented by the self tracking entities that EF will generate.
    // We will have an Adapter that converts this interface to the interface that the EF expects.
    // The Adapter will live on the server side.
    /// <summary>
    /// Interface IObjectWithChangeTracker
    /// </summary>
    public interface IObjectWithChangeTracker
    {
        // Has all the change tracking information for the subgraph of a given object.
        /// <summary>
        /// Gets the change tracker.
        /// </summary>
        /// <value>The change tracker.</value>
        ObjectChangeTracker ChangeTracker { get; }
    }
}