using System;
using System.Data;
using DataCommunicator;
using System.Collections;

namespace BusinessLogic
{
    public class AccessException : Exception
    {
        private string msMessage = "";

        public override string Message
        {
            get
            {
                return msMessage;
            }
        }

        public AccessException(string asMessage)
        {
            msMessage = asMessage;
        }

    }
    public class StandardWiseDivisionListBL
    {
        #region constants
        const string S_MSG = "Please configure standards and divisions first.";
        #endregion
        StandardWiseDivisionListDC moStandardWiseDivisionDC;
        StandardWiseDivisionListDC.StandardDivision moStandardDivisionTable;

        public enum Action
        {
            Insert,
            Update,
            Delete
        };

        private Action eAction;
        
        #region constructors
        public StandardWiseDivisionListBL()
        {
            moStandardWiseDivisionDC = new StandardWiseDivisionListDC();
        }
        public StandardWiseDivisionListBL(int aiSchoolId)
        {
            moStandardWiseDivisionDC = new StandardWiseDivisionListDC(aiSchoolId);
            if (!moStandardWiseDivisionDC.IsValid)
            {
                throw(new AccessException(S_MSG));
            }
        }
        public StandardWiseDivisionListBL(int aiSchoolId, int aiStandardId, int aiDivisionId)
        {
            moStandardWiseDivisionDC = new StandardWiseDivisionListDC(aiSchoolId, aiStandardId, aiDivisionId);
            moStandardDivisionTable = moStandardWiseDivisionDC.StandardDivisionInfo;
        }
        #endregion
        #region prperties
        public Action ConfigurationAction
        {
            get { return eAction; }
            set { eAction = value; }
        }
        public int IdValue
        {
            get
            {
                return moStandardDivisionTable.iId;
            }
            set
            {
                moStandardDivisionTable.iId = value;
            }
        }
        public int SchoolId
        {
            get
            {
                return moStandardDivisionTable.iSchoolId;
            }
            set
            {
                moStandardDivisionTable.iSchoolId = value;
            }
        }
        public int StandardId
        {
            get
            {
                return moStandardDivisionTable.iStandardId;
            }
            set
            {
                moStandardDivisionTable.iStandardId = value;
            }
        }
        public int DivisionId
        {
            get
            {
                return moStandardDivisionTable.iDivisionId;
            }
            set
            {
                moStandardDivisionTable.iDivisionId = value;
            }
        }
        public string DivisionName
        {
            get
            {
                return moStandardDivisionTable.sDivisionName;
            }
            set
            {
                moStandardDivisionTable.sDivisionName = value;
            }
        }
        #endregion
        #region public methods
        public DataSet GetAllStandardDivisionsForSchool(int aiSchoolId)
        {
            return moStandardWiseDivisionDC.GetAllStandardDivisionsForSchool(aiSchoolId);
        }

        public DataSet GetAllDivisions(int aiSchoolId)
        {
            return moStandardWiseDivisionDC.GetAllDivisions(aiSchoolId);
        }
        public ArrayList GetAllDivisionsForStandard(int aiSchoolId, int aiStandardId)
        {
            return moStandardWiseDivisionDC.GetAllDivisionsForStandard(aiSchoolId, aiStandardId);
        }
        public ArrayList GetStudentDivisionsForStandard(int aiSchoolId, int aiStandardId)
        {
            return moStandardWiseDivisionDC.GetStudentDivisionsForStandard(aiSchoolId, aiStandardId);
        }
        public void UpdateConfigurationDetails(ArrayList aoArrayListConfigurationBL)
        {
            IEnumerator oEnum = aoArrayListConfigurationBL.GetEnumerator();
            ArrayList oArrayListInsertStatements = new ArrayList();

            while (oEnum.MoveNext())
            {
             //   BasicSchoolConfigurationBL oBasicSchoolConfigurationBL = (BasicSchoolConfigurationBL)oEnum.Current;
                StandardWiseDivisionListBL oStandardDivisionConfigurationBL = (StandardWiseDivisionListBL)oEnum.Current;
                switch (oStandardDivisionConfigurationBL.ConfigurationAction)
                {
                    case StandardWiseDivisionListBL.Action.Insert:
                        oArrayListInsertStatements.Add(((StandardWiseDivisionListBL)oEnum.Current).GetInsertStatementForConfigurationDetails());
                        break;

                    case StandardWiseDivisionListBL.Action.Update:
                        oArrayListInsertStatements.Add(((StandardWiseDivisionListBL)oEnum.Current).GetUpdateStatementForConfigurationDetails());
                        break;

                    case StandardWiseDivisionListBL.Action.Delete:
                        
                        oArrayListInsertStatements.Add(((StandardWiseDivisionListBL)oEnum.Current).GetDeleteStatementForConfigurationDetails());
                        break;
                }

            }
            moStandardWiseDivisionDC.UpdateConfigurationDetails(oArrayListInsertStatements);
        }
        public string GetInsertStatementForConfigurationDetails()
        {
            moStandardWiseDivisionDC.StandardDivisionInfo = moStandardDivisionTable;
            return moStandardWiseDivisionDC.GetInsertStatementForDivision();
        }

        public string GetUpdateStatementForConfigurationDetails()
        {
            moStandardWiseDivisionDC.StandardDivisionInfo = moStandardDivisionTable;
            return moStandardWiseDivisionDC.GetUpdateStatementForConfigurationDetails();
        }
        public string GetDeleteStatementForConfigurationDetails()
        {
            moStandardWiseDivisionDC.StandardDivisionInfo = moStandardDivisionTable;
            return moStandardWiseDivisionDC.GetDeleteStatementForConfigurationDetails();
        }
        #endregion
    }
}
