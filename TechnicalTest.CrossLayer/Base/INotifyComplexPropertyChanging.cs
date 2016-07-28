using System;

namespace TechnicalTest.CrossLayer
{
    // An interface that provides an event that fires when complex properties change.
    // Changes can be the replacement of a complex property with a new complex type instance or
    // a change to a scalar property within a complex type instance.
    /// <summary>
    /// Interface INotifyComplexPropertyChanging
    /// </summary>
    public interface INotifyComplexPropertyChanging
    {
        /// <summary>
        /// Occurs when [complex property changing].
        /// </summary>
        event EventHandler ComplexPropertyChanging;
    }
}