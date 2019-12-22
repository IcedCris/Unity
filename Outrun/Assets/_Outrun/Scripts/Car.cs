using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animator))]
public abstract class Car : MonoBehaviour
{

    [Tooltip("Speed with which the car moves forward.")]
    [SerializeField]
    [Range(1,100)]
    private float _speed = 50;

    [Tooltip("Speed at which the car changes lanes.")]
    [SerializeField]
    [Range(0.1f, 2)]
    private float _turnSpeed = 1;

    [Space(10)]
    [Tooltip("Appears on a random lane of the road.")]
    [SerializeField]
    private bool _randomLane = true;

    [Tooltip("Moves forward or not")]
    public bool run = true;

    [Range(-2, 2)]
    private int _currentLane = 0;
    private int _lastLane;
    private float _originalSpeed;
    private bool _turning; 
    private Animator _anim;

    [HideInInspector]
    public Rigidbody body;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _originalSpeed = _speed;
        _speed = 0;

        if (_randomLane)
        {
            _currentLane = Random.Range(-2,2);
            transform.position = new Vector3(_currentLane * 4, 0.5f, transform.position.z);
        }

        _anim.speed = _turnSpeed;
    }

    void FixedUpdate()
    {
        if (!run && _speed != 0)
        {
            _speed = Mathf.Lerp(_speed, 0, Time.fixedDeltaTime * 1.5f);
            
        }
        else if (_speed != _originalSpeed)
        {
            _speed = Mathf.Lerp(_speed, _originalSpeed, Time.fixedDeltaTime * 1.5f);
        }

        _anim.enabled = run;

        //body.velocity = new Vector3(0, 0, _speed);
        //transform.Translate(new Vector3(0,0,1) * (Time.deltaTime * _speed));
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (Time.fixedDeltaTime * _speed));

    }

    public void TurnLeft()
    {
        Turn(-1);
    }

    public void TurnRight()
    {
        Turn(1);
    }

    void Turn(int dir)
    {
        if (!_turning && run)
        {
            _currentLane = Mathf.Clamp(_currentLane + dir, -2, 2);

            if (_lastLane != _currentLane)
            {

                if (dir == -1)
                {
                    _anim.SetTrigger("Left");
                }
                else
                {
                    _anim.SetTrigger("Right");
                }

                StartCoroutine(Move());
                _lastLane = _currentLane;
            }
        }
        
    }

    IEnumerator Move()
    {
        _turning = true;
        float time = 0;

        while (Mathf.Abs(transform.position.x - _currentLane * 4) > 0.05f) 
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(_currentLane * 4, 0.5f, transform.position.z), time);
            time += Time.deltaTime * _turnSpeed;
            yield return null;
        }

        transform.position = new Vector3(_currentLane * 4, 0.5f, transform.position.z);

        _turning = false;
        yield break;
    }
    

}
