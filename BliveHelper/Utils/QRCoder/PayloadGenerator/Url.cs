using System;

namespace BliveHelper.Utils.QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates a URL payload for QR codes.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Url"/> class.
    /// </remarks>
    /// <param name="url">The target URL. If the protocol is not specified, the http protocol will be added.</param>
    public class Url(string url) : Payload
    {
        private readonly string _url = url;

        /// <summary>
        /// Returns the URL payload as a string.
        /// </summary>
        /// <returns>The URL payload as a string, ensuring it starts with "http://" if no protocol is specified.</returns>
        public override string ToString()
            => !_url.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? "http://" + _url : _url;
    }
}