using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickMover : MonoBehaviour
{

	[SerializeField] private float moveSpeed = 1;
	[SerializeField] private float moveRange = 1;
	private bool _isRight;
	private Transform _transform;

	private Vector3 _startPosition;
	private Vector3 _endPosition;
	
	// Use this for initialization
	void Start () {
		_transform = GetComponent<Transform>();
		_startPosition = _transform.position;
		_endPosition = new Vector2(moveRange+_transform.position.x, _transform.position.y);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!_isRight)
		{
			_transform.position = Vector2.Lerp(_transform.position,
				new Vector2(_endPosition.x, _endPosition.y), Time.deltaTime * moveSpeed);
			if (Vector2.Distance(_transform.position,_endPosition)<=0.05f)
			{
				_isRight = true;
			}
		}
		else
		{
			_transform.position = Vector2.Lerp(_transform.position,
				new Vector2(_startPosition.x, _startPosition.y), Time.deltaTime * moveSpeed);
			if (Vector2.Distance(_transform.position,_startPosition)<=0.05f)
			{
				_isRight = false;
			}
		}
	}
}
