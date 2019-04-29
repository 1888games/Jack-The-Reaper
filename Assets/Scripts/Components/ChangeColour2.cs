using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChangeColour2: MonoBehaviour
{


	float currentH = 0;
	public float changeSpeed = 0.004f;
	public Image right;
	public RectTransform rect;
	
    // Start is called before the first frame update
    void Start()
    {
		rect = GetComponent<RectTransform> ();
	
    }

    // Update is called once per frame
    void Update()
    {

		if (rect.localScale.x > 0f) {

			right.color = Color.HSVToRGB (currentH, 0.8f, 1f);

			currentH += changeSpeed;

			if (currentH > 1f) {
				currentH = 0f;
			}

		}
    }
}
