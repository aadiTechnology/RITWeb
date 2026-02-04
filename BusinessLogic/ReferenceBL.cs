using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using PreCondionEntities;
using Utility;
using DataCommunicator;
using System.Xml;


namespace BusinessLogic
{
    public class ReferenceBL
    {
        #region Overloaded Constructor 

            public ReferenceBL()
            { 
            }

        #endregion
        #region public methods

        public static string GetPreConditionMsg(Constants.SchoolConfigurations aiConfigId)
        {
            int iConfigId = Convert.ToInt32(aiConfigId);
            int iSchoolId = Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_SCHOOL_ID]);
            int iAcademicYearId = Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR]);
            int iUserRoleId = Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]);
            DataSet oDsPreCondMsg = ReferenceDC.GetPreConditionMsg(iSchoolId, iAcademicYearId, iConfigId);
            return ReferenceBL.FormatData(oDsPreCondMsg, iUserRoleId);
        }

        public static string GetPreConditionMsg(Constants.SchoolConfigurations aiConfigId, int aiAcademicYearId)
        {
            int iConfigId = Convert.ToInt32(aiConfigId);
            int iSchoolId = Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_SCHOOL_ID]);
            //int iAcademicYearId = Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR]);
            int iUserRoleId = Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]);
            DataSet oDsPreCondMsg = ReferenceDC.GetPreConditionMsg(iSchoolId, aiAcademicYearId, iConfigId);
            return ReferenceBL.FormatData(oDsPreCondMsg, iUserRoleId);
        }

        public static string CheckDependenciesAndGetErrorMessages(int aiParentId, int aiParentIdValue, string aiName, int aiAcademicYearId)
        {
            return ReferenceDC.CheckDependenciesAndGetErrorMessages(aiParentId, aiParentIdValue, aiName, aiAcademicYearId);
        }
        public  string CheckDependencies(Constants.ReferenceId aParentId, Hashtable aoHash, int aiAcademicYearId)
        {
            string sXml =  GetXML(aoHash);
            DataTable oDt = ReferenceDC.CheckDependenciesAndGetErrorMessages(aParentId, sXml, aiAcademicYearId);
            return FormatMessage(oDt);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aoDt"></param>
        /// <returns></returns>
        private static string FormatMessage(DataTable aoDt)
        {
            string sReturn = "";
            string sMessage = "";
            aoDt.DefaultView.Sort = "Id" + " " + Constants.S_ASCENDING;
            DataView oDv = aoDt.DefaultView;
            for (int i = 0; i < oDv.Table.Rows.Count; i++)
            {
                sMessage = oDv.ToTable().Rows[i]["Msg"].ToString();
                if (!sReturn.Contains(sMessage))
                    sReturn = sReturn + sMessage + "<BR>";
            }
            return sReturn;
        }

        public static string GetPreConditionMsg(int aiConfigId)
        { 
             int iSchoolId  = Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_SCHOOL_ID]);
             int iAcademicYearId = Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR]);
             int iUserRoleId = Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]);
             DataSet oDsPreCondMsg = ReferenceDC.GetPreConditionMsg(iSchoolId, iAcademicYearId, aiConfigId);
             return ReferenceBL.FormatData(oDsPreCondMsg, iUserRoleId);
         }

        public static string CheckPrecondition(int aiSchoolId, int aiAcademicYearId, int aiConfigId)
        {
            DataSet oDs =  ReferenceDC.CheckPrecondition(aiSchoolId, aiAcademicYearId, aiConfigId);
            return ReferenceBL.FormatData(oDs);
        }

        /// <summary>
        /// This is BL function to generalise Prcondition checking Methode.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiConfigId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <returns> string</returns>
        public static string CheckPrecondition(int aiSchoolId, int aiAcademicYearId, int aiConfigId, int aiUserRoleId)
        {
            DataSet oDs = ReferenceDC.CheckPrecondition(aiSchoolId, aiAcademicYearId, aiConfigId);
            return ReferenceBL.FormatData(oDs, aiUserRoleId);

        }
        /// <summary>
        /// This function is used to format  message and configuration deatils taken from DB in appropriate HTML format.
        /// </summary>
        /// <param name="aoDs"></param>
        /// <returns></returns>
        private static string FormatData(DataSet aoDs)
        {
            string sReturn = "";
            if (aoDs.Tables[0].Rows.Count > 0)
            {
                sReturn = "<table class=\"LblNoRecord\"><tr><td class=\"ClsConfigText\">" + Constants.S_NONADMIN_PRECONDITION_MSG + "</td></tr>";
                for(int i=0;i<aoDs.Tables[0].Rows.Count;i++)
                {
                    sReturn = sReturn + "<tr><td><a class=\"ClsConfigLink\" href=" + aoDs.Tables[0].Rows[i]["NavigateURL"].ToString() + ">" + aoDs.Tables[0].Rows[i]["Configure_Name"] + "</a></td></tr>";
                }
                sReturn = sReturn + "</table>";

            }
            return sReturn;
        }

        private static string FormatData(DataSet aoDs, int aiUserRoleId)
        {
            string sReturn = "";
            if (aoDs.Tables[0].Rows.Count > 0)
            {
                if (aiUserRoleId == Convert.ToInt32(Constants.UserRoles.Admin))
                {
                    sReturn = "<table class=\"LblNoRecord\" width=\"100%\"  cellpadding=\"0\" cellspacing=\"0\"><tr><td class=\"ClsConfigText\">"+Constants.S_ERROR_MSG_FOR_ALL_CONFIGURATION+"</td></tr>";
                    for (int i = 0; i < aoDs.Tables[0].Rows.Count; i++)
                    {
                        sReturn = sReturn + "<tr><td><a class=\"ClsConfigLink\" href=" + aoDs.Tables[0].Rows[i]["NavigateURL"].ToString() + ">" + aoDs.Tables[0].Rows[i]["Configure_Name"] + "</a></td></tr>";
                    }
                    sReturn = sReturn + "</table>";
                }
                else
                {
                    sReturn = "<table class=\"LblNoRecord\" width=\"100%\" ><tr><td class=\"ClsConfigText\"><span >" + Constants.S_NONADMIN_PRECONDITION_MSG + "</span></td></tr></table>";
 
                }

            }
            return sReturn;
        }

        public static string GetStudentUIPreConditionMsg(int aiStandardId)
        {
            int iSchoolId = Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_SCHOOL_ID]);
            int iAcademicYearId = Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR]);
            int iUserRoleId = Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]);
            DataSet oDsPreCondMsg = ReferenceDC.GetStudentUIPreConditionMsg(iSchoolId, iAcademicYearId, aiStandardId);
            return ReferenceBL.FormatData(oDsPreCondMsg, iUserRoleId);
        }

     


		public static string GetPreConditionMsgForStudentWiseProgressReport(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId)
        {
            int iUserRoleId = Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]);
			return ReferenceBL.FormatData(ReferenceDC.GetPreConditionMsgForStudentWiseProgressReport(aiSchoolId, aiAcademicYearId, aiStandardDivisionId), iUserRoleId);
        }

        private static string FormatData(List<PreCondition> olstPreCondition, int aiUserRoleId)
        {
            string sReturn = "";
            if (olstPreCondition.Count > 0)
            {
                if (aiUserRoleId == Convert.ToInt32(Constants.UserRoles.Admin))
                {
                    sReturn = "<table class=\"LblNoRecord\" width=\"100%\"  cellpadding=\"0\" cellspacing=\"0\"><tr><td class=\"ClsConfigText\">" + Constants.S_ERROR_MSG_FOR_ALL_CONFIGURATION + "</td></tr>";
                    olstPreCondition.ForEach(
                        oPreCondition =>
                    {
                        sReturn = sReturn + "<tr><td><a class=\"ClsConfigLink\" href=" + oPreCondition.NavigateUrl.ToString() + ">" + oPreCondition.ConfigureName + "</a></td></tr>";
                    });
                    sReturn = sReturn + "</table>";
                }
                else
                {
                    sReturn = "<table class=\"LblNoRecord\" width=\"100%\" ><tr><td class=\"ClsConfigText\"><span >" + Constants.S_NONADMIN_PRECONDITION_MSG + "</span></td></tr></table>";

                }

            }
            return sReturn;
        }
        
        /// <summary>
        /// This function Checks if Junior OR Senior KG standard is configured for school
		/// This is required for the PrePrimaryProgressReportMonths screen.
        /// </summary>
        /// <param name="aiShcoolId"></param>
        /// <returns></returns>
		public static bool IsPrePrimaryStdConfigured(int aiShcoolId) {
			return ReferenceDC.IsPrePrimaryStdConfigured(aiShcoolId);
		}

        #endregion

        private  string GetXML(Hashtable oHash)
        {
            XmlDocument oDoc = new XmlDocument();

            XmlNode oRoot = oDoc.CreateElement("ROOT");
            
            foreach(DictionaryEntry oEntry in oHash)
            {
                XmlNode oNode = oDoc.CreateNode(XmlNodeType.Element, "Reference", "");
                XmlAttribute oAttr = oDoc.CreateAttribute("ID");
                oAttr.Value = oEntry.Key.ToString();
                oNode.Attributes.Append(oAttr);

                oAttr = oDoc.CreateAttribute("Name");
                oAttr.Value = oEntry.Value.ToString();
                oNode.Attributes.Append(oAttr);

                oRoot.AppendChild(oNode);
            }

            return oRoot.OuterXml;
 
        }
        private string GetXML(int[] aoArrIds,string[] aoArrName)
        {
            XmlDocument oDoc = new XmlDocument();
            XmlElement oRoot = oDoc.CreateElement("ROOT");
            for(int i=0;i<aoArrIds.Length;i++)
            {
                XmlNode oNode = oDoc.CreateNode(XmlNodeType.Element, "Reference", "");
               
                XmlAttribute oAttr = oDoc.CreateAttribute("ID");
                oAttr.Value = aoArrIds[i].ToString();

                oAttr = oDoc.CreateAttribute("Name");
                oAttr.Value = aoArrName[i].ToString();
                oRoot.AppendChild(oNode);
            }
            return oRoot.InnerXml;
        }
    }
}
