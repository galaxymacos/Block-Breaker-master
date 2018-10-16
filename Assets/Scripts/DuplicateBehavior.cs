using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class DuplicateBehavior : MonoBehaviour
{
    private GameManager code;
    private GameObject ball;
    [SerializeField] private Transform copiedBall;
    private bool isRunned;

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
        GameObject.Find("GameManager").GetComponent<AudioSource>().Play();
        if (other.gameObject.CompareTag("paddle")&&!isRunned)
        {
            isRunned = true;
            print("create a bew ball");
            var extraBall = Instantiate(ball, ball.transform,true);
            MoveBall moveBall = extraBall.GetComponent<MoveBall>();
            moveBall.initializeDuplicateBall();
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("death"))
        {
            Destroy(gameObject);
        }
    }
}