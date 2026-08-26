namespace BliveHelper.Utils.QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates a payload for Bitcoin payment addresses.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="BitcoinAddress"/> class.
    /// Generates a Bitcoin payment payload. QR Codes with this payload can open a payment app.
    /// </remarks>
    /// <param name="address">The Bitcoin address of the payment receiver.</param>
    /// <param name="amount">The amount of Bitcoin to transfer.</param>
    /// <param name="label">A reference label.</param>
    /// <param name="message">A reference text or message.</param>
    public class BitcoinAddress(string address, double? amount, string label = null, string message = null) 
        : BitcoinLikeCryptoCurrencyAddress(BitcoinLikeCryptoCurrencyType.Bitcoin, address, amount, label, message)
    {
    }
}