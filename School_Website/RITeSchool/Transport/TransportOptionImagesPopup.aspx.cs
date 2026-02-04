/* File Name :- TransportOptionImagesPopup.aspx.cs
 * Created Date :- 10-Apr-2020
 * Class Description :- This class is used to Display Transport Images. 
 * Created By :- Dnyaneshwar Shinde.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Reflection;
using Utility;
using SchoolEntities.Transport;
using BusinessLogic.TransportBL;
using System.IO;

public partial class TransportOptionImagesPopup : SchoolBase
{
    #region Constant's

    private const string S_DELETE_MESSAGE = "Transport Option image deleted successfully !!!";
    private const string S_PUC_FOLDER_PATH = "../downloads/TransportModule/VehiclePUCDetails/";
    private const string S_SERVICING_FOLDER_PATH = "../downloads/TransportModule/VehicleServicingDetails/";
    private const string S_PASSING_FOLDER_PATH = "../downloads/TransportModule/VehiclePassingDetails/"; 

    #endregion

    #region DataMember

    private VehiclePUCDetailsBL moVehiclePUCDetailsBL;

    #endregion

    #region Enum 

    private Constants.TransportOptions CurrentTransportOption
    {
        get
        {
            if (hidTypeId.Value == "1")
                return Constants.TransportOptions.Servicing;
            else if (hidTypeId.Value == "2")
                return Constants.TransportOptions.Passing;
            else
                return Constants.TransportOptions.PUC;
        }
    }

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to load the controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moVehiclePUCDetailsBL = new VehiclePUCDetailsBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                ReadQueryString();
                FillImages();
                btnClose.Attributes.Add("onclick", "ClosePopup(); return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is Used to set values in list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwImages_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton imgTransportPhoto = e.Item.FindControl("imgTransportPhoto") as ImageButton;
                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                string sImageName = Convert.ToString(lstvwImages.DataKeys[e.Item.DataItemIndex]["Images"]);
                string S_FOLDER_PATH = string.Empty;

                if (CurrentTransportOption == Constants.TransportOptions.Servicing)
                    S_FOLDER_PATH = S_SERVICING_FOLDER_PATH;
                else if (CurrentTransportOption == Constants.TransportOptions.Passing)
                    S_FOLDER_PATH = S_PASSING_FOLDER_PATH;
                else if (CurrentTransportOption == Constants.TransportOptions.PUC)
                    S_FOLDER_PATH = S_PUC_FOLDER_PATH;

                string sFileName = S_FOLDER_PATH + sImageName;

                imgTransportPhoto.ImageUrl = sFileName;

                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");                
                imgTransportPhoto.Attributes.Add("onclick", "OpenWindow('" + sFileName + "'); return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is used to list view Item Command.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwImages_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {   
                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    int iDetailId = Convert.ToInt32(lstvwImages.DataKeys[e.Item.DisplayIndex]["DetailId"]);
                    int iTypeId = Convert.ToInt32(lstvwImages.DataKeys[e.Item.DisplayIndex]["TypeId"]);
                    string sFileName = moVehiclePUCDetailsBL.DeleteTransportOptionImage(iDetailId, iTypeId);

                    if (sFileName != string.Empty)
                    {
                        if (CurrentTransportOption == Constants.TransportOptions.Servicing)
                        { 
                            if (File.Exists(Server.MapPath("..") + S_SERVICING_FOLDER_PATH + sFileName))
                                File.Delete(Server.MapPath("..") + S_SERVICING_FOLDER_PATH + sFileName);
                        }
                        else if (CurrentTransportOption == Constants.TransportOptions.Passing)
                        {
                            if (File.Exists(Server.MapPath("..") + S_PASSING_FOLDER_PATH + sFileName))
                                File.Delete(Server.MapPath("..") + S_PASSING_FOLDER_PATH + sFileName);
                        }
                        else if (CurrentTransportOption == Constants.TransportOptions.PUC)
                        {
                            if (File.Exists(Server.MapPath("..") + S_PUC_FOLDER_PATH + sFileName))
                                File.Delete(Server.MapPath("..") + S_PUC_FOLDER_PATH + sFileName);
                        }
                    }

                    FillImages();
                    base.DisplayMessage(S_DELETE_MESSAGE, false, tdMessage);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is used to read query string and set values to hidden fields.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["TypeId"] != null)
        {
            hidTypeId.Value = QueryString["TypeId"];
            if (CurrentTransportOption == Constants.TransportOptions.Servicing)
                lblTypeName.Text = "Servicing";
            else if (CurrentTransportOption == Constants.TransportOptions.Passing)
                lblTypeName.Text = "Passing";
            else if (CurrentTransportOption == Constants.TransportOptions.PUC)
                lblTypeName.Text = "PUC";
        }

        if (QueryString["DetailsId"] != null)
            hidDetailsId.Value = QueryString["DetailsId"];

        if (QueryString["VehicleId"] != null)
            hidVehicleId.Value = QueryString["VehicleId"];
    }

    /// <summary>
    /// This method is used to fill Image list view.
    /// </summary>
    private void FillImages()
    {
        List<TransportOptionImages> olstTransportOptionImages = new List<TransportOptionImages>();
        olstTransportOptionImages = moVehiclePUCDetailsBL.GetTransportOptionImages(hidTypeId.Value.ToInt(), hidDetailsId.Value.ToInt(),hidVehicleId.Value.ToInt(), miSchoolId);

        if (olstTransportOptionImages.Count > Constants.I_ZERO)
            lblVehicleName.Text = olstTransportOptionImages[0].Vehicle;

        hidImageCount.Value = olstTransportOptionImages.Count.ToString();
        lstvwImages.DataSource = olstTransportOptionImages;
        lstvwImages.DataBind();
    }

    #endregion
}