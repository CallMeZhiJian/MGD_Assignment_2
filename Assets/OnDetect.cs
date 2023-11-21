using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using UnityEngine.UI;

public class OnDetect : MonoBehaviour
{
    [SerializeField]
    GameObject arCamera;

    [SerializeField]
    ARTrackedImageManager m_TrackedImageManager;

    [SerializeField]
    private GameObject BallPrefab;

    private GameObject BallSpawned;

    private float distance = 0.5f;

    private float startTime, endTime, swipeDistance, swipeTime;
    private Vector2 startPos, endPos;

    private float ballVelocity;
    private float ballSpeed;
    public float maxBallSpeed;
    private Vector3 angle, ballPosition;

    private bool throwing;

    private Rigidbody rb;

    public TextMeshProUGUI DebugText;
    public Slider slider;
    private int count;
    

    // Start is called before the first frame update
    void Start()
    {
        BallSpawned = Instantiate(BallPrefab, Camera.main.transform.position + Vector3.forward * distance, Quaternion.identity);

        BallSpawned.transform.position = arCamera.transform.position + arCamera.transform.forward * distance;

        BallSpawned.transform.SetParent(arCamera.transform);

        BallSpawned.transform.localPosition = Vector3.forward * distance;

        rb = BallSpawned.GetComponent<Rigidbody>();

        ballPosition = BallSpawned.transform.position;

        ResetBall();
    }

    public void ResetBall()
    {
        startPos = Vector2.zero;
        
        endPos = Vector2.zero;
        
        angle = Vector3.zero;
        
        ballSpeed = 0;
        
        startTime = 0;
        
        endTime = 0;
        
        swipeDistance = 0;
        
        swipeTime = 0;
        
        throwing = false;

        rb.velocity = Vector3.zero;
        
        rb.useGravity = false;

        BallSpawned.transform.position = arCamera.transform.position + arCamera.transform.forward * distance;

        BallSpawned.transform.SetParent(arCamera.transform);

        BallSpawned.transform.localPosition = Vector3.forward * distance;

        count++;
        DebugText.text = count.ToString();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (throwing)
            {
                return;
            }

            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTime = Time.time;
                startPos = touch.position;
            }

            else if (touch.phase == TouchPhase.Ended)
            {
                endTime = Time.time;
                endPos = touch.position;
                swipeDistance = (endPos - startPos).magnitude;
                swipeTime = endTime - startTime;

                if (swipeTime < 0.5f && swipeDistance > 30f)
                {
                    CalSpeed();
                    CalAngle();
                    rb.AddForce(new Vector3(angle.x * ballSpeed, angle.y * ballSpeed, angle.z * ballSpeed));
                    rb.useGravity = true;
                    throwing = true;
                }   
            }
        }
        //else
        //{
        //    if (throwing)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        BallSpawned.transform.position = arCamera.transform.position + arCamera.transform.forward * distance;
        //    }  
        //}
    }

    void CalSpeed()
    {
        angle = Camera.main.ScreenToWorldPoint(new Vector3(endPos.x, endPos.y + 50f, Camera.main.nearClipPlane + 5));
    }

    void CalAngle()
    {
        if (swipeTime > 0)
        {
            ballVelocity = swipeDistance / (swipeDistance - swipeTime);
        }

        ballSpeed = ballVelocity * 40f;

        if (ballSpeed >= maxBallSpeed)
        {
            ballSpeed = maxBallSpeed;
        }

        if (ballSpeed <= maxBallSpeed)
        {
            ballSpeed = ballSpeed += 40;
        }

        swipeTime = 0;
    }
    public void UpdateBall()
    {
        distance = slider.value;

        BallSpawned.transform.position = arCamera.transform.position + arCamera.transform.forward * distance;

        BallSpawned.transform.SetParent(arCamera.transform);

        BallSpawned.transform.localPosition = Vector3.forward * distance;
    }
}
