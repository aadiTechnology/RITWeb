using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using Utility;

/// <summary>
/// Summary description for UserControlBase
/// </summary>
public class UserControlBase : System.Web.UI.UserControl
{
    #region -- MEMBER(s) --

    protected int miSchoolId;
    protected int miAcademicYearId;
    protected int miFinancialYearId;
    protected int miUserId;
    protected Constants.UserRoles moUserRole;

    #endregion -- MEMBER(s) --

    #region -- PROPERTIES --

    protected NameValueCollection QueryString { get; private set; }

    #endregion -- PROPERTIES --

    #region -- PUBLIC METHOD(s) --

    /// <summary>
    /// This method is used to initialize member variables.
    /// </summary>
    public void InitializeMemberVariables()
    {
        if (HttpContext.Current.Session != null)
        {
            if (Session[Constants.S_SESSION_SCHOOL_ID] != null)
                this.miSchoolId = Session[Constants.S_SESSION_SCHOOL_ID].ToInt();

            if (Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID] != null)
                this.miAcademicYearId = Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID].ToInt();

            if (Session[Constants.S_SESSION_FINANCIAL_YEAR_ID] != null)
                this.miFinancialYearId = Session[Constants.S_SESSION_FINANCIAL_YEAR_ID].ToInt();

            if (Session[Constants.S_SESSION_USER_ID] != null)
                this.miUserId = Session[Constants.S_SESSION_USER_ID].ToInt();

            if (Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] != null)
                this.moUserRole = (Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID];
        }
    }

    /// <summary>
    /// This method is used to apply mouse hover effect for buttons.
    /// </summary>
    /// <param name="aolstButtons"></param>
    public void ApplyMouseHoverEffect(List<Button> aolstButtons)
    {
        aolstButtons.ForEach(btn =>
        {
            if (btn.IsNull())
                return;
            btn.Attributes["onmouseover"] = "javascript:fnover('" + btn.ClientID + "',this);";
            btn.Attributes["onmouseout"] = "javascript:fnout('" + btn.ClientID + "',this);";
        });
    }

    /// <summary>
    /// This method is used to generate document XML.
    /// </summary>
    /// <param name="alstGenerateXML"></param>
    /// <returns></returns>
    public string GenerateXml(object alstGenerateXML)
    {
        var oStrwrtr = new StringWriter();
        new XmlSerializer(alstGenerateXML.GetType()).Serialize(oStrwrtr, alstGenerateXML);
        string sXml = oStrwrtr.ToString();
        return sXml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", string.Empty);
    }

    /// <summary>
    ///		This method is used to convert image in binary format.
    /// </summary>
    /// <param name="aoFileField"></param>
    /// <returns></returns>
    public byte[] GetByteArrayFromFileField(FileUpload aoFileField)
    {
        // Returns a byte array from the passed file field controls file
        var bytedata = new byte[0];
        if (aoFileField.PostedFile != null && aoFileField.PostedFile.ContentLength != 0)
        {
            int intFileLength = aoFileField.PostedFile.ContentLength;
            bytedata = new byte[intFileLength];
            Stream oStream = aoFileField.PostedFile.InputStream;
            oStream.Read(bytedata, 0, intFileLength);
        }

        return bytedata;
    }

    /// <summary>
    /// This method is used to set sort image.
    /// </summary>
    public void AddSortImage(ListView alstvwSections, string asSortExpression, string asSortDirection)
    {
        HtmlTableRow oHtmlTableHeaderRow = alstvwSections.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, asSortExpression, asSortDirection);
    }

    /// <summary>
    /// This method is used to revert sort order.
    /// </summary>
    /// <param name="ahidSortDirection"></param>
    public void RevertSortOrder(HiddenField ahidSortDirection)
    {
        if (ahidSortDirection.Value == Constants.S_ASCENDING)
            ahidSortDirection.Value = Constants.S_DESCENDING;
        else
            ahidSortDirection.Value = Constants.S_ASCENDING;
    }

    /// <summary>
    /// This method is used to display message.
    /// </summary>
    /// <param name="asMessage"></param>
    /// <param name="abIsError"></param>
    public virtual void DisplayMessage(string asMessage, bool abIsError, HtmlTableCell aoHtmlTableCell)
    {
        Label oLabel = aoHtmlTableCell.FindControl("lblMessage") as Label;
        if (oLabel != null)
        {
            oLabel.Text = asMessage;
            if (abIsError)
            {
                oLabel.ForeColor = Color.Red;
                aoHtmlTableCell.Align = "Left";
                oLabel.Font.Bold = false;
                oLabel.Style.Add("padding-left", "0");
            }
            else
            {
                oLabel.ForeColor = Color.Blue;
                aoHtmlTableCell.Align = "Center";
                oLabel.Font.Bold = true;
            }
        }
    }

    #endregion -- PUBLIC METHOD(s) --
}