using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Reflection;
using BusinessLogic.Exceptions;
using System.Data;
using System.Xml;
using Utility;
using BusinessLogic;
using ProgressReportEntities;

public partial class SortSubSubjectPopup : SchoolBase
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
                FillModuleCombo();
                imgBtnShow_Click(new object(), EventArgs.Empty);
                btnCancel.Attributes["onclick"] = "javascript:DisableButtons()";                
                ApplyMouseHoverEffect(new List<Button> { imgBtnSave, btnCancel, btnShow });
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbModuleName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
            if (oPrePrimaryProgressSheetConfigBL.IsSubjectApplicable(Convert.ToInt32(cmbModuleName.SelectedValue)) > 0)
            {
                FillSubjectCombobox();
                tdSubjectName.Visible = true;
                cmbSubjectName.Visible = true;
            }
            else
            {
                cmbSubjectName.Visible = false;
                tdSubjectName.Visible = false;
            }
            divSubSubjectGrid.Visible = false;
            imgBtnSave.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void grdSubSubject_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                HtmlSelect oDropDownList = (HtmlSelect)e.Row.FindControl("ddlOrder");
                List<PrePrimaryProgressReportSubSubjects> olstPrePrimaryProgressReportSubSubjects = (List<PrePrimaryProgressReportSubSubjects>)grdSubSubject.DataSource;
                for (int iCnt = 0; iCnt < olstPrePrimaryProgressReportSubSubjects.Count(); iCnt++)
                {
                    ListItem oListItem = new ListItem((iCnt + 1).ToString(), (iCnt + 1).ToString());
                    oDropDownList.Items.Add(oListItem);
                    if (iCnt == e.Row.RowIndex)
                        oListItem.Selected = true;
                }
                oDropDownList.Attributes.Add("onchange", "Reorder(this, '" + oDropDownList.ID + "','" + grdSubSubject.ClientID + "',"
                                                            + e.Row.RowIndex + ", " + olstPrePrimaryProgressReportSubSubjects.Count() + ",'" + lblSuccess.ClientID + "')");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void imgBtnSave_Click(object sender, EventArgs e)
    {
        // Save the changes to database.
        try
        {
            string sSubSubjectOrderXML = GenerateSubSubjectOrderXML();
            PrePrimaryProgressSheetConfigBL.UpdateExamSortOrder(sSubSubjectOrderXML);
            imgBtnShow_Click(new object(), EventArgs.Empty);
            lblSuccess.Text = "<b>Skills / Behaviour sort order saved successfully !!!</b>";
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions)
        {
            imgBtnShow_Click(new object(), EventArgs.Empty);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbSubjectName_SelectedIndexChanged(object sender, EventArgs e)
    {
        divSubSubjectGrid.Visible = false;
        imgBtnSave.Visible = false;
    }

    protected void imgBtnShow_Click(object sender, EventArgs e)
    {
        if (cmbSubjectName.Visible)
            FillSubSubjectGridView(Convert.ToInt32(cmbModuleName.SelectedValue), Convert.ToInt32(cmbSubjectName.SelectedValue));
        else
            FillSubSubjectGridView(Convert.ToInt32(cmbModuleName.SelectedValue), 0);
    }   

    protected void grdSubSubject_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdSubSubject.PageIndex = e.NewPageIndex;
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
    protected void grdSubSubject_RowCreated(object sender, GridViewRowEventArgs e)
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

    private void FillModuleCombo()
    {
        PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
        oPrePrimaryProgressSheetConfigBL.GetPrePrimaryStandardsAndModuleName(miSchoolId, miAcademicYearId);
        oPrePrimaryProgressSheetConfigBL.LstPrePrimaryModule.ForEach(module => cmbModuleName.Items.Add(new ListItem(module.ModuleName, module.ModuleID.ToString())));

        if (oPrePrimaryProgressSheetConfigBL.IsSubjectApplicable(Convert.ToInt32(cmbModuleName.SelectedValue)) > 0)
        {
            FillSubjectCombobox();
            tdSubjectName.Visible = true;
            cmbSubjectName.Visible = true;
        }
        else
        {
            cmbSubjectName.Visible = false;
            tdSubjectName.Visible = false;
        }
    }

    private void FillSubjectCombobox()
    {
        int iModuleId = Convert.ToInt32(Convert.ToInt32(cmbModuleName.SelectedValue));
        cmbSubjectName.Items.Clear();
        PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
        oPrePrimaryProgressSheetConfigBL.GetPrePrimaryProgressReportSubjects(miSchoolId, miAcademicYearId, iModuleId);
        oPrePrimaryProgressSheetConfigBL.LstPrePrimarySubjects.ForEach(subject => cmbSubjectName.Items.Add(new ListItem(subject.PrePrimaryProgressReportSubjectName, subject.PrePrimaryProgressReportSubjectID.ToString())));
    }

    /// <summary>
    /// Generate XML for the Exam order.
    /// </summary>
    /// <returns></returns>
    private string GenerateSubSubjectOrderXML()
    {
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("SubSubjects");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "SubSubjects", "");

        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount < grdSubSubject.Rows.Count; iRowCount++)
        {
            HtmlSelect oDropDownList = (HtmlSelect)grdSubSubject.Rows[iRowCount].FindControl("ddlOrder");
            // Create root xml element.
            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "SubSubjects", "");

            string sAtrrName = "SubSubjectId";
            XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = grdSubSubject.DataKeys[iRowCount][0].ToString();
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

    private void FillSubSubjectGridView(int ModuleId, int SubjectId)
    {
        divSubSubjectGrid.Visible = true;
        int iStandardId = Convert.ToInt32(hidStandardId.Value);
        List<PrePrimaryProgressReportSubSubjects> olstPrePrimaryProgressReportSubSubjects = PrePrimaryProgressSheetConfigBL.GetConfiguredSubSubject(miSchoolId, miAcademicYearId, iStandardId, ModuleId, SubjectId);
        if (olstPrePrimaryProgressReportSubSubjects.Count() <= 0)
            imgBtnSave.Visible = false;
        else
            imgBtnSave.Visible = true;
        grdSubSubject.DataSource = olstPrePrimaryProgressReportSubSubjects;
        grdSubSubject.DataBind();
    }

    #endregion
}
