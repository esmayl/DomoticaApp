using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;

public class NetworkingHelper : MonoBehaviour
{
    const string checkedIn = "koffieAan";

    string currentIp;
    string url;

    int networkIp = 1;
    int subnetIp = 107;

    HttpWebRequest webLink;

    void Start()
    {
        currentIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
        url = String.Format("http://192.168.{0}.{1}", networkIp, subnetIp);
    }

    public void KoffieAan()
    {
        webLink = new HttpWebRequest(new Uri(url));
        
        webLink.UserAgent = "Domotica app " + currentIp;
        webLink.Method = "POST";
        webLink.ContentLength = checkedIn.Length;
        webLink.ContentType = "application/x-www-form-urlencoded";

        Stream webStream = webLink.GetRequestStream();

        byte[] encoded = Encoding.UTF8.GetBytes(checkedIn);

        webStream.Write(encoded,0,encoded.Length);

        webStream.Close();

        WebResponse response = webLink.GetResponse();

        webStream = response.GetResponseStream();

        StreamReader reader = new StreamReader(webStream);

        string responseString = reader.ReadToEnd();

        if (responseString != "")
        {
            reader.Close();
            webStream.Close();
            response.Close();

            Debug.Log("KoffieAan response: "+responseString);

        }

    }


}
