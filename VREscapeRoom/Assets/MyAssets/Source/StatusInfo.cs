using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusCode
{
    None,
    ERROR,
    ON,
    OFF,
    LOCKED,
    UNLOCKED,
}

public class StatusInfo
{
    public StatusCode[] statusCodes;
    public string objectName;
    public string statusMessage;
}