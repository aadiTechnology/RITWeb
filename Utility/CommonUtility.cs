/**
 *  File Name : CommonUtility.cs
 *  Purpose   : This class contains methods that provieds the common functionality required for porject.
 *  Date      : 17-04-2007.
 */


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using System.Resources;
using System.Collections;
using System.Threading;
using System.Configuration;

namespace Utility
{
	public class CommonUtility
	{
        public static Hashtable htMarathi = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
        public static Hashtable htEnglish = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
		#region Password Encription

		public static string GetEncryptedPassword(String asKeyString, String asPassword)
		{
			//This method will encrypt the user password and returns the same.

			SymmetricAlgorithm skSymKey = StringUtility.GeneratePasswordKey(asKeyString.ToLower());

			return StringUtility.EncryptString(skSymKey, asPassword);
		}

		public static string GetDecryptedPassword(String asKeyString, String asPassword)
		{
			//This method will decrypt the user password and returns the same.

			SymmetricAlgorithm skSymKey = StringUtility.GeneratePasswordKey(asKeyString.ToLower());

			return StringUtility.DecryptString(skSymKey, asPassword);
		}

		#endregion

		#region Header Image Display

		// This is a helper method used to determine the index of the
		// column being sorted. If no column is being sorted, -1 is returned.
		public static int GetSortColumnIndex(GridView aoGridview, string asSortExpression)
		{
			if (asSortExpression.Trim().Equals(""))
				return -1;
			// Iterate through the Columns collection to determine the index
			// of the column being sorted.
			foreach (DataControlField field in aoGridview.Columns)
			{
				asSortExpression = asSortExpression.Replace(" ", "").Replace("asc", "").Replace("desc", "");
				if (field.SortExpression == asSortExpression)
					return aoGridview.Columns.IndexOf(field);
			}

			return -1;
		}

		// This is a helper method used to determine the index of the column being sorted.
		// and add a sort direction image to the header of the column being sorted. 
		// for listview
		public static void AddSortImage(HtmlTableRow aoHtmlTableRow, string asSortExpression, string asSortDirection)
		{
			if (asSortExpression.Trim().Equals(""))
				return;

			// Create the sorting image based on the sort direction.
			var sortImage = new Image();
			if (asSortDirection == "asc")
			{
				sortImage.ImageUrl = "~/RITeSchool/images/up.gif";
				sortImage.AlternateText = "Ascending Order";
			}
			else if (asSortDirection == "desc")
			{
				sortImage.ImageUrl = "~/RITeSchool/images/down.gif";
				sortImage.AlternateText = "Descending Order";
			}
			// Iterate through the Columns collection to determine the index
			// of the column being sorted.
			foreach (HtmlTableCell oHtmlTableCell in aoHtmlTableRow.Cells)
			{
				asSortExpression = asSortExpression.Replace(" ", "").Replace("asc", "").Replace("desc", "");

				// Iterate through the cells collection to determine the index
				// of the cell being sorted.
				foreach (Control oControl in oHtmlTableCell.Controls)
				{
					var oLinkButton = oControl as LinkButton;
					if (oLinkButton != null && oLinkButton.CommandArgument == asSortExpression)
					{
						// Add the image to the appropriate header cell.
						if (sortImage.ImageUrl != "")
						{
							oHtmlTableCell.Controls.Add(sortImage);
							break;
						}
					}
				}
			}
		}

		// This is a helper method used to add a sort direction
		// image to the header of the column being sorted.
		public static void AddSortImage(int columnIndex, GridViewRow headerRow, string asSortDirection)
		{
			// Create the sorting image based on the sort direction.
			var sortImage = new Image();
			if (asSortDirection == "asc")
			{
				sortImage.ImageUrl = "~/RITeSchool/images/up.gif";
				sortImage.AlternateText = "Ascending Order";
			}
			else if (asSortDirection == "desc")
			{
				sortImage.ImageUrl = "~/RITeSchool/images/down.gif";
				sortImage.AlternateText = "Descending Order";
			}

			// Add the image to the appropriate header cell.
			if (headerRow.Cells[columnIndex].Controls.Count < 2 && sortImage.ImageUrl != "")
				headerRow.Cells[columnIndex].Controls.Add(sortImage);
		}

		// This is a helper method used to add a sort direction
		// image to the header of the column being sorted.
		public static void AddSortImage(int columnIndex, GridViewRow headerRow, SortDirection aSortDirection)
		{
			// Create the sorting image based on the sort direction.
			var sortImage = new Image();

			// if (asSortDirection == SortDirection.Ascending.ToString())

			if (SortDirection.Ascending.Equals(aSortDirection))
			{
				sortImage.ImageUrl = "~/RITeSchool/images/up.gif";
				sortImage.AlternateText = "Ascending Order";
			}
			else if (SortDirection.Descending.Equals(aSortDirection))
			{
				sortImage.ImageUrl = "~/RITeSchool/images/down.gif";
				sortImage.AlternateText = "Descending Order";
			}

			// Add the image to the appropriate header cell.
			if (headerRow.Cells[columnIndex].Controls.Count < 2 && sortImage.ImageUrl != "")
				headerRow.Cells[columnIndex].Controls.Add(sortImage);
		}

		#endregion

		#region -- EMAIL RELATED --

		/// <summary>
		///		Sends an email to the specified recipients.
		/// </summary>
		/// <param name="asToMailAddress">Email address of recipient (comma separated if multiple).</param>
		/// <param name="asFromMailAddress">Email address of person sending the mail.</param>
		/// <param name="asSubject">Subject line of the email.</param>
		/// <param name="asBodyText">Contents of the email.</param>
		public static void SendE_Mail(string asToMailAddress, string asFromMailAddress, string asSubject, string asBodyText)
		{
			//This method sends e-mail to provided emailaddress with subject and bodytext.
			//This method takes parameter asToMailAddress,asSubject,asBodyText. Which refers to
			// - asToMailAddress -- E-mail address to which e-mail has to be send.
			// - asSubject       -- E-mail subject.
			// - asBodyText      -- E-mail body text.

			if (Constants.SENDMAIL != Constants.S_YES)
				return;

			if (String.IsNullOrEmpty(asFromMailAddress))
				asFromMailAddress = asToMailAddress;
			
			using (var oMessage = new MailMessage())
			using (var oMail = new SmtpClient(Constants.S_IP_ADDRESS_SMTP, Constants.S_PORT_NUMBER_SMTP.ToInt()))
			{
				oMessage.From = new MailAddress(asFromMailAddress);
				
				foreach (string sEmail in asToMailAddress.Split(','))
					oMessage.To.Add(new MailAddress(sEmail));
				
				oMessage.Subject = asSubject;
				oMessage.IsBodyHtml = true;
				oMessage.Body = asBodyText;
				oMail.Send(oMessage);
			}
		}

		/// <summary>
		/// 	This method is used to send a mail.
		/// </summary>
		/// <param name="asToMailAddress"> </param>
		/// <param name="asFromMailAddress"> </param>
		/// <param name="asSubject"> </param>
		/// <param name="asMailText"> </param>
		/// <param name="aoArrayListAttachments"> </param>
		public static void SendMail(string asToMailAddress, string asFromMailAddress, string asSubject, string asMailText, string asFileName)
		{
			//This method sends e-mail to provided emailaddress with subject and bodytext.
			//This method takes parameter asToMailAddress,asSubject,asBodyText. Which refers to
			// - asToMailAddress -- E-mail address to which e-mail has to be send.
			// - asSubject       -- E-mail subject.
			// - asBodyText      -- E-mail body text.

			SendMail(asToMailAddress, asFromMailAddress, asSubject, asMailText, asFileName, MailPriority.Normal);
		}

		/// <summary>
		/// 	This method is used to send a mail.
		/// </summary>
		/// <param name="asToMailAddress"> </param>
		/// <param name="asFromMailAddress"> </param>
		/// <param name="asSubject"> </param>
		/// <param name="asMailText"> </param>
		/// <param name="aoArrayListAttachments"> </param>
		public static void SendMail(string asToMailAddress, string asFromMailAddress, string asSubject, string asMailText, string asFileName, MailPriority aoMailPriority)
		{
			//This method sends e-mail to provided emailaddress with subject and bodytext.
			//This method takes parameter asToMailAddress,asSubject,asBodyText. Which refers to
			// - asToMailAddress -- E-mail address to which e-mail has to be send.
			// - asSubject       -- E-mail subject.
			// - asBodyText      -- E-mail body text.

			char sSendMail;
			sSendMail = Convert.ToChar(Constants.SENDMAIL);
			var oMessage = new MailMessage();
			var oMail = new SmtpClient(Constants.S_IP_ADDRESS_SMTP);
			oMail.Port = Convert.ToInt32(Constants.S_PORT_NUMBER_SMTP);

			//var oToEMailId = new MailAddress(asToMailAddress);
			if (string.IsNullOrEmpty(asFromMailAddress))
				asFromMailAddress = asToMailAddress.Split(',')[0];
			var oFromEMailId = new MailAddress(asFromMailAddress);

			oMessage.From = oFromEMailId;

            foreach (string sEmail in asToMailAddress.Split(','))
                oMessage.To.Add(sEmail);	
		
			oMessage.Subject = asSubject;
			oMessage.Priority = aoMailPriority;

            if (!string.IsNullOrEmpty(asFileName))
            {
                //oMessage.Attachments.Add(new Attachment(asFileName));
                foreach(string sFile in asFileName.Split(','))
                    oMessage.Attachments.Add(new Attachment(sFile));
            }

			oMessage.IsBodyHtml = true;
			oMessage.Body = asMailText;
			if (sSendMail == Constants.C_YES)
				oMail.Send(oMessage);
			oMessage.Dispose();
			oMail = null;
		}

		#endregion -- EMAIL RELATED --

		/// <summary>
		/// 	This method is used to delete any empty rows present in the table.
		/// </summary>
		/// <param name="aoDTDetails"> </param>
		/// <returns> </returns>
		public static DataTable DeleteEmptyRows(DataTable aoDTDetails)
		{
			for (int iRowIndex = 0; iRowIndex < aoDTDetails.Rows.Count; iRowIndex++)
			{
				bool bDeleteRow = true;
				for (int iColumnIndex = 0; iColumnIndex < aoDTDetails.Columns.Count; iColumnIndex++)
				{
					if (!aoDTDetails.Rows[iRowIndex][iColumnIndex].ToString().Trim().IsNullOrEmpty())
					{
						bDeleteRow = false;
						break;
					}
				}

				if (bDeleteRow)
				{
					aoDTDetails.Rows[iRowIndex].Delete();
					aoDTDetails.AcceptChanges();
					iRowIndex--;
				}
			}

			return aoDTDetails;
		}

		#region Read records from excel sheet into dataset

		/// <summary>
		/// </summary>
		/// <param name="asFQNFileName"> </param>
		/// <param name="asLogin"> </param>
		/// <param name="asPassword"> </param>
		/// <returns> </returns>
		public static DataSet ReadExcelSheetAndFetchData(string asFQNFileName, string asPassword, string asTableName)
		{
			//string sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + asFQNFileName + ";Extended Properties=\"Excel 12.0;\"";
			// Connectionstring is modified to support xls as well as xlsx.
			string sConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + asFQNFileName + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";

			OleDbConnection oCNRecords = null;
			OleDbDataAdapter oDARecords = null;
			DataSet oDSRecords = null;
			try
			{
				oCNRecords = new OleDbConnection(sConnectionString);
				string sSeletcStatement = string.Format("SELECT * FROM [" + asTableName + "$]");
				oDARecords = new OleDbDataAdapter(sSeletcStatement, oCNRecords);
				oDSRecords = new DataSet("Student Data");
				//oDSRecords.Tables[0].Rows[4].ItemArray[0].ToString()
				oDARecords.Fill(oDSRecords);
				oCNRecords.Close();
			}
			catch (Exception)
			{
			}
			finally
			{
			}
			return oDSRecords;
		}

		#endregion

		#region Utility to convert a number into its words

		private static string[] US = new string[1003];
		private static string[] SNu = new string[20];
		private static string[] DNu = new string[22];
		private static string[] SNt = new string[10];

		public static string GetNumberInWords(string asNumber)
		{
			string Number;
			string deciml;
			string _number;
			string _deciml;

			string currency = "Rupees ";
			string _currency = " Paise Only";
			string sOutput = "";
			if (Convert.ToDouble(asNumber) == 0)
				sOutput = "Null Value";
			if (Convert.ToDouble(asNumber) < 0)
				sOutput = "Invalid Value";

			string sNumber = asNumber;
			if (!sNumber.Contains("."))
				sNumber += ".00";
			string[] no = sNumber.Split('.');

			if ((no[0] != null) && (no[1] != "00"))
			{
				Number = no[0];
				deciml = no[1];
				_number = RupeeInIndianFormat(Number);
				_deciml = GetPaiseInWords(deciml);
				sOutput = currency + _number.Trim() + " and " + _deciml.Trim() + _currency;
			}
			if ((no[0] != null) && (no[1] == "00"))
			{
				Number = no[0];
				_number = RupeeInIndianFormat(Number);
				sOutput = currency + _number + " Only";
			}
			if ((Convert.ToDouble(no[0]) == 0) && (no[1] != null))
			{
				deciml = no[1];
				_deciml = GetPaiseInWords(deciml);
				sOutput = _deciml + _currency;
			}
			return sOutput;
		}

		/// <summary>
		/// 	This function is used to get date in words.
		/// </summary>
		/// <param name="asText"> </param>
		/// <returns> </returns>
		public static string GetDateInWords(DateTime dtBirthDate)
		{
			string sDay = "";
			string sMonth = "";
			string sYear = "";
			string sBirthDate = "";
			string sDayVal = "";
			int iYear = 0;

			if (Convert.ToString(dtBirthDate.Day).Length == 1)
				sDayVal = "0" + dtBirthDate.Day;
			else
				sDayVal = Convert.ToString(dtBirthDate.Day);
			iYear = dtBirthDate.Year;
			sBirthDate = dtBirthDate.ToString(sDayVal + "-MMMM-" + iYear);

			string[] sArrDate = sBirthDate.Split('-');

			Initialize();

			sDay = GroupNumberAboveHundred(sArrDate[0]);
			sMonth = sArrDate[1];

			sYear = GetYearInWords(sArrDate[2]);

			sBirthDate = sDay + " " + sMonth + " " + sYear;

			return sBirthDate.Trim();
		}

		/// <summary>
		/// 	Converts a Date of Birth into words.
		/// </summary>
		/// <param name="adtDOB"> A DateTime object representing the Date of Birth </param>
		/// <returns> String representing the Date of Birth in words </returns>
		public static string GetDOBInWords(DateTime adtDOB)
		{
			Initialize(); // Initialize the Number Map

			var sbOutput = new StringBuilder();
			string sSeparator = " ";

			sbOutput.Append(GetDayOfBirth(adtDOB.Day));
			sbOutput.Append(sSeparator);
			sbOutput.Append(adtDOB.ToString("MMMM"));
			sbOutput.Append(sSeparator);
			sbOutput.Append(GetYearInWords(adtDOB.Year.ToString()));

			return sbOutput.ToString();
		}

		/// <summary>
		/// 	Converts a number into words, for use in Date of Birth
		/// </summary>
		/// <param name="asNumber"> The Number to convert </param>
		/// <returns> String representing the Number in words </returns>
		private static string GetDayOfBirth(int aiNumber)
		{
			string sOutput = String.Empty;
			string sSeparator = " ";

			if (aiNumber == 20)
				sOutput = DNu[20];
			else if (aiNumber == 30)
				sOutput = DNu[21];
			else if (aiNumber > 30)
				sOutput = SNt[3] + sSeparator + DNu[1];
			else if (aiNumber > 20)
				sOutput = SNt[2] + sSeparator + DNu[aiNumber % 20];
			else
				sOutput = DNu[aiNumber];

			return sOutput;
		}

		private static string RupeeInIndianFormat(string Number)
		{
			string NumberAboveHundred, NumberBelowHundred, OutputOfNumberAboveHundred, OutputOfNumberBelowHundred;
			Initialize();
			Number = Number.PadLeft((Number.Length + (3 - (Number.Length % 3))), '0');
			NumberBelowHundred = Number.Substring(Number.Length - 3);

			OutputOfNumberBelowHundred = Group(NumberBelowHundred);
			OutputOfNumberAboveHundred = "";
			if (Number.Length > 3)
			{
				NumberAboveHundred = Number.Substring(0, Number.Length - 3);
				OutputOfNumberAboveHundred = NameOfNumber(NumberAboveHundred);
			}
			string Output = OutputOfNumberAboveHundred + " " + OutputOfNumberBelowHundred;
			return Output;
		}

		private static string GetPaiseInWords(string Number)
		{
			string GroupName = "";
			string OutPut = GroupNumberAboveHundred(Number.Trim()) + " " + GroupName + " ";
			return OutPut;
		}

		private static string NameOfNumber(string Number)
		{
			string GroupName = "";
			string OutPut = "";
			Initialize();
			if ((Number.Length % 2) != 0)
				Number = Number.PadLeft((Number.Length + (2 - (Number.Length % 2))), '0');
			var Array = new string[Number.Length / 2];
			Int16 Element = -1;
			Int32 DisplayCount = -1;
			bool LimitGroupsShowAll = false;
			int LimitGroups = 0;
			bool GroupToWords = true;
			for (Int16 Count = 0; Count <= Number.Length - 2; Count += 2)
			{
				Element += 1;
				Array[Element] = Number.Substring(Count, 2);
			}

			if (LimitGroups == 0)
				LimitGroupsShowAll = true;

			for (Int16 Count = 0; (Count <= ((Number.Length / 2) - 1)); Count++)
			{
				DisplayCount++;
				if (((DisplayCount < LimitGroups) || LimitGroupsShowAll))
				{
					if (Array[Count] == "00")
						continue;

					GroupName = US[((Number.Length / 2)) - Count + 1];

					if (GroupToWords)
						OutPut += GroupNumberAboveHundred(Array[Count]).TrimEnd(' ') + " " + GroupName + " ";
					else
						OutPut += Array[Count].TrimStart('0') + " " + GroupName;
				}
			}
			Array = null;
			return OutPut.Trim();
		}

		private static string Group(string Argument)
		{
			string Hyphen = "";
			string OutPut = "";
			Int16 d1 = Convert.ToInt16(Argument.Substring(0, 1));
			Int16 d2 = Convert.ToInt16(Argument.Substring(1, 1));
			Int16 d3 = Convert.ToInt16(Argument.Substring(2, 1));
			if ((d1 >= 1))
				OutPut += SNu[d1] + " Hundred ";
			if ((double.Parse(Argument.Substring(1, 2)) < 20))
				OutPut += SNu[Convert.ToInt16(Argument.Substring(1, 2))];
			if ((double.Parse(Argument.Substring(1, 2)) >= 20))
			{
				if (Convert.ToInt16(Argument.Substring(2, 1)) == 0)
					Hyphen += " ";
				else
					Hyphen += " ";
				OutPut += SNt[d2] + Hyphen + SNu[d3];
			}
			return OutPut;
		}

		private static string GroupNumberAboveHundred(string Argument)
		{
			string Hyphen = "";
			string OutPut = "";
			Int16 d2 = Convert.ToInt16(Argument.Substring(0, 1));
			Int16 d3 = Convert.ToInt16(Argument.Substring(1, 1));
			if ((double.Parse(Argument.Substring(0, 2)) < 20))
				OutPut += SNu[Convert.ToInt16(Argument.Substring(0, 2))];
			if ((double.Parse(Argument.Substring(0, 2)) >= 20))
			{
				if (Convert.ToInt16(Argument.Substring(1, 1)) == 0)
					Hyphen += " ";
				else
					Hyphen += " ";
				OutPut += SNt[d2] + Hyphen + SNu[d3];
			}
			return OutPut;
		}

		/// <summary>
		/// 	Initialize the Number Mapping variables.
		/// </summary>
		private static void Initialize()
		{
			if (String.IsNullOrEmpty(SNu[1]))
			{
				SNu[0] = "";
				SNu[1] = "One";
				SNu[2] = "Two";
				SNu[3] = "Three";
				SNu[4] = "Four";
				SNu[5] = "Five";
				SNu[6] = "Six";
				SNu[7] = "Seven";
				SNu[8] = "Eight";
				SNu[9] = "Nine";
				SNu[10] = "Ten";
				SNu[11] = "Eleven";
				SNu[12] = "Twelve";
				SNu[13] = "Thirteen";
				SNu[14] = "Fourteen";
				SNu[15] = "Fifteen";
				SNu[16] = "Sixteen";
				SNu[17] = "Seventeen";
				SNu[18] = "Eighteen";
				SNu[19] = "Nineteen";
			}
			if (String.IsNullOrEmpty(DNu[1]))
			{
				DNu[0] = "";
				DNu[1] = "First";
				DNu[2] = "Second";
				DNu[3] = "Third";
				DNu[4] = "Fourth";
				DNu[5] = "Fifth";
				DNu[6] = "Sixth";
				DNu[7] = "Seventh";
				DNu[8] = "Eighth";
				DNu[9] = "Ninth";
				DNu[10] = "Tenth";
				DNu[11] = "Eleventh";
				DNu[12] = "Twelfth";
				DNu[13] = "Thirteenth";
				DNu[14] = "Fourteenth";
				DNu[15] = "Fifteenth";
				DNu[16] = "Sixteenth";
				DNu[17] = "Seventeenth";
				DNu[18] = "Eighteenth";
				DNu[19] = "Nineteenth";
				DNu[20] = "Twentieth";
				DNu[21] = "Thirtieth";
			}
			if (String.IsNullOrEmpty(SNt[2]))
			{
				SNt[2] = "Twenty";
				SNt[3] = "Thirty";
				SNt[4] = "Forty";
				SNt[5] = "Fifty";
				SNt[6] = "Sixty";
				SNt[7] = "Seventy";
				SNt[8] = "Eighty";
				SNt[9] = "Ninety";
			}
			if (String.IsNullOrEmpty(US[2]))
			{
				US[1] = "";
				US[2] = "Thousand";
				US[3] = "Lakh";
				US[4] = "Crore";
				US[5] = "Abja";
				US[6] = "Kharva";
				US[7] = "Nikharva";
				US[8] = "Padma";
				US[9] = "Septillion";
				US[10] = "Octillion";
			}
		}

		/// <summary>
		/// 	This function is used to get year value in text
		/// </summary>
		/// <param name="asYear"> </param>
		/// <returns> </returns>
		private static string GetYearInWords(string asYear)
		{
			string sYear = "";

			string sFirstTwoDigits = asYear.Substring(0, 2);
			string sLastTwoDigits = asYear.Substring(2, 2);

			if (Convert.ToInt32(asYear) < 2000)
				sYear = GroupNumberAboveHundred(sFirstTwoDigits) + " " + GroupNumberAboveHundred(sLastTwoDigits);
			else
			{
				sYear = GetNumberInWords(asYear);
				sYear = sYear.Replace("Rupees", "");
				sYear = sYear.Replace("Only", "");
				//sYear;
			}

			return sYear.Trim();
		}

		#endregion

		#region QueryString Encription

		public static string EncryptQuerystring(string asQueryString)
		{
			SymmetricAlgorithm skSymKey = StringUtility.GeneratePasswordKey("Regulus");
			string sEncryptString = StringUtility.EncryptString(skSymKey, asQueryString);
			// We append a 'q' character to the query string to avoid a failure while decrypting query string incase it ends with a '+' character.
			// This also allows us to check if we have encrypted the query string.
			return sEncryptString + "q";
		}

		public static string DecryptQuerystring(string asQueryString)
		{
			// If the query string does not end with q, it means we did not encrypt it (see EncryptQuerystring function)
			// In this case, we return query string without decrypting it.
            if (!asQueryString.EndsWith("q") || asQueryString.Contains("ReturnUrl"))
				return asQueryString;

			// The change is avoid failure in case if the encrypted string ends with +.
			asQueryString = asQueryString.Substring(0, asQueryString.LastIndexOf("q"));
			SymmetricAlgorithm skSymKey = StringUtility.GeneratePasswordKey("Regulus");
			return StringUtility.DecryptString(skSymKey, asQueryString.Replace(" ", "+"));
		}

		public static Dictionary<string, string> DecryptQuerystring(HttpRequest currentRequest, string decodedQueryString)
		{
			string sDecryptedQueryString = DecryptQuerystring(decodedQueryString);
			var oHttpRequest = new HttpRequest(currentRequest.FilePath, currentRequest.Url.ToString(), sDecryptedQueryString);

			var oQueryStringKeyValues = new Dictionary<string, string>();
			oHttpRequest.QueryString.AllKeys.ToList().ForEach(key => oQueryStringKeyValues.Add(key, oHttpRequest.QueryString[key] != null ? oHttpRequest.QueryString[key].ToString() : string.Empty));
			return oQueryStringKeyValues;
		}

		#endregion

		#region Dataset related

		/// <summary>
		/// 	This method checks if the specified value contains the value.
		/// </summary>
		/// <param name="aoDS"> </param>
		/// <param name="asFieldToCheck"> </param>
		/// <param name="asValueToCheck"> </param>
		/// <returns> </returns>
		public static bool CheckIfValueExistsInDataset(DataSet aoDS, string asFieldToCheck, string asValueToCheck)
		{
			if (aoDS == null)
				return false;
			if (aoDS.Tables[0].Rows.Count == 0)
				return false;

			for (int iCount = 0; iCount < aoDS.Tables[0].Rows.Count; iCount++)
			{
				if (aoDS.Tables[0].Rows[iCount][asFieldToCheck].ToString() == asValueToCheck)
					return true;
			}
			return false;
		}

		/// <summary>
		/// 	Check if specified value exists in dataset.
		/// </summary>
		/// <param name="aoDT"> </param>
		/// <param name="asFieldToCheck"> </param>
		/// <param name="asValueToCheck"> </param>
		/// <returns> </returns>
		public static bool CheckIfValueExistsInDataTable(DataTable aoDT, string asFieldToCheck, string asValueToCheck)
		{
			if (aoDT == null)
				return false;
			if (aoDT.Rows.Count == 0)
				return false;

			for (int iCount = 0; iCount < aoDT.Rows.Count; iCount++)
			{
				if (aoDT.Rows[iCount][asFieldToCheck].ToString() == asValueToCheck)
					return true;
			}
			return false;
		}

		/// <summary>
		/// 	Check if specified value exists in dataset.
		/// </summary>
		/// <param name="aoDT"> </param>
		/// <param name="asFieldToCheck"> </param>
		/// <param name="asValueToCheck"> </param>
		/// <returns> </returns>
		public static bool IsUserHasScreenAccess(Constants.SchoolConfigurations oSchoolConfigurations)
		{
			if (HttpContext.Current.Session[Constants.S_SESSION_SCREENACCESS_DATATABLE] == null)
				return false;
			var odtAccess = (DataTable) HttpContext.Current.Session[Constants.S_SESSION_SCREENACCESS_DATATABLE];
			if (odtAccess.Rows.Count == 0)
				return false;

			DataRow[] oArrDataRow = odtAccess.Select("Configure_Id=" + Convert.ToInt32(oSchoolConfigurations).ToString());
			if (oArrDataRow == null)
				return false;
			if (oArrDataRow.Length == 0)
				return false;
			return true;
		}

		/// <summary>
		/// 	Check if specified value exists in dataset.
		/// </summary>
		/// <param name="aoDT"> </param>
		/// <param name="asFieldToCheck"> </param>
		/// <param name="asValueToCheck"> </param>
		/// <returns> </returns>
		public static char IsUserHasEditAccess(Constants.SchoolConfigurations oSchoolConfigurations)
		{
			if (HttpContext.Current.Session[Constants.S_SESSION_SCREENACCESS_DATATABLE] == null)
				return Constants.C_NO;
			var odtAccess = (DataTable) HttpContext.Current.Session[Constants.S_SESSION_SCREENACCESS_DATATABLE];
			if (odtAccess.Rows.Count == 0)
				return Constants.C_NO;

			DataRow[] oArrDataRow = odtAccess.Select("Configure_Id=" + Convert.ToInt32(oSchoolConfigurations).ToString());
			if (oArrDataRow == null)
				return Constants.C_NO;
			if (oArrDataRow.Length == 0)
				return Constants.C_NO;
			if (oArrDataRow[0]["Can_Edit"] == DBNull.Value)
				return Constants.C_NO;

			return Convert.ToChar(oArrDataRow[0]["Can_Edit"]);
		}

		#endregion

		#region XML Creation

		/// <summary>
		/// 	This method accepts parameters as aoGridView, asRootElementName and asElementName.
		/// 	It fetches all the columns for all the rows from the specified grid and creates the XML structure
		/// 	for the same.
		/// </summary>
		/// <param name="aoGridView"> </param>
		/// <param name="asRootElementName"> </param>
		/// <param name="asElementName"> </param>
		/// <returns> </returns>
		public static string GetXMLStringFromGridRows(GridView aoGridView, string asRootElementName, string asElementName, int aiStartColumnIndex, int aiStandardDivisionIdColumnIndex)
		{
			const string S_ELEMENT = "element";
			var oDoc = new XmlDocument();

			// Create a root level element.
			XmlElement root = oDoc.CreateElement(asRootElementName);
			XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, asRootElementName, "");
			CheckBox oChkSubject;
			string sAtrrName;
			XmlAttribute attr;

			// Loop through all the grid rows.            
			for (int iRowCount = 0; iRowCount <= aoGridView.Rows.Count - 1; iRowCount++)
			{
				GridViewRow oRow = aoGridView.Rows[iRowCount];

				// Loop through all the columns for the row.
				for (int iColCount = aiStartColumnIndex; iColCount <= oRow.Cells.Count - 1; iColCount++)
				{
					// Create root xml element.
					XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, asElementName, "");
					sAtrrName = "Standard_Division_Id"; //this is for first parameter
					attr = oDoc.CreateAttribute(sAtrrName);
					attr.Value = oRow.Cells[aiStandardDivisionIdColumnIndex].Text;
					oXmlNode.Attributes.Append(attr);

					if (iColCount >= aiStartColumnIndex)
					{
						oChkSubject = (CheckBox) (oRow.Cells[iColCount].Controls[0]);
						if ((oChkSubject.Visible) && oChkSubject.Checked == true)
						{
							// Fetch data from the column. Key represents the column Id which is the field name in database.
							// Value represents the actual value stored in the column.
							sAtrrName = "Subject_Id"; // this is for 2nd parameter.
							attr = oDoc.CreateAttribute(sAtrrName);
							attr.Value = oRow.Cells[iColCount].Text;
							oXmlNode.Attributes.Append(attr);
						}
					}
					//here comparing attributes count for creating xml string
					//with standard_division_id and subject_id as 2 attributes.
					if (oXmlNode.Attributes.Count == 2)
					{
						// Add the node to root node.
						oXmlRootNode.AppendChild(oXmlNode);
					}
				}
			}
			// Add the root node to document element. 
			root.AppendChild(oXmlRootNode);

			// return the string generated.
			return root.InnerXml;
		}

		/// <summary>
		/// 	Convert a List of objects into a XML string
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="alstEntityClass"> The List of objects to be converted to XML </param>
		/// <returns> a string in XML format representing the List </returns>
		public static string GetXMLForList<T>(List<T> alstEntityClass)
		{
			var sw = new StringWriter();
			new XmlSerializer(alstEntityClass.GetType()).Serialize(sw, alstEntityClass);
			string sXML = sw.ToString();
			sXML = sXML.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", String.Empty);
			return sXML;
		}

		/// <summary>
		/// 	This method is used to generate document XML.
		/// </summary>
		/// <param name="alstGenerateXML"> </param>
		/// <returns> </returns>
		public static string GenerateXml(Object alstGenerateXML)
		{
			var oStrwrtr = new StringWriter();
			new XmlSerializer(alstGenerateXML.GetType()).Serialize(oStrwrtr, alstGenerateXML);
			string sXml = oStrwrtr.ToString();
			sXml = sXml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", string.Empty);
			return sXml;
		}

		#endregion XML Creation

		#region CheckIsBackOrCancelClickEvent

		public static bool CheckCancelOrBackClickEvent(Page aoPage)
		{
			bool bUseSubmitBehavior = true;
			string sCtrlname = aoPage.Request.Params.Get("__EVENTTARGET");
			if (sCtrlname != null && sCtrlname != "")
			{
				if (aoPage.FindControl(sCtrlname) != null)
				{
					Type oType = aoPage.FindControl(sCtrlname).GetType();
					var objButton = new Button();
					if (oType.Equals(objButton.GetType()))
					{
						var oButton = (Button) aoPage.FindControl(sCtrlname);
						if (oButton.CausesValidation == false)
							bUseSubmitBehavior = false;
					}
				}
			}
			return bUseSubmitBehavior;
		}

		#endregion

		#region Cache Related
		/// <summary>
		/// This method is used to add data in cache for given string and object.
		/// </summary>
		/// <param name="keyString"></param>
		/// <param name="value"></param>
		/// <param name="expiryInHours"></param>
		public static void AddToCache(string askeyString, object value)
		{
			HttpRuntime.Cache.Add(askeyString, value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
		}

        public static void AddToCache(string keyString, object value, int expiryInHours)
        {
            HttpRuntime.Cache.Add(keyString, value, null, DateTime.Now.AddHours(expiryInHours), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);            
        }
        public static void FillResourcesValuesInHashTable()
        {
            htMarathi = CommonUtility.FillCachingMarathiResources();
            htEnglish = FillCachingResources();
        }

		/// <summary>
		/// This method is used to retrieve data from cache.
		/// </summary>
		/// <param name="keyString"></param>
		/// <returns></returns>
		public static object GetFromCache(string keyString)
		{
			return HttpRuntime.Cache[keyString];
		}
		/// <summary>
		/// This method is used to clear cache.
		/// </summary>
		/// <param name="keyString"></param>
		public static void ClearCache(string keyString)
		{
			 HttpRuntime.Cache.Remove(keyString);
		}
     
        public static Hashtable FillCachingMarathiResources()
        {
            if (HttpRuntime.Cache[Constants.S_HASHTABLE_MARATHI.ToString()] == null)
                AddToCache(Constants.S_HASHTABLE_MARATHI.ToString(), GetMarathiResourceHashTable(), 48);

            return GetFromCache(Constants.S_HASHTABLE_MARATHI.ToString()) as Hashtable;
        }
        public static Hashtable FillCachingResources()
        {
            if (HttpRuntime.Cache[Constants.S_HASHTABLE_ENGLISH.ToString()] == null)
                AddToCache(Constants.S_HASHTABLE_ENGLISH.ToString(), GetResourceHashTable(), 48);

            return GetFromCache(Constants.S_HASHTABLE_ENGLISH.ToString()) as Hashtable;
        }
        
		#endregion


		/// <summary>
		/// 	This method is used to get encrypted query string for selecting config menu item index.
		/// </summary>
		/// <param name="aeschoolConfigMenuId"> </param>
		/// <returns> </returns>
		public static object GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId aeSchoolConfigMenuId)
		{
			string sQuerystring = "";
			sQuerystring = "MenuId=" + Convert.ToInt32(aeSchoolConfigMenuId);
			string sEncryptedString = Constants.S_PAGE_SCHOOL_CONFIG_CONTROL_PANEL.ToString() + "?" + EncryptQuerystring(sQuerystring);

			return sEncryptedString;
		}

		// IsNumeric Function
		public static bool IsNumeric(object Expression)
		{
			// Variable to collect the Return value of the TryParse method.
			bool isNum;

			// Define variable to collect out parameter of the TryParse method. If the conversion fails, the out parameter is zero.
			double retNum;

			// The TryParse method converts a string in a specified style and culture-specific format to its double-precision floating point number equivalent.
			// The TryParse method does not generate an exception if the conversion fails. If the conversion passes, True is returned. If it does not, False is returned.
			isNum = Double.TryParse(Convert.ToString(Expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out retNum);
			return isNum;
		}

		/// <summary>
		/// 	This method is used to display current academic year label.
		/// </summary>
		/// <param name="sYear"> </param>
		/// <returns> </returns>
		public static string DisplayAcademicYear(string sYear)
		{
			return "You are viewing data of old academic year (" + sYear + ").";
		}

		/// <summary>
		/// 	This method is used to return page request.
		/// </summary>
		/// <returns> </returns>
		public static string GetPageRequest()
		{
			string sPageRequest = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath;
			sPageRequest = sPageRequest.Remove(sPageRequest.LastIndexOf("."));
			sPageRequest = sPageRequest.Substring(sPageRequest.LastIndexOf("/") + 1);
			return sPageRequest;
		}

		public static string FormatCurrency(decimal adAmount, bool abPrefixSymbol)
		{
			var cultureInfo = new CultureInfo("hi-IN");
			if (!abPrefixSymbol)
				cultureInfo.NumberFormat.CurrencySymbol = String.Empty;
			return adAmount.ToString("C2", cultureInfo);
		}

		public static string FormatCurrency(decimal adAmount)
		{
			return FormatCurrency(adAmount, false);
		}

		public static string FormatCurrency(object aoAmount, bool abPrefixSymbol)
		{
			decimal adAmount = Convert.ToDecimal(aoAmount);
			return FormatCurrency(adAmount, abPrefixSymbol);
		}

		public static string FormatCurrency(object aoAmount)
		{
			return FormatCurrency(aoAmount, false);
		}

		/// <summary>
		/// 	This method is used to return  file name with appending time.
		/// </summary>
		/// <param name="asFileName"> </param>
		/// <returns> </returns>
		public static string GetFileNameForRenaming(string asFileName)
		{
			// This method modifies the file name as it is duplicate on the server.
			// Current time:monutes:seconds are appended to the file name and same is then returned.
			string sFileName;

			// Remove the extension from the file name.
			sFileName = asFileName.Substring(0, asFileName.LastIndexOf("."));

			// Append the time format to the file name.
            sFileName = sFileName + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();

			// Again append the original extension of the file.
			sFileName = sFileName + asFileName.Substring(asFileName.LastIndexOf("."));

			// Return the file name.
			return sFileName;
		}

        /// <summary>
        /// 	This method is used to return  file name with appending time.
        /// </summary>
        /// <param name="asFileName"> </param>
        /// <returns> </returns>
        public static string GetFamilyFileNameForRenaming(string asFamilyFileName)
        {
            // This method modifies the file name as it is duplicate on the server.
            // Current time:monutes:seconds are appended to the file name and same is then returned.
            string sFamilyFileName;

            // Remove the extension from the file name.
            sFamilyFileName = asFamilyFileName.Substring(0, asFamilyFileName.LastIndexOf("."));

            // Append the time format to the file name.
            sFamilyFileName = sFamilyFileName + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();

            // Again append the original extension of the file.
            sFamilyFileName = sFamilyFileName + asFamilyFileName.Substring(asFamilyFileName.LastIndexOf("."));

            // Return the file name.
            return sFamilyFileName;
        }

        /// <summary>
        /// This method is used to get Hexadecimal string from a Image.
        /// </summary>
        /// <param name="aByteArray"></param>
        /// <returns></returns>
        public static string ConvertImageToHex(byte[] aByteArray)
        {
            string sHexString = string.Empty;
            if (aByteArray != null && aByteArray.Length > 1)
            {
                StringBuilder oResult = new StringBuilder(aByteArray.Length * 2);
                string HexAlphabet = "0123456789ABCDEF";
                foreach (byte B in aByteArray)
                {
                    oResult.Append(HexAlphabet[(int)(B >> 4)]);
                    oResult.Append(HexAlphabet[(int)(B & 0xF)]);
                }
                sHexString = "0x" + oResult.ToString();
            }
            return sHexString;
        }

        /// <summary>
        /// This method is added to get the Image from binary data.
        /// </summary>
        /// <param name="asPhotoFilePath"></param>
        /// <param name="BinaryFormatPhoto"></param>
        /// <returns></returns>
        public static string GetImage(Byte[] BinaryFormatPhoto, string asPhotoFilePath = ".jpeg")
        {
            string sFileType = string.Empty;
            string sExtension = string.Empty;
            string sImageURL = string.Empty;
            if (!asPhotoFilePath.IsNullOrEmpty())
                sExtension = System.IO.Path.GetExtension(asPhotoFilePath);
            switch (sExtension)
            {
                case ".gif":
                case ".GIF":
                    sFileType = "image/gif";
                    break;
                case ".jpg":
                case ".jpeg":
                case ".jpe":
                case ".JPG":
                case ".JPEG":
                case ".JPE":
                    sFileType = "image/jpeg";
                    break;
                case ".png":
                case ".PNG":
                    sFileType = "image/png";
                    break;
                default:
                    sFileType = "image/jpeg";
                    break;
            }

            if (!BinaryFormatPhoto.IsNull())
                sImageURL = "data:" + sFileType + ";charset=utf-8;base64," + Convert.ToBase64String(BinaryFormatPhoto);
            return sImageURL;
        }

		/// <summary>
		/// 	This method is used to rename existing file.
		/// </summary>
		/// <param name="asFileName"> </param>
		/// <param name="asNewFileName"> </param>
		/// <returns> </returns>
		public static string GetFileNameForRenaming(string asFileName, string asNewFileName)
		{
			// This method modifies the file name as it is duplicate on the server.
			// Current time:monutes:seconds are appended to the file name and same is then returned.

			string sFileName;

			// Remove the extension from the file name.
			sFileName = asFileName.Substring(0, asFileName.LastIndexOf("."));

			// Append the time format to the file name.
			sFileName = asNewFileName;

			// Again append the original extension of the file.
			sFileName = sFileName + asFileName.Substring(asFileName.LastIndexOf("."));

			// Return the file name.
			return sFileName;
		}

		/// <summary>
		/// 	Checks if a given DateTime object is a valid for SqlDateTime type
		/// 	To be a valid SqlDateTime type, it has to be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM
		/// </summary>
		/// <param name="adtDateTime"> The DateTime object to check </param>
		/// <returns> true if its valid for SqlDateTime type, false otherwise </returns>
		public static bool IsAValidSqlDateTime(DateTime adtDateTime)
		{
			return !(adtDateTime < SqlDateTime.MinValue.Value || adtDateTime > SqlDateTime.MaxValue.Value);
		}

		/// <summary>
		/// 	Checks if a given DateTime object is a valid for SqlSmallDateTime type
		/// 	To be a valid SqlSmallDateTime type, it has to be between 1900-01-01 and 2079-06-06
		/// </summary>
		/// <param name="adtDateTime"> The DateTime object to check </param>
		/// <returns> true if its valid for SqlSmallDateTime type, false otherwise </returns>
		public static bool IsAValidSqlSmallDateTime(DateTime adtDateTime)
		{
			DateTime dtMinValue = DateTime.Parse("1900-01-01");
			DateTime dtMaxValue = DateTime.Parse("2079-06-06");

			return !(adtDateTime < dtMinValue || adtDateTime > dtMaxValue);
		}

		/// <summary>
		/// This method is used to modify exception message according to language.
		/// </summary>
		/// <param name="asValidationMsg"></param>
		/// <param name="asReplaceBy"></param>
		/// <param name="asReplaceTo"></param>
		/// <param name="asStringToBeRemove"></param>
		/// <returns></returns>
		public static string ModifyExceptionMessage(String asValidationMsg, string asFirstWordReplaceBy, string asFirstWordReplaceTo, string asStringTobReplaced, string asStringToBePlaced)
		{
			string sResult = string.Empty;
            string[] sSepearator = new string[] { "<BR>", "<BR />" };
			string[] sValidationMsg = asValidationMsg.Split(sSepearator, StringSplitOptions.RemoveEmptyEntries);
			for (int iMessageCnt = 0; iMessageCnt < sValidationMsg.Length; iMessageCnt++)
				sResult += ReplaceFirst(sValidationMsg[iMessageCnt], asFirstWordReplaceBy, asFirstWordReplaceTo).Replace(asStringTobReplaced, asStringToBePlaced) + "<BR>";
			return sResult;
		}

		/// <summary>
		/// This method is used to replace first word of exception string.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="search"></param>
		/// <param name="replace"></param>
		/// <returns></returns>
		public static string ReplaceFirst(string text, string search, string replace)
		{
			int pos = text.IndexOf(search);
			if (pos < 0)
			{
				return text;
			}
			return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
		}

        public static string GetResourceValue(string key)
        {           
                if (Thread.CurrentThread.CurrentCulture.ToString() == Constants.S_MARATHI_LANGUAGE)
                {
                    if (!htMarathi.ContainsKey(key))
                    {
                        htMarathi = GetMarathiResourceHashTable();
                        AddToCache(Constants.S_HASHTABLE_MARATHI.ToString(), GetMarathiResourceHashTable(), 48);
                    }
                    return htMarathi[key].ToString();
                }
                else
                {
                    if (!htEnglish.ContainsKey(key))
                    {
                        htEnglish = GetResourceHashTable();
                        AddToCache(Constants.S_HASHTABLE_ENGLISH.ToString(), GetResourceHashTable(), 48);
                    }
                    return htEnglish[key].ToString();
                }            
            
        }

        public static Hashtable GetMarathiResourceHashTable()
        {
            try
            {
                htMarathi = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
					ResXResourceReader reader1 = new ResXResourceReader(AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["GlobalResourceFilePath"].ToString() + "\\LocalizedResources.mr.resx");
				foreach (DictionaryEntry entry in reader1)
                    {
                        htMarathi.Add(entry.Key, entry.Value);
                    }
                    return htMarathi;
            }
            catch (Exception)
            {
                return htMarathi;
            }
        }

        public static Hashtable GetResourceHashTable()
        {
            try
            {
                htEnglish=new Hashtable(StringComparer.InvariantCultureIgnoreCase);
				ResXResourceReader reader = new ResXResourceReader(AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["GlobalResourceFilePath"].ToString() + "\\LocalizedResources.resx");    
                    //htEnglish = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
                    foreach (DictionaryEntry entry in reader)
                    {
                        htEnglish.Add(entry.Key, entry.Value);
                    }
                    return htEnglish;               
               
            }
            catch (Exception)
            {
                return htEnglish;
            }
        }
	}

	public class GenericClass<T> where T : class, new()
	{
		public T GetFilledObject(SqlDataReader aoReader)
		{
			var temp = new T();
			PropertyInfo[] oPropertyInfos = temp.GetType().GetProperties();
			for (int index = 0; index < oPropertyInfos.Length; index++)
			{
				PropertyInfo oPropertyInfo = oPropertyInfos[index];
				if (CheckPropertyColumnExists(aoReader, oPropertyInfo.Name))
					temp.GetType().GetProperty(oPropertyInfo.Name).SetValue(temp, GetValue(oPropertyInfo, aoReader), null);
			}
			return temp;
		}

		public List<T> GetFilledObjectList(SqlDataReader reader)
		{
			var t = new T();
			var TList = new List<T>();
			PropertyInfo[] oPropertyInfos = t.GetType().GetProperties();

			var temp = new T();

			while (reader.Read())
			{
				temp = new T();
				for (int index = 0; index < oPropertyInfos.Length; index++)
				{
					PropertyInfo oPropertyInfo = oPropertyInfos[index];
					if (CheckPropertyColumnExists(reader, oPropertyInfo.Name))
						temp.GetType().GetProperty(oPropertyInfo.Name).SetValue(temp, GetValue(oPropertyInfo, reader), null);
				}
				TList.Add(temp);
			}
			return TList;
		}

		private bool CheckPropertyColumnExists(SqlDataReader reader, string columnName)
		{
			reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName= '" + columnName + "'";
			return (reader.GetSchemaTable().DefaultView.Count > 0);
		}

		private object GetValue(PropertyInfo oPropertyInfo, SqlDataReader oSqlDataReader)
		{
			string sPropertyName = oPropertyInfo.PropertyType.Name;
			string sDBFieldName = oPropertyInfo.Name;
			if (oSqlDataReader[sDBFieldName] == DBNull.Value)
				return (oPropertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(oPropertyInfo.PropertyType) : null);
			if (oPropertyInfo.PropertyType.IsEnum)
				return oSqlDataReader[sDBFieldName];
			switch (sPropertyName)
			{
				case "String":
					return Convert.ToString(oSqlDataReader[sDBFieldName]);
				case "Int32":
					return Convert.ToInt32(oSqlDataReader[sDBFieldName]);
				case "Decimal":
					return Convert.ToDecimal(oSqlDataReader[sDBFieldName]);
				case "Double":
					return Convert.ToDouble(oSqlDataReader[sDBFieldName]);
				case "Boolean":
					return Convert.ToBoolean(oSqlDataReader[sDBFieldName]);
				case "DateTime":
					return Convert.ToDateTime(oSqlDataReader[sDBFieldName]);
				case "Int16":
					return Convert.ToInt16(oSqlDataReader[sDBFieldName]);
				default:
					return Convert.ToString(oSqlDataReader[sDBFieldName]);
			}
		}  
	}
}