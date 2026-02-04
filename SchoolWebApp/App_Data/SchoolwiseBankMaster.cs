using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using BusinessLogic;
using Utility;

/// <summary>
/// Summary description for SchoolwiseBankMaster
/// </summary>
public class SchoolwiseBankMaster
{

    DataTable moDTSchoolwiseBankMaster;
    SchoolwiseBankMasterCollectionBL oSchoolwiseBankMaster;
    public SchoolwiseBankMaster()
    {
        moDTSchoolwiseBankMaster = new DataTable();
        oSchoolwiseBankMaster = new SchoolwiseBankMasterCollectionBL();
    }
    
    public DataTable GetSchoolwiseBankMasterDetails(int aiSchoolId)
    {
        AddColumns();
        oSchoolwiseBankMaster.GetSchoolwiseBankMasterDetails(aiSchoolId);
        var BankDetails = from BankMaster in oSchoolwiseBankMaster.lstSchoolwiseBankMaster.AsEnumerable()
                          select new clsSchoolwiseBankMaster
                   {
                       Schoolwise_Bank_Id = BankMaster.Schoolwise_Bank_Id,
                       Bank_Name = BankMaster.Bank_Name,
                       Count = BankMaster.Count,
                       SchoolId = BankMaster.SchoolId,
                       Is_Deleted = BankMaster.Is_Deleted
                   };
        int iRowIndex = 0;
        foreach (clsSchoolwiseBankMaster bankdetails in BankDetails)
        {
            moDTSchoolwiseBankMaster.Rows.Add();
            SetSchoolwiseBankDetails(bankdetails, iRowIndex);
            iRowIndex++;
        }
        return moDTSchoolwiseBankMaster;
    }
      
    private void SetSchoolwiseBankDetails(clsSchoolwiseBankMaster bankdetails, int iRowIndex)
    {
        moDTSchoolwiseBankMaster.Rows[iRowIndex]["Schoolwise_Bank_Id"] = bankdetails.Schoolwise_Bank_Id;
        moDTSchoolwiseBankMaster.Rows[iRowIndex]["Bank_Name"] = bankdetails.Bank_Name;
        moDTSchoolwiseBankMaster.Rows[iRowIndex]["Count"] = bankdetails.Count;
        moDTSchoolwiseBankMaster.Rows[iRowIndex]["SchoolId"] = bankdetails.SchoolId;
        moDTSchoolwiseBankMaster.Rows[iRowIndex]["Is_Deleted"] = bankdetails.Is_Deleted;
    }
    private void AddColumns()
    {
        moDTSchoolwiseBankMaster.Columns.Add("Schoolwise_Bank_Id");
        moDTSchoolwiseBankMaster.Columns.Add("Bank_Name");
        moDTSchoolwiseBankMaster.Columns.Add("Count");
        moDTSchoolwiseBankMaster.Columns.Add("SchoolId");
        moDTSchoolwiseBankMaster.Columns.Add("Is_Deleted");
    }
}
