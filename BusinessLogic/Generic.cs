using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using Utility;
using System.Reflection;

namespace BusinessLogic
{
    public class GenericReferenceList<T>
    {
        private Collection<T> objMasterCollection;
        private List<T> objMasterList;

        private int iAcadYrId;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aoCollection"></param>
        /// <param name="aiAcadYrId"></param>
        public GenericReferenceList(Collection<T> aoCollection, int aiAcadYrId)
        {
            objMasterCollection = aoCollection;
            iAcadYrId = aiAcadYrId;
        }

        public GenericReferenceList(List<T> aoList, int aiAcadYrId)
        {
            objMasterList = aoList;
            iAcadYrId = aiAcadYrId;
        }
     
        /// <summary>
        /// This is a wrapper method to to get RI messages.
        /// It gets a Hashtable  and passes on to the function to get reference messages. 
        /// </summary>
        /// <param name="asIdProperty"></param>
        /// <param name="asNameProperty"></param>
        /// <param name="asConfigureAction"></param>
        /// <param name="oReferId"></param>
        /// <returns></returns>
        public string CheckDependencies(string asIdProperty, string asNameProperty, string asConfigureAction, Constants.ReferenceId oReferId, bool bConsiderUpdate)
        {
            //get the id and name of the to be deleted/updated into hashtable.
            Hashtable oHash = GetHashTable(asIdProperty, asNameProperty, asConfigureAction, bConsiderUpdate);
            //call method to return references
            return GetReferenceMsg(oReferId, oHash);
        }

        public string CheckDependenciesForList(string asIdProperty, string asNameProperty, string asConfigureAction, Constants.ReferenceId oReferId, bool bConsiderUpdate)
        {
            //get the id and name of the to be deleted/updated into hashtable.
            Hashtable oHash = GetHashTableForList(asIdProperty, asNameProperty, asConfigureAction, bConsiderUpdate);
            //call method to return references
            return GetReferenceMsg(oReferId, oHash);
        }

        public string CheckDependenciesForCategory(string aiCategoryId, string asCategoryName, Constants.ReferenceId oReferId)
        {
            //get the id and name of the to be deleted/updated into hashtable.
            Hashtable oHash = GetHashTable(aiCategoryId, asCategoryName);
            //call method to return references
            return GetReferenceMsg(oReferId, oHash);
        }

        public void CheckDependenciesAndThrowException(string asIdProperty, string asNameProperty, Constants.ReferenceId oReferId)
        {
            //get the id and name of the to be deleted/updated into hashtable.
            Hashtable oHash = GetHashTable(asIdProperty, asNameProperty);
            //call method to return references
            string sMessage = GetReferenceMsg(oReferId, oHash);
            if (!string.IsNullOrEmpty(sMessage))
            {
                throw new Exceptions.ReferenceExceptions(sMessage);
            }
        }

        public void CheckDependenciesAndThrowException(string asIdProperty, string asNameProperty, string asConfigureAction, Constants.ReferenceId oReferId, bool bConsiderUpdate)
        {
            Hashtable oHash = GetHashTable(asIdProperty, asNameProperty, asConfigureAction, bConsiderUpdate);
            string sMessage = GetReferenceMsg(oReferId, oHash);
            if (!string.IsNullOrEmpty(sMessage))
            {
                throw new Exceptions.ReferenceExceptions(sMessage);
            }
        }
        /// <summary>
        /// This method calls the method to get referece messages.
        /// </summary>
        /// <param name="oReferId">ReferenceId which helps to get the dependancies.</param>
        /// <param name="oHash">Hash table of the objects to checked for RI</param>
        /// <returns> the message informing about violated dependencies/constraints. 
        /// Blank if none of the constraints are violated.</returns>
        private string GetReferenceMsg(Constants.ReferenceId oReferId, Hashtable oHash)
        {
            //message string 
            string sReturn = "";
            if (oHash.Count > 0)
            {
                ReferenceBL oRef = new ReferenceBL();
                sReturn = oRef.CheckDependencies(oReferId, oHash, iAcadYrId);
            }
            return sReturn;
        }
        /// <summary>
        /// This method creates the hash table of the objects to be considered for RI.
        /// </summary>
        /// <param name="asIdProperty">Name of the Property which contains Id</param>
        /// <param name="asNameProperty">Name of the Property which contains Name</param>
        /// <param name="asConfigureAction">Name of the Property which contains Configuration Action</param>
        /// <param name="abConsiderUpdate"> true: if RI check is to be performed for update also</param>
        /// <returns></returns>
        private Hashtable GetHashTable(string asIdProperty, string asNameProperty, string asConfigureAction, bool abConsiderUpdate)
        {
            Hashtable oHash = new Hashtable();
            IEnumerator oIEnum = objMasterCollection.GetEnumerator();
            //itereate through collection
            while (oIEnum.MoveNext())
            {

                T obj = (T)oIEnum.Current;
                Type oType = obj.GetType();

                PropertyInfo oProperty = oType.GetProperty(asConfigureAction);
                Constants.Action Action = (Constants.Action)oProperty.GetValue(obj, null);
                //if only delete action is to be considered for RI
                if (!abConsiderUpdate)
                {
                    //if object is to be included for RI check
                    //add into hashtable.
                    if (Action.Equals(Constants.Action.Delete))
                    {
                        //get the value of id
                        oProperty = oType.GetProperty(asIdProperty);
                        int iId = Convert.ToInt32(oProperty.GetValue(obj, null));

                        //get the value of name property.
                        oProperty = oType.GetProperty(asNameProperty);
                        string sName = Convert.ToString(oProperty.GetValue(obj, null));

                        oHash.Add(iId, sName);
                    }

                }
                else  //if update is to be considered for  RI.
                {
                    if (Action.Equals(Constants.Action.Delete) || Action.Equals(Constants.Action.Update))
                    {
                        oProperty = oType.GetProperty(asIdProperty);
                        int iId = Convert.ToInt32(oProperty.GetValue(obj, null));

                        oProperty = oType.GetProperty(asNameProperty);
                        string sName = Convert.ToString(oProperty.GetValue(obj, null));

                        oHash.Add(iId, sName);
                    }

                }

            }
            return oHash;
        }

        private Hashtable GetHashTableForList(string asIdProperty, string asNameProperty, string asConfigureAction, bool abConsiderUpdate)
        {
            Hashtable oHash = new Hashtable();
            IEnumerator oIEnum = objMasterList.GetEnumerator();
            //itereate through collection
            while (oIEnum.MoveNext())
            {

                T obj = (T)oIEnum.Current;
                Type oType = obj.GetType();

                PropertyInfo oProperty = oType.GetProperty(asConfigureAction);
                Constants.Action Action = (Constants.Action)oProperty.GetValue(obj, null);
                //if only delete action is to be considered for RI
                if (!abConsiderUpdate)
                {
                    //if object is to be included for RI check
                    //add into hashtable.
                    if (Action.Equals(Constants.Action.Delete))
                    {
                        //get the value of id
                        oProperty = oType.GetProperty(asIdProperty);
                        int iId = Convert.ToInt32(oProperty.GetValue(obj, null));

                        //get the value of name property.
                        oProperty = oType.GetProperty(asNameProperty);
                        string sName = Convert.ToString(oProperty.GetValue(obj, null));

                        oHash.Add(iId, sName);
                    }

                }
                else  //if update is to be considered for  RI.
                {
                    if (Action.Equals(Constants.Action.Delete) || Action.Equals(Constants.Action.Update))
                    {
                        oProperty = oType.GetProperty(asIdProperty);
                        int iId = Convert.ToInt32(oProperty.GetValue(obj, null));

                        oProperty = oType.GetProperty(asNameProperty);
                        string sName = Convert.ToString(oProperty.GetValue(obj, null));

                        oHash.Add(iId, sName);
                    }

                }

            }
            return oHash;
        }

        private Hashtable GetHashTable(string asIdProperty, string asNameProperty)
        {
            Hashtable oHash = new Hashtable();
            IEnumerator oIEnum = objMasterCollection.GetEnumerator();
            //itereate through collection
            while (oIEnum.MoveNext())
            {
                T obj = (T)oIEnum.Current;
                Type oType = obj.GetType();
                //add into hashtable.
                //get the value of id
                PropertyInfo oProperty = oType.GetProperty(asIdProperty);
                int iId = Convert.ToInt32(oProperty.GetValue(obj, null));

                //get the value of name property.
                oProperty = oType.GetProperty(asNameProperty);
                string sName = Convert.ToString(oProperty.GetValue(obj, null));
                oHash.Add(iId, sName);
            }
            return oHash;
        }

    }  
}
