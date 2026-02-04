using System;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using SchoolEntities.Accounts;
using System.Collections.Generic;
using SchoolEntities;
using System.Web;
using SchoolEntities.Admin;
public partial class OnlineAdmissionUI : SchoolBase
{
	#region const
	const string S_FORM_CLOSE = "Online admissions forms are closed.";
    private const int I_BANK_DETAILS_TABLE = 0;
    private const int I_CARD_DETAILS_TABLE = 1;

	#endregion


   Dictionary<string, string> dictGoogleForm = new Dictionary<string, string>();
   List<InternalLinkStandardDetails> mlstInternalLinkStandardDetails = new List<InternalLinkStandardDetails>();

	#region -- EVENT HANDLER(s) --

	public string msEnableAdmissionFormFee = Constants.S_YES;
	/// <summary>
	/// 	This method is used to handle a page load event.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			string sPath = GetFromPageUrl();
			if (!IsPostBack)
			{
                //if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.PPSN.ToInt())
                //{
                //    divOuter.Visible = false;
                //    return;
                //}
                //else 

                //if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.PPS.ToInt())
                //{
                //    if (QueryString["RestrictedAccess"] == null || QueryString["RestrictedAccess"].ToString().Trim() != Constants.S_YES)
                //    {
                //        if (QueryString["sIsSubling"] == null || QueryString["sIsSubling"].ToString() != Constants.S_YES)
                //        {
                //            divOuter.Visible = false;
                //            return;
                //        }
                //    }
                //}

                
				DecryptQueryString();
				if (sPath != "AdmissionThankYouUI.aspx")
					Session[Constants.S_SESSION_STUDENT_ADMISSION_ID] = null;

                //if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.PPSH.ToInt())
                //    trNetbankingDetails.Visible = false;
                //else
                //    trNetbankingDetails.Visible = true;
			}
           
            hidAcademicYearForOnlineAdmission.Value = SchoolWiseAcademicYearMasterBL.GetAcademicYearForOnlineAdmission(ConfigurationManager.AppSettings["SchoolId"].ToInt());
            BindAdmissionStatusListViewNxtYear();  //for next year
            
            if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.PPSN.ToInt())
            {
                trAdmissionProcessDetails.Visible = false;
                trAdmissionDetailsForPPSN.Visible = false;
                trBankLabel.Visible = false;
                lstvwBankDetails.Visible = false;
                OnlineAdmissionText.Visible = false;
                lblStandardList.Text = "Standard List for admission in 2025-26 :";
                trNetbankingDetails.Visible = false;

                trPaymentInfoPPSN.Visible = true;
                trPaymentInfo.Visible = false;

                //trOldStandardListview.Visible = false;
                //trOldStandardRow.Visible = false;
                //spnNextYearLabel.InnerText = "Grade List (2022-23) :";

                //trNextYear.Visible = false;
                spnNextYearLabel.InnerText = "Standard List for admission in 2026-27 :";
            }
            else
            {
                trAdmissionProcessDetails.Visible = true;
                trAdmissionDetailsForPPSN.Visible = false;
                trBankLabel.Visible = true;
                lstvwBankDetails.Visible = true;
                OnlineAdmissionText.Visible = true;
                lblStandardList.Text = "Standard List :";

                if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.PPS.ToInt())
                {
                    lblStandardList.Text = "Standard selection for admission application for year 2021-22";
                    trHeight1.Visible = true;
                    trHeight2.Visible = true;
                }
            }
            
           if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.PPSH.ToInt())
           {
               trAdmissionProcessDetails.Visible = false;
               trAdmissionProcessPPSH.Visible = true;
               trPPSHAgeCriteria.Visible = true;
               //lblStandardList.Text = "Standard List for admission in 2022-23 :";
               
               //trOldStandardRow.Visible = false;
               //trOldStandardListview.Visible = false;

               lblStandardList.Text = "Standard List for admission in 2025-26 :";

               trNextYear.Visible = true;
               spnNextYearLabel.InnerText = "Standard List for admission in 2026-27 :";
               trHeight2.Visible = true;
           }

           //if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.DPISRAVET.ToInt())
           //{
           //    trNextYear.Visible = false;
           //    lstvwAdmissionStatusNxtYear.DataSource = null;
           //    lstvwAdmissionStatusNxtYear.DataBind();
           //}

                SetControls();

                if (ConfigurationManager.AppSettings["SchoolId"].ToInt() != Constants.SchoolId.PPS.ToInt() && ConfigurationManager.AppSettings["SchoolId"].ToInt() != Constants.SchoolId.PPSN.ToInt())
                    BindAdmissionStausListView();
                else
                {
                    trOldStandardRow.Visible = false;
                    trHeight1.Visible = false;
                }

            FillNetBankingDetails();
            
            if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.JOS.ToInt())
            {
                trJOSAdmissionClosed.Visible = true;
                lblErrorMsg.Visible = false;
                trPaymentInfo.Visible = false;
            }
            else
            {
                trJOSAdmissionClosed.Visible = false;                
            }

            if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.BFS.ToInt())
            {
                trPaymentOffline.Visible = false;
                trConfirmationText_Copy.Visible = false;
                trConfirmationText_BFS.Visible = true;
            }
            else
                trConfirmationText_BFS.Visible = false;

            if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.MVPS.ToInt() || ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.PEMS.ToInt())
            {
                trPrint.Visible = false;
                spnNextYearLabel.InnerText = "Standard selection for admission application for year 2023-24";
                trOldStandardRow.Visible = false;
                trOldStandardListview.Visible = false;
            }
            if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.BFS.ToInt())
            {
                if (!Settings.ShowAdmissionForCurrentYear)
                {
                    lstvwAdmissionStatus.Visible = false;
                    trOldStandardRow.Visible = false;
                }
            }                 
		}

		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	This method is used to handle a lstvwAdmissionStatus lisview Item databound and enable disable admission link according to dates.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void lstvwAdmissionStatus_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			HtmlControl thStartDate = lstvwAdmissionStatus.FindControl("thStartDate") as HtmlControl;
			HtmlControl thEndDate = lstvwAdmissionStatus.FindControl("thEndDate") as HtmlControl;
			HtmlControl thLottaryDate = lstvwAdmissionStatus.FindControl("thLottaryDate") as HtmlControl;
			HtmlControl thTotalForms = lstvwAdmissionStatus.FindControl("thTotalForms") as HtmlControl;
            HtmlControl thDOBMinLimit = lstvwAdmissionStatus.FindControl("thDOBMinLimit") as HtmlControl;
            HtmlControl thDOBMaxLimit = lstvwAdmissionStatus.FindControl("thDOBMaxLimit") as HtmlControl;

			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				var oCurrentItem = e.Item as ListViewDataItem;
				var oDataRowView = oCurrentItem.DataItem as DataRowView;
				var olnkbtnAdmission = e.Item.FindControl("lnkbtnAdmission") as LinkButton;

				HtmlControl tdStartDate = oCurrentItem.FindControl("tdStartDate") as HtmlControl;
				HtmlControl tdEndDate = oCurrentItem.FindControl("tdEndDate") as HtmlControl;
				HtmlControl tdLottaryDate = oCurrentItem.FindControl("tdLottaryDate") as HtmlControl;
				HtmlControl tdTotalForms = oCurrentItem.FindControl("tdTotalForms") as HtmlControl;
				Label lblCloseDt = oCurrentItem.FindControl("lblCloseDt") as Label;
				Label lblformOpenDate = oCurrentItem.FindControl("formOpenDate") as Label;
				Label lblTotalformsCount = oCurrentItem.FindControl("TotalformsCount") as Label;
                int iStandardId = Convert.ToInt32(lstvwAdmissionStatus.DataKeys[e.Item.DisplayIndex]["Standard_Id"]);
                Label lblMinDOB = oCurrentItem.FindControl("lblMinDOB") as Label;
                Label lblMaxDOB = oCurrentItem.FindControl("lblMaxDOB") as Label;

                HtmlControl tdDOBMin = oCurrentItem.FindControl("tdDOBMin") as HtmlControl;
                HtmlControl tdDOBMax = oCurrentItem.FindControl("tdDOBMax") as HtmlControl;

				if (msEnableAdmissionFormFee == Constants.S_NO)                
				{
					thStartDate.Visible = false;
					thEndDate.Visible = false;
					thLottaryDate.Visible = false;
					thTotalForms.Visible = false;
					tdStartDate.Visible = false;
					tdEndDate.Visible = false;
					tdLottaryDate.Visible = false;
					tdTotalForms.Visible = false;
                    thDOBMinLimit.Visible = false;
                    thDOBMaxLimit.Visible = false;
				}

				if ( oDataRowView["FormOpenDate"]!=DBNull.Value && DateTime.Now < oDataRowView["FormOpenDate"].ToDateTime())
				{
					olnkbtnAdmission.Text = "Admission Not Started";
					olnkbtnAdmission.PostBackUrl = "";
					olnkbtnAdmission.Enabled = false;
				}

				if (oDataRowView["FormCloseDate"] == DBNull.Value)
					lblCloseDt.Text = "-";
				if (oDataRowView["FormOpenDate"] == DBNull.Value)
					lblformOpenDate.Text = "-";

                if (ConfigurationManager.AppSettings["SchoolId"].ToInt() != Constants.SchoolId.PPSH.ToInt())
                {
                    if (oDataRowView["TotalOnlineForms"].ToString() == "-1")
                        lblTotalformsCount.Text = "-";
                }
                else
                    lblTotalformsCount.Text = "-";

				if (oDataRowView["RemainingformsCount"].ToInt() == -1 && // If RemainingFormsCount == -1, it means forms are unlimited, hence we do not close it.
						((oDataRowView["FormCloseDate"] != DBNull.Value && DateTime.Now >= oDataRowView["FormCloseDate"].ToDateTime()) // When form close date has past the current date.
							)) // When forms remaining count is 0)
				{
					olnkbtnAdmission.Text = "Forms Closed";
					olnkbtnAdmission.ForeColor = System.Drawing.Color.Red;
					olnkbtnAdmission.PostBackUrl = "";
					olnkbtnAdmission.Enabled = false;
				}
				// Close the form submission if the following conditions are met.
				else if (oDataRowView["RemainingformsCount"].ToInt() != -1 && // If RemainingFormsCount == -1, it means forms are unlimited, hence we do not close it.
						((oDataRowView["FormCloseDate"] != DBNull.Value && DateTime.Now >= oDataRowView["FormCloseDate"].ToDateTime()) // When form close date has past the current date.
							|| oDataRowView["RemainingformsCount"].ToInt() <= 0)) // When forms remaining count is 0)
				{
					olnkbtnAdmission.Text = "Forms Closed";
					olnkbtnAdmission.ForeColor = System.Drawing.Color.Red;
					olnkbtnAdmission.PostBackUrl = "";
					olnkbtnAdmission.Enabled = false;
				}

                int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();                
                if (iSchoolId == Constants.SchoolId.PPSH.ToInt() && (iStandardId == 990) || (iStandardId == 991))
                {   
                    olnkbtnAdmission.Attributes.Add("onclick", "if(!AdmissionAlertMessage(" + iStandardId + "," + iSchoolId + ")) {return false}");
                }

                if (iSchoolId == Constants.SchoolId.ZLSP.ToInt())
                {
                    thDOBMinLimit.Visible = true;
                    thDOBMaxLimit.Visible = true;

                    if (lblMinDOB.Text != string.Empty)
                        lblMinDOB.Text = lblMinDOB.Text.ToDateTime().ToString(Constants.S_DATE_FORMAT);

                    if(lblMaxDOB.Text != string.Empty)
                        lblMaxDOB.Text = lblMaxDOB.Text.ToDateTime().ToString(Constants.S_DATE_FORMAT);
                }
                else
                {
                    tdDOBMin.Visible = false;
                    tdDOBMax.Visible = false;
                }
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	This method is used to handle a lstvwAdmissionStatus lisview Item databound and redirect to admission form page with selcted standard.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void lstvwAdmissionStatus_ItemCommand(object sender, ListViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName == "Admission")
			{
                bool bEnableAdmissionFormFee = Convert.ToBoolean(lstvwAdmissionStatus.DataKeys[e.Item.DisplayIndex]["EnableAdmissionFormFee"]);
                int iAcademicYearId = Convert.ToInt32(lstvwAdmissionStatus.DataKeys[e.Item.DisplayIndex]["Academic_Year_Id"]);
                string sStanadrdName = Convert.ToString(lstvwAdmissionStatus.DataKeys[e.Item.DisplayIndex]["Standard_Name"]);
				int iStandardId = e.CommandArgument.ToInt();
                string sQuerystring = "StandardId=" + iStandardId + "&StandardName=" + sStanadrdName + "&EnableAdmissionFormFee=" + bEnableAdmissionFormFee + "&AcademicYearId=" + iAcademicYearId;
				sQuerystring = CommonUtility.EncryptQuerystring(sQuerystring);
				Response.Redirect("~/RITeSchool/Admission/AdmissionFormDocuments.aspx?" + sQuerystring, false);

			}
		}
		catch (ThreadAbortException)
		{ }
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}
	protected void lstvwAdmissionStatus_DataBound(object sender, EventArgs e)
	{
		try
		{
			HtmlControl thStartDate = lstvwAdmissionStatus.FindControl("thStartDate") as HtmlControl;
			HtmlControl thEndDate = lstvwAdmissionStatus.FindControl("thEndDate") as HtmlControl;
			HtmlControl thLottaryDate = lstvwAdmissionStatus.FindControl("thLottaryDate") as HtmlControl;
			HtmlControl thTotalForms = lstvwAdmissionStatus.FindControl("thTotalForms") as HtmlControl;
            HtmlControl thDOBMinLimit = lstvwAdmissionStatus.FindControl("thDOBMinLimit") as HtmlControl;
            HtmlControl thDOBMaxLimit = lstvwAdmissionStatus.FindControl("thDOBMaxLimit") as HtmlControl;

			bool bIsHiddenStartDt = false;
			bool bIsHiddenEndDt = false;
			bool bIsHiddenCountDt = false;
			bool bIsHiddenLotteryDt = false;

			foreach (ListViewDataItem item in lstvwAdmissionStatus.Items)
			{
				HtmlControl tdStartDate = item.FindControl("tdStartDate") as HtmlControl;
				Label lbStartDate = tdStartDate.FindControl("formOpenDate") as Label;
				if (lbStartDate.Text == "" || lbStartDate.Text == "-")
					bIsHiddenStartDt = false;
				else
				{
					bIsHiddenStartDt = true;
					break;
				}
			}

			foreach (ListViewDataItem item in lstvwAdmissionStatus.Items)
			{
				HtmlControl tdEndDate = item.FindControl("tdEndDate") as HtmlControl;
				Label lbEndDate = tdEndDate.FindControl("lblCloseDt") as Label;
				if (lbEndDate.Text == "" || lbEndDate.Text == "-")
					bIsHiddenEndDt = false;
				else
				{
					bIsHiddenEndDt = true;
					break;
				}
			}
			foreach (ListViewDataItem item in lstvwAdmissionStatus.Items)
			{
				HtmlControl tdTotalForms = item.FindControl("tdTotalForms") as HtmlControl;
				Label lbFormCount = tdTotalForms.FindControl("TotalformsCount") as Label;
				if (lbFormCount.Text == "" || lbFormCount.Text == "-" || lbFormCount.Text == "-1")
					bIsHiddenCountDt = false;
				else
				{
					bIsHiddenCountDt = true;
					break;
				}
			}
			foreach (ListViewDataItem item in lstvwAdmissionStatus.Items)
			{
				HtmlControl tdLotteryDt = item.FindControl("tdLottaryDate") as HtmlControl;
				Label lbLotteryDate = tdLotteryDt.FindControl("lblLottoryDate") as Label;
				if (lbLotteryDate.Text == "" || lbLotteryDate.Text == "-" || lbLotteryDate.Text == "-1")
					bIsHiddenLotteryDt = false;
				else
				{
					bIsHiddenLotteryDt = true;
					break;
				}
			}

			foreach (ListViewDataItem item in lstvwAdmissionStatus.Items)
			{
				HtmlControl tdStartDate = item.FindControl("tdStartDate") as HtmlControl;
				HtmlControl tdEndDate = item.FindControl("tdEndDate") as HtmlControl;
				HtmlControl tdTotalForms = item.FindControl("tdTotalForms") as HtmlControl;
				HtmlControl tdLotteryDt = item.FindControl("tdLottaryDate") as HtmlControl;
				tdStartDate.Visible = bIsHiddenStartDt;
				tdEndDate.Visible = bIsHiddenEndDt;
				tdTotalForms.Visible = bIsHiddenCountDt;
				tdLotteryDt.Visible = bIsHiddenLotteryDt;
			}
			thStartDate.Visible = bIsHiddenStartDt;
			thEndDate.Visible = bIsHiddenEndDt;
			thTotalForms.Visible = bIsHiddenCountDt;
			thLottaryDate.Visible = bIsHiddenLotteryDt;
			trSelectionCriteria.Visible = bIsHiddenLotteryDt;
			trSelectedCandidates.Visible = bIsHiddenLotteryDt;
			trSelectiontext.Visible = bIsHiddenLotteryDt;
			tdSelectiontext.Visible = bIsHiddenLotteryDt;
			trSelectedtext.Visible = bIsHiddenLotteryDt;
			tdSelectedtext.Visible = bIsHiddenLotteryDt;

            if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.ZLSP.ToInt())
            {
                thDOBMinLimit.Visible = true;
                thDOBMaxLimit.Visible = true;                
            }
            else
            {
                thDOBMinLimit.Visible = false;
                thDOBMaxLimit.Visible = false;                
            }
            
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}
    /// <summary>
    /// 	This method is used to handle a lstvwAdmissionStatusNxtYear lisview Item databound and redirect to admission form page with selcted standard.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void lstvwAdmissionStatusNxtYear_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Admission")
            {
                bool bEnableAdmissionFormFee = Convert.ToBoolean(lstvwAdmissionStatusNxtYear.DataKeys[e.Item.DisplayIndex]["EnableAdmissionFormFee"]);
                int iAcademicYearId = Convert.ToInt32(lstvwAdmissionStatusNxtYear.DataKeys[e.Item.DisplayIndex]["Academic_Year_Id"]);
                int iStandardId = e.CommandArgument.ToInt();
                string sStandardName = Convert.ToString(lstvwAdmissionStatusNxtYear.DataKeys[e.Item.DisplayIndex]["Standard_Name"]);
                string sQuerystring = "StandardId=" + iStandardId + "&StandardName=" + sStandardName + "&EnableAdmissionFormFee=" + bEnableAdmissionFormFee + "&AcademicYearId=" + iAcademicYearId;
                sQuerystring = CommonUtility.EncryptQuerystring(sQuerystring);
                Response.Redirect("~/RITeSchool/Admission/AdmissionFormDocuments.aspx?" + sQuerystring, false);
            }
        }
        catch (ThreadAbortException)
        { }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwAdmissionStatusNxtYear_DataBound(object sender, EventArgs e)
    {
        try
        {
            HtmlControl thStartDate = lstvwAdmissionStatusNxtYear.FindControl("thStartDate") as HtmlControl;
            HtmlControl thEndDate = lstvwAdmissionStatusNxtYear.FindControl("thEndDate") as HtmlControl;
            HtmlControl thLottaryDate = lstvwAdmissionStatusNxtYear.FindControl("thLottaryDate") as HtmlControl;
            HtmlControl thTotalForms = lstvwAdmissionStatusNxtYear.FindControl("thTotalForms") as HtmlControl;
            HtmlControl thDOBMinLimit = lstvwAdmissionStatusNxtYear.FindControl("thDOBMinLimit") as HtmlControl;
            HtmlControl thDOBMaxLimit = lstvwAdmissionStatusNxtYear.FindControl("thDOBMaxLimit") as HtmlControl;

            bool bIsHiddenStartDt = false;
            bool bIsHiddenEndDt = false;
            bool bIsHiddenCountDt = false;
            bool bIsHiddenLotteryDt = false;

            foreach (ListViewDataItem item in lstvwAdmissionStatusNxtYear.Items)
            {
                HtmlControl tdStartDate = item.FindControl("tdStartDate") as HtmlControl;
                Label lbStartDate = tdStartDate.FindControl("formOpenDate") as Label;
                if (lbStartDate.Text == "" || lbStartDate.Text == "-")
                    bIsHiddenStartDt = false;
                else
                {
                    bIsHiddenStartDt = true;
                    break;
                }
            }

            foreach (ListViewDataItem item in lstvwAdmissionStatusNxtYear.Items)
            {
                HtmlControl tdEndDate = item.FindControl("tdEndDate") as HtmlControl;
                Label lbEndDate = tdEndDate.FindControl("lblCloseDt") as Label;
                if (lbEndDate.Text == "" || lbEndDate.Text == "-")
                    bIsHiddenEndDt = false;
                else
                {
                    bIsHiddenEndDt = true;
                    break;
                }
            }
            foreach (ListViewDataItem item in lstvwAdmissionStatusNxtYear.Items)
            {
                HtmlControl tdTotalForms = item.FindControl("tdTotalForms") as HtmlControl;
                Label lbFormCount = tdTotalForms.FindControl("TotalformsCount") as Label;
                if (lbFormCount.Text == "" || lbFormCount.Text == "-" || lbFormCount.Text == "-1")
                    bIsHiddenCountDt = false;
                else
                {
                    bIsHiddenCountDt = true;
                    break;
                }
            }
            foreach (ListViewDataItem item in lstvwAdmissionStatusNxtYear.Items)
            {
                HtmlControl tdLotteryDt = item.FindControl("tdLottaryDate") as HtmlControl;
                Label lbLotteryDate = tdLotteryDt.FindControl("lblLottoryDate") as Label;
                if (lbLotteryDate.Text == "" || lbLotteryDate.Text == "-" || lbLotteryDate.Text == "-1")
                    bIsHiddenLotteryDt = false;
                else
                {
                    bIsHiddenLotteryDt = true;
                    break;
                }
            }

            foreach (ListViewDataItem item in lstvwAdmissionStatusNxtYear.Items)
            {
                HtmlControl tdStartDate = item.FindControl("tdStartDate") as HtmlControl;
                HtmlControl tdEndDate = item.FindControl("tdEndDate") as HtmlControl;
                HtmlControl tdTotalForms = item.FindControl("tdTotalForms") as HtmlControl;
                HtmlControl tdLotteryDt = item.FindControl("tdLottaryDate") as HtmlControl;
                tdStartDate.Visible = bIsHiddenStartDt;
                tdEndDate.Visible = bIsHiddenEndDt;
                tdTotalForms.Visible = bIsHiddenCountDt;
                tdLotteryDt.Visible = bIsHiddenLotteryDt;
            }
            thStartDate.Visible = bIsHiddenStartDt;
            thEndDate.Visible = bIsHiddenEndDt;
            thTotalForms.Visible = bIsHiddenCountDt;
            thLottaryDate.Visible = bIsHiddenLotteryDt;
            trSelectionCriteria.Visible = bIsHiddenLotteryDt;
            trSelectedCandidates.Visible = bIsHiddenLotteryDt;
            trSelectiontext.Visible = bIsHiddenLotteryDt;
            tdSelectiontext.Visible = bIsHiddenLotteryDt;
            trSelectedtext.Visible = bIsHiddenLotteryDt;
            tdSelectedtext.Visible = bIsHiddenLotteryDt;

            if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.ZLSP.ToInt())
            {
                thDOBMinLimit.Visible = true;
                thDOBMaxLimit.Visible = true;
            }
            else
            {
                thDOBMinLimit.Visible = false;
                thDOBMaxLimit.Visible = false;
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    protected void lstvwAdmissionStatusNxtYear_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            HtmlControl thStartDate = lstvwAdmissionStatusNxtYear.FindControl("thStartDate") as HtmlControl;
            HtmlControl thEndDate = lstvwAdmissionStatusNxtYear.FindControl("thEndDate") as HtmlControl;
            HtmlControl thLottaryDate = lstvwAdmissionStatusNxtYear.FindControl("thLottaryDate") as HtmlControl;
            HtmlControl thTotalForms = lstvwAdmissionStatusNxtYear.FindControl("thTotalForms") as HtmlControl;
            HtmlControl thDOBMinLimit = lstvwAdmissionStatusNxtYear.FindControl("thDOBMinLimit") as HtmlControl;
            HtmlControl thDOBMaxLimit = lstvwAdmissionStatusNxtYear.FindControl("thDOBMaxLimit") as HtmlControl;

            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var oCurrentItem = e.Item as ListViewDataItem;
                var oDataRowView = oCurrentItem.DataItem as DataRowView;
                var olnkbtnAdmission = e.Item.FindControl("lnkbtnAdmission") as LinkButton;

                HtmlControl tdStartDate = oCurrentItem.FindControl("tdStartDate") as HtmlControl;
                HtmlControl tdEndDate = oCurrentItem.FindControl("tdEndDate") as HtmlControl;
                HtmlControl tdLottaryDate = oCurrentItem.FindControl("tdLottaryDate") as HtmlControl;
                HtmlControl tdTotalForms = oCurrentItem.FindControl("tdTotalForms") as HtmlControl;
                Label lblCloseDt = oCurrentItem.FindControl("lblCloseDt") as Label;
                Label lblformOpenDate = oCurrentItem.FindControl("formOpenDate") as Label;
                Label lblTotalformsCount = oCurrentItem.FindControl("TotalformsCount") as Label;
                int iStandardId = Convert.ToInt32(lstvwAdmissionStatusNxtYear.DataKeys[e.Item.DisplayIndex]["Standard_Id"]);
                Label lblMinDOB = oCurrentItem.FindControl("lblMinDOB") as Label;
                Label lblMaxDOB = oCurrentItem.FindControl("lblMaxDOB") as Label;

                HtmlControl tdDOBMin = oCurrentItem.FindControl("tdDOBMin") as HtmlControl;
                HtmlControl tdDOBMax = oCurrentItem.FindControl("tdDOBMax") as HtmlControl;

                if (msEnableAdmissionFormFee == Constants.S_NO)
                {
                    thStartDate.Visible = false;
                    thEndDate.Visible = false;
                    thLottaryDate.Visible = false;
                    thTotalForms.Visible = false;
                    tdStartDate.Visible = false;
                    tdEndDate.Visible = false;
                    tdLottaryDate.Visible = false;
                    tdTotalForms.Visible = false;
                    thDOBMinLimit.Visible = false;
                    thDOBMaxLimit.Visible = false;
                }

                if (oDataRowView["FormOpenDate"] != DBNull.Value && DateTime.Now < oDataRowView["FormOpenDate"].ToDateTime())
                {
                    olnkbtnAdmission.Text = "Admission Not Started";
                    olnkbtnAdmission.PostBackUrl = "";
                    olnkbtnAdmission.Enabled = false;
                }

                if (oDataRowView["FormCloseDate"] == DBNull.Value)
                    lblCloseDt.Text = "-";
                if (oDataRowView["FormOpenDate"] == DBNull.Value)
                    lblformOpenDate.Text = "-";

                if (ConfigurationManager.AppSettings["SchoolId"].ToInt() != Constants.SchoolId.PPSH.ToInt())
                {
                    if (oDataRowView["TotalOnlineForms"].ToString() == "-1")
                        lblTotalformsCount.Text = "-";
                }
                else
                    lblTotalformsCount.Text = "-";

                if (oDataRowView["RemainingformsCount"].ToInt() == -1 && // If RemainingFormsCount == -1, it means forms are unlimited, hence we do not close it.
                        ((oDataRowView["FormCloseDate"] != DBNull.Value && DateTime.Now >= oDataRowView["FormCloseDate"].ToDateTime()) // When form close date has past the current date.
                            )) // When forms remaining count is 0)
                {
                    olnkbtnAdmission.Text = "Forms Closed";
                    olnkbtnAdmission.ForeColor = System.Drawing.Color.Red;
                    olnkbtnAdmission.PostBackUrl = "";
                    olnkbtnAdmission.Enabled = false;
                }
                // Close the form submission if the following conditions are met.
                else if (oDataRowView["RemainingformsCount"].ToInt() != -1 && // If RemainingFormsCount == -1, it means forms are unlimited, hence we do not close it.
                        ((oDataRowView["FormCloseDate"] != DBNull.Value && DateTime.Now >= oDataRowView["FormCloseDate"].ToDateTime()) // When form close date has past the current date.
                            || oDataRowView["RemainingformsCount"].ToInt() <= 0)) // When forms remaining count is 0)
                {
                    olnkbtnAdmission.Text = "Forms Closed";
                    olnkbtnAdmission.ForeColor = System.Drawing.Color.Red;
                    olnkbtnAdmission.PostBackUrl = "";
                    olnkbtnAdmission.Enabled = false;
                }

                int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
                if (iSchoolId == Constants.SchoolId.PPSH.ToInt() && (iStandardId == 990) || (iStandardId == 991))
                {
                    olnkbtnAdmission.Attributes.Add("onclick", "if(!AdmissionAlertMessage(" + iStandardId + "," + iSchoolId + ")) {return false}");
                }

                if (iSchoolId == Constants.SchoolId.ZLSP.ToInt())
                {
                    thDOBMinLimit.Visible = true;
                    thDOBMaxLimit.Visible = true;

                    if (lblMinDOB.Text != string.Empty)
                        lblMinDOB.Text = lblMinDOB.Text.ToDateTime().ToString(Constants.S_DATE_FORMAT);

                    if (lblMaxDOB.Text != string.Empty)
                        lblMaxDOB.Text = lblMaxDOB.Text.ToDateTime().ToString(Constants.S_DATE_FORMAT);
                }
                else
                {
                    tdDOBMin.Visible = false;
                    tdDOBMax.Visible = false;
                }

                if (iSchoolId == Constants.SchoolId.PPS.ToInt())
                {
                    Label lblStdName = oCurrentItem.FindControl("StdName") as Label;
                    string sStdName = HttpUtility.HtmlDecode(lblStdName.Text);

                    // Check if standard name exists in Internal Link Standards
                    InternalLinkStandardDetails oInternalLinkStandard = mlstInternalLinkStandardDetails.Find(x => x.StandardName == sStdName);
                    if (oInternalLinkStandard != null)
                    {
                        olnkbtnAdmission.Attributes.Remove("onclick");
                        olnkbtnAdmission.Text = oInternalLinkStandard.DisplayMessage;
                        olnkbtnAdmission.ForeColor = System.Drawing.Color.Red;
                        olnkbtnAdmission.PostBackUrl = "";
                        olnkbtnAdmission.Enabled = false;
                    }                    
                }

                if (iSchoolId == Constants.SchoolId.PPSH.ToInt())
                {
                    Label lblStdName = oCurrentItem.FindControl("StdName") as Label;
                    string sStdName = HttpUtility.HtmlDecode(lblStdName.Text);

                    if (dictGoogleForm.ContainsKey(sStdName))
                    {
                        olnkbtnAdmission.Text = "Add to waiting list";
                        olnkbtnAdmission.ForeColor = System.Drawing.Color.Blue;
                        olnkbtnAdmission.Attributes["style"] = "text-decoration: underline; cursor: pointer;";
                        olnkbtnAdmission.Attributes.Add("onclick", "window.open('" + dictGoogleForm[sStdName] + "','_blank'); return false;");
                    }
                    //if (sStdName == "1" || sStdName == "2" || sStdName == "3" || sStdName == "4" ||  sStdName == "5")
                    //{
                    //    olnkbtnAdmission.Text = "Link will be shared soon";
                    //    olnkbtnAdmission.Attributes.Add("onclick", "return false;");
                    //    olnkbtnAdmission.CssClass = "disabled-link";
                    //    olnkbtnAdmission.ToolTip = string.Empty;
                    //}

                    InternalLinkStandardDetails oInternalLinkStandard = mlstInternalLinkStandardDetails.Find(x => x.StandardName == sStdName);
                    if (oInternalLinkStandard != null)
                    {
                        olnkbtnAdmission.Text = oInternalLinkStandard.DisplayMessage;
                        olnkbtnAdmission.Enabled = false;
                    }
                }
                else if (iSchoolId == Constants.SchoolId.PPSN.ToInt())
                {
                    Label lblStdName = oCurrentItem.FindControl("StdName") as Label;
                    string sStdName = HttpUtility.HtmlDecode(lblStdName.Text);

                    // Check if standard name exists in Internal Link Standards
                    InternalLinkStandardDetails oInternalLinkStandard = mlstInternalLinkStandardDetails.Find(x => x.StandardName == sStdName);
                    if (oInternalLinkStandard != null)
                    {
                        olnkbtnAdmission.Text = oInternalLinkStandard.DisplayMessage;
                        olnkbtnAdmission.Enabled = false;
                    }
                    //else
                    //{
                    //    if (sStdName == "1" || sStdName == "Senior KG" || sStdName == "6")
                    //    {
                    //        olnkbtnAdmission.Text = "No seats are available.";
                    //        olnkbtnAdmission.Enabled = false;
                    //    }

                    //    if (sStdName == "9" || sStdName == "10")
                    //    {
                    //        olnkbtnAdmission.Text = "-";
                    //        olnkbtnAdmission.Enabled = false;
                    //    }

                    //    if (sStdName == "Junior KG")
                    //    {
                    //        olnkbtnAdmission.Text = "Forms Closed";
                    //        olnkbtnAdmission.Enabled = false;
                    //    }
                    //}


                    //if (sStdName == "2" || sStdName == "3" || sStdName == "4" || sStdName == "6" || sStdName == "7" || sStdName == "8" || sStdName == "9" || sStdName == "10")
                    //{
                    //    olnkbtnAdmission.Text = "Admission not started.";
                    //    olnkbtnAdmission.Enabled = false;
                    //}
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

    /// <summary>
    /// This method is used to fill bank and card details of net banking.
    /// </summary>
    private void FillNetBankingDetails()
    {
        List<PaymentGateWayDetails> lstPaymentGateWayDetails = NetBankingPaymentTransactionsBL.GetPaymentGatewayDetails("0");
        PaymentGateWayDetails oPaymentGateWayDetails = new PaymentGateWayDetails();

        if (lstPaymentGateWayDetails.Exists(a => a.GatewayId == Constants.PaymentGateways.TPSL.ToInt()))
            lblNote2.Visible = false;
        else
            lblNote1.Visible = false;

        var oStudentFeeDetailsBL = new StudentFeeDetailsBL();
        DataSet oDataSet = oStudentFeeDetailsBL.GetBankDetailsForNetBanking(Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]));

        lstvwBankDetails.DataSource = oDataSet.Tables[I_BANK_DETAILS_TABLE];
        lstvwBankDetails.DataBind();

        if (oDataSet.Tables[I_CARD_DETAILS_TABLE].Rows.Count > 0)
        {
            lstvwCardDetails.DataSource = oDataSet.Tables[I_CARD_DETAILS_TABLE];
            lstvwCardDetails.DataBind();
        }
        else
        {
            trCardGateway.Visible = false;
            trCardDetails.Visible = false;
        }
    }

	/// <summary>
	/// This function is used to set control visibility.
	/// </summary>
	private void SetControls()
	{
		msEnableAdmissionFormFee = Settings.EnableAdmissionFormFee ? Constants.S_YES : Constants.S_NO;
        int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
		if (msEnableAdmissionFormFee == Constants.S_NO)
		{            
			SubmissionWizardSteps.EnableFormFee = false;
			trPaymentOnline.Visible = false;
            if (iSchoolId == Constants.SchoolId.SS.ToInt())
            {
                trPaymentOffline.Visible = false;
                trPaymentOffline_SS.Visible = true;
            }
            else
            {
                trPaymentOffline.Visible = true;
                trPaymentOffline_SS.Visible = false;
            }
			trPrintPayment.Visible = false;
			trFive.Visible = false;            
			trNetbankingDetails.Visible = false;
			trSelectionCriteria.Visible = false;
			trSelectedtext.Visible = false;
			trSelectedCandidates.Visible = false;
			trSelectiontext.Visible = false;
			trConfirmation.Visible = false;
			trConfirmationText.Visible = false;
			trConfirmation_Copy.Visible = true;
			trConfirmationText_Copy.Visible = true;
		}
		else
		{
			trPaymentOnline.Visible = true;
			trPaymentOffline.Visible = false;
            trPaymentOffline_SS.Visible = false;
            if (iSchoolId == Constants.SchoolId.PPSH.ToInt())
                trSix.Visible = true;
            else
                trSix.Visible = false;
			trPrint.Visible = false;
			trFour.Visible = false;
		}

        if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.DPIS.ToInt())
            trFour.Visible = false;

        if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.JPS.ToInt() ||
            ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.NEMS.ToInt() || 
            ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.JOS.ToInt() ||
            ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.EPPS.ToInt()
            ) 
        {
            trPaymentOnline.Visible = false;
            trPaymentOffline.Visible = false;
        }

        if(ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.MCPS.ToInt())
        {
            trAdmissionSpace.Visible = true;
            trAdmission.Visible=true;
        }

        if (iSchoolId == Constants.SchoolId.DPIS.ToInt() || iSchoolId == Constants.SchoolId.DPISRAVET.ToInt())
        {
            trOnlineHeaderDPIS.Visible = true;
            trOnlineHeaderOther.Visible = false;
            trOnlinePaymentDPIS.Visible = true;
            trPaymentOnline.Visible = false;
            trPaymentOffline.Visible = false;
            trPaymentOffline_SS.Visible = false;
            trFive.Visible = false;
            trFiveDPIS.Visible = true;
            trPaymentInfo.Visible = false;
            trNetbankingDetails.Visible = false;
            lblStandardList.Text = "Standard selection for admission application 2022-23 :";
            spnNextYearLabel.InnerText = "Standard selection for admission application 2023-24 :";
            trNextYear.Visible = true;
            trOldStandardRow.Visible = true;
            trPrintPayment.Visible = false;
            //trPrintPaymentDPIS.Visible = true;
            trPrint.Visible = false;
            lblNo.InnerText = "4";

            if (iSchoolId == Constants.SchoolId.DPIS.ToInt())
            {
                trDPISBranch.Visible = true;
                lblBranchName.Text = "Branch - Pimple Saudagar";
            }
        }
        else
        {
            if (iSchoolId == Constants.SchoolId.ZLSP.ToInt())
            {
                trPrint.Visible = false;
                trPaymentInfo.Visible = false;
                trNetbankingDetails.Visible = false;
            }
            else if (iSchoolId == Constants.SchoolId.PPS.ToInt())
            {
                //not to show trPrint.
            }
            else
                trPrint.Visible = true;

            trOnlineHeaderDPIS.Visible = false;
            trOnlineHeaderOther.Visible = true;
            trOnlinePaymentDPIS.Visible = false;
            trFiveDPIS.Visible = false;
            trPrintPaymentDPIS.Visible = false;
            
            lblNo.InnerText = "5";
            
        }

        if (iSchoolId == Constants.SchoolId.DYPV.ToInt())
            trPrint.Visible = false;

        if (iSchoolId == Constants.SchoolId.DPISRAVET.ToInt())
        {
            trDPISBranch.Visible = true;
            lblBranchName.Text = "Branch - Ravet";
        }
	}

    

	/// <summary>
	///		Gets the Name of the page that referred to this page.
	/// </summary>
	/// <returns></returns>
	private string GetFromPageUrl()
	{
		string sSourcePageUrl = string.Empty;
		if (Request.UrlReferrer != null)
		{
			sSourcePageUrl = Request.UrlReferrer.AbsolutePath;
			sSourcePageUrl = sSourcePageUrl.Substring(sSourcePageUrl.LastIndexOf("/") + 1);
		}
		return sSourcePageUrl;
	}

	private void DecryptQueryString()
	{
		if (QueryString["sIsSubling"] == null || QueryString["sIsSubling"] != Constants.S_YES)
			Session.Remove(Constants.S_SESSION_STUDENT_ADMISSION_ID);
        
	}

	/// <summary>
	/// 	This method is used to bind data to admission status data.
	/// </summary>
	private void BindAdmissionStausListView()
	{
        var oStudentAdmissionsBL = new StudentAdmissionsBL();
		DataSet oDDataSet = oStudentAdmissionsBL.GetCurrentAdmissionStatus(ConfigurationManager.AppSettings["SchoolID"].ToInt());
		if (oDDataSet.Tables.Count <= 0 || oDDataSet.Tables[0].Rows.Count <= 0)
		{
				lblErrorMsg.Visible = true;
				lstvwBankDetails.Visible = false;
				trNetbankingDetails.Visible = false;
				trAdmissionProcessDetails.Visible = false;
                lblStandardList.Visible = false;
				if (oDDataSet.Tables.Count <= 0 || oDDataSet.Tables[0].Rows.Count <= 0)
				{
                    if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.MCPS.ToInt())
                    {
                        lblErrorMsg.Visible = false;
                        trMCPSadmissionClosed.Visible = true;
                    }
                    else
                    {
                        trMCPSadmissionClosed.Visible = false;
                        lblErrorMsg.Text = S_FORM_CLOSE;
                    }
					return;
				}
		}
		if (oDDataSet.Tables.Count >= 0 && oDDataSet.Tables[0].Rows.Count > 0 )
		{
			//int iRemainingFormCount = 0;
			////DataRow[] oDataRow = oDDataSet.Tables[0].Select("Standard_Id=" + Session["StandardId"].ToInt());
			////if (oDataRow.Length<=0 || oDataRow[0].IsNull("RemainingformsCount"))
			//    Int32.TryParse(oDDataSet.Tables[0].Rows[0]["RemainingformsCount"].ToString(), out iRemainingFormCount);
			////else
			////    Int32.TryParse(oDataRow[0]["RemainingformsCount"].ToString(), out iRemainingFormCount);
			//Session["RemainingformsCount"] = iRemainingFormCount;
			if(oDDataSet.Tables[0].Rows[0]["LottoryDate"]!=DBNull.Value)
			hidLotteryDate.Value = Convert.ToDateTime(oDDataSet.Tables[0].Rows[0]["LottoryDate"]).ToString("dd-MMM-yyyy");
		}
			//DataTable odtFormDetail = oDDataSet.Tables[2];
			//hidFormCount.Value = (odtFormDetail.Rows[0]["FormCount"]).ToString();
			DataTable oDataTable = oDDataSet.Tables[0];

            //if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.PPS.ToInt())
            //{
            //    DataRow[] dr = oDataTable.Select("Standard_Name='" + QueryString["std"] + "'");
            //    if (dr.Length > 0)
            //    {
            //        oDataTable = dr.CopyToDataTable();
            //        trOldStandardRow.Visible = true;
            //    }
            //    else
            //    {
            //        oDataTable.Rows.Clear();
            //        trOldStandardRow.Visible = false;
            //    }
            //}


            if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.DPIS.ToInt() || ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.DPISRAVET.ToInt())
            {
                DataRow[] dr = oDataTable.Select("FormCloseDate<'" + DateTime.Now + "'");
                if (oDataTable.Rows.Count == dr.Length)
                {
                    lstvwAdmissionStatus.Visible = false;
                    trOldStandardRow.Visible = false;
                }
                else
                {
                    lstvwAdmissionStatus.Visible = true;
                    trOldStandardRow.Visible = true;
                    lblStandardList.Visible = true;
                    //trNextYear.Visible = false;
                    trOldStandardRow.Visible = false;
                    //lblStandardList.Text = "Standard selection for admission application for year 2023-24";
                    spnNextYearLabel.InnerText = "Standard selection for admission application for year 2025-26";

                    //lstvwAdmissionStatus.DataSource = oDataTable;
                    //lstvwAdmissionStatus.DataBind();
                }
            }
            else
            {
                if (ConfigurationManager.AppSettings["SchoolId"].ToInt() != Constants.SchoolId.PPSH.ToInt())
                {
                    lstvwAdmissionStatus.DataSource = oDataTable;
                    lstvwAdmissionStatus.DataBind();
                }
                else
                    trOldStandardRow.Visible = false;
            }

            if (oDataTable.Rows.Count > 0)
                Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID] = oDataTable.Rows[0]["Academic_Year_Id"];
                
			if (Session[Constants.S_SESSION_STUDENT_FORM_NUMBER] != null && Session[Constants.S_SESSION_STUDENT_ADMISSION_ID] != null)
				trLoginButton.Visible = false;

			lstvwBankDetails.DataSource = oDDataSet.Tables[1];
			lstvwBankDetails.DataBind();

			if (msEnableAdmissionFormFee == Constants.S_YES)
				return;
			
			lblErrorMsg.Visible = true;
			//lblErrorMsg.Text =S_FORM_CLOSE;
			// If Online admission forms are closed, we hide the Netbanking details and admission process details.
			trNetbankingDetails.Visible = false;
		
	}
    private void BindAdmissionStatusListViewNxtYear() //for nxt year
    {
        int aiSchoolId = ConfigurationManager.AppSettings["SchoolId"].ToInt();
        int aiAcademicYearId = Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID].ToInt();
        int aiUpdatedById = 0;

        var oStudentAdmissionsBL = new StudentAdmissionsBL(aiSchoolId, aiAcademicYearId, aiUpdatedById);                                                                                 
        DataSet oDDataSet = oStudentAdmissionsBL.GetCurrentAdmissionStatusNxtYear(ConfigurationManager.AppSettings["SchoolID"].ToInt());     
        if (oDDataSet.Tables.Count <= 0 || (oDDataSet.Tables[0].Rows.Count <= 0 && Settings.ShowAdmissionForCurrentYear == false))
        {
            lblErrorMsg.Visible = true;
            lstvwBankDetails.Visible = false;
            trNetbankingDetails.Visible = false;
            trAdmissionProcessDetails.Visible = false;
            lblStandardList.Visible = false;
            if (oDDataSet.Tables.Count <= 0 || oDDataSet.Tables[0].Rows.Count <= 0)
            {
                if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.MCPS.ToInt())
                {
                    lblErrorMsg.Visible = false;
                    trMCPSadmissionClosed.Visible = true;
                }
                else
                {
                    trMCPSadmissionClosed.Visible = false;
                    lblErrorMsg.Text = S_FORM_CLOSE;
                }
               // return;  //line comment
            }
        }
        if (oDDataSet.Tables.Count >= 0 && oDDataSet.Tables[0].Rows.Count > 0)
        {
            //int iRemainingFormCount = 0;
            ////DataRow[] oDataRow = oDDataSet.Tables[0].Select("Standard_Id=" + Session["StandardId"].ToInt());
            ////if (oDataRow.Length<=0 || oDataRow[0].IsNull("RemainingformsCount"))
            //    Int32.TryParse(oDDataSet.Tables[0].Rows[0]["RemainingformsCount"].ToString(), out iRemainingFormCount);
            ////else
            ////    Int32.TryParse(oDataRow[0]["RemainingformsCount"].ToString(), out iRemainingFormCount);
            //Session["RemainingformsCount"] = iRemainingFormCount;
            if (oDDataSet.Tables[0].Rows[0]["LottoryDate"] != DBNull.Value)
                hidLotteryDate.Value = Convert.ToDateTime(oDDataSet.Tables[0].Rows[0]["LottoryDate"]).ToString("dd-MMM-yyyy");

            SetInternalLink(aiSchoolId, oDDataSet.Tables[0].Rows[0]["Academic_Year_Id"].ToInt());
        }
        //DataTable odtFormDetail = oDDataSet.Tables[2];
        //hidFormCount.Value = (odtFormDetail.Rows[0]["FormCount"]).ToString();
        DataTable oDataTable = oDDataSet.Tables[0];

        if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.PPS.ToInt())
        {
            //DataRow[] dr = oDataTable.Select("Standard_Name='" + QueryString["std"] + "'");
            //if (dr.Length > 0)
            //{
            //    oDataTable = dr.CopyToDataTable();
            //    trNextYear.Visible = true;
            //}
            //else
            //{
            //    oDataTable.Rows.Clear();
            //    trNextYear.Visible = false;
            //}

            spnFormFeeNote.InnerText = "Admission Application Form Fee of Rs." + SchoolBase.Settings.AdmissionFormFees + "/- is payable for each admission of any standard.";
            if (!SchoolBase.Settings.ShowAdmissionForCurrentYear)
            {
                int iNxtAccId = oDataTable.Rows[0]["Academic_Year_Id"].ToInt();

                SchoolBL oSchoolBL = new SchoolBL();
                Dictionary<int, YearwiseSchoolSettings> dictAllAcademicYearSettings = oSchoolBL.GetSchoolSettings(ConfigurationManager.AppSettings["SchoolId"].ToInt());
                YearwiseSchoolSettings oYearwiseSchoolSettings = dictAllAcademicYearSettings[iNxtAccId];

                spnFormFeeNote.InnerText = "Admission Application Form Fee of Rs." + oYearwiseSchoolSettings.AdmissionFormFees + "/- is payable for each admission of any standard.";
            }

            //lblStandardList.Visible = false;
            lblStandardList.Text = "Standard selection for admission application for year 2022-23";
            spnNextYearLabel.InnerText = "Standard selection for admission application for year 2024-25";
        }
        else
        {
            trNextYear.Visible = oDataTable.Rows.Count > 0;
        }
      
        if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.PPSH.ToInt())
        {
            int iNxtAccId = oDataTable.Rows[0]["Academic_Year_Id"].ToInt();
            oStudentAdmissionsBL = new StudentAdmissionsBL(aiSchoolId, iNxtAccId, aiUpdatedById);
         
            List<StandardsWaitingList> lstStds = oStudentAdmissionsBL.GetWaitingStandardsList();
             
           foreach (var item in lstStds)
            {
                if (!dictGoogleForm.ContainsKey(item.StandardName) && !string.IsNullOrWhiteSpace(item.WaitingListURL))
                {
                    dictGoogleForm.Add(item.StandardName, item.WaitingListURL);
                }
            }
        }

        lstvwAdmissionStatusNxtYear.DataSource = oDataTable;   //
        lstvwAdmissionStatusNxtYear.DataBind();               //

        //if (oDataTable.Rows.Count > 0)
        //   // Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID] = oDataTable.Rows[0]["Academic_Year_Id"]; //
        //    Session[Constants.S_SESSION_ACADEMIC_YEAR_IS_NEWLYCREATED] = oDataTable.Rows[0]["Academic_Year_Id"];//
        //if (Session[Constants.S_SESSION_STUDENT_FORM_NUMBER] != null && Session[Constants.S_SESSION_STUDENT_ADMISSION_ID] != null)
        //    trLoginButton.Visible = false;

        lstvwBankDetails.DataSource = oDDataSet.Tables[1];
        lstvwBankDetails.DataBind();

        if (msEnableAdmissionFormFee == Constants.S_YES)
            return;

        lblErrorMsg.Visible = true;
        //lblErrorMsg.Text =S_FORM_CLOSE;
        // If Online admission forms are closed, we hide the Netbanking details and admission process details.
        trNetbankingDetails.Visible = false;
    }
	private void HideListviewColumn()
	{

	}

    /// <summary>
    /// This method is sued to set internal link.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiAcademicYearId"></param>
    private void SetInternalLink(int aiSchoolId, int aiAcademicYearId)
    {
        AdmissionProcessDetailsBL oAdmissionProcessDetailsBL = new AdmissionProcessDetailsBL();
        mlstInternalLinkStandardDetails = oAdmissionProcessDetailsBL.GetInternalLinkStandards(aiSchoolId, aiAcademicYearId);
    }

	#endregion -- PRIVATE METHOD(s) --


}