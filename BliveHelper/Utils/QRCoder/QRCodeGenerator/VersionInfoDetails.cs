using System.Collections.Generic;

namespace BliveHelper.Utils.QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// Represents the detailed information about each error correction level and its corresponding capacities in different encoding modes for a specific version of a QR code.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="VersionInfoDetails"/> struct, detailing the error correction level and the capacity for each encoding mode.
    /// </remarks>
    /// <param name="errorCorrectionLevel">The error correction level, which determines how much of the code can be restored if the QR code gets damaged.</param>
    /// <param name="capacityDict">A dictionary mapping each encoding mode to its capacity for the specific error correction level.</param>
    private readonly struct VersionInfoDetails(ECCLevel errorCorrectionLevel, Dictionary<EncodingMode, int> capacityDict)
    {
        /// <summary>
        /// Gets the error correction level of the QR code, influencing how robust the QR code is against errors and damage.
        /// </summary>
        public ECCLevel ErrorCorrectionLevel { get; } = errorCorrectionLevel;

        /// <summary>
        /// Gets a dictionary that contains the capacities of different encoding modes under the specified error correction level.
        /// These capacities dictate how many characters can be encoded under each mode.
        /// </summary>
        public Dictionary<EncodingMode, int> CapacityDict { get; } = capacityDict;
    }
}