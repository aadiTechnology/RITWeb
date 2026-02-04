/* File Name :- HealthDetailsStudentListUI.aspx.cs
 * Created Date :- 22-Nov-2018
 * Class Description :- This class is used to display class wise Student list for Health Details.
 * Created By :- Dnyaneshwar Shinde.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;
using BusinessLogic;
using System.Data;
using BusinessLogic.Exceptions;
using SchoolEntities;

public partial class HealthDetailsStudentListUI : SchoolBase
{

    #region DataMember

    private HealthDetailsBL moHealthDetailsBL;

    #endregion

    #region Event's

    /// <summary>
    /// This Event is used to load default controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moHealthDetailsBL = new HealthDetailsBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                SetJavascriptAttribute();
                FillStandardCombo();
                ReadQueryString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This Event is used to Selected index change event of Combobox;
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {           
            FillDivisionCombo();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display Student Details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            FillStudentDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is used to bound data to list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                HyperLink hyplnkEdit = ((HyperLink)(oCurrentItem.FindControl("hyplnkEdit")));
                HealthDetails oHealthDetails = e.Item.DataItem as HealthDetails;

                if (oHealthDetails.Status == Constants.I_ONE)
                    hyplnkEdit.Text = "Edit";
                else
                    hyplnkEdit.Text = "Add";

                if (oHealthDetails.IsSubmited == Constants.I_ONE)
                    hyplnkEdit.Text = "View";

                string sQueryString = "StudentId=" + oHealthDetails.StudentId.ToString() + 
                                      "&StandardId=" + cmbStandard.SelectedValue +
                                      "&DivisionId=" + cmbDivision.SelectedValue;
                hyplnkEdit.NavigateUrl = "~/RITeSchool/HealthDetails/StudentHealthDetailsUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString);

                if (oHealthDetails.IsLeft == Constants.I_ONE)
                {
                    Label lblRollNo = ((Label)(oCurrentItem.FindControl("lblRollNo")));
                    Label lblStudentName = ((Label)(oCurrentItem.FindControl("lblStudentName")));
                    lblRollNo.ForeColor = System.Drawing.Color.Red;
                    hyplnkEdit.ForeColor = System.Drawing.Color.Red;
                    lblStudentName.ForeColor = System.Drawing.Color.Red; 
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method's

    /// <summary>
    /// This method is used to fill standard's combo.
    /// </summary>
    private void FillStandardCombo()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oStandardCollectionBL.GetAssociatedStandardsForHealth();
        ControlUtility.FillDropDownList(oDSStandardCollection, ref cmbStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       Constants.S_SELECT);

        // Add item into division combobox.
        ListItem olstDivision = new ListItem();
        olstDivision.Text = "-- Select --";
        cmbDivision.Items.Add(olstDivision);
    }

    /// <summary>
    /// This method is used to fill Division's combo.
    /// </summary>
    private void FillDivisionCombo()
    {
        DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(cmbStandard.SelectedValue.ToInt());
        ControlUtility.FillDropDownList(oDSStandardCollection, ref cmbDivision,
                                       Constants.S_DIVISION_ID_FIELD,
                                       Constants.S_DIVISION_NAME_FIELD,
                                       Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill studentDetails Listview;
    /// </summary>
    private void FillStudentDetails()
    {
        List<HealthDetails> lstHealthDetails = new List<HealthDetails>();
        lstHealthDetails = moHealthDetailsBL.GetAllStudentDetails(cmbStandard.SelectedValue.ToInt(), cmbDivision.SelectedValue.ToInt());

        lstvwStudentDetails.DataSource = lstHealthDetails;
        lstvwStudentDetails.DataBind();

        //if (lstHealthDetails.Count > Constants.I_ZERO)
        //    trLegend.Visible = true;
        //else
        //    trLegend.Visible = false;
    }

    /// <summary>
    /// This Method is used to set Javascript Attributes..
    /// </summary>
    private void SetJavascriptAttribute()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        base.ApplyMouseHoverEffect(new List<Button> { btnShow });
    }

    /// <summary>
    /// This method is used to read Query string and set values to Combobox;
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["StandardId"] != null && QueryString["DivisionId"] != null)
        {
            if (QueryString["StandardId"] != null)
                cmbStandard.SelectedValue = QueryString["StandardId"];

            cmbStandard_SelectedIndexChanged(cmbStandard, null);

            if (QueryString["DivisionId"] != null)
                cmbDivision.SelectedValue = QueryString["DivisionId"];

            btnShow_Click(btnShow, null);
        }
    }

    #endregion
}