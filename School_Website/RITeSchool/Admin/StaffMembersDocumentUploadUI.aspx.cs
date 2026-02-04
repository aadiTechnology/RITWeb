using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using MasterEntities;
using SchoolEntities;
using Utility;

/// <summary>
/// This method is used to save users education, experience details and upload users document.
/// </summary>
public partial class StaffMembersDocumentUploadUI : SchoolBase
{
	#region "Data Members"

	private UserDetailsBL moUserDetailsBL; 

	#endregion

	#region "Events"
	protected void Page_Load(object sender, EventArgs e)
    {
		try
		{
			moUserDetailsBL = new UserDetailsBL(miSchoolId, miAcademicYearId, miUserId);
			if (!IsPostBack)
			{
				SetDefaultValues();
				FillUserRoleCombobox();
				FillQualificationCombobox();
				FillPassingClassCombobox();
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
    }

	/// <summary>
	/// This event is used to get details for update and delete the record.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwExpDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
	{
		try
		{
			ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
			int iExpDetailsId = Convert.ToInt32(lstvwExpDetails.DataKeys[oCurrentItem.DisplayIndex]["Id"].ToString());
			if (e.CommandName == Constants.S_COMMAND_UPDATE)
			{
				UserExperienceDetails oUserExperienceDetails = moUserDetailsBL.GetExperienceDetails(iExpDetailsId, cmbUsers.SelectedValue.ToInt());
				txtSchoolname.Text = oUserExperienceDetails.SchoolName;
				txtLeftDate.Text = oUserExperienceDetails.LeftDate.ToString(Constants.S_DATE_FORMAT);
				txtjoinedDate.Text = oUserExperienceDetails.JoiningDate.ToString(Constants.S_DATE_FORMAT);
				hidExpDetailsId.Value = oUserExperienceDetails.Id.ToString();
				lblDuplicateDetails.Visible = false;
			}
			else if (e.CommandName == Constants.S_COMMAND_REMOVE)
			{
				moUserDetailsBL.DeleteExperienceDetails(iExpDetailsId, cmbUsers.SelectedValue.ToInt());
				FillExperiencListview();
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This method is used to set pop up url for attachment link.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwExpDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
				int iEducationDetailsId = Convert.ToInt32(lstvwExpDetails.DataKeys[oCurrentItem.DisplayIndex]["Id"].ToString());
				LinkButton lnkAttachmentCnt = oCurrentItem.FindControl("lnkAttachmentCnt") as LinkButton;
				ImageButton oImgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
				oImgbtnDelete.Attributes.Add("onclick", "if(!DeleteExpDetails()) {return false;}");

				string sQueryString = "UserId=" + cmbUsers.SelectedValue.ToInt() +
									  "&DocumentId=" + iEducationDetailsId +
									"&DocumentTypeId=" + Constants.DocumentTypes.ExperienceCertificate.ToInt();

				sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
				lnkAttachmentCnt.Attributes.Add("onclick", "OpenPopup('" + sQueryString + "'); return false;");
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to get details for update and delete the record.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwEducationDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
	{
		try
		{
			ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
			int iEducationDetailsId = Convert.ToInt32(lstvwEducationDetails.DataKeys[oCurrentItem.DisplayIndex]["Id"].ToString());
			if (e.CommandName == Constants.S_COMMAND_UPDATE)
			{
				UserEducationDetails oUserEducationDetails = moUserDetailsBL.GetEducationDetails(iEducationDetailsId, cmbUsers.SelectedValue.ToInt());
				txtYearOfPassing.Text = oUserEducationDetails.YearOfPassing;
				txtPassingUniversity.Text = oUserEducationDetails.University;
				cmbQualification.SelectedValue = oUserEducationDetails.Qualification.Id.ToString();
				cmbPassingClass.SelectedValue = oUserEducationDetails.PassClassId.ToString();
				hidEducationId.Value = iEducationDetailsId.ToString();
				lblDuplicateDetails.Visible = false;
			}
			else if (e.CommandName == Constants.S_COMMAND_REMOVE)
			{
				moUserDetailsBL.DeleteEducationDetails(iEducationDetailsId, cmbUsers.SelectedValue.ToInt());
				FillEducationListview();
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to set popup url to attachment link.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwEducationDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
				int iExpDetailsId = Convert.ToInt32(lstvwEducationDetails.DataKeys[oCurrentItem.DisplayIndex]["Id"].ToString());
				LinkButton oLinkButton = oCurrentItem.FindControl("lnkAttachmentCnt") as LinkButton;

				ImageButton oImgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
				oImgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");

				string sQueryString = "UserId=" + cmbUsers.SelectedValue.ToInt() +
									  "&DocumentId=" + iExpDetailsId +
									"&DocumentTypeId=" + Constants.DocumentTypes.EducationCertificate.ToInt();

				sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
				oLinkButton.Attributes.Add("onclick", "OpenPopup('" + sQueryString + "'); return false;");
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to set url to attachment pop up.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwUserDocuments_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
				int iDocumentId = Convert.ToInt32(lstvwUserDocuments.DataKeys[oCurrentItem.DisplayIndex]["Id"]);
				int iDocumentTypeId = Convert.ToInt32(lstvwUserDocuments.DataKeys[oCurrentItem.DisplayIndex]["DocumentTypeId"].ToString());
				LinkButton oLinkButton = oCurrentItem.FindControl("lnkDocumentCount") as LinkButton;
				string sQueryString = "UserId=" + cmbUsers.SelectedValue.ToInt() +
					  "&DocumentId=" + iDocumentId +
					"&DocumentTypeId=" + iDocumentTypeId;

				sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
				oLinkButton.Attributes.Add("onclick", "OpenPopup('" + sQueryString + "'); return false;");
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to update investment declaration listview.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void HidItemCount_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			int iDocumentId = 0;
			const int I_DOCUMENT_COUNT = 0;
			const int I_DOCUMENT_ID = 1;
			const int I_USER_ID = 2;
			const int I_DOC_TYPE_ID = 3;

			string[] sArrayIds = hidItemCount.Value.Split('$');
			if (sArrayIds[I_DOCUMENT_COUNT] != string.Empty && sArrayIds[I_USER_ID] == cmbUsers.SelectedValue)
			{
				switch (sArrayIds[I_DOC_TYPE_ID])
				{
					// Update education certificate attachment count
					case "4": foreach (ListViewDataItem oCurrentItem in lstvwEducationDetails.Items)
						{
							iDocumentId = Convert.ToInt32(lstvwEducationDetails.DataKeys[oCurrentItem.DisplayIndex]["Id"]);
							if (iDocumentId == sArrayIds[I_DOCUMENT_ID].ToInt())
							{
								LinkButton lnkAttachment = oCurrentItem.FindControl("lnkAttachmentCnt") as LinkButton;
								lnkAttachment.Text = sArrayIds[I_DOCUMENT_COUNT];
							}
						}
						break;
					// Update experience certificate attachment count
					case "5": foreach (ListViewDataItem oCurrentItem in lstvwExpDetails.Items)
								{
									iDocumentId = Convert.ToInt32(lstvwExpDetails.DataKeys[oCurrentItem.DisplayIndex]["Id"]);
									if (iDocumentId == sArrayIds[I_DOCUMENT_ID].ToInt())
									{
										LinkButton lnkAttachment = oCurrentItem.FindControl("lnkAttachmentCnt") as LinkButton;
										lnkAttachment.Text = sArrayIds[I_DOCUMENT_COUNT];
									}
								}
						break;
					// Update Other document attachment count
					 default: foreach (ListViewDataItem oCurrentItem in lstvwUserDocuments.Items)
							{
								iDocumentId = Convert.ToInt32(lstvwUserDocuments.DataKeys[oCurrentItem.DisplayIndex]["Id"]);
								if (iDocumentId == sArrayIds[I_DOCUMENT_ID].ToInt())
								{
									LinkButton lnkAttachment = oCurrentItem.FindControl("lnkDocumentCount") as LinkButton;
									lnkAttachment.Text = sArrayIds[I_DOCUMENT_COUNT];
								}
							}
						break;
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to save experience details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			UserExperienceDetails oUserExperienceDetails = new UserExperienceDetails()
			{
				Id = hidExpDetailsId.Value.ToInt(),
				Organization = txtSchoolname.Text.Trim(),
				LeftDate = txtLeftDate.Text.ToDateTime(),
				JoiningDate = txtjoinedDate.Text.ToDateTime(),
				UserId = cmbUsers.SelectedValue.ToInt()
			};
			moUserDetailsBL.SaveExperienceDetails(oUserExperienceDetails);
			FillExperiencListview();
			ClearExperienceControls();
		}
		catch (SqlException ex)
		{
			lblDuplicateDetails.Visible = true;
			lblDuplicateDetails.Text = ex.Message;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to save education details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnAddDetails_Click(object sender, EventArgs e)
	{
		try
		{
			UserEducationDetails oUserEducationDetails = new UserEducationDetails()
			{
				UserId = cmbUsers.SelectedValue.ToInt(),
				Qualification = new Qualification { Id = cmbQualification.SelectedValue.ToInt() },
				PassClassId = cmbPassingClass.SelectedValue.ToInt(),
				University = txtPassingUniversity.Text,
				YearOfPassing = txtYearOfPassing.Text,
				Id = hidEducationId.Value == string.Empty ? Constants.I_ZERO : hidEducationId.Value.ToInt()
			};
			moUserDetailsBL.SaveEducationDetails(oUserEducationDetails);
			FillEducationListview();
			ClearEducationControls();
		}
		catch (SqlException ex)
		{
			lblDuplicateDetails.Visible = true;
			lblDuplicateDetails.Text = ex.Message;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to fill user combobox.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void cmbUserRole_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			FillUserCombobox();
			FillEducationListview();
			FillExperiencListview();
			FillUserDocumentListview();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to fill experience, education and document details listview.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void cmbUsers_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			hidEducationId.Value = hidExpDetailsId.Value = Constants.S_ZERO;
			FillEducationListview();
			FillExperiencListview();
			FillUserDocumentListview();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion

	#region "Private Method"
	/// <summary>
	/// This method is used to clear experience details related controls.
	/// </summary>
	private void ClearExperienceControls()
	{
		txtSchoolname.Text = txtLeftDate.Text = txtjoinedDate.Text = string.Empty;
		hidExpDetailsId.Value = Constants.S_ZERO;
		lblDuplicateDetails.Visible = false;
	}	

	/// <summary>
	/// This method is used to clear education details related controls.
	/// </summary>
	private void ClearEducationControls()
	{
		cmbQualification.ClearSelection();
		cmbPassingClass.ClearSelection();
		txtPassingUniversity.Text = txtYearOfPassing.Text = string.Empty;
		hidEducationId.Value = Constants.S_ZERO;
	}	

	/// <summary>
	/// This method is used to fill users combobox according to selected user role.
	/// </summary>
	private void FillUserCombobox()
	{
		SchoolUserCollectionBL oSchoolUserBL = new SchoolUserCollectionBL();
        //DataTable oDtUsers = oSchoolUserBL.GetUserDetails(miSchoolId, cmbUserRole.SelectedValue.ToInt(), miAcademicYearId, "", "", "", 500, 0);
        //ListSource.FillDropDownList(oDtUsers, cmbUsers, "Name", "User_Id", Constants.S_SELECT);
	}

	/// <summary>
	/// This method is sued to fill applicable user documents listview
	/// </summary>
	private void FillUserDocumentListview()
	{
		List<UserDocument> lstUserDocuments = moUserDetailsBL.GetApplicableDocumentList(cmbUsers.SelectedValue.ToInt());
		lstvwUserDocuments.DataSource = lstUserDocuments;
		lstvwUserDocuments.DataBind();
	}

	/// <summary>
	/// This method is used to fill experience details of users.
	/// </summary>
	private void FillExperiencListview()
	{
		List<UserExperienceDetails> lstExperienceDatials = moUserDetailsBL.GetExperienceDetailsList(cmbUsers.SelectedValue.ToInt());
		lstvwExpDetails.DataSource = lstExperienceDatials;
		lstvwExpDetails.DataBind();
	}

	/// <summary>
	/// This method is used to fill educational details of user.
	/// </summary>
	private void FillEducationListview()
	{
		List<UserEducationDetails> lstEducationDetails = moUserDetailsBL.GetEducationDetailsList(cmbUsers.SelectedValue.ToInt());
		lstvwEducationDetails.DataSource = lstEducationDetails;
		lstvwEducationDetails.DataBind();
	}

	/// <summary>
	/// This method is used to passing class combobox.
	/// </summary>
	private void FillPassingClassCombobox()
	{
		MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
		DataTable oDtClasses = MasterDataCollectionBL.GetListOfClassType();
		ListSource.FillDropDownList(oDtClasses, cmbPassingClass, "Class_Name", "Class_Id", Constants.S_SELECT);
	}

	/// <summary>
	/// This method is used to fill Qualification combobox.
	/// </summary>
	private void FillQualificationCombobox()
	{
		List<Qualification> lstQualifications = MasterDataCollectionBL.GetAllQualification();
		ListSource.FillDropDownList(lstQualifications, cmbQualification, "Name", "Id", Constants.S_SELECT);
	}

	/// <summary>
	/// This method is used fill user role combobox.
	/// </summary>
	private void FillUserRoleCombobox()
	{
		MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
		DataTable oDtUserRole = oMasterDataCollectionBL.GetAllUserRoles().Select("User_Role_Id<>" + Constants.UserRoles.Parent.ToInt() + "AND User_Role_Id<>" + Constants.UserRoles.Student.ToInt()).CopyToDataTable();
		ListSource.FillDropDownList(oDtUserRole, cmbUserRole, Constants.S_USER_ROLE_NAME_FIELD, Constants.S_USER_ROLE_ID_FIELD, Constants.S_SELECT);
	}

	/// <summary>
	/// This method is used to set default values to hiddent variables and apply hover effect to buttons.
	/// </summary>
	private void SetDefaultValues()
	{
		ApplyMouseHoverEffect(new List<Button>() { btnAdd, btnAddDetails, btnCancel, btnCancelDetails});
		cmbUsers.Items.Add(new ListItem() { Text = Constants.S_SELECT, Value = Constants.S_ZERO });
		hidEducationId.Value = hidExpDetailsId.Value = Constants.S_ZERO;
		cmbUserRole.Focus();
	}

	#endregion
}
