using System;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using System.Collections.Generic;

public partial class TimeTable : SchoolBase
{
    #region Event

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {			
            SetJavaScriptAtributes();
            DataSet oDataSet = GetLectureTiming();
            CreateTable(oDataSet);
        }
        catch (Exception ex)
        {
			BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region MEthods

    /// <summary>
    /// This method is used to get lecture timing dataset.
    /// </summary>
    /// <returns></returns>
    private DataSet GetLectureTiming()
    {
        DataSet oDataSet = null;
        if (moUserRole == Constants.UserRoles.Admin
            || moUserRole == Constants.UserRoles.Supervisor)
            oDataSet = MasterDataCollectionBL.GetAllLectureLimings(miSchoolId, miAcademicYearId, 0);
		else if (moUserRole == Constants.UserRoles.Student || moUserRole == Constants.UserRoles.Teacher)
			oDataSet = MasterDataCollectionBL.GetAllLectureLimings(miSchoolId, miAcademicYearId, Convert.ToInt32(Session[Constants.S_SESSION_USER_STD_SECTION].ToString()));
        return oDataSet;
    }


    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavaScriptAtributes()
    {
        btnClose.Attributes.Add("onclick", "Closewindow()");
		ApplyMouseHoverEffect(new List<Button>() { btnClose });
    }

    private void CreateTable(DataSet aoDS)
    {
        DataTable oDTSections = aoDS.Tables[1];
        DataTable oDTLecs = aoDS.Tables[0];
        int iSectionNo = 1;
        int iTotalSections = oDTSections.Rows.Count;

        if (moUserRole == Constants.UserRoles.Student || moUserRole == Constants.UserRoles.Teacher)
        {
            int iSection = Convert.ToInt32(Session[Constants.S_SESSION_USER_STD_SECTION].ToString());

            DataRow[] oDTRow = oDTSections.Select("section<>" + iSection);
            for (int iCount = 0; iCount < oDTRow.Length; iCount++)
            {
                oDTRow[0].Delete();
                oDTSections.AcceptChanges();
            }
        }

        foreach (DataRow oSRow in oDTSections.Rows)
        {
            int iSection = Convert.ToInt32(oSRow["section"]);
            DataRow[] oArrLects = oDTLecs.Select("section = " + iSection.ToString());

            TableRow oRow = new TableRow();
            TableCell oCell = new TableCell();

            // Add section header only if there are multiple sections
            if (iTotalSections != 1)
            {
                oRow = new TableRow();
                oCell = new TableCell();
                oCell.Text = "Section " + iSectionNo.ToString();
                oCell.CssClass = "ColorBg";
                oCell.ColumnSpan = 2;
                oRow.Cells.Add(oCell);
            }

            int iLecCount = 1;

            oRow = new TableRow();
            oCell = new TableCell();
            oCell.Text = "Lectures";
            oCell.CssClass = "ColorBg";
            oRow.Cells.Add(oCell);

            oCell = new TableCell();
            oCell.Text = "Timings";
            oCell.CssClass = "ColorBg";
            oRow.Cells.Add(oCell);
            tblTimings.Rows.Add(oRow);

            foreach (DataRow oLRow in oArrLects)
            {
                oRow = new TableRow();
                oCell = new TableCell();
                //if (oLRow["Description"] == DBNull.Value)
                if (Convert.ToInt32(oLRow["Lecture_No"]) != Constants.I_ZERO)
                {
                    oCell.Text = "Lecture " + iLecCount;
                    oCell.CssClass = "dataBG TxtNormal paddingL";
                    oRow.Cells.Add(oCell);

                    oCell = new TableCell();
                    oCell.Text = oLRow["Start_Time"].ToString().Substring(12) + " - " + oLRow["End_Time"].ToString().Substring(12);
                    oCell.CssClass = "dataBG paddingL";
                    oRow.Cells.Add(oCell);
                    iLecCount++;
                }
                else
                {
                    oCell.Text = oLRow["description"].ToString() + " - " + oLRow["Start_Time"].ToString().Substring(12) + " - " + oLRow["End_Time"].ToString().Substring(12);
                    oCell.ColumnSpan = 2;
                    oCell.CssClass = "ClsGridRow paddingL ClsConfigText";
                    oRow.Cells.Add(oCell);
                }
                tblTimings.Rows.Add(oRow);
            }
            iSectionNo++;
        }
    } 

   #endregion  
}
