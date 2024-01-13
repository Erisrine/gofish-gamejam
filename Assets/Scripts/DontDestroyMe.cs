using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMe : MonoBehaviour
{

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		Debug.Log(gameObject.name + " set to persistent.");
	}

}
