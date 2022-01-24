using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawingBoardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    Camera cam;


    float MouseZoomSpeed = 15.0f;
    float TouchZoomSpeed = 0.1f;
    float ZoomMinBound = 0.1f;
    float ZoomMaxBound = 179.9f;



    private void Start()
    {
        cam = Camera.main;

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("begin dragging..");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Now dragging..");

        Vector3 tempPosition = Input.mousePosition;

        tempPosition.z = 15;

        this.transform.position = cam.ScreenToWorldPoint(tempPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("end dragging..");
    }

    void Update()
    {
        if (Input.touchSupported)
        {
            // Pinch to zoom
            if (Input.touchCount == 2)
            {

                // get current touch positions
                Touch tZero = Input.GetTouch(0);
                Touch tOne = Input.GetTouch(1);
                // get touch position from the previous frame
                Vector2 tZeroPrevious = tZero.position - tZero.deltaPosition;
                Vector2 tOnePrevious = tOne.position - tOne.deltaPosition;

                float oldTouchDistance = Vector2.Distance(tZeroPrevious, tOnePrevious);
                float currentTouchDistance = Vector2.Distance(tZero.position, tOne.position);

                // get offset value
                float deltaDistance = oldTouchDistance - currentTouchDistance;
                Zoom(deltaDistance, TouchZoomSpeed);

                if(deltaDistance > 0)
                {
                    deltaDistance = -0.02f;
                }else if(deltaDistance < 0)
                {

                    deltaDistance = 0.02f;
                }

                Vector3 scaleChange = new Vector3(deltaDistance, deltaDistance, deltaDistance);

                if (this.transform.localScale.x <= 4 && this.transform.localScale.x >= -4)
                {

                    this.transform.localScale += scaleChange;
                }


            }
        }
        else
        {

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            Debug.Log("Scrolling: " + scroll);
            Zoom(scroll, MouseZoomSpeed);


            Vector3 scaleChange = new Vector3(scroll, scroll, scroll);

            this.transform.localScale += scaleChange;
        }



        if (cam.fieldOfView < ZoomMinBound)
        {
            cam.fieldOfView = 0.1f;
        }
        else
        if (cam.fieldOfView > ZoomMaxBound)
        {
            cam.fieldOfView = 179.9f;
        }
    }

    void Zoom(float deltaMagnitudeDiff, float speed)
    {

        cam.fieldOfView += deltaMagnitudeDiff * speed;
        // set min and max value of Clamp function upon your requirement
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, ZoomMinBound, ZoomMaxBound);
    }




}
