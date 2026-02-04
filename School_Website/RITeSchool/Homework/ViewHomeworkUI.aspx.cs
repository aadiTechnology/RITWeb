using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;

/// <summary>
/// This class is used to display view homework.
/// </summary>
public partial class ViewHomeworkUI : SchoolBase
{
	#region "Constant"

	private string S_SERVER_PATH = "../DOWNLOADS/Homework/";
	
	#endregion

	#region "Events"
	/// <summary>
	/// This event is used to read query string and display homework details for selected homework id.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			if (!IsPostBack)
			{
				ApplyMouseHoverEffect(new List<Button> { btnCancel });				
				GetHomeworkDetails();
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
		}
	}

	#endregion 

	#region "Private method"

	/// <summary>
	/// This method is used to get homework details.
	/// </summary>
	private void GetHomeworkDetails()
	{
		HomeWorkBL oHomeWorkBL = new HomeWorkBL(miSchoolId, miAcademicYearId, miUserId);
		Homework oHomework = oHomeWorkBL.Get(QueryString["HomeworkId"] != null ? QueryString["HomeworkId"].ToInt() : Constants.I_ZERO);
        if (oHomework != null)
        {
            lblAssignedDt.Text = oHomework.AssignedDate.ToString(Constants.S_STANDARD_DATE_FORMAT);
            lblCompleteDt.Text = oHomework.CompleteByDate.ToString(Constants.S_STANDARD_DATE_FORMAT);
            lblTitle.Text = oHomework.Title;
            lnkAttachment.Text = oHomework.AttachmentPath;
            lnkAttachment.NavigateUrl = S_SERVER_PATH + oHomework.AttachmentPath;
            lnkAttachment.Attributes.Add("onclick", "window.open('" + lnkAttachment.NavigateUrl + "' , '_blank','scrollbars=yes,resizable=yes,top=0,left=0,width=800,height=600'); return false;");
            lblSubject.Text = oHomework.Subject.SubjectName;
            txtDetails.Text = oHomework.Details;

            if (oHomework.Flag != 0)
            {
                lnkAddAttachments.Visible = true;
                string sQueryString = CommonUtility.EncryptQuerystring("HomeworkId=" + oHomework.Id);
                lnkAddAttachments.Attributes.Add("onclick", "window.open('../Homework/HomeAdditionalAttachmentPopUp.aspx?" + sQueryString + "' , '_blank','scrollbars=yes,resizable=yes,top=0,left=0,width=800,height=600'); return false;");
            }
            else
                lnkAddAttachments.Visible = false;
        }
	}

	#endregion
}