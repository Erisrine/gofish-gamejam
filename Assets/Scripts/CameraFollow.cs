using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] private GameObject player;
	private Transform playerTransform;
	[SerializeField] private bool followPlayer = true;
	private Transform cameraTransform;

	void Start()
	{
		cameraTransform = gameObject.GetComponent<Transform>();
		player = GameObject.FindWithTag("Player");
		if (player != null)
		{
			playerTransform = player.GetComponent<Transform>();
		}
		
	}
	// Update is called once per frame
	void FixedUpdate()
	{
		if(player != null)
		{
			if(followPlayer)
			{
				transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -30f);
			}
		}
	}
}
