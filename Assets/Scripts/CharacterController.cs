using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
	private Rigidbody2D body;
	private Animator animator;
	[SerializeField] private float speed = 4.0f;
	private float horizontal;
	private float vertical;
	

	void Start()
	{
		body = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	void Update()
	{	
		//input
		horizontal = Input.GetAxisRaw("Horizontal");
		vertical = Input.GetAxisRaw("Vertical");
		
		//pass speed to Animator
		float horizontalSpeed = Input.GetAxisRaw("Horizontal") * speed;
		float vericalSpeed = Input.GetAxisRaw("Vertical") * speed;
		
		animator.SetFloat("hSpeed", horizontalSpeed);
		animator.SetFloat("vSpeed", vericalSpeed);
		
		//pass idle state to animator
		if(vericalSpeed == 0f && horizontalSpeed == 0)
		{
			animator.SetBool("isStill", true);
		}
		else
		{
			animator.SetBool("isStill", false);
		}
	}

	private void FixedUpdate()
	{
		if(DialogueSystem.playerControl)
		{
			body.velocity = new Vector2(horizontal * speed, vertical * speed);
		}
		else 
		{
			body.velocity = new Vector2(0, 0);
		}
	}
}