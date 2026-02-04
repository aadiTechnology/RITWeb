<%@ WebHandler Language="C#" Class="Ajax" %>
using System;
using System.Web;
using BusinessLogic;
using System.Data;
using Utility;
using System.Web.UI;

public class Ajax : IHttpHandler 
{
    
    
    public void ProcessRequest (HttpContext context) 
    {
        string sReturn = "";
        int iStandardId;
        int iSchooolId;
        int iTeacherId;
        int iAcademicYearId;
        int iTestId;
        int iStandardDivisionId;
        int iSubjectId;
        int iInsertedBy;
        try
        {
            string sTask = context.Request.QueryString.Get("task");
            
            switch (sTask)
            {
                //query string parameters: CasteId  
                case "Caste":
                    int iCasteId = Convert.ToInt32(context.Request.QueryString.Get("CasteId"));
                    sReturn = getAllSubCatsts(iCasteId);
                    break;

                //query string parameters: StandardId, SchoolId   
                case "Standard":
                     iStandardId = Convert.ToInt32(context.Request.QueryString.Get("StandardId"));
                     iSchooolId = Convert.ToInt32(context.Request.QueryString.Get("SchoolId"));
                     iAcademicYearId = Convert.ToInt32(context.Request.QueryString.Get("AcademicYearId"));
                     sReturn = getAllDivisions(iSchooolId, iAcademicYearId, iStandardId);

                    break;
                case "TeacherStandard":
                     iStandardId = Convert.ToInt32(context.Request.QueryString.Get("StandardId"));
                     iSchooolId = Convert.ToInt32(context.Request.QueryString.Get("SchoolId"));
                     iTeacherId = Convert.ToInt32(context.Request.QueryString.Get("TeacherId"));
                     iAcademicYearId = Convert.ToInt32(context.Request.QueryString.Get("AcademicYearIdId"));
                     sReturn =  getAllDivisionsAccordingToTeacher(iSchooolId,iAcademicYearId, iStandardId, iTeacherId);
                    break;
              
                case "SubmitMarksToClassTeacher":
                    iStandardDivisionId = Convert.ToInt32(context.Request.QueryString.Get("StandardDivisionId"));
                    iSchooolId = Convert.ToInt32(context.Request.QueryString.Get("SchoolId"));
                    iSubjectId = Convert.ToInt32(context.Request.QueryString.Get("SubjectId"));
                    iAcademicYearId = Convert.ToInt32(context.Request.QueryString.Get("AcademicYearId"));
                    iTestId = Convert.ToInt32(context.Request.QueryString.Get("testId"));
                    iInsertedBy = Convert.ToInt32(context.Request.QueryString.Get("InsertedById"));
                    string SIsSubmitted = context.Request.QueryString.Get("IsSubmitted").ToString();
                    sReturn = SubmitMarksToClassTeacher(iStandardDivisionId,
                                                                           iSubjectId,
                                                                           iTestId,
                                                                           iSchooolId,
                                                                           iAcademicYearId,
                                                                           iInsertedBy,
                                                                           SIsSubmitted);
                    break;
                case "ValidateDateForAcademicYear":
                    
                    iSchooolId = Convert.ToInt32(context.Request.QueryString.Get("SchoolId"));
                    iAcademicYearId = Convert.ToInt32(context.Request.QueryString.Get("AcademicYearId"));
                    string sFieldName =context.Request.QueryString.Get("FieldName").ToString();
                    DateTime oDt = Convert.ToDateTime(context.Request.QueryString.Get("DateToValidate"));
                    sReturn = validateDateForAcademicYear(iSchooolId,iAcademicYearId, oDt,sFieldName);
                    break;
                case "MenuItems":
                    iSchooolId = Convert.ToInt32(context.Request.QueryString.Get("SchoolId"));
                    sReturn = GetAllMenuItems(iSchooolId);
                    break;
                case "CloseEventWindow":
                    string sEventdate = context.Request.QueryString.Get("EventDate");
                    string sStandardId ="&Standard_Id="+ context.Request.QueryString.Get("Standard_Id");
                    sReturn = GetEncryptedEventDate(sEventdate + sStandardId);
                    break;
                    
            }
            
        }
        catch (Exception)
        {
            

        }
        finally
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(sReturn); 
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
        
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="aiSchooolId"></param>
    /// <param name="aiStandardId"></param>
    /// <param name="aiDivisionId"></param>
    /// <returns></returns>
    private string SubmitMarksToClassTeacher(int aiStandardDivisionId,
                                             int aiSubjectId,
                                             int aiTestId,
                                             int aiSchoolId,
                                             int aiAcademicYrId,
                                             int aiInsertedBy,
                                             string asIsSubmitted)
    {
        string strReturn;
        try
        {
            if (aiSubjectId != -1)
            {
                // StudentBL oStudentBL = new StudentBL();
                SubjectTestTypeConfigurationCollectionBL.SubmitTestMarksToClassTeacher(aiStandardDivisionId,
                                                                               aiSubjectId,
                                                                               aiTestId,
                                                                               aiSchoolId,
                                                                               aiAcademicYrId,
                                                                               asIsSubmitted);

                StandardDivisionMasterBL oStandardDivisionMasterBL = new StandardDivisionMasterBL(aiStandardDivisionId);

                SchoolwiseStandardExamScheduleMasterBL oTestSchedule = new SchoolwiseStandardExamScheduleMasterBL(oStandardDivisionMasterBL.StandardId, aiTestId);				
            }
            else
            {
                PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
                oPrePrimaryProgressSheetConfigBL.SubmitPrePrimaryTest(aiStandardDivisionId, aiSchoolId, aiTestId, aiAcademicYrId, aiInsertedBy);
            }
           
            strReturn = "Marks published succesfully. ";
        }
        catch (Exception)
        {
            strReturn = "Marks could not be Published.";
        }
        return strReturn;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="aiSchooolId"></param>
    /// <param name="aiStandardId"></param>
    /// <returns></returns>
    private  string getAllDivisions(int aiSchooolId,int aiAcademicYearId, int aiStandardId)
    {
        
        DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(aiSchooolId, aiAcademicYearId);
        DataTable oDt = oDivisionCollectionBL.GetAllDivisionsForStandard(aiStandardId);
        
        string sReturn = "";
        DataTable oDT = oDt;
        int iCount = oDT.Rows.Count;
        string sRecord = "";
        for (int i = 0; i < iCount; i++)
        {
            sRecord = oDT.Rows[i][Constants.S_DIVISION_ID_FIELD] + "###" + oDT.Rows[i][Constants.S_DIVISION_NAME_FIELD];
            if (!sReturn.Equals(""))
            {
                sReturn = sReturn + "@@@" + sRecord;
                
            }
            else
            {
                sReturn = sRecord;
            }
        }
        return sReturn;
        
        
    }
    
    private string getAllSubCatsts(int aiCasteId)
    {
        YearWIseStudentsBL oYearWiseBL = new YearWIseStudentsBL();
        MasterDataCollectionBL oMasterBL = new MasterDataCollectionBL();
        
        DataTable oDT = oMasterBL.GetAllSubCastes(aiCasteId);
        string sReturn = "";
        int iCount = oDT.Rows.Count;
        string sRecord = "";
        for (int i = 0; i < iCount; i++)
        {
            sRecord = oDT.Rows[i]["Sub_Caste_Id"] + "###" + oDT.Rows[i]["Sub_Caste_Name"];
            if (sReturn.Equals(""))
            {
                sReturn = sRecord;
            }
            else
            {
                sReturn = sReturn + "@@@" + sRecord;
            }
        }
        return sReturn;


    }
    private string getAllDivisionsAccordingToTeacher(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiTeacherId)
    {         
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();

        DataTable oDT = oMasterDataCollectionBL.GetAllDivisionForTeacher(aiSchoolId,aiAcademicYearId, aiStandardId, aiTeacherId);
          
        string sReturn = "";
        int iCount = oDT.Rows.Count;
        string sRecord = "";
        for (int i = 0; i < iCount; i++)
        {
            sRecord = oDT.Rows[i][Constants.S_DIVISION_ID_FIELD] + "###" + oDT.Rows[i][Constants.S_DIVISION_NAME_FIELD];
            if (sReturn.Equals(""))
            {
                sReturn = sRecord;
            }
            else
            {
                sReturn = sReturn + "@@@" + sRecord;
            }
        }
        return sReturn;
    }

    private string GetAllMenuItems(int aiSchoolId)
    {

        ConfigureCollectionMenuBL oConfigureCollectionMenuBL = new ConfigureCollectionMenuBL();
        DataTable oDT = oConfigureCollectionMenuBL.FetchConfigureMenuCollection(aiSchoolId);

        string sReturn = "";
        int iCount = oDT.Rows.Count;
        string sRecord = "";
        for (int i = 0; i < iCount; i++)
        {
            sRecord = oDT.Rows[i]["configuremenuid"] + "###" + oDT.Rows[i]["ConfigureMenuName"];
            if (sReturn.Equals(""))
            {
                sReturn = sRecord;
            }
            else
            {
                sReturn = sReturn + "@@@" + sRecord;
            }
        }
        return sReturn;
    }

    /// <summary>
    /// This function is used to get encrypted querystring for eventdate.
    /// </summary>
    /// <param name="asQuerystring"></param>
    /// <returns></returns>
    private string GetEncryptedEventDate(string asQuerystring)
    {

        string sReturn = "";
        sReturn = Utility.CommonUtility.EncryptQuerystring(asQuerystring);
        return sReturn;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiAcademicYearId"></param>
    /// <returns></returns>
    private string validateDateForAcademicYear(int aiSchoolId, int aiAcademicYearId,DateTime oDt,string sField)
    {
        SchoolWiseAcademicYearMasterBL oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL(aiSchoolId, aiAcademicYearId);
        DateTime odtStartDate = oSchoolWiseAcademicYearMasterBL.StartDate;
        //oSchoolWiseAcademicYearMasterBL.StartDate.
        DateTime odtEndDate = oSchoolWiseAcademicYearMasterBL.EndDate;
        string sReturn ="";
        if (oDt < odtStartDate || oDt > odtEndDate)
        {
            sReturn = sField + " should be whithin current academic year (i.e. between )" + odtStartDate.ToShortDateString() + " to " + odtEndDate.ToShortDateString();
             
        }

        return sReturn;
         
    }

}