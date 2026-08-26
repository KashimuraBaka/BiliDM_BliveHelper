namespace BliveHelper.Utils.QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates a payload for Bitcoin Cash payment addresses.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="BitcoinCashAddress"/> class.
    /// Generates a Bitcoin Cash payment payload. QR Codes with this payload can open a payment app.
    /// </remarks>
    /// <param name="address">The Bitcoin Cash address of the payment receiver.</param>
    /// <param name="amount">The amount of Bitcoin Cash to transfer.</param>
    /// <param name="label">A reference label.</param>
    /// <param name="message">A reference text or message.</param>
    public class BitcoinCashAddress(string address, double? amount, string label = null, string message = null) 
        : BitcoinLikeCryptoCurrencyAddress(BitcoinLikeCryptoCurrencyType.BitcoinCash, address, amount, label, message)
    {
    }
}