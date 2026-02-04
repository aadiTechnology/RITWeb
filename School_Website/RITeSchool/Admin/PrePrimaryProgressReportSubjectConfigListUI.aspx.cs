//--------------------------------------------------------------------------------------------------------
// Class Name       :- PrePrimaryProgressReportSubjectConfigListUI
// Purpose          :- This class is used to manage PrePrimaryProgressReportSubjectConfigList details.
// Date Of creation :- 1/3/2011
// Author Name      :- Shobha 
//--------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Reflection;
using BusinessLogic.Exceptions;
using BusinessLogic;
using Utility;
using System.Xml;
using ProgressReportEntities;

public partial class PrePrimarySubjectsConfigUI : SchoolBase
{
    #region Constants

    const string S_DATAKEY_SUBJECT_ID = "PrePrimarySubjectId";
    const string S_DATAKEY_ORIGINAL_SUBJECT_ID = "OriginalPrePrimarySubjectId";
    const string S_DATAKEY_SCHOOL_ID = "SchoolId";
    const string S_SELECT_CHECKBOX = "ChkSelect";
    const string S_SUBJECT_TEXTBOX = "txtSubject";
    const string S_SUBJECT_DISPLAY_NAME = "txtDisplayName";

    #endregion

    #region Data Members

    int miRowCount = 0;

    #endregion

    #region "Events"

    /// <summary>
    /// This event is used to initialise the controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
                FillSubjectList();                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set the check box checked for saved subjects.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPrePrimarySubjects_ItemDataBound(object sender, ListViewItemEventArgs e)
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
                CheckBox oChkIsVisibleonReport = (CheckBox)oCurrentItem.FindControl("ChkIsVisibleonReport");
                if (lstvwPrePrimarySubjects.DataKeys[iRowId][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
                    chkSelect.Checked = true;

                if (chkSelect.Checked && lstvwPrePrimarySubjects.DataKeys[iRowId]["IsVisibleInReport"].ToString() != "0")
                    oChkIsVisibleonReport.Checked = true;
                else
                    oChkIsVisibleonReport.Checked = false;
                    chkSelect.Attributes.Add("onclick", "VisibleControls(" + iRowId + ");");

                DropDownList cmbSortOrder = oCurrentItem.FindControl("cmbSortOrder") as DropDownList;
                cmbSortOrder.Items.Add(new ListItem(Constants.S_SELECT, "0"));
                for (int iRowNo = 1; iRowNo <= miRowCount; iRowNo++)
                    cmbSortOrder.Items.Add(iRowNo.ToString());
                cmbSortOrder.SelectedValue = lstvwPrePrimarySubjects.DataKeys[iRowId]["SortOrder"].ToString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save pre-primary subjects.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
            FillSubjectList();
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
        BtnSave.Attributes.Add("onclick", "if(!CheckSelectedSubject(this)) return false;");
        valSummSubjects.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;        
        ApplyMouseHoverEffect(new List<Button> { BtnSave, btnCancel});
        btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Exam_Related));
    }

    /// <summary>
    /// This method is used to fill the subject listview,
    /// </summary>
    private void FillSubjectList()
    {
        List<PrePrimarySubject> lstPrePrimarySubjects = PrePrimarySubjectsBL.GetAll(miSchoolId, miAcademicYearId);
        miRowCount = lstPrePrimarySubjects.Count();
        lstvwPrePrimarySubjects.DataSource = lstPrePrimarySubjects;
        lstvwPrePrimarySubjects.DataBind(); 
    }

    /// <summary>
    /// This method is used to populate PreprimaryProgressreportconfiglist BL properties.
    /// </summary>
    /// <returns></returns>
    private PrePrimarySubject PopulateBL()
    {
        PrePrimarySubject oPrePrimarySubjectsConfig = new PrePrimarySubject
        {
            InsertedById = miUserId,
            UpdatedById = miUserId,
            InsertDate = System.DateTime.Now.ToString(),
            UpdateDate = System.DateTime.Now.ToString(),
            SchoolId = miSchoolId,
            AcademicYearId = miAcademicYearId
        };
        return oPrePrimarySubjectsConfig;
    }

    /// <summary>
    /// This method is used to save, update or delete the subject details.
    /// </summary>
    private void Save()
    {
        List<PrePrimarySubject> lstPrePrimarySubjectsList = new List<PrePrimarySubject>();

        for (int iItemCount = 0; iItemCount < lstvwPrePrimarySubjects.Items.Count; iItemCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwPrePrimarySubjects.Items[iItemCount];
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);

            TextBox txtSubject = (TextBox)oCurrentItem.FindControl(S_SUBJECT_TEXTBOX);
            CheckBox oChkIsVisibleonReport = (CheckBox)oCurrentItem.FindControl("ChkIsVisibleonReport");
            DropDownList ocmbSortOrder = (DropDownList)oCurrentItem.FindControl("cmbSortOrder");
            CheckBox oChkSubject = (CheckBox)oCurrentItem.FindControl(S_SELECT_CHECKBOX);
            if (oChkSubject.Checked == true)
            {
                PrePrimarySubject oPrePrimarySubjectConfig = PopulateBL();
                oPrePrimarySubjectConfig.PrePrimarySubjectName = txtSubject.Text.Trim();
                oPrePrimarySubjectConfig.IsVisibleInReport = Convert.ToInt32(oChkIsVisibleonReport.Checked);
                oPrePrimarySubjectConfig.ModuleId = Convert.ToInt32(lstvwPrePrimarySubjects.DataKeys[iItemCount]["ModuleId"]);
                oPrePrimarySubjectConfig.SortOrder = Convert.ToInt32(ocmbSortOrder.SelectedValue);
                if (lstvwPrePrimarySubjects.DataKeys[iItemCount][S_DATAKEY_SCHOOL_ID].ToString() == Constants.S_DEFAUL_SCHOOL_ID)
                {
                    oPrePrimarySubjectConfig.Action = Constants.Action.Insert;
                    oPrePrimarySubjectConfig.OriginalPrePrimarySubjectId = Convert.ToInt32(lstvwPrePrimarySubjects.DataKeys[iItemCount][S_DATAKEY_SUBJECT_ID]);
                }
                else if (lstvwPrePrimarySubjects.DataKeys[iItemCount][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
                {
                    oPrePrimarySubjectConfig.Action = Constants.Action.Update;
                    oPrePrimarySubjectConfig.OriginalPrePrimarySubjectId = Convert.ToInt32(lstvwPrePrimarySubjects.DataKeys[iItemCount][S_DATAKEY_ORIGINAL_SUBJECT_ID]);
                    oPrePrimarySubjectConfig.PrePrimarySubjectId = Convert.ToInt32(lstvwPrePrimarySubjects.DataKeys[iItemCount][S_DATAKEY_SUBJECT_ID]);
                }
                lstPrePrimarySubjectsList.Add(oPrePrimarySubjectConfig);
            }
            else if (oChkSubject.Checked == false && lstvwPrePrimarySubjects.DataKeys[iItemCount][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
            {
                PrePrimarySubject oPrePrimarySubjectConfig = PopulateBL();
                oPrePrimarySubjectConfig.Action = Constants.Action.Delete;
                oPrePrimarySubjectConfig.PrePrimarySubjectName = txtSubject.Text.Trim();
                oPrePrimarySubjectConfig.PrePrimarySubjectId = Convert.ToInt32(lstvwPrePrimarySubjects.DataKeys[iItemCount][S_DATAKEY_SUBJECT_ID]);
                lstPrePrimarySubjectsList.Add(oPrePrimarySubjectConfig);
            }
        }
        if (lstPrePrimarySubjectsList.Count > 0)
        {
            PrePrimarySubjectsBL oPrePrimarySubjectConfigBL = new PrePrimarySubjectsBL();
            oPrePrimarySubjectConfigBL.Update(lstPrePrimarySubjectsList, miAcademicYearId, miSchoolId);
        }
        string sIsConfig = ReadQuerystring();
        if (sIsConfig != "Y")
            SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.PrePrimarySubjectsConfiguration));
    }

    /// <summary>
    /// This method is used to decrypt querystring to set subject config details.
    /// </summary>
    /// <returns></returns>
    private string ReadQuerystring()
    {
        return QueryString["Is_Configured"];
    }

    #endregion
}
