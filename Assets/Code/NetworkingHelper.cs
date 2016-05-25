using System;
using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;


public enum KoffieStates
{
    KoffieAan = 0,
    KoffieZetten = 1,
    KoffieUit = 2,
    Default = -1
}

public class NetworkingHelper : MonoBehaviour
{
    const string koffieAan = "koffieAan";
    const string koffieZetten = "koffieZetten";

    KoffieStates currentState = KoffieStates.Default;

    string url;

    int networkIp = 137;
    int subnetIp = 150;

    bool initializing = false;
    bool sendingRequest = false;

    TcpClient webSocket;
    NetworkStream webStream;
    StreamWriter webWriter;
    StreamReader webReader;

    void Start()
    {
        //url = String.Format("192.168.{0}.{1}", networkIp, subnetIp);

        //test only!
        url = "127.0.0.1";

        InitializeConnection();
    }

    void Update()
    {
        if (!webSocket.Connected)
        {
            InitializeConnection();
        }
        if (!webStream.CanRead)
        {
            InitializeConnection();
        }

    }

    void InitializeConnection()
    {
        if (initializing) { return;}

        try
        {
            webSocket = new TcpClient(url, 15326);
            webSocket.NoDelay = true;
            
        }
        catch (Exception)
        {
            return;
        }

        initializing = true;

        if (webSocket.Connected)
        {
            webStream = webSocket.GetStream();

            webReader = new StreamReader(webStream);
            webWriter = new StreamWriter(webStream,Encoding.ASCII);

            Debug.Log("Connected");
            initializing = false;
        }
    }

    public void KoffieAan()
    {
        if (sendingRequest) { return; }

        if (!webStream.CanWrite)
        {
            InitializeConnection();
        }

        currentState = KoffieStates.KoffieAan;

        StartCoroutine("WaitForConnection");

    }

    public void KoffieZetten()
    {

        if (sendingRequest) { return;}

        if (!webStream.CanWrite)
        {
            InitializeConnection();
        }

        currentState = KoffieStates.KoffieZetten;

        StartCoroutine("WaitForConnection");

    }


    public IEnumerator WaitForConnection()
    {
        if (sendingRequest) { yield break; }

        sendingRequest = true;

        while (initializing)
        {
            yield return new WaitForSeconds(1f);
        }

        Debug.Log("State: "+(int)currentState);

        WriteString((int)currentState);

        string responseString = GetResponse();

        if (responseString != "")
        {
            Debug.Log(String.Format("Koffie request: {0} response: {1} ",currentState,responseString));
        }

        sendingRequest = false;

        InitializeConnection();

        yield return null;
    }

    public void WriteString(int valueToWrite)
    {

        if (webStream.CanWrite)
        {
            webWriter.Write(valueToWrite);
            webWriter.Flush();
        }
    }


    public string GetResponse()
    {

        if (webStream.CanRead)
        {
            string returnString = webReader.ReadToEnd();

            return returnString;
        }
        return "No response!";
    }

    public void CloseConnection()
    {
        initializing = true;
        webWriter.Close();
        webReader.Close();
        webStream.Close();
        webSocket.Close();
    }
}
