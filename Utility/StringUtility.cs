using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Xml;
using System.Globalization;

namespace Utility
{
    public static class StringUtility
    {
        #region Replace Fields For DB INSERT
   
        /// <summary>
        /// this method is used to ReplaceSingleQuoteInString
        /// </summary>
        /// <param name="asValue"></param>
        /// <param name="abCanStringBeNull"></param>
        /// <returns></returns>
        public static string ReplaceSingleQuoteInString(string asValue, bool abCanStringBeNull)
        {
            if (abCanStringBeNull)
            {
                if (asValue == "" || asValue == null)
                    return "";
            }

            return asValue.Trim().Replace("'", "''");

        }

        /// <summary>
        ///  this method is used to ReplaceDefaultDateToNull
        /// </summary>
        /// <param name="adtCheckDate"></param>
        /// <returns></returns>
        public static string ReplaceDefaultDateToNull(DateTime adtCheckDate)
        {
            if (Convert.ToString(adtCheckDate) == "1/1/0001 12:00:00 AM")
                return null;
            else
                return adtCheckDate.ToString("yyyy-MM-dd",new CultureInfo("en"));
        }

        /// <summary>
        /// this method is used to GetBooleanValueForYOrN
        /// </summary>
        /// <param name="acValue"></param>
        /// <returns></returns>
        public static bool GetBooleanValueForYOrN(char acValue)
        {
	        // This function accepts parameter as acValue. It returns true if the character passed is 'Y'
            // otherwise returns false.
	        return acValue == Constants.C_YES;
        }

	    /// <summary>
        /// this method is used to GetYOrNoValueForBoolen
        /// </summary>
        /// <param name="abValue"></param>
        /// <returns></returns>
        public static char GetYOrNoValueForBoolen(bool abValue)
	    {
		    // This function accepts parameter as abValue. It returns 'Y' if the passed value is true
            // otherwise returns 'N'.
		    return abValue ? Constants.C_YES : Constants.C_NO;
	    }

	    #endregion
        
        #region -- ENCRYPTION RELATED --

        /// <summary>
        /// this method is used to EncryptString
        /// </summary>
        /// <param name="askSymmetricKey"></param>
        /// <param name="asValueToEncrypt"></param>
        /// <returns></returns>
        public static string EncryptString(SymmetricAlgorithm askSymmetricKey, String asValueToEncrypt)
        {
            //This method will encrypt the given string by using given symmetric key.

            string sBase64EncryptedString = string.Empty;            

            byte[] oArrDataToEncrypt = Encoding.Unicode.GetBytes(asValueToEncrypt);
            using (MemoryStream oMemoryStream = new MemoryStream())
            {
                CryptoStream csBase64 = new CryptoStream(oMemoryStream, new ToBase64Transform(), CryptoStreamMode.Write);
                CryptoStream csRijndael = new CryptoStream(csBase64, askSymmetricKey.CreateEncryptor(), CryptoStreamMode.Write);

                csRijndael.Write(oArrDataToEncrypt, 0, (int)oArrDataToEncrypt.Length);
                csRijndael.FlushFinalBlock();

                sBase64EncryptedString = Encoding.ASCII.GetString(oMemoryStream.GetBuffer(), 0, (int)oMemoryStream.Length);
            }
            return sBase64EncryptedString;
        }

        /// <summary>
        /// this method is used to DecryptString
        /// </summary>
        /// <param name="askSymmetricKey"></param>
        /// <param name="asValueToDecrypt"></param>
        /// <returns></returns>
        public static string DecryptString(SymmetricAlgorithm askSymmetricKey, String asValueToDecrypt)
        {
            //This method will decrypt the given string by using given symmetric key.

            string sUnEncryptedString = string.Empty;
            
            byte[] oArrDataToDecrypt = Convert.FromBase64String(asValueToDecrypt);
            using (MemoryStream oMemoryStream = new MemoryStream())
            {
                CryptoStream csRijndael = new CryptoStream(oMemoryStream, askSymmetricKey.CreateDecryptor(), CryptoStreamMode.Write);

                csRijndael.Write(oArrDataToDecrypt, 0, (int)oArrDataToDecrypt.Length);
                csRijndael.FlushFinalBlock();

                sUnEncryptedString = Encoding.Unicode.GetString(oMemoryStream.GetBuffer(), 0, (int)oMemoryStream.Length);
            }
            return sUnEncryptedString;
        }

        /// <summary>
        /// this method is used to GeneratePasswordKey
        /// </summary>
        /// <param name="asSymmetricKey"></param>
        /// <returns></returns>
        public static SymmetricAlgorithm GeneratePasswordKey(string asSymmetricKey)
        {
            //This method will genreate password specific symmetric key by using symmetric string.

            SymmetricAlgorithm skSymmKey = new RijndaelManaged();
            
            // Generate symmetric key using password and username as the salt
            byte[] oArrSaltValueBytes = Encoding.ASCII.GetBytes("RegulusIT");
            PasswordDeriveBytes pdPasswordKey = new PasswordDeriveBytes(asSymmetricKey, oArrSaltValueBytes,
                                                                        "SHA1", 1);
            skSymmKey.Key = pdPasswordKey.GetBytes(skSymmKey.KeySize / 8);
            skSymmKey.IV = pdPasswordKey.GetBytes(skSymmKey.BlockSize / 8);

            return skSymmKey;
        }

	    #endregion -- ENCRYPTION RELATED --
        
        #region -- CONVERSION RELATED --

	    /// <summary>
	    /// Encoding method for special chars
	    /// </summary>
	    /// <param name="asString"></param>
	    /// <param name="abConvertSpace"></param>
	    /// <returns></returns>
	    public static  string DoHTMLEncoding(string asString, bool abConvertSpace)
        {
            var osrcStrbuilder = new StringBuilder(asString);
            var oDestStrbuilder = new StringBuilder(asString.Length);
            for (int i = 0; i < osrcStrbuilder.Length; i++)
            {
                switch (osrcStrbuilder[i])
                {
                    case '?': oDestStrbuilder.Append("%3f"); break;
                    case '=': oDestStrbuilder.Append("%3D"); break;
                    case '\"': oDestStrbuilder.Append("%22"); break;
                    case '<': oDestStrbuilder.Append("%3C"); break;
                    case '>': oDestStrbuilder.Append("%3E"); break;
                    case '&': oDestStrbuilder.Append("%26"); break;
                    case '+': oDestStrbuilder.Append("%2B"); break;
                    case '#': oDestStrbuilder.Append("%23"); break;
                    case '%': oDestStrbuilder.Append("%25"); break;
                    case '*': oDestStrbuilder.Append("%2A"); break;
                    case '!': oDestStrbuilder.Append("%21"); break;
                    case ',': oDestStrbuilder.Append("%2C"); break;
                    case '\'': oDestStrbuilder.Append("%27"); break;
                    case '\\': oDestStrbuilder.Append("%5C"); break;
                    case '\n': oDestStrbuilder.Append("\n"); break;
                    case ' ':
		                oDestStrbuilder.Append(abConvertSpace ? "&nbsp;" : "%20");
		                break;    
                    default: oDestStrbuilder.Append(osrcStrbuilder[i]); break;
                }
            }

            return oDestStrbuilder.ToString();
        }


        /// <summary>
        /// Encoding method for special chars
        /// </summary>
        /// <param name="asString"></param>
        /// <param name="abConvertSpace"></param>
        /// <returns></returns>
        public static string UpdateSMSText(string asString)
        {
            var osrcStrbuilder = new StringBuilder(asString);
            var oDestStrbuilder = new StringBuilder(asString.Length);
            for (int i = 0; i < osrcStrbuilder.Length; i++)
            {
                //int iNumber = Convert.ToInt32(osrcStrbuilder[i]);
                int iNumber = (int)(osrcStrbuilder[i]);
                switch (iNumber)
                {   
                    case 8217: oDestStrbuilder.Append(((char)39)); break;
                    case 8211: oDestStrbuilder.Append(((char)45)); break;
                    default: oDestStrbuilder.Append(osrcStrbuilder[i]); break;
                }
            }

            return oDestStrbuilder.ToString();
        }

		/// <summary>
		///		Sanitizes the given string such that it can be converted into valid xml, skipping invalid xml chars.
		///		Returns null if xml conversion fails.
		/// </summary>
		/// <param name="asXML">The string to be sanitized.</param>
		/// <returns>System.String if conversion succeeds, null otherwise.</returns>
		public static string SanitizeXML(string asXML)
		{
			return SanitizeXML(asXML, false, false);
		}

	    /// <summary>
	    ///		Sanitizes the given string such that it can be converted into valid xml.
	    /// </summary>
	    /// <param name="asXML">The string to be sanitized.</param>
	    ///<param name="abEncodeInvalidChars">If true, invalid chars are encoded in the returned string, else they are skipped.</param>
	    ///<param name="abReturnOriginal">If true, will return the original string if xml sanitization fails, else will return null.</param>
	    ///<returns>System.String</returns>
	    public static string SanitizeXML(string asXML, bool abEncodeInvalidChars, bool abReturnOriginal)
		{
			try
			{
				try
				{
					return XmlConvert.VerifyXmlChars(asXML);
				}
				catch (XmlException)
				{
					return asXML.Aggregate(String.Empty, (s, c) => s + (XmlConvert.IsXmlChar(c) ? c.ToString() : abEncodeInvalidChars ? XmlConvert.EncodeName(c.ToString()) : String.Empty));
				}
			}
			catch (Exception)
			{
				return abReturnOriginal ? asXML : null;
			}
		}

        #endregion -- CONVERSION RELATED --
    }
}
