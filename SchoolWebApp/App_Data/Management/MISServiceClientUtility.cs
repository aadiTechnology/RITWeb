using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;
using BusinessLogic;
using Management.Client;
using Management.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Utility;

/// <summary>
///		Client class for the ManagementService, which can be used from the UI.
///		Marked Serializable for Custom / StateServer / SQLServer session modes.
///		WCF clients and BL instances are NonSerialized and recreated after deserialize.
/// </summary>
[Serializable]
public class MISServiceClientUtility
{

	#region -- MEMBER(s) --

	// WCF ClientBase / MarshalByRef objects cannot be serialized into session state.
	[NonSerialized]
	private ManagementServiceClient moMgmtServiceClient;

	[NonSerialized]
	private ManagementServiceConfigBL moMgmtServiceConfigBL;

	private int miSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();

	#endregion -- MEMBER(s) --

	#region -- PROPERTIES --

	public List<SchoolMISDetails> AssociatedMISSchools { get; set; }
	public List<AcademicYear> AcademicYears { get; set; }
	public List<FinancialYear> FinancialYears { get; set; }
	public Dictionary<Tuple<int, int>, int> AcademicYearMap { get; set; }
	public Dictionary<Tuple<int, int>, int> FinancialYearMap { get; set; }
	public Dictionary<Tuple<int, int>, List<AccountsEntities.MISReportSection>> MISReports { get; set; }
	public int CurrentAcademicYearId { get; set; }
	public int CurrentFinancialYearId { get; set; }
	public DateTime StudentAttendanceDate { get; set; }
	public bool IncludeInternalFees { get; set; }
	public bool IncludeCautionMoney { get; set; }

	#endregion -- PROPERTIES --

	#region -- CONSTRUCTOR(s) --

	public MISServiceClientUtility()
	{
		InitializeTransientMembers();
	}

	/// <summary>
	///		Recreates non-serializable members after the object is restored from session.
	/// </summary>
	[OnDeserialized]
	private void OnDeserialized(StreamingContext context)
	{
		InitializeTransientMembers();
	}

	private void InitializeTransientMembers()
	{
		moMgmtServiceClient = new ManagementServiceClient();
		moMgmtServiceConfigBL = new ManagementServiceConfigBL(miSchoolId);
	}

	/// <summary>
	///		Ensures non-serializable members exist (needed if custom session store
	///		does not invoke [OnDeserialized]).
	/// </summary>
	private void EnsureTransientMembers()
	{
		if (moMgmtServiceConfigBL == null)
			moMgmtServiceConfigBL = new ManagementServiceConfigBL(miSchoolId);
		if (moMgmtServiceClient == null)
			moMgmtServiceClient = new ManagementServiceClient();
	}

	#endregion -- CONSTRUCTOR(s) --

	#region -- PUBLIC METHOD(s) --

	/// <summary>
	///		Initializes academic and financial year information and all widgets.
	/// </summary>
	public void Initialize()
	{
		EnsureTransientMembers();
		AssociatedMISSchools = moMgmtServiceConfigBL.GetAssociatedSchools();

		foreach (var school in AssociatedMISSchools)
		{
			var oUri = new Uri(school.ServiceURL);
			var oAddress = new EndpointAddress(oUri);
			moMgmtServiceClient = new ManagementServiceClient("default", oAddress);

			List<AcademicYear> lstAcademicYears = moMgmtServiceClient.GetAcademicYears(school.Token);
			if (lstAcademicYears != null && lstAcademicYears.Count > 0)
				school.AcademicYears = lstAcademicYears;

			List<FinancialYear> lstFinancialYears = moMgmtServiceClient.GetFinancialYears(school.Token);
			if (lstFinancialYears != null && lstFinancialYears.Count > 0)
				school.FinancialYears = lstFinancialYears;
		}

		CreateAcademicYearMap();
		CreateFinancialYearMap();

		InitializeWidgets();
	}

	/// <summary>
	///		Initializes all widgets.
	/// </summary>
	public void InitializeWidgets()
	{
		GetStudentMISDetails();
		GetStaffMISDetails();
		GetFeeCollectionDetails();
	}

	/// <summary>
	///		Initializes the Student details widget.
	/// </summary>
	public void GetStudentMISDetails()
	{
		foreach (var school in AssociatedMISSchools)
		{
			var oUri = new Uri(school.ServiceURL);
			var oAddress = new EndpointAddress(oUri);
			moMgmtServiceClient = new ManagementServiceClient("default", oAddress);
			
			if (AcademicYearMap.ContainsKey(new Tuple<int, int>(CurrentAcademicYearId, school.SchoolId)))
			{
				int iAcademicYearId = AcademicYearMap[new Tuple<int,int>(CurrentAcademicYearId, school.SchoolId)];
				school.StudentDetails = moMgmtServiceClient.GetStudentDetails(school.Token, iAcademicYearId, StudentAttendanceDate);
			}
			else
				school.StudentDetails = null;
		}
	}

	/// <summary>
	///		Initializes the Staff details widget.
	/// </summary>
	public void GetStaffMISDetails()
	{
		foreach (var school in AssociatedMISSchools)
		{
			var oUri = new Uri(school.ServiceURL);
			var oAddress = new EndpointAddress(oUri);
			moMgmtServiceClient = new ManagementServiceClient("default", oAddress);
			
			if (AcademicYearMap.ContainsKey(new Tuple<int, int>(CurrentAcademicYearId, school.SchoolId)))
			{
				int iAcademicYearId = AcademicYearMap[new Tuple<int,int>(CurrentAcademicYearId, school.SchoolId)];
				school.StaffDetails = moMgmtServiceClient.GetStaffDetails(school.Token, iAcademicYearId, DateTime.Now);
			}
			else
				school.StaffDetails = null;
		}		
	}

	/// <summary>
	///		Initializes the Fee collection details widget.
	/// </summary>
	public void GetFeeCollectionDetails()
	{
		foreach (var school in AssociatedMISSchools)
		{
			var oUri = new Uri(school.ServiceURL);
			var oAddress = new EndpointAddress(oUri);
			moMgmtServiceClient = new ManagementServiceClient("default", oAddress);
			
			if (AcademicYearMap.ContainsKey(new Tuple<int, int>(CurrentAcademicYearId, school.SchoolId)))
			{
				int iAcademicYearId = AcademicYearMap[new Tuple<int,int>(CurrentAcademicYearId, school.SchoolId)];
				school.FeeCollection = moMgmtServiceClient.GetFeeCollection(school.Token, iAcademicYearId, IncludeInternalFees, IncludeCautionMoney);
			}
			else
				school.FeeCollection = null;
		}		
	}

	/// <summary>
	///		Serializes the data in json format.
	/// </summary>
	/// <returns></returns>
	public string GetJSONSerializedData()
	{
		if (AssociatedMISSchools == null || AssociatedMISSchools.Count <= 0)
			return null;

		var schoolArray = new JArray();
		var schoolDataSourceArray = new JArray();

		foreach (var sc in AssociatedMISSchools)
			schoolArray.Add(JObject.Parse(String.Format("{{ SchoolId: {0}, SchoolName: '{1}', SchoolShortName: '{2}' }}", sc.SchoolId, sc.SchoolName, sc.SchoolShortName)));

		string sSchools = JsonConvert.SerializeObject(schoolArray, Formatting.None);

		for (int iSchoolIndex = 0; iSchoolIndex < AssociatedMISSchools.Count; iSchoolIndex++)
		{
			var sc = AssociatedMISSchools[iSchoolIndex];
			if (iSchoolIndex == 0)
			{
				schoolDataSourceArray.Add(JObject.Parse(String.Format("{{ {0}: '{1}' }}", sc.SchoolShortName, GetStudentDetailsString(sc))));

				schoolDataSourceArray.Add(JObject.Parse(String.Format("{{ {0}: '{1}' }}", sc.SchoolShortName, GetStaffDetailsString(sc))));

				schoolDataSourceArray.Add(JObject.Parse(String.Format("{{ {0} : '{1}' }}", sc.SchoolShortName, GetFeeCollectionString(sc))));
			
				if (FinancialYears != null && FinancialYears.Count > 0)
                    schoolDataSourceArray.Add(JObject.Parse(String.Format("{{ {0}: '<span data-key=\"{1}\" class=\"k-button k-button-icontext{2}\">View Report</span>' }}",
																		   sc.SchoolShortName,
																		   ConfigurationManager.AppSettings["SchoolID"].ToInt() == sc.SchoolId ? Constants.I_ZERO : sc.SchoolId,
																		   SchoolContainsFinancialYear(sc.SchoolId) ? String.Empty : " k-state-disabled")));

                schoolDataSourceArray.Add(JObject.Parse(String.Format("{{ {0}: '<span data-key=\"{1}\" class=\"k-button k-button-icontext{2}\">Login</span>' }}",
                                                                           sc.SchoolShortName,
                                                                           sc.SchoolId,
                                                                           SchoolContainsFinancialYear(sc.SchoolId) ? String.Empty : " k-state-disabled")));

			//	schoolDataSourceArray.Add(JObject.Parse(String.Format("{{ {0}: '<span class=\"k-button k-button-icontext\">Login</span>' }}", sc.SchoolShortName)));
			}
			else
			{
				int iIndex = 0;
				(schoolDataSourceArray[iIndex++] as JObject).Add(sc.SchoolShortName, new JValue(GetStudentDetailsString(sc)));
				
				(schoolDataSourceArray[iIndex++] as JObject).Add(sc.SchoolShortName, new JValue(GetStaffDetailsString(sc)));
				
				(schoolDataSourceArray[iIndex++] as JObject).Add(sc.SchoolShortName, GetFeeCollectionString(sc));

                if (FinancialYears != null && FinancialYears.Count > 0)
                    (schoolDataSourceArray[iIndex++] as JObject).Add(sc.SchoolShortName, new JValue(String.Format("<span id=\"btnView\" data-key=\"{0}\" class='k-button k-button-icontext{1}'>View Report</span>",
																												   ConfigurationManager.AppSettings["SchoolID"].ToInt() == sc.SchoolId ? Constants.I_ZERO : sc.SchoolId,
																												   SchoolContainsFinancialYear(sc.SchoolId) ? String.Empty : " k-state-disabled")));

                (schoolDataSourceArray[iIndex++] as JObject).Add(sc.SchoolShortName, new JValue(String.Format("<span data-key=\"{0}\" class='k-button k-button-icontext{1}'>Login</span>",
                                                                                                                   sc.SchoolId,
                                                                                                                   SchoolContainsFinancialYear(sc.SchoolId) ? String.Empty : " k-state-disabled")));
			
			//	(schoolDataSourceArray[iIndex++] as JObject).Add(sc.SchoolShortName, new JValue("<span class='k-button k-button-icontext'>Login</span>"));
			}
		}

		string sDataSource = JsonConvert.SerializeObject(schoolDataSourceArray, Formatting.None);

		string json = @"[{
			schools: " + sSchools + @",
			datasource: " + sDataSource + @"
		}]";

		return json;
	}

	/// <summary>
	///		Gets the MIS Reports details for the given school. CurrenFinancialYearId is considered as the Financial year id.
	/// </summary>
	/// <param name="aiSchoolId"></param>
	/// <returns></returns>
	public List<AccountsEntities.MISReportSection> GetMISReport(int aiSchoolId)
	{
		if (MISReports == null)
			MISReports = new Dictionary<Tuple<int, int>, List<AccountsEntities.MISReportSection>>();
		
		var oTuple = new Tuple<int, int>(CurrentFinancialYearId, aiSchoolId);
		if (MISReports.ContainsKey(oTuple))
			return MISReports[oTuple];
		
		var oSchool = AssociatedMISSchools.First(sc => sc.SchoolId == aiSchoolId);

		if (oSchool == null)
			return null;

		int iFinancialYearId = FinancialYearMap[oTuple];

		var oUri = new Uri(oSchool.ServiceURL);
		var oAddress = new EndpointAddress(oUri);
		moMgmtServiceClient = new ManagementServiceClient("default", oAddress);

		MISReports[oTuple] = moMgmtServiceClient.GetMISReport(oSchool.Token, iFinancialYearId);

		return MISReports[oTuple];
	}

	/// <summary>
	///		Returns the financial year details for 
	/// </summary>
	/// <param name="aiSchoolId"></param>
	/// <returns></returns>
	public FinancialYear GetFinancialYear(int aiSchoolId)
	{
		var oTuple = new Tuple<int, int>(CurrentFinancialYearId, aiSchoolId);

		if (FinancialYearMap.ContainsKey(oTuple))
		{
			int iFinancialYearId = FinancialYearMap[oTuple];
			return (from sc in AssociatedMISSchools
				   where sc.SchoolId == aiSchoolId
				    from fy in sc.FinancialYears
				   where fy.Id == iFinancialYearId
				  select fy).FirstOrDefault();
		}
	
		return null;
	}

	#endregion -- PUBLIC METHOD(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	///		Creates a map of Academic years of all associated schools.
	/// </summary>
	private void CreateAcademicYearMap()
	{
		if (AssociatedMISSchools == null || AssociatedMISSchools.Count <= 0)
			return;

        AcademicYears = AssociatedMISSchools.Where(sc => sc.AcademicYears != null)
										 .SelectMany(sc => sc.AcademicYears)
										 .GroupBy(ay => new { StartYear = ay.StartDate.Year, EndYear = ay.EndDate.Year })
										 .Select(grp => new AcademicYear { StartDate = new DateTime(grp.Key.StartYear, 1, 1), EndDate = new DateTime(grp.Key.EndYear, 1, 1) })
										 .OrderBy(ay => ay.StartDate)
										 .ToList();
		int iSeed = 1;
		AcademicYearMap = new Dictionary<Tuple<int,int>,int>();
		
		foreach (var ay in AcademicYears)
		{
			ay.Id = iSeed++;
			foreach (var sc in AssociatedMISSchools.Where(sc => sc.AcademicYears != null))
			{
				AcademicYear oAcademicYear = sc.AcademicYears.FirstOrDefault(iay => ay.StartDate.Year == iay.StartDate.Year && ay.EndDate.Year == iay.EndDate.Year);
				if (oAcademicYear != null && oAcademicYear.Id != 0)
				{
					AcademicYearMap.Add(new Tuple<int, int>(ay.Id, sc.SchoolId), oAcademicYear.Id);
					if (sc.SchoolId == miSchoolId && oAcademicYear.Id == HttpContext.Current.Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID].ToInt())
					{
						ay.IsCurrent = true;
						CurrentAcademicYearId = ay.Id;
					}
				}
			}
		}
	}

	/// <summary>
	///		Creates a map of Financial years of all associated schools.
	/// </summary>
	private void CreateFinancialYearMap()
	{
		if (AssociatedMISSchools == null || AssociatedMISSchools.Count <= 0 || !AssociatedMISSchools.Where(sc => sc.FinancialYears != null).SelectMany(sc => sc.FinancialYears).Any())
			return;

		FinancialYears = AssociatedMISSchools.Where(sc => sc.FinancialYears != null)
										  .SelectMany(sc => sc.FinancialYears)
										  .GroupBy(fy => new { StartYear = fy.StartDate.Year, EndYear = fy.EndDate.Year })
										  .Select(grp => new FinancialYear { StartDate = new DateTime(grp.Key.StartYear, 1, 1), EndDate = new DateTime(grp.Key.EndYear, 1, 1) })
										  .OrderBy(fy => fy.StartDate)
										  .ToList();
		int iSeed = 1;
		FinancialYearMap = new Dictionary<Tuple<int,int>,int>();

		foreach (var fy in FinancialYears)
		{
			fy.Id = iSeed++;
			foreach (var sc in AssociatedMISSchools.Where(sc => sc.FinancialYears != null))
			{
				FinancialYear oFinancialYear = sc.FinancialYears.FirstOrDefault(ify => fy.StartDate.Year == ify.StartDate.Year && fy.EndDate.Year == ify.EndDate.Year);
				if (oFinancialYear != null && oFinancialYear.Id != 0)
				{
					FinancialYearMap.Add(new Tuple<int, int>(fy.Id, sc.SchoolId), oFinancialYear.Id);
					if (sc.SchoolId == miSchoolId && oFinancialYear.Id == HttpContext.Current.Session[Constants.S_SESSION_FINANCIAL_YEAR_ID].ToInt())
					{
						fy.IsCurrent = true;
						CurrentFinancialYearId = fy.Id;
					}
				}
			}
		}
	}

	/// <summary>
	///		Determines if a school contains CurrentAcademicYearId.
	/// </summary>
	/// <param name="aiSchoolId"></param>
	/// <returns></returns>
	private bool SchoolContainsAcademicYear(int aiSchoolId)
	{
		return AcademicYearMap.ContainsKey(new Tuple<int, int>(CurrentAcademicYearId, aiSchoolId));
	}

	/// <summary>
	///		Determines if a school contains CurrentFinancialYearId.
	/// </summary>
	/// <param name="aiSchoolId"></param>
	/// <returns></returns>
	private bool SchoolContainsFinancialYear(int aiSchoolId)
	{
		return FinancialYearMap.ContainsKey(new Tuple<int, int>(CurrentFinancialYearId, aiSchoolId));
	}

	/// <summary>
	///		Returns the final student details string to be displayed.
	/// </summary>
	/// <param name="aoSchool"></param>
	/// <returns></returns>
	private string GetStudentDetailsString(SchoolMISDetails aoSchool)
	{
		if (aoSchool == null || aoSchool.StudentDetails == null)
			return "N / A";

		return String.Format("{0} / {1}", aoSchool.StudentDetails.TotalAttendanceCount, aoSchool.StudentDetails.TotalCount);
	}

	/// <summary>
	///		Returns the final staff details string to be displayed.
	/// </summary>
	/// <param name="aoSchool"></param>
	/// <returns></returns>
	private string GetStaffDetailsString(SchoolMISDetails aoSchool)
	{
		if (aoSchool == null || aoSchool.StaffDetails == null)
			return "N / A";

		return aoSchool.StaffDetails.TotalCount.ToString();
	}

	/// <summary>
	///		Returns the final fee collection details string to be displayed.
	/// </summary>
	/// <param name="aoSchool"></param>
	/// <returns></returns>
	private string GetFeeCollectionString(SchoolMISDetails aoSchool)
	{
		if (aoSchool == null || aoSchool.FeeCollection == null)
			return "N / A";

		return String.Format("<div class=\"feeBlock\"><div>Rs. {0}</div><div>{1}</div><div>{2}</div></div>",
							  CommonUtility.FormatCurrency(aoSchool.FeeCollection.Fees),
							  IncludeInternalFees ? "Rs. " + CommonUtility.FormatCurrency(aoSchool.FeeCollection.InternalFees) : "-",
							  IncludeCautionMoney ? "Rs. " + CommonUtility.FormatCurrency(aoSchool.FeeCollection.CautionMoney) : "-");
	}

	#endregion -- PRIVATE METHOD(s) --

}