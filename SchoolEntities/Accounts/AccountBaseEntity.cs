/* ----------------------------------------------------------------------------------
 *	FileName	: AccountsBaseEntity.cs
 *	Author		: Rohini V. Ghule
 *	Date		: 4-Oct-2011
 *	Description : These are the entity classes that are used in the accounts module.
 * ----------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using MasterEntities;
using SchoolEntities;
using Utility;

namespace AccountsEntities
{
	[Serializable]
	public class AccountsBase
	{
		[XmlIgnore]
		public int SchoolId { get; set; }
		[XmlIgnore]
		public int AcademicYearId { get; set; }
		[XmlIgnore]
		public int FinancialYearId { get; set; }
		[XmlIgnore]
		public int InsertedById { get; set; }
		[XmlIgnore]
		public int UpdatedById { get; set; }
		[XmlIgnore]
		public string InsertDate { get; set; }
		[XmlIgnore]
		public string UpdateDate { get; set; }
		[XmlIgnore]
		public bool IsDeleted { get; set; }
	}
	
	[Serializable]
	public class FinancialYear : AccountsBase
	{
		public int Id { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public bool IsCurrent { get; set; }
		public bool IsClosed { get; set; }
	}
	
	[Serializable]
	public class GroupNature : AccountsBase
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
	
	[Serializable]
	public class VoucherType : AccountsBase
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public bool RequiresApproval { get; set; }
	}
	
	public class VoucherStatus : AccountsBase
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
	
	[Serializable]
	public class Group : AccountsBase
	{
		// Database mapped fields
		public int Id { get; set; }
		public Group OriginalGroup { get; set; }
		public string Name { get; set; }
		public Group ParentGroup { get; set; }
		public bool IsPrimary { get; set; }
		public GroupNature GroupNature { get; set; }
		public bool IsConsideredForTrialBalance { get; set; }
        public bool IsPANDetailsRequired { get; set; }
	    public bool IsSystemDefined { get; set; }

        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
	    // Entities which have a reference to Group
		public List<Ledger> Ledgers { get; set; }
	}
	
	[Serializable]
	[KnownType(typeof(BankAccount))]
	public class Ledger : AccountsBase
	{
		// Database mapped fields
		public int Id { get; set; }
		public Ledger OriginalLedger { get; set; }
		public string Name { get; set; }
		public Group Group { get; set; }
		public decimal OpeningBalance { get; set; }
		public decimal ClosingBlanace { get; set; }
		public decimal Budget { get; set; }
		public bool IsDebit { get; set; }
		public bool IsSystemDefined { get; set; }
        public bool IsPanApplicable { get; set; }
	    public string PanNo { get; set; }
        public string FilePath { get; set; }
	    public List<VoucherParticular> VoucherParticulars { get; set; }
	}

	[Serializable]
	public class BankAccount : Ledger
	{
		public Bank Bank { get; set; }
		public string Alias { get; set; }
		public string AccountNumber { get; set; }
		public string Address { get; set; }
		public bool IsForOnlineTransactions { get; set; }
        public bool IsDefault { get; set; }
        public bool IsInternalDefault { get; set; }
	}

	[Serializable]
	public class ChequeConfiguration
	{
		public int Id { get; set; }
		public Bank Bank { get; set; }
		public string Name { get; set; }
		public string ConfigXML { get; set; }
		public int InsertedById { get; set; }
		public int SchoolId { get; set; }
	}
	
	public class ApprovalConfig : AccountsBase
	{
		// Database mapped fields
		public int Id { get; set; }
		public VoucherType VoucherType { get; set; }
		public DesignationMaster CreatorDesignation { get; set; }

		// Collection of Entities which refer to this entity
		public List<ApprovalConfigDetail> ApprovalConfigDetails { get; set; }
	}
	
	public class ApprovalConfigDetail
	{
		public int Id { get; set; }
		public ApprovalConfig ApprovalConfig { get; set; }
		public DesignationMaster ApproverDesignation { get; set; }
		public bool IsFinalApprover { get; set; }
		public int ApprovalOrder { get; set; }
	}
	
	public class UserPermissions : AccountsBase
	{
		public int User_Id { get; set; }
		public bool CanApproveVoucher { get; set; }
		public bool CanCreateVoucher { get; set; }
		public bool CanSelfApprove { get; set; }
		public bool CanDeleteVoucher { get; set; }
		public bool CanEditOldFinancialYear { get; set; }
		public bool Is_Locked { get; set; }
        public bool IsApprovalConfigured { get; set; }
	}

	public class Voucher : AccountsBase
	{
		// Database mapped fields
		public int VoucherId { get; set; }
		public string SerialNumber { get; set; }
		public VoucherType VoucherType { get; set; }
		public DateTime Date { get; set; }
		public string Narration { get; set; }
		public decimal Amount { get; set; }
		public bool IsSubmitted { get; set; }
		public Constants.RequisitionStatus Status { get; set; }
		public int ApprovalOrder { get; set; }
		public bool IsCompleted { get; set; }
		public DateTime TransactionDate { get; set; }
		
		// Need to replace the following fields with entities (A user entity which will have fields like id, fname, lname etc.)
		public string CreatedBy { get; set; }
		
		// Need to change logic so the following fields are not required in this entity
		public int CurrentUserDesigId { get; set; }
		public bool IsFinalApprover { get; set; }
		public int NextApproverDesigId { get; set; }
		public string NextApprover { get; set; }

		// Particulars for the Voucher.
		public List<VoucherParticular> VoucherParticulars { get; set; }
		public List<VoucherAction> VoucherActions { get; set; }

		// Unrelated fields.
		public bool IsFeeVoucher { get; set; }
        public bool IsInternalFeeVoucher { get; set; }
	}
	
	public class VoucherParticular : AccountsBase
	{
		public int Id { get; set; }
        public int VoucherId { get; set; }
		public Voucher Voucher { get; set; }
		public Ledger Ledger { get; set; }
		public bool IsDebit { get; set; }
		public decimal Amount { get; set; }
	}
	
	public class VoucherAction : AccountsBase
	{
		public int Id { get; set; }
		public Voucher Voucher { get; set; }
		public string Comment { get; set; }
		public Constants.RequisitionStatus Status { get; set; }

		// Unrelated fields
		public string UserName { get; set; }
		public bool FinalApprove { get; set; }
	}
	
	public class FeeVoucherDetails : AccountsBase
	{
		public string RegNo { get; set; }
		public string StudentName { get; set; }
		public string Class { get; set; }
		public Constants.PaymentMode PaymentMode { get; set; }
		public string PaymentDetails { get; set; }
		public decimal Amount { get; set; }
		public string PayableFor { get; set; }
	//	public int DepositedLedgerId { get; set; }
	//	public string DepositedLedgerName { get; set; }
		public Ledger DepositLedger { get; set; }
		public string AcademicYear { get; set; }
        public string ReceiptNumber { get; set; }
        public string TransactionNumber { get; set; }
	}
	
	public class FeeVoucherParticulars : AccountsBase
	{
		public int LedgerId { get; set; }
		public bool IsDebit { get; set; }
		public decimal Amount { get; set; }
		public bool IsAdvanceFee { get; set; }
	}

    public class MonthlyTrialBalance : AccountsBase
    {
        public int MonthId { get; set; }
        public string MonthName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Group oGroup { get; set; }
    }

    [Serializable]
	public class MISReportGroup : Group
	{
		public List<MISReportLedger> MISReportLedgers { get; set; }
		public LedgerTotals MonthlyTotals { get; set; }
		
		public decimal Budget
		{
			get { return MISReportLedgers.Select(ledger => ledger.Budget).Sum(); }
		}
	}

	[Serializable]
	public class MISReportLedger : Ledger
	{
		public LedgerTotals MonthlyTotals { get; set; }
	}
    [Serializable]
	public class MISReportSection : AccountsBase
	{
		public string Title { get; set; }
		public int SortOrder { get; set; }
		public List<MISReportGroup> MISReportGroups { get; set; }
		public LedgerTotals MonthlyTotals { get; set; }

		public decimal Budget
		{
			get { return MISReportGroups.Select(grp => grp.Budget).Sum(); }
		}
	}
	
	[Serializable]
	public class LedgerTotals
	{
		public decimal January { get; set; }
		public decimal February { get; set; }
		public decimal March { get; set; }
		public decimal April { get; set; }
		public decimal May { get; set; }
		public decimal June { get; set; }
		public decimal July { get; set; }
		public decimal August { get; set; }
		public decimal September { get; set; }
		public decimal October { get; set; }
		public decimal November { get; set; }
		public decimal December { get; set; }
		
		// Auto properties (they are calculated using the previous properties).
		public decimal Quarter1 { get { return April + May + June; } }
		public decimal Quarter2 { get { return July + August + September; } }
		public decimal Quarter3 { get { return October + November + December; } }
		public decimal Quarter4 { get { return January + February + March; } }
		public decimal Term1 { get { return Quarter1 + Quarter2; } }
		public decimal Term2 { get { return Quarter3 + Quarter4; } }
		public decimal Annual { get { return Term1 + Term2; } }
	}

    public class FeeReceiptDetails
    {
        public string FeeType { get; set; }
        public int Amount { get; set; }
        public string ReceiptNumber { get; set; }
    }

    public class DatewiseVoucher
    {
        public DateTime Date { get; set; }
        public string Particulars { get; set; }
        public string VoucherType { get; set; }
        public int SortOrder { get; set; }
        public string LedgerName { get; set; }
        public int Amount { get; set; }
        public bool IsDebit { get; set; }
        public string SerialNumber { get; set; }
    }

    public class DatewiseVoucherDetails
    {
        public List<DatewiseVoucher> DatewiseVouchers { get; set; }
        public SchoolEntity SchoolDetails { get; set; }
    }
}