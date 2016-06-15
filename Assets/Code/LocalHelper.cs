using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

public class LocalHelper : MonoBehaviour
{
    public NetworkingHelper network;

    public Button alarmButton;

    public Dropdown[] tijdInfo = new Dropdown[5];

    public Dropdown alarmOption;

    public int alarmCount = 0;
    public bool dirty = false;
    public bool settingAlarm = false;

    StreamReader reader;
    XmlSerializer serializer = new XmlSerializer(typeof(AlarmSerialization));
    FileStream fileStream;
    AlarmSerialization fullfile;

    Thread checkAlarmTimes;

    private static string[] formats = new string[]
    {
            "MM/dd/yyyy HH:mm:ss tt",
            "M/dd/yyyy HH:mm:ss tt",
            "M/d/yyyy HH:mm:ss tt",
            "MM/d/yyyy HH:mm:ss tt",
            "MM/dd/yyyy H:mm:ss tt",
            "M/dd/yyyy H:mm:ss tt",
            "M/d/yyyy H:mm:ss tt",
            "MM/d/yyyy H:mm:ss tt",
    };


    public string pathForDocumentsFile(string filename)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(Path.Combine(path, "Documents"), filename);
        }

        else if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }

        else
        {
            string path = Application.dataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
    }

    void Awake()
    {
        fullfile = new AlarmSerialization();
        fullfile.alarms = new List<AlarmElement>();

        if (fileStream==null)
        {
            fileStream = new FileStream(pathForDocumentsFile("alarms.xml"), FileMode.OpenOrCreate);

            if (fileStream.Length>0)
            {
                reader =new StreamReader(fileStream);
            }

            if (reader != null)
            {

                fullfile = (AlarmSerialization)serializer.Deserialize(reader);
                alarmCount = fullfile.alarms.Count;
            }
        }
        fileStream.Close();

        checkAlarmTimes = new Thread(CheckAlarmTimes);
        checkAlarmTimes.IsBackground = true;
        checkAlarmTimes.Start();

    }

    private void CheckAlarmTimes()
    {
        while (true)
        {
            foreach (Alarm alarm in GetAlarms())
            {

                if (DateTime.Now>=alarm.AlarmTime())
                {
                    alarm.actionToPerform();
                    Debug.Log(alarm.AlarmTime().ToString());
                }
                Debug.Log("Checking alarms");
            }


            Thread.Sleep(1000);
        }
    }

    public bool CheckDatetime(DateTime time1, DateTime time2)
    {
        if (time1.Day != time2.Day)
        {
            return false;
        }
        if (time1.Month != time2.Month)
        {
            return false;
        }
        if (time1.Year != time2.Year)
        {
            return false;
        }
        if (time1.Hour != time2.Hour)
        {
            return false;
        }
        if (time1.Minute != time2.Minute)
        {
            return false;
        }

        return true;
    }

    public Alarm[] GetAlarms()
    {
        if (!dirty)
        {

            List<Alarm> alarms = new List<Alarm>();

            foreach (AlarmElement alarmElement in fullfile.alarms)
            {
                Alarm temp = new Alarm();
                DateTime alarmDateTime = DateTime.ParseExact(alarmElement.alarmTime, formats, CultureInfo.CurrentCulture, DateTimeStyles.None);
                switch (alarmElement.alarmAction)
                {
                    case "KoffieAan":
                        temp = new Alarm(alarmDateTime, KoffieAan);
                        break;
                    case "KoffieZetten":
                        temp = new Alarm(alarmDateTime, KoffieZetten);
                        break;
                }
                if (temp != null)
                {
                    alarms.Add(temp);
                }
            }
            return alarms.ToArray();

        }
        throw new Exception();
    }

    public void SetAlarm()
    {
        if (settingAlarm) { return;}

        settingAlarm = true;

        int jaarId = tijdInfo[4].value;
        int maandId = tijdInfo[3].value;
        int dagId = tijdInfo[2].value;
        int minuutId = tijdInfo[1].value;
        int urenId = tijdInfo[0].value;


        int jaar = int.Parse(tijdInfo[4].options[jaarId].text);
        int maand = int.Parse(tijdInfo[3].options[maandId].text);
        int dag = int.Parse(tijdInfo[2].options[dagId].text);
        int uren = int.Parse(tijdInfo[1].options[minuutId].text);
        int minuten = int.Parse(tijdInfo[0].options[urenId].text);

        if (jaar == null)
        {
            jaar = 0;
        }

        if (maand == null)
        {
            maand = 0;
        }

        if (dag == null)
        {
            dag = 0;
        }

        if (uren == null)
        {
            uren = 0;
        }

        if (minuten == null)
        {
            minuten = 0;
        }

        DateTime alarmTime = new DateTime(jaar, maand , dag , uren, minuten, 0);

        Alarm newAlarm = new Alarm();

        switch (alarmOption.value)
        {
            case 0:
                newAlarm = new Alarm(alarmTime, KoffieAan);
                break;
            case 1:
                newAlarm = new Alarm(alarmTime,KoffieZetten);
                break;
            default:
                newAlarm = new Alarm(DateTime.Now, null);
                break;

        }

        if (!newAlarm.Equals(null))
        {
            AlarmElement temp = new AlarmElement(newAlarm.AlarmTime().ToString(),newAlarm.actionToPerform.Method.Name);
            
            fullfile.alarms.Add(temp);
            alarmCount++;

            dirty = true;
        }

        settingAlarm = false;
    }

    public void KoffieAan()
    {
        network.KoffieAan();
    }

    public void KoffieZetten()
    {
        network.KoffieZetten1();
    }

    void OnApplicationQuit()
    {
        checkAlarmTimes.Abort();
        if (dirty)
        {
            if(!fileStream.CanRead)
                fileStream = new FileStream(pathForDocumentsFile("alarms.xml"), FileMode.OpenOrCreate);

            serializer.Serialize(fileStream, fullfile);
            fileStream.Close();

            dirty = false;
        }
    }
}
