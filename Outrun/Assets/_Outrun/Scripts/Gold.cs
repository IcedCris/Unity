using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    public static int total;
    
    void Start()
    {
        int currentLane = Random.Range(-2, 2);
        transform.position = new Vector3(currentLane * 4, transform.position.y, transform.position.z);
        total = PlayerPrefs.GetInt("Gold");
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + 1);
            total = PlayerPrefs.GetInt("Gold");
            Destroy(gameObject);
        }
    }
}
