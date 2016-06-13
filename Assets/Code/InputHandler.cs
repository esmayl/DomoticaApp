using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour
{
    public NetworkingHelper networkObj;

	void Update ()
    {
	    if (Input.GetKeyUp(KeyCode.Escape))
	    {
            Application.Quit();
	    }
	}

    void OnApplicationQuit()
    {
        networkObj.CloseResponseConnection();
        networkObj.CloseSendCon();
    }
}
