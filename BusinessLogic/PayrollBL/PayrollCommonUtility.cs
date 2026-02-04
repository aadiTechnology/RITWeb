using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Web.UI.WebControls;

using Utility;
using SchoolEntities;
using System.Web.UI;
using System.Web;

namespace BusinessLogic
{
    public static class PayrollCommonUtility
    {
        public static bool IsNonEmpty(this DataTable oDataTable)
        {
            if (oDataTable != null && oDataTable.Rows.Count > 0 && oDataTable.Rows[0][0] != DBNull.Value)
                return true;
            return false;
        }

        public static void AddColumns(this DataTable oDataTable, string[] asArrColumns)
        {
            foreach (string sColumnName in asArrColumns)
                oDataTable.Columns.Add(sColumnName);
        }

        public static void ApplyEffect(this System.Web.UI.WebControls.Button[] asArrButtons)
        {
            foreach (Button button in asArrButtons)
            {
                button.Attributes["onmouseover"] = "javascript:fnover('" + button.ClientID + "',this);";
                button.Attributes["onmouseout"] = "javascript:fnout('" + button.ClientID + "',this);";
            }
        }

        public static bool IsBetween(this DateTime currentDate, DateTime firstDate, DateTime LastDate)
        {
            return currentDate >= firstDate && currentDate <= LastDate || currentDate >= LastDate && currentDate <= firstDate;
        }

        public static string GetXML(this object baseObj, string element, List<Dictionary<string, object>> oList)
        {
            string sXml = string.Empty;
            if (oList.Count > 0)
            {
                XmlDocument doc = new XmlDocument();
                XmlElement root = doc.CreateElement(element);
                XmlNode oXmlNode = doc.CreateNode("element", element, "");
                XmlAttribute attr = null;
                foreach (Dictionary<string, object> listItem in oList)
                {
                    XmlNode oNode = doc.CreateNode("element", element, "");
                    foreach (KeyValuePair<string, object> kvp in listItem)
                    {
                        attr = doc.CreateAttribute(kvp.Key);
                        attr.Value = kvp.Value.ToString();
                        oNode.Attributes.Append(attr);
                    }
                    oXmlNode.AppendChild(oNode);
                }

                root.AppendChild(oXmlNode);
                sXml = root.InnerXml;
            }
            return sXml;
        }

        public static string ToTitleCase(this string asText)
        {
            string sText = string.Empty;

            if (string.IsNullOrEmpty(asText))
                return asText;

            try
            {
                asText = asText.Trim().ToLower();
                System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Globalization.TextInfo TextInfo = cultureInfo.TextInfo;
                sText = TextInfo.ToTitleCase(asText);
            }
            catch
            {  
                sText = asText;
            }
            return sText;
        }
    }

    public class SalaryPublishException : Exception
    {
        string msMessage;
        public override string Message
        {
            get
            {
                return msMessage;
            }
        }

        public SalaryPublishException(string asMessage)
        {
            msMessage = asMessage;
        }
    }

    public class NoRecordFoundException : Exception
    {
        string msMessage;
        public override string Message
        {
            get
            {
                return msMessage;
            }
        }

        public NoRecordFoundException(string asMessage)
        {
            msMessage = asMessage;
        }
    }

    public static class ListSource
    {
        public static void FillDropDownList(object aoDataSource, DropDownList aoDropDownList, string asDataTextField, string asDataValueField, string asTopElement)
        {
            aoDropDownList.Items.Clear();
            aoDropDownList.DataSource = aoDataSource;
            aoDropDownList.DataTextField = asDataTextField;
            aoDropDownList.DataValueField = asDataValueField;

            if (!(string.IsNullOrEmpty(asTopElement)))
            {
                aoDropDownList.AppendDataBoundItems = true;
                aoDropDownList.Items.Insert(0, new ListItem(asTopElement, "0"));
            }
            aoDropDownList.DataBind();
        }
        public static void FillCheckBoxList(object aoDataSource, CheckBoxList aoCheckBoxList, string asDataTextField, string asDataValueField)
        {
            aoCheckBoxList.Items.Clear();
            aoCheckBoxList.DataSource = aoDataSource;
            aoCheckBoxList.DataTextField = asDataTextField;
            aoCheckBoxList.DataValueField = asDataValueField;
            aoCheckBoxList.DataBind();
        }
    }
}
