/* File Name - ClasswiseOptionalSubjectsUI.aspx.cs
 * Created Date - 
 * Created by - 
 * Class Description - This class is used transfering student marks fom one optional subject to another.
 * 
 * Modified By :- Vipul
 * Date :- 3 July 2012
 * Description - To allow optional subjects and subject groups for selection.
 */

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using MasterEntities;
using SchoolEntities;
using Utility;

/// <summary>
/// This class is used to configure optional subjects.
/// </summary>
public partial class ClasswiseOptionalSubjectsUI : SchoolBase
{
    #region "Constants"
    private const string S_MESSAGE = "Optional subject(s) {0} ";
    private const string S_SAVE_MESSAGE = "saved";
    private const string S_DELETE_MESSAGE = "deleted";
    private const string S_UPDATE_MESSAGE = "updated";
    private const string S_SAVE = "Save";
    private const string S_UPDATE = "Update";
    #endregion

    #region "Data member"

    private ClasswiseOptionalSubjectBL moClasswiseOptionalSubjectBL = null;

    #endregion

    #region "Event"

    /// <summary>
    /// This event is used to set default values.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Initialize();
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                if (CheckPreCondition())
                {
                    FillStandardCombobox();
                }
                RefreshValue();
                SetJavaScriptAttributes();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill optional subjects listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillOptionalSubjectListview();
            FillOptionalSubjectsDetailsListview();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set values to listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwClassWiseOptionalSubject_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                CheckBox chkSelect = oCurrentItem.FindControl("ChkSelect") as CheckBox;
                CheckBox chkIsDefault = oCurrentItem.FindControl("chkIsDefault") as CheckBox;

                var oOptionalSubject = oCurrentItem.DataItem as OptionalSubject;

                //if (hidMode.Value != S_SAVE)
                //{
                    chkSelect.Checked = oOptionalSubject.OptionalSubjectsId != Constants.I_ZERO;
                    chkIsDefault.Checked = chkSelect.Checked && oOptionalSubject.IsDefault;
                    if (chkSelect.Checked)
                        chkIsDefault.InputAttributes.Remove("disabled");
                    else
                        chkIsDefault.InputAttributes.Add("disabled", "disabled");
                //}
                //else
                //{
                //    chkIsDefault.Enabled = false;
                //    chkIsDefault.InputAttributes.Add("disabled", "disabled");
                //}
                
               
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save optional subject details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();
            if (!(QueryString[Constants.S_IS_CONFIGURED] == Constants.S_YES))
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.OptionalSubject));
            lblUpdateSucess.Text = String.Format(Resources.LocalizedResources.OptionalSubjectsSuccessfully, btnSave.Text == Resources.LocalizedResources.Save ? Resources.LocalizedResources.Saved : Resources.LocalizedResources.Updated);
        }
        catch (SqlException oEx)
        {
			string sMessage = CommonUtility.ModifyExceptionMessage(oEx.Message, "Subject", Resources.LocalizedResources.Subject, "can not be removed since it is associated with", Resources.LocalizedResources.valRemoveText); ;
			lblErrorMessage.Text = CommonUtility.ModifyExceptionMessage(sMessage, "Subject", Resources.LocalizedResources.Subject, "can not be added since it is associated with", Resources.LocalizedResources.valCanNotBeAdded); ;
            FillOptionalSubjectListview();
            FillOptionalSubjectsDetailsListview();

        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to update and delete optional subject groups.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwOptionalSubjectDetalis_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            int iParentOptionalSubjectId = lstvwOptionalSubjectDetalis.DataKeys[oCurrentItem.DisplayIndex]["ParentOptionalSubjectId"].ToInt();
            if (e.CommandName == "UpdateSubjectGroup")
            {

                btnSave.Text = hidMode.Value = Resources.LocalizedResources.Update;
                FillOptionalSubjectListview(iParentOptionalSubjectId);
                FillOptionalSubjectsDetailsListview();
                hidParentOptionalSubjectGroupId.Value = iParentOptionalSubjectId.ToString();
                hidNoOfSubjects.Value = txtNoOfSubjects.Text = ((Label)oCurrentItem.FindControl("lblNoOfSubjects")).Text;
                hidOptionalSubjectGroupName.Value = txtOptionalSubjectGrouptName.Text = ((Label)oCurrentItem.FindControl("lblGroupName")).Text;
            }
            else if (e.CommandName == "RemoveSubjectGroup")
            {
                int iCount = moClasswiseOptionalSubjectBL.Delete(iParentOptionalSubjectId);
				lblUpdateSucess.Text = String.Format(Resources.LocalizedResources.OptionalSubjectsSuccessfully, Resources.LocalizedResources.DeleteOptionalSubject);
                FillOptionalSubjectListview(iParentOptionalSubjectId);
                FillOptionalSubjectsDetailsListview();
                if (iCount == 0)
                    DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.OptionalSubject));
            }
        }
        catch (ReferenceExceptions oEx)
        {	string sMessage = CommonUtility.ModifyExceptionMessage(oEx.Message, "", "", "Optional subject group cannot be removed since", Resources.LocalizedResources.CanNotRemoveOptionalSubject);
			sMessage = CommonUtility.ModifyExceptionMessage(sMessage, "", "", "Marks assignment is already done for subject(s)", Resources.LocalizedResources.CanNotRemoveOptionalSubjectReason1);
			sMessage = CommonUtility.ModifyExceptionMessage(sMessage, "", "", "Timetable is configured for subject(s)", Resources.LocalizedResources.CanNotRemoveOptionalSubjectReason3);
			
			lblErrorMessage.Text = sMessage = CommonUtility.ModifyExceptionMessage(sMessage, "", "", "Students are associated with subject(s)", Resources.LocalizedResources.CanNotRemoveOptionalSubjectReason2); ;
            FillOptionalSubjectListview();
            FillOptionalSubjectsDetailsListview();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set confirmation message for deleting optional subject group.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwOptionalSubjectDetalis_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                ImageButton oimgbtnDelete = oCurrentItem.FindControl("imgBtnDelete") as ImageButton;
                oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to show optional subject details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwOptionalSubjectDetalis_DataBound(object sender, EventArgs e)
    {
        try
        {
            divOptionalSubjectDetalis.Visible = lstvwOptionalSubjectDetalis.Items.Count > 0;
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    #endregion

    #region "Private members"
    /// <summary>
    /// This method is used to save optional subject details.
    /// </summary>
    private void Save()
    {
        int iStandardWiseDivisionId = Convert.ToInt32(cmbClass.SelectedValue);
        string sXml = PopulateOptionalSubjectDetails();
        moClasswiseOptionalSubjectBL.Save(sXml);
        FillOptionalSubjectListview();
        FillOptionalSubjectsDetailsListview();
    }

    /// <summary>
    /// This method is used to set default controls.
    /// </summary>
    private void SetDefaultControls()
    {
        btnSave.Text = hidMode.Value = Resources.LocalizedResources.Save; 
        hidOptionalSubjectGroupName.Value = txtOptionalSubjectGrouptName.Text = string.Empty;
        hidNoOfSubjects.Value = txtNoOfSubjects.Text = string.Empty;
        hidParentOptionalSubjectGroupId.Value = Constants.S_ZERO;
    }

    /// <summary>
    /// This method is used to fill optional subject details.
    /// </summary>
    private void FillOptionalSubjectsDetailsListview()
    {
        int iStandardWiseDivisionId = Convert.ToInt32(cmbClass.SelectedValue);
        lstvwOptionalSubjectDetalis.DataSource = moClasswiseOptionalSubjectBL.GetAll();
        lstvwOptionalSubjectDetalis.DataBind();
    }

    /// <summary>
    /// This method is used to fill optional subjects listview. 
    /// </summary>
    private void FillOptionalSubjectListview(int aiParentOptionalSubjectId = 0)
    {
        int iStandardWiseDivisionId = Convert.ToInt32(cmbClass.SelectedValue);
        lstvwClassWiseOptionalSubject.DataSource = moClasswiseOptionalSubjectBL.GetAllChildSubjects(aiParentOptionalSubjectId);
        lstvwClassWiseOptionalSubject.DataBind();
        SetControlStates();
        SetDefaultControls();
    }

    /// <summary>
    /// This method is used to fill standard combobox.
    /// </summary>
    private void FillStandardCombobox()
    {
        List<StandardMaster> oStandard = StandardCollectionBL.GetAll(miSchoolId, miAcademicYearId);
        ListSource.FillDropDownList(oStandard, cmbClass, "StandardName", "StandardId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
       // valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        //btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Basic_Configuration));
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Basic_Configuration));
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnBack });
        btnSave.Attributes.Add("onclick", "return ValidateSubjects();");
    }

    /// <summary>
    /// This method is used to set control states based on no. of optional subjects.
    /// </summary>
    private void SetControlStates()
    {
        btnSave.Enabled = Convert.ToInt32(cmbClass.SelectedValue) != Constants.I_ZERO;
        cmbClass.Focus();

        if (lstvwClassWiseOptionalSubject.Items.Count > 0)
        {
            divContainer.Visible = true;
            trNoRecordMsg.Visible = false;
            HtmlTableRow oHtmlTableRow = (HtmlTableRow)lstvwClassWiseOptionalSubject.FindControl("trHeader");
            CheckBox chkSelectAll = oHtmlTableRow.FindControl("ChkSelectAll") as CheckBox;
            chkSelectAll.Checked = false;
            trLegend.Visible = true;
            btnSave.Enabled = true;
        }
        else
        {
            btnSave.Enabled = false;
            trLegend.Visible = false;
            divContainer.Visible = false;
            trNoRecordMsg.Visible = true;
        }
    }

    /// <summary>
    /// This method is used to populate optional subject details.
    /// </summary>
    /// <returns></returns>
    private string PopulateOptionalSubjectDetails()
    {
        List<OptionalSubject> oOptionalSubjectDetails = new List<OptionalSubject>();
        int iRowId = 0;
        foreach (ListViewDataItem oDataItem in lstvwClassWiseOptionalSubject.Items)
        {
            iRowId = Convert.ToInt32(oDataItem.DisplayIndex);
            CheckBox chkSelect = oDataItem.FindControl("ChkSelect") as CheckBox;
            CheckBox chkIsDefault = oDataItem.FindControl("chkIsDefault") as CheckBox;
            int iOptionalSubjectsId = Convert.ToInt32(lstvwClassWiseOptionalSubject.DataKeys[iRowId]["OptionalSubjectsId"]);
            bool bIsDefault = lstvwClassWiseOptionalSubject.DataKeys[iRowId]["IsDefault"].ToBool();
            bool bIsGroupDetailsChanged = hidNoOfSubjects.Value != txtNoOfSubjects.Text || hidOptionalSubjectGroupName.Value != txtOptionalSubjectGrouptName.Text;

            // if ( New || To be Deleted || Default subject changed || Group Details Changed )
            if ((chkSelect.Checked && iOptionalSubjectsId == Constants.I_ZERO) || (!chkSelect.Checked && iOptionalSubjectsId != 0) || (chkSelect.Checked && bIsDefault != chkIsDefault.Checked) || (chkSelect.Checked && bIsGroupDetailsChanged))
            {
                oOptionalSubjectDetails.Add(new OptionalSubject()
                                           {
                                               SubjectId = lstvwClassWiseOptionalSubject.DataKeys[iRowId]["SubjectId"].ToInt(),
                                               ParentOptionalSubjectId = hidParentOptionalSubjectGroupId.Value.ToInt(),
                                               ChildOptionalSubjectId = lstvwClassWiseOptionalSubject.DataKeys[iRowId]["ParentOptionalSubjectId"].ToInt(),
                                               SubjectGroupId = lstvwClassWiseOptionalSubject.DataKeys[iRowId]["SubjectGroupId"].ToInt(),
                                               IsDefault = chkIsDefault.Checked,
                                               NoOfSubjects = txtNoOfSubjects.Text.ToInt(),
                                               OptionalSubjectName = txtOptionalSubjectGrouptName.Text,
                                               OptionalSubjectsId = iOptionalSubjectsId,
                                               Action = !chkSelect.Checked ? Constants.Action.Delete : Constants.Action.Insert,
                                           });
            }
        }
        return CommonUtility.GenerateXml(oOptionalSubjectDetails);
    }

    /// <summary>
    /// This function checks the preconditons of Exams.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;

        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.OptionalSubject);
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

    /// <summary>
    /// This method is used to hide controls if pre-coditoin is not satisfied.
    /// </summary>
    private void VisibleOrHideControls()
    {
        tblOptionalSubTable.Visible = false;
        divOptionalSubjectDetalis.Visible = false;
        btnSave.Visible = false;
    }

    /// <summary>
    /// This method is used to set initialize the page.
    /// </summary>
    private void Initialize()
    {
        InitializeMemberVariables();
        moClasswiseOptionalSubjectBL = new ClasswiseOptionalSubjectBL(miSchoolId, miAcademicYearId, cmbClass.SelectedValue.IsNullOrEmpty() ? Constants.I_ZERO : cmbClass.SelectedValue.ToInt());
        trLegend.Visible = false;
        divOptionalSubjectDetalis.Visible = false;
        divContainer.Visible = false;
        trNoRecordMsg.Visible = false;
        btnSave.Enabled = false;
    }
    /// <summary>
    /// This method used to refresh value based on Culture
    /// </summary>
    private void RefreshValue()
    {
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidAreYouSureYouWantToDeleteThisRecords.Value = Resources.LocalizedResources.AreYouSureYouWantToDeleteThisRecords;
        hidAtLeastOneOptionalSubjectShouldBeSelected.Value = Resources.LocalizedResources.ValAtLeastOneOptionalSubjectShouldBeSelected;
        hidDefaultSubjectCanBeSelected.Value = Resources.LocalizedResources.DefaultSubjectCanBeSelected;
        hidAtLeast.Value = Resources.LocalizedResources.Atleast;
        hidAtMost.Value = Resources.LocalizedResources.AtMost;
        hidOptionalSubjectGroupNameShouldNotBeDuplicated.Value = Resources.LocalizedResources.OptionalSubjectGroupNameShouldNotBeDuplicated;
    }
    private void ClearFields()
    {
        cmbClass.ClearSelection();
        txtNoOfSubjects.Text = string.Empty;
        txtOptionalSubjectGrouptName.Text = string.Empty;
        btnSave.Text = S_SAVE;
    }
    #endregion
}
