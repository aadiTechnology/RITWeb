/* File Name = MenuFileBL
 * Created Date - 12 July 2011
 * Created by - Vipul
 * Class Description - This class is defined to manage Menu Files.*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using DataCommunicator;
using SchoolEntities;
using Utility;

namespace BusinessLogic
{
    public class MenuFileBL
    {
        #region -- MEMBER(s) --

        private MenuFileDetailsDC moMenuFileDetailsDC;
        private int miTotalRows;

        #endregion -- MEMBER(s) --

        #region -- PROPERTIE(s) --

        public MenuFile MenuFileDetails 
        {
            get { return moMenuFileDetailsDC.moMenuFileDetails; }
            set { moMenuFileDetailsDC.moMenuFileDetails = value; }
        }

        #endregion -- PROPERTIE(s) --

        #region -- CONSTRUCTOR(s) --

        /// <summary>
        ///		Default constructor.
        /// </summary>
		public MenuFileBL()
        {
            moMenuFileDetailsDC = new MenuFileDetailsDC();
        }

        public MenuFileBL(int aiMenuFileDetailsId)
        {
            moMenuFileDetailsDC = new MenuFileDetailsDC(aiMenuFileDetailsId);
        }

        #endregion -- CONSTRUCTOR(s) --

        #region -- PUBLIC METHOD(s) --

        /// <summary>
        /// This method is used to get details of all menu files.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="sortDirection"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public List<MenuFile> GetAll(int aiSchoolId,  string asSearchText,string sortExpression, string sortDirection, int maximumRows, int startRowIndex)
        {
            int iEndIndex = startRowIndex + maximumRows;
            List<MenuFile> lstMenus = moMenuFileDetailsDC.GetAll(aiSchoolId,asSearchText, sortExpression + " " + sortDirection, iEndIndex, startRowIndex);
            if (lstMenus.Count > 0)
                miTotalRows = lstMenus[0].TotalRows;
            else
                miTotalRows = 0;
            return lstMenus;
        }

        /// <summary>
        /// This method is used to get total count of menu files.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="sortDirection"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public int GetCount(int aiSchoolId,string asSearchText, string sortExpression, string sortDirection, int maximumRows, int startRowIndex)
        {
            return miTotalRows;
        }

	    /// <summary>
	    ///		Saves the menu file to database.
	    /// </summary>
	    /// <param name="aoMenuFile"></param>
	    /// <param name="aoFile"></param>
	    /// <param name="abIsNewFile"></param>
	    /// <exception cref="FileNotFoundException">If the aoFile param passed is null.</exception>
        public static void SaveMenuFile(MenuFile aoMenuFile, HttpPostedFile aoFile, bool abIsNewFile, string asOldFileName, string asURL)
		{
            if (!aoMenuFile.IsURL)
            {
                string sFilePath;
                string sPathPrefix = @"RITeSchool\Downloads\";
                if (!aoFile.IsNull() && aoFile.FileName != string.Empty)
                {
                    if (aoFile.IsNull())
                        throw new FileNotFoundException();
                    string sFileName = aoFile.FileName.Substring(aoFile.FileName.LastIndexOf("\\") + 1);
                    ValidateExtension(sFileName);
                    if (aoFile.ContentLength > 5242880)
                        throw new FileNotFoundException("File size exceeds max limit of 5mb.");

                    string sServerPath = HttpContext.Current.Server.MapPath(@"~\" + sPathPrefix);
                    if (!sServerPath.EndsWith(@"\"))
                        sServerPath = sServerPath + @"\";

                    sFilePath = RenameFile(sServerPath + sFileName);
                    aoFile.SaveAs(sFilePath);
                    aoMenuFile.Path = sPathPrefix + Path.GetFileName(sFilePath);
                }
                else
                {
                    sFilePath = asOldFileName;
                    aoMenuFile.Path = sPathPrefix + Path.GetFileName(sFilePath);
                }
            }
            else
               
                aoMenuFile.Path = asURL;
            MenuFileDetailsDC.SaveMenuFile(aoMenuFile, abIsNewFile);
		}

		/// <summary>
		///		Deletes the specified menu file.
		/// </summary>
		/// <param name="aiMenuFileId"></param>
		public static void DeleteMenuFile(int aiMenuFileId)
		{
			MenuFileDetailsDC.DeleteMenuFile(aiMenuFileId);
		}

		/// <summary>
		///		Updates the old file path in the menus with the new path.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="asOldFilePath"></param>
		/// <param name="asNewFilePath"></param>
        public static void UpdateFileDetails(int aiSchoolId, string asOldFilePath, string asNewFilePath, string asURL)
        {
            MenuFileDetailsDC.UpdateFileDetails(aiSchoolId, asOldFilePath, asNewFilePath,asURL);
        }
        
		#endregion -- PUBLIC METHOD(s) --

		/// <summary>
		///		
		/// </summary>
		/// <param name="asFilename"></param>
		private static void ValidateExtension(string asFilename)
		{
			asFilename = asFilename.ToLower();

            if (!(asFilename.EndsWith(".pdf") || asFilename.EndsWith(".doc") || asFilename.EndsWith(".docx") || asFilename.EndsWith(".xls") || asFilename.EndsWith(".xlsx") || asFilename.EndsWith(".ppt") || asFilename.EndsWith(".pptx") || asFilename.EndsWith(".pps") || asFilename.EndsWith(".ppsx")))
                throw new FileNotFoundException("Invalid file type uploaded. Valid extensions are .pdf, .doc, .docx, .xls, .xlsx. , .ppt ,.pptx ,.pps and .ppsx");
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="asFilePath"></param>
		/// <returns></returns>
		private static string RenameFile(string asFilePath)
		{
			if (asFilePath.IsNullOrEmpty())
				return String.Empty;

			if (!File.Exists(asFilePath))
				return asFilePath;

			return String.Format(@"{0}\{1}.{2}{3}",
								 Path.GetDirectoryName(asFilePath),
								 Path.GetFileNameWithoutExtension(asFilePath),
								 DateTime.Now.ToString("yyyyMMddhhmmss"),
								 Path.GetExtension(asFilePath));
		}
    }
}
