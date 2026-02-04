using System.Collections;
using Utility;

namespace DataCommunicator
{
    public class TeacherEducationDetailsDC : DataCommunicatorBaseDC
    {
        #region " Data Members & Properties "

        #region " Data Members "

        //This Structure is for the members of the Item

        public struct TeacherEduInfoStruct
        {
            public int miTeacherEducationId;
            public int miTeacherId;
            public int miQualificationId;
            public string miSpecialization;
            public int miYearOfPassingId;
            public string msPassingUniversity;
            public int miClassId;
            public int miInsertedById;
            public int miUpdatedById;

        }
        TeacherEduInfoStruct moTeacherEduInfoStruct;

        #endregion

        #region " Properties "

        public TeacherEduInfoStruct TeacherEduInfoStructure
        {
            get
            {
                return moTeacherEduInfoStruct;
            }
            set
            {
                moTeacherEduInfoStruct = value;
            }
        }

        #endregion

        #endregion

        #region " Overloaded Constructor"

        public TeacherEducationDetailsDC()
        {
            //Default constructor is used to create the object.
            moTeacherEduInfoStruct.miTeacherId = 0;
        }

        public TeacherEducationDetailsDC(int aiTeacherId)
        {
            // This Overloaded constructor get the parameter as TeacherId.
            // And is used to View / Edit the Item.
            // LoadTeacherEducationalDetails(aiTeacherId);
        }


        #endregion

        #region " Public Methods "

        /// <summary>
        /// constructs a statement for inserting an item.
        /// </summary>
        /// <returns></returns>
        public string GetEducationDetailsInsertStatement()
        {
            string sTeacherId;
            if (moTeacherEduInfoStruct.miTeacherId != 0)
                sTeacherId = "   " + moTeacherEduInfoStruct.miTeacherId;
            else
                sTeacherId = "   " + Constants.S_LAST_INSERTED_P_KEY;

            string sInsertStatement = "INSERT INTO Teacher_Education_Details (" +
                                  " Teacher_Id " +
                                  ", Qualification_Id " +
                                  ", Specialization " +
                                  ",Year_Of_Passing" +
                                  ",Passing_University " +
                                  ",Class_Id " +
                                  ",Inserted_By_id " +
                                  ",Updated_By_Id " +

                " ) VALUES ( " + sTeacherId +
                    " ,  " + moTeacherEduInfoStruct.miQualificationId +
                    " , N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherEduInfoStruct.miSpecialization, false) + "' " +
                    " ,  " + moTeacherEduInfoStruct.miYearOfPassingId +
                    " , N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherEduInfoStruct.msPassingUniversity, false) + "' " +
                    " ,  " + moTeacherEduInfoStruct.miClassId +
                    " ,  " + moTeacherEduInfoStruct.miInsertedById +
                    " ,  " + moTeacherEduInfoStruct.miUpdatedById +
                " ) ";

            return sInsertStatement;
        }

        #endregion

    }

    public class TeacherEducationDetailsCollectionDC : DataCommunicatorBaseDC
    {
        public TeacherEducationDetailsCollectionDC()
        {
        }

        public bool DeleteTeacherEducationDetails(ArrayList aoArrDeleteTeacherIds)
        {
            string sDeleteTeacherIdList = "(";
            for (int iCount = 0; iCount < aoArrDeleteTeacherIds.Count; iCount++)
            {
                sDeleteTeacherIdList = sDeleteTeacherIdList + aoArrDeleteTeacherIds[iCount];
                sDeleteTeacherIdList = sDeleteTeacherIdList + ",";
            }
            sDeleteTeacherIdList = sDeleteTeacherIdList + ")";
            sDeleteTeacherIdList = sDeleteTeacherIdList.Remove(sDeleteTeacherIdList.Length - 2, 1);

            string sSqlDeleteEducationDetails = " UPDATE Teacher_Education_Details " +
                                " SET Is_Deleted =N'" + Utility.Constants.C_YES + "'" +
                                " WHERE Teacher_Id in " + sDeleteTeacherIdList;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sSqlDeleteEducationDetails);
            return true;
        }

        public bool DeleteTeacherEducationDetails(int aiTeacherId)
        {
            string sSqlDeleteEducationDetails = " UPDATE Teacher_Education_Details " +
                                " SET Is_Deleted =N'" + Utility.Constants.C_YES + "'" +
                                " WHERE Teacher_Id = " + aiTeacherId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sSqlDeleteEducationDetails);
            return true;
        }


        public static string RemoveEducationalDetailsForTeacherId(int aiTeacherId)
        {
            // This procedure accepts parameter as asBusinessId. This method logically deletes all the 
            // locations for the passed businessid from the database.
            string sDeleteStatement;

            sDeleteStatement = " DELETE Teacher_Education_Details " +
                               " WHERE " +
                                   " teacher_id in (" + aiTeacherId + ")";

            return sDeleteStatement;
        }
    }
}
