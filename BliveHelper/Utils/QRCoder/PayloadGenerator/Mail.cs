using System;
using System.Collections.Generic;
using System.Linq;

namespace BliveHelper.Utils.QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates an email payload that can be used to create a QR code for sending an email.
    /// </summary>
    /// <remarks>
    /// Creates an email payload with subject and message/text.
    /// </remarks>
    /// <param name="mailReceiver">Receiver's email address.</param>
    /// <param name="subject">Subject line of the email.</param>
    /// <param name="message">Message content of the email.</param>
    /// <param name="encoding">Payload encoding type. Choose dependent on your QR Code scanner app.</param>
    public class Mail(string mailReceiver = null, string subject = null, string message = null, Mail.MailEncoding encoding = Mail.MailEncoding.MAILTO) : Payload
    {
        private readonly string _mailReceiver = mailReceiver;
        private readonly string _subject = subject;
        private readonly string _message = message;
        private readonly MailEncoding _encoding = encoding;

        /// <summary>
        /// Returns the email payload as a string.
        /// </summary>
        /// <returns>The email payload as a string.</returns>
        public override string ToString()
        {
            switch (_encoding)
            {
                case MailEncoding.MAILTO:
                    var parts = new List<string>();
                    if (!string.IsNullOrEmpty(_subject))
                        parts.Add("subject=" + Uri.EscapeDataString(_subject));
                    if (!string.IsNullOrEmpty(_message))
                        parts.Add("body=" + Uri.EscapeDataString(_message));
                    var queryString = parts.Any() ? $"?{string.Join("&", parts)}" : "";
                    return $"mailto:{_mailReceiver}{queryString}";
                case MailEncoding.MATMSG:
                    return $"MATMSG:TO:{_mailReceiver};SUB:{EscapeInput(_subject ?? "")};BODY:{EscapeInput(_message ?? "")};;";
                case MailEncoding.SMTP:
                    return $"SMTP:{_mailReceiver}:{EscapeInput(_subject ?? "", true)}:{EscapeInput(_message ?? "", true)}";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Defines the encoding types for the email payload.
        /// </summary>
        public enum MailEncoding
        {
            /// <summary>
            /// Uses the "mailto:" URI scheme.
            /// </summary>
            MAILTO,

            /// <summary>
            /// Uses the "MATMSG:" format.
            /// </summary>
            MATMSG,

            /// <summary>
            /// Uses the "SMTP:" format.
            /// </summary>
            SMTP
        }
    }
}
