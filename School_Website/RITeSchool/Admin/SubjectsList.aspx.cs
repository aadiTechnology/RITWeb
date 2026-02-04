using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic.Exceptions;
using BusinessLogic;
using Utility;

public partial class SubjectsList : SchoolBase
{
    private string IsConfig;

    #region Constants

    const string S_SELECT_AT_LEAST_ONE_Subject = "At least one subject name should be selected for saving.";

    #endregion

    #region Events
    /// <summary>
    /// This page Load Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            hidSchoolId.Value = Convert.ToString(miSchoolId);
            hidConfigType.Value = Convert.ToInt32(Constants.BasicSchoolConfigurationType.Subject).ToString();
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                RefreshValue();
                FillSubjectGridView();
                btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Basic_Configuration));
                btnCancel.Attributes["onclick"] = "javascript:DisableButtons()";                
                ApplyMouseHoverEffect(new List<Button> { imgBtnSave, btnCancel });
                
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Method is used Gridview row databound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdGroupDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                // If the school id is not the default id i.e. -9999 that means the Subject is already assigned
                // to the school. Thus check the checkbox.

                if (grdSubjects.DataKeys[e.Row.RowIndex]["School_Id"].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
                    ((CheckBox)e.Row.FindControl("ChkBoxDelete")).Checked = true;
                
                Control oControl = e.Row.FindControl("ddlOrder");
                if (Convert.ToBoolean(grdSubjects.DataKeys[e.Row.RowIndex]["Is_CoCurricularActivity"].ToString()))
                    ((CheckBox)e.Row.FindControl("ChkBoxIsCoCurricularActivity")).Checked = true;
                if (Convert.ToBoolean(grdSubjects.DataKeys[e.Row.RowIndex]["IsAttitudeSubject"].ToString()))
                    ((CheckBox)e.Row.FindControl("chkIsAttitudeSubject")).Checked = true;

                if (oControl != null)
                {
                    HtmlSelect oDropDownList = (HtmlSelect)oControl ;
                    oDropDownList.Name = "ViewOrder" + e.Row.RowIndex;
                    DataView oDataView = (DataView)grdSubjects.DataSource;
                    for (int iCnt = 0; iCnt < oDataView.Table.Rows.Count; iCnt++)
                    {
                        ListItem oListItem = new ListItem((iCnt + 1).ToString(), (iCnt + 1).ToString());
                        oDropDownList.Items.Add(oListItem);
                        if (iCnt == e.Row.RowIndex)
                            oListItem.Selected = true;
                    }
                    oDropDownList.Attributes.Add("onchange", "Reorder(this, '" + grdSubjects.ClientID + "'," + e.Row.RowIndex + ", " + oDataView.Table.Rows.Count + ")");
                }
                CheckBox ChkBoxDelete = (CheckBox)e.Row.FindControl("ChkBoxDelete");
                CheckBox ChkBoxIsCoCurricularActivity = (CheckBox)e.Row.FindControl("ChkBoxIsCoCurricularActivity");
                CheckBox chkIsAttitudeSubject = (CheckBox)e.Row.FindControl("chkIsAttitudeSubject");
                ChkBoxIsCoCurricularActivity.Attributes.Add("onclick", "EnableAttitudeField(this,"+chkIsAttitudeSubject.ClientID + ")");
                ChkBoxDelete.Attributes.Add("onclick", "EnableCheck(this," + ChkBoxIsCoCurricularActivity.ClientID + "," + chkIsAttitudeSubject.ClientID + ")");
                
            }          
            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This is for Save button 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgBtnSave_Click(object sender, EventArgs e)
    {
        // Save the changes to database.
        try
        {
            CheckBox Deleteflag = new CheckBox();
            CheckBox chkIsCoCurricularActivity = new CheckBox();
            CheckBox chkIsAttitudeSubject = new CheckBox();
            Collection<SubjectMasterBL> oSubjects = new Collection<SubjectMasterBL>();
            string sErrorMessage = GetErrorMessage(miSchoolId, miAcademicYearId);
            if (sErrorMessage == string.Empty)
            {
                for (int i = 0; i < grdSubjects.Rows.Count; i++)
                {
                    Deleteflag = (CheckBox)grdSubjects.Rows[i].FindControl("ChkBoxDelete");
                    TextBox txtPrefixEdit = ((TextBox)grdSubjects.Rows[i].FindControl("txtSubjectName"));
					TextBox txtShortName = grdSubjects.Rows[i].FindControl("txtShortName") as TextBox;
                    
                    chkIsCoCurricularActivity = (CheckBox)grdSubjects.Rows[i].FindControl("ChkBoxIsCoCurricularActivity");
                    chkIsAttitudeSubject = (CheckBox)grdSubjects.Rows[i].FindControl("chkIsAttitudeSubject");

                    // Check if new Subject is being inserted.
                    // I.e. If the checkbox is checked and the school id is -9999 then it is the new Subject being
                    // introduced.
                    if (Deleteflag.Checked == true && grdSubjects.DataKeys[i]["School_Id"].ToString() == Constants.S_DEFAUL_SCHOOL_ID)
                    {
                        SubjectMasterBL oSubjectMasterBL = GetCommonSubjectMasterBL(txtPrefixEdit.Text, txtShortName.Text.Trim(),
                                                               Convert.ToInt32(grdSubjects.DataKeys[i]["Original_Subject_Id"].ToString()),
                                                               chkIsCoCurricularActivity.Checked,
                                                               chkIsAttitudeSubject.Checked);
                        oSubjectMasterBL.ConfigurationAction = Constants.Action.Insert;
                        oSubjects.Add(oSubjectMasterBL);
                    }

                    // Check if existing Subject name is being updated.
                    // I.e. If the checkbox is checked and the school is not -9999 and the value in text box differs 
                    // from the value in the Subject name column then update the existing Subject name.
                    else if (Deleteflag.Checked == true &&
                            grdSubjects.DataKeys[i]["School_Id"].ToString() != Constants.S_DEFAUL_SCHOOL_ID &&
                            grdSubjects.DataKeys[i]["Subject_Name"].ToString() != txtPrefixEdit.Text.Trim())
                    {
						SubjectMasterBL oSubjectMasterBL = GetCommonSubjectMasterBL(txtPrefixEdit.Text, txtShortName.Text.Trim(),
                                                                                Convert.ToInt32(grdSubjects.DataKeys[i]["Original_Subject_Id"].ToString()),
                                                                                chkIsCoCurricularActivity.Checked,
                                                                                chkIsAttitudeSubject.Checked);
                        oSubjectMasterBL.ConfigurationAction = Constants.Action.Update;
                        oSubjectMasterBL.SubjectId = Convert.ToInt32(grdSubjects.DataKeys[i]["Subject_Id"].ToString());
                        oSubjects.Add(oSubjectMasterBL);
                    }

                    // Check if existing Subject is being removed.
                    // I.e. If the checkbox is NOT checked and the school id is not -9999. 
                    // In such case need to check if any of the related data is entered for the unchecked Subject then
                    // the warning message should be given to user and the related data should be removed from db.
					else if(Deleteflag.Checked == false && grdSubjects.DataKeys[i]["School_Id"].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
                    {
						SubjectMasterBL oSubjectMasterBL = GetCommonSubjectMasterBL(txtPrefixEdit.Text, txtShortName.Text.Trim(),
																			   Convert.ToInt32(grdSubjects.DataKeys[i]["Original_Subject_Id"].ToString()),
                                                                               chkIsCoCurricularActivity.Checked,
                                                                               chkIsAttitudeSubject.Checked);
                        oSubjectMasterBL.ConfigurationAction = Constants.Action.Delete;
						oSubjectMasterBL.SubjectId = Convert.ToInt32(grdSubjects.DataKeys[i]["Subject_Id"].ToString());
                        oSubjects.Add(oSubjectMasterBL);
                    }
                    else if (Deleteflag.Checked == true &&
							grdSubjects.DataKeys[i]["School_Id"].ToString() != Constants.S_DEFAUL_SCHOOL_ID &&
							grdSubjects.DataKeys[i]["Subject_Name"].ToString() == txtPrefixEdit.Text.Trim())
                    {
						SubjectMasterBL oSubjectMasterBL = GetCommonSubjectMasterBL(txtPrefixEdit.Text, txtShortName.Text.Trim(),
																				Convert.ToInt32(grdSubjects.DataKeys[i]["Original_Subject_Id"].ToString()),
                                                                                chkIsCoCurricularActivity.Checked,
                                                                                chkIsAttitudeSubject.Checked);
                        oSubjectMasterBL.ConfigurationAction = Constants.Action.Update;
						oSubjectMasterBL.SubjectId = Convert.ToInt32(grdSubjects.DataKeys[i]["Subject_Id"].ToString());
                        oSubjects.Add(oSubjectMasterBL);
                    }
                }
                // If there are Subjects to be deleted then give warning message to user about the same. 
                // Update database with the configured Subjects.          

                if (oSubjects.Count > 0)
                {
                    SubjectCollectionBL oSubjectCollectionBL = new SubjectCollectionBL(miSchoolId,miAcademicYearId);
                    oSubjectCollectionBL.UpdateSubjects(oSubjects, miAcademicYearId);
                }

                ReadQuerystring();
                if (IsConfig != "Y")
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.Subjects));
        
                MasterPage oMasterPage = (MasterPage)this.Master;
                oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Basic_Configuration)));
            }
            else
            {
                lblErr.Visible = true;
                lblErr.Text = sErrorMessage;
                FillSubjectGridView();
            }
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            lblErr.Text = CommonUtility.ModifyExceptionMessage(ex.Message, string.Empty, string.Empty, "can not be removed since associated with", Resources.LocalizedResources.valRemoveText);
            FillSubjectGridView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is for grid view page index change
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

    #endregion

    #region Private Methods


    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        try
        {
            IsConfig = QueryString["Is_Configured"];
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
			oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
    }

    /// <summary>
    /// This is for fill the gridview
    /// </summary>
    private void FillSubjectGridView()
    {
        // This method fills the Grid with available Group details.
        SubjectCollectionBL oSubjectCollectionBL = new SubjectCollectionBL(miSchoolId, miAcademicYearId);

        DataTable oDSUserDetails = oSubjectCollectionBL.GetAllSubject();
        grdSubjects.DataSource = oDSUserDetails.DefaultView;
        grdSubjects.DataBind();
        imgBtnSave.Attributes.Add("Onclick", "if(!(ConfirmAction('" + grdSubjects.AllowPaging + "','" + hidsSelectAtLeastOneSubject.Value + "'))){return false;}");
    }

    /// <summary>
    /// This is for getting common subject
    /// </summary>
    /// <param name="asSubjectName"></param>
    /// <param name="asShortName"></param>
    /// <param name="aiOriginalFieldId"></param>
    /// <param name="bIsCoCurricularActivity"></param>
    /// <returns></returns>
    private SubjectMasterBL GetCommonSubjectMasterBL(string asSubjectName, string asShortName, int aiOriginalFieldId,bool bIsCoCurricularActivity, bool abIsAttitudeSubject)
    {
        // This method creates the default object for the configuration and returns the same.        
        SubjectMasterBL oSubjectMasterBL = new SubjectMasterBL();
        oSubjectMasterBL.SchoolId = miSchoolId;
        oSubjectMasterBL.AcademicYearId = miAcademicYearId;
        oSubjectMasterBL.UpdatedById = miUserId;
        oSubjectMasterBL.InsertedByid = miUserId;
		oSubjectMasterBL.SubjectName = asSubjectName;
		oSubjectMasterBL.ShortName = asShortName;
        oSubjectMasterBL.OriginalSubjectId = aiOriginalFieldId;
        oSubjectMasterBL.IsCoCurricularActivity = bIsCoCurricularActivity;
        oSubjectMasterBL.IsAttitudeSubject = abIsAttitudeSubject;
        return oSubjectMasterBL;
    }

    /// <summary>
    /// This is for error message
    /// </summary>
    /// <param name="iSchoolId"></param>
    /// <param name="iAcademicYearId"></param>
    /// <returns></returns>
    private string GetErrorMessage(int iSchoolId, int iAcademicYearId)
    {
        string sErrorList = string.Empty;
        int iIsMarksAssigned = 0;

        for (int i = 0; i < grdSubjects.Rows.Count; i++)
        {
            CheckBox Deleteflag = new CheckBox();
            CheckBox chkIsCoCurricularActivity = new CheckBox();
            chkIsCoCurricularActivity = (CheckBox)grdSubjects.Rows[i].FindControl("ChkBoxIsCoCurricularActivity");
            Deleteflag = (CheckBox)grdSubjects.Rows[i].FindControl("ChkBoxDelete");
            TextBox txtPrefixEdit = ((TextBox)grdSubjects.Rows[i].FindControl("txtSubjectName"));

            int iIsCoCurricularFlag = Convert.ToInt32(grdSubjects.DataKeys[i]["Is_CoCurricularActivity"]);
            if (Deleteflag.Checked == true)
            {
                if ((iIsCoCurricularFlag == 1 && chkIsCoCurricularActivity.Checked == false) || (iIsCoCurricularFlag == 0 && chkIsCoCurricularActivity.Checked == true))
                {
                    int iSubId = Convert.ToInt32(grdSubjects.DataKeys[i]["Subject_Id"]);
                    iIsMarksAssigned = SubjectMasterBL.CheckMarksAssigned(iSubId, iSchoolId, iAcademicYearId);
                    if (iIsMarksAssigned > 0)
                        sErrorList = sErrorList + Resources.LocalizedResources.TheIsCoCurricularActivityValueFor + txtPrefixEdit.Text + Resources.LocalizedResources.CanNotBeModifiedSinceMarksAreAlreadyAssigned;
                }
            }
        }
        if (sErrorList.StartsWith(","))
            sErrorList = sErrorList.Substring(1);
        return sErrorList;
    }
    /// <summary>
    /// This method used to value based on Culture
    /// </summary>
    private void RefreshValue()
    {
        ValidationSummary1.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidShortNameDuplicated.Value = Resources.LocalizedResources.ShortNameShouldNotBeDuplicated;
        hidsSelectAtLeastOneSubject.Value = Resources.LocalizedResources.sSelectAtLeastOneSubject;
        imgBtnSave.Attributes.Add("Onclick", "if(!(ConfirmAction('" + grdSubjects.AllowPaging + "','" + hidsSelectAtLeastOneSubject.Value + "'))){return false;}");
    }
    #endregion
}
