using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject flashLightON;
    public GameObject flashLightOFF;
    public GameObject flashLightOB;

    public GameObject m4;
    public GameObject m4OB;

    public GameObject glock;
    public GameObject glockOB;

    public GameObject knife;
    public GameObject knifeOB;




    void Start()
    {
        flashLightON.SetActive(false);
        
    }




    void Update()
    {
        if(flashLightOB.activeInHierarchy)
        {
            flashLightON.SetActive(true);
            flashLightOFF.SetActive(false);
        }
        
        else
        {
            flashLightON.SetActive(false);
            flashLightOFF.SetActive(true);
        }

        if (m4OB.activeInHierarchy)
        {
            m4.SetActive(true);
        }

        else
        {
            m4.SetActive(false);
        }

        if (glockOB.activeInHierarchy)
        {
            glock.SetActive(true);
        }

        else
        {
            glock.SetActive(false);
        }

        if (knifeOB.activeInHierarchy)
        {
            knife.SetActive(true);
        }

        else
        {
            knife.SetActive(false);
        }
    }
}
