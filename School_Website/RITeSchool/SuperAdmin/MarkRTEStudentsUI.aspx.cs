using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Data.SqlClient;
using SuperAdminEntities;
using Utility;
using SchoolEntities;
using System.Data;
using System.Web.UI.HtmlControls;


public partial class MarkRTEStudentsUI : SchoolBase
{
    #region Constants
    private const string S_SAVE_MESSAGE_FOR_RTE = "Student(s) successfully marked as RTE.";
    private const string S_SAVE_MESSAGE_FOR_NONRTE = "Student(s) successfully marked as Non-RTE.";
    #endregion

    #region --EVENTS--
    /// <summary>
    /// /// <summary>
    /// This event is used to set the page Load Events.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>    
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                FillStandardCombobox();
                SetButtonValue();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
  
    /// <summary>
    /// This event is used for Saving RTE/NONRTE Students. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            CheckBox chkSelect;
            StringBuilder sbStudentId = new StringBuilder();
            string sStudentId = string.Empty;
            foreach (ListViewDataItem oCurrentItem in lstvwStudentRTE1.Items)
            {
                chkSelect = oCurrentItem.FindControl("chkSelect") as CheckBox;
                if (chkSelect.Checked)
                    sbStudentId = sbStudentId.Append("," + lstvwStudentRTE1.DataKeys[oCurrentItem.DisplayIndex]["StudentId"].ToString());
            }
            if (sbStudentId.ToString().StartsWith(","))
                sStudentId = sbStudentId.ToString().Substring(1);
            SuperAdminDetailsBL oSuperAdminDetailsBL = new SuperAdminDetailsBL();
            oSuperAdminDetailsBL.Save(sStudentId, miSchoolId, miAcademicYearId, miUserId, optRTE.Checked);
            if (optNONRTE.Checked)
            base.DisplayMessage(S_SAVE_MESSAGE_FOR_RTE, false, tdMessage);
            else base.DisplayMessage(S_SAVE_MESSAGE_FOR_NONRTE, false, tdMessage);
                FillStudentListview();
        }
        catch (SqlException ex)
        {
            base.DisplayMessage(ex.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is used Fill divisions combbox and Listview as per the Conditions mentioned.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillDivisions();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This Event is used For Search Student .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillStudentListview();

            HtmlTableRow trHeader = lstvwStudentRTE1.FindControl("trHeader") as HtmlTableRow;
            if (trHeader != null)
            {
                CheckBox CheckBoxSelect = trHeader.FindControl("CheckBoxSelect") as CheckBox;
                if (CheckBoxSelect != null)
                    CheckBoxSelect.Checked = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to bound data for paging.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentRTE1_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwStudentRTE1.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwStudentRTE1, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used for paging comboobx .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwStudentRTE1);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Methods
    /// <summary>
    /// This method is used for standard combobox.
    /// </summary>
    private void FillStandardCombobox()
    {
        var oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        cmbStandard.Bind(oDtStandardCollection, Utility.Constants.S_STANDARD_ID_FIELD, Utility.Constants.S_STANDARD_NAME_FIELD, Utility.Constants.S_SELECT_ALL);
        cmbDivision.Items.Insert(Constants.I_ZERO, new ListItem(Constants.S_SELECT_ALL,"0"));
    }
    /// <summary>
    ///    //This method is used to fill current division's combo.
    /// </summary>
    private void FillDivisions()
    {        
        int aiStandardId = cmbStandard.SelectedValue.ToInt();
        DivisionCollectionBL oDiv = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtClass = oDiv.GetAllDivisionsForStandard(aiStandardId);
        ControlUtility.FillDropDownList(oDtClass, ref cmbDivision,
                                       "division_Id",
                                       Constants.S_DIVISION_NAME_FIELD,
                                       Constants.S_SELECT_ALL);
    }
    /// <summary>
    /// This method is used Fill listview of RET/NON RTE Students.
    /// </summary>
    /// <param name="isRTE"></param>
    private void FillStudentListview()
    {
        lstvwStudentRTE1.DataSourceID = ObjDSlstvwStudentRTE1.ID;
        lstvwStudentRTE1.DataBind();

        if (lstvwStudentRTE1.Items.Count > 0)
            btnSave.Enabled = true;
        else
            btnSave.Enabled = false;
    }
    /// <summary>
    /// This methods is used for set value for back button.
    /// </summary>
    private void SetButtonValue()
    {
        btnBack.PostBackUrl = "../SuperAdmin/ScreensUI.aspx";
        optNONRTE.Checked = true;
        btnSave.Enabled = false;
    }
 #endregion    
}