using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpMain : MonoBehaviour
{
    private GameObject infosMainObject;

    private GameObject infoFirstHelmetObject;
    private GameObject infoFirstMedkit;

    void Start()
    {
        infosMainObject = GameObject.FindGameObjectWithTag("InfosMainTag");
        infoFirstHelmetObject = GameObject.FindGameObjectWithTag("InfoFirstHelmet");
        infoFirstMedkit = GameObject.FindGameObjectWithTag("InfoFirstMedkit");
    }

    public void handleOkButton () {
        infosMainObject.SetActive(false);
        infoFirstHelmetObject.SetActive(false);
        infoFirstMedkit.SetActive(false);
    }
}
