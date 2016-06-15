using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KoffieLight : MonoBehaviour
{
    public NetworkingHelper networkHelper;
    public GameObject koffieZetObj;

    CanvasGroup koffieZetGroup;
    Image thisImage;

    bool startedBlink = false;
    bool canTurnOff = false;

	void Start ()
	{
	    thisImage = GetComponent<Image>();
	    if (koffieZetObj.activeSelf)
	    {
	        koffieZetGroup = koffieZetObj.GetComponent<CanvasGroup>();
	        koffieZetGroup.interactable = false;
	    }
	}
	
	void Update ()
    {
	    if (networkHelper.koffieUit && canTurnOff)
	    {
	        canTurnOff = false;
	        thisImage.color = Color.red;
	    }
	    if (networkHelper.opwarmen && !startedBlink)
	    {
	        startedBlink = true;
	        StartCoroutine("BlinkLight");
	    }
	}

    IEnumerator BlinkLight()
    {
        int seconds = 100;
        float blinktime = 0.5f;
        float blinkCounter = 0;

        while (seconds>0)
        {

            if (blinkCounter < blinktime)
            {
                blinkCounter+=0.5f;
            }
            else
            {
                blinkCounter = 0;
                if (thisImage.color == Color.green)
                {
                    thisImage.color = Color.red;
                }
                else
                {
                    thisImage.color = Color.green;
                }
            }

            seconds--;

            yield return new WaitForSeconds(0.5f);
        }
        koffieZetObj.GetComponent<CanvasGroup>().interactable = true;

        thisImage.color = Color.green;
        networkHelper.opwarmen = false;
        canTurnOff = true;
        yield break;
    }

    void OnApplicationQuit()
    {
        StopAllCoroutines();
    }
}
