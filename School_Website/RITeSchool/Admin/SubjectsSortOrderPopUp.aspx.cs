// File Name     :- SubjectsSortOrderPopUp.aspx.cs
// Modified By   :- Amit
// Modified Date :- 22-09-2009
// Description   :- This class is used to set subject sort order for standard.

using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic.Exceptions;
using System.Xml;
using BusinessLogic;
using Utility;

public partial class SubjectsSortOrderPopUp : SchoolBase
{
    #region " Constants "

    const string S_DATAKEY_SUBJECT_ID = "Subject_Id";

    #endregion " Constants "

    #region " Events "

    /// <summary>
    /// This event is used to fill all page controls and set java scripts to controls. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                ReadQuerystring();
                FillStandardCombobox();
                FillSubjectGridView();
                SetClientScriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set configured sort order of respective subject for that standard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdGroupDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                HtmlSelect oDropDownList = (HtmlSelect)e.Row.FindControl("ddlOrder");
                DataView oDataView = (DataView)grdSubjects.DataSource;
                for (int iCnt = 0; iCnt < oDataView.Table.Rows.Count; iCnt++)
                {
                    ListItem oListItem = new ListItem((iCnt + 1).ToString(), (iCnt + 1).ToString());
                    oDropDownList.Items.Add(oListItem);
                    if (iCnt == e.Row.RowIndex)
                        oListItem.Selected = true;
                }
                oDropDownList.Attributes.Add("onchange", "Reorder(this, '" + oDropDownList.ID + "','" + grdSubjects.ClientID + "'," + 
                                                                    e.Row.RowIndex + ", " + oDataView.Table.Rows.Count + ",'" + lblSuccess.ClientID +"')");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save subject sort order for that standard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        // Save the changes to database.
        try
        {
            string sXmlSubjectOrder = GenerateSubjectOrderXML();            
            int iStandardId = Convert.ToInt32(cmbStandard.SelectedValue);
            SubjectMasterBL oSubjectMasterBL = new SubjectMasterBL();
            oSubjectMasterBL.UpdateSubjectSortOrder(miSchoolId, miAcademicYearId, iStandardId, sXmlSubjectOrder);
            FillSubjectGridView();
            lblSuccess.Text = "<b>" + Resources.LocalizedResources.SubjectSortOrderSavedSuccessfully + "!!!</b>";
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions)
        {
            FillSubjectGridView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to change page in grid view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSubjects_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdSubjects.PageIndex = e.NewPageIndex;
            FillSubjectGridView();
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
    protected void grdSubjects_RowCreated(object sender, GridViewRowEventArgs e)
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

    /// <summary>
    /// This event is used to fill subject grid view as per seelected standard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillSubjectGridView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " Events "

    #region " Private Methods "

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        if (QueryString["Standard_Id"] != null)
            hidStandardId.Value = QueryString["Standard_Id"];
    }

    /// <summary>
    /// This method is used to fill subject grid view.
    /// </summary>
    private void FillSubjectGridView()
    {
        int iStandardId = Convert.ToInt32(cmbStandard.SelectedValue);
        SubjectCollectionBL oSubjectCollectionBL = new SubjectCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSUserDetails = oSubjectCollectionBL.GetSubjectsForStandard(iStandardId);
        if (oDSUserDetails.Rows.Count <= 0)
            btnSave.Visible = false;
        else
            btnSave.Visible = true;
        
        grdSubjects.DataSource = oDSUserDetails.DefaultView;
        grdSubjects.DataBind();
    }

    /// <summary>
    /// This method is used to set java scripts to page controls.
    /// </summary>
    private void SetClientScriptAttributes()
    {
        btnCancel.Attributes["onclick"] = "javascript:DisableButtons()";        
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel });
    }

    /// <summary>
    /// This method is used to fill combo with standards.
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
    /// This method is used to generate XML for the subject order.
    /// </summary>
    /// <returns></returns>
    private string GenerateSubjectOrderXML()
    {
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("SubjectOrderCollection");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "SubjectOrder", "");

        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount < grdSubjects.Rows.Count; iRowCount++)
        {
            HtmlSelect oDropDownList = (HtmlSelect)grdSubjects.Rows[iRowCount].FindControl("ddlOrder");
            // Create root xml element.
            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "SubjectOrder", "");

            string sAtrrName = "Subject_Id";
            XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = grdSubjects.DataKeys[iRowCount][S_DATAKEY_SUBJECT_ID].ToString();
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

    #endregion " Private Methods "

}
