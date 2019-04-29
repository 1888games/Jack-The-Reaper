using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{

	public float spinSpeed = 1f;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

		transform.eulerAngles = new Vector3 (90f, transform.eulerAngles.y + (spinSpeed * Time.deltaTime), 0f);
        
    }
}
