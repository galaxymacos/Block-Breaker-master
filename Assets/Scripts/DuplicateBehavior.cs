using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class DuplicateBehavior : MonoBehaviour
{
    private GameManager code;
    private GameObject ball;
    [SerializeField] private Transform copiedBall;

    // Use this for initialization
    void Start()
    {
        code = GameObject.Find("GameManager").GetComponent<GameManager>();
        ball = GameObject.Find("ball");
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
            var extraBall = Instantiate(ball, ball.transform,true);
            var rig = extraBall.GetComponent<Rigidbody2D>();
            rig.simulated = true;
            rig.transform.parent = null;
//            Instantiate(copiedBall, ball.transform.position, Quaternion.identity);
//            copiedBall.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(150,150));
            rig.AddForce(new Vector2(150, 150));
            print("force added");
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("death"))
        {
            Destroy(gameObject);
        }
    }
}