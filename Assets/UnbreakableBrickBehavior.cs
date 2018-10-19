using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnbreakableBrickBehavior : MonoBehaviour {

    private GameManager code;

	// Use this for initialization
	void Start () {
        code = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {

        code.PlayDingSound(gameObject.transform.position.x);
    }
}
