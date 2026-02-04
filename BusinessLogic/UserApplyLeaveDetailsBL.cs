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
      public void Save(UserApplyLeaveDetails oUserApplyLeaveDetails, int aiSchoolId, int aiInsertedById, int aiAcademicYearId)
       {
           moUserApplyLeaveDetailsDC.Save(oUserApplyLeaveDetails, aiSchoolId, aiInsertedById, aiAcademicYearId);
       }

      public void SaveLeaveApprovalDetails(LeaveApprovalDetails oLeaveApprovalDetails)
      {
          moUserApplyLeaveDetailsDC.SaveLeaveApprovalDetails(oLeaveApprovalDetails);
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




      public List<UserApplyLeaveDetails> GetAll(int aiSchoolId, int aiUserId, string aiCategoryId, string sortExpression, string sortDirection, int startRowIndex, int maximumRows)
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

          List<UserApplyLeaveDetails> lstUserApplyLeaveDetails =moUserApplyLeaveDetailsDC.GetAll(aiSchoolId, aiUserId, aiCategoryId, sortExpression, startRowIndex, maximumRows);

          //miTotalRows

          if (lstUserApplyLeaveDetails.Count > 0)
              miTotalRows = lstUserApplyLeaveDetails[0].TotalRows;

          return lstUserApplyLeaveDetails;
      }

      public int Count(int aiSchoolId, int aiUserId, string aiCategoryId, string sortExpression, string sortDirection, int startRowIndex, int maximumRows)
      {
          return miTotalRows;
      }
      public UserApplyLeaveDetails GetLeaveDetailsCategory(int aiId)
      {

          return moUserApplyLeaveDetailsDC.GetLeaveDetailsCategory(aiId);
      }
    }
}
