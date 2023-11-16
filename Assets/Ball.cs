using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class Ball : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera;

    [SerializeField]
    private GameObject BallPrefab;

    private ARRaycastManager m_RaycastManager;

    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();

    private void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    private void Update()
    {
        if (Input.touchCount == 0)
            return;

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);

                RaycastHit hitObj;

                if(Physics.Raycast(ray, out hitObj))
                {
                    if (hitObj.collider.CompareTag("Player"))
                    {
                        hitObj.transform.position = touch.position;
                    }
                }
            }
        }
    }
}
