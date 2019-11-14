using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSequence : MonoBehaviour
{
    //Kameror
    public GameObject cam1;
    public GameObject cam2;
    public GameObject cam3;
    public GameObject MainCamera;

    void Start()
    {
        StartCoroutine("TheSequence");
    }
    IEnumerator TheSequence()
    {
        yield return new WaitForSeconds(4); //Hur lång tid innan man byter till nästa kamera.
        cam2.SetActive(true);
        cam1.SetActive(false);
        yield return new WaitForSeconds(4);
        cam3.SetActive(true);
        cam2.SetActive(false);
        yield return new WaitForSeconds(4);
        MainCamera.SetActive(true); //Här börjar spelaren spela
        cam3.SetActive(false);
    }
}
