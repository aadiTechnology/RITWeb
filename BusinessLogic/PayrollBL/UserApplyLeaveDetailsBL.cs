using System.Collections.Generic;
using DataCommunicator.PayrollDC;
using PayrollEntities;
using Utility;
using SchoolEntities.Payroll;
using System;
using System.Data;



namespace BusinessLogic.PayrollBL
{
   public class UserApplyLeaveDetailsBL
    {
       #region Data Member(s)

       UserApplyLeaveDetailsDC moUserApplyLeaveDetailsDC ;
       private int miTotalRows;

       #endregion
       public UserApplyLeaveDetailsBL()
       {
           moUserApplyLeaveDetailsDC = new UserApplyLeaveDetailsDC();
       }
       public UserApplyLeaveDetailsBL(int aiSchoolId, int aiInsertedById, int aiAcademicYearId)
       {
           moUserApplyLeaveDetailsDC = new UserApplyLeaveDetailsDC(aiSchoolId, aiInsertedById, aiAcademicYearId);
       }
       //public UserApplyLeaveDetailsBL(int aiSchoolId, int aiInsertedById, int aiAcademicYearId)
       // {
       //     moUserApplyLeaveDetailsDC = new UserApplyLeaveDetailsDC(aiSchoolId, aiInsertedById, aiAcademicYearId);
       // }
      public void Save(UserApplyLeaveDetails oUserApplyLeaveDetails)
       {
           moUserApplyLeaveDetailsDC.Save(oUserApplyLeaveDetails);
       }

      public void SaveLeaveApprovalDetails(LeaveApprovalDetails oLeaveApprovalDetails, bool IsFromFinalApproval)
      {
          moUserApplyLeaveDetailsDC.SaveLeaveApprovalDetails(oLeaveApprovalDetails, IsFromFinalApproval);
      }

      public  DataTable GetStaffName(int aiUserId)
      {        
          return moUserApplyLeaveDetailsDC.GetStaffName(aiUserId);
      }
      public DataTable GetCategory(int Id)
      {

          return moUserApplyLeaveDetailsDC.GetCategory(Id);
      }

      //public List<UserApplyLeaveDetails> GetAllFillCategories(int aiCategoryId, int aiUserId)
      //{
      //    return this.moUserApplyLeaveDetailsDC.GetAllFillCategories(aiCategoryId,aiUserId);
      //}
      public void Delete(int aiuserId)
      {
          moUserApplyLeaveDetailsDC.Delete(aiuserId);
      }




      public List<UserApplyLeaveDetails> GetAll(int aiSchoolId, int aiUserId, string aiCategoryId, bool abShowOldNonUpdated, int aiAcademicYearId, bool abShowOnlyNonUpdated, string sortExpression, string sortDirection, int startRowIndex, int maximumRows)
      {
          if (sortExpression == string.Empty)
              sortExpression = "StartDate Desc, EndDate asc, DesignationId asc ,FirstName  asc, MiddleName asc, LastName asc";
            //  sortExpression = "StartDate Desc";

          if (sortExpression.Contains("UserName"))
          {
              if (sortExpression.Contains("DESC"))
                  sortDirection = "Desc";
              else
                  sortDirection = "Asc";
            //  sortExpression = "Description " + sortDirection;
              sortExpression = "Status " + sortDirection + ", DesignationId " + sortDirection + ", FirstName  " + sortDirection + ", MiddleName " + sortDirection + ", LastName " + sortDirection;
          }

          if (aiCategoryId == null)
              aiCategoryId = string.Empty;

          maximumRows = startRowIndex + Constants.I_GRID_PAGE_COUNT;

          List<UserApplyLeaveDetails> lstUserApplyLeaveDetails = moUserApplyLeaveDetailsDC.GetAll(aiSchoolId, aiUserId, aiCategoryId.ToInt(),abShowOldNonUpdated, aiAcademicYearId, abShowOnlyNonUpdated, sortExpression, startRowIndex, maximumRows);

          //miTotalRows

          if (lstUserApplyLeaveDetails.Count > 0)
              miTotalRows = lstUserApplyLeaveDetails[0].TotalRows;

          return lstUserApplyLeaveDetails;
      }

      public int Count(int aiSchoolId, int aiUserId, string aiCategoryId, bool abShowOldNonUpdated, int aiAcademicYearId, bool abShowOnlyNonUpdated, string sortExpression, string sortDirection, int startRowIndex, int maximumRows)
      {
          return miTotalRows;
      }
      public UserApplyLeaveDetails GetLeaveDetailsCategory(int aiId, int aiUserId, int aiLoginUserId)
      {

          return moUserApplyLeaveDetailsDC.GetLeaveDetailsCategory(aiId, aiUserId, aiLoginUserId);
      }

       /// <summary>
      /// This method is used to get login user leave balance details.
       /// </summary>
       /// <param name="aiUSerId"></param>
       /// <returns></returns>
      public List<LeaveBalance> GetLeaveTypeWiseLeaveBalance(int aiUSerId)
      {
          return moUserApplyLeaveDetailsDC.GetLeaveTypeWiseLeaveBalance(aiUSerId);
      }

      public void UpdateLeaveRecordinPayroll(int aiLeaveConfigId, int aiLeaveTypeId, DateTime adtStartDate, DateTime adtEndDate, Decimal adTotalDays)
      {
          moUserApplyLeaveDetailsDC.UpdateLeaveRecordinPayroll(aiLeaveConfigId, aiLeaveTypeId, adtStartDate, adtEndDate, adTotalDays);
      }

      public string ValidateDates(DateTime adtDate, int aiLeaveTypeId, int aiLeaveConfigId)
      {
          return moUserApplyLeaveDetailsDC.ValidateDates(adtDate, aiLeaveTypeId, aiLeaveConfigId);
      }

      public bool ValidateDateOverlapping(DateTime adtStartDate, DateTime adtEndDate, int aiUserId, int aiLeaveConfigId)
      {
          return moUserApplyLeaveDetailsDC.ValidateDateOverlapping(adtStartDate, adtEndDate, aiUserId, aiLeaveConfigId);
      }

      public bool AllowUserToViewAllLeaves()
      {
          return moUserApplyLeaveDetailsDC.AllowUserToViewAllLeaves();
      }
       /// <summary>
      /// This method is used to count pending approval leaves.
       /// </summary>
       /// <param name="aiSchoolId"></param>
       /// <param name="aiAcademicYearId"></param>
       /// <param name="aiUserId"></param>
       /// <returns></returns>
      public static int CountRowsOfWatingAppLeaves(int aiSchoolId, int aiAcademicYearId, int aiUserId)
      {
          return UserApplyLeaveDetailsDC.CountRowsOfRequisition(aiSchoolId, aiAcademicYearId , aiUserId);

      }
    }
}
