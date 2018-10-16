using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBehaviour : MonoBehaviour
{

	[SerializeField] private GameObject dropItem;
	[SerializeField] private int PowerUPPercentage = 20;
	[SerializeField] private int blockLives = 1;
	private GameObject ball;
	
	private GameManager code;
	// Use this for initialization
	void Start () {
		code = GameObject.Find("GameManager").GetComponent<GameManager>();
		for (int i = 0; i < blockLives; i++)
		{
			code.AddBlock();
		}
		ball = GameObject.Find("ball");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void OnCollisionExit2D(Collision2D other)
	{
		blockLives--;
		float destiny = Random.Range(1, 100);
		if (destiny > 0 && destiny <= PowerUPPercentage)
		{
			Instantiate(dropItem, ball.transform.position,Quaternion.identity);
		}
//		GetComponent<AudioSource>().Play(); TODO 
		code.AddPoints();
		if (blockLives == 0)
		{
			Destroy(gameObject);			
		}
	}

	private void OnTriggerEnter2D(Collider2D other)		// trigger when the ball has fire effect
	{
		float destiny = Random.Range(1, 100);
		if (destiny > 0 && destiny <= PowerUPPercentage)
		{
			Instantiate(dropItem, ball.transform.position,Quaternion.identity);
		}
//		GetComponent<AudioSource>().Play(); TODO 
		for (int i = 0; i < blockLives; i++)
		{
			code.AddPoints();
		}
			Destroy(gameObject);			
	}
}
