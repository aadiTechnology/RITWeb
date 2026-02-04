// File Name  : PrePrimaryProgressReportConfigList.aspx.cs
// Created By : Shankar
// Description :This class provided preprimary progress report configuration.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using MasterEntities;
using Utility;

public partial class PrePrimaryProgressReportConfigList : SchoolBase
{
    #region Constant

    private const int I_PK_HEADER_ID = 0;
    private const int I_COLOUMN_INDEX_HEADER = 0;
	private const int I_COL_INDEX_SUBHEADER = 3;
    private const string S_CMD_NAME_DELETE_HEADER = "DELETE_HEADER";
    private const string S_PAGE_SUBHEADER_DETAILS = "PrePrimaryProgressReportConfig.aspx";
    
	#endregion

	#region DataMmember

	private int miStandard_Id = 0;

	#endregion

	#region Events

	/// <summary>
    /// This method is used to fill Headers's list grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
			if (CheckPreCondition())
			{
				ReadQuerystring();
				if (!IsPostBack)
				{
					SetDefaultProperties();
					btnAdd.Visible = false;
					FillStandardCombobox();
				}
			}

	        btnCopy.Attributes.Add("Onclick", "if(!(ConfirmCopyAction())){return false;}");
            ApplyMouseHoverEffect(new List<Button> { btnAdd, btnBack, btnCopy });
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to navigate control to control panel page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            if (QueryString != null && QueryString["ParentHeading_Id"] != null && QueryString["ParentHeading_Id"].ToInt() != 0)
            {
                string sQuerystring = "StandardId=" + cmbStandard.SelectedValue;
                string sEncrypt = CommonUtility.EncryptQuerystring(sQuerystring);
                
				MasterPage oMasterPage = (MasterPage)this.Master;
				oMasterPage.RedirectToNextPage("~/RITeSchool/Admin/PrePrimaryProgressReportConfigList.aspx?" + sEncrypt);
            }
            else
            {
                MasterPage oMasterPage = (MasterPage)this.Master;
                oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Exam_Related)));
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to add for same school.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string sQuerystring = "Is_Configured=" + hidIsConfig.Value;
            string sEncrypt = CommonUtility.EncryptQuerystring(sQuerystring);
            string sRedirectUrl = S_PAGE_SUBHEADER_DETAILS + "?" + sEncrypt;
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(sRedirectUrl);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle standard combo change.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbStandard.SelectedIndex > 0)
        {
            miStandard_Id = Convert.ToInt32(cmbStandard.SelectedValue);
            FillPageData();
            btnAdd.Visible = true;
			hidStandard.Value = cmbStandard.SelectedValue;
        }
        else
        {
            btnAdd.Visible = false;
            trCopyStandard.Visible = false;
            grdHeaders.Visible = false;
        }
    }

    private void FillPageData()
    {
        string sQuerystring = "Mode=" + hidMode.Value + "&StandardId=" + cmbStandard.SelectedValue + "&ParentHeading_Id=" + hidHeaderId.Value + "&IsConfig=" + hidIsConfig.Value;
        string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQuerystring);
        btnAdd.Attributes.Add("onclick", "window.open('" + S_PAGE_SUBHEADER_DETAILS + "?" + sEncrypt+ "', '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=650,height=370');return false;");
		FillHeaderListGrid();
        btnAdd.Enabled = true;
            trCopyStandard.Visible = true;

	    List<StandardMaster> lstStandards = GetStandardList();
	    FillCopyStandardCombobox(lstStandards);
    }

    protected void btnCopy_Click(object sender, EventArgs e)
    {
        try
        {
            int iSrcStandard = Convert.ToInt32(cmbStandard.SelectedValue);
            int iDestStandard = Convert.ToInt32(cmbCopyStandard.SelectedValue);
            PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
            oPrePrimaryProgressSheetConfigBL.CopyConfiguration(iSrcStandard, iDestStandard);

            cmbStandard.SelectedValue = cmbCopyStandard.SelectedValue;
            miStandard_Id = Convert.ToInt32(cmbCopyStandard.SelectedValue);
            FillPageData();
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            lblErrorMsg.Text = ex.Message;
            lblErrorMsg.Visible = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion Events

    #region Grid Events

    /// <summary>
    /// This method is used for sorting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdHeaders_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            if (hidSortDirection.Value == Constants.S_DESCENDING)
                hidSortDirection.Value = Constants.S_ASCENDING;
            else
                hidSortDirection.Value = Constants.S_DESCENDING;

            FillHeaderListGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to set sortImage.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdHeaders_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView oGridviewName = (GridView)sender;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                int iSortColumnIndex = CommonUtility.GetSortColumnIndex(oGridviewName, hidSortExpression.Value);

                if (iSortColumnIndex != -1)
                {
                    CommonUtility.AddSortImage(iSortColumnIndex, e.Row, hidSortDirection.Value);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdHeaders_RowCommand(object sender, GridViewCommandEventArgs e)
    {
		switch (e.CommandName)
        {
            case S_CMD_NAME_DELETE_HEADER:
                try
                {
                    Int32 iRowIndex = Convert.ToInt32(e.CommandArgument);

                    string sReturn = ReferenceBL.CheckDependenciesAndGetErrorMessages(Convert.ToInt32(Constants.ReferenceId.PrePrimaryProgrssSheetConf), Convert.ToInt32(cmbStandard.SelectedValue), "", miAcademicYearId);
                    if (sReturn.Equals(""))
                    {
                        PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
                        oPrePrimaryProgressSheetConfigBL.Heading_Id = Convert.ToInt32(grdHeaders.DataKeys[iRowIndex][I_PK_HEADER_ID].ToString());
                        oPrePrimaryProgressSheetConfigBL.DeletePrePrimaryProgressSheetConfig();
                        FillHeaderListGrid();

                        if (grdHeaders.Rows.Count == 0)
                            DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.PrePrimaryProgrssSheetConf));
                    }
                    else
                    {
                        throw new ReferenceExceptions(sReturn);
                    }
                }
                catch (ReferenceExceptions ex)
                {
                    lblErrorMsg.Text = ex.Message;
                    FillHeaderListGrid();
                }
                catch (Exception ex)
                {
                    ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
                }
                break;
        }
    }

    /// <summary>
    /// This method is used to set attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdHeaders_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            const Int32 I_COL_INDEX_EDIT = 1;
            const Int32 I_COL_INDEX_DELETE = 2;

            int iRowIndex = e.Row.RowIndex;
            if (iRowIndex >= 0)
            {
                int iHeaderId = Convert.ToInt32(grdHeaders.DataKeys[iRowIndex][I_PK_HEADER_ID]);
                Image oEditDetails = (Image)e.Row.Cells[I_COL_INDEX_EDIT].Controls[Constants.I_ZERO];
                string sQuerystring = "HeaderId=" + iHeaderId + "&Mode=" + hidMode.Value + "&StandardId=" + cmbStandard.SelectedValue + "&ParentHeading_Id=" + hidHeaderId.Value
                    + "&StandardName=" + cmbStandard.SelectedItem.Text;
                string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQuerystring);
                oEditDetails.Attributes.Add("onclick", "window.open('" + S_PAGE_SUBHEADER_DETAILS + "?" + sEncrypt
                                                                  + "', '_new','scrollbars=yes,resizable=no,top=0,left=0,width=650,height=370');return false;");
                Image oDelete = (Image)e.Row.Cells[I_COL_INDEX_DELETE].Controls[Constants.I_ZERO];
                oDelete.Attributes.Add("Onclick", "if(!(ConfirmAction('" + grdHeaders.Columns[I_COLOUMN_INDEX_HEADER].HeaderText + "'))){return false;}");

                if (hidMode.Value != "SubHeader")
                {
                    String strUrl;
                    string sQueryString;
                    HyperLink oHyperLinkField = (HyperLink)(e.Row.Cells[I_COL_INDEX_SUBHEADER].Controls[0]);                    
                    strUrl = oHyperLinkField.NavigateUrl;
                    sQueryString = strUrl.Substring(strUrl.IndexOf("?") + 1) + "&StandardId=" + cmbStandard.SelectedValue + "&StandardName=" + cmbStandard.SelectedItem.Text;
                    oHyperLinkField.NavigateUrl = strUrl.Substring(0, strUrl.IndexOf("?") + 1) + CommonUtility.EncryptQuerystring(sQueryString);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion Grid Events

    #region Private Methods

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        try
        {
            if (QueryString["Is_Configured"] != null)
                hidIsConfig.Value = QueryString["Is_Configured"];

            if (QueryString["ParentHeading_Id"] != null && QueryString["ParentHeading_Id"].ToInt() != 0)
            {
                hidHeaderId.Value = QueryString["ParentHeading_Id"];
                tblStdCmb.Visible = false;
                tblHeading.Visible = true;
                lblmandatory.Visible = false;
                PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL(Convert.ToInt32(hidHeaderId.Value));
                lblHeading.Text = oPrePrimaryProgressSheetConfigBL.Heading_Text;
            }
            if (QueryString["Mode"] != null)
                hidMode.Value = QueryString["Mode"];

            if (QueryString["StandardName"] != null)
                lblStandardName.Text = QueryString["StandardName"];
            if (QueryString["StandardId"] != null)
                miStandard_Id = QueryString["StandardId"].ToInt();
            
			string sQuerystring = "Mode=" + hidMode.Value + "&StandardId=" + miStandard_Id.ToString() + "&ParentHeading_Id=" + hidHeaderId.Value + "&IsConfig=" + hidIsConfig.Value + "&StandardName=" + lblStandardName.Text;
            string sEncrypt = CommonUtility.EncryptQuerystring(sQuerystring);
            btnAdd.Attributes.Add("onclick", "window.open('" + S_PAGE_SUBHEADER_DETAILS + "?" + sEncrypt + "', '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=650,height=370');return false;");
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
			oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
    }

    ///<Summary>
    ///This method is used to set default properties to controls.
    ///</Summary>
    private void SetDefaultProperties()
    {
        ValSummaryErrMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidSortExpression.Value = grdHeaders.Columns[I_COLOUMN_INDEX_HEADER].SortExpression;
        hidSortDirection.Value = Constants.S_ASCENDING;
    }

    ///<Summary>
    /// This method is used to fill  grid
    ///</Summary>
    private void FillHeaderListGrid()
    {
		if (cmbStandard.SelectedIndex > 0)
		{
			if (hidMode.Value == "SubHeader")
			{
				grdHeaders.Columns[I_COL_INDEX_SUBHEADER].Visible = false;
				grdHeaders.Columns[I_COLOUMN_INDEX_HEADER].HeaderText = "Skills";
				btnAdd.Enabled = true;
				trCopyStandard.Visible = false;
			}
			else
			{
				miStandard_Id = Convert.ToInt32(cmbStandard.SelectedValue);
				trCopyStandard.Visible = true;
				List<StandardMaster> lstStandards = GetStandardList();
				FillCopyStandardCombobox(lstStandards);
			}
		}

		PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
        DataTable oDTUserDetails = oPrePrimaryProgressSheetConfigBL.FetchPrePrimaryProgressSheetConfigDetails(Convert.ToInt32(hidHeaderId.Value)
            , miStandard_Id,miAcademicYearId,miSchoolId);
        oDTUserDetails.DefaultView.Sort = hidSortExpression.Value + " " + hidSortDirection.Value;
        grdHeaders.DataSource = oDTUserDetails.DefaultView;
        grdHeaders.DataBind();
        grdHeaders.Visible = true;       
    }

	/// <summary>
	/// This method is used to fill copy standard dropdown list.
	/// </summary>
	/// <param name="aLstStandards"></param>
	private void FillCopyStandardCombobox(List<StandardMaster> aLstStandards)
	{
		List<StandardMaster> lstConfiguredStd =
			aLstStandards.Where(standards => standards.StandardId != cmbStandard.SelectedValue.ToInt()).ToList();
		ListSource.FillDropDownList(lstConfiguredStd, cmbCopyStandard, "StandardName", "StandardId", Constants.S_SELECT);
	}

	/// <summary>
	/// This method returns list of standards.
	/// </summary>
	/// <returns></returns>
	private List<StandardMaster> GetStandardList()
	{
		StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
		return oStandardCollectionBL.GetStandardsForExamConfiguration(false);
	}
	
	/// <summary>
    /// This function fills combobox with standards
    /// </summary>
    private void FillStandardCombobox()
	{
		List<StandardMaster> lstStandards = GetStandardList();
		ListSource.FillDropDownList(lstStandards, cmbStandard, "StandardName", "StandardId", Constants.S_SELECT);

		if (lstStandards != null && lstStandards.Count > 0)
        {
            cmbStandard.SelectedIndex = 0;
            btnAdd.Visible = false;
        }
   
        if (QueryString != null && QueryString["StandardId"] != null)
        {
            cmbStandard.SelectedValue = QueryString["StandardId"];
            miStandard_Id = cmbStandard.SelectedValue.ToInt();
            btnAdd.Visible = true;
            btnAdd.Enabled = true;

			if (lstStandards != null)
				FillCopyStandardCombobox(lstStandards);
			
	        FillHeaderListGrid();
        }
    }

    private bool CheckPreCondition()
    {
        bool bReturn = false;

        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.PrePrimaryProgrssSheetConf);
        if (sLinks.Equals(string.Empty))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            VisibleOrHideControls();
        }

        return bReturn;
    }

    private void VisibleOrHideControls()
    {
        btnAdd.Visible = false;
        lblErrorMsg.Visible = false;
        lblmandatory.Visible = false;
        tblHeading.Visible = false;
        grdHeaders.Visible = false;
        trCombo.Visible = false;
    }

    #endregion
}
