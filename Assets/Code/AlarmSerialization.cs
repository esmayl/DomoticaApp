using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using System;

[XmlRoot("Root")]
public class AlarmSerialization
{

    public List<AlarmElement> documentRoot;
}

public class AlarmElement
{
    public string alarmTime;

    public string alarmAction;


    public AlarmElement(string time, string action)
    {
        alarmTime = time;
        alarmAction = action;
    }

    public AlarmElement()
    {
        alarmTime = DateTime.Now.ToString();
        alarmAction = "emptyAction";
    }
}
