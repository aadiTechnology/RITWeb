using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Linq;
using PushNotificationService;

public partial class TravelerSmsPopup : SchoolBase
{

	#region -- MEMBER(s) --

	private DataTable moDTTravlers;    
    
	#endregion -- MEMBER(s) --

	#region -- EVENT HANDLER(s) --

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			if (!IsPostBack)
			{
				ReadQuerystring();
				FillTravelersList();
				SetJavascriptAttributes();
				valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;

			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	protected void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			Response.Write("<Script language='Javascript'>window.close();</Script>");
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	protected void btnBack_Click(object sender, EventArgs e)
	{
		try
		{
			Response.Write("<Script language='Javascript'>window.close();</Script>");
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	protected void btnSendSms_Click(object sender, EventArgs e)
	{
		try
		{
			lblSuccessMsg.Text = "";

            if (rbSMS.Checked) 
            {
                SendSMS(moDTTravlers);
                lblSuccessMsg.Text = "SMS Sent Successfully !!!";
            }
            else if (rbMessage.Checked)   
            {
                SendMessage();
                lblSuccessMsg.Text = "Message Sent Successfully !!!";
            }

            lblSuccessMsg.Visible = true;
		    txtReason.Text = "";
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

   protected void lstvwTravelersDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				if (Convert.ToInt32(hidUserRoleId.Value) == Convert.ToInt32(Constants.UserRoles.Student))
				{
					HtmlTableCell oHtmlTableCell = ((HtmlTableCell)e.Item.FindControl("tdMobileNo"));
					oHtmlTableCell.Visible = false;
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

	private void FillTravelersList()
	{
		moDTTravlers = TravelerTransportDetailsBL.GetTravelersDetails(miSchoolId, miAcademicYearId, Convert.ToInt32(hidUserRoleId.Value), Convert.ToInt32(hidRouteId.Value), Convert.ToInt32(hidStopId.Value), Convert.ToInt32(hidShiftId.Value), Convert.ToInt32(hidStandardId.Value), Convert.ToInt32(hidDivisionId.Value));

		if (moDTTravlers != null && moDTTravlers.Rows.Count > 0)
		{
			trSMS.Visible = true;
			btnBack.Visible = false;
			btnCancel.Visible = true;
			btnSendSms.Visible = true;
			divContainer.Visible = true;
			trNoRecordMsg.Visible = false;
			ViewState.Add("oDTTravlers", moDTTravlers);
         }
		else
		{
			trSMS.Visible = false;
			trNoRecordMsg.Visible = true;
			btnBack.Visible = true;
			btnCancel.Visible = false;
			btnSendSms.Visible = false;
			divContainer.Visible = false;
		}

        moDTTravlers = moDTTravlers.AsEnumerable().GroupBy(row => new
            {
                Name = row.Field<string>("Name"),
                UserId = row.Field<int>("UserId"),
                MobileNo = row.Field<string>("MobileNo"),
                MobileNo2 = row.Field<string>("MobileNo2")
               })
            .Select(g => g.First()) 
            .CopyToDataTable();    
             
        lstvwTravelersDetails.DataSource = moDTTravlers;
		lstvwTravelersDetails.DataBind();
		if (moDTTravlers != null && moDTTravlers.Rows.Count > 0)
		{
			if (Convert.ToInt32(hidUserRoleId.Value) == Convert.ToInt32(Constants.UserRoles.Student))
			{
				HtmlTable oHtmltblTravlerInfo = ((HtmlTable)lstvwTravelersDetails.FindControl("tblTravlerInfo"));
				HtmlTableRow oHTMLtrHeader = ((HtmlTableRow)oHtmltblTravlerInfo.FindControl("trHeader"));
				HtmlTableCell oHTMLthMobileNo = ((HtmlTableCell)oHTMLtrHeader.FindControl("thMobileNo"));
				oHTMLthMobileNo.Visible = false;
			}
		}
	}

	private void SendSMS(DataTable moDTTravlers)
	{
		moDTTravlers = (DataTable)ViewState["oDTTravlers"];
		string sSalarySMSText = string.Empty;
		string sSmsSubject = string.Empty;

		if (moDTTravlers != null && moDTTravlers.Rows.Count > 0 && moDTTravlers.Rows[0][0] != DBNull.Value)
		{
            SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
			string sSMSSenderName = oSchoolBL.SMSSenderName;
			int iUserId;
			string sMobileNo;
			string sMobileNo2;
			string sTransportSMS = string.Empty;
			string sDisplayText = string.Empty;

            moDTTravlers = moDTTravlers.AsEnumerable()
              .GroupBy(row => new
              {
                  Name = row.Field<string>("Name"),
                  UserId = row.Field<int>("UserId"),
                  MobileNo = row.Field<string>("MobileNo"),
                  MobileNo2 = row.Field<string>("MobileNo2")
              })
            .Select(g => g.First())
            .CopyToDataTable();

          foreach (DataRow oDR in moDTTravlers.Rows)
			{
				iUserId = Convert.ToInt32(oDR["UserId"]);
				sDisplayText = Convert.ToString(oDR["Name"]);
				sMobileNo = Convert.ToString(oDR["MobileNo"]);
				sMobileNo2 = Convert.ToString(oDR["MobileNo2"]);
				sTransportSMS = txtReason.Text;
				SMS oSMS = new SMS();
				oSMS.InsertedByID = -9999;
				oSMS.Sender = oSchoolBL.SMSSenderName;
				oSMS.School_Name = oSchoolBL.SchoolName + "::" + "Transport";
				oSMS.SMSText = sTransportSMS;
				oSMS.AcademicYearID = miAcademicYearId;
				oSMS.SchoolID = miSchoolId;
				oSMS.DisplayText = sDisplayText;
				oSMS.To.Add(iUserId, sMobileNo);
				if (sMobileNo2 != string.Empty)
					oSMS.To.Add(iUserId + "sm;", sMobileNo2);
				oSMS.Send();
             }
		}
	}

    private void SendMessage()
    {
        DataTable oDTTravlers = (DataTable)ViewState["oDTTravlers"];        
        if (oDTTravlers != null && oDTTravlers.Rows.Count > 0 && oDTTravlers.Rows[0][0] != DBNull.Value)
        {   
            oDTTravlers = oDTTravlers.AsEnumerable()
              .GroupBy(row => new
               {
                   Name = row.Field<string>("Name"),
                   UserId = row.Field<int>("UserId"),
                   MobileNo = row.Field<string>("MobileNo"),
                   MobileNo2 = row.Field<string>("MobileNo2")
               })
            .Select(g => g.First())
            .CopyToDataTable();

            MessageDetailsBL oMessageDetailsBL = new MessageDetailsBL();
            oMessageDetailsBL.Subject = "Important message from Transport Department";
            oMessageDetailsBL.Message_Body = "Dear Parent,<BR></BR> Here is message from transport department - <BR></BR>" + txtReason.Text.Trim() + "<BR></BR>Thanks and regards,<p>" + Session[Constants.S_SESSION_USER_FULLNAME] + "</p>";
            oMessageDetailsBL.Display_Text = "";
            oMessageDetailsBL.Cc_Display_Text = "";
            oMessageDetailsBL.Inserted_By_Id = miUserId;
            oMessageDetailsBL.Is_Deleted = "N";
            oMessageDetailsBL.Is_DeletedFromUser = "N";
            oMessageDetailsBL.Sender_User_Id = miUserId;
            oMessageDetailsBL.Sender_User_Role_Id = Convert.ToInt32(moUserRole);
            oMessageDetailsBL.Updated_By_Id = miUserId;
            oMessageDetailsBL.AcademicYrId = miAcademicYearId;
            oMessageDetailsBL.RequestReadReceipt = false;
            oMessageDetailsBL.Insert_Date = DateTime.Now;
        
            List<MessageReceiverDetailsBL> MessageReceiverDetailsBLList = new List<MessageReceiverDetailsBL>();
            MessageReceiverDetailsBL oMessageReceiverDetailsBL;
            foreach (DataRow oDR in oDTTravlers.Rows)
            {                
                oMessageReceiverDetailsBL = new MessageReceiverDetailsBL();
                oMessageReceiverDetailsBL.New_Message_Flag = "Y";
                oMessageReceiverDetailsBL.Message_Details_Id = 0;
                oMessageReceiverDetailsBL.Read_Message_Flag = "N";
                oMessageReceiverDetailsBL.Receiver_User_Id = Convert.ToInt32(oDR["UserId"]);
                oMessageReceiverDetailsBL.Receiver_User_Role_Id = 3;
                oMessageReceiverDetailsBL.Updated_By_Id = miUserId;
                oMessageReceiverDetailsBL.Inserted_By_Id = miUserId;
                oMessageReceiverDetailsBL.Is_Archive = "N";
                oMessageReceiverDetailsBL.Is_Deleted = "N";
                oMessageReceiverDetailsBL.Is_DeletedFromReceiver = "N";
                oMessageReceiverDetailsBL.IsCc = 0;
                oMessageReceiverDetailsBL.Insert_Date = DateTime.Now;
                oMessageReceiverDetailsBL.IsForwardReply = "N";
                MessageReceiverDetailsBLList.Add(oMessageReceiverDetailsBL);
            }

            if (MessageReceiverDetailsBLList.Count > 0)
            {
                oMessageDetailsBL.InsertMessageDetails(MessageReceiverDetailsBLList, new List<string>());
                string userIds = string.Join(";", oDTTravlers.AsEnumerable().Select(r => r["UserId"].ToString()));
                SendPushNotification(userIds);
            }
        }       
    }

    public void SendPushNotification(string sUserIds)
       {
           if (sUserIds != string.Empty)
           {
               PushNotificationClient pushNotificationClient = null;
               try
               {
                   pushNotificationClient = new PushNotificationClient();
                    string[] strArrayUserid = sUserIds.Split(';');
                    int[] intArrayUserId = Array.ConvertAll(strArrayUserid, userId => int.Parse(userId));
                   Dictionary<string, string> dictionaryNotificationParameter = new Dictionary<string, string>();
                   dictionaryNotificationParameter.Add(Constants.S_NOTIFICATION_PARAMETER_FULLNAME, Session[Constants.S_SESSION_USER_FULLNAME].ToString());
                   dictionaryNotificationParameter.Add(Constants.S_NOTIFICATION_PARAMETER_MESSAGE_SUBJECT, "Important message from Transport Department");
                   pushNotificationClient.SendNotification(NotificationMessageHeadings.NewMessageArrived, this.miSchoolId.ToString(), intArrayUserId, dictionaryNotificationParameter);
                   pushNotificationClient.Close();
               }
               catch (Exception ex)
               {
                   ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
               }
               finally
               {
                   if (pushNotificationClient.State != System.ServiceModel.CommunicationState.Faulted)
                       pushNotificationClient.Close();
               }
           }
       }

    private void ReadQuerystring()
	{
		if (QueryString.Count <= 0)
			return;

		if (QueryString["UserRoleId"] != null)
			hidUserRoleId.Value = QueryString["UserRoleId"];
		if (QueryString["RouteId"] != null)
			hidRouteId.Value = QueryString["RouteId"];
		if (QueryString["StopId"] != null)
			hidStopId.Value = QueryString["StopId"];
		if (QueryString["TransportShiftId"] != null)
			hidShiftId.Value = QueryString["TransportShiftId"];
		if (QueryString["StandardId"] != null)
			hidStandardId.Value = QueryString["StandardId"] == String.Empty ? "0" : QueryString["StandardId"];
		if (QueryString["DivisionId"] != null)
			hidDivisionId.Value = QueryString["DivisionId"] == String.Empty ? "0" : QueryString["DivisionId"];
	}

	/// <summary>
	/// This method is used to set javascript attributes.
	/// </summary>
	private void SetJavascriptAttributes()
	{
		ApplyMouseHoverEffect(new List<Button>
			{
				btnCancel,
				btnSendSms,
				btnBack
			});
		btnSendSms.Attributes["onclick"] = "ResetUpdateLbl()";
	}

	#endregion -- PRIVATE METHOD(s) --   
 }
