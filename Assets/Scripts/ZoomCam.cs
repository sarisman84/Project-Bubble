using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Eliott & Gabriel
public class ZoomCam : MonoBehaviour
{
    [SerializeField]
    Camera cam = null;
    [SerializeField]
    float maxFov = 80, minFov = 31.5f, zoomPS;
    [SerializeField]
    Slider ZoomIcon;

    public void AdjustSlider(float newZoomIcon) //Om man vill ha en slider för FOV slider istället för att den byter mellan FOVS instant
    {
        //ZoomIcon.value = newZoomIcon; 
    }

    void Start()
    {
        
    }


    void LateUpdate()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        if (Input.GetAxis ("Mouse ScrollWheel") > 0)
        {
            cam.fieldOfView = minFov;
            AdjustSlider(maxFov);
            //if (cam.fieldOfView > minFov)
            //{
            //    AdjustSlider(zoomPS * Time.deltaTime);
            //    cam.fieldOfView -= zoomPS * Time.deltaTime;
            //}

            
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 /*&& cam.fieldOfView <= maxFov*/)
        {
            cam.fieldOfView = maxFov;
            AdjustSlider(minFov);
            
            //AdjustSlider(-zoomPS * Time.deltaTime);
            //cam.fieldOfView += zoomPS * Time.deltaTime;
        }
    }
}
