using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class BackGroundSwitch : MonoBehaviour {

	public bool enter_dialogue = false;
	public bool exit_dialogue = false;
	public float speed = 0.6f;
	public bool finale;

	public GameObject focused_toy;

	public int score;
	public string aspect;


	Dictionary <string, SpriteRenderer> backgrounds  = new Dictionary<string, SpriteRenderer>();

	void Awake () {

		GameObject[] backgrounds_array = GameObject.FindGameObjectsWithTag("Background");
		
		foreach ( GameObject t in  backgrounds_array) {

			backgrounds.Add(t.name , t.GetComponent<SpriteRenderer>() );
			
		}
		
		foreach ( KeyValuePair<string, SpriteRenderer> pair in backgrounds ){
			
			//Debug.Log("Pair is " + pair.Key + " " + pair.Value);

			SetTransparency(pair.Value , 0 );
		}

		//set the normal background alpha to 1

		SetTransparency(backgrounds["ToysBackground"], 1);

	}

	void Start () {

		StartCoroutine("CoStart");
	}

	IEnumerator CoStart(){

		while ( true )
			yield return StartCoroutine("CoUpdate");
	}

	IEnumerator CoUpdate(){
		
		if ( enter_dialogue ){
			//We want the Coroutines to run only once...
			enter_dialogue = false;
			StartCoroutine(FadeOut(backgrounds["ToysBackground"]));
			StartCoroutine(FadeIn(backgrounds["ToysBackground_blur"]));
			yield return StartCoroutine(FadeIn(focused_toy.GetComponent<SpriteRenderer>()));
			SetTransparency(backgrounds["ToysBackground"], 0);
			SetTransparency(backgrounds["ToysBackground_blur"], 1);
			SetTransparency(focused_toy.GetComponent<SpriteRenderer>(), 1);


		}
		
		if ( exit_dialogue ) {
			SetTransparency(backgrounds["ToysBackground"], 0);
			//We want the Coroutines to run only once...
			exit_dialogue = false;

			//The Camera can be a little tricky. We "cheat" : We reduce its size and then disable it. 
			//Then we wait for the background to fade in and then we reenable it. 
			Camera.main.GetComponent<CameraController>().enabled = false;
			Camera.main.orthographicSize = 1.5f;
			backgrounds["ToysBackground_end"].sprite = Resources.LoadAll<Sprite>("Sprites/Aspects/" + aspect)[score];


			StartCoroutine(FadeOut(backgrounds["ToysBackground_blur"]));
			yield return StartCoroutine(FadeIn(backgrounds["ToysBackground_end"]));
			//Wait a little zoomed in on the toy...
			yield return new WaitForSeconds(1);
			Camera.main.GetComponent<CameraController>().enabled = true;

			//Wait until the camera zoomed out... And give a little pause too for the image to settle in...
			yield return new WaitForSeconds(8);
			StartCoroutine(FadeOut(backgrounds["ToysBackground_end"]));

			//I usually make one of the Fades in yield return in order to be sure that it will be finished 
			//Before the next one begins
			yield return StartCoroutine(FadeIn(backgrounds["ToysBackground_black"]));

			//Another small pause for the dark picture...
			yield return new WaitForSeconds(2);

			//Here is where it changes if it is finale
			finale = GameObject.FindGameObjectsWithTag("Toys").All(t => !string.IsNullOrEmpty(t.GetComponent<ToyController>().aspect));
	
			if ( !finale ){

				StartCoroutine(FadeOut(backgrounds["ToysBackground_black"]));
				//No yield return here I want the toys to appear naturally. Or else it would
				//first load the picture and the toys would then "pop out"
				StartCoroutine(FadeIn(backgrounds["ToysBackground"]));
				GameObject.Find("ToysBackground").GetComponent<BackgroundController>().ReactivateHiddenToys();
			}

			if ( finale ){
				StartCoroutine(FadeOut(backgrounds["ToysBackground_black"]));
				yield return StartCoroutine(FadeIn(backgrounds["ToysBackground_message"]));
				SetTransparency(backgrounds["ToysBackground_black"], 0);
				SetTransparency(backgrounds["ToysBackground_message"], 1);
				yield return new WaitForSeconds(2);


		

				StartCoroutine(FadeOut(backgrounds["ToysBackground_message"]));
				yield return StartCoroutine(FadeIn(backgrounds["ToysBackground_black"]));
				SetTransparency(backgrounds["ToysBackground_message"], 0);
				SetTransparency(backgrounds["ToysBackground_black"], 1);
				yield return new WaitForSeconds(2);


		
				StartCoroutine(FadeOut(backgrounds["ToysBackground_black"]));
				yield return StartCoroutine(FadeIn(backgrounds["ToysBackground_finale"]));
				SetTransparency(backgrounds["ToysBackground_black"], 0);
				SetTransparency(backgrounds["ToysBackground_finale"], 1);
				yield return new WaitForSeconds(3);



				StartCoroutine(FadeOut(backgrounds["ToysBackground_finale"]));
				yield return StartCoroutine(FadeIn(backgrounds["ToysBackground_black"]));
				SetTransparency(backgrounds["ToysBackground_finale"], 0);
				SetTransparency(backgrounds["ToysBackground_black"], 1);
				yield return new WaitForSeconds(1);
				

				StartCoroutine(FadeOut(backgrounds["ToysBackground_black"]));
				yield return StartCoroutine(FadeIn(backgrounds["ToysBackground_credits"]));	
				SetTransparency(backgrounds["ToysBackground_black"], 0);
				SetTransparency(backgrounds["ToysBackground_credits"], 1);
				yield return new WaitForSeconds(3);


				StartCoroutine(FadeOut(backgrounds["ToysBackground_credits"]));
				yield return StartCoroutine(FadeIn(backgrounds["ToysBackground_credits2"]));
				SetTransparency(backgrounds["ToysBackground_credits"], 0);
				SetTransparency(backgrounds["ToysBackground_credits2"], 1);
				yield return new WaitForSeconds(3);

				StartCoroutine(FadeOut(backgrounds["ToysBackground_credits2"]));
				yield return StartCoroutine(FadeIn(backgrounds["ToysBackground_black"]));
				SetTransparency(backgrounds["ToysBackground_credits2"], 0);
				SetTransparency(backgrounds["ToysBackground_black"], 1);
				yield return new WaitForSeconds(2);



				Application.LoadLevel("MenuScene");
				
			}
		}

		yield return null;
	}



	
	IEnumerator FadeOut(SpriteRenderer fadeout_sprite ){
	
		Color temp_fade_out;

		for ( float f = 1f ; f >= 0f ; f -= speed*Time.deltaTime ) {
			//Debug.Log("to f eiani " + f);
			temp_fade_out = fadeout_sprite.color;
			temp_fade_out.a = f;
			fadeout_sprite.color = temp_fade_out;
			yield return null;
		}
	

	}

	IEnumerator FadeIn(SpriteRenderer fadein_sprite ){

		Color temp_fade_in;

		for ( float t = 0f ; t <= 1f ; t += speed*Time.deltaTime ) {
			//Debug.Log("to t eiani " + t);
			temp_fade_in = fadein_sprite.color;
			temp_fade_in.a = t;
			fadein_sprite.color = temp_fade_in;
			yield return null;
		}
	
		
	}
	
	void SetTransparency(SpriteRenderer renderer , float transparency){

		Color temp;
		temp = renderer.color;
		temp.a = transparency;
		renderer.color = temp;
	
	}
}
