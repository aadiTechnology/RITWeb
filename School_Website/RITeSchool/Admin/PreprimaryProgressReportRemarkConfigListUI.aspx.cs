
//--------------------------------------------------------------------------------------------------------
// Class Name       :- PreprimaryProgressReportRemarkConfigListUI
// Purpose          :- This class is used to manage PreprimaryProgressReportRemarkConfigList details.
// Date Of creation :- 3/3/2011
// Author Name      :- Shobha 
//--------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Reflection;
using BusinessLogic.Exceptions;
using BusinessLogic;
using Utility;
using System.Xml;
using ProgressReportEntities;

public partial class PreprimaryRemarksUI : SchoolBase
{
    #region Constants

    const string S_DATAKEY_REMARK_ID = "PrePrimaryProgressReportRemarkId";
    const string S_DATAKEY_ORIGINAL_REMARK_ID = "OriginalPrePrimaryProgressReportRemarkId";
    const string S_DATAKEY_SCHOOL_ID = "SchoolId";
    const string S_SELECT_CHECKBOX = "ChkSelect";
    const string S_REMARK_TEXTBOX = "txtRemark";

    #endregion

    #region Data Members

    int miRowCount = 0;

    #endregion

    #region "Events"

    /// <summary>
    /// This event is used to initialise the controls.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
                FillRemarksList();                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set the check box checked for saved remarks.
    /// </summary>
    protected void lstvwPrePrimaryProgressRemark_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);

                // If the school id is not the default id i.e. -9999 that means the subject name is already assigned
                // to the school. Thus check the checkbox.

                CheckBox chkSelect = ((CheckBox)oCurrentItem.FindControl(S_SELECT_CHECKBOX));
                if (lstvwPrePrimaryProgressRemark.DataKeys[iRowId][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
                    chkSelect.Checked = true;
                DropDownList cmbSortOrder = oCurrentItem.FindControl("cmbSortOrder") as DropDownList;
                cmbSortOrder.Items.Add(new ListItem(Constants.S_SELECT, "0"));
                for (int iRowNo = 1; iRowNo <= miRowCount; iRowNo++)
                {
                    cmbSortOrder.Items.Add(iRowNo.ToString());
                    cmbSortOrder.DataTextField = "PrePrimaryProgressReportRemarkId";
                    cmbSortOrder.DataValueField = "PrePrimaryProgressReportRemarkId";
                }
                cmbSortOrder.SelectedValue = lstvwPrePrimaryProgressRemark.DataKeys[iRowId]["SortOrder"].ToString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save remarks.
    /// </summary>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            trErrorMessage.Visible = false;
            Save();

            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Exam_Related)));
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            //Display RI check message and reset changes.
            trErrorMessage.Visible = true;
            lblErrorMessage.Text = ex.Message;
            FillRemarksList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region "Private Methods"

    /// <summary>
    /// This method is used to set the default attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        valSummRemarks.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        BtnSave.Attributes.Add("onclick", "if(!CheckSelectedRemark(this)) return false;");        
        ApplyMouseHoverEffect(new List<Button> { BtnSave, btnCancel});
        btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Exam_Related));
    }

    /// <summary>
    /// This method is used to fill the remark listview.
    /// </summary>
    private void FillRemarksList()
    {
        List<PrePrimaryRemark> lstPrePrimaryRemarkList = PrePrimaryRemarksBL.GetAll(miSchoolId, miAcademicYearId);
        miRowCount = lstPrePrimaryRemarkList.Count();
        lstvwPrePrimaryProgressRemark.DataSource = lstPrePrimaryRemarkList;
        lstvwPrePrimaryProgressRemark.DataBind();
    }

    /// <summary>
    /// This method is used to save, update or delete the remark details.
    /// </summary>
    private void Save()
    {
        List<PrePrimaryRemark> lstPrePrimaryRemarkList = new List<PrePrimaryRemark>();

        for (int iItemCount = 0; iItemCount < lstvwPrePrimaryProgressRemark.Items.Count; iItemCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwPrePrimaryProgressRemark.Items[iItemCount];
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);

            TextBox txtRemark = (TextBox)oCurrentItem.FindControl(S_REMARK_TEXTBOX);
            CheckBox oChkRemark = (CheckBox)oCurrentItem.FindControl(S_SELECT_CHECKBOX);
            DropDownList ocmbSortOrder = (DropDownList)oCurrentItem.FindControl("cmbSortOrder");
            if (oChkRemark.Checked == true)
            {
                PrePrimaryRemark oPrePrimaryRemarkConfig = PopulateBL();
                oPrePrimaryRemarkConfig.PrePrimaryProgressReportRemarkName = txtRemark.Text.Trim();
                oPrePrimaryRemarkConfig.SortOrder = Convert.ToInt32(ocmbSortOrder.SelectedValue);
                if (oChkRemark.Checked == true && lstvwPrePrimaryProgressRemark.DataKeys[iItemCount][S_DATAKEY_SCHOOL_ID].ToString() == Constants.S_DEFAUL_SCHOOL_ID)
                {
                    oPrePrimaryRemarkConfig.Action = Constants.Action.Insert;
                    oPrePrimaryRemarkConfig.OriginalPrePrimaryProgressReportRemarkId = Convert.ToInt32(lstvwPrePrimaryProgressRemark.DataKeys[iItemCount][S_DATAKEY_REMARK_ID]);
                }
                if (oChkRemark.Checked == true && lstvwPrePrimaryProgressRemark.DataKeys[iItemCount][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
                {
                    oPrePrimaryRemarkConfig.Action = Constants.Action.Update;
                    oPrePrimaryRemarkConfig.OriginalPrePrimaryProgressReportRemarkId = Convert.ToInt32(lstvwPrePrimaryProgressRemark.DataKeys[iItemCount][S_DATAKEY_ORIGINAL_REMARK_ID]);
                    oPrePrimaryRemarkConfig.PrePrimaryProgressReportRemarkId = Convert.ToInt32(lstvwPrePrimaryProgressRemark.DataKeys[iItemCount][S_DATAKEY_REMARK_ID]);
                }
                lstPrePrimaryRemarkList.Add(oPrePrimaryRemarkConfig);
            }
            else if (oChkRemark.Checked == false && lstvwPrePrimaryProgressRemark.DataKeys[iItemCount][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
            {
                PrePrimaryRemark oPrePrimaryRemarkConfig = PopulateBL();
                oPrePrimaryRemarkConfig.Action = Constants.Action.Delete;
                oPrePrimaryRemarkConfig.PrePrimaryProgressReportRemarkName = txtRemark.Text.Trim();
                oPrePrimaryRemarkConfig.PrePrimaryProgressReportRemarkId = Convert.ToInt32(lstvwPrePrimaryProgressRemark.DataKeys[iItemCount][S_DATAKEY_REMARK_ID]);
                lstPrePrimaryRemarkList.Add(oPrePrimaryRemarkConfig);
            }
        }
        if (lstPrePrimaryRemarkList.Count > 0)
        {
            PrePrimaryRemarksBL oPrePrimaryProgressReportRemarksBL = new PrePrimaryRemarksBL();
            oPrePrimaryProgressReportRemarksBL.Update(lstPrePrimaryRemarkList, miSchoolId, miAcademicYearId);
        }
        string sIsConfig = ReadQuerystring();
        if (sIsConfig != "Y")
            SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.PrePrimaryProgressReportRemarkConfiguration));
    }

    /// <summary>
    /// This method is used to populate PreprimaryProgressreportRemarkconfiglist BL properties.
    /// </summary>
    /// <returns></returns>
    private PrePrimaryRemark PopulateBL()
    {
        PrePrimaryRemark oPrePrimaryRemarkConfig = new PrePrimaryRemark
        {
            InsertedById = miUserId,
            UpdatedById = miUserId,
            InsertDate = System.DateTime.Now.ToString(),
            UpdateDate = System.DateTime.Now.ToString(),
            SchoolId = miSchoolId,
            AcademicYearId = miAcademicYearId
        };
        return oPrePrimaryRemarkConfig;
    }

    /// <summary>
    /// This method is used to decrypt querystring to set remarks config details.
    /// </summary>
    /// <returns></returns>
    private string ReadQuerystring()
    {
        return QueryString["Is_Configured"];
    }
    
    #endregion
}
