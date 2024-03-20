using System.Reflection.Metadata;

namespace BO;

[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
            : base(message, innerException) { }
}

[Serializable]
public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }
    public BlNullPropertyException(string message, Exception innerException)
          : base(message, innerException) { }
}

[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException)
          : base(message, innerException) { }
}

[Serializable]
public class BlDeletionImpossible : Exception
{
    public BlDeletionImpossible(string? message) : base(message) { }
    public BlDeletionImpossible(string message, Exception innerException)
          : base(message, innerException) { }
}
public class BlWrongValueException : Exception
{
    public BlWrongValueException(string? message) : base(message) { }
    public BlWrongValueException(string message, Exception innerException)
          : base(message, innerException) { }
}
public class BlTaskCantBeAssignedException : Exception
{
    public BlTaskCantBeAssignedException(string? message) : base(message) { }
    public BlTaskCantBeAssignedException(string message, Exception innerException)
          : base(message, innerException) { }
}
public class BlDataException : Exception
{
    public BlDataException(string? message) : base(message) { }
    public BlDataException(string message, Exception innerException)
          : base(message, innerException) { }
}
public class BlNotAppropriateTheProjectStageException : Exception
{
    public BlNotAppropriateTheProjectStageException(string? message) : base(message) { }
    public BlNotAppropriateTheProjectStageException(string message, Exception innerException)
          : base(message, innerException) { }
}
public class BlScheduledDateException : Exception
{
    public BlScheduledDateException(string? message) : base(message) { }
    public BlScheduledDateException(string message, Exception innerException)
          : base(message, innerException) { }
}
//public class DalXMLFileLoadCreateException : Exception
//{
//    public DalXMLFileLoadCreateException(string? message) : base(message) { }
//}
