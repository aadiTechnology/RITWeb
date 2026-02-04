//// File Name  : ExamTypePopup.aspx.cs
//// Created By : Sanket Bhujbal
//// Date       : 28/05/2015
//// Description :This class is used to perform operation on exam type. 
////   

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities.Admin;
using Utility;
using System.Data;
using System.Reflection;
using System.Resources;
using System.Data.SqlClient;
public partial class ExamTypePopup : SchoolBase
{

    #region Data Member(s)

    ExamTypesConfigurationBL moExamTypesConfigurationBL;

    #endregion
   
    #region Constants(s)

    private const string  S_SAVE_MESSAGE = "Exam Type Saved Successfully!!!";
    private const string  S_UPDATE_MESSAGE = "Exam Type Updated Successfully!!!";
    private const string  S_DELETE_MESSAGE = "Exam Type Deleted Successfully!!!";

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to load page controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moExamTypesConfigurationBL = new ExamTypesConfigurationBL(miUserId);
            if (!IsPostBack)
            {
                FillExamTypeListView();
                SetJavascriptAttributes();   
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to capture image status according to value come database
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwExamTypes_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                bool  bConsiderExamStatus = lstvwExamTypes.DataKeys[e.Item.DisplayIndex]["ConsiderExamStatus"].ToBool();
                var imgbtn =e.Item.FindControl("ImgBtn1") as ImageButton;
                if (bConsiderExamStatus == true)
                    imgbtn.Visible = true;
                else
                    imgbtn.Visible = false;
               ImageButton btnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to save test type
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SubjectwiseExamTypeDetails oSubjectwiseExamTypeDetails = new SubjectwiseExamTypeDetails();
            oSubjectwiseExamTypeDetails.TestTypeName = txtExamName.Text;
            oSubjectwiseExamTypeDetails.SortOrder = Convert.ToInt32(TxtSortOrder.Text);
            if (CheckBox1.Checked == true)
                oSubjectwiseExamTypeDetails.ConsiderExamStatus = true;
            else
                oSubjectwiseExamTypeDetails.ConsiderExamStatus = false;
            if (HidTestTypeId.Value == Constants.S_ZERO)
            {
                oSubjectwiseExamTypeDetails.TestTypeId = 0;
                moExamTypesConfigurationBL.SaveTestType(oSubjectwiseExamTypeDetails.TestTypeId, oSubjectwiseExamTypeDetails.TestTypeName, oSubjectwiseExamTypeDetails.ConsiderExamStatus, oSubjectwiseExamTypeDetails.SortOrder);
                FillExamTypeListView();
                base.DisplayMessage( S_SAVE_MESSAGE, false, tdMessage);
                ClearFields();
            }
            else
            {
                oSubjectwiseExamTypeDetails.TestTypeId = Convert.ToInt32(HidTestTypeId.Value);
                moExamTypesConfigurationBL.SaveTestType(oSubjectwiseExamTypeDetails.TestTypeId, oSubjectwiseExamTypeDetails.TestTypeName, oSubjectwiseExamTypeDetails.ConsiderExamStatus, oSubjectwiseExamTypeDetails.SortOrder);
                FillExamTypeListView();
                base.DisplayMessage(S_UPDATE_MESSAGE, false, tdMessage);
                ClearFields();
            }
        }
        catch (Exception ex)
        {

            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }
/// <summary>
/// This event is used to take decision about to perform update or delete
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
    protected void lstvwExamTypes_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)//if data item is selected
            {
                int iTestTypeId;
                iTestTypeId = Convert.ToInt32(lstvwExamTypes.DataKeys[e.Item.DisplayIndex]["TestTypeId"]);
                HidTestTypeId.Value = iTestTypeId.ToString();
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    SubjectwiseExamTypeDetails oTestType = moExamTypesConfigurationBL.GetTestType(iTestTypeId);
                    HidTestTypeId.Value = oTestType.TestTypeId.ToString();
                    txtExamName.Text = oTestType.TestTypeName;
                    TxtSortOrder.Text = oTestType.SortOrder.ToString();
                    if (oTestType.ConsiderExamStatus == true)
                        CheckBox1.Checked = true;
                    else
                        CheckBox1.Checked = false;
                }

                if (e.CommandName ==Constants.S_COMMAND_REMOVE)
                {
                    iTestTypeId = Convert.ToInt32(HidTestTypeId.Value);
                    moExamTypesConfigurationBL.Delete(iTestTypeId);
                    FillExamTypeListView();
                    base.DisplayMessage( S_DELETE_MESSAGE, false, tdMessage);
                    HidTestTypeId.Value = Constants.S_ZERO;
                }
            }
        }
        catch (SqlException se)
        {
            base.DisplayMessage(se.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to clear the control value
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to close the popup
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            
            Response.Write("<Script language='Javascript'>window.opener.location = window.opener.location.pathname;window.close();window.opener.focus(); </Script>");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method(s)

    private void FillExamTypeListView()
    {
        List<SubjectwiseExamTypeDetails> lstSubjecwiseExamTypeDetails = moExamTypesConfigurationBL.GetAllTestType(0);
        lstvwExamTypes.DataSource = lstSubjecwiseExamTypeDetails;
        lstvwExamTypes.DataBind();
    }

    private void SetJavascriptAttributes()
    {
        HidTestTypeId.Value = Constants.S_ZERO;
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnClose, btnCancel });
        txtExamName.Focus();
       
        
    }

    private void ClearFields()
    {
        txtExamName.Text = string.Empty;
        CheckBox1.Checked = false;
        HidTestTypeId.Value = Constants.S_ZERO;
        TxtSortOrder.Text = string.Empty;
    }

    #endregion
   
}