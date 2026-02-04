using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;
using DataCommunicator;

namespace BusinessLogic
{
    public class AutoSuggestBL
    {
        #region Student

        public static List<String> GetStudentDataForAutoSearch(string asSearchText, int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStdDivId, bool asShowLeftStudents, bool abIncludeRegNo, bool abShowOnlyLeftStudents)
        {
            List<string> lstFilteredStudents;
            var lstStudents = CacheManagerBL.GetStudentList(aiSchoolId, aiAcademicYearId);

            if (aiStandardId != 0)
                lstStudents = lstStudents.Where(st => st.StandardId == aiStandardId).ToList();

            if (aiDivisionId != 0)
                lstStudents = lstStudents.Where(st => st.DivisionId == aiDivisionId).ToList();

            if (aiStdDivId != 0)
                lstStudents = lstStudents.Where(st => st.StdDivId == aiStdDivId).ToList();

            var tst = lstStudents.Where(obj => obj.IsLeft == true).Select(obj => abIncludeRegNo ? obj.RegistraionNo + " - " + obj.Name : obj.Name);

            if (abShowOnlyLeftStudents)
            {
                lstFilteredStudents = lstStudents.Where(obj => (obj.Name.ToLower().Contains(asSearchText.ToLower()) || obj.RegistraionNo.ToLower().Contains(asSearchText.ToLower()) || obj.NameFL.ToLower().Contains(asSearchText.ToLower())) && obj.IsLeft == true).Select(obj => abIncludeRegNo ? obj.RegistraionNo + " - " + obj.Name : obj.Name).Take(20).ToList();
            }
            else
            {
                if (asShowLeftStudents)
                    lstFilteredStudents = lstStudents.Where(obj => obj.Name.ToLower().Contains(asSearchText.ToLower()) || obj.RegistraionNo.ToLower().Contains(asSearchText.ToLower()) || obj.NameFL.ToLower().Contains(asSearchText.ToLower())).Select(obj => abIncludeRegNo ? obj.RegistraionNo + " - " + obj.Name : obj.Name).Take(20).ToList();
                else
                    lstFilteredStudents = lstStudents.Where(obj => (obj.Name.ToLower().Contains(asSearchText.ToLower()) || obj.RegistraionNo.ToLower().Contains(asSearchText.ToLower()) || obj.NameFL.ToLower().Contains(asSearchText.ToLower())) && obj.IsLeft == false).Select(obj => abIncludeRegNo ? obj.RegistraionNo + " - " + obj.Name : obj.Name).Take(20).ToList();
            }
            return lstFilteredStudents;
        }

        public static void RefreshStudentCache(int aiSchoolId, int aiAcademicYearId, List<int> alstYearwiseStudentIds, Constants.Action aoAction)
        {
            string sYearwiseStudentIDs = string.Join(",", alstYearwiseStudentIds);
            if (sYearwiseStudentIDs.StartsWith(","))
                sYearwiseStudentIDs = sYearwiseStudentIDs.Substring(1);
            var lstUpdatedStudents = AutoSuggestDC.GetStudentDataForAutoSearch(aiSchoolId, aiAcademicYearId, sYearwiseStudentIDs);
            var lstStudents = CacheManagerBL.GetStudentList(aiSchoolId, aiAcademicYearId);

            if (lstUpdatedStudents.Count > 0)
            {
                if (alstYearwiseStudentIds.Count == 1)
                {
                    var oStudent = lstUpdatedStudents.FirstOrDefault();
                    if (aoAction == Constants.Action.Insert)
                    {
                        if (!lstStudents.Where(st => st.StudentId == oStudent.StudentId).Any())
                            lstStudents.Add(oStudent);
                    }
                    else if (aoAction == Constants.Action.Delete)
                    {
                        int iStudentId = alstYearwiseStudentIds.FirstOrDefault();
                        if (lstStudents.Where(st => st.StudentId == iStudentId).Any())
                            lstStudents.Remove(lstStudents.Where(st => st.StudentId == iStudentId).FirstOrDefault());
                    }
                    else
                    {
                        var oSelectedStudent = lstStudents.Where(st => st.StudentId == oStudent.StudentId).FirstOrDefault();
                        if (oSelectedStudent != null)
                        {
                            oSelectedStudent.Name = oStudent.Name;
                            oSelectedStudent.RegistraionNo = oStudent.RegistraionNo;
                            oSelectedStudent.StandardId = oStudent.StandardId;
                            oSelectedStudent.DivisionId = oStudent.DivisionId;
                            oSelectedStudent.StdDivId = oStudent.StdDivId;
                            oSelectedStudent.IsLeft = oStudent.IsLeft;
                        }
                    }
                }
                else
                {
                    lstUpdatedStudents.ForEach
                        (
                            stdt =>
                            {
                                if (!lstStudents.Where(st => st.StudentId == stdt.StudentId).Any())
                                    lstStudents.Add(stdt);
                                else
                                {
                                    var existingStudent = lstStudents.Where(st => st.StudentId == stdt.StudentId).FirstOrDefault();
                                    existingStudent.Name = stdt.Name;
                                    existingStudent.RegistraionNo = stdt.RegistraionNo;
                                    existingStudent.StandardId = stdt.StandardId;
                                    existingStudent.DivisionId = stdt.DivisionId;
                                    existingStudent.StdDivId = stdt.StdDivId;
                                    existingStudent.IsLeft = stdt.IsLeft;
                                }
                            }
                        );
                }
            }
            else if (aoAction == Constants.Action.Delete)
            {
                int iStudentId = alstYearwiseStudentIds.FirstOrDefault();
                if (lstStudents.Where(st => st.StudentId == iStudentId).Any())
                    lstStudents.Remove(lstStudents.Where(st => st.StudentId == iStudentId).FirstOrDefault());
            }

            CacheManagerBL.RebuildUserCache(aiSchoolId, aiAcademicYearId, 0, 0);
        }


        #endregion

        #region Staff 

        public static List<String> GetStaffDataForAutoSearch(string asSearchText, int aiSchoolId, int aiAcademicYearId, int aiUserRoleId, bool asShowDeleted, int aiStatusId=0)
        {
            List<string> lstFilteredStaff;
            var lstStaff = CacheManagerBL.GetStaffList(aiSchoolId, aiAcademicYearId);

            if(aiStatusId != 0)
                lstStaff = lstStaff.Where(st => st.StatusId == aiStatusId).ToList();

            if (aiUserRoleId != 0)
                lstStaff = lstStaff.Where(st => st.UserRoleId == aiUserRoleId).ToList();

            if (asShowDeleted)
                lstFilteredStaff = lstStaff.Where(obj => obj.Name.ToLower().Contains(asSearchText.ToLower()) || obj.NameFL.ToLower().Contains(asSearchText.ToLower())).Select(obj => obj.Name).Take(20).ToList();
            else
                lstFilteredStaff = lstStaff.Where(obj => (obj.Name.ToLower().Contains(asSearchText.ToLower()) || obj.NameFL.ToLower().Contains(asSearchText.ToLower())) && obj.IsDeleted == false).Select(obj => obj.Name).Take(20).ToList();
            return lstFilteredStaff;
        }

        public static void RefreshStaffCache(int aiSchoolId, int aiAcademicYearId, List<int> alstUserIds, Constants.Action aoAction)
        {
            string sUserIds = string.Join(",", alstUserIds);
            if (sUserIds.StartsWith(","))
                sUserIds = sUserIds.Substring(1);
            var lstUpdatedStaffMembers = AutoSuggestDC.GetStaffDataForAutoSearch(aiSchoolId, aiAcademicYearId, sUserIds);
            var lstStaffMembers = CacheManagerBL.GetStaffList(aiSchoolId, aiAcademicYearId);

            if (alstUserIds.Count == 1)
            {
                var oStaffMember = lstUpdatedStaffMembers.FirstOrDefault();
                if (aoAction == Constants.Action.Insert)
                {
                    if (!lstStaffMembers.Where(st => st.UserId == oStaffMember.UserId).Any())
                        lstStaffMembers.Add(oStaffMember);
                }
                else if (aoAction == Constants.Action.Delete)
                {
                    int iUserId = alstUserIds.FirstOrDefault();
                    if (lstStaffMembers.Where(st => st.UserId == iUserId).Any())
                        lstStaffMembers.Remove(lstStaffMembers.Where(st => st.UserId == iUserId).FirstOrDefault());
                }
                else
                {
                    var oSelectedStudent = lstStaffMembers.Where(st => st.UserId == oStaffMember.UserId).FirstOrDefault();
                    if (oSelectedStudent != null)
                    {
                        oSelectedStudent.Name = oStaffMember.Name;
                        oSelectedStudent.UserRoleId = oStaffMember.UserRoleId;                        
                        oSelectedStudent.IsDeleted = oStaffMember.IsDeleted;                        
                    }
                }
            }
            else
            {
                lstUpdatedStaffMembers.ForEach
                    (
                        stdt =>
                        {
                            if (!lstStaffMembers.Where(st => st.UserId == stdt.UserId).Any())
                                lstStaffMembers.Add(stdt);
                        }
                    );
            }

            CacheManagerBL.RebuildUserCache(aiSchoolId, aiAcademicYearId, 0, 0);
        }

        public static List<String> GetUserDataForAutoSearch(string asSearchText, int aiSchoolId, int aiAcademicYearId, int aiUserRoleId, bool asShowDeleted)
        {
            List<string> lstFilteredUser;
            
            List<User> lstUsers = new List<User>();
            var lstStaff = CacheManagerBL.GetStaffList(aiSchoolId, aiAcademicYearId);
            var lstStudents = CacheManagerBL.GetStudentList(aiSchoolId, aiAcademicYearId);

            if (aiUserRoleId != 0)
            {
                if (aiUserRoleId == Constants.UserRoles.Student.ToInt())
                {
                    //lstStudents = lstStudents.Select(std => std.Name);
                    if (asShowDeleted)
                        lstFilteredUser = lstStudents.Where(obj => obj.Name.ToLower().Contains(asSearchText.ToLower()) || obj.NameFL.ToLower().Contains(asSearchText.ToLower())).Select(obj => obj.Name).Take(20).ToList();
                    else
                        lstFilteredUser = lstStudents.Where(obj => (obj.Name.ToLower().Contains(asSearchText.ToLower()) || obj.NameFL.ToLower().Contains(asSearchText.ToLower())) && obj.IsLeft == false).Select(obj => obj.Name).Take(20).ToList();
                }
                else
                {
                    lstStaff = lstStaff.Where(st => st.UserRoleId == aiUserRoleId).ToList();
                    if (asShowDeleted)
                        lstFilteredUser = lstStaff.Where(obj => obj.Name.ToLower().Contains(asSearchText.ToLower()) || obj.NameFL.ToLower().Contains(asSearchText.ToLower())).Select(obj => obj.Name).Take(20).ToList();
                    else
                        lstFilteredUser = lstStaff.Where(obj => (obj.Name.ToLower().Contains(asSearchText.ToLower()) || obj.NameFL.ToLower().Contains(asSearchText.ToLower())) && obj.IsDeleted == false).Select(obj => obj.Name).Take(20).ToList();
                }
            }
            else
            {
                if (asShowDeleted)
                {   
                    lstUsers.AddRange(lstStaff.Select(st => new User { Name = st.Name, NameFL=st.NameFL}).Union(lstStudents.Select(std => new User { Name = std.Name, NameFL = std.NameFL})));
                    lstFilteredUser = lstUsers.Where(obj => obj.Name.ToLower().Contains(asSearchText.ToLower()) || obj.NameFL.ToLower().Contains(asSearchText.ToLower())).Select(obj => obj.Name).Take(20).ToList();
                }
                else
                {   
                    lstUsers.AddRange(lstStaff.Where(st => !st.IsDeleted).Select(st => new User{Name = st.Name, NameFL = st.NameFL}).Union(lstStudents.Where(std => !std.IsLeft).Select(std => new User{ Name = std.Name, NameFL = std.NameFL})));
                    lstFilteredUser = lstUsers.Where(obj => obj.Name.ToLower().Contains(asSearchText.ToLower()) || obj.NameFL.ToLower().Contains(asSearchText.ToLower())).Select(obj => obj.Name).Take(20).ToList();
                }
            }

            return lstFilteredUser;
        }

        public static List<string> GetDataForMessageCenter(string asSearchText, int aiSchoolId, int aiAcademicYearId, int aiUserRoleId, int aiUserId, bool abShowOnlyCoordinator)
        {
            List<string> lstFilteredStaff;
            var lstStaff = CacheManagerBL.GetUserListForMessageCenter(aiSchoolId, aiAcademicYearId, aiUserRoleId, aiUserId);
            var lstClasses = CacheManagerBL.GetClassesForMessageCenter(aiSchoolId, aiAcademicYearId, aiUserRoleId, aiUserId);

            if (aiUserRoleId != 0)
                lstStaff = lstStaff.Where(st => st.UserRoleId == aiUserRoleId).ToList();

            List<AutoSearchUser> lstAutoSearchUser = new List<AutoSearchUser>();
            if (lstStaff.Any(st => st.UserId == aiUserId && st.UserRoleId == 2 && st.HasFullAccess == false))
            {
                if (!abShowOnlyCoordinator)
                {
                    List<int> lstStdDivIds = lstClasses.Where(cs => cs.TeacherUserId == aiUserId).Select(cs => cs.StdDivId).Distinct().ToList();
                    lstAutoSearchUser = lstStaff.Join(lstStdDivIds, st => st.StdDivId, sd => sd, (st, sd) => new { st }).Select(ss => ss.st).ToList();
                }

                lstAutoSearchUser.AddRange(lstStaff.Where(st => st.UserRoleId != 3).ToList());
            }
            else if (lstStaff.Any(st => st.UserId == aiUserId && st.UserRoleId == 3))
            {
                if (abShowOnlyCoordinator)
                    lstAutoSearchUser = lstStaff.Where(st => st.UserRoleId == 2 && st.IsCoordinator == true).ToList();
                else
                {
                    int iStdDivId = lstStaff.Where(st => st.UserId == aiUserId).Select(st => st.StdDivId).FirstOrDefault();
                    List<int> lstTeacherIds = lstClasses.Where(cs => cs.StdDivId == iStdDivId).Select(cs => cs.TeacherUserId).Distinct().ToList();
                    lstAutoSearchUser = lstStaff.Join(lstTeacherIds, st => st.UserId, sd => sd, (st, sd) => new { st }).Select(ss => ss.st).ToList();
                }
                lstAutoSearchUser.AddRange(lstStaff.Where(st => st.UserRoleId != 2 && st.UserRoleId != 3).ToList());
            }
            else
                lstAutoSearchUser = lstStaff;

            lstFilteredStaff = lstAutoSearchUser.Where(obj => (obj.Name.ToLower().Contains(asSearchText.ToLower()) || obj.NameFL.ToLower().Contains(asSearchText.ToLower()))).Select(obj => obj.Name).Take(20).ToList();
            return lstFilteredStaff;
        }

        #endregion
    }
}
