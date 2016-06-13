using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class LocalHelper : MonoBehaviour
{
    public NetworkingHelper network;

    public Button alarmButton;

    public Dropdown[] tijdInfo = new Dropdown[5];

    public Dropdown alarmOption;


    XmlReader reader;
    XmlSerializer serializer = new XmlSerializer(typeof(AlarmSerialization));
    Stream fileStream;


    void Start()
    {
        fileStream = new FileStream("alarms.xml",FileMode.OpenOrCreate);
        reader = XmlReader.Create(fileStream);
    }

    public void SetAlarm()
    {
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
            case 2:
                break;
            case 3:
                break;
            default:
                break;

        }

        if (!newAlarm.Equals(null))
        {
            AlarmSerialization fullfile = new AlarmSerialization();
            fullfile.documentRoot = new List<AlarmElement>();

            if (!fileStream.CanRead)
            {
                fileStream = new FileStream("alarms.xml", FileMode.Open);

                if (fileStream.CanRead)
                {
                    reader = XmlReader.Create(fileStream);
                }
            }



            //if (serializer.CanDeserialize(reader))
            //{
            //    fullfile.documentRoot = ((AlarmSerialization) serializer.Deserialize(reader)).documentRoot;
            //}
            //else
            //{
            //    fullfile.documentRoot = new List<AlarmElement>();
            //}

            AlarmElement temp = new AlarmElement(newAlarm.AlarmTime().ToString(),newAlarm.actionToPerform.Method.Name);
            
            fullfile.documentRoot.Add(temp);

            serializer.Serialize(fileStream,fullfile);

            fileStream.Flush();
            fileStream.Close();
            reader.Close();

            //newAlarm.actionToPerform();

            Debug.Log(newAlarm.AlarmTime());
        }
    }

    public void KoffieAan()
    {
        network.KoffieAan();
    }

    public void KoffieZetten()
    {
        network.KoffieZetten1();
    }
}
