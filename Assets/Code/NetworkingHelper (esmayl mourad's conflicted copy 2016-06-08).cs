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

    int networkIp = 1;
    int subnetIp = 7;

    bool initializing = false;
    bool sendingRequest = false;

    TcpListener responseListener;
    TcpClient responseSocket;

    TcpClient webSocket;
    NetworkStream webStream;
    StreamWriter webWriter;
    StreamReader responseReader;

    void Start()
    {
        url = String.Format("192.168.{0}.{1}", networkIp, subnetIp);

        Thread initThread = new Thread(InitializeConnection);
        initThread.IsBackground = true;
        initThread.Start();

        Thread responseThread = new Thread(CheckForResponse);
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
            webSocket = new TcpClient(url, 15326);
            debugString = ("Connected");
        }
        catch (Exception e)
        {
            debugString = (e.Message);
            initializing = false;
            return;
        }

        if (responseListener != null)
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

        if (webSocket.Connected)
        {
            webStream = webSocket.GetStream();

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
            InitializeConnection();
        }

        currentState = KoffieStates.KA;

        StartCoroutine("WaitForConnection");
    }

    public void KoffieZetten1()
    {

        if (sendingRequest) { return; }

        if (!webStream.CanWrite)
        {
            InitializeConnection();
        }

        currentState = KoffieStates.K1;

        StartCoroutine("WaitForConnection");
    }

    public void KoffieZetten2()
    {

        if (sendingRequest) { return; }

        if (!webStream.CanWrite)
        {
            InitializeConnection();
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
        webSocket.Close();


        yield return null;
    }

    public void CheckForResponse()
    {
        while (true)
        {
            if (responseListener != null)
            {
                if (responseListener.Pending())
                {
                    responseSocket = responseListener.AcceptTcpClient();
                    responseReader = new StreamReader(responseSocket.GetStream());

                    debugString = ("Read stream initialized");
                }

                if (responseReader != null)
                {
                    responseString = responseReader.ReadToEnd();
                }

                if (responseString != null)
                {
                   debugString=("Response: " + responseString);
                }

            }

            Thread.Sleep(500);
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

    public string GetResponse()
    {

        if (webStream.CanRead)
        {
            string returnString = responseReader.ReadToEnd();

            return returnString;
        }
        return "No response!";
    }

    public void CloseConnection()
    {
        webSocket.Close();
        responseReader.Close();
        webStream.Close();
        responseSocket.Close();
        responseListener.Stop();
    }

    void OnApplicationQuit()
    {
        CloseConnection();
    }
}
