using System;

namespace TechnicalTest.CrossLayer
{
    #region EnumForObjectState

    /// <summary>
    /// Enum ObjectState
    /// </summary>
    [Flags]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public enum ObjectState
    {
        /// <summary>
        /// The unchanged
        /// </summary>
        Unchanged = 0x1,

        /// <summary>
        /// The added
        /// </summary>
        Added = 0x2,

        /// <summary>
        /// The modified
        /// </summary>
        Modified = 0x4,

        /// <summary>
        /// The deleted
        /// </summary>
        Deleted = 0x8
    }

    #endregion EnumForObjectState
}