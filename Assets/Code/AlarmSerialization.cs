using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using System;

public class AlarmSerialization
{
    [XmlArray("Alarms")]
    public List<AlarmElement> alarms;
}


public class AlarmElement
{
    [XmlElement("Time")]
    public string alarmTime;

    [XmlElement("Action")]
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
