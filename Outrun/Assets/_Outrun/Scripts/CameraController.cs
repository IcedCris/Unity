using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private Transform _player;

    [SerializeField]
    private Vector3 _positionOffset = new Vector3(0,19.5f,-13);

    [SerializeField]
    private Vector3 _rotationOffset = new Vector3(32, 0, 0);

    private PlayerController _ctrlPLayer;
    private Vector3 _originalRotOffset;

    void Start()
    {
        if (!_player)
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        _ctrlPLayer = _player.GetComponent<PlayerController>();
        _originalRotOffset = _rotationOffset;
 
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _player.position + _positionOffset;
        transform.position = new Vector3(transform.position.x, 0.5f + _positionOffset.y, transform.position.z);

        if (_ctrlPLayer.run && _rotationOffset != _originalRotOffset)
        {
            _rotationOffset = Vector3.Lerp(_rotationOffset, _originalRotOffset, Time.deltaTime * 1.2f);
        }
        else if (!_ctrlPLayer.run && _rotationOffset != Vector3.zero)
        {
            _rotationOffset = Vector3.Lerp(_rotationOffset, Vector3.zero, Time.deltaTime * 1.2f);
        }

        transform.LookAt(_player.transform.position + _rotationOffset);
    }
}
