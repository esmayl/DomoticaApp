using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour
{
    public NetworkingHelper networkObj;

	void Update ()
    {
	    if (Input.GetKeyUp(KeyCode.Escape))
	    {
            //networkObj.CloseConnection();

            Application.Quit();
	    }
	}
}
