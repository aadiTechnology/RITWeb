// File Name     : SupportDetailsUI.aspx.cs
// Modified By   : Ashish 
// Modified Date : 18/10/2013
// Description   : This class is used to view support details incuding attachement .
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;

public partial class SupportDetailsUI : SchoolBase
{
    #region Constants

    private SupportBL moSupportBL;
    
    #endregion Constants

    #region "Events"
   
    /// <summary>
    /// This event use to fill list view and set javascript event to controls 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try 
        {
            moSupportBL = new SupportBL(miSchoolId, miAcademicYearId);
            if (!IsPostBack) 
            {
                InitializeFields();
                FillSupportDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        
    }
   
    /// <summary>
    /// This event use to handle UpdateCommand event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSupportDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            int iSupportId = Convert.ToInt32(lstvwSupportDetails.DataKeys[((ListViewDataItem)e.Item).DisplayIndex]["Id"]);
            if (e.CommandName == Constants.S_COMMAND_UPDATE)
            {
                FillSupportDetails(iSupportId);
                SetAttachment();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    #endregion "Events"

    #region "Private Method"
   
    /// <summary>
    /// This method use to set controls to value that store in database
    /// </summary>
    /// <param name="aiSupportDetails"></param>
    private void FillSupportDetails(int aiSupportDetails)
    {
        SupportDetails oSupportDetail = moSupportBL.Get(aiSupportDetails);
        txtSubject.Text = oSupportDetail.Subject;
        lblMobileNumber.Text = oSupportDetail.MobileNo;
        lblEmailAddress.Text = oSupportDetail.EmailAddress;
        txtDescription.Text = oSupportDetail.Description;
        hidFileName.Value = btnDownload.Text = oSupportDetail.FileName;
    }
    
    /// <summary>
    /// This method use to view attachement
    /// </summary>
    private void SetAttachment()
    {
        if (!string.IsNullOrEmpty(hidFileName.Value))
        {
            btnDownload.Visible = true;
            attachement.Visible = true;
            btnDownload.Text = hidFileName.Value;
            string sServerPath = Server.MapPath("..");
            if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
                sServerPath = sServerPath + "\\";
            string sDestination = sServerPath + "Uploads\\Support\\" + hidFileName.Value;
            if (File.Exists(sDestination))
                btnDownload.Attributes.Add("onclick", "window.open('" + Constants.S_SUPPORT_FOLDER_LOCATION_URL + "" + hidFileName.Value + "','_blank'); return false;");
        }
        else
            attachement.Visible = false;
    }
    
    /// <summary>
    /// This method is used to initialize control values.
    /// </summary>
    private void InitializeFields()
    {
        lblEmailAddress.Text = " - ";
        lblMobileNumber.Text = " - ";
        btnDownload.Text = " - "; 
    }
   
    /// <summary>
    /// This method use to fill list view with support details
    /// </summary>
    private void FillSupportDetails()
    {
        List<SupportDetails> lstSupportDetails = moSupportBL.GetAll();
        lstvwSupportDetails.DataSource = lstSupportDetails;
        lstvwSupportDetails.DataBind();
    }
    #endregion "Private Method"
}