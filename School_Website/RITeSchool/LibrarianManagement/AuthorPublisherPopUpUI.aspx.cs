using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class AuthorPublisherUI : SchoolBase
{
    #region " Constants "

    const string S_DUPLICATE_AUTHOR = "Author Name is already exist";
    const string S_DUPLICATE_PUBLISHER = "Publisher Name is already exist";

    const string S_AUTHOR_NAME = "Author";
    const string S_PUBLISHER_NAME = "Publisher";

    const string S_AUTHOR_LABLE_NAME = "Author Name";
    const string S_PUBLISHER_LABLE_NAME = "Publisher Name";

    const string S_AUTHOR_DATA_FIELD = "Author_Name";
    const string S_PUBLISHER_DATA_FIELD = "Publisher_Name";

    #endregion

    #region Data Members
    
	string msBookAuthority;
    
	#endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
	        if (!IsPostBack)
		        FillDefaultGridFormat();

	        lblMessage.Visible = false;
            btnNew.Attributes.Add("onclick", "if(!ClearText()) {return false;}");
			ApplyMouseHoverEffect(new List<Button> { btnAdd, btnClose, btnNew });
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    private void FillDefaultGridFormat()
    {
		msBookAuthority = QueryString["AuthorOrPublisher"];
        SetDefaultProperty(msBookAuthority);        
    }

    private void SetDefaultProperty(string msBookAuthority)
    {
        switch (msBookAuthority)
        {
            case S_AUTHOR_NAME:
                {
                    BoundField oName = (BoundField)grdAuthorPublisher.Columns[Constants.I_ZERO];
                    oName.HtmlEncode = false;
                    oName.HeaderText = "Author Name";
                    oName.DataField = S_AUTHOR_DATA_FIELD;
                    lblName.Text = S_AUTHOR_LABLE_NAME;
                    FillAuthorGridView();
                }
                break;
            /*CategoryBL oCategoryBL = InitialiseMembers();
            RetriveCategoryList();*/

            case S_PUBLISHER_NAME:
                {
                    BoundField oName = (BoundField)grdAuthorPublisher.Columns[Constants.I_ZERO];
                    oName.HtmlEncode = false;
                    oName.HeaderText = "Publisher Name";
                    oName.DataField = S_PUBLISHER_DATA_FIELD;
                    lblName.Text = S_PUBLISHER_LABLE_NAME;
                    FillPublisherGridView();
                }
                break;
        }
    }

    private void FillPublisherGridView()
    {
        
    }

    private void FillAuthorGridView()
    {
        AuthorBL oAuthorBL = new AuthorBL();
//        oAuthorBL.FetchAuthorDetails();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {

    }
    protected void grdAuthorPublisher_RowDataBound(object sender, GridViewRowEventArgs e)
    {
       
    }
}
