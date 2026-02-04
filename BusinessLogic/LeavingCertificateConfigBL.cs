/*   Author		 : Vishal Shah
 *   Date		 : 10 Sept 2011
 *	 Description : This is the BusinessLogic Layer for the Leaving Certificate Report configuration screen.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using DataCommunicator;
using SchoolEntities;

namespace BusinessLogic
{

	public class LeavingCertificateConfigBL
	{

		#region -- MEMBER(s) --
		
		private int miSchoolId;
		private int miUserId;
		
		#endregion -- MEMBER(s) --
		
		
		#region -- CONSTRUCTOR(s) --
		
		public LeavingCertificateConfigBL(int aiSchoolId, int aiUserId)
		{
			miSchoolId = aiSchoolId;
			miUserId = aiUserId;
		}
		
		#endregion -- CONSTRUCTOR(s) --
		
		
		#region -- PUBLIC METHOD(s) --

		/// <summary>
		/// This function gets all the LC Report Config fields, those which are configured by the school & also those which are not.
		/// </summary>
		/// <param name="aiSchoolId">The Id of the School to fetch Configuration for.</param>
		/// <returns>A List of LeavingCertificateConfig entity</returns>
		public static List<LeavingCertificateConfig> GetLeavingCertificateConfigList(int aiSchoolId)
		{
			return LeavingCertificateConfigDC.GetLeavingCertificateConfigList(aiSchoolId);
		}

		/// <summary>
		/// This function saves the LC Report Config to the database.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiUserId"></param>
		/// <param name="alstReportConfig"></param>
		public void SaveLeavingCertificateConfig(List<LeavingCertificateConfig> alstReportConfig)
		{
			string sConfigXML = GenerateXML(alstReportConfig);
			LeavingCertificateConfigDC.SaveLeavingCertificateConfig(miSchoolId, miUserId, sConfigXML);
		}

		#endregion -- PUBLIC METHOD(s) --


		#region -- PRIVATE METHOD(s) --

		/// <summary>
		/// This function is used to generate an XML for inserting/updating records 
		/// </summary>
		/// <param name="alstReportConfig"></param>
		/// <returns></returns>
		private string GenerateXML(List<LeavingCertificateConfig> alstReportConfig)
		{
			StringWriter sw = new StringWriter();
			new XmlSerializer(alstReportConfig.GetType()).Serialize(sw, alstReportConfig);
			string sXML = sw.ToString();
			sXML = sXML.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", String.Empty);
			return sXML;
		}

		#endregion -- PRIVATE METHOD(s) --

	}

}