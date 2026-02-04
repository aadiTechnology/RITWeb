/* -----------------------------------------------------------------------
 *	Author		: Vishal B. Shah
 *	Date		: 7-Mar-2012
 *	Purpose		: Contains common extension methods.
 * -----------------------------------------------------------------------
 */

using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace Utility
{
	/// <summary>
	///		Contains common extension methods.
	/// </summary>
	public static class Extensions
	{
		#region -- OBJECT EXTENSION(s) --

		/// <summary>
		///		Determines if the given object is null.
		/// </summary>
		/// <param name="source">The object to be converted.</param>
		/// <returns>true is it is null, false otherwise.</returns>
		public static bool IsNull(this object source)
		{
			return source == null;
		}
		
		/// <summary>
		///		Converts the given object into an integer.
		/// </summary>
		/// <param name="source">The object to be converted.</param>
		/// <returns>System.Int32</returns>
		public static int ToInt(this object source)
		{
			return Convert.ToInt32(source);
		}

		/// <summary>
		///		Converts the given object into a boolean.
		/// </summary>
		/// <param name="source">The object to be converted.</param>
		/// <returns>System.Boolean</returns>
		public static bool ToBool(this object source)
		{
			return Convert.ToBoolean(source);
		}

		/// <summary>
		///		Converts the given object into a decimal.
		/// </summary>
		/// <param name="source">The object to be converted.</param>
		/// <returns>System.Decimal</returns>
		public static decimal ToDecimal(this object source)
		{
			return Convert.ToDecimal(source);
		}

		/// <summary>
		///		Converts the given object into a double.
		/// </summary>
		/// <param name="source">The object to be converted.</param>
		/// <returns>System.Double</returns>
		public static double ToDouble(this object source)
		{
			return Convert.ToDouble(source);
		}

		/// <summary>
		///		Converts the object into a DateTime.
		/// </summary>
		/// <param name="source">The object to be converted.</param>
		/// <returns>System.DateTime</returns>
		public static DateTime ToDateTime(this object source)
		{
			return Convert.ToDateTime(source);
		}
	
		#endregion -- OBJECT EXTENSION(s) --

		#region -- STRING EXTENSION(s) --

		/// <summary>
		/// Determines if a string is null or empty.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static bool IsNullOrEmpty(this string source)
		{
			return String.IsNullOrEmpty(source);
		}

		/// <summary>
		/// Truncates a string value and appends it with the ellipse (...) if its length exceeds aiMaxLength.
		/// </summary>
		/// <param name="text">The string object to truncate.</param>
		/// <param name="aiMaxLength">Maximum length of the string passed.</param>
		/// <returns>Truncated string appended with ellipse.</returns>
		public static string Truncate(this string text, int aiMaxLength)
		{
			if (!text.IsNullOrEmpty())
				return text.Length > aiMaxLength ? text.Substring(0, aiMaxLength) + "..." : text;
			return string.Empty;
		}
		
		/// <summary>
		///		Determines if the given string is a valid date.
		/// </summary>
		/// <param name="asDate">The string to be converted to a System.DateTime object.</param>
		/// <returns>true if the given string is a valid date, false otherwise.</returns>
		public static bool IsValidDate(this string asDate)
		{
			DateTime bResult;
			DateTime.TryParse(asDate, out bResult);
			return bResult >= new DateTime(1900, 1, 1) && bResult != DateTime.MinValue;
		}

		/// <summary>
		/// This method is used to trim all the extra spaces.
		/// </summary>
		/// <param name="asText"></param>
		/// <returns></returns>
        public static string TrimAll(this string asText)
        {
            Regex regex = new Regex(@"\s{2,}", RegexOptions.Multiline);
            asText = regex.Replace(asText.Trim(), " ");
            asText = regex.Replace(asText, "$1");
            return asText;
        }

		/// <summary>
		///		Removes single quotes from a string.
		/// </summary>
		/// <param name="source">The string object to be processed.</param>
		/// <returns>System.String</returns>
		public static string RemoveSingleQuote(this string source)
		{
			return source.Replace("'", string.Empty);
		}

		#endregion -- STRING EXTENSION(s) --

		#region -- WEBCONTROL EXTENSION(s) --

		/// <summary>
		///		Truncates the Text of a Label if it exceeds aiMaxLength and appends it with the ellipse (...)
		/// </summary>
		/// <param name="label">The Label object to truncate.</param>
		/// <param name="aiMaxLength">Maximum length of the Text property of the Label.</param>
		/// <param name="abSetToolTip">Whether to set the original text as Labels ToolTip if its Text is truncated.</param>
		public static void Truncate(this Label label, int aiMaxLength, bool abSetToolTip)
		{
			if (label.IsNull() || label.Text.Length <= aiMaxLength)
				return;
			if (abSetToolTip)
			{
				label.ToolTip = label.Text;
				label.Style.Add("cursor", "help");
			}
			label.Text = label.Text.Truncate(aiMaxLength);
		}

		/// <summary>
		///		Binds a DropDownList to a given DataSource and a Top element.
		/// </summary>
		/// <param name="source">The System.Web.UI.WebControls.DropDownList object to be bound.</param>
		/// <param name="aoDataSource">The datasource of the DropDownList.</param>
		/// <param name="asValueField">Indicates the field in the datasource which is to be set as the value field of the DropDownList.</param>
		/// <param name="asTextField">Indicates the field in the datasource which is to be set as the text field of the DropDownList.</param>
		/// <param name="asTopElement">If asTopElement is not null or empty, it is added at the first entry in the DropDownList.</param>
		public static void Bind(this DropDownList source, object aoDataSource, string asValueField, string asTextField, string asTopElement)
		{
			source.Items.Clear();
			source.DataSource = aoDataSource;
			source.DataValueField = asValueField;
			source.DataTextField = asTextField;

			if (!asTopElement.IsNullOrEmpty())
			{
				source.AppendDataBoundItems = true;
				source.Items.Insert(Constants.I_ZERO, new ListItem(asTopElement, Constants.S_ZERO));
			}

			source.DataBind();
		}

		/// <summary>
		///		Binds a DropDownList to a given DataSource.
		/// </summary>
		/// <param name="source">The System.Web.UI.WebControls.DropDownList object to be bound.</param>
		/// <param name="aoDataSource">The datasource of the DropDownList.</param>
		/// <param name="asValueField">Indicates the field in the datasource which is to be set as the value field of the DropDownList.</param>
		/// <param name="asTextField">Indicates the field in the datasource which is to be set as the text field of the DropDownList.</param>
		public static void Bind(this DropDownList source, object aoDataSource, string asValueField, string asTextField)
		{
			source.Bind(aoDataSource, asValueField, asTextField, null);
		}

		/// <summary>
		///		Binds a DropDownList to an Enum.
		/// </summary>
		/// <typeparam name="T">The type of the Enum</typeparam>
		/// <param name="source">The System.Web.UI.WebControls.DropDownList object to be bound.</param>
		/// <param name="asTopElement">If asTopElement is not null or empty, it is added at the first entry in the DropDownList.</param>
		/// <exception cref="InvalidOperationException">Extensions.BindToEnum : Type T must be an Enum.</exception>
		public static void BindToEnum<T>(this DropDownList source, string asTopElement)
		{
			if (!typeof(T).IsEnum)
				throw new InvalidOperationException("Extensions.BindToEnum<T> : Type T must be an Enum.");
			
			var dataSource = Enum.GetValues(typeof(T))
								 .Cast<T>()
							//	 .Where(e => e.ToString() != "None") // This will be needed if an enum contains members that need to be filtered out. For e.g. VoucherType.None (Basically default values, i.e. 0).
								 .Select(e => new { Value = e.ToInt(), Text = e.ToString().Replace("_", " ") })
								 .ToList();

			source.Bind(dataSource, "Value", "Text", asTopElement);
		}

		/// <summary>
		///		Binds a DropDownList to an Enum.
		/// </summary>
		/// <typeparam name="T">The type of the Enum</typeparam>
		/// <param name="source">The System.Web.UI.WebControls.DropDownList object to be bound.</param>
		public static void BindToEnum<T>(this DropDownList source)
		{
			source.BindToEnum<T>(null);
		}

		/// <summary>
		///		This method is used to find listitem by value and select it. If not found then select first item.
		///		Use this method only when you know that there will be such case otherwise use SelectedValue property.
		/// </summary>
		/// <param name="source">The DropDownList object to be searched.</param>
		/// <param name="value">The value to be searched.</param>
		public static void FindByValue(this DropDownList source, string value)
		{
			ListItem oListItem = source.Items.FindByValue(value);
			if (oListItem != null)
				oListItem.Selected = true;
			else
				source.SelectedIndex = 0;
		}

		#endregion -- WEBCONTROL EXTENSION(s) --
	}
}