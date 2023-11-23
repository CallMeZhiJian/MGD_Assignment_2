using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using UnityEngine.UI;

public class GameManager : UIManager
{
    //Ball Spawning related
    [SerializeField]
    GameObject arCamera;
    [SerializeField]
    ARTrackedImageManager m_TrackedImageManager;
    [SerializeField]
    private GameObject BallPrefab;
    private GameObject BallSpawned;
    private float distance = 0.5f;
    private Rigidbody rb;

    //Ball throwing related
    private float startTime, endTime, swipeDistance, swipeTime;
    private Vector2 startPos, endPos;

    private float ballVelocity;
    private float ballSpeed;
    public float maxBallSpeed;
    private Vector3 angle;

    public static bool throwing;

    //UI related
    public Slider slider;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI streakText;
    public TextMeshProUGUI highestStreakText;
    public GameObject findBasketTotuial;
    public GameObject swipeTutorial;

    private int streak;
    private int highestStreak;
    private int point;
    public static bool getPoint;
    private bool isStarted;

    // Start is called before the first frame update
    void Start()
    {
        distance = slider.value;

        point = 0;
        streak = 0;
        highestStreak = 0;
        scoreText.text = "Point: " + point;
        streakText.text = "Streak: " + streak;
        highestStreakText.text = "Highest Streak: " + highestStreak;

        CreateBall();

        ResetBall();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_TrackedImageManager.trackables.count > 0)
        {
            findBasketTotuial.SetActive(false);
            if (!isStarted)
            {
                swipeTutorial.SetActive(true);
                isStarted = true;
            }   
        }

        if (AudioManager.instance._BGMSource.mute)
        {
            BGM_Checkmark.SetActive(false);
        }
        else
        {
            BGM_Checkmark.SetActive(true);
        }

        if (AudioManager.instance._SFXSource.mute)
        {
            SFX_Checkmark.SetActive(false);
        }
        else
        {
            SFX_Checkmark.SetActive(true);
        }

        if (getPoint)
        {
            point++;
            streak++;
            scoreText.text = "Point: " + point;
            streakText.text = "Streak: " + streak;

            Destroy(BallSpawned, 1f);
            Invoke("CreateBall", 1f);

            getPoint = false;
        }

        if(streak >= highestStreak)
        {
            highestStreak = streak;
            highestStreakText.text = "Highest Streak: " + highestStreak;
        }

        if (Input.touchCount > 0)
        {
            if (throwing)
            {
                swipeTutorial.SetActive(false);
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
    }

    public void ResetBall()
    {
        AudioManager.instance.PlaySFX("JiSFX");

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

        streak = 0;
        streakText.text = "Streak: " + streak;
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

    public void CreateBall()
    {
        BallSpawned = Instantiate(BallPrefab, Camera.main.transform.position + new Vector3(0, -1, 1) * distance, Quaternion.identity);

        BallSpawned.transform.position = arCamera.transform.position + arCamera.transform.forward * distance;

        BallSpawned.transform.SetParent(arCamera.transform);

        BallSpawned.transform.localPosition = Vector3.forward * distance;

        rb = BallSpawned.GetComponent<Rigidbody>();

        throwing = false;
    }

    public void UpdateBall()
    {
        distance = slider.value;

        BallSpawned.transform.position = arCamera.transform.position + arCamera.transform.forward * distance;

        BallSpawned.transform.SetParent(arCamera.transform);

        BallSpawned.transform.localPosition = Vector3.forward * distance;
    }
}
