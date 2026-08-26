namespace BliveHelper.Utils.QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates a Skype call payload
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SkypeCall"/> class.
    /// </remarks>
    /// <param name="skypeUsername">Skype username which will be called</param>
    public class SkypeCall(string skypeUsername) : Payload
    {
        private readonly string _skypeUsername = skypeUsername;

        /// <summary>
        /// Converts the Skype call payload to a string.
        /// </summary>
        /// <returns>A string representation of the Skype call payload.</returns>
        public override string ToString() => $"skype:{_skypeUsername}?call";
    }
}