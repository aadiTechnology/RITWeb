using System;
using System.Data;
using System.Collections;
using Utility;
using DataCommunicator;

/// <summary>
/// This class displays the Category details.
/// Only Admin level users have access to this page.
/// 1. User 1st Enter the category name in textbox.
/// 2. And then Update or Delete existing category.
/// </summary>

namespace BusinessLogic
{
    public class CategoryBL
    {
        #region Data members

        private CategoryDC.CategoryStructDetails moCategoryStructDetails;
        private CategoryDC moCategoryDC = new CategoryDC();

        #endregion

        #region " Constructor "

        public CategoryBL()
        {
            moCategoryDC.CategoryInfo = moCategoryStructDetails;
        }

        public CategoryBL(int aiCategoryId)
        {
            moCategoryDC = new CategoryDC(aiCategoryId);
            moCategoryStructDetails = moCategoryDC.CategoryInfo;
        }

        public CategoryBL(int aiCatagoryID, int iSchoolId, int iAcademicYearId)
        {
            moCategoryDC = new CategoryDC(aiCatagoryID, iSchoolId, iAcademicYearId);
            moCategoryStructDetails = moCategoryDC.CategoryInfo;
        }
        
        #endregion " Constructor "
       
        public DataSet RetriveCategoryList()
        {
            moCategoryDC.CategoryInfo = moCategoryStructDetails;
            return moCategoryDC.RetriveCategoryList();
        }

        public DataTable RetriveMainCategoryList()
        {
            moCategoryDC.CategoryInfo = moCategoryStructDetails;
            return moCategoryDC.RetriveMainCategoryList();
        }

        public void IsDuplicateCategory()
        {
            moCategoryDC.CategoryInfo = moCategoryStructDetails;
            bool bFlag = moCategoryDC.IsDuplicateCategory();
            if (bFlag == false)
                throw new BusinessLogic.Exceptions.DuplicateEntityException("Category name is already exist.");
        }

        public void AddCategory()
        {
            moCategoryDC.CategoryInfo = moCategoryStructDetails;
            moCategoryDC.AddCategory();
        }
        
        public void UpdateCategory()
        {
            moCategoryDC.CategoryInfo = moCategoryStructDetails;
            moCategoryDC.UpdateCategory();
        }

        public void DeleteCategory()
        {
            moCategoryDC.CategoryInfo = moCategoryStructDetails;
            moCategoryDC.DeleteCategory();
        }


        /// <summary>
        /// this method is used for RI check, when we delete category.
        /// </summary>
        /// <param name="iCategoryID"></param>
        /// <param name="sCategoryName"></param>
        /// <param name="aAcademicYearId"></param>
        public static void GetDependanciesForCategory(int iCategoryID, string sCategoryName, int aAcademicYearId)
        {
            ArrayList oArrChgStdsMsg = new ArrayList();

            string sMessage = "";
            string sMsg = "";
            int iParentId = Convert.ToInt32(Constants.ReferenceId.CategoryId);

            sMessage = ReferenceDC.CheckDependenciesAndGetErrorMessages(iParentId, iCategoryID, sCategoryName, aAcademicYearId);
            if (!sMessage.Equals(""))
            {
                oArrChgStdsMsg.Add(sMessage);
            }

            if (oArrChgStdsMsg.Count != 0)
            {
                IEnumerator ie = oArrChgStdsMsg.GetEnumerator();
                while (ie.MoveNext())
                {
                    sMsg = sMsg + Convert.ToString(ie.Current) + "<BR>";
                }
                throw new BusinessLogic.Exceptions.ReferenceExceptions(sMsg);
            }
        }

        /// <summary>
        /// this method is used for RI check, when we delete category.
        /// </summary>
        /// <param name="iCategoryID"></param>
        /// <param name="sCategoryName"></param>
        /// <param name="aAcademicYearId"></param>
        public static void GetDependanciesForSubCategory(int iCategoryID, string sCategoryName, int aAcademicYearId)
        {
            ArrayList oArrChgStdsMsg = new ArrayList();

            string sMessage = "";
            string sMsg = "";
            int iParentId = Convert.ToInt32(Constants.ReferenceId.SubCategoryId);

            sMessage = ReferenceDC.CheckDependenciesAndGetErrorMessages(iParentId, iCategoryID, sCategoryName, aAcademicYearId);
            if (!sMessage.Equals(""))
            {
                oArrChgStdsMsg.Add(sMessage);
            }

            if (oArrChgStdsMsg.Count != 0)
            {
                IEnumerator ie = oArrChgStdsMsg.GetEnumerator();
                while (ie.MoveNext())
                {
                    sMsg = sMsg + Convert.ToString(ie.Current) + "<BR>";
                }
                throw new BusinessLogic.Exceptions.ReferenceExceptions(sMsg);
            }

        }

        #region " Property "

        public Int32 SchoolId
        {
            get { return moCategoryStructDetails.miSchoolId; }
            set { moCategoryStructDetails.miSchoolId = value; }
        }
        public string CategoryName
        {
            get { return moCategoryStructDetails.msCategoryName; }
            set { moCategoryStructDetails.msCategoryName = value; }
        }
        public Int32 CategoryId
        {
            get { return moCategoryStructDetails.miCategoryId; }
            set { moCategoryStructDetails.miCategoryId = value; }
        }
        public string SubCategoryName
        {
            get { return moCategoryStructDetails.msSubCategoryName; }
            set { moCategoryStructDetails.msSubCategoryName = value; }
        }
        public Int32 SubCategoryId
        {
            get { return moCategoryStructDetails.miSubCategoryId; }
            set { moCategoryStructDetails.miSubCategoryId = value; }
        }
        public Int32 ParentId
        {
            get { return moCategoryStructDetails.miParentId; }
            set { moCategoryStructDetails.miParentId = value; }
        }
        public Int32 CategoryLevel
        {
            get { return moCategoryStructDetails.miCategoryLevel; }
            set { moCategoryStructDetails.miCategoryLevel = value; }
        }
        public Int16 IsPrintable
        {
            get { return moCategoryStructDetails.miIsPrintable; }
            set { moCategoryStructDetails.miIsPrintable = value; }
        }
        public Int32 UserId
        {
            get { return moCategoryStructDetails.miUserId; }
            set { moCategoryStructDetails.miUserId = value; }
        }
        public char IsDeleted
        {
            get { return moCategoryStructDetails.msIsDeleted; }
            set { moCategoryStructDetails.msIsDeleted = value; }
        }
        public Int32 InsertedById
        {
            get { return moCategoryStructDetails.miInsertedById; }
            set { moCategoryStructDetails.miInsertedById = value; }
        }
        public System.DateTime InsertedDate
        {
            get { return moCategoryStructDetails.mdtInsertedDate; }
            set { moCategoryStructDetails.mdtInsertedDate = value; }
        }
        public Int32 UpdatedById
        {
            get { return moCategoryStructDetails.miUpdatedById; }
            set { moCategoryStructDetails.miUpdatedById = value; }
        }
        public System.DateTime UpdatedDate
        {
            get { return moCategoryStructDetails.mdtUpdatedDate; }
            set { moCategoryStructDetails.mdtUpdatedDate = value; }
        }

        #endregion

        public void UpdateSubCategory()
        {
            moCategoryDC.CategoryInfo = moCategoryStructDetails;
            moCategoryDC.UpdateSubCategory();
        }
        
        public void IsDuplicateSubCategory()
        {
            moCategoryDC.CategoryInfo = moCategoryStructDetails;
            bool bIsDuplicate = moCategoryDC.IsDuplicateSubCategory();
            if (bIsDuplicate == false)
                throw new BusinessLogic.Exceptions.DuplicateEntityException("Sub category name is already exist.");
        }
    }
}


