// File Name- InvestmentAndIncomeBase.cs
// Created By - Sachin
// Created Date - 9 Feb 2013
// Description - This class is used to declare investment and income abstract methods.
public abstract class InvestmentAndIncomeBase : UserControlBase
{
    #region Data Member(s)

    private int miSelectedUserId;

    private int miSectionId;

    private int miRecordCount;

    private int miRegimId;
    
    #endregion

    #region Property(s)

    /// <summary>
    /// Selected User Id
    /// </summary>
    public int SelectedUserId
    {
        get { return this.miSelectedUserId; }
        set { this.miSelectedUserId = value; }        
    }

    /// <summary>
    /// Selected Section Id.
    /// </summary>
    public int SectionId
    {
        get { return this.miSectionId; }
        set { this.miSectionId = value; }        
    }

    /// <summary>
    /// Selected Section Id.
    /// </summary>
    public int RecordCount
    {
        get { return this.miRecordCount; }
        set { this.miRecordCount = value; }
    }

    public int RegimId
    {
        get { return this.miRegimId; }
        set { this.miRegimId = value; }
    }


    #endregion

    #region Abstract Method(s)

    /// <summary>
    /// This method will be used to save investment or income declarations.
    /// </summary>
    public abstract void Save();

    /// <summary>
    /// This method will be used to fetch investment or income declarations and fill up list view.
    /// </summary>
    public abstract void FillDeclarations();

    /// <summary>
    /// This method will be used to update listview details.
    /// </summary>
    /// <param name="asValue"></param>
    public virtual void UpdateDocumentCount(string asValue)
    {
    }

    #endregion
}