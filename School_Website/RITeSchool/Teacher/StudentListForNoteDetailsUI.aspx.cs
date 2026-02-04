using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities.Teacher;
using Utility;

public partial class StudentListForNoteDetailsUI : SchoolBase
{
    #region Data Member(s)

    private StudentListForNoteDetailsBL moStudentListForNoteDetailsBL;

    #endregion

    #region Event(s)

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStudentListForNoteDetailsBL = new StudentListForNoteDetailsBL(miSchoolId, miUserId, miAcademicYearId);
            if (!IsPostBack)
            {
                SetDefaultValues();
                FillStandardDropDown();
                FillDivisionDropDown();
                FillStudentListListView();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(cmbStandard.SelectedValue) == Constants.I_ZERO)
            {
                cmbDivision.Items.Clear();
                cmbDivision.Items.Add(new ListItem { Text = Constants.S_SELECT, Value = Constants.S_ZERO });
                ResetListview();
            }
            else
            {
                FillDivisionDropDown();
                ResetListview();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwStudentList_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iSchoolWiseStudentId = Convert.ToInt32(lstvwStudentList.DataKeys[oCurrentItem.DisplayIndex]["SchoolWiseStudentId"]);

                HiddenField hidData = e.Item.FindControl("hidData") as HiddenField;
                hidData.Value = CommonUtility.EncryptQuerystring("SchoolWiseStudentId=" + iSchoolWiseStudentId);

                ImageButton btnEdit = e.Item.FindControl("btnEdit") as ImageButton;
                btnEdit.Attributes.Add("onclick", "OpenPopup("+e.Item.DisplayIndex+",'"+ hidData.ClientID + "'); return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillStudentListListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    private void FillStudentListListView()
    {
        int iStandardId = Convert.ToInt32(cmbStandard.SelectedValue);
        int iDivisionId = Convert.ToInt32(cmbDivision.SelectedValue);
        if (iStandardId != Constants.I_ZERO && iDivisionId != Constants.I_ZERO)
        {
            List<StudentListForNoteDetails> lstStudentDetails = moStudentListForNoteDetailsBL.GetAllStudentList(iStandardId, iDivisionId);
            lstvwStudentList.DataSource = lstStudentDetails;
            lstvwStudentList.DataBind();
        }
        else
        {
            ResetListview();
        }
    }

    /// <summary>
    /// This method is used to fill up Standard combo box.
    /// </summary>
    private void FillStandardDropDown()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandard = oStandardCollectionBL.GetAssociatedStandards();

        if (moUserRole == Constants.UserRoles.Admin || hidHasFullAccess.Value == Constants.S_YES)
        {
            ListSource.FillDropDownList(oDtStandard, cmbStandard, "standard_name", "standard_id", Constants.S_SELECT);
        }
        else
        {
            MasterDataCollectionBL obj = new MasterDataCollectionBL();
            DataTable dtClassTeachers = obj.GetAllClassTeachers(miSchoolId, miAcademicYearId);
            
            DataRow[] drArr = dtClassTeachers.Select("Teacher_Id="+ Session[Constants.S_SESSION_TEACHER_ID].ToString());
            if (drArr.Length > 0)
            {
                oDtStandard = drArr.CopyToDataTable();
                ListSource.FillDropDownList(oDtStandard, cmbStandard, "standard_name", "standard_id", Constants.S_SELECT);

                if (oDtStandard.Rows.Count == 1)
                {
                    cmbStandard.SelectedIndex = 1;
                    cmbStandard_SelectedIndexChanged(cmbStandard, new EventArgs());
                    cmbStandard.Enabled = false;
                }
            }
        }        
    }

    /// <summary>
    /// This method fills combobox with Divisions
    /// </summary>    
    private void FillDivisionDropDown()
    {
        try
        {
            int aiStandardId = Convert.ToInt32(cmbStandard.SelectedValue);
            if (aiStandardId != 0)
            {
                DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
                DataTable oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(aiStandardId);

                if (moUserRole == Constants.UserRoles.Admin || hidHasFullAccess.Value == Constants.S_YES)
                {
                    ControlUtility.FillDropDownList(oDSStandardCollection, ref cmbDivision,
                                                   Constants.S_DIVISION_ID_FIELD,
                                                   Constants.S_DIVISION_NAME_FIELD,
                                                   Constants.S_SELECT);
                }
                else
                {
                    MasterDataCollectionBL obj = new MasterDataCollectionBL();
                    DataTable dtClassTeachers = obj.GetAllClassTeachers(miSchoolId, miAcademicYearId);

                    DataRow[] drArr = dtClassTeachers.Select("Teacher_Id=" + Session[Constants.S_SESSION_TEACHER_ID].ToString());
                    if (drArr.Length > 0)
                    {
                        oDSStandardCollection = drArr.CopyToDataTable();
                        ListSource.FillDropDownList(oDSStandardCollection, cmbDivision, "Division_Name", "Division_Id", Constants.S_SELECT);

                        if (oDSStandardCollection.Rows.Count == 1)
                        {
                            cmbDivision.SelectedIndex = 1;
                            cmbDivision_SelectedIndexChanged(cmbDivision, new EventArgs());
                            cmbDivision.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                cmbDivision.Items.Clear();
                cmbDivision.Items.Add(new ListItem { Text = Constants.S_SELECT, Value = Constants.S_ZERO });
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private void ResetListview()
    {
        lstvwStudentList.DataSource = null;
        lstvwStudentList.DataBind();
    }

    private void SetDefaultValues()
    {
        hidHasFullAccess.Value = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.StudentListForActivityDetails).ToString();
    }

    #endregion
}