using System;
using UnityEngine;
using System.Collections;

public class Alarm
{
    DateTime alarmTime;
    public Actions.ActionToPerform actionToPerform;

    public Alarm(DateTime timeToSet,Actions.ActionToPerform newAction)
    {
        alarmTime = timeToSet;

        actionToPerform = newAction;
    }

    public Alarm()
    {
        alarmTime = DateTime.Now;

    }

    public DateTime AlarmTime()
    {
        return alarmTime;
    }
}
