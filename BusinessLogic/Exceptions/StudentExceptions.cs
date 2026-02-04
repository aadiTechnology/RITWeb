using System;

namespace BusinessLogic.Exceptions
{

    public class DuplicateRegisterNumberExceptions : Exception
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
        public DuplicateRegisterNumberExceptions(string asMessage) : base(asMessage)
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
        public DuplicateRegisterNumberExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }
    public class DuplicateGeneralRegisterNumberExceptions : Exception
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
        public DuplicateGeneralRegisterNumberExceptions(string asMessage)
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
        public DuplicateGeneralRegisterNumberExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }
    public class DuplicateStudentUniqueNoExceptions  : Exception
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
        public DuplicateStudentUniqueNoExceptions (string asMessage)
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
        public DuplicateStudentUniqueNoExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }


    public class DuplicateExceptions : Exception
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
        public DuplicateExceptions(string asMessage)
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
        public DuplicateExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class DuplicateStudentExceptions : Exception
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
        public DuplicateStudentExceptions(string asMessage)
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
        public DuplicateStudentExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class InvalidRegisterNoPrefixExceptions : Exception
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
        public InvalidRegisterNoPrefixExceptions(string asMessage)
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
        public InvalidRegisterNoPrefixExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class NullRegisterNumberExceptions : Exception
    {
        private string msMessage = " ";
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
        public NullRegisterNumberExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullRegisterNumberExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }
    public class NullGeneralRegisterNumberExceptions : Exception
    {
        private string msMessage = " ";
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
        public NullGeneralRegisterNumberExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullGeneralRegisterNumberExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }


    /// <summary>
    /// This exception is thrown when student's roll number is null.
    /// </summary>
    public class NullStudentRollNumberExceptions : Exception
    {
        private string msMessage = " ";

        /// <summary>
        ///  Constructs a new exception with the specified detail message.
        /// </summary>
        /// <param name="asMessage"></param>
        public NullStudentRollNumberExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

    }
    public class NullStudentFirstNameExceptions : Exception
    {
        private string msMessage = " ";
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
        public NullStudentFirstNameExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullStudentFirstNameExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class NullStudentMiddleNameExceptions : Exception
    {
        private string msMessage = " ";
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
        public NullStudentMiddleNameExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullStudentMiddleNameExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class NullStudentLastNameExceptions : Exception
    {
        private string msMessage = " ";
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
        public NullStudentLastNameExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullStudentLastNameExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class NullStudentMotherNameExceptions : Exception
    {
        private string msMessage = " ";
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
        public NullStudentMotherNameExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullStudentMotherNameExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }


    public class NullStudentDateofBirthExceptions : Exception
    {
        private string msMessage = " ";
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
        public NullStudentDateofBirthExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullStudentDateofBirthExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }


    public class NullStudentAdmissionDateExceptions : Exception
    {
        private string msMessage = " ";
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
        public NullStudentAdmissionDateExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullStudentAdmissionDateExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class NullStudentJoiningDateExceptions : Exception
    {
        private string msMessage = " ";
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
        public NullStudentJoiningDateExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullStudentJoiningDateExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class NullStudentSexExceptions : Exception
    {
        private string msMessage = " ";
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
        public NullStudentSexExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullStudentSexExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class NullStudentBloodGroupExceptions : Exception
    {
        private string msMessage = " ";
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
        public NullStudentBloodGroupExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullStudentBloodGroupExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class NullStudentParentNameExceptions : Exception
    {
        private string msMessage = " ";
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
        public NullStudentParentNameExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullStudentParentNameExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class NullStudentParentOccupationExceptions : Exception
    {
        private string msMessage = " ";
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
        public NullStudentParentOccupationExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullStudentParentOccupationExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class NullStudentAddressExceptions : Exception
    {
        private string msMessage = " ";
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
        public NullStudentAddressExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullStudentAddressExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class NullStudentCityExceptions : Exception
    {
        private string msMessage = " ";
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
        public NullStudentCityExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullStudentCityExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class NullStudentStateExceptions : Exception
    {
        private string msMessage = " ";
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
        public NullStudentStateExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullStudentStateExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class NullStudentPincodeExceptions : Exception
    {
        private string msMessage = " ";
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
        public NullStudentPincodeExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullStudentPincodeExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class NullStudentCategoryExceptions : Exception
    {
        private string msMessage = " ";
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
        public NullStudentCategoryExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullStudentCategoryExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class ValidateStudentSubAreaName : Exception
    {
        private string msMessage = " ";
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
        public ValidateStudentSubAreaName(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public ValidateStudentSubAreaName(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }
    }

    public class NullStudentCasteSubcasteExceptions : Exception
    {
        private string msMessage = " ";
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
        public NullStudentCasteSubcasteExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullStudentCasteSubcasteExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class NullPhotoFileExceptions : Exception
    {
        private string msMessage = " ";
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
        public NullPhotoFileExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullPhotoFileExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }


    public class NullStudentMobileExceptions : Exception
    {
        private string msMessage = " ";
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
        public NullStudentMobileExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }


        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullStudentMobileExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class DuplicateRollNumberExceptions : Exception
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
        public DuplicateRollNumberExceptions(string asMessage)
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
        public DuplicateRollNumberExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }


    public class ValidMobileNumberExceptions : Exception
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
        public ValidMobileNumberExceptions(string asMessage)
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
        public ValidMobileNumberExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class ValidPincodeExceptions : Exception
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
        public ValidPincodeExceptions(string asMessage)
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
        public ValidPincodeExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class ValidExceptions : Exception
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
        public ValidExceptions(string asMessage)
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
        public ValidExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }

    public class ValidEmailAddressExceptions : Exception
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
        public ValidEmailAddressExceptions(string asMessage)
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
        public ValidEmailAddressExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }
    public class NoRecordFoundExceptions : Exception
    {
        private string msMessage = string.Empty;
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
        public NoRecordFoundExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }
    }

    public class InvalidChallanNoExceptions : Exception
    {
        private string msMessage = string.Empty;
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
        public InvalidChallanNoExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }
    }

    public class InvalidVehicleDataExceptions : Exception
    {
        private string msMessage = string.Empty;
        public override string Message
        {
            get
            {
                return base.Message;
            }
        }

        public InvalidVehicleDataExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }
    }

    public class NullTeacherDateofJoningExceptions : Exception
    {
        private string msMessage = " ";
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
        public NullTeacherDateofJoningExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullTeacherDateofJoningExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }
    public class NullEmergencyContactException : Exception
    {
        private string msMessage = " ";
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
        public NullEmergencyContactException(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }
        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullEmergencyContactException(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }
    }

    public class NullSalutationNumber : Exception
    {
        private string msMessage = " ";
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
        public NullSalutationNumber(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }
        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public NullSalutationNumber(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }
    }
}
