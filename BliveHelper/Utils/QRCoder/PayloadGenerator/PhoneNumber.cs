namespace BliveHelper.Utils.QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates a phone call payload.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="PhoneNumber"/> class.
    /// </remarks>
    /// <param name="number">Phone number of the receiver.</param>
    public class PhoneNumber(string number) : Payload
    {
        private readonly string _number = number;

        /// <summary>
        /// Returns the phone call payload as a string.
        /// </summary>
        /// <returns>The phone call payload as a string.</returns>
        public override string ToString() => $"tel:{_number}";
    }
}