using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Reflection;
using MasterEntities;
using System.Text;
/// <summary>
/// This class is used to save, delete and getting standards which are available for grading system.
/// </summary>
public partial class StandardsForGradingSystemConfigUI : SchoolBase
{  

    #region Data Member(s)

    private StandardCollectionBL moStandardCollectionBL;

    #endregion


    #region "Events"

    /// <summary>
    /// This event is used to fill standard listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
            if (!IsPostBack)
            {
                FillStandardListView();
                SetJavaScriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this event is used for saving the standards for grading system.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void imgBtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            CheckBox chkSelect;
            StringBuilder sbStandardIds = new StringBuilder();
            string sStandardIds = string.Empty;

            foreach (ListViewDataItem oCurrentItem in lstvwStandards.Items)
            {
                chkSelect = oCurrentItem.FindControl("chkSelect") as CheckBox;
                if (chkSelect.Checked)
                    sbStandardIds = sbStandardIds.Append("," + lstvwStandards.DataKeys[oCurrentItem.DisplayIndex]["StandardId"].ToString());
            }

            if (sbStandardIds.ToString().StartsWith(","))
                sStandardIds = sbStandardIds.ToString().Substring(1);

            moStandardCollectionBL.SaveStandardsForGradingSystem(sStandardIds);

            lblUpdateSucess.Text = Resources.LocalizedResources.GradingSystmStandardsSavedSuccessfully;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to move on school configuration screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Exam_Related)));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region " Private Method "

    /// <summary>
    /// This method is used to fill standard list view.
    /// </summary>
    private void FillStandardListView()
    {
        List<StandardMaster> lstStandards = moStandardCollectionBL.GetStandardsForGradingSystem();
        lstvwStandards.DataSource = lstStandards;
        lstvwStandards.DataBind();
        trbtn.Visible = trNote.Visible = lstvwStandards.Items.Count > Constants.I_ZERO;
    }

    /// <summary>
    /// This method is used to set javascript attributes to controls.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnCancel, imgBtnSave });
    }

    #endregion

}
