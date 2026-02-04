using System;

namespace BusinessLogic.Exceptions
{

    public class NoResultFound : Exception
    {
        public override string Message
        {
            get
            {
                return base.Message;
            }
        }

        /// <summary>
        ///  Constructs a new exception with the specified detail message.
        /// </summary>
        /// <param name="asMessage"></param>
        public NoResultFound(string asMessage)
            : base(asMessage)
        {
            //msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NoResultFound(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }


    public class ResultNotPublished : Exception
    {
        public override string Message
        {
            get
            {
                return base.Message;
            }
        }

        /// <summary>
        ///  Constructs a new exception with the specified detail message.
        /// </summary>
        /// <param name="asMessage"></param>
        public ResultNotPublished(string asMessage)
            : base(asMessage)
        {
            //msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public ResultNotPublished(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class ResultNotAvailableForOtherDiv : Exception
    {
        public override string Message
        {
            get
            {
                return base.Message;
            }
        }

        /// <summary>
        ///  Constructs a new exception with the specified detail message.
        /// </summary>
        /// <param name="asMessage"></param>
        public ResultNotAvailableForOtherDiv(string asMessage)
            : base(asMessage)
        {
            //msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public ResultNotAvailableForOtherDiv(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class MarksNotAvailableForResult : Exception
    {
        public override string Message
        {
            get
            {
                return base.Message;
            }
        }

        /// <summary>
        ///  Constructs a new exception with the specified detail message.
        /// </summary>
        /// <param name="asMessage"></param>
        public MarksNotAvailableForResult(string asMessage)
            : base(asMessage)
        {
            //msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public MarksNotAvailableForResult(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

	public class BlockProgessReport : Exception
	{
		public override string Message
		{
			get
			{
				return base.Message;
			}
		}
		
		private string msBlockProgressReportReason;

		public string BlockProgressReportReason
		{
			get
			{
				return msBlockProgressReportReason;
			}
		}
		/// <summary>
		///  Constructs a new exception with the specified detail message.
		/// </summary>
		/// <param name="asMessage"></param>
		public BlockProgessReport(string asMessage)
			: base(asMessage)
		{
			//msMessage = asMessage;
		}

		/// <summary>
		/// Initializes a new instance of the System.Exception class with a specified
		/// error message and a reference to the inner exception that is the cause of
		/// this exception.
		/// </summary>
		/// <param name="asMessage"></param>
		/// <param name="ex"></param>
		public BlockProgessReport(string asMessage, Exception innerException)
			: base(asMessage, innerException)
		{
			//msMessage = asMessage;
		}

		public BlockProgessReport(string asMessage, string asBlockProgressReport)
			: base(asMessage)
		{
			msBlockProgressReportReason = asBlockProgressReport;
			//msMessage = asMessage;
		}

	}

}
