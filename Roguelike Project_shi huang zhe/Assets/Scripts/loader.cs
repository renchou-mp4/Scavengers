using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loader : MonoBehaviour {

	public GameObject gameManager;

	void Awake()
	{
		if (GameManager.Instance==null)
        {
			GameObject.Instantiate(gameManager);
			print(4);
		}
	}

	void Update()
    {

    }
}
