using UnityEngine;
using System.Collections;

public class BackGroundSwitch : MonoBehaviour {

	SpriteRenderer back_ground_norm;
	SpriteRenderer back_ground_blur;
	float fadespeed = 2f;
	Color temp_color;

	public bool enter_dialogue = false;
	public GameObject focused_toy;
	

	void Awake () {

		//Get the Sprite Renderers for both backgrounds
			
		back_ground_norm = GameObject.Find("ToysBackground").GetComponent<SpriteRenderer>();
		back_ground_blur = GameObject.Find("ToysBackground_blur").GetComponent<SpriteRenderer>();
		
		//Set the blur backGround to be transparent
		temp_color = back_ground_blur.color;
		temp_color.a = 0;
		back_ground_blur.color = temp_color;

	}
	// Use this for initialization
	void Start () {
	
	}
	
	
	void Update()
	{
		
		if (enter_dialogue)
		{
			FadeIn(back_ground_blur);
			FadeOut(back_ground_norm);
			FadeIn(focused_toy.GetComponent<SpriteRenderer>());
		}

	}
	
	void FadeIn(SpriteRenderer fadein_sprite)
	{
		
		temp_color = fadein_sprite.color;
		temp_color.a = Mathf.Lerp(temp_color.a, 1, Time.deltaTime * fadespeed);
		
		fadein_sprite.color = temp_color;
	}
	
	void FadeOut(SpriteRenderer fadeout_sprite)
	{
		
		temp_color = fadeout_sprite.color;
		temp_color.a = Mathf.Lerp(temp_color.a, 0, Time.deltaTime * fadespeed);
		
		fadeout_sprite.color = temp_color;
	}
}
