/* File Name :- HouseConfigurationPopUp.aspx.cs
 * Created Date :- -Oct-2015
 * Class Description :- This Class is Used to Configure the Standard wise House. 
 * Created By :- 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SchoolEntities;
using Utility;
using BusinessLogic;
using BusinessLogic.Exceptions;
using DataCommunicator;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;


public partial class HouseConfigurationPopUp : SchoolBase
{
    #region Constant(s)

    private const string S_SAVE_MESSAGE = "House-Standard assignment saved successfully !!!";

    #endregion

    #region DataMember

    private HouseConfigurationDetailsBL moHouseConfigurationDetailsBL;

    #endregion

    #region Events

    /// <summary>
    /// This event is used to set the page Load Events.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moHouseConfigurationDetailsBL = new HouseConfigurationDetailsBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
                FillHouseConfiruredListview();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Save House Configuration Details.
    /// </summary>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            CheckBox chkSelect;
            StringBuilder sbStandardIds = new StringBuilder();
            string sStandardIds = string.Empty;

            foreach (ListViewDataItem oCurrentItem in lstvwConfigureStandards.Items)
            {
                chkSelect = oCurrentItem.FindControl("chkSelect") as CheckBox;
                if (chkSelect.Checked)
                    sbStandardIds = sbStandardIds.Append("," + lstvwConfigureStandards.DataKeys[oCurrentItem.DisplayIndex]["StandardId"].ToString());
            }
            if (sbStandardIds.ToString().StartsWith(","))
                sStandardIds = sbStandardIds.ToString().Substring(1);
            
            moHouseConfigurationDetailsBL.Save(sStandardIds);
            base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);

        }
        catch(SqlException ex)
        {
            base.DisplayMessage(ex.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// This Method is used Fill Standard wise House Configuration Details ListView.
    /// </summary>
    private void FillHouseConfiruredListview()
    {
        List<HouseConfigurationDetails> oHouseConfigurationDetails = moHouseConfigurationDetailsBL.GetAll();
        lstvwConfigureStandards.DataSource = oHouseConfigurationDetails;
        lstvwConfigureStandards.DataBind();
    }

    /// <summary>
    /// This Method is used to set Javascript Attributes..
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnClose });
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    #endregion

}