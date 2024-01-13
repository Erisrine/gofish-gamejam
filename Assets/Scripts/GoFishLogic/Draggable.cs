using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
	public bool dragging = false;
	private Vector3 offset;
	
	
	void Update()
	{
		if(dragging)
		{
			transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
		}
	}
	
	private void OnMouseDownm()
	{
		offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
		dragging = true;
	}
	private void OnMouseUp()
	{
		dragging = false;
	}
	
}
