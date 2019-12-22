using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Car
{
    [SerializeField]
    private int reboundForce = 10;

    [HideInInspector]
    public bool dead;



    private Vector2 _fingerDownPosition;
    private Vector2 _fingerUpPosition;

    private void Start()
    {
        gameObject.tag = "Player";
    }

    void Update()
    {

        if(transform.position.z < 10 || !run)
        {
            return;
        }

        //KEYBOARD:
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            TurnLeft();
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            TurnRight();
        }

        //TOUCH:
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                _fingerUpPosition = touch.position;
                _fingerDownPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                _fingerDownPosition = touch.position;
                DetectSwipeDirection();
            }

            if (touch.phase == TouchPhase.Ended)
            {
                _fingerDownPosition = touch.position;
                DetectSwipeDirection();
            }
        }
        
    }

    private void DetectSwipeDirection()
    {
        var direction = _fingerDownPosition.x - _fingerUpPosition.x;

        if (direction > 0.1f)
        {
            TurnRight();
        }
        else if(direction < -0.1f)
        {
            TurnLeft();
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            if (!dead)
            {
                if (PlayerPrefs.GetInt("BestDistance") < Mathf.RoundToInt(transform.position.z))
                {
                    PlayerPrefs.SetInt("BestDistance", Mathf.RoundToInt(transform.position.z));
                }
            }

            dead = true;
            run = false;
            body.AddForce(0, reboundForce, 0, ForceMode.Impulse);
            Vector3 torque = new Vector3(Random.Range(0, reboundForce), Random.Range(0, reboundForce), Random.Range(0, reboundForce));

            body.AddTorque(torque * reboundForce, ForceMode.Impulse);
           
           
        }
       
    }
}
