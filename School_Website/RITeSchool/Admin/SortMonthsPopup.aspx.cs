using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using ProgressReportEntities;

public partial class SortMonthsPopup : SchoolBase
{
    #region Constants

    const string S_SELECT_AT_LEAST_ONE_Exam = "At least one Exam name should be selected for saving.";

    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                ReadQuerystring();
                FillMonthsGridView();
                btnCancel.Attributes["onclick"] = "javascript:DisableButtons()";
                ApplyMouseHoverEffect(new List<Button> { imgBtnSave, btnCancel });
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void grdMonths_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                HtmlSelect oDropDownList = (HtmlSelect)e.Row.FindControl("ddlOrder");
                List<PrePrimaryProgressReportMonth> olstPrePrimaryProgressReportMonth = (List<PrePrimaryProgressReportMonth>)grdMonths.DataSource;
                for (int iCnt = 0; iCnt < olstPrePrimaryProgressReportMonth.Count(); iCnt++)
                {
                    ListItem oListItem = new ListItem((iCnt + 1).ToString(), (iCnt + 1).ToString());
                    oDropDownList.Items.Add(oListItem);
                    if (iCnt == e.Row.RowIndex)
                        oListItem.Selected = true;
                }
                oDropDownList.Attributes.Add("onchange", "Reorder(this, '" + oDropDownList.ID + "','" + grdMonths.ClientID + "',"
                                                            + e.Row.RowIndex + ", " + olstPrePrimaryProgressReportMonth.Count() + ",'" + lblSuccess.ClientID + "')");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void imgBtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string sXmlSortOrder = GenerateExamOrderXML();
            PrePrimaryProgressReportMonthsBL.UpdateSortOrder(sXmlSortOrder);           
            FillMonthsGridView();
            lblSuccess.Text = "<b>Months sort order saved successfully !!!</b>";
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions)
        {
            FillMonthsGridView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Generate XML for the Exam order.
    /// </summary>
    /// <returns></returns>
    private string GenerateExamOrderXML()
    {
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("MonthsOrder");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "MonthsOrder", "");

        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount < grdMonths.Rows.Count; iRowCount++)
        {
            HtmlSelect oDropDownList = (HtmlSelect)grdMonths.Rows[iRowCount].FindControl("ddlOrder");
            // Create root xml element.
            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "MonthsOrder", "");

            string sAtrrName = "PrePrimaryProgressReportMonthId";
            XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = grdMonths.DataKeys[iRowCount][0].ToString();
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "SortOrder";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = oDropDownList.Items[oDropDownList.SelectedIndex].Value;
            oXmlNode.Attributes.Append(attr);

            // Add the node to root node.
            oXmlRootNode.AppendChild(oXmlNode);
        }
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }    
  
    #endregion

    #region Private Methods
    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        if (QueryString["StandardId"] != null)
            hidStandardId.Value = QueryString["StandardId"];
    }


    private void FillMonthsGridView()
    {
        int iStandardId = Convert.ToInt32(hidStandardId.Value);
        List<PrePrimaryProgressReportMonth> olstPrePrimaryProgressReportMonths = PrePrimaryProgressReportMonthsBL.GetSavedMonthsList(miSchoolId,miAcademicYearId,iStandardId).ToList();
        grdMonths.DataSource = olstPrePrimaryProgressReportMonths;
        grdMonths.DataBind();
    }

    #endregion
}

