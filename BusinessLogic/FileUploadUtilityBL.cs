// *  Modified By: Sharvari
//*  Date      : 1 Oct 2012
//*  Purpose  : Added server side validations on import teacher.

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using Utility;
using BusinessLogic;
using BusinessLogic.Exceptions;
using System.Resources;
using SchoolEntities;
using BusinessLogic.TransportBL;
using System.Globalization;
using DataCommunicator;
using SchoolEntities.Transport;


namespace BusinessLogic
{
    /// <summary>
    /// Purpose :To upload items through excel sheet and save them in item Master.
    /// </summary>
    public class FileUploadUtilityBL
    {
        #region Constants

        const string S_IS_YES = "Yes";
        const string S_EXCEL_FILE_MESSAGE = "Please select the excel file only. Select another file to upload.";
        const string S_FILE_NOT_FOUND_MESSAGE = "The file specified does not exist.";
        const string S_PASSWORD_PROTECTED_FILE_MESSAGE = "The file cannot be uploaded as it is either password-protected or is in use.";
        const string S_DUPLICATE_REG_NO = "Registration number already exists. Please enter another Registration Number in Worksheet at row number(s) : ";
        const string S_DUPLICATE_REG_NO_EXCEL = "Duplicate Registration number(s) exists in file: ";
        const string S_DUPLICATE_ROLL_NO = "Roll numbers already exist. Please enter another Roll Number in Worksheet at row number(s) :";
        const string S_DUPLICATE_ROLL_NO_EXCEL = "Duplicate Roll number(s) exists in file: ";
        const string S_NULL_REG_NO = "Registration Number should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_ROLL_NO = "Roll Number should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_FIRST_NAME = "Student's First Name should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_MOTHER_NAME = "Student's Mother Name should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_DATE_OF_BIRTH = "Student's Date of Birth should not be blank in Worksheet at row number(s) : ";
        const string S_VALID_DOB = "Date of Birth should not be future date in Worksheet at row number(s) : ";
        const string S_FORMAT_DOB = " Date of birth format is wrong in Worksheet at row number(s) : ";
        const string S_CHECK_DOB = "Student's Date of Birth should not be greater than admission date in Worksheet at row number(s) : ";
        const string S_VALID_ADMISSION_DATE = "Student's Admission date is a future date in Worksheet at row number(s) : ";
        const string S_VALID_JOINING_DATE = "Joining Date should be greater than or equal to Admission Date in Worksheet at row number(s) : ";
        const string S_NULL_ADMISSION_DATE = "Student's Admission Date should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_JOINING_DATE = "Student's Joining Date should not be blank in Worksheet at row number(s) : ";
        const string S_FORMAT_ADMISSION_DATE = "Admission date format is wrong in Worksheet at row number(s) : ";
        const string S_FORMAT_JOINING_DATE = "Joining date format is wrong in Worksheet at row number(s) : ";
        const string S_NULL_SEX = "Student's Sex should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_PARENT_NAME = "Student's Parent Name should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_PARENT_OCCUPATION = "Student's Parent Occupation should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_ADDRESS = "Student's Address should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_CITY = "Student's City should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_STATE = "Student's State should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_PINCODE = "Student's Pincode should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_MOBILE = "Student's Mobile should not be blank in Worksheet at row number(s) : ";
        const string S_VALID_MOBILE_1 = "Student's Mobile number 1 should be of 10 digits in Worksheet at row number(s) : ";
        const string S_VALID_MOBILE_2 = "Student's Mobile number 2 should be of 10 digits in Worksheet at row number(s) : ";
        const string S_VALID_MOBILE_START_NUMBER_1 = "Student's Mobile number 1 should not start with zero in Worksheet at row number(s) : ";
        const string S_VALID_MOBILE_START_NUMBER_2 = "Student's Mobile number 2 should not start with zero in Worksheet at row number(s) : ";
        const string S_T_VALID_MOBILE = "Mobile number should be of 10 digits in Worksheet at row number(s) : ";
        const string S_FORMAT_PINCODE = "Pincode format is wrong in Worksheet at row number(s) : ";
        const string S_FORMAT_MOBILE = "Mobile number format is wrong in Worksheet at row number(s) : ";
        const string S_FORMAT_MOBILE_1 = "Mobile number 1 format is wrong in Worksheet at row number(s) : ";
        const string S_FORMAT_MOBILE_2 = "Mobile number 2 format is wrong in Worksheet at row number(s) : ";
        const string S_VALID_PINCODE = "Student's pincode should be of 6 digits in Worksheet at row number(s) : ";
        const string S_NO_RECORD_FOUND = "File to be imported should not be empty";
        const string S_NO_NEW_ADDMISSION = "Is New Addmission should not be blank in Worksheet at row number(s) : ";
        const string S_NO_RTE_AND_APPLICABLERULE = "Please select either applicable rule or RTE student " + S_COMMON;
        const string S_JOINING_DATE_OUTOF_ACA_YEAR = "Joining date must be ";
        const string S_NEW_STUDENT = "greater than or equal to academic year start date(i.e. ";
        const string S_EXIST_STUDENT = "less than or equal to academic year end date(i.e. ";
        const string S_COMMON = " in Worksheet at row number(s) :";
        const string S_DUPLICATE_STUDENT = "Student(s) already exist at row number(s) ";
        const string S_DUPLICATE_STUDENT_EXCEL = "Student(s) already exist in worksheet at row number(s): ";
        const string S_VALID_PASS_YR = "Year of Passing is a future date in Worksheet at row number(s) : ";
        const string S_VALID_PASS_YR_LEN = " Year of passing  should be of 4 digits at row number(s) :";
        const string S_VALID_YEAR_OF_PASS = "Invalid year of passing at row number(s) :";
        const string S_VALID_LOGIN = "User Name should be of minimum 6 characters, maximum 15 and it accepts only alphanumeric characters, an underscore (_) and a dot (.) characters at row number(s) : ";
        const string S_VALID_PWD = "Password should be of minimum 6 characters, maximum 15 characters at row number(s):";
        const string S_VALID_PASSWORD = "Password should be combination of at least one character, digit & special character at row number(s):";
        const string S_EMAIL_ADDR = "E-mail should be in valid format (For Example :\"john.smith@yahoo.com\") at row number(s) : ";
        const string S_VALID_LAST_SCHOOL_JOINING_DATE = "Last School Left Date should be greater than or equal to Last School Joining Date in Worksheet at row number(s) : ";

        const string S_VALID_JOINING_DATE1 = "Please select valid Joining Date in Worksheet at row number(s) : ";
        const string S_VALID_PERMANENT_DATE = "Please select valid Permanent Date in Worksheet at row number(s) : ";
        const string S_VALID_RESIGNATION_DATE = "Please select valid Resignation Date in Worksheet at row number(s) : ";
        const string S_SELECT_JOINING_DATE = "Please select a Joining Date in Worksheet at row number(s) : ";
        const string S_VALID_JOINING_PERMANENT_DATE = "Permanent Date should be greater than or equal to Joining Date in Worksheet at row number(s) : ";
        const string S_VALID_JOINING_RESIGNATION_DATE = "Resignation Date should be greater than Permanent Date and Joining Date in Worksheet at row number(s) : ";
        const string S_VALID_BIRTH_JOINING_DATE = "Teacher's Date of Birth should not be greater than Joining Date in Worksheet at row number(s) : ";
        const string S_VALID_PAN_NO = "Pan No. should be less that 20 characters in Worksheet at row number(s) : ";

        const string S_EMERGENCY_NO = "ErrorMsgEmergencyContantNo";
        const string S_DUPLICATE_STUDENT_ID = "Student ID already exist. Please enter another Student ID  in Worksheet at row number(s) :";
        const string S_DUPLICATE_STUDENT_ID_EXCEL = "Duplicate Student ID(s) exists in file: ";
        const string S_DUPLICATE_STUDENT_ID_SYSTEM = "Student ID already exist in System.";
        const string S_DUPLICATE_GENERAL_REGISTRATION_NUMBER = "General Registration Number(s) already exist. Please enter another General Registration Number(s)  in Worksheet at row number(s) :";
        const string S_DUPLICATE_GENERAL_REGISTRATION_NUMBER_EXCEL = "Duplicate General Registration Number(s) exists in file: ";
        const string S_DUPLICATE_GENERAL_REGISTRATION_NUMBER_SYSTEM = "General Registration Number(s) already exist in system.";

        const int I_XLS_REG_NO = 0;
        const int I_XLS_ROLL_NO = 1;
        const int I_XLS_FIRST_NAME = 2;
        const int I_XLS_MIDDLE_NAME = 3;
        const int I_XLS_LAST_NAME = 4;
        const int I_XLS_MOTHER_NAME = 5;
        const int I_XLS_DATE_OF_BIRTH = 6;
        const int I_XLS_ADMISSION_DATE = 7;
        const int I_XLS_JOINING_DATE = 8;
        const int I_XLS_SEX = 9;
        const int I_XLS_PARENT_NAME = 11;
        const int I_XLS_PARENT_OCCUPATION = 12;
        const int I_XLS_ADDRESS = 13;
        const int I_XLS_CITY = 14;
        const int I_XLS_STATE = 15;
        const int I_XLS_PINCODE = 16;
        const int I_XLS_MOBILE = 18;
        const int I_XLS_CATEGORY = 19;
        const int I_XLS_CASTE_SUB_CASTE = 20;
        const int I_XLS_APPLICABLE_RULE = 22;
        const int I_XLS_IS_RTE_STUDENT = 24;
        const int I_XLS_MOBILE_2 = 25;
        const int I_XLS_NO_DATA_IN_TABLE = 64;
        const int I_XLS_NO_NEW_ADDMISSION = 21;
        const int I_XLS_RTECATEGORY = 32;
        const int I_XLS_SEMAIL = 33;
        const int I_XLS_STUDENT_UNIQUE_NUMBER = 64;
        const int I_XLS_GENERAL_REG_NO = 65;
        const int I_XLS_STUDENTCATEGORY = 67;

        const string S_NULL_SAL_ID = "Salutation should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_T_FIRST_NAME = "Teacher's First Name should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_DESGN = "Teacher's Designtation should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_IS_TEMP = "Teacher's Service Type should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_T_MOBILE = "Teacher's Mobile should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_T_DOB = "Teacher's Date of Birth should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_NATION = "Teacher's Nationality should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_RELIGION = "Teacher's Religion should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_L_ADDRESS = "Teacher's Local Address should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_L_CITY = "Teacher's City should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_L_STATE = "Teacher's State should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_L_PIN = "Pincode should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_QUALI = "Teacher's Qualification should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_PASS_YR = "Teacher's Year of Passing should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_CLASS = "Teacher's Class should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_UNI = "Teacher's University should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_STD_ID = "Teacher's Std ID should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_SUB_ID = "Teacher's Sub ID should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_EMAIL = "Teacher's E-mail should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_LOGIN = "Teacher's Login should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_PWD = "Teacher's Password should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_CATEGORY = "Category should be selected at row number(s) : ";
        const string S_NULL_RTECATEGORY = "RTE Category should be selected at row number(s) : ";
        const string S_MATCH_CATEGORYANDRTECATEGORY = "Category and RTE Category should be same at row number(s) : ";
        const string S_NULL_LAST_SCHOOL_NAME = "Last School name should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_LAST_SCHOOL_JOINED_DATE = "Last School Joined date should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_LAST_SCHOOL_LEFT_DATE = "Last School Left date should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_T_EMERGENCY_CONTACT = "Teacher's Emergency Contact should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_ASSO_STANDARD_CATEGPRY = "Teachers' Associated Standard Category should not be blank in Worksheet at row number(s) : ";


        const string S_VALID_SAL_ID = "Valid salutation should be selected from the list in Worksheet at row number(s) : ";
        const string S_VALID_T_FIRST_NAME = "Teacher's First Name should be between 1 to 50 Characters in Worksheet at row number(s) : ";
        const string S_VALID_T_MID_NAME = "Teacher's Middle Initial should be Maximum of 1 Character in Worksheet at row number(s) : ";
        const string S_VALID_T_LAST_NAME = "Teacher's Last Name should be between 1 to 50 Characters in Worksheet at row number(s) : ";
        const string S_VALID_DESGN = "Valid Designtation should be selected from the list in Worksheet at row number(s) : ";
        const string S_VALID_IS_TEMP = "Valid Service Type should be selected from the list in Worksheet at row number(s) : ";
        const string S_VALID_T_PHONE_NUMBER = "Teacher's Phone Number should be between 1 to 15 Digits number in Worksheet at row number(s) : ";
        const string S_VALID_T_MOBILE = "Teacher's Mobile should be 10 digit number in Worksheet at row number(s) : ";
        const string S_VALID_T_DOB = "Teacher's Date of Birth should be Less than today's date and Age should be greater than 18 years in Worksheet at row number(s) : ";
        const string S_VALID_NATION = "Teacher's Nationality should be between 1 to 50 Characters in Worksheet at row number(s) : ";
        const string S_VALID_RELIGION = "Valid Religion should be selected from the list in Worksheet at row number(s) : ";
        const string S_VALID_CASTE = "Teacher's Caste & Subcaste should be between 1 to 100 Characters in Worksheet at row number(s) : ";
        const string S_VALID_CATEGORY = "Valid Category should be selected from the list in Worksheet at row number(s) : ";
        const string S_VALID_L_ADDRESS = "Teacher's Local Address should not be between 1 to 200 Characters in Worksheet at row number(s) : ";
        const string S_VALID_L_CITY = "Teacher's Local City should be between 1 to 50 Characters in Worksheet at row number(s) : ";
        const string S_VALID_L_STATE = "Teacher's Local State should be between 1 to 50 Characters in Worksheet at row number(s) : ";
        const string S_VALID_L_PIN = "Local Pincode should be of 6 digits number in Worksheet at row number(s) : ";
        const string S_VALID_P_ADDRESS = "Teacher's Permanent Address should not be between 1 to 200 Characters in Worksheet at row number(s) : ";
        const string S_VALID_P_CITY = "Teacher's Permanent City should be between 1 to 50 Characters in Worksheet at row number(s) : ";
        const string S_VALID_P_STATE = "Teacher's Permanent State should be between 1 to 50 Characters in Worksheet at row number(s) : ";
        const string S_VALID_P_PIN = "Permanent Pincode should be of 6 digits number in Worksheet at row number(s) : ";
        const string S_VALID_PAST_EXP_YEARS = "Teacher's Past Experience Years should be of 2 digits number in Worksheet at row number(s) : ";
        const string S_VALID_PAST_EXP_MONTHS = "Teacher's Past Experience Months should be of 2 digits number in Worksheet at row number(s) : ";
        const string S_VALID_ACHIEVEMENTS = "Teacher's Achievements should be between 1 to 4000 Characters in Worksheet at row number(s) : ";
        const string S_VALID_QUALI = "Valid Qualification should be selected from the list in Worksheet at row number(s) : ";
        const string S_VALID_T_PASS_YR = "Teacher's Year of Passing should be of 4 digits year in Worksheet at row number(s) : ";
        const string S_VALID_CLASS = "Valid Class should be selected from the list in Worksheet at row number(s) : ";
        const string S_VALID_UNI = "Teacher's University should be between 1 to 100 Characters in Worksheet at row number(s) : ";
        const string S_VALID_STD_ID = "Teacher's Std ID should be between 1 to 5 Digits number in Worksheet at row number(s) : ";
        const string S_VALID_SUB_ID = "Teacher's Sub ID should be between 1 to 5 Digits number in Worksheet at row number(s) : ";
        const string S_VALID_EMAIL = "Teacher's E-mail should be between 1 to 50 Characters in Worksheet at row number(s) : ";
        const string S_VALID_T_LOGIN = "Teacher's Login should be between 6 to 20 Characters in Worksheet at row number(s) : ";
        const string S_VALID_T_PWD = "Teacher's Password should be between 6 to 15 Characters in Worksheet at row number(s) : ";
        const string S_VALID_LAST_SCHOOL_NAME = "Last School name should be between 1 to 50 Characters in Worksheet at row number(s) : ";
        const string S_VALID_T_EMERGENCY_CONTACT = "Teacher's Emergency Contact Number should be between 1 to 15 Digits number in Worksheet at row number(s) : ";
        const string S_VALID_T_PANNO = "Teacher's PAN Number should be between 1 to 20 Characters in Worksheet at row number(s) : ";

        const string S_DUPLICATE_USER = "Login Name already exists. Please enter another Login Name in Worksheet at row number(s) : ";
        const string S_DUPLICATE_EMAIL = "Email Address already exists. Please enter another Login Name in Worksheet at row number(s) : ";
        const string S_MINOR_DOB = "Teacher's age should be greater than 18 in Worksheet at row number(s) : ";
        const string S_DUPLICATE_DESGN = "Designation Name 'Principal' already exists. Please enter another Designation Name in Worksheet at row number(s) : ";

        // Constants for challan

        private const int I_XLS_C_CHALLANNO = 0;
        private const int I_XLS_C_AMOUNT = 1;
        private const int I_XLS_C_PAIDDATE = 2;
        private const int I_XLS_C_CHQUENO = 3;
        private const int I_XLS_C_BANKNAME = 4;
        private const int I_XLS_C_CHEQUEDATE = 5;
        const string S_INVALID_CHALLAN_NO = "Invalid Challan No in Worksheet at row number(s) : ";
        const string S_INVALID_CHEQUE_NO = "Invalid Cheque No in Worksheet at row number(s) : ";
        const string S_DUPLICATE_CHALLAN_NO = "Challan No should not be duplicate in Worksheet at row number(s) : ";
        const string S_INVALID_CHEQUE_PAID_DATE = "Paid date should not a less than cheque date";
        const string S_INVALID_PAID_DATE = "Paid date should not be a future date";
        const string S_INVALID_AMOUNT = "Amount should not be a Zero or Null";
        const string S_INVALID_BANKNAME = "Bank Name should not be null";
        const string S_CHEQUE_DATE = "Cheque date should not be blank";

        //Constants for reading allocations

        private const string I_XLS_C_VEHICLENO = "0";
        private const string I_XLS_C_READINGDATE = "1";
        private const string I_XLS_C_RECEIPTNO = "2";        
        private const string I_XLS_C_READINGFROM = "3";
        private const string I_XLS_C_READINGTO = "4";
        private const string I_XLS_C_LITTERS = "5";
        private const string I_XLS_C_PERLITTERCOST = "6";
        private const string I_XLS_C_TOT_COST = "7";


        private const string I_XLS_C_MAINT_VEHICLENO = "0";
        private const string I_XLS_C_MAINT_DATE = "1";
        private const string I_XLS_C_MAINT_BILLDATE = "2";
        private const string I_XLS_C_MAINT_EXPDATE = "3";
        private const string I_XLS_C_MAINT_METERREADING = "4";
        private const string I_XLS_C_MAINT_BILLNO = "5";
        private const string I_XLS_C_MAINT_WORKSHOP = "6";
        private const string I_XLS_C_MAINT_WORK_DETAILS = "7";
        private const string I_XLS_C_MAINT_LABOUR_CHARGES = "8";
        private const string I_XLS_C_MAINT_MAINT_TYPE = "9";
        private const string I_XLS_C_MAINT_PARTS = "10";
        private const string I_XLS_C_MAINT_QTY = "11";
        private const string I_XLS_C_MAINT_RATE = "12";
        private const string I_XLS_C_MAINT_CHARGES = "13";
        private const string I_XLS_C_MAINT_TOTAL_AMT = "14";

        const string S_BLANK_VEHICLE_NO = "Vehicle No. should not be blank in worksheet at row number(s) :";
        const string S_BLANK_READING_DATE = "Reading Date should not be blank in worksheet at row number(s) :";
        const string S_BLANK_RECEIPT_NUMBER = "Receipt No. should not be blank or zero in worksheet at row number(s) :";
        const string S_BLANK_READING_TO = "Value for 'Reading To' should not be blank in worksheet at row number(s) :";
        const string S_BLANK_LITTERS = "Value for 'Litters' should not be blank in worksheet at row number(s) :";
        const string S_BLANK_PER_LITTER_COST = "Value for 'Per Litter Cost' should not be blank in worksheet at row number(s) :";
        const string S_BLANK_TOTAL_COST = "Value for 'Total Cost' should not be blank in worksheet at row number(s) :";
        const string S_VALID_VEHICLE_NO = "Vehicle No. should be valid in worksheet at row number(s) :";
        const string S_VALID_READING_DATE = "Reading Date should be valid in worksheet at row number(s) :";
        const string S_VALID_FUTURE_READING_DATE = "Reading Date should not be future date in worksheet at row number(s) :";

        const string S_VALID_RECEIPT_NUMBER = "Receipt NUmber should be in valid format in worksheet at row number(s) :";
        const string S_VALID_READING_FROM = "Value of 'Reading From' should be in valid format in worksheet at row number(s) :";
        const string S_VALID_READING_TO = "Value of 'Reading To' should be in valid format in worksheet at row number(s) :";
        const string S_VALID_LITTERS = "Value of 'Litters' should be in valid format in worksheet at row number(s) :";
        const string S_VALID_PER_LITTERS = "Value of 'Per Litter Cost' should be in valid format in worksheet at row number(s) :";
        const string S_VALID_TOTAL_COST = "Value of 'Total Cost' should be in valid format in worksheet at row number(s) :";

        const string S_BLANK_MAINT_DATE = "Maintenance Date should not be blank in worksheet at row number(s) :";
        const string S_BLANK_MAINT_BILL_DATE = "Bill Date should not be blank in worksheet at row number(s) :";
        const string S_BLANK_MAINT_BILL_NO = "Bill No. should not be blank in worksheet at row number(s) :";
        const string S_BLANK_MAINT_WORKSHOP_NAME = "Workshop Name should not be blank in worksheet at row number(s) :";
        const string S_BLANK_MAINT_TYPE = "Maintenance Type should not be blank in worksheet at row number(s) :";
        const string S_BLANK_MAINT_TOTAL = "Total Amount should not be blank in worksheet at row number(s) :";

        const string S_VALID_MAINT_DATE = "Maintenance Date should be valid in worksheet at row number(s) :";
        const string S_VALID_MAIN_FUTURE_DATE = "Maintenance Date should not be future date in worksheet at row number(s) :";

        const string S_VALID_MAINT_BILL_DATE = "Bill Date should be valid in worksheet at row number(s) :";
        const string S_VALID_MAIN_FUTURE_BILL_DATE = "Bill Date should not be future date in worksheet at row number(s) :";
        const string S_VALID_MAINT_EXPIRY_DATE = "Expiry Date should be valid in worksheet at row number(s) :";

        const string S_VALID_MAINT_METER_READING = "Value of 'Meter Reading' should be in valid format in worksheet at row number(s) :";        
        const string S_VALID_MAINT_LABOUR_CHARGES = "Value of 'Labour Charges' should be in valid format in worksheet at row number(s) :";
        const string S_VALID_MAINT_RATE = "Rate should be in valid format in worksheet at row number(s) :";
        const string S_VALID_MAINT_CHARGES = "Charges should be in valid format in worksheet at row number(s) :";
        const string S_VALID_MAINT_TOTAL_AMT = "Total Amount should be in valid format in worksheet at row number(s) :";
        const string S_VALID_MAINT_TYPE = "Value of 'Maintenance Type' should be in valid format in worksheet at row number(s) :";

        // Constants for teacher
        const int I_XLS_T_SAL_ID = 0;
        const int I_XLS_T_FIRST_NAME = 1;
        const int I_XLS_T_MIDDLE_NAME = 2;
        const int I_XLS_T_LAST_NAME = 3;
        const int I_XLS_T_DEGN_ID = 4;
        const int I_XLS_T_IS_TEMP = 5;
        const int I_XLS_T_PHONE = 6;
        const int I_XLS_T_MOBILE = 7;
        const int I_XLS_T_DOB = 8;
        const int I_XLS_T_NATION = 9;
        const int I_XLS_T_RELIGION = 10;
        const int I_XLS_T_CASTE_SUB_CASTE = 11;
        const int I_XLS_T_CATEGORY = 12;
        const int I_XLS_T_L_ADDRESS = 13;
        const int I_XLS_T_L_CITY = 14;
        const int I_XLS_T_L_STATE = 15;
        const int I_XLS_T_L_PINCODE = 16;
        const int I_XLS_T_P_ADDRESS = 17;
        const int I_XLS_T_P_CITY = 18;
        const int I_XLS_T_P_STATE = 19;
        const int I_XLS_T_P_PINCODE = 20;

        const int I_XLS_T_EXP_YR = 21;
        const int I_XLS_T_EXP_MON = 22;
        const int I_XLS_T_JOINDATE = 23;
        const int I_XLS_T_ACHIEVE = 24;
        const int I_XLS_T_QUALI = 25;
        const int I_XLS_T_PASS_YR = 26;
        const int I_XLS_T_CLASS = 27;
        const int I_XLS_T_UNI = 28;
        const int I_XLS_T_STD_ID = 29;
        const int I_XLS_T_SUB_ID = 30;
        const int I_XLS_T_EMAIL = 31;
        const int I_XLS_T_LOGIN = 32;
        const int I_XLS_T_PWD = 33;
        const int I_XLS_P_EXP_SCHOOL_NAME = 34;
        const int I_XLS_P_EXP_SCHOOL_JOINED_DATE = 35;
        const int I_XLS_P_EXP_SCHOOL_LEFT_DATE = 36;
        const int I_XLS_T_EMERGENCY_NO = 37;
        const int I_XLS_T_PAN_NO = 38;
        const int I_XLS_T_PERMANENT_DATE = 39;
        const int I_XLS_T_RESIGNATION_DATE = 40;
        const int I_XLS_ASSO_STANDARD_CATEGORY = 41;
        const int I_XLS_IS_ON_CHB = 42;

        // Constants for supervisor(Admin Staff)
        const int I_XLS_AS_SAL_ID = 0;
        const int I_XLS_AS_FIRST_NAME = 1;
        const int I_XLS_AS_MIDDLE_NAME = 2;
        const int I_XLS_AS_LAST_NAME = 3;
        const int I_XLS_AS_DOB = 4;
        const int I_XLS_AS_DEGN_ID = 5;
        const int I_XLS_AS_EMAIL = 6;
        const int I_XLS_AS_MOBILE = 7;
        const int I_XLS_AS_EMRGENCY_NO = 8;
        const int I_XLS_AS_ADDRESS = 9;
        const int I_XLS_AS_LOGIN = 10;
        const int I_XLS_AS_PWD = 11;
        const int I_XLS_AS_PAN_NO = 12;
        const int I_XLS_AS_JOINING_DATE = 13;
        const int I_XLS_AS_PERMANENT_DATE = 14;
        const int I_XLS_AS_RESIGNATION_DATE = 15;

        const string S_NULL_AS_FIRST_NAME = "First Name should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_AS_MIDDLE_NAME = "Middle Name should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_AS_LAST_NAME = "Last Name should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_AS_DESGN = "Designtation should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_AS_MOBILE = "Mobile should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_AS_EMAIL = "Email should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_AS_LOGIN = "Login should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_AS_PWD = "Password should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_AS_EMERGENCY_CONTACT = "Emergency contact number should not be blank in Worksheet at row number(s) : ";
        const string S_NULL_AS_ADDRESS = "Address should not be blank in Worksheet at row number(s) : ";
        const string S_AS_MINOR_DOB = "Age should be greater than 18 in Worksheet at row number(s) : ";
        const string S_AS_VALID_LOGIN = "User Name should be of minimum 6 characters, maximum 20 and it accepts only alphanumeric characters, an underscore (_) and a dot (.) characters at row number(s) : ";

        const string S_ELEMENT = "element";
        const string S_STUDENT = "Student";


        #endregion

        #region Data Members

        private string msSourceFileName = "";
        private string msServerFilePath = "";
        private string msServerFolderPath = "";
        private Constants.UploadFileType meFileType;
        private string sFileRowNumber = "";
        private DateTime mdtAcademicYearEndDate;
        private DateTime mdtAcademicYearStartDate;
        private bool mbCanPublishUnpublishExam;
        private StringBuilder msImportedStudentsRegNumbers = new StringBuilder();
        private struct StudentInfo
        {
            public int iSchoolId;
            public int iAcademicYearId;
            public int iUserId;
            public int iStandardId;
            public int iDivisionId;
            public int iPostFixLength;
            public string sRegPrefix;
            public string sRegPostfix;
            public bool bIsConcessionApplicable;
            public bool bIsRTEApplicable;
        };
        StudentInfo moStudentInfoStruct;
        Random moRandomNo = new Random((int)DateTime.Now.Ticks);
        private DataTable mdtCategory;

        #endregion

        #region Properties

        public int SchoolId
        {
            set { moStudentInfoStruct.iSchoolId = value; }
        }

        public int AcademicYearId
        {
            set { moStudentInfoStruct.iAcademicYearId = value; }
        }

        public int UserId
        {
            set { moStudentInfoStruct.iUserId = value; }
        }
        public int PostFixLength
        {
            get { return moStudentInfoStruct.iPostFixLength; }
            set { moStudentInfoStruct.iPostFixLength = value; }
        }
        public int StandardId
        {
            set { moStudentInfoStruct.iStandardId = value; }
        }

        public int DivisionId
        {
            set { moStudentInfoStruct.iDivisionId = value; }
        }

        public string RegistrationPrefix
        {
            set { moStudentInfoStruct.sRegPrefix = value; }
        }
        public string RegistrationPostfix
        {
            set { moStudentInfoStruct.sRegPostfix = value; }
        }
        public DateTime AcademicYearEndDate
        {
            set { mdtAcademicYearEndDate = value; }
        }

        public DateTime AcademicYearStartDate
        {
            set { mdtAcademicYearStartDate = value; }
        }

        public String ImportedStudentsRegNumbers
        {
            get
            {
                return msImportedStudentsRegNumbers.Length > 1 ? msImportedStudentsRegNumbers.Remove(msImportedStudentsRegNumbers.Length - 1, 1).ToString() : string.Empty;
            }
        }
        public bool bIsConcessionApplicable
        {
            set { moStudentInfoStruct.bIsConcessionApplicable = value; }
        }

        public bool bIsRTEApplicable
        {
            get { return moStudentInfoStruct.bIsRTEApplicable; }
            set { moStudentInfoStruct.bIsRTEApplicable = value; }
        }

        public bool CanPublishUnpublishExam
        {
            set { mbCanPublishUnpublishExam = value; }
        }

        public List<int> RTEStudentIDs { get; set; }


        #endregion

        /// <summary>
        /// Constructor will accept the excel file name containing the item list.
        /// </summary>
        /// <param name="asFileName"></param>
        public FileUploadUtilityBL(string asSourceFileName, string asServerFolderPath, Constants.UploadFileType aeUploadFileType)
        {
            msSourceFileName = asSourceFileName;
            meFileType = aeUploadFileType;

            msServerFilePath = asServerFolderPath;

            if (!(ValidateFileExtentionForFile()))
            {
                Exception ex = new Exception(S_EXCEL_FILE_MESSAGE);
                throw ex;
            }

        }

        /// <summary>
        /// This function will upload the excel sheet on the server.
        /// This will also store all the Item details in the database table employee_m.
        /// </summary>
        /// <returns></returns>
        public string UploadFile()
        {
            string sErrorMessage = "";
            // Validate the uploaded file.
            if (!(ValidateFileExtentionForFile())) { return S_EXCEL_FILE_MESSAGE; }

            // if the file exists or not.
            if (meFileType.ToString().Trim() == Constants.UploadFileType.Student.ToString())
            {
                // Employee file upload.
                SaveRecordsIfValidFileUploaded();
            }
            if (meFileType.ToString().Trim() == Constants.UploadFileType.RFID.ToString())
            {
                // RFID file upload.
                sErrorMessage = SaveStudentsRFIDIfValidFileUploaded();
            }
            else if (meFileType.ToString().Trim() == Constants.UploadFileType.Teacher.ToString())
            {
                // Employee file upload.
                SaveTeachersIfValidFileUploaded();
            }
            else if (meFileType.ToString().Trim() == Constants.UploadFileType.Supervisor.ToString())
            {
                // Supervisor file upload.
                SaveAdminStaffIfValidFileUploaded();
            }
            else if (meFileType.ToString().Trim() == Constants.UploadFileType.CautionMoney.ToString())
            {
                // Caution Money file upload.
                sErrorMessage = SaveCautionMoney();

            }
            else if (meFileType.ToString().Trim() == Constants.UploadFileType.Fee.ToString())
            {
                // Caution Money file upload.
                sErrorMessage = SaveFee();
            }
            else if (meFileType.ToString().Trim() == Constants.UploadFileType.StudentHealth.ToString())
            {
                sErrorMessage = SaveStudentHealthDetails();
            }

            return sErrorMessage;
        }

        /// <summary>
        /// This function checks if the extention of the file to be uploaded is .XLS
        /// Reason - only excel files can be uploaded for employee type file upload.
        /// </summary>
        /// <returns></returns>
        private bool ValidateFileExtentionForFile()
        {
            return (msSourceFileName.Trim().ToUpper().EndsWith(".XLS") || msSourceFileName.Trim().ToUpper().EndsWith(".XLSX"));
        }

        /// <summary>
        /// Save records from excel sheet to database.
        /// </summary>
        private void SaveRecordsIfValidFileUploaded()
        {
            SaveRecordDetails();
        }

       private void SaveFileOnServer()
        {
            msServerFilePath = msServerFolderPath;
        }

        private void SaveRecordDetails()
        {
            // This procedure accepts parameter as aoAgentBusinessUploadBL. 

            // Get dataset containing employee details. The dataset is created from the excel sheet uploaded by process mgr.
            DataSet oDSStudentDetails = CommonUtility.ReadExcelSheetAndFetchData(msServerFilePath, "", "Student Data");

            DataTable oDTStudents = oDSStudentDetails.Tables[0].Copy();
            oDTStudents = CommonUtility.DeleteEmptyRows(oDTStudents);
            // Check if data is loaded in dataset successfully.
            if (oDTStudents.Rows.Count >= 1)
            {
                int iSchoolId = moStudentInfoStruct.iSchoolId;
                int iAcademicYearId = moStudentInfoStruct.iAcademicYearId;
                int iInsertedById = moStudentInfoStruct.iUserId;
                int iStandardId = moStudentInfoStruct.iStandardId;
                int iDivisionId = moStudentInfoStruct.iDivisionId;

                string sStudentDetails = GetXMLStringFromXLSRows(oDTStudents, "StudentDetails", "StudentDetail");

                StudentCollectionBL oStudentCollectionBL = new StudentCollectionBL();
                string sRTEStudIDs = oStudentCollectionBL.InsertMultipleStudents(iSchoolId, iAcademicYearId, iInsertedById,
                                                            iStandardId, iDivisionId, sStudentDetails);


                if (sRTEStudIDs.Trim() != string.Empty)
                {
                    if (sRTEStudIDs.Trim().StartsWith(","))
                        sRTEStudIDs = sRTEStudIDs.Substring(1);
                    List<int> lstIDs = sRTEStudIDs.Split(',').Select(x => Convert.ToInt32(x)).ToList();

                    RTEStudentIDs = lstIDs;

                }
            }
            else
            {
                ThrowAppropriateException(I_XLS_NO_DATA_IN_TABLE, "0");
            }
        }

        private void SaveTeachersIfValidFileUploaded()
        {
            // This procedure accepts parameter as aoAgentBusinessUploadBL. 

            // Get dataset containing employee details. The dataset is created from the excel sheet uploaded by process mgr.
            DataSet oDSTeacherDetails = CommonUtility.ReadExcelSheetAndFetchData(msServerFilePath, "", "Teacher Data");

            DataTable oDTTeachers = oDSTeacherDetails.Tables[0].Copy();
            oDTTeachers = CommonUtility.DeleteEmptyRows(oDTTeachers);

            // Check if data is loaded in dataset successfully.
            if (oDTTeachers.Rows.Count >= 1)
            {
                int iSchoolId = moStudentInfoStruct.iSchoolId;
                int iAcademicYearId = moStudentInfoStruct.iAcademicYearId;
                int iInsertedById = moStudentInfoStruct.iUserId;
                string sTeacherDetails = GetTeacherXMLStringFromXLSRows(oDTTeachers, "TeacherDetails", "TeacherDetail");


                SchoolWiseTeacherMasterCollectionBL oTeacherCollectionBL = new SchoolWiseTeacherMasterCollectionBL();
                oTeacherCollectionBL.InsertMultipleTeachers(iSchoolId, iAcademicYearId, iInsertedById, sTeacherDetails, mbCanPublishUnpublishExam);

            }
            else
                ThrowAppropriateException(I_XLS_NO_DATA_IN_TABLE, "0");
          }

    private void SaveAdminStaffIfValidFileUploaded()
        {
            // This procedure accepts parameter as aoAgentBusinessUploadBL. 

            // Get dataset containing employee details. The dataset is created from the excel sheet uploaded by process mgr.
            DataSet oDSAdminStaffDetails = CommonUtility.ReadExcelSheetAndFetchData(msServerFilePath, "", "Admin staff Data");

            DataTable oDTAdminStaff = oDSAdminStaffDetails.Tables[0];
            oDTAdminStaff = CommonUtility.DeleteEmptyRows(oDTAdminStaff);
            // Check if data is loaded in dataset successfully.
            if (oDTAdminStaff.Rows.Count >= 1)
            {
                int iSchoolId = moStudentInfoStruct.iSchoolId;
                int iAcademicYearId = moStudentInfoStruct.iAcademicYearId;
                int iInsertedById = moStudentInfoStruct.iUserId;

                string sAdminStaffDetails = GetAdminStaffXMLStringFromXLSRows(oDTAdminStaff, "AdminStaffDetails", "AdminStaffDetail");


                SchoolWiseSupervisorMasterCollectionBL oSchoolWiseSupervisorMasterCollectionBL = new SchoolWiseSupervisorMasterCollectionBL();
                oSchoolWiseSupervisorMasterCollectionBL.InsertMultipleAdminStaff(iSchoolId, iAcademicYearId, iInsertedById,
                                                             sAdminStaffDetails, mbCanPublishUnpublishExam);


            }
            else
                ThrowAppropriateException(I_XLS_NO_DATA_IN_TABLE, "0");
        }

        /// <summary>
        /// This method is used to upload fee challan no.'s details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUpdatedById"></param>
        public void UploadChallanDetails(int miSchoolId, int miAcademicYearId, int miUserId, int aiOriginalFeeTypeId, int aiFinancialYearId)
        {

            ImportFeeByChallanBL moImportFeeByChallanBL = new ImportFeeByChallanBL(miSchoolId, miAcademicYearId, miUserId, aiFinancialYearId);
            DataSet oDSChallanDetails = CommonUtility.ReadExcelSheetAndFetchData(msServerFilePath, "", "Challan Data");
            DataTable oDTChallanNos = oDSChallanDetails.Tables[0];
            oDTChallanNos = CommonUtility.DeleteEmptyRows(oDTChallanNos);

            if (oDTChallanNos.Rows.Count >= 1)
            {
                string sChallanDetails = GetChallanXMLStringFromXLSRows(oDTChallanNos, "ChallanDetails", "ChallanDetail");
                if (ReadyToUpload(sChallanDetails, oDTChallanNos, miSchoolId, miAcademicYearId, aiOriginalFeeTypeId))
                    moImportFeeByChallanBL.InsertFeeByChallan(sChallanDetails, aiOriginalFeeTypeId);
            }
            else
                ThrowAppropriateException(I_XLS_NO_DATA_IN_TABLE, "0");
        }

        public void UploadTransportAllocationDetails(int aischoolId, int aiAcademicYearId, int aiUserId)
        {
            VehicleReadingAllocationBL moVehicleReadingAllocationBL = new VehicleReadingAllocationBL();
            DataSet oDsAllocationDetails = CommonUtility.ReadExcelSheetAndFetchData(msServerFilePath, "", "Reading Allocation");
            DataTable ODTAllocations = oDsAllocationDetails.Tables[0];
            ODTAllocations = CommonUtility.DeleteEmptyRows(ODTAllocations);

            if (ODTAllocations.Rows.Count >= 1)
            {   
                if (ReadyToUploadAllocationDetails(ODTAllocations, aischoolId, aiAcademicYearId))
                {
                    string sAllocationDetails = GetAllocationXMLStringFromXLSRows(ODTAllocations, "AllocationDetails", "AllocationDetail");
                    moVehicleReadingAllocationBL.InsertAllocationDetails(sAllocationDetails, aischoolId, aiAcademicYearId, aiUserId);
                }
            }
            else
                ThrowAppropriateException(I_XLS_NO_DATA_IN_TABLE, "0");
        }

        public void UploadMaintenanceDetails(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            VehicleReadingAllocationBL moVehicleReadingAllocationBL = new VehicleReadingAllocationBL();
            DataSet oDsMaintenanceDetails = CommonUtility.ReadExcelSheetAndFetchData(msServerFilePath, "", "Vehicles Maintenance Expenses");
            DataTable oDTMaintenanceDetails = oDsMaintenanceDetails.Tables[0];
            oDTMaintenanceDetails = CommonUtility.DeleteEmptyRows(oDTMaintenanceDetails);

            if (oDTMaintenanceDetails.Rows.Count >= 1)
            {   
                if (ReadyToUploadMaintenanceDetails(oDTMaintenanceDetails, aiSchoolId, aiAcademicYearId))
                {
                    string sMaintenanceDetails = GetMaintenanceXMLStringFromXLSRows(oDTMaintenanceDetails, "MaintenanceDetails", "MaintenanceDetail");
                    moVehicleReadingAllocationBL.InsertMaintenanceDetails(sMaintenanceDetails, aiSchoolId, aiAcademicYearId, aiUserId);
                }
            }
            else
                ThrowAppropriateException(I_XLS_NO_DATA_IN_TABLE, "0");
        }


        private List<string> moArrColumnNames = new List<string>();
        private List<string> moArrFeeColumnNames = new List<string>();

        private void PopulateColumnNames()
        {
            moArrColumnNames.Add("STUDENT NAME");
            moArrColumnNames.Add("GRADE");
            moArrColumnNames.Add("CHQNO");
            moArrColumnNames.Add("BANKNAME");
            moArrColumnNames.Add("BKDATE");
            moArrColumnNames.Add("CREDIT");
        }

        private void PopulateFeeColumnNames()
        {
            moArrFeeColumnNames.Add("REGNO");
            moArrFeeColumnNames.Add("NAME");
            moArrFeeColumnNames.Add("CHQNO");
            moArrFeeColumnNames.Add("BANKNAME");
            moArrFeeColumnNames.Add("AMT");
            moArrFeeColumnNames.Add("PAYMENTDT");
            moArrFeeColumnNames.Add("CLEARANCEDT");
        }

        private string SaveFee()
        {
            string sErrorMessage = "";
            int iBankId = 0;
            DataSet oDSFeeDetails = CommonUtility.ReadExcelSheetAndFetchData(msServerFilePath, "", "Fee");
            DataTable oDTFeeDetails = oDSFeeDetails.Tables[0];
            oDTFeeDetails = CommonUtility.DeleteEmptyRows(oDTFeeDetails);
            if (oDTFeeDetails.Rows.Count == 0)
                sErrorMessage = "There must be atleast one row to import.";
            else
            {
                PopulateFeeColumnNames();
                StringBuilder sInvalidcolumnNames = new StringBuilder();
                foreach (DataColumn oColumn in oDTFeeDetails.Columns)
                {
                    if (!moArrFeeColumnNames.Contains(oColumn.ColumnName.Trim().ToUpper()))
                        sInvalidcolumnNames.Append(oColumn.ColumnName.Trim() + ", ");
                }
                if (sInvalidcolumnNames.ToString().Trim() != string.Empty)
                    sErrorMessage = "Invalid column names : " + sInvalidcolumnNames.ToString().Substring(0, sInvalidcolumnNames.ToString().LastIndexOf(","));
                else
                {
                    int iSchoolId = moStudentInfoStruct.iSchoolId;
                    int iAcademicYearId = moStudentInfoStruct.iAcademicYearId;
                    DataTable oDTAllStudents = StudentBL.GetAllStudents(iSchoolId, 0, 0, iAcademicYearId);
                    DataTable oDTAllBanks = SchoolwiseBankMasterCollectionBL.FetchSchoolwiseBankMasterDetails(iSchoolId);

                    int iRowIndex = 2;
                    List<string> sErrorMessages = new List<string>();

                    foreach (DataRow datarow in oDTFeeDetails.Rows)
                    {
                        if (datarow["RegNo"].ToString().Trim() == "")
                            sErrorMessages.Add("Row " + iRowIndex + ": Reg. No. is blank");
                        if (datarow["Name"].ToString().Trim() == "")
                            sErrorMessages.Add("Row " + iRowIndex + ": Name is blank");
                        if (datarow["ChqNo"].ToString().Trim() == "")
                            sErrorMessages.Add("Row " + iRowIndex + ": ChqNo. is blank");
                        if (datarow["BANKNAME"].ToString().Trim() == "")
                            sErrorMessages.Add("Row " + iRowIndex + ": Bank Name is blank");
                        else
                        {
                            DataRow[] oBankRows = oDTAllBanks.Select("Bank_Name = '" + datarow["BankName"].ToString().Trim() + "'");
                            if (oBankRows.Length == 0)
                                sErrorMessages.Add("Row " + iRowIndex + ": Bank name not found.");
                            else if (oBankRows.Length > 1)
                                sErrorMessages.Add("Row " + iRowIndex + ": Multiple bank names found.");
                        }
                        if (datarow["Paymentdt"].ToString().Trim() == "")
                            sErrorMessages.Add("Row " + iRowIndex + ": Payment Date is blank");
                        if (datarow["Clearancedt"].ToString().Trim() == "")
                            sErrorMessages.Add("Row " + iRowIndex + ": Clearance Date is blank");

                        if (sErrorMessages.Count == 0)
                        {
                            DateTime odtBankDate;
                            try
                            {
                                odtBankDate = DateTime.Parse(datarow["Paymentdt"].ToString().Trim());
                            }
                            catch (FormatException)
                            {
                                sErrorMessages.Add("Row " + iRowIndex + ": Payment date is not in valid format.");
                            }

                            try
                            {
                                odtBankDate = DateTime.Parse(datarow["Clearancedt"].ToString().Trim());
                            }
                            catch (FormatException)
                            {
                                sErrorMessages.Add("Row " + iRowIndex + ": Clearance date is not in valid format.");
                            }
                        }
                        iRowIndex++;
                    }
                    iRowIndex = 2;
                    if (sErrorMessages.Count == 0)
                    {
                        foreach (DataRow datarow in oDTFeeDetails.Rows)
                        {
                            DataRow[] oStudentRows = oDTAllStudents.Select("Enrolment_Number = '" + datarow["RegNo"].ToString().Trim() + "'");
                            if (oStudentRows.Length == 0)
                                sErrorMessages.Add("Row " + iRowIndex + ": Student not found");
                            else if (oStudentRows.Length == 1)
                            {
                                // yearwise student id
                                string sStudentName = datarow["Name"].ToString().Trim();
                                if (sStudentName.IndexOf(" ") > 0)
                                    sStudentName = sStudentName.Substring(0, sStudentName.IndexOf(" "));

                                if (!oStudentRows[0]["Name"].ToString().Trim().ToUpper().Contains(sStudentName.ToUpper()))
                                    sErrorMessages.Add("Row " + iRowIndex + ": Student name not matching with registration number.");
                                else
                                {
                                    int iStudentId = Convert.ToInt32(oStudentRows[0]["YearWise_Student_Id"]);
                                    DateTime dtToday = System.DateTime.Today;
                                    StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
                                    DataSet dsDebitDetails = oStudentFeeDetailsBL.GetStudentFeeDetails(iStudentId, dtToday, Constants.I_ZERO);

                                    DataRow[] oFeeRows = dsDebitDetails.Tables[0].Select("Payable_for in('June','Term-I','Annual','July','August','September','October') AND [D/C]='Debit'", "Paid_Date ASC,Std_Fee_Type_Id ASC");

                                    // if import is already done then 1 debit entry for october will be retrieved.
                                    if (oFeeRows.Length > 1)
                                    {
                                        StringBuilder sFeeIds = new StringBuilder();
                                        string sStdFeeTypeId = "";
                                        foreach (DataRow oFeeRow in oFeeRows)
                                        {
                                            sFeeIds.Append(oFeeRow["Schoolwise_Student_Fee_Id"].ToString().Trim() + ",");
                                            if (oFeeRow["Payable_for"].ToString() == "October")
                                                sStdFeeTypeId = oFeeRow["Std_Fee_Type_Id"].ToString().Trim();
                                        }

                                        string sStudentFeeList = GetXMLForStudentFeeIds(sFeeIds.ToString().Substring(0, sFeeIds.ToString().LastIndexOf(",")));
                                        string sRemarks = "Amount paid for June, Term-I, Annual, July, August, September, October - Partial";

                                        DataRow[] oBankRows = oDTAllBanks.Select("Bank_Name = '" + datarow["BankName"].ToString().Trim() + "'");
                                        iBankId = Convert.ToInt32(oBankRows[0]["Schoolwise_Bank_Id"]);

                                        int iAmount = Convert.ToInt32(datarow["Amt"]);
                                        string sChequeDetails = GetXMLForChequeDetails(datarow["Paymentdt"].ToString().Trim(), datarow["ChqNo"].ToString().Trim(), iBankId, iAmount);
                                        string sLateFeeDetails = GetXMLForLateFeeDetails();
                                        string sCreditDetails = GetXMLForCreditDetails(sStdFeeTypeId);


                                        oStudentFeeDetailsBL.PayStudentFeeWithCheque(11200, iAmount, iStudentId, sStudentFeeList, sRemarks,
                                            sChequeDetails, Convert.ToDateTime(datarow["Paymentdt"].ToString()), 0, sLateFeeDetails, 0, 'N', 0, sCreditDetails, Convert.ToDateTime(datarow["Clearancedt"].ToString().Trim()));
                                    }
                                }
                            }
                            else
                                sErrorMessages.Add("Row " + iRowIndex + ": Multiple students found.");
                            iRowIndex++;
                        }
                    }
                    if (sErrorMessages.Count > 0)
                    {
                        StringBuilder sErrors = new StringBuilder();
                        foreach (string sError in sErrorMessages)
                            sErrors.Append(sError + "<br>");
                        sErrorMessage = sErrors.ToString();
                    }
                }

            }

            return sErrorMessage;
        }

        private string SaveStudentHealthDetails()
        {
            DataSet oDSStudentHealthDetails = CommonUtility.ReadExcelSheetAndFetchData(msServerFilePath, "", "Student Health Details");

            DataTable oDTHealthDetails = oDSStudentHealthDetails.Tables[0].Copy();
            oDTHealthDetails = CommonUtility.DeleteEmptyRows(oDTHealthDetails);
            // Check if data is loaded in dataset successfully.
            if (oDTHealthDetails.Rows.Count >= 1)
            {
                string sStudentDetails = GetXMLStringForHealthDetailsFromXLSRows(oDTHealthDetails, "StudentHealthDetails", "StudentHealthDetail");

                HealthDetailsBL oHealthDetailsBL = new HealthDetailsBL(moStudentInfoStruct.iSchoolId, moStudentInfoStruct.iAcademicYearId, moStudentInfoStruct.iUserId);
                oHealthDetailsBL.InsertMultipalStudentHealthDetails(moStudentInfoStruct.iUserId, sStudentDetails);
            }
            return string.Empty;
        }

        private string SaveStudentsRFIDIfValidFileUploaded()
        {
            DataSet oDSStudentRFIDDetails = CommonUtility.ReadExcelSheetAndFetchData(msServerFilePath, "", "Student RFID Details");

            DataTable oDTRFIDDetails = oDSStudentRFIDDetails.Tables[0].Copy();
            oDTRFIDDetails = CommonUtility.DeleteEmptyRows(oDTRFIDDetails);

            if (oDTRFIDDetails.Rows.Count < 1)
                return "No records found.";

            List<string> sErrorMessages = new List<string>();
            int iRowIndex = 2;

            string sBlankRegNoRows = string.Empty;
            string sBlankRFIDRows = string.Empty;

            foreach (DataRow oRow in oDTRFIDDetails.Rows)
            {
                string sRegistrationNo = Convert.ToString(oRow["Enrolment No"]).Trim();
                string sRFID = Convert.ToString(oRow["RFID"]).Trim();

                if (string.IsNullOrWhiteSpace(sRegistrationNo))
                {
                    sBlankRegNoRows += iRowIndex + ",";
                }

                if (string.IsNullOrWhiteSpace(sRFID))
                {
                    sBlankRFIDRows += iRowIndex + ",";
                }

                iRowIndex++;
            }

            string sErrorMessage = string.Empty;

            if (!string.IsNullOrEmpty(sBlankRegNoRows))
            {
                sErrorMessage += "Enrolment No blank rows are - "  + sBlankRegNoRows.TrimEnd(',') + "<br>";
            }

            if (!string.IsNullOrEmpty(sBlankRFIDRows))
            {
                sErrorMessage += "RFID blank rows are - "  + sBlankRFIDRows.TrimEnd(',')  + "<br>";
            }

            if (!string.IsNullOrEmpty(sErrorMessage))
            {
                return sErrorMessage;
            }

            ValidateRegistrationNumbersExist(oDTRFIDDetails);

            string sStudentDetails = GetXMLStringForRFIDDetailsFromXLSRows( oDTRFIDDetails, "StudentRFIDDetails","StudentRFIDDetails");

            RFIDDetailsBL moRFIDDetailsBL = new RFIDDetailsBL( moStudentInfoStruct.iSchoolId, moStudentInfoStruct.iUserId);

            moRFIDDetailsBL.ImportRFIDDetails( moStudentInfoStruct.iUserId, sStudentDetails, moStudentInfoStruct.iAcademicYearId);

           return string.Empty;
        }

          private bool ValidateRegistrationNumbersExist(DataTable aoDataTable)
            {
                string sInvalidRows = string.Empty;

                RFIDDetailsBL oRFIDDetailsBL = new RFIDDetailsBL( moStudentInfoStruct.iSchoolId,moStudentInfoStruct.iUserId);

                List<string> lstRegNumbers = oRFIDDetailsBL.GetRegistrationNumbers( moStudentInfoStruct.iAcademicYearId);

                for (int iRowCount = 0; iRowCount < aoDataTable.Rows.Count; iRowCount++)
                {
                    string sRegNo = aoDataTable.Rows[iRowCount]["Enrolment No"].ToString() .Trim();
                    if (!lstRegNumbers.Contains(sRegNo))
                    {
                        sInvalidRows += (iRowCount + 1) + ", ";
                    }
                }

                if (sInvalidRows != string.Empty)
                {
                    sInvalidRows = sInvalidRows.Substring(0, sInvalidRows.Length - 2);

                    throw new Exception( "Registration Number does not exist in the database at row(s): " + sInvalidRows + ".");
                }

                return true;
            }

          private string GetXMLForStudentFeeIds(string asStudentFeeIds)
         {
            const string S_STUDENT_FEE_ID = "Student_Fee_Id";
            const string S_STUDENT_LATE_FEE = "Late_Fee_Amt";
            const string S_STUDENT_FEE_LIST = "StudentFeeList";
            XmlDocument oDoc = new XmlDocument();
            XmlElement root = oDoc.CreateElement(S_STUDENT_FEE_LIST);
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, S_STUDENT_FEE_LIST, "");

            string sStudentFeeIds = asStudentFeeIds.Trim();
            string[] sArrStudentFeeId = sStudentFeeIds.Split(',');
            string[] sArrLateFeeAmt = " 0 + 0 + 0 + 0 + 0 + 0 + 0".Split('+');
            for (int iCnt = 0; iCnt < sArrStudentFeeId.Length; iCnt++)
            {
                int iStudentFeeId = Convert.ToInt32(sArrStudentFeeId[iCnt]);
                int iLateFeeAmt = Convert.ToInt32(sArrLateFeeAmt[iCnt]);
                string sAtrrName;
                XmlAttribute attr;

                XmlNode oXmlNode;
                oXmlNode = oDoc.CreateNode(S_ELEMENT, S_STUDENT, "");

                sAtrrName = S_STUDENT_FEE_ID;
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = iStudentFeeId.ToString();
                oXmlNode.Attributes.Append(attr);
                oXmlRootNode.AppendChild(oXmlNode);

                string sAtrrName1 = S_STUDENT_LATE_FEE;
                XmlAttribute attr1 = oDoc.CreateAttribute(sAtrrName1);
                attr1.Value = iLateFeeAmt.ToString();
                oXmlNode.Attributes.Append(attr1);
                oXmlRootNode.AppendChild(oXmlNode);
            }

            root.AppendChild(oXmlRootNode);
            return root.InnerXml;
        }

        private string GetXMLForChequeDetails(string asPaymentDate, string asChqNumber, int aiBankId, int aiChequeAmount)
        {
            XmlDocument oDoc = new XmlDocument();
            XmlElement root = oDoc.CreateElement("ChequeDetailsList");
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "ChequeDetailsList", "");

            XmlNode oXmlNode;
            oXmlNode = oDoc.CreateNode(S_ELEMENT, S_STUDENT, "");

            string sAtrrName1 = "DueDate";
            XmlAttribute attr1 = oDoc.CreateAttribute(sAtrrName1);
            attr1.Value = asPaymentDate.Trim();
            oXmlNode.Attributes.Append(attr1);

            string sAtrrName2 = "ChequeNo";
            XmlAttribute attr2 = oDoc.CreateAttribute(sAtrrName2);
            attr2.Value = asChqNumber.Trim();
            oXmlNode.Attributes.Append(attr2);

            string sAtrrName3 = "BankId";
            XmlAttribute attr3 = oDoc.CreateAttribute(sAtrrName3);
            attr3.Value = aiBankId.ToString().Trim();
            oXmlNode.Attributes.Append(attr3);

            string sAtrrName4 = "ChequeRemarks";
            XmlAttribute attr4 = oDoc.CreateAttribute(sAtrrName4);
            attr4.Value = "";
            oXmlNode.Attributes.Append(attr4);

            string sAtrrName5 = "Is_PDC";
            XmlAttribute attr5 = oDoc.CreateAttribute(sAtrrName5);
            attr5.Value = Constants.C_NO.ToString().Trim();
            oXmlNode.Attributes.Append(attr5);

            string sAtrrName6 = "ChequeAmt";
            XmlAttribute attr6 = oDoc.CreateAttribute(sAtrrName6);
            attr6.Value = aiChequeAmount.ToString().Trim();
            oXmlNode.Attributes.Append(attr6);


            oXmlRootNode.AppendChild(oXmlNode);

            root.AppendChild(oXmlRootNode);
            return root.InnerXml;
        }

        private string GetXMLForCreditDetails(string asStdFeeTypeId)
        {
            XmlDocument oDoc = new XmlDocument();
            XmlElement root = oDoc.CreateElement("CreditDetailsList");
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "CreditDetailsList", "");
            XmlNode oXmlNode;
            oXmlNode = oDoc.CreateNode(S_ELEMENT, S_STUDENT, "");

            string sAtrrName1 = "ChequeDate";
            XmlAttribute attr1 = oDoc.CreateAttribute(sAtrrName1);
            attr1.Value = "10-Oct-2009";
            oXmlNode.Attributes.Append(attr1);

            string sAtrrName2 = "PayableFor";
            XmlAttribute attr2 = oDoc.CreateAttribute(sAtrrName2);
            attr2.Value = "October";
            oXmlNode.Attributes.Append(attr2);

            string sAtrrName3 = "FeeType";
            XmlAttribute attr3 = oDoc.CreateAttribute(sAtrrName3);
            attr3.Value = "Monthly";
            oXmlNode.Attributes.Append(attr3);

            string sAtrrName4 = "Remarks";
            XmlAttribute attr4 = oDoc.CreateAttribute(sAtrrName4);
            attr4.Value = "";
            oXmlNode.Attributes.Append(attr4);

            string sAtrrName5 = "Std_Fee_Type_id";
            XmlAttribute attr5 = oDoc.CreateAttribute(sAtrrName5);
            attr5.Value = asStdFeeTypeId.Trim();
            oXmlNode.Attributes.Append(attr5);

            oXmlRootNode.AppendChild(oXmlNode);

            root.AppendChild(oXmlRootNode);
            return root.InnerXml;
        }

        private string GetXMLForLateFeeDetails()
        {
            const string S_LATE_FEE_AMT = "Late_Fee_Amt";
            const string S_LATE_FEE_DESC = "Late_Fee_Desc";

            XmlDocument oDoc = new XmlDocument();
            XmlElement root = oDoc.CreateElement("LateFeeDetails");
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "LateFeeDetails", "");

            XmlNode oXmlNode;
            oXmlNode = oDoc.CreateNode(S_ELEMENT, "LateFee", "");

            string sAtrrName = S_LATE_FEE_AMT.Trim();
            XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = "0";
            oXmlNode.Attributes.Append(attr);


            string sAtrrName1 = S_LATE_FEE_DESC.Trim();
            XmlAttribute attr1 = oDoc.CreateAttribute(sAtrrName1);
            attr1.Value = "";
            oXmlNode.Attributes.Append(attr1);

            oXmlRootNode.AppendChild(oXmlNode);
            root.AppendChild(oXmlRootNode);
            return root.InnerXml;
        }

        private string SaveCautionMoney()
        {
            // This procedure accepts parameter as aoAgentBusinessUploadBL. 
            string sErrorMessage = "";
            const char C_CHEQUE_MODE = 'Q';
            DataSet oDSCautionMoneyDetails = CommonUtility.ReadExcelSheetAndFetchData(msServerFilePath, "", "CautionMoney");

            DataTable oDTCautionMoneyDetails = oDSCautionMoneyDetails.Tables[0];
            oDTCautionMoneyDetails = CommonUtility.DeleteEmptyRows(oDTCautionMoneyDetails);

            StudentCautionMoneyDetailsCollectionBL oStudentCautionMoneyDetailsCollectionBL = new StudentCautionMoneyDetailsCollectionBL();
            List<StudentCautionMoneyDetailsBL> oArrStudentCautionMoneyDetailsBL = new List<StudentCautionMoneyDetailsBL>();

            if (oDTCautionMoneyDetails.Rows.Count == 0)
                sErrorMessage = "There must be atleast one row to import.";
            else
            {
                // Check column names.
                PopulateColumnNames();
                StringBuilder sInvalidcolumnNames = new StringBuilder();
                foreach (DataColumn oColumn in oDTCautionMoneyDetails.Columns)
                {
                    if (!moArrColumnNames.Contains(oColumn.ColumnName.ToUpper()))
                        sInvalidcolumnNames.Append(oColumn.ColumnName + ", ");
                }

                if (sInvalidcolumnNames.ToString().Trim() != string.Empty)
                    sErrorMessage = "Invalid column names : " + sInvalidcolumnNames.ToString().Trim().Substring(0, sInvalidcolumnNames.ToString().Trim().LastIndexOf(","));
                else
                {

                    int iSchoolId = moStudentInfoStruct.iSchoolId;
                    int iAcademicYearId = moStudentInfoStruct.iAcademicYearId;
                    DataTable oDTAllStudents = StudentBL.GetAllStudents(iSchoolId, 0, 0, iAcademicYearId);
                    DataTable oDTAllBanks = SchoolwiseBankMasterCollectionBL.FetchSchoolwiseBankMasterDetails(iSchoolId);

                    int iRowIndex = 2;
                    List<string> sErrorMessages = new List<string>();
                    foreach (DataRow datarow in oDTCautionMoneyDetails.Rows)
                    {
                        StudentCautionMoneyDetailsBL oStudentCautionMoneyDetailsBL = new StudentCautionMoneyDetailsBL();
                        StudentCautionMoneyChequeDetailsBL oStudentCautionMoneyChequeDetailsBL = new StudentCautionMoneyChequeDetailsBL();

                        DataRow[] oStudentRows = oDTAllStudents.Select("Student_Name like '%" + datarow["STUDENT NAME"].ToString().Trim() + "%'");// OR First_Name_Last_Name like '%" + datarow["STUDENT NAME"].ToString().Trim() + "%'");

                        int iStudentId = 0;
                        if (oStudentRows.Length == 0)
                        {
                            oStudentRows = oDTAllStudents.Select("First_Name_Last_Name like '%" + datarow["STUDENT NAME"].ToString().Trim() + "%'");
                            if (oStudentRows.Length == 0)
                            {
                                oStudentRows = oDTAllStudents.Select("First_Name_Middle_Name like '%" + datarow["STUDENT NAME"].ToString().Trim() + "%'");
                                if (oStudentRows.Length == 0)
                                    sErrorMessages.Add("Row " + iRowIndex + ": Student not found");
                                else
                                    iStudentId = Convert.ToInt32(oStudentRows[0]["SchoolWise_Student_Id"]);
                            }
                            else if (oStudentRows.Length == 1)
                                iStudentId = Convert.ToInt32(oStudentRows[0]["SchoolWise_Student_Id"]);
                        }
                        else if (oStudentRows.Length > 1)
                        {
                            int iStudentCount = 0;
                            foreach (DataRow oStudentRow in oStudentRows)
                                if (datarow["Grade"].ToString().Trim().Replace("Std ", "").ToUpper() == oStudentRow["Standard_Name"].ToString().Trim().ToUpper())
                                {
                                    iStudentId = Convert.ToInt32(oStudentRows[0]["SchoolWise_Student_Id"]);
                                    iStudentCount++;
                                }

                            if (iStudentCount == 0)
                                sErrorMessages.Add("Row " + iRowIndex + ": Student's grade in database is not matching.");
                            else if (iStudentCount > 1)
                                sErrorMessages.Add("Row " + iRowIndex + ": Multiple students found for same grade.");
                        }
                        else if (oStudentRows.Length == 1)
                            iStudentId = Convert.ToInt32(oStudentRows[0]["SchoolWise_Student_Id"]);

                        oStudentCautionMoneyDetailsBL.Schoolwise_Student_Id = iStudentId;

                        int iBankId = 0;
                        if (datarow["ChqNo"].ToString().Trim() == string.Empty)
                            sErrorMessages.Add("Row " + iRowIndex + ": Cheque number is blank.");
                        else
                            oStudentCautionMoneyChequeDetailsBL.Cheque_Number = datarow["ChqNo"].ToString().Trim();

                        if (datarow["BankName"].ToString().Trim() == string.Empty)
                            sErrorMessages.Add("Row " + iRowIndex + ": Bank name is blank.");
                        else
                        {
                            DataRow[] oBankRows = oDTAllBanks.Select("Bank_Name = '" + datarow["BankName"].ToString().Trim() + "'");
                            if (oBankRows.Length == 0)
                                sErrorMessages.Add("Row " + iRowIndex + ": Bank name not found.");
                            else if (oBankRows.Length > 1)
                                sErrorMessages.Add("Row " + iRowIndex + ": Multiple bank names found.");
                            else
                                iBankId = Convert.ToInt32(oBankRows[0]["Schoolwise_Bank_Id"].ToString().Trim());
                        }
                        oStudentCautionMoneyChequeDetailsBL.Bank_Id = iBankId;

                        if (datarow["BkDate"].ToString().Trim() == string.Empty)
                            sErrorMessages.Add("Row " + iRowIndex + ": Cheque date is blank.");
                        else
                        {
                            DateTime odtBankDate;
                            try
                            {
                                odtBankDate = DateTime.Parse(datarow["BkDate"].ToString().Trim());
                                oStudentCautionMoneyChequeDetailsBL.Cheque_Date = odtBankDate;
                                oStudentCautionMoneyDetailsBL.Payment_Date = odtBankDate;
                            }
                            catch (FormatException)
                            {
                                sErrorMessages.Add("Row " + iRowIndex + ": Cheque date is not in valid format.");
                            }
                        }

                        oStudentCautionMoneyChequeDetailsBL.StudentId = iStudentId;

                        if (datarow["Credit"].ToString().Trim() == string.Empty)
                            sErrorMessages.Add("Row " + iRowIndex + ": Amount is blank.");
                        else if (datarow["Credit"].ToString().Trim() != "10000")
                            sErrorMessages.Add("Row " + iRowIndex + ": Invalid amount, Amount should be 10000.");
                        else
                            oStudentCautionMoneyDetailsBL.Amount = Convert.ToInt32(datarow["Credit"].ToString().Trim());

                        oStudentCautionMoneyDetailsBL.School_Id = iSchoolId;
                        oStudentCautionMoneyDetailsBL.Inserted_By_id = moStudentInfoStruct.iUserId;
                        oStudentCautionMoneyDetailsBL.Updated_By_Id = moStudentInfoStruct.iUserId;
                        oStudentCautionMoneyDetailsBL.Update_Date = System.DateTime.Now;
                        oStudentCautionMoneyChequeDetailsBL.Inserted_By_id = moStudentInfoStruct.iUserId;
                        oStudentCautionMoneyChequeDetailsBL.Updated_By_Id = moStudentInfoStruct.iUserId;
                        oStudentCautionMoneyChequeDetailsBL.Remarks = "";
                        oStudentCautionMoneyDetailsBL.Paid_By_Student = true;
                        oStudentCautionMoneyDetailsBL.Payment_Mode = C_CHEQUE_MODE.ToString().Trim();
                        oStudentCautionMoneyDetailsBL.StudentCautionMoneyChequeDetails = oStudentCautionMoneyChequeDetailsBL;

                        iRowIndex++;

                        oArrStudentCautionMoneyDetailsBL.Add(oStudentCautionMoneyDetailsBL);
                    }

                    if (sErrorMessages.Count > 0)
                    {
                        StringBuilder sErrors = new StringBuilder();
                        foreach (string sError in sErrorMessages)
                            sErrors.Append(sError + "<br>");
                        sErrorMessage = sErrors.ToString().Trim();
                    }
                    else
                        oStudentCautionMoneyDetailsCollectionBL.InsertMultipleCautionMoney(oArrStudentCautionMoneyDetailsBL);
                }
            }
            return sErrorMessage;
        }

        #region "XML Creation"

        /// <summary>
        /// This method is used to create XML file for import students health details.
        /// </summary>
        /// <param name="aoDataTable"></param>
        /// <param name="asRootElementName"></param>
        /// <param name="asElementName"></param>
        /// <returns></returns>
        public string GetXMLStringForHealthDetailsFromXLSRows(DataTable aoDataTable, string asRootElementName, string asElementName)
        {
            const string S_ELEMENT = "element";
            XmlDocument oDoc = new XmlDocument();
            string sAtrrName;
            XmlAttribute attr;

            XmlElement root = oDoc.CreateElement(asRootElementName);
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, asRootElementName, "");

            ArrayList oArrayList = new ArrayList();
            oArrayList.Add("Enrolment_Number");
            oArrayList.Add("FatherWeight");
            oArrayList.Add("MotherWeight");
            oArrayList.Add("FatherHeight");
            oArrayList.Add("MotherHeight");
            oArrayList.Add("FatherAdharcardNo");
            oArrayList.Add("MotherAadharCardNo");
            oArrayList.Add("FatherBloodGroup");
            oArrayList.Add("MotherBloodGroup");
            oArrayList.Add("FatherDateOfBirth");
            oArrayList.Add("MotherDateOfBirth");
            oArrayList.Add("FamilyMonthlyIncome");
            oArrayList.Add("CWSN");

            if (!CheckForHealthDetailsMandatoryFields(aoDataTable))
            {
                for (int iRowCount = 0; iRowCount <= aoDataTable.Rows.Count - 1; iRowCount++)
                {
                    // Create root xml element.
                    XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, asElementName, "");
                    DataRow oDataRow = aoDataTable.Rows[iRowCount];

                    // Loop through all the columns for the row.
                    string sRegistrationNumber = "";
                    sRegistrationNumber = aoDataTable.Rows[iRowCount][I_XLS_REG_NO].ToString().Trim();
                    msImportedStudentsRegNumbers.Append("'" + sRegistrationNumber + "'");
                    msImportedStudentsRegNumbers.Append(",");

                    for (int iCount = 0; iCount < oArrayList.Count; iCount++)
                    {
                        sAtrrName = oArrayList[iCount].ToString().Trim();
                        attr = oDoc.CreateAttribute(sAtrrName);

                        if (sAtrrName == "Enrolment_Number")
                        {
                            string sValue = aoDataTable.Rows[iRowCount]["Registration Number"].ToString().Trim();
                            attr.Value = sValue;
                        }
                        else if (sAtrrName == "FatherWeight")
                        {
                            string sValue = aoDataTable.Rows[iRowCount]["Father Weight"].ToString().Trim();
                            attr.Value = sValue;
                        }
                        else if (sAtrrName == "MotherWeight")
                        {
                            string sValue = aoDataTable.Rows[iRowCount]["Mother Weight"].ToString().Trim();
                            attr.Value = sValue;
                        }
                        else if (sAtrrName == "FatherHeight")
                        {
                            string sValue = aoDataTable.Rows[iRowCount]["Father Height"].ToString().Trim();
                            attr.Value = sValue;
                        }
                        else if (sAtrrName == "MotherHeight")
                        {
                            string sValue = aoDataTable.Rows[iRowCount]["Mother Height"].ToString().Trim();
                            attr.Value = sValue;
                        }
                        else if (sAtrrName == "FatherAdharcardNo")
                        {
                            string sValue = aoDataTable.Rows[iRowCount]["Father Aadhar Card No"].ToString().Trim();
                            attr.Value = sValue;
                        }
                        else if (sAtrrName == "MotherAadharCardNo")
                        {
                            string sValue = aoDataTable.Rows[iRowCount]["Mother Aadhar Card No"].ToString().Trim();
                            attr.Value = sValue;
                        }
                        else if (sAtrrName == "FatherBloodGroup")
                        {
                            string sValue = aoDataTable.Rows[iRowCount]["Father Blood Group"].ToString().Trim();
                            attr.Value = sValue;
                        }
                        else if (sAtrrName == "MotherBloodGroup")
                        {
                            string sValue = aoDataTable.Rows[iRowCount]["Mother Blood Group"].ToString().Trim();
                            attr.Value = sValue;
                        }
                        else if (sAtrrName == "FatherDateOfBirth")
                        {
                            string sValue = aoDataTable.Rows[iRowCount]["Father Date Of Birth"].ToString().Trim();
                            attr.Value = sValue;
                        }
                        else if (sAtrrName == "MotherDateOfBirth")
                        {
                            string sValue = aoDataTable.Rows[iRowCount]["Mother Date Of Birth"].ToString().Trim();
                            attr.Value = sValue;
                        }
                        else if (sAtrrName == "FamilyMonthlyIncome")
                        {
                            string sValue = aoDataTable.Rows[iRowCount]["Family Monthly Income"].ToString().Trim();

                            if (sValue == string.Empty)
                                sValue = Constants.S_ZERO;

                            attr.Value = sValue;
                        }
                        else if (sAtrrName == "CWSN")
                        {
                            string sValue = aoDataTable.Rows[iRowCount]["CWSN"].ToString().Trim();
                            attr.Value = sValue;
                        }
                        oXmlNode.Attributes.Append(attr);
                    }
                    oXmlRootNode.AppendChild(oXmlNode);
                }
            }
            root.AppendChild(oXmlRootNode);

            // return the string generated.
            return root.InnerXml;
        }

        public string GetXMLStringFromXLSRows(DataTable aoDataTable, string asRootElementName, string asElementName)
        {
            const string S_ELEMENT = "element";
            const int I_OTHER_OCCUPATION_ID = 5;
            const string S_OTHER_OCCUPATTION = "Other";
            XmlDocument oDoc = new XmlDocument();
            string sAtrrName;
            XmlAttribute attr;
            // Create a root level element.
            XmlElement root = oDoc.CreateElement(asRootElementName);
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, asRootElementName, "");

            MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();

            ArrayList oArrayList = new ArrayList();
            oArrayList.Add("Enrolment_Number");
            oArrayList.Add("Roll_No");
            oArrayList.Add("First_Name");
            oArrayList.Add("Middle_Name");
            oArrayList.Add("Last_Name");
            oArrayList.Add("Mother_Name");
            oArrayList.Add("DOB");
            oArrayList.Add("Admission_Date");
            oArrayList.Add("Joining_Date");
            oArrayList.Add("Sex");
            oArrayList.Add("Blood_Group");
            oArrayList.Add("Parent_Name");
            oArrayList.Add("Parent_Occupation");
            oArrayList.Add("Address");
            oArrayList.Add("City");
            oArrayList.Add("State");
            oArrayList.Add("Pincode");
            oArrayList.Add("Residence_Phone_Number");
            oArrayList.Add("Mobile_Number");
            oArrayList.Add("Category_Id");
            oArrayList.Add("CasteAndSubCaste");
            oArrayList.Add("IsNewStudent");
            oArrayList.Add("ApplicableRule");
            oArrayList.Add("Mother_Tongue");
            oArrayList.Add("IsRTEStudent");
            oArrayList.Add("Mobile_Number2");
            oArrayList.Add("LastSchoolName");
            oArrayList.Add("LastStandard");
            oArrayList.Add("LastSchoolBoardName");
            oArrayList.Add("IsRecognised");
            oArrayList.Add("Birth_Place");
            oArrayList.Add("Nationality");
            oArrayList.Add("RTECategory_Id");
            oArrayList.Add("E-mail");            
            //oArrayList.Add("Status");
            oArrayList.Add("Admission_academic_year");
            oArrayList.Add("Admission_Standard");
            oArrayList.Add("Current_Standard");
            oArrayList.Add("Current_Academic_Year");
            oArrayList.Add("Is_Handicapped");
            oArrayList.Add("Previous_Marks_Obtained");
            oArrayList.Add("Previous_Marks_Out_Off");
            oArrayList.Add("Previous_Year_of_Passing");
            oArrayList.Add("Subject_Name_Fields");
            oArrayList.Add("Religion");
            oArrayList.Add("Birth_Taluka");
            oArrayList.Add("Birth_District");
            oArrayList.Add("HouseNoPlotNo");
            oArrayList.Add("Main_Area");
            oArrayList.Add("Subarea_Name");
            oArrayList.Add("Landmark");
            oArrayList.Add("Taluka");
            oArrayList.Add("District");
            oArrayList.Add("Fee_Area_Name");
            oArrayList.Add("Father_Occupation");
            oArrayList.Add("Father_Qualification");
            oArrayList.Add("Father_Email");
            oArrayList.Add("Father_Office_Name");
            oArrayList.Add("Father_Office_Address");
            oArrayList.Add("Mother_Occupation");
            oArrayList.Add("Mother_Qualification");
            oArrayList.Add("Mother_Email");
            oArrayList.Add("Mother_Office_Name");
            oArrayList.Add("Mother_Office_Address");
            oArrayList.Add("Board_Registration_No");
            oArrayList.Add("GRNumber");
            oArrayList.Add("StudentUniqueNo");
            oArrayList.Add("IsForDayBoarding");
            oArrayList.Add("FeeCategoryId");   
            oArrayList.Add("AadharCardNo");      //Aadhar card no add
            oArrayList.Add("DateOfBirthInText");
            oArrayList.Add("Is_Dummy_Admission");
            oArrayList.Add("Login_Name");


            if (!CheckForMandatoryFields(aoDataTable))
            {
                DataTable odtNextLogin = StudentBL.GetNextStudentRollNoAndLogin(moStudentInfoStruct.iStandardId, moStudentInfoStruct.iDivisionId, moStudentInfoStruct.iSchoolId);
                int iNextLoginId = Convert.ToInt32(odtNextLogin.Rows[0]["LoginId"]);
                // Loop through all the grid rows.
                for (int iRowCount = 0; iRowCount <= aoDataTable.Rows.Count - 1; iRowCount++)
                {
                    // Create root xml element.
                    XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, asElementName, "");
                    DataRow oDataRow = aoDataTable.Rows[iRowCount];

                    // Loop through all the columns for the row.
                    string sFirstName = "", sRegistrationNumber = "";
                    sRegistrationNumber = aoDataTable.Rows[iRowCount][I_XLS_REG_NO].ToString().Trim();
                    msImportedStudentsRegNumbers.Append("'" + sRegistrationNumber + "'");
                    msImportedStudentsRegNumbers.Append(",");
                    sFirstName = aoDataTable.Rows[iRowCount][I_XLS_FIRST_NAME].ToString().Trim();

                    for (int iCount = 0; iCount < oArrayList.Count; iCount++)
                    {
                        sAtrrName = oArrayList[iCount].ToString().Trim();
                        attr = oDoc.CreateAttribute(sAtrrName);
                        if (sAtrrName == "Sex")
                        {
                            string sValue = aoDataTable.Rows[iRowCount][iCount].ToString().Trim();
                            attr.Value = sValue.Substring(0, 1);
                        }
                        if (sAtrrName == "Blood_Group")
                        {
                            string sValue = aoDataTable.Rows[iRowCount][iCount].ToString().Trim().ToUpper();
                            if (sValue == "O+" || sValue == "A+" || sValue == "B+" || sValue == "AB+"
                                || sValue == "O-" || sValue == "A-" || sValue == "B-" || sValue == "AB-")
                                attr.Value = sValue;
                            else
                                attr.Value = string.Empty;
                        }
                        else if (sAtrrName == "Login_Name")
                        {
                            string sValue = string.Empty;
                            if (moStudentInfoStruct.iSchoolId == Constants.SchoolId.SVNP.ToInt())
                                sValue = sRegistrationNumber;
                            else
                                sValue = iNextLoginId.ToString().Trim();

                            attr.Value = sValue;
                        }
                        else if (sAtrrName == "Category_Id")
                        {
                            string sCategory = aoDataTable.Rows[iRowCount][iCount].ToString().Trim();
                            int iCategoryId = oMasterDataCollectionBL.GetCategoryIdForCategory(sCategory);
                            attr.Value = iCategoryId.ToString().Trim();
                        }
                        else if (sAtrrName == "Parent_Occupation")
                        {
                            string sParentOccupation = aoDataTable.Rows[iRowCount][iCount].ToString().Trim();
                            int iParentOccupationId = oMasterDataCollectionBL.GetParentOccupationIdForParentOccupationName(sParentOccupation);

                            if (iParentOccupationId == I_OTHER_OCCUPATION_ID && sParentOccupation != S_OTHER_OCCUPATTION)
                                attr.Value = 5.ToString().Trim();
                            else if (iParentOccupationId == I_OTHER_OCCUPATION_ID && sParentOccupation == S_OTHER_OCCUPATTION)
                                attr.Value = iParentOccupationId.ToString().Trim();
                            else
                                attr.Value = iParentOccupationId.ToString().Trim();

                            oXmlNode.Attributes.Append(attr);

                            sAtrrName = "Other_Occupation";
                            attr = oDoc.CreateAttribute(sAtrrName);

                            if (iParentOccupationId == I_OTHER_OCCUPATION_ID && sParentOccupation != S_OTHER_OCCUPATTION)
                                attr.Value = sParentOccupation;
                            else
                                attr.Value = "";

                        }

                        else if (sAtrrName == "DateOfBirthInText")
                        {
                            DateTime dtBithDate = Convert.ToDateTime(aoDataTable.Rows[iRowCount]["Date of Birth"]);
                            attr.Value = CommonUtility.GetDateInWords(dtBithDate);
                        }
                        else if (sAtrrName == "Is_Dummy_Admission")
                        {
                            if ((System.Web.HttpContext.Current.Session[Constants.S_SESSION_ACADEMIC_YEAR_IS_NEWLYCREATED] != null) &&
            (Convert.ToChar(System.Web.HttpContext.Current.Session[Constants.S_SESSION_ACADEMIC_YEAR_IS_NEWLYCREATED]) == Constants.C_YES))
                            {
                                attr.Value = Constants.C_YES.ToString();
                            }
                            else
                                attr.Value = Constants.C_NO.ToString();
                        }

                        else if (sAtrrName == "IsNewStudent")
                        {
                            if (aoDataTable.Rows[iRowCount][iCount].ToString().Trim() == S_IS_YES)
                                attr.Value = true.ToString();
                            else
                                attr.Value = false.ToString();
                        }

                        else if (sAtrrName == "IsRTEStudent")
                        {
                            if (moStudentInfoStruct.bIsRTEApplicable)
                            {
                                if (aoDataTable.Rows[iRowCount][iCount].ToString().Trim() == S_IS_YES)
                                    attr.Value = true.ToString();
                                else
                                    attr.Value = false.ToString();
                            }
                            else
                                attr.Value = false.ToString();
                        }

                        else if (sAtrrName == "RTECategory_Id")
                        {
                            if (moStudentInfoStruct.bIsRTEApplicable && aoDataTable.Rows[iRowCount][I_XLS_IS_RTE_STUDENT].ToString().Trim() == S_IS_YES)
                            {
                                //string sRTECategory = aoDataTable.Rows[iRowCount][iCount].ToString().Trim();
                                //int iRTECategoryId = oMasterDataCollectionBL.GetRTECategoryIdForCategory(sRTECategory);
                                //attr.Value = iRTECategoryId.ToString().Trim();
                            }
                        }
                        else if (sAtrrName == "ApplicableRule")
                        {
                            if (moStudentInfoStruct.bIsConcessionApplicable)
                            {
                                string sRule = aoDataTable.Rows[iRowCount][iCount].ToString().Trim();
                                if (!aoDataTable.Rows[iRowCount][iCount].ToString().Trim().Equals(string.Empty))
                                {
                                    int iRuleId = oMasterDataCollectionBL.GetRuleIdForRule(sRule, moStudentInfoStruct.iSchoolId, moStudentInfoStruct.iAcademicYearId);
                                    attr.Value = iRuleId.ToString().Trim();
                                }
                                else
                                    attr.Value = Constants.I_ZERO.ToString();
                            }
                            else
                                attr.Value = Constants.I_ZERO.ToString();
                        }
                        else if (sAtrrName == "DOB" || sAtrrName == "Admission_Date" || sAtrrName == "Joining_Date")
                        {
                            attr.Value = aoDataTable.Rows[iRowCount][iCount].ToDateTime().ToString(Constants.S_DATE_FORMAT_MARATHI).Trim();
                        }
                        //else if (sAtrrName == "Status")
                        //{
                        //    string sStatus = aoDataTable.Rows[iRowCount]["Status"].ToString().Trim();
                        //    attr.Value = sStatus.ToString().Trim();
                        //}
                        else if (sAtrrName == "Admission_academic_year")
                        {
                            string sStatus = aoDataTable.Rows[iRowCount]["Admission academic year"].ToString().Trim();
                            attr.Value = sStatus.ToString().Trim();
                        }
                        else if (sAtrrName == "Admission_Standard")
                        {
                            string sStatus = aoDataTable.Rows[iRowCount]["Admission Standard"].ToString().Trim();
                            attr.Value = sStatus.ToString().Trim();
                        }
                        else if (sAtrrName == "Current_Standard")
                        {
                            string sStatus = aoDataTable.Rows[iRowCount]["Current Standard"].ToString().Trim();
                            attr.Value = sStatus.ToString().Trim();
                        }
                        else if (sAtrrName == "Current_Academic_Year")
                        {
                            string sStatus = aoDataTable.Rows[iRowCount]["Current Academic Year"].ToString().Trim();
                            attr.Value = sStatus.ToString().Trim();
                        }
                        else if (sAtrrName == "Is_Handicapped")
                        {
                            string sStatus = aoDataTable.Rows[iRowCount]["Is Handicapped?"].ToString().Trim();
                            if (sStatus == "Yes")
                                attr.Value = Constants.S_ONE;
                            else
                                attr.Value = Constants.S_ZERO;
                        }
                        else if (sAtrrName == "Previous_Marks_Obtained")
                        {
                            string sStatus = aoDataTable.Rows[iRowCount]["Previous Marks Obtained"].ToString().Trim();
                            attr.Value = sStatus.ToString().Trim();
                        }
                        else if (sAtrrName == "Previous_Marks_Out_Off")
                        {
                            string sStatus = aoDataTable.Rows[iRowCount]["Previous Marks Out Off"].ToString().Trim();
                            attr.Value = sStatus.ToString().Trim();
                        }
                        else if (sAtrrName == "Previous_Year_of_Passing")
                        {
                            string sStatus = aoDataTable.Rows[iRowCount]["Previous Year of Passing"].ToString().Trim();
                            attr.Value = sStatus.ToString().Trim();
                        }
                        else if (sAtrrName == "Subject_Name_Fields")
                        {
                            string sStatus = aoDataTable.Rows[iRowCount]["Subject Name Fields"].ToString().Trim();
                            attr.Value = sStatus.ToString().Trim();
                        }
                        else if (sAtrrName == "Religion")
                        {
                            string sReligion = aoDataTable.Rows[iRowCount]["Religion"].ToString().Trim();
                            attr.Value = sReligion.ToString().Trim();
                        }
                        else if (sAtrrName == "Birth_Taluka")
                        {
                            string sBirthTaluka = aoDataTable.Rows[iRowCount]["Birth Taluka"].ToString().Trim();
                            attr.Value = sBirthTaluka.ToString().Trim();
                        }
                        else if (sAtrrName == "Birth_District")
                        {
                            string sBirthDistrict = aoDataTable.Rows[iRowCount]["Birth District"].ToString().Trim();
                            attr.Value = sBirthDistrict.ToString().Trim();
                        }
                        else if (sAtrrName == "HouseNoPlotNo")
                        {
                            string sHouseNo = aoDataTable.Rows[iRowCount]["House No / Plot No#"].ToString().Trim();
                            attr.Value = sHouseNo.ToString().Trim();
                        }
                        else if (sAtrrName == "Main_Area")
                        {
                            string sMainArea = aoDataTable.Rows[iRowCount]["Main Area"].ToString().Trim();
                            attr.Value = sMainArea.ToString().Trim();
                        }
                        else if (sAtrrName == "Subarea_Name")
                        {
                            string sSubArea = aoDataTable.Rows[iRowCount]["Subarea Name"].ToString().Trim();
                            attr.Value = sSubArea.ToString().Trim();
                        }
                        else if (sAtrrName == "Landmark")
                        {
                            string sLandMark = aoDataTable.Rows[iRowCount]["Landmark"].ToString().Trim();
                            attr.Value = sLandMark.ToString().Trim();
                        }
                        else if (sAtrrName == "Taluka")
                        {
                            string sTaluka = aoDataTable.Rows[iRowCount]["Taluka"].ToString().Trim();
                            attr.Value = sTaluka.ToString().Trim();
                        }
                        else if (sAtrrName == "District")
                        {
                            string sDistrict = aoDataTable.Rows[iRowCount]["District"].ToString().Trim();
                            attr.Value = sDistrict.ToString().Trim();
                        }
                        else if (sAtrrName == "Fee_Area_Name")
                        {
                            string sFeeAreaName = aoDataTable.Rows[iRowCount]["Fee Area Name"].ToString().Trim();
                            attr.Value = sFeeAreaName.ToString().Trim();
                        }
                        else if (sAtrrName == "Father_Occupation")
                        {
                            string sFatherOccupation = aoDataTable.Rows[iRowCount]["Father Occupation"].ToString().Trim();
                            attr.Value = sFatherOccupation.ToString().Trim();
                        }
                        else if (sAtrrName == "Father_Qualification")
                        {
                            string sFatherQualification = aoDataTable.Rows[iRowCount]["Father Qualification"].ToString().Trim();
                            attr.Value = sFatherQualification.ToString().Trim();
                        }
                        else if (sAtrrName == "Father_Email")
                        {
                            string sFatherEmail = aoDataTable.Rows[iRowCount]["Father E-mail"].ToString().Trim();
                            attr.Value = sFatherEmail.ToString().Trim();
                        }
                        else if (sAtrrName == "Father_Office_Name")
                        {
                            string sFatherOfficeName = aoDataTable.Rows[iRowCount]["Father Office Name"].ToString().Trim();
                            attr.Value = sFatherOfficeName.ToString().Trim();
                        }
                        else if (sAtrrName == "Father_Office_Address")
                        {
                            string sFatherOfficeAddress = aoDataTable.Rows[iRowCount]["Father Office Address"].ToString().Trim();
                            attr.Value = sFatherOfficeAddress.ToString().Trim();
                        }
                        else if (sAtrrName == "Mother_Occupation")
                        {
                            string sMotherOccupation = aoDataTable.Rows[iRowCount]["Mother Occupation"].ToString().Trim();
                            attr.Value = sMotherOccupation.ToString().Trim();
                        }
                        else if (sAtrrName == "Mother_Qualification")
                        {
                            string sMotherQualification = aoDataTable.Rows[iRowCount]["Mother Qualification"].ToString().Trim();
                            attr.Value = sMotherQualification.ToString().Trim();
                        }
                        else if (sAtrrName == "Mother_Email")
                        {
                            string sMotherEmail = aoDataTable.Rows[iRowCount]["Mother E-mail"].ToString().Trim();
                            attr.Value = sMotherEmail.ToString().Trim();
                        }
                        else if (sAtrrName == "Mother_Office_Name")
                        {
                            string sMotherOfficeName = aoDataTable.Rows[iRowCount]["Mother Office Name"].ToString().Trim();
                            attr.Value = sMotherOfficeName.ToString().Trim();
                        }
                        else if (sAtrrName == "Mother_Office_Address")
                        {
                            string sMotherOfficeAddress = aoDataTable.Rows[iRowCount]["Mother Office Address"].ToString().Trim();
                            attr.Value = sMotherOfficeAddress.ToString().Trim();
                        }
                        else if (sAtrrName == "Board_Registration_No")
                        {
                            string sBoardRegistrationNo = aoDataTable.Rows[iRowCount]["Board Registration No"].ToString().Trim();
                            attr.Value = sBoardRegistrationNo.ToString().Trim();
                        }
                        else if (sAtrrName == "GRNumber")
                        {
                            if (!CheckIsGeneralRegistrationNumberIsDuplicate(aoDataTable))
                            {
                                string sGRNumber = aoDataTable.Rows[iRowCount]["General Registration Number"].ToString().Trim();
                                attr.Value = sGRNumber.ToString().Trim();
                            }
                        }
                        else if (sAtrrName == "StudentUniqueNo")
                        {
                            if (!CheckIsStudentUniqueNumberIsDuplicate(aoDataTable))
                            {
                                string sStudentUniqueNo = aoDataTable.Rows[iRowCount]["Student ID"].ToString().Trim();
                                attr.Value = sStudentUniqueNo.ToString().Trim();
                            }
                        }
                        else if (sAtrrName == "IsForDayBoarding")
                        {
                                bool bIsForDayBoarding = aoDataTable.Rows[iRowCount]["Is For Day Boarding?"].ToBool();
                                attr.Value = bIsForDayBoarding.ToBool().ToString();                            
                        }
                        else if (sAtrrName == "FeeCategoryId")      
                        {
                            string sCategoryName = aoDataTable.Rows[iRowCount]["Fee Category"].ToString().Trim();
                            DataRow[] adt = mdtCategory.Select("Name='" + sCategoryName.Trim() + "'");
                            if (adt.Length > Constants.I_ZERO)
                                attr.Value = adt[0]["Id"].ToString();
                        }
                        else if (sAtrrName == "AadharCardNo")         //added for aadhar card no
                        {
                            string sAadharCardNo = aoDataTable.Rows[iRowCount]["Aadhar Card No"].ToString().Trim();
                            attr.Value = sAadharCardNo.ToString().Trim();
                        }
                        else
                        {
                            attr.Value = aoDataTable.Rows[iRowCount][iCount].ToString().Trim();
                        }
                        oXmlNode.Attributes.Append(attr);
                    }
                    if (oXmlNode.Attributes.Count > 0)
                    {
                        if (moStudentInfoStruct.iSchoolId == Constants.SchoolId.SVNP.ToInt())
                            oXmlNode.Attributes.Append(GetPasswordAttribute(ref oDoc, sRegistrationNumber));
                        else
                            oXmlNode.Attributes.Append(GetPasswordAttribute(ref oDoc, iNextLoginId.ToString()));
                    }
                    // Add the node to root node.
                    oXmlRootNode.AppendChild(oXmlNode);
                    iNextLoginId++;
                }
            }
            // Add the root node to document element. 
            root.AppendChild(oXmlRootNode);

            // return the string generated.
            return root.InnerXml;
        }
      
       public string GetXMLStringForRFIDDetailsFromXLSRows( DataTable aoDataTable, string asRootElementName, string asElementName)
        {
            const string S_ELEMENT = "element";

            XmlDocument oDoc = new XmlDocument();

            string sAtrrName;
            XmlAttribute attr;

            XmlElement root = oDoc.CreateElement(asRootElementName);
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, asRootElementName, "");

            ArrayList oArrayList = new ArrayList();

            oArrayList.Add("EnrolmentNo");
            oArrayList.Add("RFID");

            for (int iRowCount = 0; iRowCount <= aoDataTable.Rows.Count - 1; iRowCount++)
            {
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, asElementName, "");

                for (int iCount = 0; iCount < oArrayList.Count; iCount++)
                {
                    sAtrrName = oArrayList[iCount].ToString().Trim();

                    attr = oDoc.CreateAttribute(sAtrrName);

                    if (sAtrrName == "EnrolmentNo")
                    {
                        attr.Value =  aoDataTable.Rows[iRowCount]["Enrolment No"] .ToString() .Trim();
                    }
                    else if (sAtrrName == "RFID")
                    {
                        attr.Value = aoDataTable.Rows[iRowCount]["RFID"] .ToString() .Trim();
                    }

                    oXmlNode.Attributes.Append(attr);
                }

                oXmlRootNode.AppendChild(oXmlNode);
            }

            root.AppendChild(oXmlRootNode);

            return root.InnerXml;
        }

        public string GetChallanXMLStringFromXLSRows(DataTable aoDataTable, string asRootElementName, string asElementName)
        {
            const string S_ELEMENT = "element";
            string sAtrrName;
            XmlAttribute attr;
            XmlDocument oDoc = new XmlDocument();
            // Create a root level element.
            XmlElement root = oDoc.CreateElement(asRootElementName);
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, asRootElementName, "");

            ArrayList oArrayList = new ArrayList();
            oArrayList.Add("ChallanNo");
            oArrayList.Add("Amount");
            oArrayList.Add("PaidDate");
            oArrayList.Add("ChequeNo");
            oArrayList.Add("BankName");
            oArrayList.Add("ChequeDate");
            for (int iRowCount = 0; iRowCount <= aoDataTable.Rows.Count - 1; iRowCount++)
            {
                // Create root xml element.
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, asElementName, string.Empty);

                // Loop through all the columns for the row.

                for (int iCount = 0; iCount < oArrayList.Count; iCount++)
                {
                    sAtrrName = oArrayList[iCount].ToString().Trim();
                    attr = oDoc.CreateAttribute(sAtrrName);

                    attr.Value = aoDataTable.Rows[iRowCount][iCount].ToString().Trim();
                    oXmlNode.Attributes.Append(attr);
                }
                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
            }
            // Add the root node to document element. 
            root.AppendChild(oXmlRootNode);

            // return the string generated.
            return root.InnerXml;
        }

        public string GetAllocationXMLStringFromXLSRows(DataTable aoDataTable, string asRootElementName, string asElementName)
        {
            const string S_ELEMENT = "element";
            string sAtrrName;
            XmlAttribute attr;
            XmlDocument oDoc = new XmlDocument();
            // Create a root level element.
            XmlElement root = oDoc.CreateElement(asRootElementName);
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, asRootElementName, "");

            ArrayList oArrayList = new ArrayList();
            oArrayList.Add("VehicleNumber");
            oArrayList.Add("ReadingDate");
            oArrayList.Add("ReceiptNumber");
            oArrayList.Add("ReadingFrom");
            oArrayList.Add("ReadingTo");
            oArrayList.Add("Litters");
            oArrayList.Add("PerLitterCost");
            oArrayList.Add("TotalCost");
            oArrayList.Add("FuelStationName");
            for (int iRowCount = 0; iRowCount <= aoDataTable.Rows.Count - 1; iRowCount++)
            {
                // Create root xml element.
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, asElementName, string.Empty);

                // Loop through all the columns for the row.

                for (int iCount = 0; iCount < oArrayList.Count; iCount++)
                {
                    sAtrrName = oArrayList[iCount].ToString().Trim();
                    attr = oDoc.CreateAttribute(sAtrrName);

                    attr.Value = aoDataTable.Rows[iRowCount][iCount].ToString().Trim();
                    oXmlNode.Attributes.Append(attr);
                }
                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
            }
            // Add the root node to document element. 
            root.AppendChild(oXmlRootNode);

            // return the string generated.
            return root.InnerXml;
        }

        public string GetMaintenanceXMLStringFromXLSRows(DataTable aoDataTable, string asRootElementName, string asElementName)
        {
            const string S_ELEMENT = "element";
            string sAtrrName;
            XmlAttribute attr;
            XmlDocument oDoc = new XmlDocument();
            // Create a root level element.
            XmlElement root = oDoc.CreateElement(asRootElementName);
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, asRootElementName, "");

            ArrayList oArrayList = new ArrayList();
            oArrayList.Add("VehicleNumber");
            oArrayList.Add("MaintenanceDate");
            oArrayList.Add("BillDate");
            oArrayList.Add("ExpiryDate");
            oArrayList.Add("MeterReading");
            oArrayList.Add("BillNumber");
            oArrayList.Add("WorkshopName");
            oArrayList.Add("WorkDetails");
            oArrayList.Add("LabourCharges");
            oArrayList.Add("MaintenanceType");
            oArrayList.Add("PartsUsed");
            oArrayList.Add("Qauntity");
            oArrayList.Add("Rate");
            oArrayList.Add("Charges");
            oArrayList.Add("TotalAmount");
            for (int iRowCount = 0; iRowCount <= aoDataTable.Rows.Count - 1; iRowCount++)
            {
                // Create root xml element.
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, asElementName, string.Empty);

                // Loop through all the columns for the row.

                for (int iCount = 0; iCount < oArrayList.Count; iCount++)
                {
                    sAtrrName = oArrayList[iCount].ToString().Trim();
                    attr = oDoc.CreateAttribute(sAtrrName);

                    attr.Value = aoDataTable.Rows[iRowCount][iCount].ToString().Trim();
                    oXmlNode.Attributes.Append(attr);
                }
                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
            }
            // Add the root node to document element. 
            root.AppendChild(oXmlRootNode);

            // return the string generated.
            return root.InnerXml;
        }


        public bool ReadyToUpload(string asChallanDetails, DataTable aoDataTable, int aiSchoolId, int aiAcademicYearId, int aiOriginalFeeTypeId)
        {
            string sRowChallnNo = "";
            int iChallanNo = 0;
            int iChequeNo = 0;
            string sRowNumber = "";

            for (int iVal = 0; iVal < aoDataTable.Rows.Count; iVal++)
            {
                iChallanNo = aoDataTable.Rows[iVal][I_XLS_C_CHALLANNO].ToInt();
                DataRow[] dr = aoDataTable.Select("[Bank Challan No#]=" + iChallanNo);
                if (dr.Length > 1)
                    sRowChallnNo = sRowChallnNo + "," + iChallanNo;
            }

            if (sRowChallnNo.Length > Constants.I_ZERO)
            {
                sRowChallnNo = sRowChallnNo.Substring(1);
                ThrowChallanException(2, sRowChallnNo);
            }
            for (int iColCount = 0; iColCount < aoDataTable.Columns.Count; iColCount++)
            {

                int iPaidAmount = 0;
                DateTime dtChequeDate;
                DateTime dtPaidDate;
                string sBankName;

                if (iColCount == I_XLS_C_CHALLANNO)
                {
                    ImportFeeByChallanBL moImportFeeByChallanBL = new ImportFeeByChallanBL(aiSchoolId, aiAcademicYearId, 0);

                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        iChallanNo = aoDataTable.Rows[iRowcount][iColCount].ToInt();

                        if (moImportFeeByChallanBL.InvalidChallanNo(iChallanNo, aiOriginalFeeTypeId))
                        {
                            sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                        }
                    }

                    if (sRowNumber.Trim() != "")
                        ThrowChallanException(iColCount, sRowNumber.Substring(0, sRowNumber.Length - 2));
                }

                if (iColCount == I_XLS_C_AMOUNT)
                {
                    ImportFeeByChallanBL moImportFeeByChallanBL = new ImportFeeByChallanBL(aiSchoolId, aiAcademicYearId, 0);

                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        if (aoDataTable.Rows[iRowcount][I_XLS_C_AMOUNT] == DBNull.Value)
                            aoDataTable.Rows[iRowcount][I_XLS_C_AMOUNT] = Constants.I_ZERO;

                        iPaidAmount = aoDataTable.Rows[iRowcount][I_XLS_C_AMOUNT].ToInt();
                        if (iPaidAmount <= Constants.I_ZERO)
                        {
                            sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                        }
                    }
                    if (sRowNumber.Trim() != "")
                        throw new InvalidChallanNoExceptions(S_INVALID_AMOUNT + sRowNumber + ".");
                }
                if (iColCount == I_XLS_C_PAIDDATE)
                {
                    ImportFeeByChallanBL moImportFeeByChallanBL = new ImportFeeByChallanBL(aiSchoolId, aiAcademicYearId, 0);

                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        dtPaidDate = aoDataTable.Rows[iRowcount][I_XLS_C_PAIDDATE].ToDateTime();
                        if (dtPaidDate > DateTime.Now.ToDateTime())
                        {
                            sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                        }
                    }
                    if (sRowNumber.Trim() != "")
                        throw new InvalidChallanNoExceptions(S_INVALID_PAID_DATE + sRowNumber + ".");
                }
                if (iColCount == I_XLS_C_CHQUENO || iColCount == I_XLS_C_CHEQUEDATE)
                {
                    ImportFeeByChallanBL moImportFeeByChallanBL = new ImportFeeByChallanBL(aiSchoolId, aiAcademicYearId, 0);

                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        if (aoDataTable.Rows[iRowcount][I_XLS_C_CHQUENO] == DBNull.Value || aoDataTable.Rows[iRowcount][I_XLS_C_CHQUENO].ToString().TrimAll() == string.Empty)
                            aoDataTable.Rows[iRowcount][I_XLS_C_CHQUENO] = Constants.I_ZERO;
                        iChequeNo = aoDataTable.Rows[iRowcount][I_XLS_C_CHQUENO].ToInt();
                        if (iChequeNo != 0)
                        {
                            sBankName = aoDataTable.Rows[iRowcount][I_XLS_C_BANKNAME].ToString();
                            if (sBankName == string.Empty)
                                throw new InvalidChallanNoExceptions(S_INVALID_BANKNAME + sRowNumber + ".");

                            if (aoDataTable.Rows[iRowcount][I_XLS_C_CHEQUEDATE] == DBNull.Value)
                                throw new InvalidChallanNoExceptions(S_CHEQUE_DATE + sRowNumber + ".");
                            else
                            {
                                dtChequeDate = aoDataTable.Rows[iRowcount][I_XLS_C_CHEQUEDATE].ToDateTime();
                                dtPaidDate = aoDataTable.Rows[iRowcount][I_XLS_C_PAIDDATE].ToDateTime();

                                if (dtPaidDate > DateTime.Now.ToDateTime() || dtPaidDate < dtChequeDate)
                                {
                                    sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                                }
                            }

                        }
                    }
                    if (sRowNumber.Trim() != "")
                        throw new InvalidChallanNoExceptions(S_INVALID_CHEQUE_PAID_DATE + sRowNumber + ".");
                }
            }

            return true;
        }


        public bool ReadyToUploadAllocationDetails(DataTable aoDataTable, int aiSchoolId, int aiAcademicYearId)
        {
            string sBlankVehicleNo = "", sInvalidVehicleNo = "";
            string sVehicleNo = "";
            string sMaintenanceDate = "";
            string sRowNumber = "";

            VehicleReadingAllocationBL moVehicleReadingAllocationBL = new VehicleReadingAllocationBL();
            List<string> lstVehicleNo = moVehicleReadingAllocationBL.GetVehicleNumbers(aiSchoolId, aiAcademicYearId);
          
            for (int iColCount = 0; iColCount < aoDataTable.Columns.Count; iColCount++)
            {
                if (iColCount == I_XLS_C_VEHICLENO.ToInt())
                {
                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        sVehicleNo = aoDataTable.Rows[iRowcount][iColCount].ToString();

                        if(string.IsNullOrEmpty(sVehicleNo))
                            sBlankVehicleNo = sBlankVehicleNo + (iRowcount + 1).ToString() + ", ";
                        else if (lstVehicleNo.Contains(sVehicleNo) == false)
                            sInvalidVehicleNo = sInvalidVehicleNo + (iRowcount + 1).ToString() + ", ";
                    }

                    if (sBlankVehicleNo.Trim() != "")
                        ThrowBlankDataException(iColCount, sBlankVehicleNo.Substring(0, sBlankVehicleNo.Length - 2));
                    else if (sInvalidVehicleNo.Trim() != "")
                        throw new InvalidVehicleDataExceptions(S_VALID_VEHICLE_NO + sInvalidVehicleNo.Substring(0, sInvalidVehicleNo.Length - 2) + ".");
                }
                else if (iColCount == I_XLS_C_READINGDATE.ToInt())
                {
                    sRowNumber = string.Empty;
                    string sInvalidDate = string.Empty, sFutureDate = string.Empty;
                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        sMaintenanceDate = aoDataTable.Rows[iRowcount][iColCount].ToString();
                        if (sMaintenanceDate == string.Empty)
                        {
                            sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                        }
                        else
                        {   
                            DateTime result;
                            bool bIsValid = DateTime.TryParse(sMaintenanceDate, out result);

                            if (!bIsValid)
                                sInvalidDate = sInvalidDate + (iRowcount + 1).ToString() + ", ";
                            else if(result.Date > DateTime.Now.Date)
                            {
                                sFutureDate = sFutureDate + (iRowcount + 1).ToString() + ", ";
                            }
                        }
                    }
                    if (sRowNumber.Trim() != "")
                        ThrowBlankDataException(iColCount, sRowNumber.Substring(0, sRowNumber.Length - 2));
                    else if (sInvalidDate.Trim() != "")
                        throw new InvalidVehicleDataExceptions(S_VALID_READING_DATE + sInvalidDate.Substring(0, sInvalidDate.Length - 2) + ".");
                    else if (sFutureDate.Trim() != "")
                        throw new InvalidVehicleDataExceptions(S_VALID_FUTURE_READING_DATE + sFutureDate.Substring(0, sFutureDate.Length - 2) + ".");
                }
                else if (iColCount == I_XLS_C_RECEIPTNO.ToInt())
                {
                    string sInvalidReceiptNo = string.Empty;
                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        string sReceiptNO = aoDataTable.Rows[iRowcount][iColCount].ToString();
                        if (sReceiptNO.Trim() == string.Empty || sReceiptNO.Trim() == Constants.S_ZERO)
                            sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                        //else
                        //{
                        //    int result;
                        //    bool bIsValid = Int32.TryParse(sReceiptNO, out result);
                        //    if (!bIsValid)
                        //        sInvalidReceiptNo = sInvalidReceiptNo + (iRowcount + 1).ToString() + ", ";
                        //}
                    }
                    if (sRowNumber.Trim() != "")
                        ThrowBlankDataException(iColCount, sRowNumber.Substring(0, sRowNumber.Length - 2));
                    //else if(sInvalidReceiptNo.Trim() != string.Empty)
                    //    throw new InvalidVehicleDataExceptions(S_VALID_RECEIPT_NUMBER + sInvalidReceiptNo.Substring(0, sInvalidReceiptNo.Length - 2) + ".");
                }
                else if (iColCount == I_XLS_C_READINGFROM.ToInt())
                {
                    string sInvalidFrom = string.Empty;
                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        string sFrom = aoDataTable.Rows[iRowcount][iColCount].ToString();
                        if (sFrom.Trim() != string.Empty && sFrom.Trim() != Constants.S_ZERO)                            
                        {
                            int result;
                            bool bIsValid = Int32.TryParse(sFrom, out result);
                            if (!bIsValid)
                                sInvalidFrom = sInvalidFrom + (iRowcount + 1).ToString() + ", ";
                        }
                    }
                    if (sInvalidFrom.Trim() != string.Empty)
                        throw new InvalidVehicleDataExceptions(S_VALID_READING_FROM + sInvalidFrom.Substring(0, sInvalidFrom.Length - 2) + ".");
                }
                else if (iColCount == I_XLS_C_READINGTO.ToInt())
                {
                    string sInvalidTo = string.Empty;
                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        string sTo = aoDataTable.Rows[iRowcount][iColCount].ToString();
                        if (sTo.Trim() == string.Empty || sTo.Trim() == Constants.S_ZERO)
                            sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                        else
                        {
                            int result;
                            bool bIsValid = Int32.TryParse(sTo, out result);
                            if (!bIsValid)
                                sInvalidTo = sInvalidTo + (iRowcount + 1).ToString() + ", ";
                        }
                    }
                    if (sRowNumber.Trim() != "")
                        ThrowBlankDataException(iColCount, sRowNumber.Substring(0, sRowNumber.Length - 2));
                    else if (sInvalidTo.Trim() != string.Empty)
                        throw new InvalidVehicleDataExceptions(S_VALID_READING_TO + sInvalidTo.Substring(0, sInvalidTo.Length - 2) + ".");
                }
                else if (iColCount == I_XLS_C_LITTERS.ToInt())
                {
                    string sInvalidLitter = string.Empty;
                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        string sLitters = aoDataTable.Rows[iRowcount][iColCount].ToString();
                        if (sLitters.Trim() == string.Empty || sLitters.Trim() == Constants.S_ZERO)
                            sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                        else
                        {
                            decimal result;
                            bool bIsValid = decimal.TryParse(sLitters, out result);
                            if (!bIsValid)
                                sInvalidLitter = sInvalidLitter + (iRowcount + 1).ToString() + ", ";
                        }
                    }
                    if (sRowNumber.Trim() != "")
                        ThrowBlankDataException(iColCount, sRowNumber.Substring(0, sRowNumber.Length - 2));
                    else if (sInvalidLitter.Trim() != string.Empty)
                        throw new InvalidVehicleDataExceptions(S_VALID_LITTERS + sInvalidLitter.Substring(0, sInvalidLitter.Length - 2) + ".");
                }
                else if (iColCount == I_XLS_C_PERLITTERCOST.ToInt())
                {
                    string sInvalidPerLitter = string.Empty;
                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        string sLitPerCost = aoDataTable.Rows[iRowcount][iColCount].ToString();
                        if (sLitPerCost.Trim() == string.Empty || sLitPerCost.Trim() == Constants.S_ZERO)
                            sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                        else
                        {
                            decimal result;
                            bool bIsValid = decimal.TryParse(sLitPerCost, out result);
                            if (!bIsValid)
                                sInvalidPerLitter = sInvalidPerLitter + (iRowcount + 1).ToString() + ", ";
                        }
                    }
                    if (sRowNumber.Trim() != "")
                        ThrowBlankDataException(iColCount, sRowNumber.Substring(0, sRowNumber.Length - 2));
                    else if (sInvalidPerLitter.Trim() != string.Empty)
                        throw new InvalidVehicleDataExceptions(S_VALID_PER_LITTERS + sInvalidPerLitter.Substring(0, sInvalidPerLitter.Length - 2) + ".");
                }
                else if (iColCount == I_XLS_C_TOT_COST.ToInt())
                {
                    string sInvalidTotalCost = string.Empty;
                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        string sTotalCost = aoDataTable.Rows[iRowcount][iColCount].ToString();
                        if (sTotalCost.Trim() == string.Empty || sTotalCost.Trim() == Constants.S_ZERO)
                            sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                        else
                        {
                            decimal result;
                            bool bIsValid = decimal.TryParse(sTotalCost, out result);
                            if (!bIsValid)
                                sInvalidTotalCost = sInvalidTotalCost + (iRowcount + 1).ToString() + ", ";
                        }
                    }
                    if (sRowNumber.Trim() != "")
                        ThrowBlankDataException(iColCount, sRowNumber.Substring(0, sRowNumber.Length - 2));
                    else if (sInvalidTotalCost.Trim() != string.Empty)
                        throw new InvalidVehicleDataExceptions(S_VALID_TOTAL_COST + sInvalidTotalCost.Substring(0, sInvalidTotalCost.Length - 2) + ".");
                }
            }

            return true;
        }

        public bool ReadyToUploadMaintenanceDetails(DataTable aoDataTable, int aiSchoolId, int aiAcademicYearId)
        {
            string sBlankVehicleNo = "", sInvalidVehicleNo = "";
            string sVehicleNo = "";            
            string sRowNumber = "";

            VehicleReadingAllocationBL moVehicleReadingAllocationBL = new VehicleReadingAllocationBL();
            List<string> lstVehicleNo = moVehicleReadingAllocationBL.GetVehicleNumbers(aiSchoolId, aiAcademicYearId);

            VehicleMaintenanceExpensesDC oVehicleMaintenanceExpensesDC = new VehicleMaintenanceExpensesDC();
            List<Maintanance> lstMaintenanceTypes = oVehicleMaintenanceExpensesDC.GetMaintenanceTypeList();

            for (int iColCount = 0; iColCount < aoDataTable.Columns.Count; iColCount++)
            {
                if (iColCount == I_XLS_C_VEHICLENO.ToInt())
                {
                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        sVehicleNo = aoDataTable.Rows[iRowcount][iColCount].ToString();

                        if (string.IsNullOrEmpty(sVehicleNo))
                            sBlankVehicleNo = sBlankVehicleNo + (iRowcount + 1).ToString() + ", ";
                        else if (lstVehicleNo.Contains(sVehicleNo) == false)
                            sInvalidVehicleNo = sInvalidVehicleNo + (iRowcount + 1).ToString() + ", ";
                    }

                    if (sBlankVehicleNo.Trim() != "")
                        ThrowBlankMaintenanceDataException(iColCount, sBlankVehicleNo.Substring(0, sBlankVehicleNo.Length - 2));
                    else if (sInvalidVehicleNo.Trim() != "")
                        throw new InvalidVehicleDataExceptions(S_VALID_VEHICLE_NO + sInvalidVehicleNo.Substring(0, sInvalidVehicleNo.Length - 2) + ".");
                }
                else if (iColCount == I_XLS_C_MAINT_DATE.ToInt())
                {
                    sRowNumber = string.Empty;
                    string sInvalidDate = string.Empty, sFutureDate = string.Empty;
                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        string sMaintenanceDate = aoDataTable.Rows[iRowcount][iColCount].ToString();
                        if (sMaintenanceDate == string.Empty)
                        {
                            sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                        }
                        else
                        {
                            DateTime result;
                            bool bIsValid = DateTime.TryParse(sMaintenanceDate, out result);

                            if (!bIsValid)
                                sInvalidDate = sInvalidDate + (iRowcount + 1).ToString() + ", ";
                            else if (result.Date > DateTime.Now.Date)
                            {
                                sFutureDate = sFutureDate + (iRowcount + 1).ToString() + ", ";
                            }
                        }
                    }
                    if (sRowNumber.Trim() != "")
                        ThrowBlankMaintenanceDataException(iColCount, sRowNumber.Substring(0, sRowNumber.Length - 2));
                    else if (sInvalidDate.Trim() != "")
                        throw new InvalidVehicleDataExceptions(S_VALID_MAINT_DATE + sInvalidDate.Substring(0, sInvalidDate.Length - 2) + ".");
                    else if (sFutureDate.Trim() != "")
                        throw new InvalidVehicleDataExceptions(S_VALID_MAIN_FUTURE_DATE + sFutureDate.Substring(0, sFutureDate.Length - 2) + ".");
                }
                else if (iColCount == I_XLS_C_MAINT_BILLDATE.ToInt())
                {
                    sRowNumber = string.Empty;
                    string sInvalidDate = string.Empty, sFutureDate = string.Empty;
                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        string sBilLDate = aoDataTable.Rows[iRowcount][iColCount].ToString();
                        if (sBilLDate == string.Empty)
                        {
                            sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                        }
                        else
                        {
                            DateTime result;
                            bool bIsValid = DateTime.TryParse(sBilLDate, out result);

                            if (!bIsValid)
                                sInvalidDate = sInvalidDate + (iRowcount + 1).ToString() + ", ";
                            else if (result.Date > DateTime.Now.Date)
                            {
                                sFutureDate = sFutureDate + (iRowcount + 1).ToString() + ", ";
                            }
                        }
                    }
                    if (sRowNumber.Trim() != "")
                        ThrowBlankMaintenanceDataException(iColCount, sRowNumber.Substring(0, sRowNumber.Length - 2));
                    else if (sInvalidDate.Trim() != "")
                        throw new InvalidVehicleDataExceptions(S_VALID_MAINT_BILL_DATE + sInvalidDate.Substring(0, sInvalidDate.Length - 2) + ".");
                    else if (sFutureDate.Trim() != "")
                        throw new InvalidVehicleDataExceptions(S_VALID_MAIN_FUTURE_BILL_DATE + sFutureDate.Substring(0, sFutureDate.Length - 2) + ".");
                }
                else if (iColCount == I_XLS_C_MAINT_EXPDATE.ToInt())
                {
                    sRowNumber = string.Empty;
                    string sInvalidDate = string.Empty, sFutureDate = string.Empty;
                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        string sExpiryDate = aoDataTable.Rows[iRowcount][iColCount].ToString();
                        if (sExpiryDate != string.Empty)
                        {
                            DateTime result;
                            bool bIsValid = DateTime.TryParse(sExpiryDate, out result);

                            if (!bIsValid)
                                sInvalidDate = sInvalidDate + (iRowcount + 1).ToString() + ", ";
                        }
                    }

                    if (sInvalidDate.Trim() != "")
                        throw new InvalidVehicleDataExceptions(S_VALID_MAINT_EXPIRY_DATE + sInvalidDate.Substring(0, sInvalidDate.Length - 2) + ".");
                }
                else if (iColCount == I_XLS_C_MAINT_METERREADING.ToInt())
                {
                    string sInvalidTo = string.Empty;
                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        string sMeterReading = aoDataTable.Rows[iRowcount][iColCount].ToString();
                        if (sMeterReading.Trim() != string.Empty && sMeterReading.Trim() != Constants.S_ZERO)                            
                        {
                            int result;
                            bool bIsValid = Int32.TryParse(sMeterReading, out result);
                            if (!bIsValid)
                                sInvalidTo = sInvalidTo + (iRowcount + 1).ToString() + ", ";
                        }
                    }
                    if (sInvalidTo.Trim() != string.Empty)
                        throw new InvalidVehicleDataExceptions(S_VALID_MAINT_METER_READING + sInvalidTo.Substring(0, sInvalidTo.Length - 2) + ".");
                }
                else if (iColCount == I_XLS_C_MAINT_BILLNO.ToInt())
                {
                    string sInvalidBillNo = string.Empty;
                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        string sBillNo = aoDataTable.Rows[iRowcount][iColCount].ToString();
                        if (sBillNo.Trim() == string.Empty || sBillNo.Trim() == Constants.S_ZERO)
                            sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                        //else
                        //{
                        //    int result;
                        //    bool bIsValid = Int32.TryParse(sBillNo, out result);
                        //    if (!bIsValid)
                        //        sInvalidBillNo = sInvalidBillNo + (iRowcount + 1).ToString() + ", ";
                        //}
                    }
                    if (sRowNumber.Trim() != "")
                        ThrowBlankMaintenanceDataException(iColCount, sRowNumber.Substring(0, sRowNumber.Length - 2));
                    //else if (sInvalidBillNo.Trim() != string.Empty)
                    //    throw new InvalidVehicleDataExceptions(S_VALID_MAINT_BILL_NO + sInvalidBillNo.Substring(0, sInvalidBillNo.Length - 2) + ".");
                }
                else if (iColCount == I_XLS_C_MAINT_WORKSHOP.ToInt())
                {
                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        string sWorkshop = aoDataTable.Rows[iRowcount][iColCount].ToString();
                        if (sWorkshop.Trim() == string.Empty)
                            sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";                        
                    }
                    if (sRowNumber.Trim() != "")
                        ThrowBlankMaintenanceDataException(iColCount, sRowNumber.Substring(0, sRowNumber.Length - 2));                    
                }
                else if (iColCount == I_XLS_C_MAINT_LABOUR_CHARGES.ToInt())
                {
                    string sInvalidLabourCharges = string.Empty;
                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        string sLabourCharges = aoDataTable.Rows[iRowcount][iColCount].ToString();
                        if (sLabourCharges.Trim() != string.Empty)                        
                        {
                            decimal result;
                            bool bIsValid = decimal.TryParse(sLabourCharges, out result);
                            if (!bIsValid)
                                sInvalidLabourCharges = sInvalidLabourCharges + (iRowcount + 1).ToString() + ", ";
                        }
                    }

                    if (sInvalidLabourCharges.Trim() != string.Empty)
                        throw new InvalidVehicleDataExceptions(S_VALID_MAINT_LABOUR_CHARGES + sInvalidLabourCharges.Substring(0, sInvalidLabourCharges.Length - 2) + ".");
                }
                else if (iColCount == I_XLS_C_MAINT_MAINT_TYPE.ToInt())
                {
                    string sInvalidMainteNanceTypes = string.Empty;
                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        string sMaintenanceType = aoDataTable.Rows[iRowcount][iColCount].ToString();
                        if (sMaintenanceType.Trim() == string.Empty)
                            sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                        else
                        {
                            if(!lstMaintenanceTypes.Any(mt=> mt.MaintenanceType == sMaintenanceType))
                                sInvalidMainteNanceTypes = sInvalidMainteNanceTypes + (iRowcount + 1).ToString() + ", ";
                        }
                    }
                    if (sRowNumber.Trim() != "")
                        ThrowBlankMaintenanceDataException(iColCount, sRowNumber.Substring(0, sRowNumber.Length - 2));
                    else if (sInvalidMainteNanceTypes.Trim() != "")
                        throw new InvalidVehicleDataExceptions(S_VALID_MAINT_TYPE + sInvalidMainteNanceTypes.Substring(0, sInvalidMainteNanceTypes.Length - 2) + ".");
                }
                else if (iColCount == I_XLS_C_MAINT_QTY.ToInt())
                {
                    string sInvalidQuantity = string.Empty;
                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        string sQuantity = aoDataTable.Rows[iRowcount][iColCount].ToString();
                        if (sQuantity.Trim() != string.Empty)
                        {
                            decimal result;
                            bool bIsValid = decimal.TryParse(sQuantity, out result);
                            if (!bIsValid)
                                sInvalidQuantity = sInvalidQuantity + (iRowcount + 1).ToString() + ", ";
                        }
                    }

                    if (sInvalidQuantity.Trim() != string.Empty)
                        throw new InvalidVehicleDataExceptions(S_VALID_MAINT_LABOUR_CHARGES + sInvalidQuantity.Substring(0, sInvalidQuantity.Length - 2) + ".");
                }
                else if (iColCount == I_XLS_C_MAINT_RATE.ToInt())
                {
                    string sInvalidRate = string.Empty;
                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        string sRate = aoDataTable.Rows[iRowcount][iColCount].ToString();
                        if (sRate.Trim() != string.Empty)
                        {
                            decimal result;
                            bool bIsValid = decimal.TryParse(sRate, out result);
                            if (!bIsValid)
                                sInvalidRate = sInvalidRate + (iRowcount + 1).ToString() + ", ";
                        }
                    }

                    if (sInvalidRate.Trim() != string.Empty)
                        throw new InvalidVehicleDataExceptions(S_VALID_MAINT_RATE + sInvalidRate.Substring(0, sInvalidRate.Length - 2) + ".");
                }
                else if (iColCount == I_XLS_C_MAINT_CHARGES.ToInt())
                {
                    string sInvalidCharges = string.Empty;
                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        string sCharges = aoDataTable.Rows[iRowcount][iColCount].ToString();
                        if (sCharges.Trim() != string.Empty)
                        {
                            decimal result;
                            bool bIsValid = decimal.TryParse(sCharges, out result);
                            if (!bIsValid)
                                sInvalidCharges = sInvalidCharges + (iRowcount + 1).ToString() + ", ";
                        }
                    }

                    if (sInvalidCharges.Trim() != string.Empty)
                        throw new InvalidVehicleDataExceptions(S_VALID_MAINT_CHARGES + sInvalidCharges.Substring(0, sInvalidCharges.Length - 2) + ".");
                }
                else if (iColCount == I_XLS_C_MAINT_TOTAL_AMT.ToInt())
                {
                    string sInvalidTotalAmt = string.Empty;
                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        string sTotalAmt = aoDataTable.Rows[iRowcount][iColCount].ToString();
                        if (sTotalAmt.Trim() == string.Empty || sTotalAmt.Trim() == Constants.S_ZERO)
                            sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                        else
                        {
                            decimal result;
                            bool bIsValid = decimal.TryParse(sTotalAmt, out result);
                            if (!bIsValid)
                                sInvalidTotalAmt = sInvalidTotalAmt + (iRowcount + 1).ToString() + ", ";
                        }
                    }
                    if (sRowNumber.Trim() != "")
                        ThrowBlankMaintenanceDataException(iColCount, sRowNumber.Substring(0, sRowNumber.Length - 2));
                    else if (sInvalidTotalAmt.Trim() != string.Empty)
                        throw new InvalidVehicleDataExceptions(S_VALID_MAINT_TOTAL_AMT + sInvalidTotalAmt.Substring(0, sInvalidTotalAmt.Length - 2) + ".");
                }
            }

            return true;
        }
        
        public string GetAdminStaffXMLStringFromXLSRows(DataTable aoDataTable, string asRootElementName, string asElementName)
        {
            const string S_ELEMENT = "element";
            string sAtrrName;
            XmlAttribute attr;
            XmlDocument oDoc = new XmlDocument();
            // Create a root level element.
            XmlElement root = oDoc.CreateElement(asRootElementName);
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, asRootElementName, "");

            MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();

            ArrayList oArrayList = new ArrayList();
            oArrayList.Add("Salutation_Id");
            oArrayList.Add("Supervisor_First_Name");
            oArrayList.Add("Supervisor_Middle_Name");
            oArrayList.Add("Supervisor_Last_Name");
            oArrayList.Add("Date_of_Birth");
            oArrayList.Add("Designation_Id");
            oArrayList.Add("Email_Address");
            oArrayList.Add("Mobile_Number");
            oArrayList.Add("EmergencyContactNumber");
            oArrayList.Add("Address");
            oArrayList.Add("User_Login");
            oArrayList.Add("User_Password");
            oArrayList.Add("PanNo");
            oArrayList.Add("JoiningDate");
            oArrayList.Add("PermanentDate");
            oArrayList.Add("ResignationDate");

            if (CheckForAdminStaffMandatoryFields(aoDataTable))
            {
                for (int iRowCount = 0; iRowCount <= aoDataTable.Rows.Count - 1; iRowCount++)
                {
                    // Create root xml element.
                    XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, asElementName, string.Empty);
                    DataRow oDataRow = aoDataTable.Rows[iRowCount];

                    // Loop through all the columns for the row.
                    string sLoginName = string.Empty, sPassword = string.Empty;
                    sPassword = aoDataTable.Rows[iRowCount][I_XLS_AS_PWD].ToString();
                    sLoginName = aoDataTable.Rows[iRowCount][I_XLS_AS_LOGIN].ToString().Trim();

                    for (int iCount = 0; iCount < oArrayList.Count; iCount++)
                    {
                        sAtrrName = oArrayList[iCount].ToString().Trim();
                        attr = oDoc.CreateAttribute(sAtrrName);

                        if (sAtrrName == "Salutation_Id")
                        {
                            string sSalutaion = aoDataTable.Rows[iRowCount][iCount].ToString().Trim();
                            attr.Value = oMasterDataCollectionBL.GetSalutationIdForSalutationName(sSalutaion).ToString();
                        }

                        else if (sAtrrName == "Designation_Id")
                        {
                            string sDesignation = aoDataTable.Rows[iRowCount][iCount].ToString().Trim();
                            attr.Value = oMasterDataCollectionBL.GetDesignationIdForDesignationName(sDesignation).ToString();
                        }
                        else
                        {
                            attr.Value = aoDataTable.Rows[iRowCount][iCount].ToString().Trim();
                        }
                        oXmlNode.Attributes.Append(attr);
                    }
                    if (oXmlNode.Attributes.Count > 0)
                    {
                        XmlAttribute attrib = oDoc.CreateAttribute("User_Password");
                        attrib.Value = Utility.CommonUtility.GetEncryptedPassword(sLoginName, sPassword);
                        oXmlNode.Attributes.Append(attrib);
                    }
                    // Add the node to root node.
                    oXmlRootNode.AppendChild(oXmlNode);
                }
            }
            // Add the root node to document element. 
            root.AppendChild(oXmlRootNode);

            // return the string generated.
            return root.InnerXml;
        }

        public string GetTeacherXMLStringFromXLSRows(DataTable aoDataTable, string asRootElementName, string asElementName)
        {
            const string S_ELEMENT = "element";
            XmlDocument oDoc = new XmlDocument();
            string sAtrrName;
            XmlAttribute attr;
            // Create a root level element.
            XmlElement root = oDoc.CreateElement(asRootElementName);
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, asRootElementName, "");

            MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();

            ArrayList oArrayList = new ArrayList();
            oArrayList.Add("Salutation_Id");
            oArrayList.Add("Teacher_First_Name");
            oArrayList.Add("Teacher_Middle_Name");
            oArrayList.Add("Teacher_Last_Name");
            oArrayList.Add("Designation_Id");
            oArrayList.Add("Is_Temporary");
            oArrayList.Add("Phone_Number");
            oArrayList.Add("Mobile_Number");
            oArrayList.Add("Date_of_Birth");
            oArrayList.Add("Nationality");
            oArrayList.Add("Religion_Id");
            oArrayList.Add("CasteAndSubCaste");
            oArrayList.Add("Category_Id");
            oArrayList.Add("Local_Address");
            oArrayList.Add("Local_City");
            oArrayList.Add("Local_State");
            oArrayList.Add("Local_Pincode");
            oArrayList.Add("Permanent_Address");
            oArrayList.Add("Permanent_City");
            oArrayList.Add("Permanent_State");
            oArrayList.Add("Permanent_Pincode");
            oArrayList.Add("Exprince_In_Years");
            oArrayList.Add("Exprince_In_Months");
            oArrayList.Add("JoiningDate");
            oArrayList.Add("Achivements");

            // Teacher_Education_Details
            oArrayList.Add("Qualification_Id");
            oArrayList.Add("Year_Of_Passing");
            oArrayList.Add("Class_Id");
            oArrayList.Add("Passing_University");

            // Teacher_Standard_Details
            oArrayList.Add("Standard_Id");

            // Teacher_Subject_Details
            oArrayList.Add("Subject_Id");

            // User_Master
            oArrayList.Add("Email_Address");
            oArrayList.Add("User_Login");
            oArrayList.Add("User_Password");

            //Past Experience Deetails
            oArrayList.Add("Last_School_Name");//col 34
            oArrayList.Add("Last_School_Joined_Date");//col 35
            oArrayList.Add("Last_School_Left_Date");//col 36

            oArrayList.Add("EmergencyContactNumber");
            oArrayList.Add("PanNo");
            oArrayList.Add("PermanentDate");
            oArrayList.Add("ResignationDate");
            oArrayList.Add("AssocStandardCategory");
            oArrayList.Add("IsONCHB");
            oArrayList.Add("Date_of_Retirement"); // DOB + 58



            if (CheckForTeacherMandatoryFields(aoDataTable))
            {
                ValidateTeacherDetails(aoDataTable);
                // Loop through all the grid rows.
                for (int iRowCount = 0; iRowCount <= aoDataTable.Rows.Count - 1; iRowCount++)
                {
                    // Create root xml element.
                    XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, asElementName, "");
                    DataRow oDataRow = aoDataTable.Rows[iRowCount];

                    // Loop through all the columns for the row.
                    string sLoginName = "", sPassword = "";
                    sPassword = aoDataTable.Rows[iRowCount][I_XLS_T_PWD].ToString();
                    sLoginName = aoDataTable.Rows[iRowCount][I_XLS_T_LOGIN].ToString().Trim();

                    for (int iCount = 0; iCount < oArrayList.Count; iCount++)
                    {
                        sAtrrName = oArrayList[iCount].ToString().Trim();
                        attr = oDoc.CreateAttribute(sAtrrName);
                        if (sAtrrName == "Salutation_Id")
                        {
                            string sSalutaion = aoDataTable.Rows[iRowCount][iCount].ToString().Trim();
                            attr.Value = oMasterDataCollectionBL.GetSalutationIdForSalutationName(sSalutaion).ToString();
                        }

                        else if (sAtrrName == "Designation_Id")
                        {
                            string sDesignation = aoDataTable.Rows[iRowCount][iCount].ToString().Trim();
                            attr.Value = oMasterDataCollectionBL.GetDesignationIdForDesignationName(sDesignation).ToString();

                        }
                        else if (sAtrrName == "Religion_Id")
                        {
                            string sReligion = aoDataTable.Rows[iRowCount][iCount].ToString().Trim();
                            attr.Value = oMasterDataCollectionBL.GetReligionIdForReligionName(sReligion).ToString();
                        }

                        else if (sAtrrName == "Category_Id")
                        {
                            string sCategory = aoDataTable.Rows[iRowCount][iCount].ToString().Trim();
                            attr.Value = oMasterDataCollectionBL.GetCategoryIdForCategory(sCategory).ToString();
                        }

                        else if (sAtrrName == "Qualification_Id")
                        {
                            string asQuali = aoDataTable.Rows[iRowCount][iCount].ToString().Trim();
                            attr.Value = oMasterDataCollectionBL.GetQualiIdForQualiName(asQuali).ToString();
                        }

                        else if (sAtrrName == "Class_Id")
                        {
                            string sClass = aoDataTable.Rows[iRowCount][iCount].ToString().Trim();
                            attr.Value = oMasterDataCollectionBL.GetClassIdForClassName(sClass).ToString();
                        }

                        else if (sAtrrName == "Date_of_Retirement")
                        {
                            DateTime dtBithDate = Convert.ToDateTime(aoDataTable.Rows[iRowCount]["Date of Birth"]);
                            attr.Value = dtBithDate.AddYears(58).ToString();
                        }
                        else if (sAtrrName == "AssocStandardCategory")
                        {
                            string sAssociatedCategory = aoDataTable.Rows[iRowCount][iCount].ToString().TrimAll();
                            if (sAssociatedCategory == "Pre – Primary")
                                attr.Value = Constants.S_ONE;
                            else if (sAssociatedCategory == "Primary")
                                attr.Value = Constants.S_TWO;
                            else
                                attr.Value = Constants.S_ZERO;
                        }
                        else if (sAtrrName == "IsONCHB")
                        {
                            string sIsOnCHK = aoDataTable.Rows[iRowCount][iCount].ToString().Trim();
                            if (sIsOnCHK == "Yes")
                                attr.Value = Constants.S_ONE;
                            else
                                attr.Value = Constants.S_ZERO;
                        }
                        else
                        {
                            attr.Value = aoDataTable.Rows[iRowCount][iCount].ToString().Trim();
                        }
                        if (iCount != 33)
                            oXmlNode.Attributes.Append(attr);
                    }
                    if (oXmlNode.Attributes.Count > 0)
                    {
                        XmlAttribute attrib = oDoc.CreateAttribute("User_Password");
                        attrib.Value = Utility.CommonUtility.GetEncryptedPassword(sLoginName, sPassword);
                        oXmlNode.Attributes.Append(attrib);
                    }
                    // Add the node to root node.
                    oXmlRootNode.AppendChild(oXmlNode);
                }
            }
            // Add the root node to document element. 
            root.AppendChild(oXmlRootNode);

            // return the string generated.
            return root.InnerXml;
        }

        /// <summary>
        /// Validates Teacher Details
        /// </summary>
        /// <param name="aoDataTable"></param>
        /// <returns></returns>
        private void ValidateTeacherDetails(DataTable aoDataTable)
        {
            for (int iColCount = 0; iColCount < aoDataTable.Columns.Count; iColCount++)
            {
                string sRowNumber = String.Empty;
                string sContents = String.Empty;
                for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                {
                    sContents = aoDataTable.Rows[iRowcount][iColCount].ToString().Trim();
                    if (!ValidateTeacherData(iColCount, sContents))
                        sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                }

                if (sRowNumber.Trim() != String.Empty)
                    ThrowTeacherDataValidException(iColCount, sRowNumber.Substring(0, sRowNumber.Length - 2));
            }
        }

        /// <summary>
        /// Server side validation of teacher data
        /// </summary>
        /// <param name="aiColCount"></param>
        /// <param name="sContents"></param>
        /// <returns></returns>
        private bool ValidateTeacherData(int aiColCount, string sContents)
        {
            // DateTime oDate,oDateNow=DateTime.Now.ToDateTime();
            MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
            switch (aiColCount)
            {
                case I_XLS_T_SAL_ID: if (oMasterDataCollectionBL.GetSalutationIdForSalutationName(sContents) == 0)  //Salutation
                        return false;
                    break;
                case I_XLS_T_FIRST_NAME: if (sContents.Length > 50)  //First Name
                        return false;
                    break;
                case I_XLS_T_MIDDLE_NAME: if (sContents.Length > 1)   //Middle Name
                        return false;
                    break;
                case I_XLS_T_LAST_NAME: if (sContents.Length > 50)   //Last Name
                        return false;
                    break;
                case I_XLS_T_DEGN_ID: if (oMasterDataCollectionBL.GetDesignationIdForDesignationName(sContents.Trim()) == 0)   //Designation
                        return false;
                    break;
                case I_XLS_T_IS_TEMP: if (!(sContents.Trim().Equals(Constants.S_YES) || sContents.Trim().Equals(Constants.S_NO))) //Is Temparary
                        return false;
                    break;
                case I_XLS_T_PHONE: if (sContents.Length > 15)    //Phone Number
                        return false;
                    break;
                case I_XLS_T_MOBILE: if (sContents.Length > 10)     //Mobile Number
                        return false;
                    break;
                case I_XLS_T_NATION: if (sContents.Length > 50)     //Nationality
                        return false;
                    break;
                case I_XLS_T_RELIGION: if (oMasterDataCollectionBL.GetReligionIdForReligionName(sContents.Trim()) == 0)   //Religion
                        return false;
                    break;
                case I_XLS_T_CASTE_SUB_CASTE: if (sContents.Length > 100)    //Caste - subcaste
                        return false;
                    break;
                case I_XLS_T_CATEGORY: if (oMasterDataCollectionBL.GetCategoryIdForCategory(sContents.Trim()) == 0)   //Category
                        return false;
                    break;
                case I_XLS_T_L_ADDRESS: if (sContents.Length > 200)    //Local Address
                        return false;
                    break;
                case I_XLS_T_L_CITY: if (sContents.Length > 50)     //City
                        return false;
                    break;
                case I_XLS_T_L_STATE: if (sContents.Length > 50)     //State
                        return false;
                    break;
                case I_XLS_T_L_PINCODE: if (sContents.Length > 6)      //Pincode
                        return false;
                    break;
                case I_XLS_T_P_ADDRESS: if (sContents.Length > 200)    //Permanent Address
                        return false;
                    break;
                case I_XLS_T_P_CITY: if (sContents.Length > 50)     //City
                        return false;
                    break;
                case I_XLS_T_P_STATE: if (sContents.Length > 50)     //State
                        return false;
                    break;
                case I_XLS_T_P_PINCODE: if (sContents.Length > 6)      //Pincode
                        return false;
                    break;
                case I_XLS_T_EXP_YR: if (sContents.Length > 2)      //Past Experience Years
                        return false;
                    break;
                case I_XLS_T_EXP_MON: if (sContents.Length > 2)      //Past Experience Months
                        return false;
                    break;
                case I_XLS_T_ACHIEVE: if (sContents.Length > 4000)   //Achievements
                        return false;
                    break;
                case I_XLS_T_QUALI: if (oMasterDataCollectionBL.GetQualiIdForQualiName(sContents.Trim()) == 0)     //Qualification
                        return false;
                    break;
                case I_XLS_T_PASS_YR: if (sContents.Length != 4)     //Year of passing
                        return false;
                    break;
                case I_XLS_T_CLASS: if (oMasterDataCollectionBL.GetClassIdForClassName(sContents.Trim()) == 0)    //Class
                        return false;
                    break;
                case I_XLS_T_UNI: if (sContents.Length > 100)    //University
                        return false;
                    break;
                case I_XLS_T_STD_ID: if (sContents.Length > 5)      //StdId
                        return false;
                    break;
                case I_XLS_T_SUB_ID: if (sContents.Length > 5)      //SubId
                        return false;
                    break;
                case I_XLS_T_EMAIL: if (sContents.Length > 50)     //Email
                        return false;
                    break;
                case I_XLS_T_LOGIN: if (sContents.Length > 20)     //Login
                        return false;
                    break;
                case I_XLS_T_PWD: if (sContents.Length > 15)     //Password
                        return false;
                    break;
                case I_XLS_P_EXP_SCHOOL_NAME: if (sContents.Length > 50)     //Last school
                        return false;
                    break;
                case I_XLS_T_EMERGENCY_NO: if (sContents.Length > 15)     //emergency contact
                        return false;
                    break;
                case I_XLS_T_PAN_NO: if (sContents.Length > 20)     //Pan no
                        return false;
                    break;
            }

            return true;
        }

        private void ThrowTeacherDataValidException(int iColCount, string sRowNumber)
        {
            switch (iColCount)
            {
                case I_XLS_T_SAL_ID:
                    throw new NullRegisterNumberExceptions(S_VALID_SAL_ID + sRowNumber + ".");
                case I_XLS_T_FIRST_NAME:
                    throw new NullStudentFirstNameExceptions(S_VALID_T_FIRST_NAME + sRowNumber + ".");
                case I_XLS_T_MIDDLE_NAME:
                    throw new NullStudentFirstNameExceptions(S_VALID_T_MID_NAME + sRowNumber + ".");
                case I_XLS_T_LAST_NAME:
                    throw new NullStudentFirstNameExceptions(S_VALID_T_LAST_NAME + sRowNumber + ".");
                case I_XLS_T_DEGN_ID:
                    throw new NullStudentMotherNameExceptions(S_VALID_DESGN + sRowNumber + ".");
                case I_XLS_T_IS_TEMP:
                    throw new NullStudentDateofBirthExceptions(S_VALID_IS_TEMP + sRowNumber + ".");
                case I_XLS_T_PHONE:
                    throw new NullStudentAdmissionDateExceptions(S_VALID_T_PHONE_NUMBER + sRowNumber + ".");
                case I_XLS_T_MOBILE:
                    throw new NullStudentAdmissionDateExceptions(S_VALID_T_MOBILE + sRowNumber + ".");
                case I_XLS_T_NATION:
                    throw new NullStudentSexExceptions(S_VALID_NATION + sRowNumber + ".");
                case I_XLS_T_RELIGION:
                    throw new NullStudentParentNameExceptions(S_VALID_RELIGION + sRowNumber + ".");
                case I_XLS_T_CASTE_SUB_CASTE:
                    throw new NullStudentParentNameExceptions(S_VALID_CASTE + sRowNumber + ".");
                case I_XLS_T_CATEGORY:
                    throw new NullStudentParentNameExceptions(S_VALID_CATEGORY + sRowNumber + ".");
                case I_XLS_T_L_ADDRESS:
                    throw new NullStudentAddressExceptions(S_VALID_L_ADDRESS + sRowNumber + ".");
                case I_XLS_T_L_CITY:
                    throw new NullStudentCityExceptions(S_VALID_L_CITY + sRowNumber + ".");
                case I_XLS_T_L_STATE:
                    throw new NullStudentStateExceptions(S_VALID_L_STATE + sRowNumber + ".");
                case I_XLS_T_L_PINCODE:
                    throw new NullStudentPincodeExceptions(S_VALID_L_PIN + sRowNumber + ".");
                case I_XLS_T_P_ADDRESS:
                    throw new NullStudentAddressExceptions(S_VALID_P_ADDRESS + sRowNumber + ".");
                case I_XLS_T_P_CITY:
                    throw new NullStudentCityExceptions(S_VALID_P_CITY + sRowNumber + ".");
                case I_XLS_T_P_STATE:
                    throw new NullStudentStateExceptions(S_VALID_P_STATE + sRowNumber + ".");
                case I_XLS_T_P_PINCODE:
                    throw new NullStudentPincodeExceptions(S_VALID_P_PIN + sRowNumber + ".");
                case I_XLS_T_EXP_YR:
                    throw new NullStudentPincodeExceptions(S_VALID_PAST_EXP_YEARS + sRowNumber + ".");
                case I_XLS_T_EXP_MON:
                    throw new NullStudentPincodeExceptions(S_VALID_PAST_EXP_MONTHS + sRowNumber + ".");
                case I_XLS_T_QUALI:
                    throw new NullStudentMobileExceptions(S_VALID_QUALI + sRowNumber + ".");
                case I_XLS_T_PASS_YR:
                    throw new NullStudentMobileExceptions(S_VALID_PASS_YR_LEN + sRowNumber + ".");
                case I_XLS_T_CLASS:
                    throw new NullStudentMobileExceptions(S_VALID_CLASS + sRowNumber + ".");
                case I_XLS_T_UNI:
                    throw new NullStudentMobileExceptions(S_VALID_UNI + sRowNumber + ".");
                case I_XLS_T_STD_ID:
                    throw new NullStudentMobileExceptions(S_VALID_STD_ID + sRowNumber + ".");
                case I_XLS_T_SUB_ID:
                    throw new NullStudentMobileExceptions(S_VALID_SUB_ID + sRowNumber + ".");
                case I_XLS_T_EMAIL:
                    throw new NullStudentMobileExceptions(S_VALID_EMAIL + sRowNumber + ".");
                case I_XLS_T_LOGIN:
                    throw new NullStudentMobileExceptions(S_VALID_LOGIN + sRowNumber + ".");
                case I_XLS_T_PWD:
                    throw new NullStudentMobileExceptions(S_VALID_PWD + sRowNumber + ".");
                case I_XLS_P_EXP_SCHOOL_NAME:
                    throw new NullTeacherDateofJoningExceptions(S_VALID_LAST_SCHOOL_NAME + sRowNumber + ".");
                case I_XLS_T_PAN_NO:
                    throw new NullStudentMobileExceptions(S_VALID_PAN_NO + sRowNumber + ".");
                case I_XLS_T_EMERGENCY_NO:
                    throw new NullStudentMobileExceptions(S_VALID_T_EMERGENCY_CONTACT + sRowNumber + ".");
            }
        }
        /// <summary>
        /// Checks for mandatory fields
        /// </summary>
        /// <param name="aoDataTable"></param>
        /// <returns></returns>
        private bool CheckForHealthDetailsMandatoryFields(DataTable aoDataTable)
        {
            int iColumnCount = aoDataTable.Columns.Count - 1;
            string sRowNo = string.Empty;

            for (int iColCount = 0; iColCount <= Constants.I_ZERO; iColCount++)
            {
                string sRowNumber = "";
                string sContents = "";
                for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                {
                    sContents = aoDataTable.Rows[iRowcount][iColCount].ToString().Trim();
                    if (sContents == "")
                        sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                }

                if (sRowNumber.Trim() != "")
                    ThrowAppropriateException(iColCount, sRowNumber.Substring(0, sRowNumber.Length - 2));
            }

            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                if (aoDataTable.Rows[iRowcount]["Father Weight"].ToString().Trim() == string.Empty && aoDataTable.Rows[iRowcount]["Mother Weight"].ToString().Trim() == string.Empty && aoDataTable.Rows[iRowcount]["Father Height"].ToString().Trim() == string.Empty && aoDataTable.Rows[iRowcount]["Mother Height"].ToString().Trim() == string.Empty && aoDataTable.Rows[iRowcount]["Father Aadhar Card No"].ToString().Trim() == string.Empty && aoDataTable.Rows[iRowcount]["Mother Aadhar Card No"].ToString().Trim() == string.Empty && aoDataTable.Rows[iRowcount]["Father Blood Group"].ToString().Trim() == string.Empty && aoDataTable.Rows[iRowcount]["Mother Blood Group"].ToString().Trim() == string.Empty && aoDataTable.Rows[iRowcount]["Father Date Of Birth"].ToString().Trim() == string.Empty && aoDataTable.Rows[iRowcount]["Mother Date Of Birth"].ToString().Trim() == string.Empty && aoDataTable.Rows[iRowcount]["Family Monthly Income"].ToString().Trim() == string.Empty && aoDataTable.Rows[iRowcount]["CWSN"].ToString().Trim() == string.Empty)
                {
                    sRowNo = sRowNo + (iRowcount + 1).ToString() + ", ";
                }
            }

            if (sRowNo.Trim() != "")
                throw new ApplicationException("Please enter data for at least one field (other than Registration Number) for row(s) : " + sRowNo.Substring(0, sRowNo.Length - 2) + ".");


            ValidateAadharCardNos(aoDataTable);

            return false;
        }

        private void ValidateAadharCardNos(DataTable aoDataTable)
        {
            HealthDetailsBL oHealthDetailsBL = new HealthDetailsBL(moStudentInfoStruct.iSchoolId, moStudentInfoStruct.iAcademicYearId, moStudentInfoStruct.iUserId);
            List<SiblingStudentDetails> lstSiblingStudentDetails = new List<SiblingStudentDetails>();
            List<SiblingStudentDetails> lstSiblingList = new List<SiblingStudentDetails>();

            lstSiblingStudentDetails = oHealthDetailsBL.GetSiblingStudentDetails();
            lstSiblingList.AddRange(lstSiblingStudentDetails);

            List<SiblingStudentDetails> lstSiblings = new List<SiblingStudentDetails>();
            lstSiblingStudentDetails.ForEach(std =>
            {
                lstSiblings.Add(new SiblingStudentDetails { EnrolmentNumber = std.EnrolmentNumber, YearwiseStudentId = std.YearwiseStudentId, SiblingEnrolmentNumber = std.SiblingEnrolmentNumber, SiblingStudentId = std.SiblingStudentId });

                lstSiblingStudentDetails.Where(sd => sd.YearwiseStudentId == std.SiblingStudentId).ToList().ForEach(sd =>
                {
                    lstSiblings.Add(new SiblingStudentDetails { EnrolmentNumber = std.EnrolmentNumber, YearwiseStudentId = std.YearwiseStudentId, SiblingEnrolmentNumber = sd.SiblingEnrolmentNumber, SiblingStudentId = sd.SiblingStudentId });

                    lstSiblingStudentDetails.Where(sdNext => sdNext.YearwiseStudentId == sd.SiblingStudentId).ToList().ForEach(sdNext =>
                        {
                            lstSiblings.Add(new SiblingStudentDetails { EnrolmentNumber = std.EnrolmentNumber, YearwiseStudentId = std.YearwiseStudentId, SiblingEnrolmentNumber = sdNext.SiblingEnrolmentNumber, SiblingStudentId = sdNext.SiblingStudentId });

                            lstSiblingStudentDetails.Where(sdNext1 => sdNext1.YearwiseStudentId == sdNext.SiblingStudentId).ToList().ForEach(sdNext1 =>
                            {
                                lstSiblings.Add(new SiblingStudentDetails { EnrolmentNumber = std.EnrolmentNumber, YearwiseStudentId = std.YearwiseStudentId, SiblingEnrolmentNumber = sdNext1.SiblingEnrolmentNumber, SiblingStudentId = sdNext1.SiblingStudentId });
                                lstSiblingStudentDetails.Remove(sdNext1);
                            });

                            lstSiblingStudentDetails.Remove(sdNext);
                        });

                    lstSiblingStudentDetails.Remove(sd);
                });
            });


            StringBuilder sFather = new StringBuilder();
            StringBuilder sMother = new StringBuilder();
            StringBuilder sFatherExternal = new StringBuilder();
            StringBuilder sMotherExternal = new StringBuilder();
            StringBuilder sFatherColumn = new StringBuilder();
            string sFAadharcardNo = string.Empty;
            string sMAadharcardNo = string.Empty;
            string sFirstRegNo = string.Empty;
            string sFatherAadhar = string.Empty;
            string sMotherAadhar = string.Empty;
            string sSecondRegNo = string.Empty;
            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                int iRowCnt = iRowcount + 1;
                sFAadharcardNo = aoDataTable.Rows[iRowcount]["Father Aadhar Card No"].ToString().Trim();
                sMAadharcardNo = aoDataTable.Rows[iRowcount]["Mother Aadhar Card No"].ToString().Trim();
                sFirstRegNo = aoDataTable.Rows[iRowcount]["Registration Number"].ToString().Trim();

                for (int iColCount = iRowcount + 1; iColCount < aoDataTable.Rows.Count; iColCount++)
                {
                    sFatherAadhar = aoDataTable.Rows[iColCount]["Father Aadhar Card No"].ToString().Trim();
                    sMotherAadhar = aoDataTable.Rows[iColCount]["Mother Aadhar Card No"].ToString().Trim();
                    sSecondRegNo = aoDataTable.Rows[iColCount]["Registration Number"].ToString().Trim();

                    if (sFAadharcardNo != string.Empty && sFAadharcardNo != null)
                    {
                        if (sFAadharcardNo == sFatherAadhar)
                        {
                            if (!(lstSiblingList.Any(s => s.EnrolmentNumber == sFirstRegNo && s.SiblingEnrolmentNumber == sSecondRegNo) || lstSiblingList.Any(s => s.SiblingEnrolmentNumber == sFirstRegNo && s.EnrolmentNumber == sSecondRegNo)))
                            {
                                if (!(lstSiblings.Any(s => s.EnrolmentNumber == sFirstRegNo && s.SiblingEnrolmentNumber == sSecondRegNo) || lstSiblings.Any(s => s.SiblingEnrolmentNumber == sFirstRegNo && s.EnrolmentNumber == sSecondRegNo)))
                                    sFather.Append((iColCount + 1).ToString() + ", ");
                            }
                        }
                    }

                    if (sMAadharcardNo != string.Empty && sMAadharcardNo != null)
                    {
                        if (sMAadharcardNo == sMotherAadhar)
                        {
                            if (!(lstSiblingList.Any(s => s.EnrolmentNumber == sFirstRegNo && s.SiblingEnrolmentNumber == sSecondRegNo) || lstSiblingList.Any(s => s.SiblingEnrolmentNumber == sFirstRegNo && s.EnrolmentNumber == sSecondRegNo)))
                            {
                                if (!(lstSiblings.Any(s => s.EnrolmentNumber == sFirstRegNo && s.SiblingEnrolmentNumber == sSecondRegNo) || lstSiblings.Any(s => s.SiblingEnrolmentNumber == sFirstRegNo && s.EnrolmentNumber == sSecondRegNo)))
                                    sMother.Append((iColCount + 1).ToString() + ", ");
                            }
                        }
                    }
                }

                if (sFather.ToString() != string.Empty)
                    sFatherExternal.Append(iRowCnt.ToString() + "(" + sFather.ToString().Substring(0, sFather.ToString().Length - 2) + "), ");

                if (sMother.ToString() != string.Empty)
                    sMotherExternal.Append(iRowCnt.ToString() + "(" + sMother.ToString().Substring(0, sMother.ToString().Length - 2) + "), ");
                sFather.Clear();
                sMother.Clear();
            }
            string sDuplicateFather = string.Empty;
            string sDuplicateMother = string.Empty;

            if (sFatherExternal.ToString().EndsWith(", "))
                sDuplicateFather = sFatherExternal.ToString().Substring(0, sFatherExternal.ToString().Length - 2);

            if (sMotherExternal.ToString().EndsWith(", "))
                sDuplicateMother = sMotherExternal.ToString().Substring(0, sMotherExternal.ToString().Length - 2);

            if (sDuplicateFather != string.Empty && sDuplicateMother != string.Empty)
                throw new ApplicationException("Father Aadhar card no. should not be duplicate for row(s) : " + sDuplicateFather + "." + "<br/>" + "Mother Aadhar card no. should not be duplicate for row(s) : " + sDuplicateMother + ".");
            else if (sDuplicateFather != string.Empty && sDuplicateMother == string.Empty)
                throw new ApplicationException("Father Aadhar card no. should not be duplicate for row(s) : " + sDuplicateFather + ".");
            else if (sDuplicateFather == string.Empty && sDuplicateMother != string.Empty)
                throw new ApplicationException("Mother Aadhar card no. should not be duplicate for row(s) : " + sDuplicateMother + ".");

            sFAadharcardNo = "";
            sFatherColumn.Clear();
            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                sFAadharcardNo = aoDataTable.Rows[iRowcount]["Father Aadhar Card No"].ToString().Trim();

                for (int iColCount = 0; iColCount < aoDataTable.Rows.Count; iColCount++)
                {
                    sMAadharcardNo = aoDataTable.Rows[iColCount]["Mother Aadhar Card No"].ToString().Trim();

                    if (sFAadharcardNo != string.Empty && sFAadharcardNo != null && sFAadharcardNo == sMAadharcardNo)
                        sFatherColumn.Append((iColCount + 1).ToString() + ", ");
                }

                if (sFatherColumn.ToString() != string.Empty)
                    sFatherExternal.Append((iRowcount + 1).ToString() + "(" + sFatherColumn.ToString().Substring(0, sFatherColumn.ToString().Length - 2) + "), ");

                sFatherColumn.Clear();
            }
            var msg = "";
            if (sFatherExternal.ToString().EndsWith(", "))
                msg = sFatherExternal.ToString().Substring(0, sFatherExternal.ToString().Length - 2);

            if (msg != string.Empty)
                throw new ApplicationException("Father Aadhar card no. should not be duplicate with Mother Aadhar card no. for row(s) : " + msg + ".");
        }

        /// <summary>
        /// Checks for mandatory fields
        /// </summary>
        /// <param name="aoDataTable"></param>
        /// <returns></returns>
        private bool CheckForMandatoryFields(DataTable aoDataTable)
        {
            for (int iColCount = 0; iColCount < aoDataTable.Columns.Count; iColCount++)
            {
                string sRowNumber = "";
                string sContents = "";
                for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                {

                    sContents = aoDataTable.Rows[iRowcount][iColCount].ToString().Trim();
                    if (sContents == "")
                        sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                }

                if (sRowNumber.Trim() != "")
                    ThrowAppropriateException(iColCount, sRowNumber.Substring(0, sRowNumber.Length - 2));
            }
            if (!CheckIsRegistrationNumberIsDuplicate(aoDataTable))
            {
                if (!CheckIsRegNoHasValidPrefixorPostFix(aoDataTable))
                {
                    if (ValidationsForDates(aoDataTable))
                    {
                        if (CheckIsStudentIsDuplicate(aoDataTable))
                        {
                            if (CheckIsPincodeValid(aoDataTable))
                            {
                                if (CheckIsApplicableRuleOrRTE(aoDataTable))
                                {
                                    ValidateCategory(aoDataTable);
                                    ValidateRTECategory(aoDataTable);
                                    if (CheckIsMobileNumberIsValid(aoDataTable))
                                    {
                                        if (CheckIsMobileNumber2IsValid(aoDataTable))
                                        {
                                            if (ValidateFeeAreaName(aoDataTable))
                                            {
                                                ValidateCategoryForStudent(aoDataTable);
                                                if (!ValidateStudentEmailAddress(aoDataTable))
                                                {   
                                                   return false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return false;
            }
            return true;

        }

        /// <summary>
        /// Checks for teacher mandatory fields
        /// </summary>
        /// <param name="aoDataTable"></param>
        /// <returns></returns>
        private bool CheckForTeacherMandatoryFields(DataTable aoDataTable)
        {
            for (int iColCount = 0; iColCount < aoDataTable.Columns.Count; iColCount++)
            {
                string sRowNumber = "";
                string sContents = "";
                if (iColCount != I_XLS_P_EXP_SCHOOL_NAME && iColCount != I_XLS_P_EXP_SCHOOL_JOINED_DATE && iColCount != I_XLS_P_EXP_SCHOOL_LEFT_DATE && iColCount != I_XLS_IS_ON_CHB)
                {
                    if (moStudentInfoStruct.iSchoolId == Constants.SchoolId.SNS.ToInt() && iColCount == I_XLS_ASSO_STANDARD_CATEGORY)
                    {
                        for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                        {
                            sContents = aoDataTable.Rows[iRowcount][iColCount].ToString().Trim();
                            if (sContents == "")
                            {
                                sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                            }
                        }
                    }
                    else
                    {
                        if (iColCount != I_XLS_ASSO_STANDARD_CATEGORY)
                        {
                            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                            {
                                sContents = aoDataTable.Rows[iRowcount][iColCount].ToString().Trim();
                                if (sContents == "")
                                {
                                    sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                                }
                            }
                        }
                    }
                    if (sRowNumber.Trim() != "")
                        ThrowTeacherDataNullException(iColCount, sRowNumber.Substring(0, sRowNumber.Length - 2));
                }
                if (iColCount == I_XLS_P_EXP_SCHOOL_LEFT_DATE)
                    IsPastExperienceDetailsValid(aoDataTable, iColCount);
            }
            if (!CheckIsDuplicateTeacherDesignationInFile(aoDataTable))
                if (ValidationsForTeacherDates(aoDataTable))
                    if (!CheckTeacherEmailAddressDuplicateInFile(aoDataTable))
                        if (!CheckIsUserNameIsDuplicate(aoDataTable))
                            if (!IsValidPassword(aoDataTable, I_XLS_T_PWD))
                                if (CheckIsTeacherPincodeValid(aoDataTable))
                                    if (!CheckIsTeacherMobileNumberIsValid(aoDataTable))

                                        return false;

            return true;
        }



        /// <summary>
        /// Checks for admin mandatory fields
        /// </summary>
        /// <param name="aoDataTable"></param>
        /// <returns></returns>
        private bool CheckForAdminStaffMandatoryFields(DataTable aoDataTable)
        {
            for (int iColCount = 0; iColCount < aoDataTable.Columns.Count; iColCount++)
            {
                string sRowNumber = "";
                string sContents = "";
                if (iColCount != I_XLS_T_MIDDLE_NAME)
                {
                    for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                    {
                        sContents = aoDataTable.Rows[iRowcount][iColCount].ToString().Trim();
                        if (sContents == "")
                            sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                    }

                    if (sRowNumber.Trim() != "")
                        ThrowAdminStaffDataNullException(iColCount, sRowNumber.Substring(0, sRowNumber.Length - 2));
                }
            }
            if (ValidateAdminStaffEmailAddress(aoDataTable))
                if (CheckIsTeacherMobileNumberIsValid(aoDataTable))
                    if (!IsValidPassword(aoDataTable, I_XLS_AS_PWD))
                        if (ValidationsForAdminStaffDates(aoDataTable))
                            if (!CheckAdminStaffUserNameIsDuplicateInFile(aoDataTable))
                                if (CheckIsAdminStaffUserNameIsDuplicate(aoDataTable))
                                    return false;
            return true;

        }

        /// <summary>
        /// This method is used to validate Past Experienced details.
        /// </summary>
        /// <param name="aoDataTable"></param>
        private void IsPastExperienceDetailsValid(DataTable aoDataTable, int iColCount)
        {
            string sRowNumber = string.Empty;
            string sContents = string.Empty;
            int iColNo = 0;
            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                sContents = aoDataTable.Rows[iRowcount][iColCount].ToString().Trim();
                if (iColCount == I_XLS_P_EXP_SCHOOL_LEFT_DATE)
                {
                    //if Left date is not empty
                    if (aoDataTable.Rows[iRowcount][I_XLS_P_EXP_SCHOOL_NAME].ToString().Trim() != string.Empty || aoDataTable.Rows[iRowcount][I_XLS_P_EXP_SCHOOL_JOINED_DATE].ToString().Trim() != string.Empty || aoDataTable.Rows[iRowcount][I_XLS_P_EXP_SCHOOL_LEFT_DATE].ToString().Trim() != string.Empty)
                    {
                        if (aoDataTable.Rows[iRowcount][I_XLS_P_EXP_SCHOOL_NAME].ToString().Trim() != string.Empty)
                        {
                            if (aoDataTable.Rows[iRowcount][I_XLS_P_EXP_SCHOOL_JOINED_DATE].ToString().Trim() != string.Empty)
                            {
                                if (aoDataTable.Rows[iRowcount][I_XLS_P_EXP_SCHOOL_LEFT_DATE].ToString().Trim() != string.Empty)
                                {
                                    if (Convert.ToDateTime(aoDataTable.Rows[iRowcount][I_XLS_P_EXP_SCHOOL_JOINED_DATE]) > Convert.ToDateTime(sContents))
                                        IsValidJoinedANDLeftDate(aoDataTable);
                                }
                                else
                                {
                                    iColNo = I_XLS_P_EXP_SCHOOL_LEFT_DATE;
                                    sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                                }
                            }
                            else
                            {
                                iColNo = I_XLS_P_EXP_SCHOOL_JOINED_DATE;
                                sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                            }
                        }
                        else
                        {
                            iColNo = I_XLS_P_EXP_SCHOOL_NAME;
                            sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                        }
                    }
                }
            }
            if (sRowNumber.Trim() != string.Empty)
                ThrowTeacherDataNullException(iColNo, sRowNumber.Substring(0, sRowNumber.Length - 2));
        }

        /// <summary>
        /// This method is used to validate Joining and Left dates.
        /// </summary>
        /// <param name="aoDataTable"></param>
        private void IsValidJoinedANDLeftDate(DataTable aoDataTable)
        {
            string sRowNumber = string.Empty;
            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                if (Convert.ToDateTime(aoDataTable.Rows[iRowcount][I_XLS_P_EXP_SCHOOL_JOINED_DATE]) > Convert.ToDateTime(aoDataTable.Rows[iRowcount][I_XLS_P_EXP_SCHOOL_LEFT_DATE]))
                    sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
            }
            if (sRowNumber.Trim() != string.Empty)
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);
                throw new NullTeacherDateofJoningExceptions(S_VALID_LAST_SCHOOL_JOINING_DATE + sRowNumber + ".");
            }
        }

        /// <summary>
        /// Validates all dates
        /// </summary>
        /// <param name="aoDataTable"></param>
        /// <returns></returns>
        private bool ValidationsForDates(DataTable aoDataTable)
        {
            string sNew = "";
            string sJoiningNewRowOutOfAcademicYear = "";
            string sNo = "No";
            string sRowNumber = "";
            string sAdmissionRow = "";
            string sJoiningRow = "";
            string sJoiningRowOutOfAcademicYear = "";
            string sDobRow = "";
            string sBithFromat = "";
            string sAdmissionFormat = "";
            string sJoiningFormat = "";

            DateTime dtCurrent = DateTime.Today;
            DateTime dtBirth;
            DateTime dtAdmission;
            DateTime dtJoining;
            object oGrdDtBirth;
            object oGrdDtAdmission;
            object oGrdDtJoining;
            Type tDtBirth;
            Type tDtAdmission;
            Type tDtJoining;

            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                sNew = aoDataTable.Rows[iRowcount][I_XLS_NO_NEW_ADDMISSION].ToString().Trim();

                oGrdDtBirth = aoDataTable.Rows[iRowcount][I_XLS_DATE_OF_BIRTH];
                tDtBirth = oGrdDtBirth.GetType();

                if (!tDtBirth.FullName.Trim().Equals("System.DateTime"))
                {
                    sBithFromat = sBithFromat + (iRowcount + 1).ToString() + ", ";
                }

                oGrdDtAdmission = aoDataTable.Rows[iRowcount][I_XLS_ADMISSION_DATE];
                tDtAdmission = oGrdDtAdmission.GetType();
                if (!tDtAdmission.FullName.Trim().Equals("System.DateTime"))
                {
                    sAdmissionFormat = sAdmissionFormat + (iRowcount + 1).ToString() + ", ";
                }
                if (sBithFromat.Trim().Equals("") && sAdmissionFormat.Trim().Equals(""))
                {
                    dtBirth = Convert.ToDateTime(oGrdDtBirth);
                    dtAdmission = Convert.ToDateTime(oGrdDtAdmission);
                    if (dtBirth > dtCurrent)
                    {
                        sDobRow = sDobRow + (iRowcount + 1).ToString() + ", ";
                    }
                    else if (dtBirth > dtAdmission)
                    {
                        sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                    }
                    else if (dtAdmission > dtCurrent)
                    {
                        sAdmissionRow = sAdmissionRow + (iRowcount + 1).ToString() + ", ";
                    }
                }

                oGrdDtJoining = aoDataTable.Rows[iRowcount][I_XLS_JOINING_DATE];
                tDtJoining = oGrdDtJoining.GetType();
                if (!tDtJoining.FullName.Trim().Equals("System.DateTime"))
                {
                    sJoiningFormat = sJoiningFormat + (iRowcount + 1).ToString() + ", ";
                }
                if (sBithFromat.Trim().Equals("") && sJoiningFormat.Trim().Equals(""))
                {
                    dtJoining = Convert.ToDateTime(oGrdDtJoining);
                    dtAdmission = Convert.ToDateTime(oGrdDtAdmission);
                    if (dtJoining < dtAdmission)
                        sJoiningRow = sJoiningRow + (iRowcount + 1).ToString() + ", ";
                    if (sNew == sNo)
                    {
                        if (dtJoining > mdtAcademicYearEndDate)
                            sJoiningRowOutOfAcademicYear = sJoiningRowOutOfAcademicYear + (iRowcount + 1).ToString() + ", ";
                    }
                    else
                    {
                        if (dtJoining > mdtAcademicYearEndDate || dtJoining < mdtAcademicYearStartDate)
                            sJoiningNewRowOutOfAcademicYear = sJoiningNewRowOutOfAcademicYear + (iRowcount + 1).ToString() + ", ";
                    }
                }

            }
            if (sBithFromat != "")
            {
                sBithFromat = sBithFromat.Substring(0, sBithFromat.Length - 2);
                throw new NullStudentDateofBirthExceptions(CommonUtility.GetResourceValue("valDateBirthFormat") + sBithFromat + ".");
            }
            else if (sAdmissionFormat != "")
            {
                sAdmissionFormat = sAdmissionFormat.Substring(0, sAdmissionFormat.Length - 2);
                throw new NullStudentDateofBirthExceptions(CommonUtility.GetResourceValue("valAdmissionDateFormat") + sAdmissionFormat + ".");
            }
            else if (sJoiningFormat != "")
            {
                sJoiningFormat = sJoiningFormat.Substring(0, sJoiningFormat.Length - 2);
                throw new NullStudentJoiningDateExceptions(CommonUtility.GetResourceValue("valJoiningDateFormat") + sJoiningFormat + ".");
            }
            else if (sDobRow != "")
            {
                sDobRow = sDobRow.Substring(0, sDobRow.Length - 2);
                throw new NullStudentDateofBirthExceptions(CommonUtility.GetResourceValue("valDateOfBirth") + sDobRow + ".");
            }
            else if (sRowNumber != "")
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);
                throw new NullStudentDateofBirthExceptions(CommonUtility.GetResourceValue("ValDateOfBirthGreater") + sRowNumber + ".");
            }
            else if (sAdmissionRow != "")
            {
                sAdmissionRow = sAdmissionRow.Substring(0, sAdmissionRow.Length - 2);
                throw new NullStudentDateofBirthExceptions(CommonUtility.GetResourceValue("valStudentAdmissionDateGreater") + sAdmissionRow + ".");
            }
            else if (sJoiningRow != "")
            {
                sJoiningRow = sJoiningRow.Substring(0, sJoiningRow.Length - 2);
                throw new NullStudentJoiningDateExceptions(CommonUtility.GetResourceValue("ValJoiningDateGreaterAdmissionDate") + sJoiningRow + ".");
            }
            else if (sJoiningRowOutOfAcademicYear != "")
            {
                sJoiningRowOutOfAcademicYear = sJoiningRowOutOfAcademicYear.Substring(0, sJoiningRowOutOfAcademicYear.Length - 2);
                throw new NullStudentJoiningDateExceptions(S_JOINING_DATE_OUTOF_ACA_YEAR + S_EXIST_STUDENT + mdtAcademicYearEndDate.ToString("dd-MMM-yyyy") + ") " + S_COMMON + sJoiningRowOutOfAcademicYear + ".");
            }
            else if (sJoiningNewRowOutOfAcademicYear != "")
            {
                sJoiningNewRowOutOfAcademicYear = sJoiningNewRowOutOfAcademicYear.Substring(0, sJoiningNewRowOutOfAcademicYear.Length - 2);
                throw new NullStudentJoiningDateExceptions(S_JOINING_DATE_OUTOF_ACA_YEAR + S_NEW_STUDENT + mdtAcademicYearStartDate.ToString("dd-MMM-yyyy") + ") and " + S_EXIST_STUDENT + mdtAcademicYearEndDate.ToString("dd-MMM-yyyy") + ") " + S_COMMON + sJoiningNewRowOutOfAcademicYear + ".");
            }

            return true;
        }

        /// <summary>
        /// Validates all dates of teacher
        /// </summary>
        /// <param name="aoDataTable"></param>
        /// <returns></returns>
        private bool ValidationsForTeacherDates(DataTable aoDataTable)
        {
            string sRowNumber = "";
            string sJoiningRow = "";
            string sDobRow = "";
            string sDobminorRow = "";
            string sBithFromat = "";
            string sJoiningFormat = "";
            DateTime dtCurrent = DateTime.Today;
            DateTime dtBirth;
            DateTime dtJoining;
            object oGrdDtBirth;
            object oGrdDtJoining;
            Type tDtBirth;
            Type tDtJoining;
            string sPassYrFormat = "";
            string sPassYrRow = "";
            int iGrdPassYr;
            string sPassingYrRow = "";
            string sValidPassYrRow = "";

            ////////////////////////////////////////////////////////////////////////////////////
            string sJoiningDateRow = "";
            string sPermanentRow = "";
            string sResignationRow = "";
            string sJoinigDateFromat = "";
            string sPermanentDateFromat = "";
            string sResignationDateFromat = "";
            DateTime dtJoiningDate;
            DateTime dtPermanentDate;
            DateTime dtResignationDate;
            object oGrdDtJoiningDate;
            object oGrdDtPermanentDate;
            object oGrdDtResignationDate;
            Type tDtJoiningDate;
            Type tDtPermanentDate;
            Type tDtResignationDate;

            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                int iCurrentYear = dtCurrent.Year;
                iGrdPassYr = Convert.ToInt32(aoDataTable.Rows[iRowcount][I_XLS_T_PASS_YR]);
                oGrdDtBirth = aoDataTable.Rows[iRowcount][I_XLS_T_DOB];
                tDtBirth = oGrdDtBirth.GetType();

                if (!tDtBirth.FullName.Trim().Equals("System.DateTime"))
                {
                    sBithFromat = sBithFromat + (iRowcount + 1).ToString() + ", ";
                }

                oGrdDtJoining = aoDataTable.Rows[iRowcount][I_XLS_T_JOINDATE];
                if (aoDataTable.Rows[iRowcount][I_XLS_T_JOINDATE].ToString().Trim() != "")
                {
                    tDtJoining = oGrdDtJoining.GetType();
                    if (!tDtJoining.FullName.Trim().Equals("System.DateTime"))
                    {
                        sJoiningRow = sJoiningRow + (iRowcount + 1).ToString() + ", ";
                    }
                }
                if (sBithFromat.Trim().Equals("") && sJoiningRow.Trim().Equals(""))
                {
                    dtBirth = Convert.ToDateTime(oGrdDtBirth);
                    if (dtBirth > dtCurrent)
                    {
                        sDobRow = sDobRow + (iRowcount + 1).ToString() + ", ";
                    }
                    else if (dtBirth.Year + 18 > dtCurrent.Year)
                    {
                        sDobminorRow = sDobminorRow + (iRowcount + 1).ToString() + ", ";
                    }
                    if (aoDataTable.Rows[iRowcount][I_XLS_T_JOINDATE].ToString().Trim() != "")
                    {
                        dtJoining = Convert.ToDateTime(oGrdDtJoining);
                        if (dtBirth > dtJoining)
                        {
                            sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                        }
                    }
                }
                if (sPassYrFormat.Trim().Equals(""))
                {
                    if (iGrdPassYr > iCurrentYear)
                        sPassYrRow = sDobRow + (iRowcount + 1).ToString() + ", ";
                    else if (Convert.ToString(iGrdPassYr).Trim().Length != 4)
                        sPassingYrRow = sDobRow + (iRowcount + 1).ToString() + ", ";
                    else if ((iCurrentYear - iGrdPassYr) > 60)
                        sValidPassYrRow = sValidPassYrRow + (iRowcount + 1).ToString() + ", ";
                }

                /////////////////////////////////////////////////////////////////////////////////

                oGrdDtJoiningDate = aoDataTable.Rows[iRowcount][I_XLS_T_JOINDATE];
                tDtJoiningDate = oGrdDtJoiningDate.GetType();

                oGrdDtPermanentDate = aoDataTable.Rows[iRowcount][I_XLS_T_PERMANENT_DATE];
                tDtPermanentDate = oGrdDtPermanentDate.GetType();

                oGrdDtResignationDate = aoDataTable.Rows[iRowcount][I_XLS_T_RESIGNATION_DATE];
                tDtResignationDate = oGrdDtResignationDate.GetType();

                if ((aoDataTable.Rows[iRowcount][I_XLS_T_PERMANENT_DATE].ToString().Trim() != string.Empty) && (!tDtPermanentDate.FullName.Trim().Equals("System.DateTime")))
                    sPermanentDateFromat = sPermanentDateFromat + (iRowcount + 1).ToString() + ", ";

                if ((aoDataTable.Rows[iRowcount][I_XLS_T_RESIGNATION_DATE].ToString().Trim() != string.Empty) && (!tDtResignationDate.FullName.Trim().Equals("System.DateTime")))
                    sResignationDateFromat = sResignationDateFromat + (iRowcount + 1).ToString() + ", ";

                if (aoDataTable.Rows[iRowcount][I_XLS_T_JOINDATE].ToString().Trim() != string.Empty)
                {

                    if (!tDtJoiningDate.FullName.Trim().Equals("System.DateTime"))
                        sJoinigDateFromat = sJoinigDateFromat + (iRowcount + 1).ToString() + ", ";

                    if (sJoinigDateFromat.Trim().Equals("") && sPermanentDateFromat.Trim().Equals(""))
                    {
                        dtJoiningDate = Convert.ToDateTime(oGrdDtJoiningDate);
                        if (!oGrdDtPermanentDate.ToString().Trim().IsNullOrEmpty())
                        {
                            dtPermanentDate = Convert.ToDateTime(oGrdDtPermanentDate);
                            if (dtJoiningDate > dtPermanentDate)
                                sPermanentRow = sPermanentRow + (iRowcount + 1).ToString() + ", ";
                            else if ((sResignationDateFromat.Trim().Equals("")) && (!oGrdDtResignationDate.ToString().Trim().IsNullOrEmpty()))
                            {
                                dtResignationDate = Convert.ToDateTime(oGrdDtResignationDate);
                                if (dtResignationDate <= dtPermanentDate || dtResignationDate <= dtJoiningDate)
                                    sResignationRow = sResignationRow + (iRowcount + 1).ToString() + ", ";
                            }
                        }
                        else if (!oGrdDtResignationDate.ToString().Trim().IsNullOrEmpty())
                        {
                            dtResignationDate = Convert.ToDateTime(oGrdDtResignationDate);
                            if (dtResignationDate <= dtJoiningDate)
                                sResignationRow = sResignationRow + (iRowcount + 1).ToString() + ", ";
                        }
                    }

                    else if (sJoinigDateFromat.Trim().Equals("") && sResignationDateFromat.Trim().Equals(""))
                    {
                        dtJoiningDate = Convert.ToDateTime(oGrdDtJoiningDate);
                        if (!oGrdDtResignationDate.ToString().Trim().IsNullOrEmpty())
                        {
                            dtResignationDate = Convert.ToDateTime(oGrdDtResignationDate);
                            if (dtJoiningDate >= dtResignationDate && sResignationRow == "")
                                sResignationRow = sResignationRow + (iRowcount + 1).ToString() + ", ";
                        }
                    }
                }
                else if ((!oGrdDtPermanentDate.ToString().Trim().IsNullOrEmpty()) || (!oGrdDtResignationDate.ToString().Trim().IsNullOrEmpty()))
                {
                    if (sPermanentDateFromat.Trim().Equals("") || sResignationDateFromat.Trim().Equals(""))
                        sJoiningDateRow = sJoiningDateRow + (iRowcount + 1).ToString() + ", ";
                }
                //////////////////////////////////////////////////////////
            }
            if (sBithFromat != "")
            {
                sBithFromat = sBithFromat.Substring(0, sBithFromat.Length - 2);
                throw new NullStudentDateofBirthExceptions(S_FORMAT_DOB + sBithFromat + ".");
            }
            else if (sJoiningFormat != "")
            {
                sJoiningFormat = sJoiningFormat.Substring(0, sJoiningFormat.Length - 2);
                throw new NullStudentJoiningDateExceptions(S_FORMAT_JOINING_DATE + sJoiningFormat + ".");
            }
            else if (sDobRow != "")
            {
                sDobRow = sDobRow.Substring(0, sDobRow.Length - 2);
                throw new NullStudentDateofBirthExceptions(S_VALID_DOB + sDobRow + ".");
            }
            else if (sDobminorRow != "")
            {
                sDobminorRow = sDobminorRow.Substring(0, sDobminorRow.Length - 2);
                throw new NullStudentDateofBirthExceptions(S_MINOR_DOB + sDobminorRow + ".");
            }

            else if (sRowNumber != "")
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);
                throw new NullStudentDateofBirthExceptions(S_VALID_BIRTH_JOINING_DATE + sRowNumber + ".");
            }
            else if (sJoiningRow != "")
            {
                sJoiningRow = sJoiningRow.Substring(0, sJoiningRow.Length - 2);
                throw new NullStudentJoiningDateExceptions(S_VALID_JOINING_DATE1 + sJoiningRow + ".");
            }
            else if (sPassYrRow != "")
            {
                sPassYrRow = sPassYrRow.Substring(0, sPassYrRow.Length - 2);
                throw new NullStudentMobileExceptions(S_VALID_PASS_YR + sPassYrRow + ".");
            }
            else if (sPassingYrRow != "")
            {
                sPassingYrRow = sPassingYrRow.Substring(0, sPassingYrRow.Length - 2);
                throw new NullStudentMobileExceptions(S_VALID_PASS_YR_LEN + sPassingYrRow + ".");
            }
            else if (sValidPassYrRow != "")
            {
                sValidPassYrRow = sValidPassYrRow.Substring(0, sValidPassYrRow.Length - 2);
                throw new ValidMobileNumberExceptions(S_VALID_T_MOBILE + sValidPassYrRow + ".");
            }

            if (sJoinigDateFromat != "")
            {
                sJoinigDateFromat = sJoinigDateFromat.Substring(0, sJoinigDateFromat.Length - 2);
                throw new NullStudentDateofBirthExceptions(S_VALID_JOINING_DATE1 + sJoinigDateFromat + ".");
            }
            if (sPermanentDateFromat != "")
            {
                sPermanentDateFromat = sPermanentDateFromat.Substring(0, sPermanentDateFromat.Length - 2);
                throw new NullStudentDateofBirthExceptions(S_VALID_PERMANENT_DATE + sPermanentDateFromat + ".");
            }
            if (sResignationDateFromat != "")
            {
                sResignationDateFromat = sResignationDateFromat.Substring(0, sResignationDateFromat.Length - 2);
                throw new NullStudentDateofBirthExceptions(S_VALID_RESIGNATION_DATE + sResignationDateFromat + ".");
            }
            if (sPermanentRow != "")
            {
                sPermanentRow = sPermanentRow.Substring(0, sPermanentRow.Length - 2);
                throw new NullStudentDateofBirthExceptions(S_VALID_JOINING_PERMANENT_DATE + sPermanentRow + ".");
            }
            if (sJoiningDateRow != "")
            {
                sJoiningDateRow = sJoiningDateRow.Substring(0, sJoiningDateRow.Length - 2);
                throw new NullStudentDateofBirthExceptions(S_SELECT_JOINING_DATE + sJoiningDateRow + ".");
            }
            if (sResignationRow != "")
            {
                sResignationRow = sResignationRow.Substring(0, sResignationRow.Length - 2);
                throw new NullStudentDateofBirthExceptions(S_VALID_JOINING_RESIGNATION_DATE + sResignationRow + ".");
            }
            return true;
        }

        /// <summary>
        ///		
        /// </summary>
        /// <param name="aoDataTable"></param>
        private void ValidateCategory(DataTable aoDataTable)
        {
            var oMasterDataCollectionBL = new MasterDataCollectionBL();
            var lstRowNums = new List<int>();
            var lstRowNumsForBlank = new List<int>();
            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                if (!aoDataTable.Rows[iRowcount][I_XLS_CATEGORY].ToString().Trim().IsNullOrEmpty())
                {
                    string sCategory = aoDataTable.Rows[iRowcount][I_XLS_CATEGORY].ToString().Trim();
                    if (oMasterDataCollectionBL.GetCategoryIdForCategory(sCategory) == Constants.I_ZERO)
                        lstRowNums.Add(iRowcount + 1);
                }
                else
                {
                    lstRowNumsForBlank.Add(iRowcount + 1);
                }
            }

            if (lstRowNumsForBlank.Count > 0)
                throw new NullStudentCategoryExceptions(CommonUtility.GetResourceValue("CategorySelected") + String.Join(", ", lstRowNumsForBlank));
            if (lstRowNums.Count > 0)
                throw new NullStudentCategoryExceptions(CommonUtility.GetResourceValue("ValidCategorySelected") + String.Join(", ", lstRowNums));
        }

        /// <summary>
        /// This method is used to validate Fee area name.
        /// </summary>
        /// <param name="aoDataTable"></param>
        private bool ValidateFeeAreaName(DataTable aoDataTable)
        {
            var oMasterDataCollectionBL = new MasterDataCollectionBL();
            var lstRowNums = new List<int>();
            List<string> lstFeeAreaName = oMasterDataCollectionBL.GetFeeAreas();
            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                if (!aoDataTable.Rows[iRowcount]["Fee Area Name"].ToString().Trim().IsNullOrEmpty())
                {
                    string sFeeAreaName = aoDataTable.Rows[iRowcount]["Fee Area Name"].ToString().Trim();
                    if (!lstFeeAreaName.Contains(sFeeAreaName))
                        lstRowNums.Add(iRowcount + 1);
                }
            }
            if (lstRowNums.Count > 0)
                throw new ValidateStudentSubAreaName("Please select correct Fee Area Name for line number " + String.Join(", ", lstRowNums));

            return true;
        }

        /// <summary>
        /// This method is used to validate Fee area name.
        /// </summary>
        /// <param name="aoDataTable"></param>
        private void ValidateCategoryForStudent(DataTable aoDataTable)
        {
            var oMasterDataCollectionBL = new MasterDataCollectionBL();
            DataSet dCategory = oMasterDataCollectionBL.GetAllFeeCategoriesForImport(moStudentInfoStruct.iSchoolId, moStudentInfoStruct.iAcademicYearId);

            mdtCategory = new DataTable();
            mdtCategory = dCategory.Tables[0];
            bool bIsAaryanSchool = dCategory.Tables[1].Rows[0]["IsAaryanSchool"].ToBool();

            if (bIsAaryanSchool)
            {                
                var lstRowNums = new List<int>();
                var lstRowNumsForBlank = new List<int>();
                for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
                {
                    if (!aoDataTable.Rows[iRowcount][I_XLS_STUDENTCATEGORY].ToString().Trim().IsNullOrEmpty())
                    {
                        string sCategoryName = aoDataTable.Rows[iRowcount][I_XLS_STUDENTCATEGORY].ToString();
                        if (!mdtCategory.AsEnumerable().Any(ss => ss.Field<string>("Name") == sCategoryName.Trim()))                                    
                            lstRowNums.Add(iRowcount + 1);
                    }
                    else
                    {
                        lstRowNumsForBlank.Add(iRowcount + 1);
                    }
                }
                if (lstRowNumsForBlank.Count > 0)
                    throw new NullStudentCategoryExceptions("Fee Category should not be blank for Row no. - " + String.Join(", ", lstRowNumsForBlank));
                if (lstRowNums.Count > 0)
                    throw new NullStudentCategoryExceptions("Please enter valid Fee Category for row no. - " + String.Join(", ", lstRowNums));  
            }            
        }

        /// <summary>
        ///	This mthod is used to validate RTE category.	
        /// </summary>
        /// <param name="aoDataTable"></param>
        private void ValidateRTECategory(DataTable aoDataTable)
        {
            var oMasterDataCollectionBL = new MasterDataCollectionBL();
            var lstRowNumsMatch = new List<int>();
            var lstRowNumsForBlank = new List<int>();
            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                if (moStudentInfoStruct.bIsRTEApplicable && aoDataTable.Rows[iRowcount][I_XLS_IS_RTE_STUDENT].ToString().Trim() == S_IS_YES)
                {
                    if (!aoDataTable.Rows[iRowcount][I_XLS_RTECATEGORY].ToString().Trim().IsNullOrEmpty())
                    {
                        //string sCategory = aoDataTable.Rows[iRowcount][I_XLS_CATEGORY].ToString().Trim();
                        //string sRTECategory = aoDataTable.Rows[iRowcount][I_XLS_RTECATEGORY].ToString().Trim();
                        //if (oMasterDataCollectionBL.GetRTECategoryIdForCategory(sRTECategory) != Constants.I_ONE && !sCategory.Equals(sRTECategory))
                        //    lstRowNumsMatch.Add(iRowcount + 1);
                    }
                    else
                    {
                        lstRowNumsForBlank.Add(iRowcount + 1);
                    }
                }
            }

            if (lstRowNumsForBlank.Count > 0)
                throw new NullStudentCategoryExceptions(CommonUtility.GetResourceValue("ValidRTECategory") + String.Join(", ", lstRowNumsForBlank));
            if (lstRowNumsMatch.Count > 0)
                throw new NullStudentCategoryExceptions(CommonUtility.GetResourceValue("validSameRTECategory") + String.Join(", ", lstRowNumsMatch));

        }

        /// <summary>
        /// This method is used to validate DOB.
        /// </summary>
        /// <param name="aoDataTable"></param>
        private bool ValidationsForAdminStaffDates(DataTable aoDataTable)
        {
            string sRowNumber = "";
            string sJoiningDateRow = "";
            string sPermanentRow = "";
            string sResignationRow = "";
            string sDobRow = "";
            string sDobminorRow = "";
            string sBithFromat = "";
            string sJoinigDateFromat = "";
            string sPermanentDateFromat = "";
            string sResignationDateFromat = "";
            DateTime dtCurrent = DateTime.Today;
            DateTime dtBirth;
            DateTime dtJoiningDate;
            DateTime dtPermanentDate;
            DateTime dtResignationDate;
            object oGrdDtBirth;
            object oGrdDtJoiningDate;
            object oGrdDtPermanentDate;
            object oGrdDtResignationDate;
            Type tDtBirth;
            Type tDtJoiningDate;
            Type tDtPermanentDate;
            Type tDtResignationDate;

            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                int iCurrentYear = dtCurrent.Year;
                oGrdDtBirth = aoDataTable.Rows[iRowcount][I_XLS_AS_DOB];
                tDtBirth = oGrdDtBirth.GetType();
                if (aoDataTable.Rows[iRowcount][I_XLS_AS_DOB].ToString() != string.Empty)
                {
                    if (!tDtBirth.FullName.Trim().Equals("System.DateTime"))
                        sBithFromat = sBithFromat + (iRowcount + 1).ToString() + ", ";

                    if (sBithFromat.Trim().Equals(string.Empty))
                    {
                        dtBirth = Convert.ToDateTime(oGrdDtBirth);
                        if (dtBirth > dtCurrent)
                        {
                            sDobRow = sDobRow + (iRowcount + 1).ToString() + ", ";
                        }
                        else if (dtBirth.Year + 18 > dtCurrent.Year)
                        {
                            sDobminorRow = sDobminorRow + (iRowcount + 1).ToString() + ", ";
                        }
                    }
                }
                ///////////////////////////////////////////////////////////////////////////////////////////////////////
                oGrdDtJoiningDate = aoDataTable.Rows[iRowcount][I_XLS_AS_JOINING_DATE];
                tDtJoiningDate = oGrdDtJoiningDate.GetType();

                oGrdDtPermanentDate = aoDataTable.Rows[iRowcount][I_XLS_AS_PERMANENT_DATE];
                tDtPermanentDate = oGrdDtPermanentDate.GetType();

                oGrdDtResignationDate = aoDataTable.Rows[iRowcount][I_XLS_AS_RESIGNATION_DATE];
                tDtResignationDate = oGrdDtResignationDate.GetType();

                if ((aoDataTable.Rows[iRowcount][I_XLS_AS_PERMANENT_DATE].ToString().Trim() != string.Empty) && (!tDtPermanentDate.FullName.Trim().Equals("System.DateTime")))
                    sPermanentDateFromat = sPermanentDateFromat + (iRowcount + 1).ToString().Trim() + ", ";

                if ((aoDataTable.Rows[iRowcount][I_XLS_AS_RESIGNATION_DATE].ToString().Trim() != string.Empty) && (!tDtResignationDate.FullName.Trim().Equals("System.DateTime")))
                    sResignationDateFromat = sResignationDateFromat + (iRowcount + 1).ToString() + ", ";

                if (aoDataTable.Rows[iRowcount][I_XLS_AS_JOINING_DATE].ToString().Trim() != string.Empty)
                {

                    if (!tDtJoiningDate.FullName.Trim().Equals("System.DateTime"))
                        sJoinigDateFromat = sJoinigDateFromat + (iRowcount + 1).ToString() + ", ";

                    if (sJoinigDateFromat.Trim().Equals("") && sPermanentDateFromat.Trim().Equals(""))
                    {
                        dtJoiningDate = Convert.ToDateTime(oGrdDtJoiningDate);
                        if (!oGrdDtPermanentDate.ToString().IsNullOrEmpty())
                        {
                            dtPermanentDate = Convert.ToDateTime(oGrdDtPermanentDate);
                            if (dtJoiningDate > dtPermanentDate)
                                sPermanentRow = sPermanentRow + (iRowcount + 1).ToString() + ", ";
                            else if ((sResignationDateFromat.Trim().Equals("")) && (!oGrdDtResignationDate.ToString().Trim().IsNullOrEmpty()))
                            {
                                dtResignationDate = Convert.ToDateTime(oGrdDtResignationDate);
                                if (dtResignationDate <= dtPermanentDate || dtResignationDate <= dtJoiningDate)
                                    sResignationRow = sResignationRow + (iRowcount + 1).ToString() + ", ";
                            }
                        }
                        else if (!oGrdDtResignationDate.ToString().Trim().IsNullOrEmpty())
                        {
                            dtResignationDate = Convert.ToDateTime(oGrdDtResignationDate);
                            if (dtResignationDate <= dtJoiningDate)
                                sResignationRow = sResignationRow + (iRowcount + 1).ToString() + ", ";
                        }
                    }

                    else if (sJoinigDateFromat.Trim().Equals("") && sResignationDateFromat.Trim().Equals(""))
                    {
                        dtJoiningDate = Convert.ToDateTime(oGrdDtJoiningDate);
                        if (!oGrdDtResignationDate.ToString().Trim().IsNullOrEmpty())
                        {
                            dtResignationDate = Convert.ToDateTime(oGrdDtResignationDate);
                            if (dtJoiningDate >= dtResignationDate && sResignationRow == "")
                                sResignationRow = sResignationRow + (iRowcount + 1).ToString() + ", ";
                        }
                    }
                }
                else if ((!oGrdDtPermanentDate.ToString().Trim().IsNullOrEmpty()) || (!oGrdDtResignationDate.ToString().Trim().IsNullOrEmpty()))
                {
                    if (sPermanentDateFromat.Equals("") || sResignationDateFromat.Equals(""))
                        sJoiningDateRow = sJoiningDateRow + (iRowcount + 1).ToString() + ", ";
                }
            }

            if (sBithFromat != "")
            {
                sBithFromat = sBithFromat.Substring(0, sBithFromat.Length - 2);
                throw new NullStudentDateofBirthExceptions(S_FORMAT_DOB + sBithFromat + ".");
            }
            else if (sDobRow != "")
            {
                sDobRow = sDobRow.Substring(0, sDobRow.Length - 2);
                throw new NullStudentDateofBirthExceptions(S_VALID_DOB + sDobRow + ".");
            }
            else if (sDobminorRow != "")
            {
                sDobminorRow = sDobminorRow.Substring(0, sDobminorRow.Length - 2);
                throw new NullStudentDateofBirthExceptions(S_AS_MINOR_DOB + sDobminorRow + ".");
            }

            else if (sRowNumber != "")
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);
                throw new NullStudentDateofBirthExceptions(S_CHECK_DOB + sRowNumber + ".");
            }

            if (sJoinigDateFromat != "")
            {
                sJoinigDateFromat = sJoinigDateFromat.Substring(0, sJoinigDateFromat.Length - 2);
                throw new NullStudentDateofBirthExceptions(S_VALID_JOINING_DATE1 + sJoinigDateFromat + ".");
            }
            if (sPermanentDateFromat != "")
            {
                sPermanentDateFromat = sPermanentDateFromat.Substring(0, sPermanentDateFromat.Length - 2);
                throw new NullStudentDateofBirthExceptions(S_VALID_PERMANENT_DATE + sPermanentDateFromat + ".");
            }
            if (sResignationDateFromat != "")
            {
                sResignationDateFromat = sResignationDateFromat.Substring(0, sResignationDateFromat.Length - 2);
                throw new NullStudentDateofBirthExceptions(S_VALID_RESIGNATION_DATE + sResignationDateFromat + ".");
            }
            if (sPermanentRow != "")
            {
                sPermanentRow = sPermanentRow.Substring(0, sPermanentRow.Length - 2);
                throw new NullStudentDateofBirthExceptions(S_VALID_JOINING_PERMANENT_DATE + sPermanentRow + ".");
            }
            if (sJoiningDateRow != "")
            {
                sJoiningDateRow = sJoiningDateRow.Substring(0, sJoiningDateRow.Length - 2);
                throw new NullStudentDateofBirthExceptions(S_SELECT_JOINING_DATE + sJoiningDateRow + ".");
            }
            if (sResignationRow != "")
            {
                sResignationRow = sResignationRow.Substring(0, sResignationRow.Length - 2);
                throw new NullStudentDateofBirthExceptions(S_VALID_JOINING_RESIGNATION_DATE + sResignationRow + ".");
            }
            return true;
        }

        private bool ValidateAdminStaffEmailAddress(DataTable aoDataTable)
        {
            object oEmailId = null;
            string sEmailid = string.Empty;
            string sEmailIdRow = string.Empty;
            string sPasswdRowNo = string.Empty;
            string sLogInRowNo = "";
            object oLogInId = null;
            string sLogInId = "";
            object oPasswd = null;
            string sPasswd = "";

            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                oEmailId = aoDataTable.Rows[iRowcount][I_XLS_AS_EMAIL];
                sEmailid = Convert.ToString(oEmailId).Trim();
                oLogInId = aoDataTable.Rows[iRowcount][I_XLS_AS_LOGIN];
                sLogInId = Convert.ToString(oLogInId).Trim();
                oPasswd = aoDataTable.Rows[iRowcount][I_XLS_AS_PWD];
                sPasswd = Convert.ToString(oPasswd);
                if (!Regex.Match(sEmailid, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", RegexOptions.None).Success)
                    sEmailIdRow = sPasswdRowNo + (iRowcount + 1).ToString() + ", ";
                else if (sLogInId.Trim().Length < 6 || sLogInId.Length > 20 || !Regex.Match(sLogInId, @"^[A-Za-z0-9_.]+$", RegexOptions.None).Success)
                    sLogInRowNo = sLogInRowNo + (iRowcount + 1).ToString() + ", ";
                else if (sPasswd.Length < 6 || sPasswd.Length > 15)
                    sPasswdRowNo = sPasswdRowNo + (iRowcount + 1).ToString() + ", ";
            }

            if (sEmailIdRow != "")
            {
                sEmailIdRow = sEmailIdRow.Substring(0, sEmailIdRow.Length - 2);
                throw new ValidEmailAddressExceptions(S_EMAIL_ADDR + " " + sEmailIdRow + ".");
            }
            else if (sLogInRowNo != "")
            {
                sLogInRowNo = sLogInRowNo.Substring(0, sLogInRowNo.Length - 2);
                throw new ValidPincodeExceptions(S_AS_VALID_LOGIN + " " + sLogInRowNo + ".");
            }
            else if (sPasswdRowNo != "")
            {
                sPasswdRowNo = sPasswdRowNo.Substring(0, sPasswdRowNo.Length - 2);
                throw new ValidPincodeExceptions(S_VALID_PWD + " " + sPasswdRowNo + ".");
            }
            return true;
        }

        private bool ValidateStudentEmailAddress(DataTable aoDataTable)
        {
            object oEmailId = null;
            string sEmailid = string.Empty;
            string sEmailIdRow = string.Empty;

            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                oEmailId = aoDataTable.Rows[iRowcount][I_XLS_SEMAIL];
                sEmailid = Convert.ToString(oEmailId).Trim();

                if (!sEmailid.IsNullOrEmpty() && !Regex.Match(sEmailid, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", RegexOptions.None).Success)
                    sEmailIdRow = sEmailIdRow + (iRowcount + 1).ToString() + ", ";

            }

            if (sEmailIdRow != string.Empty)
            {
                sEmailIdRow = sEmailIdRow.Substring(0, sEmailIdRow.Length - 2);
                throw new ValidEmailAddressExceptions(S_EMAIL_ADDR + " " + sEmailIdRow + ".");
            }
            return true;
        }


        private bool CheckIsPincodeValid(DataTable aoDataTable)
        {
            string sRowNumber = "";
            string sPinRowNo = "";
            string sPincode = "";
            object oPincode = null;
            Type tPinCode;

            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                oPincode = aoDataTable.Rows[iRowcount][I_XLS_PINCODE];
                sPincode = Convert.ToString(oPincode).Trim();
                tPinCode = oPincode.GetType();

                if (!(tPinCode.FullName.Trim().Equals("System.Double")))
                {
                    sPinRowNo = sPinRowNo + (iRowcount + 1).ToString() + ", ";
                }
                else if (sPincode.Length != 6)
                {
                    sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                }
            }
            if (sPinRowNo != "")
            {
                sPinRowNo = sPinRowNo.Substring(0, sPinRowNo.Length - 2);
                throw new ValidPincodeExceptions(S_FORMAT_PINCODE + sPinRowNo + ".");
            }
            else if (sRowNumber != "")
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);
                throw new ValidPincodeExceptions(S_VALID_L_PIN + sRowNumber + ".");
            }

            return true;
        }

        private bool CheckIsApplicableRuleOrRTE(DataTable aoDataTable)
        {
            string sRowNumber = "";
            string sApplicableRule = "";
            string sIsRTEStudent = "";

            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                sApplicableRule = aoDataTable.Rows[iRowcount][I_XLS_APPLICABLE_RULE].ToString().Trim();
                sIsRTEStudent = aoDataTable.Rows[iRowcount][I_XLS_IS_RTE_STUDENT].ToString().Trim();

                if (!sApplicableRule.IsNullOrEmpty() && sIsRTEStudent == S_IS_YES)
                {
                    sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                }
            }
            if (sRowNumber != "")
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);
                throw new ValidPincodeExceptions(S_NO_RTE_AND_APPLICABLERULE + sRowNumber + ".");
            }

            return true;
        }

        private bool CheckIsTeacherPincodeValid(DataTable aoDataTable)
        {
            string sRowNumber = "";
            string sPinRowNo = "";
            string sPincode = "";
            object oPincode = null;
            Type tPinCode;
            string sLogInRowNo = "";
            object oLogInId = null;
            string sLogInId = "";
            object oPasswd = null;
            string sPasswd = "";
            string sPasswdRowNo = "";
            object oEmailId = null;
            string sEmailid = "";
            string sEmailIdRow = "";


            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                oEmailId = aoDataTable.Rows[iRowcount][I_XLS_T_EMAIL];
                sEmailid = Convert.ToString(oEmailId).Trim();
                oLogInId = aoDataTable.Rows[iRowcount][I_XLS_T_LOGIN];
                sLogInId = Convert.ToString(oLogInId).Trim();
                oPasswd = aoDataTable.Rows[iRowcount][I_XLS_T_PWD];
                sPasswd = Convert.ToString(oPasswd).Trim();
                oPincode = aoDataTable.Rows[iRowcount][I_XLS_T_L_PINCODE];
                sPincode = Convert.ToString(oPincode).Trim();
                tPinCode = oPincode.GetType();

                if (!(tPinCode.FullName.Trim().Equals("System.Double")))
                {
                    sPinRowNo = sPinRowNo + (iRowcount + 1).ToString() + ", ";
                }
                else if (sPincode.Length != 6)
                {
                    sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                }
                else if (sLogInId.Length > 5 && sLogInId.Trim().Length < 15 && !Regex.Match(sLogInId, @"^[A-Za-z0-9_.]+$", RegexOptions.None).Success)
                {
                    sLogInRowNo = sLogInRowNo + (iRowcount + 1).ToString() + ", ";
                }
                else if (sPasswd.Length < 6 || sPasswd.Length > 15)
                {
                    sPasswdRowNo = sPasswdRowNo + (iRowcount + 1).ToString() + ", ";

                }
                else if (!Regex.Match(sEmailid, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", RegexOptions.None).Success)
                {
                    sEmailIdRow = sPasswdRowNo + (iRowcount + 1).ToString() + ", ";
                }

            }
            if (sPinRowNo != "")
            {
                sPinRowNo = sPinRowNo.Substring(0, sPinRowNo.Length - 2);
                throw new ValidPincodeExceptions(S_FORMAT_PINCODE + sPinRowNo + ".");
            }
            else if (sRowNumber != "")
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);
                throw new ValidPincodeExceptions(S_VALID_L_PIN + sRowNumber + ".");
            }
            else if (sLogInRowNo != "")
            {
                sLogInRowNo = sLogInRowNo.Substring(0, sLogInRowNo.Length - 2);
                throw new ValidPincodeExceptions(S_VALID_LOGIN + sLogInRowNo + ".");
            }
            else if (sPasswdRowNo != "")
            {
                sPasswdRowNo = sPasswdRowNo.Substring(0, sPasswdRowNo.Length - 2);
                throw new ValidPincodeExceptions(S_VALID_PWD + sPasswdRowNo + ".");
            }
            else if (sEmailIdRow != "")
            {
                sEmailIdRow = sEmailIdRow.Substring(0, sEmailIdRow.Length - 2);
                throw new ValidPincodeExceptions(S_EMAIL_ADDR + sEmailIdRow + ".");
            }
            return true;
        }

        /// <summary>
        /// This method is used to throw an appropriate exception.
        /// </summary>
        /// <param name="iColCount"></param>
        private void ThrowAppropriateException(int iColCount, string sRowNumber)
        {
            switch (iColCount)
            {
                case I_XLS_REG_NO:
                    throw new NullRegisterNumberExceptions(CommonUtility.GetResourceValue("ValRegistrationNumberBlank") + sRowNumber + ".");
                case I_XLS_ROLL_NO:
                    throw new NullStudentRollNumberExceptions(CommonUtility.GetResourceValue("ValRollNumberBlank") + sRowNumber + ".");
                case I_XLS_FIRST_NAME:
                    throw new NullStudentFirstNameExceptions(CommonUtility.GetResourceValue("valStudentFirstNameBlank") + sRowNumber + ".");
                case I_XLS_DATE_OF_BIRTH:
                    throw new NullStudentDateofBirthExceptions(CommonUtility.GetResourceValue("valStudentBirthDateBlank") + sRowNumber + ".");
                case I_XLS_ADMISSION_DATE:
                    throw new NullStudentAdmissionDateExceptions(CommonUtility.GetResourceValue("valStudentAdmissionDate") + sRowNumber + ".");
                case I_XLS_JOINING_DATE:
                    throw new NullStudentJoiningDateExceptions(CommonUtility.GetResourceValue("valStudentJoiningDate") + sRowNumber + ".");
                case I_XLS_SEX:
                    throw new NullStudentSexExceptions(CommonUtility.GetResourceValue("valStudentSex") + sRowNumber + ".");
                case I_XLS_PARENT_NAME:
                    throw new NullStudentParentNameExceptions(CommonUtility.GetResourceValue("valParentName") + sRowNumber + ".");
                case I_XLS_PARENT_OCCUPATION:
                    throw new NullStudentParentOccupationExceptions(CommonUtility.GetResourceValue("valParentAccupation") + sRowNumber + ".");
                case I_XLS_ADDRESS:
                    throw new NullStudentAddressExceptions(CommonUtility.GetResourceValue("valStudentAddress") + sRowNumber + ".");
                case I_XLS_CITY:
                    throw new NullStudentCityExceptions(CommonUtility.GetResourceValue("valStudentCityBlank") + sRowNumber + ".");
                case I_XLS_STATE:
                    throw new NullStudentStateExceptions(CommonUtility.GetResourceValue("valStudentStateBlank") + sRowNumber + ".");
                case I_XLS_PINCODE:
                    throw new NullStudentPincodeExceptions(CommonUtility.GetResourceValue("valStudentPinCode") + sRowNumber + ".");
                case I_XLS_MOBILE:
                    throw new NullStudentMobileExceptions(CommonUtility.GetResourceValue("valMobileNumberBlank") + sRowNumber + ".");
                case I_XLS_CATEGORY:
                    throw new NullStudentMobileExceptions(CommonUtility.GetResourceValue("valCategorySelected") + sRowNumber + ".");
                case I_XLS_NO_NEW_ADDMISSION:
                    throw new NoRecordFoundExceptions(CommonUtility.GetResourceValue("valNewAdmissionBlank") + sRowNumber + ".");
                //case I_XLS_NO_DATA_IN_TABLE:
                //    throw new NoRecordFoundExceptions(CommonUtility.GetResourceValue("ValImportedFileShouldNotEmpty") + ".");
              
            }
        }

        /// <summary>
        /// This method is used to throw an appropriate exception related to Teacher.
        /// </summary>
        /// <param name="iColCount"></param>
        private void ThrowTeacherDataNullException(int iColCount, string sRowNumber)
        {
            switch (iColCount)
            {

                case I_XLS_T_SAL_ID:
                    throw new NoRecordFoundExceptions(S_NULL_SAL_ID + sRowNumber + ".");
                case I_XLS_T_FIRST_NAME:
                    throw new NoRecordFoundExceptions(S_NULL_T_FIRST_NAME + sRowNumber + ".");
                case I_XLS_T_DEGN_ID:
                    throw new NoRecordFoundExceptions(S_NULL_DESGN + sRowNumber + ".");
                case I_XLS_T_IS_TEMP:
                    throw new NoRecordFoundExceptions(S_NULL_IS_TEMP + sRowNumber + ".");
                case I_XLS_T_MOBILE:
                    throw new NoRecordFoundExceptions(S_NULL_T_MOBILE + sRowNumber + ".");
                case I_XLS_T_DOB:
                    throw new NoRecordFoundExceptions(S_NULL_T_DOB + sRowNumber + ".");
                case I_XLS_T_NATION:
                    throw new NoRecordFoundExceptions(S_NULL_NATION + sRowNumber + ".");
                case I_XLS_T_RELIGION:
                    throw new NoRecordFoundExceptions(S_NULL_RELIGION + sRowNumber + ".");
                case I_XLS_T_CATEGORY:
                    throw new NoRecordFoundExceptions(S_NULL_CATEGORY + sRowNumber + ".");
                case I_XLS_T_L_ADDRESS:
                    throw new NoRecordFoundExceptions(S_NULL_L_ADDRESS + sRowNumber + ".");
                case I_XLS_T_L_CITY:
                    throw new NoRecordFoundExceptions(S_NULL_L_CITY + sRowNumber + ".");
                case I_XLS_T_L_STATE:
                    throw new NoRecordFoundExceptions(S_NULL_L_STATE + sRowNumber + ".");
                case I_XLS_T_L_PINCODE:
                    throw new NoRecordFoundExceptions(S_NULL_L_PIN + sRowNumber + ".");
                case I_XLS_T_QUALI:
                    throw new NoRecordFoundExceptions(S_NULL_QUALI + sRowNumber + ".");
                case I_XLS_T_PASS_YR:
                    throw new NoRecordFoundExceptions(S_NULL_PASS_YR + sRowNumber + ".");
                case I_XLS_T_CLASS:
                    throw new NoRecordFoundExceptions(S_NULL_CLASS + sRowNumber + ".");
                case I_XLS_T_UNI:
                    throw new NoRecordFoundExceptions(S_NULL_UNI + sRowNumber + ".");
                case I_XLS_T_STD_ID:
                    throw new NoRecordFoundExceptions(S_NULL_STD_ID + sRowNumber + ".");
                case I_XLS_T_SUB_ID:
                    throw new NoRecordFoundExceptions(S_NULL_SUB_ID + sRowNumber + ".");
                case I_XLS_T_EMAIL:
                    throw new NoRecordFoundExceptions(S_NULL_EMAIL + sRowNumber + ".");
                case I_XLS_T_LOGIN:
                    throw new NoRecordFoundExceptions(S_NULL_LOGIN + sRowNumber + ".");
                case I_XLS_T_PWD:
                    throw new NoRecordFoundExceptions(S_NULL_PWD + sRowNumber + ".");
                case I_XLS_T_EMERGENCY_NO:
                    throw new NoRecordFoundExceptions(S_NULL_T_EMERGENCY_CONTACT + sRowNumber + ".");
                case I_XLS_ASSO_STANDARD_CATEGORY:
                    throw new NoRecordFoundExceptions(S_NULL_ASSO_STANDARD_CATEGPRY + sRowNumber + ".");
            }
        }

        /// <summary>
        /// This method is used to throw an appropriate exception related to Challan.
        /// </summary>
        /// <param name="iColCount"></param>
        private void ThrowChallanException(int iColCount, string sRowNumber)
        {
            if (iColCount == I_XLS_C_CHALLANNO)
                throw new InvalidChallanNoExceptions(S_INVALID_CHALLAN_NO + sRowNumber + ".");
            if (iColCount == I_XLS_C_CHQUENO)
                throw new InvalidChallanNoExceptions(S_INVALID_CHEQUE_NO + sRowNumber + ".");
            if (iColCount == Constants.I_TWO)
                throw new InvalidChallanNoExceptions(S_DUPLICATE_CHALLAN_NO + sRowNumber + ".");
        }

        private void ThrowBlankDataException(int iColCount, string sRowNumber)
        {
            string sMessage = string.Empty;
            switch (iColCount)
            {
                case 0: sMessage = S_BLANK_VEHICLE_NO + sRowNumber + "."; break;
                case 1: sMessage = S_BLANK_READING_DATE + sRowNumber + "."; break;
                case 2: sMessage = S_BLANK_RECEIPT_NUMBER + sRowNumber + "."; break;
                case 4: sMessage = S_BLANK_READING_TO + sRowNumber + "."; break;
                case 5: sMessage = S_BLANK_LITTERS + sRowNumber + "."; break;
                case 6: sMessage = S_BLANK_PER_LITTER_COST + sRowNumber + "."; break;
                case 7: sMessage = S_BLANK_TOTAL_COST+ sRowNumber + "."; break;
            }

            if (sMessage != string.Empty)
                throw new InvalidVehicleDataExceptions(sMessage);
        }

        private void ThrowBlankMaintenanceDataException(int iColCount, string sRowNumber)
        {
            string sMessage = string.Empty;
            switch (iColCount)
            {
                case 0: sMessage = S_BLANK_VEHICLE_NO + sRowNumber + "."; break;
                case 1: sMessage = S_BLANK_MAINT_DATE + sRowNumber + "."; break;
                case 2: sMessage = S_BLANK_MAINT_BILL_DATE + sRowNumber + "."; break;                
                case 5: sMessage = S_BLANK_MAINT_BILL_NO + sRowNumber + "."; break;
                case 6: sMessage = S_BLANK_MAINT_WORKSHOP_NAME + sRowNumber + "."; break;
                case 9: sMessage = S_BLANK_MAINT_TYPE + sRowNumber + "."; break;
                case 14: sMessage = S_BLANK_MAINT_TOTAL + sRowNumber + "."; break;
            }

            if (sMessage != string.Empty)
                throw new InvalidVehicleDataExceptions(sMessage);
        }


        /// <summary>
        /// This method is used to throw an appropriate exception related to Admin Staff(Supervisor).
        /// </summary>
        /// <param name="iColCount"></param>
        private void ThrowAdminStaffDataNullException(int iColCount, string sRowNumber)
        {
            switch (iColCount)
            {
                case I_XLS_AS_SAL_ID:
                    throw new NullRegisterNumberExceptions(CommonUtility.GetResourceValue("valSalutationBlank") + sRowNumber + ".");
                case I_XLS_AS_FIRST_NAME:
                    throw new NullStudentFirstNameExceptions(CommonUtility.GetResourceValue("valFirstNameBlank") + sRowNumber + ".");
                case I_XLS_AS_MIDDLE_NAME:
                    throw new NullStudentFirstNameExceptions(CommonUtility.GetResourceValue("valMiddleNameBlank") + sRowNumber + ".");
                case I_XLS_AS_LAST_NAME:
                    throw new NullStudentLastNameExceptions(CommonUtility.GetResourceValue("valLastName") + sRowNumber + ".");
                case I_XLS_AS_DEGN_ID:
                    throw new NullStudentMotherNameExceptions(CommonUtility.GetResourceValue("valDesignationBlank") + sRowNumber + ".");
                case I_XLS_AS_MOBILE:
                    throw new NullStudentMobileExceptions(CommonUtility.GetResourceValue("valMobileBlank") + sRowNumber + ".");
                case I_XLS_AS_EMAIL:
                    throw new NullStudentMobileExceptions(CommonUtility.GetResourceValue("valEmailBlank") + sRowNumber + ".");
                case I_XLS_AS_LOGIN:
                    throw new NullStudentMobileExceptions(CommonUtility.GetResourceValue("valLoginBlank") + sRowNumber + ".");
                case I_XLS_AS_PWD:
                    throw new NullStudentMobileExceptions(CommonUtility.GetResourceValue("valPasswordBlank") + sRowNumber + ".");
                case I_XLS_AS_EMRGENCY_NO:
                    throw new NullEmergencyContactException(CommonUtility.GetResourceValue("ErrorMsgEmergencyContantNo") + sRowNumber + ".");
                case I_XLS_AS_ADDRESS:
                    throw new NullStudentAddressExceptions(CommonUtility.GetResourceValue("valAddressBlank") + sRowNumber + ".");
            }
        }

        #endregion

        private XmlAttribute GetPasswordAttribute(ref XmlDocument aoDoc, string asLoginId)
        {
            XmlAttribute attr = aoDoc.CreateAttribute("Password");
            attr.Value = Utility.CommonUtility.GetEncryptedPassword(asLoginId, moRandomNo.Next(100000, 999999).ToString());
            return attr;
        }

        private XmlAttribute GetPasswordAttribute(ref XmlDocument aoDoc, string asLogin, string asPassword)
        {
            XmlAttribute attr = aoDoc.CreateAttribute("User_Password");
            attr.Value = Utility.CommonUtility.GetEncryptedPassword(asLogin, moRandomNo.Next(100000, 999999).ToString());
            return attr;
        }


        private bool CheckIsUserNameIsDuplicate(DataTable aoDataTable)
        {
            int iSchoolId = moStudentInfoStruct.iSchoolId;
            bool bIsDuplicateName;
            string sRowNumber = "";
            SchoolUserBL oSchoolUserBL = new SchoolUserBL();

            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                string sLogin = "";
                sLogin = aoDataTable.Rows[iRowcount][I_XLS_T_LOGIN].ToString().Trim();
                oSchoolUserBL.Login = sLogin;

                bIsDuplicateName = oSchoolUserBL.IsUserLoginDuplicate();
                if (bIsDuplicateName)
                {
                    sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                }

            }
            if (sRowNumber != "")
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);
                throw new DuplicateExceptions(S_DUPLICATE_USER + sRowNumber + ".");
            }

            return false;
        }

        private bool IsValidPassword(DataTable aoDataTable, int aiColumnNumber)
        {
            int iSchoolId = moStudentInfoStruct.iSchoolId;
            bool bIsValidPassword = false;
            string sPassRowNo = string.Empty;
            string sPassword = string.Empty;
            object oPassword = null;

            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                oPassword = aoDataTable.Rows[iRowcount][aiColumnNumber];
                sPassword = Convert.ToString(oPassword);

                Match match = Regex.Match(sPassword, "[^a-z0-9]", RegexOptions.IgnoreCase);
                if (match.Success)
                    bIsValidPassword = true;

                if ((!bIsValidPassword) || (sPassword.Count(char.IsLetter) == Constants.I_ZERO) || (sPassword.Count(char.IsDigit) == Constants.I_ZERO))
                {
                    sPassRowNo = sPassRowNo + (iRowcount + 1).ToString() + ", ";
                }
            }
            if (sPassRowNo != "")
            {
                sPassRowNo = sPassRowNo.Substring(0, sPassRowNo.Length - 2);
                throw new ValidExceptions(S_VALID_PASSWORD + sPassRowNo + ".");
            }

            return false;
        }

        /// <summary>
        /// This method is used to validate duplicate AdminStaff UserName.
        /// </summary>
        /// <param name="aoDataTable"></param>
        /// <returns></returns>
        private bool CheckAdminStaffUserNameIsDuplicateInFile(DataTable aoDataTable)
        {
            string sRowNumber = "";
            SchoolUserBL oSchoolUserBL = new SchoolUserBL();

            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                string sLogin = "";
                sLogin = aoDataTable.Rows[iRowcount][I_XLS_AS_LOGIN].ToString().Trim();
                oSchoolUserBL.Login = sLogin;

                for (int iRowcnt = (iRowcount + 1); iRowcnt < aoDataTable.Rows.Count; iRowcnt++)
                {
                    string sLoginNm = "";
                    sLoginNm = aoDataTable.Rows[iRowcnt][I_XLS_AS_LOGIN].ToString().Trim();
                    if (sLoginNm == sLogin)
                        sRowNumber = sRowNumber + (iRowcnt + 1).ToString() + ", ";
                }
            }
            if (sRowNumber != "")
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);
                throw new DuplicateRegisterNumberExceptions(S_DUPLICATE_USER + sRowNumber + ".");
            }

            return false;
        }

        /// <summary>
        /// Check duplicat designation name Principle in Excel sheet
        /// </summary>
        /// <param name="aoDataTable"></param>
        /// <returns></returns>
        private bool CheckIsDuplicateTeacherDesignationInFile(DataTable aoDataTable)
        {
            string sRowNumber = string.Empty;
            string sDesgn = string.Empty; string sPrevsDesgn = string.Empty;
            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                sDesgn = aoDataTable.Rows[iRowcount][I_XLS_T_DEGN_ID].ToString().Trim();
                if (sDesgn == Constants.S_PRINCIPAL_DESIGNATION)
                {
                    for (int iRowcnt = (iRowcount + 1); iRowcnt < aoDataTable.Rows.Count; iRowcnt++)
                    {
                        string sDesgnName = "";
                        sDesgnName = aoDataTable.Rows[iRowcnt][I_XLS_T_DEGN_ID].ToString().Trim();
                        if (sDesgnName == sDesgn && sPrevsDesgn != sDesgn)
                        {
                            sPrevsDesgn = sDesgnName;
                            sRowNumber = sRowNumber + (iRowcnt + 1).ToString() + ", ";
                        }
                    }
                }
            }
            if (sRowNumber != "")
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);
                throw new DuplicateExceptions(S_DUPLICATE_DESGN + sRowNumber + ".");
            }

            return false;
        }

        /// <summary>
        /// This method is used to validate duplicate AdminStaff UserName.
        /// </summary>
        /// <param name="aoDataTable"></param>
        /// <returns></returns>
        private bool CheckTeacherEmailAddressDuplicateInFile(DataTable aoDataTable)
        {
            string sRowNumber = "";
            SchoolUserBL oSchoolUserBL = new SchoolUserBL();

            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                string sEmailAdd = ""; string sPrevEmailAddress = "";
                sEmailAdd = aoDataTable.Rows[iRowcount][I_XLS_T_EMAIL].ToString().Trim();
                oSchoolUserBL.Email = sEmailAdd;

                for (int iRowcnt = (iRowcount + 1); iRowcnt < aoDataTable.Rows.Count; iRowcnt++)
                {
                    string sEmail = "";
                    sEmail = aoDataTable.Rows[iRowcnt][I_XLS_T_EMAIL].ToString().Trim();
                    if (sEmail == sEmailAdd && sPrevEmailAddress != sEmailAdd)
                    {
                        sPrevEmailAddress = sEmail;
                        sRowNumber = sRowNumber + (iRowcnt + 1).ToString() + ", ";
                    }
                }
            }
            if (sRowNumber != "")
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);
                throw new DuplicateExceptions(S_DUPLICATE_EMAIL + sRowNumber + ".");
            }

            return false;
        }

        /// <summary>
        /// This method is used to validate duplicate AdminStaff UserName.
        /// </summary>
        /// <param name="aoDataTable"></param>
        /// <returns></returns>
        private bool CheckIsAdminStaffUserNameIsDuplicate(DataTable aoDataTable)
        {
            int iSchoolId = moStudentInfoStruct.iSchoolId;
            bool bIsDuplicateName;
            string sRowNumber = "";
            SchoolUserBL oSchoolUserBL = new SchoolUserBL();

            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                string sLogin = "";
                sLogin = aoDataTable.Rows[iRowcount][I_XLS_AS_LOGIN].ToString().Trim();
                oSchoolUserBL.Login = sLogin;

                bIsDuplicateName = oSchoolUserBL.IsUserLoginDuplicate();
                if (bIsDuplicateName)
                    sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";

            }
            if (sRowNumber != "")
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);
                throw new DuplicateRegisterNumberExceptions(S_DUPLICATE_USER + sRowNumber + ".");
            }

            return false;
        }
        private bool CheckIsRegistrationNumberIsDuplicate(DataTable aoDataTable)
        {
            int iSchoolId = moStudentInfoStruct.iSchoolId;
            bool bIsDuplicateNumber;
            string sRowNumber = "";
            StudentBL oStudentBL = new StudentBL();

            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                string sRegistrationNumber = "";
                sRegistrationNumber = aoDataTable.Rows[iRowcount][I_XLS_REG_NO].ToString().Trim();

                bIsDuplicateNumber = oStudentBL.CheckIsEnrollmentNumberIsDuplicate(iSchoolId, sRegistrationNumber);
                if (bIsDuplicateNumber)
                {
                    sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                }

            }
            if (sRowNumber != "")
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);
                throw new DuplicateRegisterNumberExceptions(CommonUtility.GetResourceValue("ValDuplicateRegNO") + sRowNumber + ".");
            }
            else
                if (CheckDuplicateRegNoInFile(aoDataTable))
                    throw new DuplicateRegisterNumberExceptions(CommonUtility.GetResourceValue("ValDuplicateRegNoInFile") + " " + sFileRowNumber + ".");
                else
                {
                    if (!CheckIsRollNumberDuplicate(aoDataTable))
                    {
                        return false;
                    }
                }

            return true;
        }

        private bool CheckIsGeneralRegistrationNumberIsDuplicate(DataTable aoDataTable)
        {
            string sCompareVal;
            string sValue;
            string sFileRowNumber = "";

            StudentBL oStudentBL = new StudentBL();

            DataTable dt = oStudentBL.CheckIsGeneralRegistrationNumberIsDuplicate();

            for (int iRowcnt = 0; iRowcnt < aoDataTable.Rows.Count; iRowcnt++)
            {
                sCompareVal = (aoDataTable.Rows[iRowcnt][I_XLS_GENERAL_REG_NO]).ToString().Trim();

                if (sCompareVal != string.Empty)
                {
                    for (int i = iRowcnt + 1; i < aoDataTable.Rows.Count; i++)
                    {
                        sValue = (aoDataTable.Rows[i][I_XLS_GENERAL_REG_NO]).ToString().Trim();
                        if (sValue != string.Empty)
                        {
                            if (sCompareVal.Equals(sValue))                            
                                sFileRowNumber = sFileRowNumber + ", " + (i);                            
                        }
                    }
                }

                if (sFileRowNumber != string.Empty)
                {
                    sFileRowNumber = sFileRowNumber.Substring(2);
                    throw new DuplicateGeneralRegisterNumberExceptions(S_DUPLICATE_GENERAL_REGISTRATION_NUMBER + " " + sFileRowNumber + ".");
                }
                else
                {
                    bool bIsPresent = false;
                    for (int i = iRowcnt; i < dt.Rows.Count; i++)
                    {
                        sValue = (dt.Rows[i][0]).ToString().Trim();
                        if (sCompareVal.Equals(sValue))
                            bIsPresent = true;
                    }

                    if (bIsPresent)
                        
                        throw new DuplicateGeneralRegisterNumberExceptions(S_DUPLICATE_GENERAL_REGISTRATION_NUMBER_SYSTEM + ".");
                }
            }

            return false;
        }


        private bool CheckIsStudentUniqueNumberIsDuplicate(DataTable aoDataTable)
        {
            string sCompareVal;
            string sValue;
            string sFileRowNumber = "";

            StudentBL oStudentBL = new StudentBL();

            DataTable dt = oStudentBL.CheckIsStudentUniqueNumberIsDuplicate();
            List<int> lstRowNos = new List<int>();

            for (int iRowcnt = 0; iRowcnt < aoDataTable.Rows.Count; iRowcnt++)
            {
                sCompareVal = (aoDataTable.Rows[iRowcnt][I_XLS_STUDENT_UNIQUE_NUMBER]).ToString().Trim();

                if (sCompareVal != string.Empty)
                {
                    for (int i = iRowcnt + 1; i < aoDataTable.Rows.Count; i++)
                    {
                        sValue = (aoDataTable.Rows[i][I_XLS_STUDENT_UNIQUE_NUMBER]).ToString().Trim();
                        if (sValue != string.Empty)
                        {
                            if (sCompareVal.Equals(sValue))
                                sFileRowNumber = sFileRowNumber + ", " + i;
                        }
                    }
                }

                if (sFileRowNumber != string.Empty)
                {
                    sFileRowNumber = sFileRowNumber.Substring(2);
                    throw new DuplicateStudentUniqueNoExceptions(S_DUPLICATE_STUDENT_ID + sFileRowNumber + ".");
                }
                else
                {

                    bool bIsPresent = false;
                    for (int i = iRowcnt + 1; i < dt.Rows.Count; i++)
                    {
                        sValue = (dt.Rows[i][0]).ToString().Trim();
                        if (sCompareVal.Equals(sValue))
                            bIsPresent = true;
                    }
                    if (bIsPresent)
                    {
                        sFileRowNumber = sFileRowNumber.Substring(2);
                        throw new DuplicateStudentUniqueNoExceptions(S_DUPLICATE_STUDENT_ID_SYSTEM);
                    }
                }
            }
            return false;
        }

        private bool CheckIsStudentIsDuplicate(DataTable aoDataTable)
        {
            int iSchoolId = moStudentInfoStruct.iSchoolId;
            bool bIsDuplicateStudent;
            string sRowNumber = "";
            StudentBL oStudentBL = new StudentBL();

            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                string sRegistrationNumber = "";
                sRegistrationNumber = aoDataTable.Rows[iRowcount][I_XLS_REG_NO].ToString().Trim();
                string sFirstName = aoDataTable.Rows[iRowcount][I_XLS_FIRST_NAME].ToString().Trim();
                string sLastName = aoDataTable.Rows[iRowcount][I_XLS_LAST_NAME].ToString().Trim();
                DateTime dtBirthDate = Convert.ToDateTime(aoDataTable.Rows[iRowcount][I_XLS_DATE_OF_BIRTH]);

                bIsDuplicateStudent = oStudentBL.CheckIsStudentIsDuplicate(iSchoolId, sFirstName, sLastName, dtBirthDate);
                if (bIsDuplicateStudent)
                    sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";

            }
            if (sRowNumber != "")
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);
                throw new DuplicateStudentExceptions(CommonUtility.GetResourceValue("valDuplicateStudentException") + sRowNumber + ".");
            }
            else
                if (CheckDuplicateStudentInFile(aoDataTable))
                    throw new DuplicateStudentExceptions(CommonUtility.GetResourceValue("ValDuplicateStudent") + " " + sFileRowNumber + ".");

            return true;
        }

        private bool CheckIsRegNoHasValidPrefixorPostFix(DataTable aoDataTable)
        {
            int iSchoolId = moStudentInfoStruct.iSchoolId;
            string sRowNumber = "";
            StudentBL oStudentBL = new StudentBL();
            int iCount = 0;
            string sRegistrationNumber = "";

            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {

                long iRegNumber;
                sRegistrationNumber = aoDataTable.Rows[iRowcount][I_XLS_REG_NO].ToString().Trim();
                if (moStudentInfoStruct.sRegPrefix != string.Empty)
                {
                    if (sRegistrationNumber == moStudentInfoStruct.sRegPrefix.Trim() || !sRegistrationNumber.StartsWith(moStudentInfoStruct.sRegPrefix.Trim()))
                    {
                        sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                        iCount++;
                        break;
                    }
                    else
                    {
                        sRegistrationNumber = sRegistrationNumber.Substring(moStudentInfoStruct.sRegPrefix.Length);
                        //if (!long.TryParse(sRegistrationNumber, out iRegNumber))
                        //{
                        //    sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";

                        //}
                    }
                }
                //else
                if (moStudentInfoStruct.sRegPostfix != null)
                {
                    string sCompare = moStudentInfoStruct.sRegPostfix.Trim();
                    //sCompare = sCompare.Replace("-","").Trim();
                    if (sRegistrationNumber.Trim().Length > PostFixLength)
                    {
                        string sRegistratioNumberNew = sRegistrationNumber.Substring(sRegistrationNumber.Trim().Length - PostFixLength);
                        string sActualRegistratioNumber = sRegistrationNumber.Substring(0, sRegistrationNumber.Trim().Length - PostFixLength).Replace(moStudentInfoStruct.sRegPrefix.Trim(), string.Empty);
                        if (!long.TryParse(sActualRegistratioNumber, out iRegNumber))
                        {
                            sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                            iCount++;
                            break;
                        }

                        bool bIsNew = sCompare.Contains(sRegistratioNumberNew);
                        if (sRegistrationNumber.Trim() == moStudentInfoStruct.sRegPostfix.Trim() || !(bIsNew))
                        {
                            sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                            iCount++;
                            break;
                        }
                        else
                        {
                            sRegistrationNumber = sRegistrationNumber.Substring(0, sRegistrationNumber.Trim().Length - sRegistratioNumberNew.Trim().Length);
                            if (!long.TryParse(sRegistrationNumber, out iRegNumber))
                            {
                                sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                                iCount++;
                                break;
                            }
                        }
                    }
                    else
                        sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                }

                if (!long.TryParse(sRegistrationNumber, out iRegNumber))
                {
                    sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                }

            }
            if (sRowNumber != "")
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);

                if (moStudentInfoStruct.sRegPrefix != string.Empty && iCount != 0)
                    throw new InvalidRegisterNoPrefixExceptions(CommonUtility.GetResourceValue("valRegistrationNumberStartWith") + moStudentInfoStruct.sRegPrefix + CommonUtility.GetResourceValue("ValRegNoFollowedByNum") + sRowNumber + ".");
                else if (moStudentInfoStruct.sRegPostfix != string.Empty && iCount != 0)
                    throw new InvalidRegisterNoPrefixExceptions(CommonUtility.GetResourceValue("valRegistrationNoEndWith") + moStudentInfoStruct.sRegPostfix + '.' + CommonUtility.GetResourceValue("valAtRowNumbers") + sRowNumber + ".");
                else
                    throw new InvalidRegisterNoPrefixExceptions(CommonUtility.GetResourceValue("valRegistrationNumber") + sRowNumber + ".");
            }
            else
            {
                if (!CheckIsRollNumberDuplicate(aoDataTable))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// This method is used to validate mobile number.
        /// </summary>
        /// <param name="aoDataTable"></param>
        /// <returns></returns>
        private bool CheckIsMobileNumberIsValid(DataTable aoDataTable)
        {
            string sRowNumber = string.Empty;
            string sMobileRowNumber = string.Empty;
            string sMobileNumber = string.Empty;
            string sMobileNumberStart = string.Empty;
            Type tMobilecolumnType;
            object oMobileNo = null;

            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {

                oMobileNo = aoDataTable.Rows[iRowcount][I_XLS_MOBILE];
                sMobileNumber = Convert.ToString(aoDataTable.Rows[iRowcount][I_XLS_MOBILE]).Trim();
                tMobilecolumnType = oMobileNo.GetType();
                //if (!(tMobilecolumnType.FullName.Trim().Equals("System.Double")))
                //{
                //    sMobileRowNumber = sMobileRowNumber + (iRowcount + 1).ToString() + ", ";
                //}
                //else 
                if (sMobileNumber.Trim().Length != 10)
                {
                    sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                }
                else if (sMobileNumber.Trim().StartsWith("0"))
                    sMobileNumberStart = sMobileNumberStart + (iRowcount + 1).ToString() + ", ";

            }
            if (sMobileRowNumber != "")
            {
                sMobileRowNumber = sMobileRowNumber.Substring(0, sMobileRowNumber.Length - 2);
                throw new ValidExceptions(S_FORMAT_MOBILE_1 + sMobileRowNumber + ".");
            }
            else if (sRowNumber != "")
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);
                throw new ValidExceptions(S_VALID_MOBILE_1 + sRowNumber + ".");
            }
            else if (sMobileNumberStart != "")
            {
                sMobileNumberStart = sMobileNumberStart.Substring(0, sMobileNumberStart.Length - 2);
                throw new ValidExceptions(S_VALID_MOBILE_START_NUMBER_1 + sMobileNumberStart + ".");
            }
            return true;
        }

        /// <summary>
        /// This method is used to validate mobile number 2.
        /// </summary>
        /// <param name="aoDataTable"></param>
        /// <returns></returns>
        private bool CheckIsMobileNumber2IsValid(DataTable aoDataTable)
        {
            string sRowNumber = string.Empty;
            string sMobileNumber = string.Empty;
            string sMobileNumberStart = string.Empty;
            Type tMobilecolumnType;
            object oMobileNo = null;

            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {

                oMobileNo = aoDataTable.Rows[iRowcount][I_XLS_MOBILE_2];
                sMobileNumber = Convert.ToString(aoDataTable.Rows[iRowcount][I_XLS_MOBILE_2]).Trim();
                tMobilecolumnType = oMobileNo.GetType();
                if (sMobileNumber.Trim() != string.Empty && sMobileNumber.Trim().Length != 10)
                {
                    sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                }
                else if (sMobileNumber.Trim().StartsWith("0"))
                    sMobileNumberStart = sMobileNumberStart + (iRowcount + 1).ToString() + ", ";

            }
            if (sRowNumber != "")
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);
                throw new ValidMobileNumberExceptions(S_VALID_MOBILE_2 + sRowNumber + ".");
            }
            else if (sMobileNumberStart != "")
            {
                sMobileNumberStart = sMobileNumberStart.Substring(0, sMobileNumberStart.Length - 2);
                throw new ValidMobileNumberExceptions(S_VALID_MOBILE_START_NUMBER_2 + sMobileNumberStart + ".");
            }
            return true;
        }

        private bool CheckIsTeacherMobileNumberIsValid(DataTable aoDataTable)
        {
            int iSchoolId = moStudentInfoStruct.iSchoolId;
            string sRowNumber = "";
            string sMobileRowNumber = "";
            string sMobileNumber = "";
            Type tMobilecolumnType;
            object oMobileNo = null;

            StudentBL oStudentBL = new StudentBL();

            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                oMobileNo = aoDataTable.Rows[iRowcount][I_XLS_T_MOBILE];
                sMobileNumber = Convert.ToString(aoDataTable.Rows[iRowcount][I_XLS_T_MOBILE]).Trim();
                tMobilecolumnType = oMobileNo.GetType();
                if (!(tMobilecolumnType.FullName.Trim().Equals("System.String")))
                {
                    sMobileRowNumber = sMobileRowNumber + (iRowcount + 1).ToString() + ", ";
                }
                else if (sMobileNumber.Trim().Length != 10)
                {
                    sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                }
            }
            if (sMobileRowNumber != "")
            {
                sMobileRowNumber = sMobileRowNumber.Substring(0, sMobileRowNumber.Length - 2);
                throw new ValidExceptions(S_FORMAT_MOBILE + sMobileRowNumber + ".");
            }
            else if (sRowNumber != "")
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);
                throw new ValidExceptions(S_T_VALID_MOBILE + sRowNumber + ".");
            }
            return true;
        }

        private bool CheckIsRollNumberDuplicate(DataTable aoDataTable)
        {
            int iSchoolId = moStudentInfoStruct.iSchoolId;
            int iAcademicYearId = moStudentInfoStruct.iAcademicYearId;
            int iStandardId = moStudentInfoStruct.iStandardId;
            int iDivisionId = moStudentInfoStruct.iDivisionId;

            bool bIsDuplicateNumber;
            string sRowNumber = "";

            StudentBL oStudentBL = new StudentBL();

            for (int iRowcount = 0; iRowcount < aoDataTable.Rows.Count; iRowcount++)
            {
                if (aoDataTable.Rows[iRowcount][I_XLS_ROLL_NO].ToString() != "")
                {
                    int iRollNo = Convert.ToInt32(aoDataTable.Rows[iRowcount][I_XLS_ROLL_NO]);

                    bIsDuplicateNumber = oStudentBL.CheckIsRollNumberDuplicate(iSchoolId, iAcademicYearId, iStandardId, iDivisionId, iRollNo, 0);
                    if (bIsDuplicateNumber)
                    {
                        sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                    }
                }
            }
            if (sRowNumber != "")
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);
                throw new DuplicateRollNumberExceptions(S_DUPLICATE_ROLL_NO + sRowNumber + ".");
            }
            else if (CheckDuplicateRollNoInFile(aoDataTable))
            {
                throw new DuplicateRollNumberExceptions(S_DUPLICATE_ROLL_NO_EXCEL + " " + sFileRowNumber);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// This function is used to check duplicate roll no in file.
        /// </summary>
        /// <param name="aoDataTable"></param>
        /// <returns></returns>
        private bool CheckDuplicateRollNoInFile(DataTable aoDataTable)
        {
            int iCompareVal;
            int iValue;
            bool bFlag = false;
            int iFlag;
            for (int iRowcnt = 0; iRowcnt < aoDataTable.Rows.Count; iRowcnt++)
            {
                iCompareVal = Convert.ToInt32(aoDataTable.Rows[iRowcnt][I_XLS_ROLL_NO]);
                iFlag = 1;

                for (int i = iRowcnt + 1; i < aoDataTable.Rows.Count; i++)
                {
                    iValue = Convert.ToInt32(aoDataTable.Rows[i][I_XLS_ROLL_NO]);
                    if (iCompareVal == iValue)
                    {
                        bFlag = true;
                        if (sFileRowNumber != "")
                        {
                            string[] str = sFileRowNumber.Split(new char[] { ',' });

                            for (int iCnt = 0; iCnt < str.Length; iCnt++)
                            {
                                if (Convert.ToInt32(str[iCnt]) == (iCompareVal))
                                    iFlag = 0;
                            }
                            if (iFlag == 1)
                                sFileRowNumber = sFileRowNumber + ", " + (iCompareVal);
                        }
                        else
                            sFileRowNumber += (iCompareVal);
                    }
                }
            }

            return bFlag;
        }
       

        /// <summary>
        /// This function is used to check duplicate roll no in file.
        /// </summary>
        /// <param name="aoDataTable"></param>
        /// <returns></returns>
        private bool CheckDuplicateRegNoInFile(DataTable aoDataTable)
        {
            string sCompareVal;
            string sValue;
            bool bFlag = false;
            int iFlag;
            for (int iRowcnt = 0; iRowcnt < aoDataTable.Rows.Count; iRowcnt++)
            {
                sCompareVal = (aoDataTable.Rows[iRowcnt][I_XLS_REG_NO]).ToString().Trim();
                iFlag = 1;

                for (int i = iRowcnt + 1; i < aoDataTable.Rows.Count; i++)
                {
                    sValue = (aoDataTable.Rows[i][I_XLS_REG_NO]).ToString().Trim();
                    if (sCompareVal.Equals(sValue))
                    {
                        bFlag = true;
                        if (sFileRowNumber != "")
                        {
                            string[] str = sFileRowNumber.Split(new char[] { ',' });

                            for (int iCnt = 0; iCnt < str.Length; iCnt++)
                            {
                                if ((str[iCnt]).Equals(sCompareVal))
                                    iFlag = 0;
                            }
                            if (iFlag == 1)
                                sFileRowNumber = sFileRowNumber + ", " + (sCompareVal);
                        }
                        else
                            sFileRowNumber += (sCompareVal);
                    }
                }
            }

            return bFlag;
        }

        /// <summary>
        /// This function is used to check duplicate roll no in file.
        /// </summary>
        /// <param name="aoDataTable"></param>
        /// <returns></returns>
        private bool CheckDuplicateStudentInFile(DataTable aoDataTable)
        {
            string sCompareVal;
            string sFirstName;
            string sLastName;
            DateTime dtDOB;
            string sValue;
            string sFName;
            string sLName;
            DateTime dtBirthDate;
            bool bFlag = false;
            int iFlag;
            for (int iRowcnt = 0; iRowcnt < aoDataTable.Rows.Count; iRowcnt++)
            {
                sFirstName = (aoDataTable.Rows[iRowcnt][I_XLS_FIRST_NAME]).ToString().Trim();
                sLastName = (aoDataTable.Rows[iRowcnt][I_XLS_LAST_NAME]).ToString().Trim();
                dtDOB = Convert.ToDateTime(aoDataTable.Rows[iRowcnt][I_XLS_DATE_OF_BIRTH]);
                sCompareVal = (aoDataTable.Rows[iRowcnt][I_XLS_FIRST_NAME]).ToString().Trim();
                iFlag = 1;

                for (int i = iRowcnt + 1; i < aoDataTable.Rows.Count; i++)
                {
                    sValue = (aoDataTable.Rows[i][I_XLS_FIRST_NAME]).ToString().Trim();
                    sFName = (aoDataTable.Rows[i][I_XLS_FIRST_NAME]).ToString().Trim();
                    sLName = (aoDataTable.Rows[i][I_XLS_LAST_NAME]).ToString().Trim();
                    dtBirthDate = Convert.ToDateTime(aoDataTable.Rows[i][I_XLS_DATE_OF_BIRTH]);

                    if (sFirstName.Trim().Equals(sFName) && sLastName.Trim().Equals(sLName) && dtDOB == dtBirthDate)
                    {
                        bFlag = true;
                        if (sFileRowNumber != "")
                        {
                            string[] str = sFileRowNumber.Split(new char[] { ',' });

                            for (int iCnt = 0; iCnt < str.Length; iCnt++)
                            {
                                if ((str[iCnt]).Equals(i))
                                    iFlag = 0;
                            }
                            if (iFlag == 1)
                                sFileRowNumber = sFileRowNumber + ", " + (i);
                        }
                        else
                            sFileRowNumber += (i);
                    }
                }
            }
            return bFlag;
        }
    }
}