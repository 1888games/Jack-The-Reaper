using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class colourText : MonoBehaviour
{

	TextMeshProUGUI text;
	float currentH = 0;
	public float changeSpeed = 0.004f;
	public Image right;
	public Image left;
	
    // Start is called before the first frame update
    void Start()
    {
		text = GetComponent<TextMeshProUGUI> ();
    }

    // Update is called once per frame
    void Update()
    {

		text.color = Color.HSVToRGB (currentH, 0.8f, 1f);
		right.color = Color.HSVToRGB (currentH, 0.8f, 1f);
		left.color =  Color.HSVToRGB (currentH, 0.8f, 1f);
		
		currentH += changeSpeed;

		if (currentH > 1f) {
			currentH = 0f;
		}
    }
}
