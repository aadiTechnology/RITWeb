using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;
using BusinessLogic.Exceptions;

public partial class MathFormula : SchoolBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetJavascriptAttributes();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    protected void btnDisplay_Click(object sender, EventArgs e)
    {
        try
        {
            trActualFormula.Visible = true;
            lblForumla.Text = txtFormula.Text;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnDisplay });
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }
}