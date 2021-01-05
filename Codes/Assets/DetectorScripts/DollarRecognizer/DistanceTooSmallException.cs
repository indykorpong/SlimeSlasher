using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DistanceTooSmallException : Exception
{
    public DistanceTooSmallException()
    { }

    public DistanceTooSmallException(string message)
        : base(message)
    { }

    public DistanceTooSmallException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
