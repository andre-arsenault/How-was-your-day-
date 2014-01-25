using System;
using UnityEngine;

public class ToyController : MonoBehaviour
{
    public Sprite temp;
	public String aspect;
	public int score;

    Sprite[] backgrounds;
    public Sprite[] Backgrounds
    {
        get { return backgrounds; }
        set { backgrounds = value; }
    }

    DialogueInstance dialogue;
    BackgroundController backgroundController;
    GameObject focusedToy;
	BackGroundSwitch bg_switch;



    void Awake()
    {
        backgrounds = Resources.LoadAll<Sprite>("Sprites/" + gameObject.name);
        backgroundController = GameObject.Find("ToysBackground").GetComponent<BackgroundController>();

	
        focusedToy = GameObject.Find("ToyFocused");

		// Inform the backgroundSwitch script on which toy is the focused one.

		bg_switch = GameObject.Find("GameLogic").GetComponent<BackGroundSwitch>();
		bg_switch.focused_toy =  focusedToy;
        dialogue = gameObject.GetComponent<DialogueInstance>();
    }

    void OnMouseDown()
    {


        // Hide the selected toy, and reset the counter
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CursorChanger>().ResetMouse();
        GetComponent<CursorChanger>().enabled = false;

        // Set the focused toy
        focusedToy.GetComponent<SpriteRenderer>().sprite = backgrounds[0];

        // Start the dialogue
        dialogue.startOn = 0;
        dialogue.enabled = true;

		//Make the current focused toy transparent
		Color temp_color = focusedToy.GetComponent<SpriteRenderer>().color;
		temp_color.a = 0;
		focusedToy.GetComponent<SpriteRenderer>().color = temp_color;

		//Inform the Backgroundswitch that we are entering dialogue "mode"
		bg_switch.exit_dialogue = false;
		bg_switch.enter_dialogue = true;

    }

    public void OnDialogueEnd(string aspect)
    {
		this.aspect = aspect;
        // Set no focused toy
        focusedToy.GetComponent<SpriteRenderer>().sprite = null;

        // 0 Neg 1 Pos
		// Use the provided aspect on the HashTable (as its Key) to retrieve the result, and use that to load the relative background sprite
		score = Convert.ToInt32(Convert.ToBoolean(Score.good_endings[aspect]));
		backgroundController.SetBackground(Resources.LoadAll<Sprite>("Sprites/Aspects/" + aspect)[score]);
        //focusedToy.GetComponent<SpriteRenderer>().sprite = temp;

		//Notify the Background switch

		bg_switch.score = score;
		bg_switch.aspect = aspect;
		bg_switch.enter_dialogue = false;
		bg_switch.exit_dialogue = true;

        

    }



}