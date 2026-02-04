/* File Name = StayBackLectureAssignmentPopUpUI.aspx.cs
 * Created Date - 
 * Modified Date  - 23 June 2011
 * Created by - Vipul
 * Class Description - This class is defined to manage stay back lectures for a class.*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using ExternalLectures;
using Utility;

public partial class StayBackLectureAssignmentPopUpUI : SchoolBase
{
    #region "Data Members"

    ExternalLecturesBL moExternalLecturesBL;

    #endregion "Data Members"

    #region "Constants"

    private const string S_STAYBACK_LECTURE__MESSAGE = "Stay Back lectures saved successfully !!!";
    private const string S_ASSEMBLY_LECTURE__MESSAGE = "Assembly lectures saved successfully !!!";
    private const string S_MPT_LECTURE__MESSAGE = "M.P.T. lectures saved successfully !!!";
    private const string S_WEEKLY_TEST_MESSAGE = "Weekly Test saved successfully !!!";

    private const string S_MPT_INTENT = "Assign M.P.T. Lectures For Class";
    private const string S_STAYBACK_INTENT = "Assign Stay Back Lectures For Class";
    private const string S_ASSEMBLY_INTENT = "Assign Assembly Lectures For Class";
    private const string S_WEEKLYTEST_INTENT = "Assign Weekly Test For Class";

    #endregion
   
    #region "Event"

    /// <summary>
    /// This event is used to fill stay back lecture details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!IsPostBack)
        {
            SetHiddenFields();
            SetJavaScriptAttributres();
            FillStayBackLectureDetails();
            btnClose.Attributes.Add("onclick", "CloseWindow();");
        }
        lblIndentDetails.Text = hidIndentMessage.Value;
    }

    /// <summary>
    /// This event is used to save stay back lecture details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
		try
		{
	            int iStandardDivisionId = Convert.ToInt32(hidStandardDivisionId.Value);
				int iWeekDayId = Convert.ToInt32(hidWeekDayId.Value);
				moExternalLecturesBL = new ExternalLecturesBL();
				moExternalLecturesBL.SaveStayBackLectureDetails(GenerateXml(GetStayBackLectureDetails()), miSchoolId, miAcademicYearId, miUserId, hidWeekDay.Value, iStandardDivisionId,hidLectureType.Value.ToString());
				FillStayBackLectureDetails();
			    lblUpdateSucess.Text = hidSuccessMessge.Value;
                if (!IsConfigured())
					SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.ExternalLectureConfiguration));
			}
	
		catch (Exception ex)
		{
			BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
    }

    #endregion "Event"

    #region "Private Methods"

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavaScriptAttributres()
    {
        ApplyMouseHoverEffect(new List<Button> { btnClose, btnSave });
    }

    /// <summary>
    /// This method is used to fill stay back lecture details.
    /// </summary>
    private void FillStayBackLectureDetails()
    {
        moExternalLecturesBL = new ExternalLecturesBL();
        int iStandardDivisionId = Convert.ToInt32(hidStandardDivisionId.Value);
        int iWeekDayId = Convert.ToInt32(hidWeekDayId.Value);        
        moExternalLecturesBL.GetStandardWeekDaywiseStayBackLectureDetails(iStandardDivisionId, iWeekDayId, miSchoolId, miAcademicYearId,hidLectureType.Value);
        lblStandardName.Text = moExternalLecturesBL.StandardWeekDaywsieStayBackLectureDetails.StandardName;
        lblDivisionName.Text = moExternalLecturesBL.StandardWeekDaywsieStayBackLectureDetails.DivisionName;
        hidWeekDay.Value = lblWeekDay.Text = moExternalLecturesBL.StandardWeekDaywsieStayBackLectureDetails.WeekDay;
        lblWeekDay.Text = moExternalLecturesBL.StandardWeekDaywsieStayBackLectureDetails.WeekdayShortName;
        AddLectureCheckBoxes(moExternalLecturesBL);
    }

    /// <summary>
    /// This method is used to set lecture check boxes.
    /// </summary>
    /// <param name="aoExternalLecturesBL"></param>
    private void AddLectureCheckBoxes(ExternalLecturesBL aoExternalLecturesBL)
    {
        int iMaxLectures = aoExternalLecturesBL.StandardWeekDaywsieStayBackLectureDetails.MaxNoOfLecturesForStandard;
        List<StayBackLectureDetails> lstStayBackLectureDetails = aoExternalLecturesBL.lstStayBackLectureDetails;
        if (iMaxLectures != 0)
        {
            lblMaxLecturesNotAssigned.Visible = false;
            chklstLectures.Items.Clear();
            for (int iLectureNo = 0; iLectureNo < iMaxLectures; iLectureNo++)
            {
                List<int> lstStayBackDetailsId = (from stayBackLecture in lstStayBackLectureDetails
                                                  where stayBackLecture.LectureNo == (iLectureNo + 1)
                                                  select stayBackLecture.StayBackDetailsId).ToList();
                chklstLectures.Items.Add(new ListItem((iLectureNo + 1).ToString(), (lstStayBackDetailsId.Count > 0) ? lstStayBackDetailsId[0].ToString() : Constants.I_ZERO.ToString()));
                chklstLectures.Items[iLectureNo].Selected = (lstStayBackDetailsId.Count > 0) ? true : false;
                chklstLectures.Items[iLectureNo].Attributes.Add("onclick", "SetLabeles()");
            }
        }
        else
            lblMaxLecturesNotAssigned.Visible = true;
    }

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    private bool IsConfigured()
    {
       return QueryString["Is_Configured"] == Constants.S_YES;
    }

    /// <summary>
    /// This method is used to set hidden fields.
    /// </summary>
    private void SetHiddenFields()
    {
        hidStandardDivisionId.Value = QueryString["StandardDivisionId"];
        hidWeekDayId.Value = QueryString["WeekDayId"];
        hidLectureType.Value = QueryString["ExternalLecture"];
        if (hidLectureType.Value == hidStaybackLecture.Value)
        {
            hidSuccessMessge.Value = S_STAYBACK_LECTURE__MESSAGE;
            hidIndentMessage.Value=lblIndentDetails.Text = S_STAYBACK_INTENT;
        }
        else if (hidLectureType.Value == hidAssemblyLecture.Value)
        {
            hidSuccessMessge.Value = S_ASSEMBLY_LECTURE__MESSAGE;
            hidIndentMessage.Value=lblIndentDetails.Text = S_ASSEMBLY_INTENT;
        }
        
        else if (hidLectureType.Value == hidMPTLecture.Value)
        {
            hidSuccessMessge.Value = S_MPT_LECTURE__MESSAGE;
            hidIndentMessage.Value=lblIndentDetails.Text = S_MPT_INTENT;
        }

        else if (hidLectureType.Value == hidWeeklyTest.Value)
        {
            hidSuccessMessge.Value = S_WEEKLY_TEST_MESSAGE;
            hidIndentMessage.Value = lblIndentDetails.Text = S_WEEKLYTEST_INTENT;
        }
        
    }

    /// <summary>
    /// This method is used to get stay back lecture details.
    /// </summary>
    /// <returns></returns>
    private List<StayBackLectureDetails> GetStayBackLectureDetails()
    {
        List<StayBackLectureDetails> lstStayBackLectureDetails = new List<StayBackLectureDetails>();

        foreach (ListItem oCurrentItem in chklstLectures.Items)
        {
            if (oCurrentItem.Selected || oCurrentItem.Value != Constants.I_ZERO.ToString())
            {
                StayBackLectureDetails oStayBackLectureDetails = new StayBackLectureDetails()
                {
                    LectureNo = Convert.ToInt32(oCurrentItem.Text),
                    StayBackDetailsId = Convert.ToInt32(oCurrentItem.Value),
                    IsStayBackLecture = oCurrentItem.Selected
                };
                lstStayBackLectureDetails.Add(oStayBackLectureDetails);
            }
        }
        return lstStayBackLectureDetails;
    }



    #endregion "Private Methods"
}