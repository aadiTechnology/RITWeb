// File Name     : StandardsList.aspx.cs
// Modified By   : Amit 
// Modified Date : 15/09/2009
// Description   : This class is used to configure standards for school.

using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using System.Resources;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Globalization;

public partial class StandardsList : SchoolBase
{
    #region " Constants "

    private const string S_SELECT_AT_LEAST_ONE_GROUP = "At least one standard name should be selected for saving.";
    private const string S_DATAKEY_STANDARD_ID = "Standard_Id";
    private const string S_DATAKEY_ORIGINAL_STANDARD_ID = "Original_Standard_Id";
    private const string S_DATAKEY_SCHOOL_ID = "School_Id";
    private const string S_DATAKEY_IS_PREPRIMARY = "Is_PrePrimary";
    private const string S_DATAKEY_SECTION = "Section";

    #endregion

    #region Members

    ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));

    #endregion

    #region  " Event "

    /// <summary>
    /// This event is used to fill school standards in grid and to initialise other page controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                FillStandardGridView();
				InitializePage();
				SetNoteText();
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
    /// This event is used to check checkbox in grid if particular standard is associated to the school. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdGroupDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                // If the school id is not the default id i.e. -9999 that means the standard is already assigned
                // to the school. Thus check the checkbox.
                if (grdStandards.DataKeys[e.Row.RowIndex][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
                {
                    ((CheckBox)e.Row.FindControl("ChkBoxDelete")).Checked = true;
                    ((TextBox)e.Row.FindControl("txtStrength")).Text = grdStandards.DataKeys[e.Row.RowIndex]["Strength"].ToString();
                    ((TextBox)e.Row.FindControl("txtThreshold")).Text = grdStandards.DataKeys[e.Row.RowIndex]["Threshold"].ToString();
                }
                else
                    ((HiddenField)e.Row.FindControl("hidIsNewStandard")).Value = "Y";
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save standard configuration for school.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgBtnSave_Click(object sender, EventArgs e)
    {
        // Save the changes to database.
        try
        {
            CheckBox chkDeleteFlag;
            TextBox txtStrength;
            TextBox txtThreshold;
            Collection<StandardMasterBL> oStandards = new Collection<StandardMasterBL>();
            for (int i = 0; i < grdStandards.Rows.Count; i++)
            {
                chkDeleteFlag = (CheckBox)grdStandards.Rows[i].FindControl("ChkBoxDelete");
                string txtPrefixEdit = grdStandards.Rows[i].Cells[1].Text;
                string sIsPrePrimary = grdStandards.DataKeys[i][S_DATAKEY_IS_PREPRIMARY].ToString();
                int iSectionId = Convert.ToInt32(grdStandards.DataKeys[i][S_DATAKEY_SECTION].ToString());
                
                txtStrength = (TextBox)grdStandards.Rows[i].FindControl("txtStrength");
                txtThreshold = (TextBox)grdStandards.Rows[i].FindControl("txtThreshold");

                int iStrength = (txtStrength.Text == string.Empty ? 0 : Convert.ToInt32(txtStrength.Text));
                int iThreshold = (txtThreshold.Text == string.Empty ? 0 : Convert.ToInt32(txtThreshold.Text));
                int iNextOriginalStandardId = Convert.ToInt32(grdStandards.DataKeys[i]["NextOriginalStandardId"]);

                // Check if new standard is being inserted.
                // I.e. If the checkbox is checked and the school id is -9999 then it is the new standard being
                // introduced.
                if (chkDeleteFlag.Checked == true && grdStandards.DataKeys[i][S_DATAKEY_SCHOOL_ID].ToString() == Constants.S_DEFAUL_SCHOOL_ID)
                {
                    StandardMasterBL oStandardMasterBL = GetCommonStandardMasterBL(txtPrefixEdit, sIsPrePrimary, iSectionId, Convert.ToInt32(grdStandards.DataKeys[i][S_DATAKEY_STANDARD_ID].ToString()), iStrength, iThreshold, iNextOriginalStandardId);
                    oStandardMasterBL.ConfigurationAction = Constants.Action.Insert;
                    oStandards.Add(oStandardMasterBL);
                }

                // Check if existing standard name is being updated.
                // I.e. If the checkbox is checked and the school is not -9999 and the value in text box differs 
                // from the value in the standard name column then update the existing standard name.
                else if (chkDeleteFlag.Checked == true &&
                        grdStandards.DataKeys[i][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID &&
                        grdStandards.DataKeys[i][S_DATAKEY_STANDARD_ID].ToString() != txtPrefixEdit.Trim())
                {
                    StandardMasterBL oStandardMasterBL = GetCommonStandardMasterBL(txtPrefixEdit, sIsPrePrimary, iSectionId, Convert.ToInt32(grdStandards.DataKeys[i][S_DATAKEY_ORIGINAL_STANDARD_ID].ToString()), iStrength, iThreshold, iNextOriginalStandardId);
                    oStandardMasterBL.ConfigurationAction = Constants.Action.Update;
                    oStandardMasterBL.StandardId = Convert.ToInt32(grdStandards.DataKeys[i][S_DATAKEY_STANDARD_ID].ToString());
                    oStandards.Add(oStandardMasterBL);
                }

                // Check if existing standard is being removed.
                // I.e. If the checkbox is NOT checked and the school id is not -9999. 
                // In such case need to check if any of the related data is entered for the unchecked standard then
                // the warning message should be given to user and the related data should be removed from db.
                else if (chkDeleteFlag.Checked == false && grdStandards.DataKeys[i][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
                {
                    StandardMasterBL oStandardMasterBL = GetCommonStandardMasterBL(txtPrefixEdit, sIsPrePrimary, iSectionId, Convert.ToInt32(grdStandards.DataKeys[i][S_DATAKEY_STANDARD_ID].ToString()), iStrength, iThreshold, iNextOriginalStandardId);
                    oStandardMasterBL.ConfigurationAction = Constants.Action.Delete;
                    oStandardMasterBL.StandardId = Convert.ToInt32(grdStandards.DataKeys[i][S_DATAKEY_STANDARD_ID].ToString());
                    oStandards.Add(oStandardMasterBL);
                }
            }

            // Update database with the configured standards.
            if (oStandards.Count > 0)
            {
                int iDefaultCautionMoney = Settings.StandardCautionMoneyAmt;
                StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId);
                oStandardCollectionBL.UpdateStandards(oStandards, miAcademicYearId, iDefaultCautionMoney, hidStartDate.Value.ToDateTime().ToString("MM/dd/yyyy"), Convert.ToDateTime(hidEndDate.Value).ToString("MM/dd/yyyy"));
            }

            string sIsConfig = ReadQuerystring();
            if (sIsConfig != "Y")
                //InsertStandardConfigDetails();
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.Standard));

            MasterPage oMasterPage = (MasterPage)this.Master;
			if (hidNavigate.Value != Constants.S_YES || Settings.IsMiniSite)
                oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Basic_Configuration)));
            else
                oMasterPage.RedirectToNextPage("~/RITeSchool/Common/SendMessageFromInbox.aspx?" + CommonUtility.EncryptQuerystring("From=AcademicPeriod"));
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions oEx)
        {
			lblErr.Text = CommonUtility.ModifyExceptionMessage(oEx.Message, "Standard", Resources.LocalizedResources.Standard, "can not be removed since associated with", Resources.LocalizedResources.valRemoveText);
            FillStandardGridView();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region " Private Method "

    /// <summary>
    /// This method is used to fill standard grid view.
    /// </summary>
    private void FillStandardGridView()
    {
        // This method fills the Grid with available Group details.        
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);

        DataTable oDTUserDetails = oStandardCollectionBL.GetAllStandards();
        grdStandards.DataSource = oDTUserDetails;
        grdStandards.DataBind();
    }

    /// <summary>
    /// This method is used to initialize page controls. 
    /// </summary>
    private void InitializePage()
    {
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Basic_Configuration));
		
        imgBtnSave.Attributes.Add("Onclick", "if(!(ConfirmAction('" + grdStandards.AllowPaging + "','" + Resources.LocalizedResources.sSelectAtLeastOneStandard + "'))){return false;}");

        btnCancel.Attributes["onclick"] = "javascript:DisableButtons()";
        ApplyMouseHoverEffect(new List<Button> { btnCancel, imgBtnSave });
    }

	/// <summary>
	/// This method is used to set academic year start date and end values to note. 
	/// </summary>
	private void SetNoteText()
	{
		DateTimeFormatInfo info = DateTimeFormatInfo.GetInstance(CultureInfo.CurrentCulture);		
		lblNote.Text = Resources.LocalizedResources.StandardListNoteText;		
		SchoolWiseAcademicYearMasterBL oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
		DataRow[] dr = oSchoolWiseAcademicYearMasterBL.GetAllSchoolwiseAcademicYearInfo(miSchoolId).Select("Academic_Year_Id='" + miAcademicYearId + "'");		
		hidStartDate.Value = dr[0]["start_date"].ToString().Replace('-','/');
		hidEndDate.Value = dr[0]["end_date"].ToString().Replace('-', '/');
        lblNote.Text = lblNote.Text.Replace("%startdate%", DateCultureConversion((dr[0]["start_date"].ToDateTime()).ToString("MM/dd/yyyy"), "", CultureInfo.CurrentCulture.ToString()));
		lblNote.Text = lblNote.Text.Replace("%enddate%", DateCultureConversion((dr[0]["end_date"].ToDateTime()).ToString("MM/dd/yyyy"), "", CultureInfo.CurrentCulture.ToString()));		
		hidWanttoSaveAcademicYear.Value = Resources.LocalizedResources.StandardListWantToChangeAcademicYearPeriod;
	}

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    /// <returns></returns>
    private string ReadQuerystring()
    {
        try
        {
            if (QueryString["Is_Configured"] != null)
                return QueryString["Is_Configured"];
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)Master;
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }

        return String.Empty;
    }

    /// <summary>
    /// This method is used to populate object of StandardMasterBL,
    /// which is used to save standard configuration.
    /// </summary>
    /// <param name="asFieldValue"></param>
    /// <param name="asIsPrePrimary"></param>
    /// <param name="aiSectionId"></param>
    /// <param name="aiOriginalFieldId"></param>
    /// <returns></returns>
    private StandardMasterBL GetCommonStandardMasterBL(string asFieldValue, string asIsPrePrimary, int aiSectionId, int aiOriginalFieldId, int aiStrength, int aiThreshold, int aiNextOriginalStandardId)
    {
        //// This method creates the default object for the configuration and returns the same.

        StandardMasterBL oStandardMasterBL = new StandardMasterBL
                                                 {
                                                     SchoolId = miSchoolId,
                                                     AcademicYearId = miAcademicYearId,
                                                     UpdatedById = Convert.ToString(miUserId),
                                                     InsertedByid = Convert.ToString(miUserId),
                                                     StandardName = asFieldValue,
                                                     OriginalStandardId = aiOriginalFieldId,
                                                     IsPrePrimary = asIsPrePrimary,
                                                     SectionId = aiSectionId,
                                                     InsertDate = DateTime.Now,
                                                     UpdateDate = DateTime.Now,
                                                     StudentStrength = aiStrength,
                                                     Threshold = aiThreshold,
                                                     NextOriginalStandardId = aiNextOriginalStandardId
                                                 };

        return oStandardMasterBL;
    }
    /// <summary>
    /// This method used to value based on Culture
    /// </summary>
    private void RefreshValue()
    {
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        imgBtnSave.Attributes.Add("Onclick", "if(!(ConfirmAction('" + grdStandards.AllowPaging + "','" + Resources.LocalizedResources.sSelectAtLeastOneStandard + "'))){return false;}");
		SetNoteText();
    }

    #endregion " Private Method "
}