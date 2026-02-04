// File Name     : StandardwiseExamSchedulePopup.aspx.cs
// Modified By   : Amit 
// Modified Date : 12/09/2009
// Description   : This class is used to insert exam instructions.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class StandardwiseExamSchedulePopup : SchoolBase
{

    #region Events

    /// <summary>
    /// This event is used to get class and exam name. 
    /// And to fill exam instuctions for that exam.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            GetQueryString();
            if (!IsPostBack)
            {
                FillExamScheduleData();
                SetClientScriptAttributes();
				valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save or update exam instuction. 
    /// And moves back to exam schedule page of that exam.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SchoolwiseStandardExamScheduleMasterBL oSchoolwiseStandardExamScheduleMasterBL = new SchoolwiseStandardExamScheduleMasterBL(Convert.ToInt32(hidStandardwiseExamScheduleId.Value));
            oSchoolwiseStandardExamScheduleMasterBL.Instructions = txtInstructions.Text.Trim();
            oSchoolwiseStandardExamScheduleMasterBL.Updated_By_Id = Convert.ToString(miUserId);
            oSchoolwiseStandardExamScheduleMasterBL.UpdateExamScheduleInstruction();

            string sQuerystring = "Standard_Id=" + QueryString["Standard_Id"]
                            + "&Test_Name=" + QueryString["Test_Name"]
                            + "&Standard_Name=" + QueryString["Standard_Name"]
                            + "&Standard_Test_Id=" + QueryString["Standard_Test_Id"]
                            + "&Is_Configured=" + QueryString["Is_Configured"];
            sQuerystring = sQuerystring + "&Schoolwise_Standard_Exam_Schedule_Id="
                            + QueryString["StdExamSchedId"]
                            + "&Schoolwise_Test_Id="+QueryString["Schoolwise_Test_Id"];
            sQuerystring = CommonUtility.EncryptQuerystring(sQuerystring + "&Mode=" + "EDIT");
            sQuerystring = "'?" + sQuerystring + "'";
            Response.Write("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+" + sQuerystring + ";window.close();window.opener.focus(); </Script>");
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method is used to get values coming through querystring.
    /// </summary>
    private void GetQueryString()
    {
        if (QueryString["StdExamSchedId"] != null)
            hidStandardwiseExamScheduleId.Value = QueryString["StdExamSchedId"];
    }

    ///<summary>
    /// This method is used to fill exam schedule data.
    ///</summary>
    private void FillExamScheduleData()
    {
        SchoolwiseStandardExamScheduleMasterBL oSchoolwiseStandardExamScheduleMasterBL = new SchoolwiseStandardExamScheduleMasterBL(Convert.ToInt32(hidStandardwiseExamScheduleId.Value));
        txtInstructions.Text = oSchoolwiseStandardExamScheduleMasterBL.Instructions;
    }

    /// <summary>
    /// This method is used to set java script to controls.
    /// </summary>
    private void SetClientScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel });
        btnCancel.Attributes["onclick"] = "closewindow()";
        SetDefaultButton(btnSave);
    }

    #endregion
}
