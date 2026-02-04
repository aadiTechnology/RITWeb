using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using System.Threading;
using System.Configuration;

public partial class SurveyFormDetailsUI : ExportDataTable
{
    #region Data Member(s)
    
    private SurveyStudentBL moSurveyStudentBL; 

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
                hidSortExpression.Value = "RegNo";
                hidSortDirection.Value = Constants.S_ASCENDING;
            }

            AddSortImage(lstvwForms, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display registration details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moSurveyStudentBL = new SurveyStudentBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                SetJavascriptAttribues();
                FillStudentDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
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
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwForms);
            FillStudentDetails();
            //DataPager oDataPager = lstvwForms.FindControl("DtPgDropDown") as DataPager;
            //if (oDataPager != null)
            //{
            //    DropDownList ddlCnt = oDataPager.Controls[0].FindControl("ddlCnt") as DropDownList;
            //    if (ddlCnt != null)
            //        hidPageNo.Value = ddlCnt.SelectedValue;
            //}
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set pager settings.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwForms_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwForms.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwForms, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set java script attributes to command buttons.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwForms_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                SurveyStudentDetails oSurveyStudentDetails = e.Item.DataItem as SurveyStudentDetails;

                HiddenField hidQueryString = e.Item.FindControl("hidQueryString") as HiddenField;

                ImageButton btnEdit = e.Item.FindControl("btnEdit") as ImageButton;
                btnEdit.Attributes.Add("onclick", "OpenPopup(" + e.Item.DisplayIndex + ", 1); return false;");
                hidQueryString.Value = CommonUtility.EncryptQuerystring("Id=" + oSurveyStudentDetails.Id);

                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete record.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwForms_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iId = lstvwForms.DataKeys[e.Item.DisplayIndex]["Id"].ToInt();
                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moSurveyStudentBL.Delete(iId);
                    FillStudentDetails();
                    base.DisplayMessage("Registration details deleted successfully !!!", false, tdMessage);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to export registration details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            List<SurveyStudentDetails> lstStudents = SurveyStudentBL.GetAll(miSchoolId, miAcademicYearId, "RegNo", "asc", 0, 99999);
            DataTable oDt = new DataTable();
            oDt.AddColumns(new string[] { "Sr. No.", "Registration No.", "Name", "Gender", "Mobile No. 1", "Mobile No. 2", "School", "Standard", "Economical Condition", "Is SMS Sent?" });

            int iSrNo = 1;
            lstStudents.ForEach(
                    st =>
                    {
                        DataRow dr = oDt.NewRow();
                        dr["Sr. No."] = iSrNo;
                        dr["Registration No."] = st.RegNo;
                        dr["Name"] = st.Name;
                        dr["Gender"] = st.Gender;
                        dr["Mobile No. 1"] = st.MobileNo1;
                        dr["Mobile No. 2"] = st.MobileNo2;
                        dr["School"] = st.School;
                        dr["Standard"] = st.Standard;
                        dr["Economical Condition"] = st.Category;
                        dr["Is SMS Sent?"] = (st.IsInterested == 0) ? "No" : "Yes";
                        oDt.Rows.Add(dr);
                        iSrNo++;
                    }
                );

            base.ExportToExcel("SurveyDetails.xls", oDt);
        }
        catch (ThreadAbortException)
        {
        }    
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to manage sortng.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwForms_Sorting(object sender, ListViewSortEventArgs e)
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

    /// <summary>
    /// This method is used to javascript atrinutes.
    /// </summary>
    private void SetJavascriptAttribues()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnAdd, btnExport });
        btnAdd.Attributes.Add("onclick", "OpenPopup(0,0); return false;");

        //if (ConfigurationManager.AppSettings["SendSMS"] == Constants.S_YES)
        //    btnSendSMS.Visible = true;
        //else
        //    btnSendSMS.Visible = false;
    }

    /// <summary>
    /// This method is used to fill up student lsitview.
    /// </summary>
    private void FillStudentDetails()
    {
        lstvwForms.DataSourceID = objdsSurvey.ID;
        lstvwForms.DataBind();

        if (lstvwForms.Items.Count > 0)
            btnExport.Visible = true;
        else
            btnExport.Visible = false;
    } 

    #endregion
}