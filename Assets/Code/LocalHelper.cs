using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LocalHelper : MonoBehaviour
{
    public NetworkingHelper network;

    public Button alarmButton;

    public Dropdown[] tijdInfo = new Dropdown[5];

    public Dropdown alarmOption;

    public void SetAlarm()
    {
        DateTime alarmTime = new DateTime(int.Parse(tijdInfo[4].options[tijdInfo[4].value].text), int.Parse(tijdInfo[3].options[tijdInfo[3].value].text), int.Parse(tijdInfo[2].options[tijdInfo[2].value].text), int.Parse(tijdInfo[0].options[tijdInfo[0].value].text),
            int.Parse(tijdInfo[1].options[tijdInfo[1].value].text), 0);

        Alarm newAlarm = new Alarm();

        switch (alarmOption.value)
        {
            case 0:
                newAlarm = new Alarm(alarmTime, KoffieAan);
                break;
            case 1:
                newAlarm = new Alarm(alarmTime,KoffieZetten);
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;

        }

        if (!newAlarm.Equals(null))
        {
            newAlarm.actionToPerform();

            Debug.Log(newAlarm.AlarmTime());
        }
    }

    public void Test()
    {
        Debug.Log("Helooooo");
    }

    public void KoffieAan()
    {
        network.KoffieAan();
    }

    public void KoffieZetten()
    {
        network.KoffieZetten();
    }
}
