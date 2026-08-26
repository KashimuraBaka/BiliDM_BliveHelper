using System;

namespace BliveHelper.Utils.QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates a vCard or meCard contact dataset.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ContactData"/> class.
    /// </remarks>
    /// <param name="outputType">Payload output type.</param>
    /// <param name="firstname">The first name.</param>
    /// <param name="lastname">The last name.</param>
    /// <param name="nickname">The display name.</param>
    /// <param name="phone">Normal phone number.</param>
    /// <param name="mobilePhone">Mobile phone.</param>
    /// <param name="workPhone">Office phone number.</param>
    /// <param name="email">E-Mail address.</param>
    /// <param name="birthday">Birthday.</param>
    /// <param name="website">Website / Homepage.</param>
    /// <param name="street">Street.</param>
    /// <param name="houseNumber">House number.</param>
    /// <param name="city">City.</param>
    /// <param name="stateRegion">State or Region.</param>
    /// <param name="zipCode">Zip code.</param>
    /// <param name="country">Country.</param>
    /// <param name="addressOrder">The address order format to use.</param>
    /// <param name="note">Memo text / notes.</param>            
    /// <param name="org">Organization/Company.</param>            
    /// <param name="orgTitle">Organization/Company Title.</param> 
    public class ContactData(
        ContactData.ContactOutputType outputType,
        string firstname,
        string lastname,
        string nickname = null,
        string phone = null,
        string mobilePhone = null,
        string workPhone = null,
        string email = null,
        DateTime? birthday = null,
        string website = null,
        string street = null,
        string houseNumber = null,
        string city = null,
        string zipCode = null,
        string country = null,
        string note = null,
        string stateRegion = null,
        ContactData.AddressOrder addressOrder = ContactData.AddressOrder.Default,
        string org = null,
        string orgTitle = null) : Payload
    {
        private readonly string _firstname = firstname;
        private readonly string _lastname = lastname;
        private readonly string _nickname = nickname;
        private readonly string _org = org;
        private readonly string _orgTitle = orgTitle;
        private readonly string _phone = phone;
        private readonly string _mobilePhone = mobilePhone;
        private readonly string _workPhone = workPhone;
        private readonly string _email = email;
        private readonly DateTime? _birthday = birthday;
        private readonly string _website = website;
        private readonly string _street = street;
        private readonly string _houseNumber = houseNumber;
        private readonly string _city = city;
        private readonly string _zipCode = zipCode;
        private readonly string _stateRegion = stateRegion;
        private readonly string _country = country;
        private readonly string _note = note;
        private readonly ContactOutputType _outputType = outputType;
        private readonly AddressOrder _addressOrder = addressOrder;

        /// <summary>
        /// Returns a string representation of the contact data payload.
        /// </summary>
        /// <returns>A string representation of the contact data in the specified format.</returns>
        public override string ToString()
        {
            string payload = string.Empty;
            if (_outputType == ContactOutputType.MeCard)
            {
                payload += "MECARD+\r\n";
                if (!string.IsNullOrEmpty(_firstname) && !string.IsNullOrEmpty(_lastname))
                    payload += $"N:{_lastname}, {_firstname}\r\n";
                else if (!string.IsNullOrEmpty(_firstname) || !string.IsNullOrEmpty(_lastname))
                    payload += $"N:{_firstname}{_lastname}\r\n";
                if (!string.IsNullOrEmpty(_org))
                    payload += $"ORG:{_org}\r\n";
                if (!string.IsNullOrEmpty(_orgTitle))
                    payload += $"TITLE:{_orgTitle}\r\n";
                if (!string.IsNullOrEmpty(_phone))
                    payload += $"TEL:{_phone}\r\n";
                if (!string.IsNullOrEmpty(_mobilePhone))
                    payload += $"TEL:{_mobilePhone}\r\n";
                if (!string.IsNullOrEmpty(_workPhone))
                    payload += $"TEL:{_workPhone}\r\n";
                if (!string.IsNullOrEmpty(_email))
                    payload += $"EMAIL:{_email}\r\n";
                if (!string.IsNullOrEmpty(_note))
                    payload += $"NOTE:{_note}\r\n";
                if (_birthday != null)
                    payload += $"BDAY:{(DateTime)_birthday:yyyyMMdd}\r\n";
                string addressString;
                if (_addressOrder == AddressOrder.Default)
                {
                    addressString = $"ADR:,,{(!string.IsNullOrEmpty(_street) ? _street + " " : "")}{(!string.IsNullOrEmpty(_houseNumber) ? _houseNumber : "")},{(!string.IsNullOrEmpty(_zipCode) ? _zipCode : "")},{(!string.IsNullOrEmpty(_city) ? _city : "")},{(!string.IsNullOrEmpty(_stateRegion) ? _stateRegion : "")},{(!string.IsNullOrEmpty(_country) ? _country : "")}\r\n";
                }
                else
                {
                    addressString = $"ADR:,,{(!string.IsNullOrEmpty(_houseNumber) ? _houseNumber + " " : "")}{(!string.IsNullOrEmpty(_street) ? _street : "")},{(!string.IsNullOrEmpty(_city) ? _city : "")},{(!string.IsNullOrEmpty(_stateRegion) ? _stateRegion : "")},{(!string.IsNullOrEmpty(_zipCode) ? _zipCode : "")},{(!string.IsNullOrEmpty(_country) ? _country : "")}\r\n";
                }
                payload += addressString;
                if (!string.IsNullOrEmpty(_website))
                    payload += $"URL:{_website}\r\n";
                if (!string.IsNullOrEmpty(_nickname))
                    payload += $"NICKNAME:{_nickname}\r\n";
                payload = payload.Trim(['\r', '\n']);
            }
            else
            {
                var version = _outputType.ToString().Substring(5);
                if (version.Length > 1)
                    version = version.Insert(1, ".");
                else
                    version += ".0";

                payload += "BEGIN:VCARD\r\n";
                payload += $"VERSION:{version}\r\n";

                payload += $"N:{(!string.IsNullOrEmpty(_lastname) ? _lastname : "")};{(!string.IsNullOrEmpty(_firstname) ? _firstname : "")};;;\r\n";
                payload += $"FN:{(!string.IsNullOrEmpty(_firstname) ? _firstname + " " : "")}{(!string.IsNullOrEmpty(_lastname) ? _lastname : "")}\r\n";
                if (!string.IsNullOrEmpty(_org))
                {
                    payload += $"ORG:" + _org + "\r\n";
                }
                if (!string.IsNullOrEmpty(_orgTitle))
                {
                    payload += $"TITLE:" + _orgTitle + "\r\n";
                }
                if (!string.IsNullOrEmpty(_phone))
                {
                    payload += $"TEL;";
                    if (_outputType == ContactOutputType.VCard21)
                        payload += $"HOME;VOICE:{_phone}";
                    else if (_outputType == ContactOutputType.VCard3)
                        payload += $"TYPE=HOME,VOICE:{_phone}";
                    else
                        payload += $"TYPE=home,voice;VALUE=uri:tel:{_phone}";
                    payload += "\r\n";
                }

                if (!string.IsNullOrEmpty(_mobilePhone))
                {
                    payload += $"TEL;";
                    if (_outputType == ContactOutputType.VCard21)
                        payload += $"HOME;CELL:{_mobilePhone}";
                    else if (_outputType == ContactOutputType.VCard3)
                        payload += $"TYPE=HOME,CELL:{_mobilePhone}";
                    else
                        payload += $"TYPE=home,cell;VALUE=uri:tel:{_mobilePhone}";
                    payload += "\r\n";
                }

                if (!string.IsNullOrEmpty(_workPhone))
                {
                    payload += $"TEL;";
                    if (_outputType == ContactOutputType.VCard21)
                        payload += $"WORK;VOICE:{_workPhone}";
                    else if (_outputType == ContactOutputType.VCard3)
                        payload += $"TYPE=WORK,VOICE:{_workPhone}";
                    else
                        payload += $"TYPE=work,voice;VALUE=uri:tel:{_workPhone}";
                    payload += "\r\n";
                }


                payload += "ADR;";
                if (_outputType == ContactOutputType.VCard21)
                    payload += "HOME;PREF:";
                else if (_outputType == ContactOutputType.VCard3)
                    payload += "TYPE=HOME,PREF:";
                else
                    payload += "TYPE=home,pref:";
                string addressString;
                if (_addressOrder == AddressOrder.Default)
                {
                    addressString = $";;{(!string.IsNullOrEmpty(_street) ? _street + " " : "")}{(!string.IsNullOrEmpty(_houseNumber) ? _houseNumber : "")};{(!string.IsNullOrEmpty(_zipCode) ? _zipCode : "")};{(!string.IsNullOrEmpty(_city) ? _city : "")};{(!string.IsNullOrEmpty(_stateRegion) ? _stateRegion : "")};{(!string.IsNullOrEmpty(_country) ? _country : "")}\r\n";
                }
                else
                {
                    addressString = $";;{(!string.IsNullOrEmpty(_houseNumber) ? _houseNumber + " " : "")}{(!string.IsNullOrEmpty(_street) ? _street : "")};{(!string.IsNullOrEmpty(_city) ? _city : "")};{(!string.IsNullOrEmpty(_stateRegion) ? _stateRegion : "")};{(!string.IsNullOrEmpty(_zipCode) ? _zipCode : "")};{(!string.IsNullOrEmpty(_country) ? _country : "")}\r\n";
                }
                payload += addressString;

                if (_birthday != null)
                    payload += $"BDAY:{(DateTime)_birthday:yyyyMMdd}\r\n";
                if (!string.IsNullOrEmpty(_website))
                    payload += $"URL:{_website}\r\n";
                if (!string.IsNullOrEmpty(_email))
                    payload += $"EMAIL:{_email}\r\n";
                if (!string.IsNullOrEmpty(_note))
                    payload += $"NOTE:{_note}\r\n";
                if (_outputType != ContactOutputType.VCard21 && !string.IsNullOrEmpty(_nickname))
                    payload += $"NICKNAME:{_nickname}\r\n";

                payload += "END:VCARD";
            }

            return payload;
        }

        /// <summary>
        /// Possible output types. Either vCard 2.1, vCard 3.0, vCard 4.0 or MeCard.
        /// </summary>
        public enum ContactOutputType
        {
            /// <summary>
            /// MeCard output type.
            /// </summary>
            MeCard,

            /// <summary>
            /// vCard 2.1 output type.
            /// </summary>
            VCard21,

            /// <summary>
            /// vCard 3.0 output type.
            /// </summary>
            VCard3,

            /// <summary>
            /// vCard 4.0 output type.
            /// </summary>
            VCard4
        }


        /// <summary>
        /// Define the address format.
        /// Default: European format, ([Street] [House Number] and [Postal Code] [City])
        /// Reversed: North American and others format ([House Number] [Street] and [City] [Postal Code])
        /// </summary>
        public enum AddressOrder
        {
            /// <summary>
            /// Default address order format (European format).
            /// </summary>
            Default,

            /// <summary>
            /// Reversed address order format (North American and others format).
            /// </summary>
            Reversed
        }
    }
}
