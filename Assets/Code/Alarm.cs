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
        actionToPerform += D;

    }

    public void D()
    {
        Debug.Log("Hello!");
    }

    public DateTime AlarmTime()
    {
        return alarmTime;
    }
}
