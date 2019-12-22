using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    [Tooltip("The car of the player")]
    [SerializeField]
    private Transform _player;

    [Tooltip("The new roads will be children of this transform.")]
    [SerializeField]
    private Transform _RoadContainer;

    [Space(10)]
    [Header("Road settings")]

    [Tooltip("First road must be a simple one.")]
    [SerializeField]
    private GameObject[] _RoadTypes;

    [Space(10)]

    [Tooltip("Distance")]
    [SerializeField]
    [Range(1,120)]
    private int _distOfNewRoad = 40;

    private int _newRoadPosition = -20;


    void Awake()
    {
        for (int i = 0; i < 25; i++)
        {
            CreateNewRoad();
        }

        if (!_player)
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (_player)
        {
            float distance = _newRoadPosition - (_distOfNewRoad * 20);
            if (_player.position.z > distance)
            {
                CreateNewRoad();
            }
        }
        else
        {
            Debug.LogWarning("ROAD CONTROLLER: _Player transform not found.");
        }
        
    }

    private void CreateNewRoad()
    {
        if (_RoadTypes.Length < 1)
        {
            Debug.LogWarning("ROAD CONTROLLER: no road types, must be at least one.");
            return;
        }

        int r;
        if (_newRoadPosition < 60)
        {
            r = 0;
        }
        else
        {
            r = Random.Range(0, _RoadTypes.Length);
        }

        if (!_RoadTypes[r])
        {
            Debug.LogWarning("ROAD CONTRLLER/RoadType[" + r + "]: object not set to an instance of an object.");
            return;
        }

        GameObject newRoad = Instantiate(_RoadTypes[r], new Vector3(0,0,_newRoadPosition), Quaternion.identity) as GameObject;
        if (_RoadContainer)
        {
            newRoad.transform.SetParent(_RoadContainer);
        }
        
        Destroy(newRoad, 50);
        _newRoadPosition += 20; 

    }
}
