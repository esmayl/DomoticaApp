using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour
{

	void Update ()
    {
	    if (Input.GetKeyUp(KeyCode.Escape))
	    {
	        Application.Quit();
	    }
	}
}
