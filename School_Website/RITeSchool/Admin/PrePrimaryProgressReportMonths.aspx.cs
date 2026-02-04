using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Reflection;
using BusinessLogic.Exceptions;
using System.Web.UI.WebControls;
using ProgressReportEntities;
using BusinessLogic;
using Utility;

public partial class PrePrimaryProgressReportMonths : SchoolBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
				if(CheckPrecondition()) {
					FillPrePrimaryStandardCombo();
					btnSave.Visible = false;
					valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER; 
				}

                btnSave.Attributes["onclick"] = "javascript:ResetUpdateLbl()";                
                ApplyMouseHoverEffect(new List<Button> { btnSave, btnBack});
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This function checks the Precondition requirement for this screen.
	/// ie If the Junior OR Senior KG standard is configured for the school
    /// </summary>
    /// <returns></returns>
    private bool CheckPrecondition() {
		bool bResult = false;		
		bResult = ReferenceBL.IsPrePrimaryStdConfigured(miSchoolId);
		if(bResult) {
			divErr.Visible = false;
		}
		else {
			trRow1.Visible = false;
			trRow2.Visible = false;
			btnSave.Visible = false;
		}
		
		return bResult;
    }

    private void FillPrePrimaryStandardCombo()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId,miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetConfiguredPrePrimaryStandards();
        ControlUtility.FillDropDownList(oDtStandardCollection, ref ddlStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       Constants.S_SELECT);
    }

    private string ReadQuerystring()
    {
        return QueryString["Is_Configured"];
    }
    
    private void FillMonthsGrid()
    {
        List<PrePrimaryProgressReportMonth> olstPrePrimaryProgressReportMonths  = PrePrimaryProgressReportMonthsBL.GetMonthsList(miSchoolId,miAcademicYearId,Convert.ToInt32(ddlStandard.SelectedValue)).ToList();
        lstvwConfigureMonth.DataSource = olstPrePrimaryProgressReportMonths;
        lstvwConfigureMonth.DataBind();
        divSortOrder.Visible = EnableSortOrder(olstPrePrimaryProgressReportMonths);
        SetSortOrderQueryString();
    }

    private void SetSortOrderQueryString()
    {
        if (divSortOrder.Visible)
        {
            string sQueryString = "SchoolId=" + miSchoolId;
            sQueryString += "&AcademicYear=" + miAcademicYearId;
            sQueryString += "&StandardId=" + ddlStandard.SelectedValue;
            string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);             
             hlnkSortOrder.Attributes.Add("onclick", "window.open('" + hlnkSortOrder.NavigateUrl + "?" + sEncrypt
                                                  + "' , '_blank','scrollbars=yes,resizable=yes,top=0,left=0,width=950,height=600'); return false;");
        }
    }

    private bool EnableSortOrder(List<PrePrimaryProgressReportMonth> olstPrePrimaryProgressReportMonths)
    {
        int iCount = olstPrePrimaryProgressReportMonths.Where(month => month.PrePrimaryProgressReportMonthId > 0).Count();
        if (iCount > 0)
            return true;
        else
            return false;
    }
    protected void ddlStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Text = "";
            if (ddlStandard.SelectedValue != "0")
            {
                FillMonthsGrid();
                tblMonthList.Visible = true;
                btnSave.Visible = true;
            }
            else
            {
                tblMonthList.Visible = false;
                divSortOrder.Visible = false;
                btnSave.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private string GetMonthsXML()
    {
        CheckBox oChkIsMonthSelected;
        CheckBox oChkIsCommentable;
        const string S_ELEMENT = "element";
        string sAttribute;
        XmlDocument oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement oRoot = oDoc.CreateElement("ConfigureMonth");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "ConfigureMonth", "");
        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount <= lstvwConfigureMonth.Items.Count - 1; iRowCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwConfigureMonth.Items[iRowCount];
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            int iPrePrimaryProgressReportMonthId = Convert.ToInt32(lstvwConfigureMonth.DataKeys[iRowId]["PrePrimaryProgressReportMonthId"]);
            int iMonthId = Convert.ToInt32(lstvwConfigureMonth.DataKeys[iRowId]["MonthId"]);
            string sMonth = Convert.ToString(lstvwConfigureMonth.DataKeys[iRowId]["Month"]);
            string sMonthAbbreviation = Convert.ToString(lstvwConfigureMonth.DataKeys[iRowId]["MonthAbbreviation"]);

            oChkIsCommentable = (CheckBox)oCurrentItem.FindControl("chkComment");
            oChkIsMonthSelected = (CheckBox)oCurrentItem.FindControl("ChkSelect");
            TextBox txtAbbreviation = (TextBox)oCurrentItem.FindControl("txtAbbreviation");
            TextBox txtComment = (TextBox)oCurrentItem.FindControl("txtComment");

            if ((oChkIsMonthSelected.Checked && iPrePrimaryProgressReportMonthId == 0) || iPrePrimaryProgressReportMonthId > 0)
            {
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "ConfigureMonth", "");
                sAttribute = "PrePrimaryProgressReportMonthId";
                XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = iPrePrimaryProgressReportMonthId.ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "MonthId";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = iMonthId.ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "MonthAbbreviation";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = txtAbbreviation.Text.Trim();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "StandardId";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = ddlStandard.SelectedValue;
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "SchoolId";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = Convert.ToString(miSchoolId);
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "AcademicYearId";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = Convert.ToString(miAcademicYearId);
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "Is_Deleted";
                oAttr = oDoc.CreateAttribute(sAttribute);
                if (oChkIsMonthSelected.Checked)
                    oAttr.Value = "0";
                else
                    oAttr.Value = "1";
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "InsertedById";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = Convert.ToString(miUserId);
                oXmlNode.Attributes.Append(oAttr);

                if ((oChkIsCommentable.Checked))
                {
                    sAttribute = "IsCommentable";
                    oAttr = oDoc.CreateAttribute(sAttribute);
                    oAttr.Value = "1";
                    oXmlNode.Attributes.Append(oAttr);

                    sAttribute = "CommentAbbreviation";
                    oAttr = oDoc.CreateAttribute(sAttribute);
                    oAttr.Value = txtComment.Text.Trim();
                    oXmlNode.Attributes.Append(oAttr);
                }
                else
                {
                    sAttribute = "IsCommentable";
                    oAttr = oDoc.CreateAttribute(sAttribute);
                    oAttr.Value = "0";
                    oXmlNode.Attributes.Append(oAttr);
                }
                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
            }
        }
        // Add the root node to document element.         
        oRoot.AppendChild(oXmlRootNode);
        return oRoot.InnerXml;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Text = "";
            string MonthXML = GetMonthsXML();
            PrePrimaryProgressReportMonthsBL.Save(MonthXML);
            FillMonthsGrid();
            lblUpdateSucess.Visible = true;
            lblUpdateSucess.Text = "Months configuration saved successfully!!!";
            string sIsConfigured = ReadQuerystring();
            if (sIsConfigured != "Y")
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.PrePrimaryMonthsConfiguration));
        }
        catch (SqlException ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            FillMonthsGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwConfigureMonth_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                int iPrePrimaryProgressReportMonthId = Convert.ToInt32(lstvwConfigureMonth.DataKeys[iRowId]["PrePrimaryProgressReportMonthId"]);
                string sCommentAbbreviation = lstvwConfigureMonth.DataKeys[iRowId]["CommentAbbreviation"].ToString();                
                CheckBox oChkSelect = e.Item.FindControl("ChkSelect") as CheckBox;
                CheckBox oChkComment = e.Item.FindControl("chkComment") as CheckBox;
                if (iPrePrimaryProgressReportMonthId > 0)
                    oChkSelect.Checked = true;
                else
                    oChkSelect.Checked = false;
                if (sCommentAbbreviation != "")
                    oChkComment.Checked = true;
                else
                    oChkComment.Checked = false;

            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Exam_Related)));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
}
