using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Petal : MonoBehaviour
{
    private float rotSpeed;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(Random.Range(0f,1f));
        rotSpeed = Random.Range(-1.5f,1.5f);
    }

	void Update () 
	{
        transform.Rotate(new Vector3(0, 0, rotSpeed));
	}
}
