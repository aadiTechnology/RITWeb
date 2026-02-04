using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities.Admin;
using Utility;
using StudentEntities;

public partial class UpdateStudentDetailsInBulkUI : SchoolBase
{
    #region Constant(s)

    private const string S_SAVE = " details updated successfully !!!";

    #endregion

    #region Data Member(s)

    private UpdateStudentDetailsInBulkBL moUpdateStudentDetailsInBulkBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        try
        {
            if (hidSortExpression.Value == string.Empty)
            {
                hidSortExpression.Value = "Roll_No";
                hidSortDirection.Value = Constants.S_ASCENDING;
            }

            AddSortImage(lstvwStudentDetails, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moUpdateStudentDetailsInBulkBL = new UpdateStudentDetailsInBulkBL(miSchoolId, miUserId, miAcademicYearId);
            if (!IsPostBack)
            {
                FillCategory();
                FillStandardDropDown();
                FillDivisionDropDown();
                FillOperators();
                GetPrefixes();                
                SetJavascriptAttributes();
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
            cmbDivision.Items.Clear();
            cmbDivision.Items.Add(new ListItem(Constants.S_SELECT, Constants.I_ZERO.ToString()));

            if (Convert.ToInt32(cmbStandard.SelectedValue) == Constants.I_ZERO)
            {
                FillListView(true);                
            }
            else
            {
                FillDivisionDropDown();
                FillListView(true);                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnShow.Text.ToUpper() == "SHOW")
            {
                hidSortDirection.Value = Constants.S_ASCENDING;
                FillListView(false);
                SetFieldState(false);
                if (optExact.Checked)
                {
                    cmbOperation.Enabled = true;
                    cmbPrefix.Enabled = true;
                }
            }
            else
            {
                SetFieldState(true);
                FillListView(true);                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event used set paging for list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwStudentDetails);            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                List<UpdateStudentDetailsInBulk> lstUpdateStudentDetailsInBulk = new List<UpdateStudentDetailsInBulk>();
                for (int iRowCount = 0; iRowCount < lstvwStudentDetails.Items.Count; iRowCount++)
                {
                    int iId = lstvwStudentDetails.DataKeys[iRowCount]["YearWise_Student_Id"].ToInt();
                    TextBox txtNewValue = lstvwStudentDetails.Items[iRowCount].FindControl("txtNewValue") as TextBox;
                    CheckBox chkSelect = lstvwStudentDetails.Items[iRowCount].FindControl("chkSelect") as CheckBox;

                    if (chkSelect.Checked)
                    {
                        UpdateStudentDetailsInBulk oUpdateStudentDetailsInBulk = new UpdateStudentDetailsInBulk
                        {
                            YearWise_Student_Id = iId,
                            NewValue = txtNewValue.Text

                        };
                        lstUpdateStudentDetailsInBulk.Add(oUpdateStudentDetailsInBulk);
                    }
                }

                if (lstUpdateStudentDetailsInBulk.Count > 0)
                {
                    string sUpdateStudentDetailsInBulkXML = base.GenerateXml(lstUpdateStudentDetailsInBulk);
                    moUpdateStudentDetailsInBulkBL.Save(sUpdateStudentDetailsInBulkXML, cmbStandard.SelectedValue.ToInt(), cmbDivision.SelectedValue.ToInt(), cmbCategory.SelectedValue.ToInt());
                    lblmessage.Text = cmbCategory.SelectedItem.Text + S_SAVE;
                    FillListView(false);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwStudentDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                CheckBox chkSelect = e.Item.FindControl("chkSelect") as CheckBox;
                TextBox txtNewValue = e.Item.FindControl("txtNewValue") as TextBox;
                chkSelect.Attributes.Add("onclick", "SetField(this," + txtNewValue.ClientID + ")");
                txtNewValue.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwStudentDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwStudentDetails.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwStudentDetails, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    protected void lstvwStudentDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            if (hidSortExpression.Value != e.SortExpression)
                hidSortDirection.Value = Constants.S_DESCENDING;
            base.RevertSortOrder(hidSortDirection);
            hidSortExpression.Value = e.SortExpression;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    private void FillCategory()
    {
        DataTable dtCategory = moUpdateStudentDetailsInBulkBL.GetFillCategory();
        cmbCategory.Bind(dtCategory, "Id", "Category", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill up Standard combo box.
    /// </summary>
    private void FillStandardDropDown()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandard = oStandardCollectionBL.GetAssociatedStandards();
        ListSource.FillDropDownList(oDtStandard, cmbStandard, "standard_name", "standard_id", Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method fills combobox with Divisions
    /// </summary>
    /// <param name="aiStandardId"></param>
    private void FillDivisionDropDown()
    {
        int aiStandardId = Convert.ToInt32(cmbStandard.SelectedValue);
        DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(aiStandardId);
        ControlUtility.FillDropDownList(oDSStandardCollection, ref cmbDivision,
                                       Constants.S_DIVISION_ID_FIELD,
                                       Constants.S_DIVISION_NAME_FIELD,
                                       Constants.S_SELECT_ALL);
    }

    private void SetJavascriptAttributes()
    {
        optMain.Attributes.Add("onclick", "DisableFields(1);");
        optExact.Attributes.Add("onclick", "DisableFields(2);");
        optMain.Checked = true;

        if (QueryString["IsCallFromStudentCountScreen"] != null && QueryString["IsCallFromStudentCountScreen"].ToString() == Constants.S_ONE)
            btnBack.Visible = true;
    }

    private void SetFieldState(bool abEnable)
    {
        cmbCategory.Enabled = abEnable;
        cmbStandard.Enabled = abEnable;
        cmbDivision.Enabled = abEnable;
        btnSave.Visible = !abEnable;
        btnShow.Text = (abEnable == false ? "CHANGE INPUT" : "SHOW");

        if (abEnable == false)
        {
            txtRegNumber.Enabled = abEnable;
            cmbOperation.Enabled = abEnable;
            cmbPrefix.Enabled = abEnable;
            txtReg.Enabled = abEnable;
            chkIsStudBlankRegNo.Enabled = abEnable;            
        }
        else
        {
            if (optMain.Checked)
                txtRegNumber.Enabled = abEnable;
            else
            {
                cmbOperation.Enabled = abEnable;
                cmbPrefix.Enabled = abEnable;
                txtReg.Enabled = abEnable;
            }

            chkIsStudBlankRegNo.Enabled = abEnable;
        }
                
        optMain.Enabled = abEnable;
        optExact.Enabled = abEnable;

    }

    private void FillListView(bool abReset)
    {
        hidIsResetCall.Value = (abReset ? Constants.S_ONE : Constants.S_ZERO);        
        lstvwStudentDetails.DataSourceID = objdsStudentList.ID;
        lstvwStudentDetails.DataBind();
    }

    private void FillOperators()
    {
        List<Operator> olstOperators = StudentBL.GetOperators();
        ListSource.FillDropDownList(olstOperators, cmbOperation, "Text", "Value", string.Empty);
    }

    private void GetPrefixes()
    {
        List<string> olstPrefixes = StudentBL.GetPrefixes(miSchoolId, miAcademicYearId);
        cmbPrefix.Items.Add(new ListItem(Constants.S_ALL, Constants.S_ALL));
        if (olstPrefixes.Count > Constants.I_ZERO)
            olstPrefixes.ForEach(pfx => cmbPrefix.Items.Add(new ListItem(pfx, pfx)));
    }

    private void GetAllRegNoPostfixes()
    {
        List<string> olstPostfixes = StudentBL.GetAllRegNoPostfixes(miSchoolId, miAcademicYearId);
        if (olstPostfixes.Count > Constants.I_ZERO)
            olstPostfixes.ForEach(pfx => cmbPrefix.Items.Add(new ListItem(pfx, pfx)));
    }

    #endregion
    
}