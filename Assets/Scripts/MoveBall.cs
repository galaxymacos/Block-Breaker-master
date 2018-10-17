using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;

public class MoveBall : MonoBehaviour
{
	private GameManager code;
	[SerializeField] private float dir = 150.0f;

	private Rigidbody2D ball;

	private bool onlyOnce;

	private Transform myParent;

	private Vector3 initPos;

	[SerializeField] private float paddleDirBlindSpot = 1.5f;

	[SerializeField] private float vForceMin = 0.6f;

	[SerializeField] private float vForceMultiplier = 2f;

	public bool isDupBall;
	
	// Use this for initialization
	void Start ()
	{
		print("new ball is initialized");
		code = GameObject.Find("GameManager").GetComponent<GameManager>();
		ball = GetComponent<Rigidbody2D>();
		ball.simulated = false;
		initPos = transform.localPosition;	// retrieve the coordinate relative to parent
		myParent = transform.parent;
	}

	public void Init()
	{
		transform.parent = myParent;
		transform.localPosition = initPos;
		ball.simulated = false;
		ball.velocity = new Vector2(0,0);
		onlyOnce = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if ((Input.GetButtonUp("Jump")||isDupBall)&&!onlyOnce)
		{
			onlyOnce = true;
			ball.simulated = true;
			ball.transform.parent = null;
			ball.AddForce(new Vector2(dir, dir));
		}
	}


	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("death"))
		{
			if (GameObject.FindGameObjectsWithTag("ball").Length > 1)
			{
				Destroy(gameObject);
			}
			else
			{
				code.Death();				
			}
		}
		
		if (other.gameObject.CompareTag("paddle"))
		{
			if (code.isFireBall)
			{
//				EditorApplication.isPaused = true;
				code.ConsumeFireBall();
			}

			Time.timeScale *= 1.005f;	// increase difficulty
										// TODO slow down paddle speed
			float diffX = transform.position.x - other.transform.position.x;
			if (diffX > paddleDirBlindSpot)
			{
				// Right side of the paddle
				ball.velocity = new Vector2(0, 0);
				ball.AddForce(new Vector2(dir,dir));
			}

			if (diffX < paddleDirBlindSpot)
			{
				// Left side of the paddle
				ball.velocity = new Vector2(0, 0);
				ball.AddForce(new Vector2(-dir,dir));
			}
		}
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		if (Mathf.Abs(ball.velocity.y)<vForceMin)
		{
			float velX = ball.velocity.x;
			if (ball.velocity.y < 0)
			{
				ball.velocity = new Vector2(velX,-vForceMin*vForceMultiplier);
			}
			else
			{
				ball.velocity = new Vector2(velX,vForceMin*vForceMultiplier);
			}
		}
	}

}
