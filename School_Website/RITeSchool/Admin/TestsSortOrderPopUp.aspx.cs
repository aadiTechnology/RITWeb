using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class TestsSortOrderPopUp : SchoolBase
{    
    #region Constants
    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                ReadQuerystring();
                FillStandardCombobox();
                FillExamGridView();                
                btnCancel.Attributes["onclick"] = "javascript:DisableButtons()";                
                ApplyMouseHoverEffect(new List<Button> { imgBtnSave, btnCancel });
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void grdExam_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                HtmlSelect oDropDownList = (HtmlSelect)e.Row.FindControl("ddlOrder");
                DataView oDataView = (DataView)grdExam.DataSource;
                for (int iCnt = 0; iCnt < oDataView.Table.Rows.Count; iCnt++)
                {
                    ListItem oListItem = new ListItem((iCnt + 1).ToString(), (iCnt + 1).ToString());
                    oDropDownList.Items.Add(oListItem);
                    if (iCnt == e.Row.RowIndex)
                        oListItem.Selected = true;
                }
                oDropDownList.Attributes.Add("onchange", "Reorder(this, '" + oDropDownList.ID + "','" + grdExam.ClientID + "',"
                                                            + e.Row.RowIndex + ", " + oDataView.Table.Rows.Count + ",'" + lblSuccess.ClientID + "')");
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
            string sXmlExamOrder = GenerateExamOrderXML();            
            SchoolwiseStandardTestMasterCollectionBL oSchoolwiseStandardTestMasterCollectionBL = new SchoolwiseStandardTestMasterCollectionBL(miSchoolId,miAcademicYearId);            
            int iStandardId = Convert.ToInt32(cmbStandard.SelectedValue);
            oSchoolwiseStandardTestMasterCollectionBL.UpdateExamSortOrder(miSchoolId, miAcademicYearId, iStandardId, sXmlExamOrder);
            FillExamGridView();
            lblSuccess.Text = Resources.LocalizedResources.ExamSortOrderSavedSuccessfully;
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions)
        {
            FillExamGridView();

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This function fills combobox with standards
    /// </summary>
    private void FillStandardCombobox()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        ControlUtility.FillDropDownList(oDtStandardCollection, ref cmbStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       String.Empty);
        if (cmbStandard.Items.Count > 0 && hidStandardId.Value != "0")
            cmbStandard.SelectedValue = hidStandardId.Value;

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
        XmlElement root = oDoc.CreateElement("ExamOrderCollection");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "ExamOrderCollection", "");

        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount < grdExam.Rows.Count; iRowCount++)
        {
            HtmlSelect oDropDownList = (HtmlSelect)grdExam.Rows[iRowCount].FindControl("ddlOrder");
            // Create root xml element.
            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "ExamOrder", "");

            string sAtrrName = "Exam_Id";
            XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = grdExam.DataKeys[iRowCount][0].ToString();
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "Sort_Order";
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

    protected void grdExam_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdExam.PageIndex = e.NewPageIndex;
            FillExamGridView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event is used for implementing paging style.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdExam_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                Table PagerTable = (Table)e.Row.Cells[0].Controls[0];
                PagerTable.CssClass = "ClsNwGridPaging";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillExamGridView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion

    #region Private Methods

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        if (QueryString["Standard_Id"] != null)
            hidStandardId.Value = QueryString["Standard_Id"];
    }

    private void FillExamGridView()
    {
        int iStandardId = Convert.ToInt32(cmbStandard.SelectedValue);
        SchoolwiseStandardTestMasterCollectionBL oSchoolwiseStandardTestMasterCollectionBL = new SchoolwiseStandardTestMasterCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSUserDetails = oSchoolwiseStandardTestMasterCollectionBL.GetAllTestsForStandard(iStandardId);
        grdExam.DataSource = oDSUserDetails.DefaultView;
        grdExam.DataBind();
    }

    #endregion
}
