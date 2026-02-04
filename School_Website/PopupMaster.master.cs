using System;

public partial class PopUpMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        hidServerDate.Value = Convert.ToString(DateTime.Now.Date.Year);
    }
}
