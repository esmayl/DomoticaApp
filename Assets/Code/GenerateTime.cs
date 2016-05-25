using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GenerateTime : MonoBehaviour
{
    private Dropdown list;



    void Start()
    {

        if (GetComponent<Dropdown>())
        {
            list = GetComponent<Dropdown>();
        }

        if (gameObject.name == "Uren")
        {
            for (int i = 1; i < 24; i++)
            {
                list.options.Add(new Dropdown.OptionData("" + i));
            }
        }

        if (gameObject.name == "Minuten")
        {
            for (int i = 0; i < 59; i++)
            {
                if (i < 10)
                {
                    list.options.Add(new Dropdown.OptionData("0" + i));
                }
                else
                {
                    list.options.Add(new Dropdown.OptionData("" + i));
                }
            }
        }

        if (gameObject.name == "Dag")
        {
            for (int i = 1; i < 32; i++)
            {
                list.options.Add(new Dropdown.OptionData("" + i));
            }
        }

        if (gameObject.name == "Maand")
        {

        }

    }
}