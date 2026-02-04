// File Name  : OnlineAdmissionDashBoardUI.aspx.cs
// Created By : Amit 
// Date       : 25/11/2009
//Description : This class is used to give admission allotment message to parents.

using System;
using System.Configuration;
using System.Web;
using System.Data;
using BusinessLogic;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;


public partial class OnlineAdmissionDashBoardUI : SchoolBase
{
    #region " Events "

    /// <summary>
    /// This event is used to give admission process message. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (Session[Constants.S_SESSION_STUDENT_ADMISSION_ID] != null)
                SetAdmissionMessage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " Events "

    #region " Private Methods "

    /// <summary>
    /// This method is used to set admission allotment message.
    /// </summary>
    private void SetAdmissionMessage()
    {
		int iSchoolId = Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]);
        int iAdmissionId = 0;
        string sLotteryStatus = string.Empty;
        if (Session[Constants.S_SESSION_STUDENT_ADMISSION_ID] != null)
            iAdmissionId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_ADMISSION_ID].ToString());

        StudentAdmissionsBL oStudentAdmissionsBL = new StudentAdmissionsBL();
        DataTable oDTStatus = oStudentAdmissionsBL.GetStudentAdmissionStatus(iSchoolId, iAdmissionId);

        if (oDTStatus.Rows.Count > 0 && Convert.ToBoolean(oDTStatus.Rows[0]["IsLotteryConfirmed"]) == true)
        {
            if (oDTStatus.Rows[0]["SelectedInLottery"].ToString() != string.Empty)
            {

                sLotteryStatus = oDTStatus.Rows[0]["SelectedInLottery"].ToString();
                if (sLotteryStatus == "M")
                    trAdmissionConform.Visible = true;
                else if (sLotteryStatus == "W")
                    trWaitingList.Visible = true;
                trInProcess.Visible = false;
            }
            else
            {
                trNotSelected.Visible = true;
                trInProcess.Visible = false;
            }
        }
        else if (oDTStatus.Rows.Count > 0)
            lblLotterydate.Text = Convert.ToDateTime(oDTStatus.Rows[0]["LottoryDate"]).ToString(Constants.S_STANDARD_DATE_FORMAT);
    }
    #endregion " Private Methods "
}
