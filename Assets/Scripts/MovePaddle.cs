using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePaddle : MonoBehaviour
{
    [SerializeField] private Slider slider;

    [SerializeField] private float positionLimit = 2.0f;

    private GameObject ball;
    // Use this for initialization
    void Start()
    {
        ball = GameObject.Find("ball");
       
    }

    // Update is called once per frame
    void Update()
    {
        float pos = slider.value;
        transform.position = new Vector3(pos * positionLimit, transform.position.y, transform.position.z);
    }

    
}