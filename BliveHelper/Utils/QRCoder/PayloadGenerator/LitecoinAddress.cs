namespace BliveHelper.Utils.QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates a payload for Litecoin payment addresses.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="LitecoinAddress"/> class.
    /// Generates a Litecoin payment payload. QR Codes with this payload can open a payment app.
    /// </remarks>
    /// <param name="address">The Litecoin address of the payment receiver.</param>
    /// <param name="amount">The amount of Litecoin to transfer.</param>
    /// <param name="label">A reference label.</param>
    /// <param name="message">A reference text or message.</param>
    public class LitecoinAddress(string address, double? amount, string label = null, string message = null) 
        : BitcoinLikeCryptoCurrencyAddress(BitcoinLikeCryptoCurrencyType.Litecoin, address, amount, label, message)
    {
    }
}
