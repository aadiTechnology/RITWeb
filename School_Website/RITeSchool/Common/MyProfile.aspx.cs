using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AccountsEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolBusinessService;
using SchoolEntities.Admin;
using StudentEntities;
using Utility;
using System.Collections;
using System.Configuration;
using MasterEntities;
using PayrollReportingUserEntities;
using System.IO;

public partial class MyProfile : SchoolBase
{
  
    #region DataMembers
    private StudentBL moStudentBL;
    //private YearWiseStudentInfo moYearWiseStudentInfo;
    private static bool mbIncludeUserName = false;
    public int miStudentCount;
    private static string msOperator = string.Empty;

    #endregion

    #region COnstant's

    private const string S_Family_Path = @"../DOWNLOADS/Family Photos/";

    #endregion

    #region -- PRIVATE METHOD(s) --

    /// <summary>
    /// This event is used to intialize page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                GetStudentDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    #endregion -- PRIVATE METHOD(s) --

    #region Basic Details


    public void GetStudentDetails()
    {

        int aiSchoolId = Convert.ToInt32(Session["I_SCHOOL_ID"].ToString());
        int aiStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();

        moStudentBL = new StudentBL(aiSchoolId, aiStudentId, true);

        lblStudentAddress.Text = moStudentBL.Address.ToString() != null && moStudentBL.Address != "" ? moStudentBL.Address : "&nbsp;";
        lblStudentPhoneNo.Text = moStudentBL.ResidencePhoneNo != null && moStudentBL.ResidencePhoneNo != "" ? moStudentBL.ResidencePhoneNo : "&nbsp;";
        lblReligion.Text = moStudentBL.Religion != null && moStudentBL.Religion != "" ? moStudentBL.Religion : "&nbsp;";
        lblCasteSubCaste.Text = moStudentBL.CasteAndSubCaste != null && moStudentBL.CasteAndSubCaste != "" ? moStudentBL.CasteAndSubCaste : "&nbsp;";
        lblSub.Text = moStudentBL.Category != null && moStudentBL.Category != "" ? moStudentBL.Category : "&nbsp;";
        lblUDISENO.Text = moStudentBL.UDISENumber != null && moStudentBL.UDISENumber != "" ? moStudentBL.UDISENumber : "&nbsp;";
        lblBirthPlace.Text = moStudentBL.BirthPlace != null && moStudentBL.BirthPlace != "" ? moStudentBL.BirthPlace : "&nbsp;";
        lblNationality.Text = moStudentBL.Nationality != null && moStudentBL.Nationality != "" ? moStudentBL.Nationality : "&nbsp;";
        lblMotherTongue.Text = moStudentBL.MotherTongue != null && moStudentBL.MotherTongue != "" ? moStudentBL.MotherTongue : "&nbsp;";
        lblBG.Text = moStudentBL.BloodGroup != null && moStudentBL.BloodGroup != "" ? moStudentBL.BloodGroup : "&nbsp;";
        string sFile = ".." + Constants.S_UPLOAD_FAMILY_PHOTO_IMAGE_PATH + "\\" + moStudentBL.Family_Photo_Copy_Path;
        string sServerFilePath = Server.MapPath("..") + Constants.S_UPLOAD_FAMILY_PHOTO_IMAGE_PATH + moStudentBL.Family_Photo_Copy_Path;
        string sFamilyPhotoPath = S_Family_Path + moStudentBL.Family_Photo_Copy_Path;
        if (File.Exists(sServerFilePath))
        {
            imgFamilyPhoto.ImageUrl = sFile;            
            imgFamilyPhoto.Attributes.Add("Onclick", "OpenFamilyPhoto('" + sFamilyPhotoPath + "');return false;");
        }
        else
            imgFamilyPhoto.Visible = false;
    }
    #endregion

}