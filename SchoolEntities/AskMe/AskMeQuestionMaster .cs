using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class AskMeQuestionMaster
    {
        public int QueryNo { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public int OwnerUserId { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public AskMeQuestionDetails AskMeQuestionDetails { get; set; }
        public int UserRoleId { get; set; }
        public int StudentUserId { get; set; }
        public DateTime CommunicationStartDate { get; set; }
        public bool IsPublished { get; set; }
        public bool IsClosedStatus { get; set; }
        public bool IsCommunicationStarted { get; set; }
        public bool IsInvalidQuery { get; set; }
        public string AssociatedCategories { get; set; }
        public bool ShowOwnerButton { get; set; }
        public bool IsQueryInUnsubmitState { get; set; }
        public bool AllowReply { get; set; }
        public bool AllowForward { get; set; }
        public bool AllowBackward { get; set; }
        public bool IsInvalidQuestion { get; set; }
        public bool ShowInvalidButton { get; set; }
        public int IsSubjectExpert { get; set; }
        public bool IsOwnerAssignmentSubmitted { get; set; }
        public bool ShowPublishButton { get; set; }
        public string CategoryNames { get; set; }
        public bool IsQueryPublished { get; set; }
        public bool IsPublishBtnEnabled { get; set; }
        public int TotalRowCount { get; set; }
        public bool IsCategoryEnabled { get; set; }
    }

    public class AskMeQuestionDetails
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public bool HasReadMessage { get; set; }
        public string AttachedFileName { get; set; }
        public string SenderName { get; set; }
        public int SenderUserId { get; set; }
        public string LastDescription { get; set; }
        public bool IsPublished { get; set; }
        public bool IsEditable { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsInvalidQuery { get; set; }
    }

    public class AskMeStatusMaster
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class AskMeCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
    }

    public class SubjectExperts
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool IsAssignExpert { get; set; }
    }

    public class AskMeCommunicationDetails
    {
        public int Id { get; set; }
        public DateTime CommunicationDate { get; set; }
        public string SenderUserName { get; set; }
        public string Communication { get; set; }
        public string MainQuestion { get; set; }
        public bool IsDisplayCommunication { get; set; }
        public bool IsPublished { get; set; }        
    }

    public class AskMeOwnerAssignment
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
        public int UserRoleId { get; set; }
        public string UserRole { get; set; }
    }

    public class AskMeReadReceiptDetails
    {
        public string Name { get; set; }
    }

}
