﻿namespace DO;

[Serializable]
public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException(string? message) : base(message) { }
}

[Serializable]
public class DalAlreadyExistsException : Exception
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}

[Serializable]
public class DalDeletionImpossible : Exception
{
    public DalDeletionImpossible(string? message) : base(message) { }
}
public class DalWrongValueException : Exception
{
    public DalWrongValueException(string? message) : base(message) { }
}
public class DalXMLFileLoadCreateException : Exception
{
    public DalXMLFileLoadCreateException(string? message) : base(message) { }
}