/* File Name - PhotoUpdationUtilityUI.aspx.cs
 * Created By - Sachin
 * Created Date - 26-Aug-2022
 * Description - This class is used to copy student photos.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using System.Data.OleDb;
using System.Data;

public partial class PhotoUpdationUtilityUI : SchoolBase
{
    private bool mbUseSerialNumberOption
    {
        get { return moSchool == Constants.SchoolId.VPMCPS; }
    }

    #region Event(s)

    /// <summary>
    /// This event is used to set attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetAttributes();
        }
    }

    /// <summary>
    /// This event is used to update photos.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtData = new DataTable();

            if (mbUseSerialNumberOption)
            {
                string FilePath = @"C:\Users\Admin\Desktop\VP - Photos\PendingIDCard24-25.xlsx";
                string sheetName = "ABSENT";
                string sConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";

                OleDbConnection oCNRecords = null;
                OleDbDataAdapter oDARecords = null;
                DataSet oDSRecords = null;

                oCNRecords = new OleDbConnection(sConnectionString);
                string sSeletcStatement = string.Format("SELECT * FROM [" + sheetName + "$]");
                oDARecords = new OleDbDataAdapter(sSeletcStatement, oCNRecords);
                oDSRecords = new DataSet("Student Data");
                oDARecords.Fill(oDSRecords);
                oCNRecords.Close();

                dtData = oDSRecords.Tables[0];
            }

            if (Directory.Exists(txtPath.Text.Trim()))
            {
                var ext = new List<string> { ".JPG", ".JPEG", ".BMP", ".PNG" };

                DirectoryInfo obj = new DirectoryInfo(txtPath.Text.Trim());

                FileInfo[] files = obj.GetFiles("*.*", SearchOption.AllDirectories).Where(f => ext.Contains(f.Extension.ToUpper())).ToArray();

                if (files.Length > 0)
                {
                    List<string> lstRegNo = new List<string>();
                    foreach (var file in files)
                    {
                        string regNo = file.Name.Substring(0, file.Name.LastIndexOf("."));

                        if (mbUseSerialNumberOption)
                        {
                            //var arrNo = dtData.AsEnumerable().Where(dr => "DSC_" + dr.Field<string>("Photo") == regNo).Select(dr => dr.Field<string>("GRNO")).FirstOrDefault();
                            var arrNo = dtData.AsEnumerable().Where(dr => dr.Field<string>("Photo") == regNo).Select(dr => dr.Field<string>("GRNO")).FirstOrDefault();
                            if (arrNo != null)
                                regNo = arrNo;
                        }

                        lstRegNo.Add(regNo);
                    }

                    string sRegNos = base.GenerateXml(lstRegNo);

                    StudentBL oStudentBL = new StudentBL();

                    List<StudentPhoto> lstRegNos = oStudentBL.GetNonValidRegNos(sRegNos, miSchoolId, miAcademicYearId);

                    List<string> lstNonExistregNos = lstRegNos.Where(reg => reg.SchoolwiseStudentId == 0).Select(reg => reg.RegNo).ToList();
                    List<string> lstDuplicateNos = files.GroupBy(fl => fl.Name).Select(reg => new { RegNo = reg.Key, TotalCount = files.Count(cnt => cnt.Name == reg.Key) }).Where(reg => reg.TotalCount > 1).Select(reg => reg.RegNo).ToList();
                    if (lstNonExistregNos.Count > 0)
                    {
                        string sRegNo = string.Join(", ", lstNonExistregNos);
                        lblMessage.Text = "Invalid Enrolment No. : " + sRegNo;
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        tdMessage.Align = "left";
                    }
                    else if (lstDuplicateNos.Count > 0)
                    {
                        string sDuplicate = string.Join(", ", lstDuplicateNos);
                        lblMessage.Text = "Multiple files are found with same Enrolment No. : " + sDuplicate;
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        tdMessage.Align = "left";
                    }
                    else
                    {
                        List<StudentPhoto> lstNewList = new List<StudentPhoto>();
                        foreach (var file in files)
                        {
                            string regNo = file.Name.Substring(0, file.Name.LastIndexOf("."));

                            if (mbUseSerialNumberOption)
                            {
                                //var arrNo = dtData.AsEnumerable().Where(dr => "DSC_" + dr.Field<string>("Photo") == regNo).Select(dr => dr.Field<string>("GRNO")).FirstOrDefault();
                                var arrNo = dtData.AsEnumerable().Where(dr => dr.Field<string>("Photo") == regNo).Select(dr => dr.Field<string>("GRNO")).FirstOrDefault();
                                if (arrNo != null)
                                    regNo = arrNo;
                            }

                            var oStudent = lstRegNos.Where(reg => reg.RegNo == regNo).FirstOrDefault();
                            oStudent.PhotoInBinary = ImageToBase64(file.FullName);
                        }

                        oStudentBL.UpdatePhotos(miSchoolId, miAcademicYearId, miUserId, lstRegNos);
                        lblMessage.Text = "Photos are updated successfully !!!";
                        lblMessage.ForeColor = System.Drawing.Color.Blue;
                        lblMessage.Font.Bold = true;
                        tdMessage.Align = "center";
                        txtPath.Text = string.Empty;
                    }
                }
                else
                {
                    lblMessage.Text = "No any valid file is found.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    tdMessage.Align = "left";
                }
            }
            else
            {
                lblMessage.Text = "Please enter correct path.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                tdMessage.Align = "left";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to convert image to binary format.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public byte[] ImageToBase64(string path)
    {
        using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
        {
            using (MemoryStream m = new MemoryStream())
            {
                image.Save(m, image.RawFormat);
                byte[] imageBytes = m.ToArray();
                return imageBytes;
            }
        }
    }

    /// <summary>
    /// This method is used to set attributes.
    /// </summary>
    private void SetAttributes()
    {
        btnClear.Attributes.Add("onclick", "ClearText(); return false;");
        ValSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnBack.PostBackUrl = Constants.S_PAGE_CONTROL_PANEL;
    }

    #endregion
}