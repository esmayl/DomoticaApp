using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LocalHelper : MonoBehaviour
{

    public Button alarmButton;
    public Dropdown uren;
    public Dropdown minuten;
    public Dropdown alarmOption;

    public void SetAlarm()
    {
        Alarm newAlarm = new Alarm(new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,int.Parse(uren.options[uren.value].text),int.Parse(minuten.options[minuten.value].text),0), Test);

        newAlarm.actionToPerform();
    }

    public void Test()
    {
        Debug.Log("Helooooo");
    }
}
