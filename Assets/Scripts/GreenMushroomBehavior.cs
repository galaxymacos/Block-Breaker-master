using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GreenMushroomBehavior : MonoBehaviour
{
    private bool isBig = false;

    private GameManager code;

    // Use this for initialization
    void Start()
    {
        code = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, -2 * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("paddle"))
        {
            code.MakePaddleLonger();
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("death"))
        {
            Destroy(gameObject);
        }
    }
}