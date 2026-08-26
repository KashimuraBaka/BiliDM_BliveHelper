namespace BliveHelper.Utils.QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates a bookmark payload. When scanned by a QR code reader, this creates a browser bookmark.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Bookmark"/> class.
    /// </remarks>
    /// <param name="url">The URL of the bookmark.</param>
    /// <param name="title">The title of the bookmark.</param>
    public class Bookmark(string url, string title) : Payload
    {
        private string Url { get; } = EscapeInput(url);
        private string Title { get; } = EscapeInput(title);

        /// <summary>
        /// Returns a string representation of the bookmark payload.
        /// </summary>
        /// <returns>A string representation of the bookmark payload in the MEBKM format.</returns>
        public override string ToString() => $"MEBKM:TITLE:{Title};URL:{Url};;";
    }
}