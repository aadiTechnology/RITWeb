using System;
using System.Collections;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Utility;
using DataCommunicator;

namespace BusinessLogic
{
    public class WeekEndMasterBL
    {

        private WeekEndMasterDC.WeekEndMasterStruct moWeekEndMasterStruct;
        private WeekEndMasterDC moWeekEndMasterDC = new WeekEndMasterDC();
        public Constants.Action eAction;

        public int AcademicYearId
        {
            get
            {
                return moWeekEndMasterStruct.miAcademicYearId;
            }
            set
            {
                moWeekEndMasterStruct.miAcademicYearId = value;
            }
        }

        public int WeekDaysId
        {
            get
            {
                return moWeekEndMasterStruct.miWeekEndId;
            }
            set
            {
                moWeekEndMasterStruct.miWeekEndId = value;
            }
        }

        public int SchoolId
        {
            get
            {
                return moWeekEndMasterStruct.miSchoolId;
            }
            set
            {
                moWeekEndMasterStruct.miSchoolId = value;
            }
        }

        public string WeekDayShortName
        {
            get
            {
                return moWeekEndMasterStruct.msWeekEndShortName;
            }
            set
            {
                moWeekEndMasterStruct.msWeekEndShortName = value;
            }
        }

        public int OriginalWeekDaysId
        {
            get
            {
                return moWeekEndMasterStruct.miOriginalWeekEndId;
            }
            set
            {
                moWeekEndMasterStruct.miOriginalWeekEndId = value;
            }
        }

        public string IsDeleted
        {
            get
            {
                return moWeekEndMasterStruct.msIsDeleted;
            }
            set
            {
                moWeekEndMasterStruct.msIsDeleted = value;
            }
        }

        public DateTime InsertDate
        {
            get
            {
                return moWeekEndMasterStruct.mdtInsertDate;
            }
            set
            {
                moWeekEndMasterStruct.mdtInsertDate = value;
            }
        }

        public int InsertedByid
        {
            get
            {
                return moWeekEndMasterStruct.miInsertedByid;
            }
            set
            {
                moWeekEndMasterStruct.miInsertedByid = value;
            }
        }

        public DateTime UpdateDate
        {
            get
            {
                return moWeekEndMasterStruct.mdtUpdateDate;
            }
            set
            {
                moWeekEndMasterStruct.mdtUpdateDate = value;
            }
        }

        public int UpdatedById
        {
            get
            {
                return moWeekEndMasterStruct.miUpdatedById;
            }
            set
            {
                moWeekEndMasterStruct.miUpdatedById = value;
            }
        }

        public Constants.Action ConfigurationAction
        {
            get
            {
                return eAction;
            }
            set
            {
                eAction = value;
            }
        }

        public string WeekEndName
        {
            get
            {
                return moWeekEndMasterStruct.msWeekEndName;
            }
            set
            {
                moWeekEndMasterStruct.msWeekEndName = value;
            }
        }

        public bool IsStaffApplicable
        {
            get
            {
                return moWeekEndMasterStruct.mbIsStaffApplicable;
            }
             set
            {
                moWeekEndMasterStruct.mbIsStaffApplicable = value;
            }
        }
        
       #region Constructors

        public WeekEndMasterBL()
        {
        }
        #endregion

        /// <summary>
        /// This method is used to retrieve insert statement.
        /// </summary>
        public string InsertStatmentWeekEndMaster()
        {
            moWeekEndMasterDC.WeekEndMasterStructDetails = moWeekEndMasterStruct;
            return moWeekEndMasterDC.InsertWeekEndMaster();
        }

        /// <summary>
        /// This method is used to retrieve delete statement.
        /// </summary>
        public string DeleteStatmentWeekEndMaster()
        {

            moWeekEndMasterDC.WeekEndMasterStructDetails = moWeekEndMasterStruct;
            return moWeekEndMasterDC.DeleteWeekEndMaster();
        }

        /// <summary>
        /// This statement is used to retrieve update statement.
        /// </summary>
        public string UpdateStatementWeekEndMaster()
        {
            moWeekEndMasterDC.WeekEndMasterStructDetails = moWeekEndMasterStruct;
            return moWeekEndMasterDC.UpdateWeekEndMaster();
        }

        /// <summary>
        /// This function is used to check whether the Weekends are applicable to all staff.
        /// </summary>
        public bool ChkIfOtherStaffApplicable(int aiSchoolId, int aiAcademicYearId)
        {
            return moWeekEndMasterDC.chkIfOtherStaffApplicable(aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This function is used get all weekends.
        /// </summary>
        public List<int> GetAllWeeknds(int aiSchoolId, int aiAcademicYearId)
        {
            return moWeekEndMasterDC.GetAllWeekends(aiSchoolId, aiAcademicYearId);
        }
    }

    /// <summary>
    /// This collection class is used to update all weekend configuration details. 
    /// </summary>
    public class WeekEndConfigCollectionBL : IEnumerable
    {
        private Collection<WeekEndMasterBL> moWeekEndConfigListBL = null;
        WeekEndMasterCollectionDC oWeekEndMasterCollectionDC;

        public Collection<WeekEndMasterBL> WeekdaysConfigListBL
        {
            get
            {
                return moWeekEndConfigListBL;
            }
            set
            {
                moWeekEndConfigListBL = value;
            }
        }

        public WeekEndConfigCollectionBL()
        {
            moWeekEndConfigListBL = new Collection<WeekEndMasterBL>();
            oWeekEndMasterCollectionDC = new WeekEndMasterCollectionDC();
        }

        /// <summary>
        /// This method is used to add collection data.
        /// </summary>
        /// <param name="aoWeekDaysMasterBL"></param>
        public void Add(WeekEndMasterBL aoWeekDaysMasterBL)
        {
            moWeekEndConfigListBL.Add(aoWeekDaysMasterBL);
        }

        /// <summary>
        /// This method is used to remove collection data.
        /// </summary>
        /// <param name="aoWeekDaysMasterBL"></param>
        public void Remove(WeekEndMasterBL aoWeekDaysMasterBL)
        {
            moWeekEndConfigListBL.Remove(aoWeekDaysMasterBL);

        }

        public IEnumerator GetEnumerator()
        {
            return new WeekEndCollectionEnumerator(this);
        }

        /// <summary>
        /// This method is used to update all weekend configuration details.
        /// </summary>
        public void UpdateAllWeekEndConfigurationDetails(int aiAcadYrId)
        {
            {
                IEnumerator oEnum = moWeekEndConfigListBL.GetEnumerator();
                ArrayList oArrayListInsertWeekEnd = new ArrayList();
                while (oEnum.MoveNext())
                {
                    WeekEndMasterBL oWeekEndsMasterBL = (WeekEndMasterBL)oEnum.Current;
                    switch (oWeekEndsMasterBL.ConfigurationAction)
                    {
                        case Constants.Action.Insert:
                            oArrayListInsertWeekEnd.Add(((WeekEndMasterBL)oEnum.Current).InsertStatmentWeekEndMaster());
                            break;
                        case Constants.Action.Delete:
                            oArrayListInsertWeekEnd.Add(((WeekEndMasterBL)oEnum.Current).DeleteStatmentWeekEndMaster());
                            break;
                        case Constants.Action.Update:
                            oArrayListInsertWeekEnd.Add(((WeekEndMasterBL)oEnum.Current).UpdateStatementWeekEndMaster());
                            break;
                    }
                }
            }
        }

        private class WeekEndCollectionEnumerator : IEnumerator
        {
            #region DataMember
            private int position = -1;
            private WeekEndConfigCollectionBL moWeekEndCollection;
            #endregion

            #region Constructor
            public WeekEndCollectionEnumerator(WeekEndConfigCollectionBL aoWeekEndCollection)
            {
                moWeekEndCollection = aoWeekEndCollection;
            }
            #endregion

            #region Public Method
            // Declare the MoveNext method required by IEnumerator:
            public bool MoveNext()
            {
                if (position < moWeekEndCollection.moWeekEndConfigListBL.Count - 1)
                {
                    position++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // Declare the Reset method required by IEnumerator:
            public void Reset()
            {
                position = -1;
            }
            #endregion

            #region Property
            // Declare the Current property required by IEnumerator:
            public object Current
            {
                get
                {
                    return moWeekEndCollection.moWeekEndConfigListBL[position];
                }
            }
            #endregion
        }
    }
}
