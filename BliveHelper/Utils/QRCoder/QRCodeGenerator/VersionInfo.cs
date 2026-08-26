using System.Collections.Generic;

namespace BliveHelper.Utils.QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// Represents version-specific information of a QR code.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="VersionInfo"/> struct with a specific version number and its details.
    /// </remarks>
    /// <param name="version">The version number of the QR code. Each version has a different module configuration.</param>
    /// <param name="versionInfoDetails">A list of detailed information related to error correction levels and capacity for each encoding mode.</param>
    private readonly struct VersionInfo(int version, List<VersionInfoDetails> versionInfoDetails)
    {
        /// <summary>
        /// Gets the version number of the QR code. Each version number specifies a different size of the QR matrix.
        /// </summary>
        public int Version { get; } = version;

        /// <summary>
        /// Gets a list of details about the QR code version, including the error correction levels and encoding capacities.
        /// </summary>
        public List<VersionInfoDetails> Details { get; } = versionInfoDetails;
    }
}