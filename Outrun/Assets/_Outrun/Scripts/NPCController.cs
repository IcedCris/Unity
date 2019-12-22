using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class NPCController : Car
{
    [Space(10)]
    [Tooltip("the shortest time the car starts to turn.")]
    [Range (0,10)]
    [SerializeField]
    private float _baseTimeToTurn = 10;

    [Tooltip("random time modifier to turn.")]
    [Range(0, 10)]
    [SerializeField]
    private float _randomTimeToTurn = 5;

    private float _time;
    private float _nextTurnTime;
    
    void Start()
    {
        _nextTurnTime = Mathf.Abs(_baseTimeToTurn - Random.Range(0, _randomTimeToTurn));
    }

    void Update()
    {
     

        Vector3 dir = transform.TransformDirection(transform.forward);
        RaycastHit CarHit;

        bool fwd = false;
        bool rgt = false;
        bool lft = false;
        bool bck = false;

        if (Physics.Raycast(transform.position, dir , out CarHit, 10))
        {
            if (CarHit.transform.CompareTag("Car"))
            {
                fwd = true;
       
            }
        }

        dir = transform.TransformDirection(transform.right);
        if (Physics.Raycast(transform.position, dir, out CarHit, 3f))
        {
            if (CarHit.transform.CompareTag("Car") || CarHit.transform.tag.ToLower().Contains("barrier"))
            {
                rgt = true;
     
            }
        }

        dir = transform.TransformDirection(-transform.right);
        if (Physics.Raycast(transform.position, dir, out CarHit, 3f))
        {
            if (CarHit.transform.CompareTag("Car") || CarHit.transform.tag.ToLower().Contains("barrier"))
            {
                lft = true;
    
            }
        }

        dir = transform.TransformDirection(-transform.forward);
        if (Physics.Raycast(transform.position, dir, out CarHit, 10))
        {
            if (CarHit.transform.CompareTag("Car"))
            {
                bck = true;
      
            }
        }

        int randomTurn = Random.Range(0, 2);

        if (randomTurn == 0)
        {
            if ((fwd || bck) && !lft)
            {
                TurnLeft();
            }
            else if ((fwd || bck) && !rgt)
            {
                TurnRight();
            }
        }
        else
        {
            if ((fwd || bck) && !rgt)
            {
                TurnRight();
            }
            else if ((fwd || bck) && !lft)
            {
                TurnLeft();
            }
           
        }


        if (_time > _nextTurnTime)
        {
            int turnDir = Random.Range(0, 2);

            if (turnDir == 0)
            {
                if (!lft)
                {
                    TurnLeft();
                }
                else if (!rgt)
                {
                    TurnRight();
                }
                
            }
            else 
            {
                if (!rgt)
                {
                    TurnRight();
                }
                else if (!lft)
                {
                    TurnLeft();
                }
            }


            _nextTurnTime = Mathf.Abs(_baseTimeToTurn - Random.Range(0, _randomTimeToTurn));
            _time = 0;
        }
        else
        {
            _time += Time.deltaTime;
        }


    }

}
