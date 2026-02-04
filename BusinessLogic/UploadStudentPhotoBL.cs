// File Name - UploadStudentPhotoBL.cs
// Creator - Vishakha
// Created Date - 01 dec 2022

using DataCommunicator;
using SchoolEntities;

namespace BusinessLogic
{
    /// <summary>
    /// This class is used for processing business logic and communicate with data access layer.
    /// </summary>
    public class UploadStudentPhotoBL
    {
        #region Data Member(s)

         UploadStudentPhotoDC moUploadStudentPhotoDC;
        
        #endregion

        #region Properties

       public bool IsSubmitted
        {
            get
            {
                return moUploadStudentPhotoDC.bIsSubmitted;
            }
            set
            {
                moUploadStudentPhotoDC.bIsSubmitted = value;
            }
        }

      #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleDocumentBL" /> class.
        /// </summary>
        public UploadStudentPhotoBL()
        {
            this.moUploadStudentPhotoDC = new UploadStudentPhotoDC();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleDocumentBL" /> class.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="aiUpdatedById"></param>
        public UploadStudentPhotoBL(int aiSchoolId, int aiAcademicYearId, int aiInsertedById)
        {
            this.moUploadStudentPhotoDC = new UploadStudentPhotoDC(aiSchoolId, aiAcademicYearId, aiInsertedById);
        } 

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to save student photo.
        /// </summary>
        /// <param name="aoSavePhotoFile"></param>
        public void Save(SavePhotoFile aoSavePhotoFile)
        {
            moUploadStudentPhotoDC.Save(aoSavePhotoFile);
        }

        /// <summary>
        /// This method is used to delete student photo.
        /// </summary>
        /// <param name="aiStudentId"></param>
        public void Delete(int aiStudentId)
        {
            moUploadStudentPhotoDC.Delete(aiStudentId);
        }

        /// <summary>
        /// This method is used to submit student photo.
        /// </summary>
        /// <param name="aiStudentId"></param>
        public void Submit(int aiStudentId)
        {
            moUploadStudentPhotoDC.Submit(aiStudentId);
        }
        
        /// <summary>
        /// This method is used to get student name for label.
        /// </summary>
        /// <param name="aiSchoolwiseStudentId"></param>
        /// <returns></returns>
        public StudentPhotoUploadDetails GetStudentPhotoUploadDetails(int aiStudentId)
        {
            return moUploadStudentPhotoDC.GetStudentPhotoUploadDetails(aiStudentId);
        }

         #endregion
    }
}
