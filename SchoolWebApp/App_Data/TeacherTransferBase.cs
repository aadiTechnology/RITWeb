using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BusinessLogic;
using Utility;

/// <summary>
/// Summary description for TeacherTransferBase.
/// This class is abase class for teacher transfer UI and prevw classes.
/// It contains the common constants for table indices and row states.
/// It also contains the methods to render timetable for both the classes.
/// </summary>
public class TeacherTransferBaseUI : SchoolBase
{
    protected Timetable oTransferTT;
    



    protected TeacherTransferBaseUI()
    {

        //
        // TODO: Add constructor logic here
        //
    }
    
    #region protected methods
  
  
   
    ///// <summary>
    ///// This is a wrapper method to display the preview of timetable.
    ///// It calls base class method to display timetable.
    ///// And the formats it for preview display.
    ///// </summary>
    ///// <param name="aiSrcTeacherId"></param>
    ///// <param name="aiTargetTeacherId"></param>
    //protected void DisplayTTPreview(int aiSrcTeacherId, int aiTargetTeacherId)
    //{
    //    oTransferTT.DisplayTT();
    //    FormatPreVw(aiSrcTeacherId, aiTargetTeacherId);
    //}
    ///// <summary>
    ///// This method formats timetable display by applying Style to each of the timetable cells.
    ///// The style for each cell is chosen as per its rowstate.
    ///// </summary>
    ///// <param name="aiSrcTeacherId"></param>
    ///// <param name="aiTargetTeacherId"></param>
    //private void FormatPreVw(int aiSrcTeacherId, int aiTargetTeacherId)
    //{
    //    HtmlTable oTbl = oTransferTT.tblTT;
    //    int iWeekDayCnt = oTransferTT.moDtWeekday.Rows.Count;
    //    const string S_CSS_ADDED = "ClsHilightBGB";
    //    const string S_CSS_TRANSFERED = "TTNotClassTchr";
    //    const string S_CSS_DELETED = "SubDeleted";
    //    int iTeacherId = 0;
    //    //loop through teachers 
    //    for (int iTeacherIndex = 2; iTeacherIndex < 4; iTeacherIndex++)
    //    {
    //        switch (iTeacherIndex)
    //        {
    //            case 2://row for src teacher
    //                iTeacherId = aiSrcTeacherId;
    //                break;
    //            case 3://row for target teacher
    //                iTeacherId = aiTargetTeacherId;
    //                break;
    //        }
            
    //        int iCellIndex = 0;
    //        //loop through weekdays
    //        for (int iWeekDay = 0; iWeekDay < iWeekDayCnt; iWeekDay++)
    //        {
    //            int iLectureCnt = Convert.ToInt32(oTransferTT.moDtWeekday.Rows[iWeekDay]["LecturesCnt"]);
    //            string sWeekDayId = oTransferTT.moDtWeekday.Rows[iWeekDay]["Weekdays_Id"].ToString();
    //            string sWeekDayName = oTransferTT.moDtWeekday.Rows[iWeekDay]["Weekday_Name"].ToString();
    //            //loop through lectures on the current weekday.
    //            for (int i = 1; i <= iLectureCnt; i++)
    //            {
    //                HtmlTableCell oCell = oTbl.Rows[iTeacherIndex].Cells[iCellIndex];

    //                DataRow[] oDtRows = oTransferTT.moDtTimeTable.Select("Teacher_Id= " + iTeacherId.ToString() + " AND Weekday_Id=" + sWeekDayId + " AND Lecture_Number=" + i.ToString());
    //                string sRowState = RowStates.Original.ToString();//Constants.S_ORIGINAL;
    //                if (oDtRows.Length > 0)
    //                {
    //                    sRowState = oDtRows[0][S_DB_COL_ROWSTATE].ToString();
    //                }
    //                //if src teacher
    //                if (iTeacherId == aiSrcTeacherId)
    //                {
    //                    switch (sRowState)
    //                    {
    //                        case Constants.S_UPDATED: //transferred
    //                            oCell.Attributes.Remove("class");
    //                            oCell.Attributes.Add("class", S_CSS_TRANSFERED);
    //                            break;
    //                        case Constants.S_DELETED: //removed
    //                            oCell.Attributes.Remove("class");
    //                            oCell.Attributes.Add("class", S_CSS_DELETED);
    //                            break;
    //                    }
    //                }
    //                else //target
    //                {
    //                    switch (sRowState)
    //                    {
    //                        case Constants.S_ADDED://transferred from src
    //                            oCell.Attributes.Remove("class");
    //                            oCell.Attributes.Add("class", S_CSS_ADDED);
    //                            break;
    //                        case Constants.S_DELETED://removed
    //                            oCell.Attributes.Remove("class");
    //                            oCell.Attributes.Add("class", S_CSS_DELETED);
    //                            break;
    //                    }

    //                }
    //                iCellIndex = iCellIndex + 1;

    //            }
    //        }
    //    }
    //}
    
    
    #endregion


}
