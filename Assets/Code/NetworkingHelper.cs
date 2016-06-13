using System;
using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;


public enum KoffieStates
{
    KA = 0,
    K1 = 1,
    K2 = 2,
    Default = -1
}

public class NetworkingHelper : MonoBehaviour
{
    public GameObject debugText;

    KoffieStates currentState = KoffieStates.Default;

    string url;
    string responseString;
    string debugString;

    int networkIp = 0;
    int subnetIp = 1;

    int errorCodeRfid = 3;
    
    bool initializing = false;
    bool sendingRequest = false;

    TcpListener responseListener;
    TcpClient responseClient;

    TcpClient webClient;
    NetworkStream webStream;
    StreamWriter webWriter;
    StreamReader responseReader;
    Thread responseThread;
    Thread initThread;

    void Start()
    {
        url = String.Format("10.0.{0}.{1}", networkIp, subnetIp);

        if (responseListener == null)
        {
            try
            {
                responseListener = new TcpListener(IPAddress.Any, 15327);
                responseListener.Start();
                debugString = ("Listening");
            }
            catch (Exception e)
            {
                debugString = (e.Message);
                initializing = false;
                return;
            }
        }

        initThread = new Thread(InitializeConnection);
        initThread.IsBackground = true;
        initThread.Start();

        responseThread = new Thread(CheckForResponse);
        responseThread.IsBackground = true;
        responseThread.Start();
    }

    void Update()
    {
        if (debugString != null)
        {
            debugText.GetComponent<Text>().text = debugString;
        }
    }

    void InitializeConnection()
    {
        if (initializing || sendingRequest) { return; }

        initializing = true;

        try
        {
            webClient = new TcpClient(url, 15326);
            debugString = ("Connected");
        }
        catch (Exception e)
        {
            debugString = (e.Message);
            initializing = false;
            return;
        }



        if (webClient.Connected)
        {
            webStream = webClient.GetStream();

            webWriter = new StreamWriter(webStream, Encoding.ASCII);

            debugString = ("Write stream initialized");
        }

        webClient.Close();

        initializing = false;

    }

    void InitWriter()
    {
        try
        {
            webClient = new TcpClient(url, 15326);
            debugString = ("Connected");
        }
        catch (Exception e)
        {
            debugString = (e.Message);
            initializing = false;
            return;
        }

        if (webClient.Connected)
        {
            webStream = webClient.GetStream();

            webWriter = new StreamWriter(webStream, Encoding.ASCII);

            debugString = ("Write stream initialized");
        }

        initializing = false;

    }

    public void KoffieAan()
    {
        if (sendingRequest) { return; }

        if (!webStream.CanWrite)
        {
            InitWriter();
        }

        currentState = KoffieStates.KA;

        StartCoroutine("WaitForConnection");

    }

    public void KoffieZetten1()
    {

        if (sendingRequest) { return; }

        if (!webStream.CanWrite)
        {
            InitWriter();

        }

        currentState = KoffieStates.K1;

        StartCoroutine("WaitForConnection");

    }

    public void KoffieZetten2()
    {

        if (sendingRequest) { return; }

        if (!webStream.CanWrite)
        {
            InitWriter();
        }

        currentState = KoffieStates.K2;

        StartCoroutine("WaitForConnection");


    }


    public IEnumerator WaitForConnection()
    {
        if (sendingRequest) { yield break; }

        sendingRequest = true;

        while (initializing)
        {
            debugString=("Initing");
            yield return new WaitForSeconds(0.1f);
        }



        debugString = ("State: " + currentState.ToString());

        WriteString(currentState.ToString());

        sendingRequest = false;

        CloseSendCon();

        yield return null;
    }

    public void CheckForResponse()
    {
        while (true)
        {
            Debug.Log("Running");
            if (responseListener != null)
            {
                if (responseListener.Pending())
                {
                    responseClient = responseListener.AcceptTcpClient();
                    responseReader = new StreamReader(responseClient.GetStream());

                    debugString = ("Read stream initialized");
                }

                if (responseReader != null)
                {
                    responseString = responseReader.ReadToEnd();
                    CloseResponseConnection();
                }

                if (responseString != null)
                {
                    if (responseString.Trim() == errorCodeRfid + "")
                    {
                        debugString = ("RFID: niet ingechecket");
                        continue;
                    }
                    if (responseString.Trim() != "")
                    {
                        debugString = ("Response: " + responseString);
                        continue;
                    }
                }
            }
            Thread.Sleep(50);
        }
    }

    public void WriteString(string valueToWrite)
    {

        if (webStream.CanWrite)
        {
            webWriter.Write(valueToWrite);
            webWriter.Flush();
            webWriter.Close();
        }

    }
    public void CloseResponseConnection()
    {
        responseReader.Close();
        responseClient.Close();
    }

    public void CloseSendCon()
    {
        webClient.Close();
        webStream.Close();
        webWriter.Close();
    }

}
