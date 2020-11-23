using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float smoothTime = .15f;

    public float YMaxValue = 0;
    public float YMinValue = 0;
    public float XMaxValue = 0;
    public float XMinValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            CameraController.Instance.smoothTime = smoothTime;
            CameraController.Instance.YMaxValue = YMaxValue;
            CameraController.Instance.YMinValue = YMinValue;
            CameraController.Instance.XMaxValue = XMaxValue;
            CameraController.Instance.XMinValue = XMinValue;
        }
    }
}
