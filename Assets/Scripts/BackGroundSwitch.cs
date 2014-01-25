using UnityEngine;
using System.Collections;

public class BackGroundSwitch : MonoBehaviour {

	SpriteRenderer back_ground_norm;
	SpriteRenderer back_ground_blur;
	SpriteRenderer back_ground_end;
	float fadespeed = 2f;
	Color temp_color;

	public bool enter_dialogue = false;
	public bool exit_dialogue = false;
	public bool bg_loaded = false; 
	public GameObject focused_toy;

	public int score;
	public string aspect;
	public float camera_original;

	bool zoom_out = false;
	

	void Awake () {

		//Get the Sprite Renderers for both backgrounds
			
		back_ground_norm = GameObject.Find("ToysBackground").GetComponent<SpriteRenderer>();
		back_ground_blur = GameObject.Find("ToysBackground_blur").GetComponent<SpriteRenderer>();
		back_ground_end = GameObject.Find("ToysBackground_end").GetComponent<SpriteRenderer>();
		
		//Set the blur backGround to be transparent
		temp_color = back_ground_blur.color;
		temp_color.a = 0;
		back_ground_blur.color = temp_color;
		back_ground_end.color = temp_color;


	}
	// Use this for initialization
	void Start () {
		camera_original = Camera.main.GetComponent<CameraController>().originalSize;
	}
	
	
	void Update()
	{
		
		if (enter_dialogue)
		{
			FadeIn(back_ground_blur);
			FadeOut(back_ground_norm);
			FadeIn(focused_toy.GetComponent<SpriteRenderer>());
		}

		if (exit_dialogue){

			if ( !zoom_out ){
				Camera.main.GetComponent<CameraController>().enabled = false;
				Camera.main.orthographicSize = 1.5f;
				zoom_out = true;
			}

	
			if (!bg_loaded) {
				back_ground_end.sprite = Resources.LoadAll<Sprite>("Sprites/Aspects/" + aspect)[score];
				bg_loaded = true;
			}

			FadeOut(back_ground_blur);
			FadeOut(focused_toy.GetComponent<SpriteRenderer>());
			FadeIn(back_ground_end);

			if ( back_ground_end.color.a > 0.95){

				Camera.main.GetComponent<CameraController>().enabled = true;

			}
		
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

		if ( temp_color.a < 0.05 ) {

			temp_color.a = 0;
			fadeout_sprite.color = temp_color;
			
		}
	}
}
