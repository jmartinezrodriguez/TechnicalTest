using System;

namespace TechnicalTest.CrossLayer
{
    /// <summary>
    /// Class EqualityComparer
    /// </summary>
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public static class EqualityComparer
    {
        // Helper method to determine if two byte arrays are the same value even if they are different object references
        /// <summary>
        /// Binaries the equals.
        /// </summary>
        /// <param name="binaryValue1">The binary value1.</param>
        /// <param name="binaryValue2">The binary value2.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public static bool BinaryEquals(object binaryValue1, object binaryValue2)
        {
            if (Object.ReferenceEquals(binaryValue1, binaryValue2))
            {
                return true;
            }

            byte[] array1 = binaryValue1 as byte[];
            byte[] array2 = binaryValue2 as byte[];

            if (array1 != null && array2 != null)
            {
                if (array1.Length != array2.Length)
                {
                    return false;
                }

                for (int i = 0; i < array1.Length; i++)
                {
                    if (array1[i] != array2[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}