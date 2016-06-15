using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlarmLayout : MonoBehaviour
{
    [Tooltip("Drag in the localHelper object")]
    public LocalHelper localHelper;

    public GameObject template;

    bool update = false;

    public void Update()
    {
        if (localHelper.alarmCount != transform.childCount && !update && !localHelper.dirty)
        {
            update = true;


            foreach (Transform t in transform)
            {
                Destroy(t.gameObject);
            }

            if (localHelper.alarmCount <= 0) { return; }

            foreach (Alarm t in localHelper.GetAlarms())
            {
                GameObject temp = Instantiate(template);

                Transform alarmTime1 = temp.transform.FindChild("AlarmTemplate").FindChild("AlarmTijd");
                Transform alarmTime2 = temp.transform.FindChild("AlarmInfo").FindChild("BG").FindChild("AlarmTijd2");
                Transform alarmAction = temp.transform.FindChild("AlarmInfo").FindChild("BG").FindChild("AlarmAction");

                string time = String.Format("{0}/{1}/{2}",t.AlarmTime().Day,t.AlarmTime().Month,t.AlarmTime().Year);
                string time2 = t.AlarmTime().TimeOfDay.ToString();

                alarmTime1.GetComponent<Text>().text = time;
                alarmTime2.GetComponent<Text>().text = time2;
                alarmAction.GetComponent<Text>().text = t.actionToPerform.Method.Name;

                temp.transform.SetParent(transform, false);
            }

            update = false;
        }
    }

}
