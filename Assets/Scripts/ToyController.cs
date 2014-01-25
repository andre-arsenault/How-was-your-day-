using System;
using UnityEngine;

public class ToyController : MonoBehaviour
{
    Sprite[] backgrounds;

    public Sprite[] Backgrounds
    {
        get { return backgrounds; }
        set { backgrounds = value; }
    }

    DialogueInstance dialogue;
    BackgroundController backgroundController;
    GameObject focusedToy;

    // Fade variables
    Color temp_color;
    bool fade_in = false;
    float fadespeed = 1.1f;

    void Awake()
    {
        backgrounds = Resources.LoadAll<Sprite>("Sprites/" + gameObject.name);
        backgroundController = GameObject.Find("ToysBackground").GetComponent<BackgroundController>();

        focusedToy = GameObject.Find("ToyFocused");

        dialogue = gameObject.GetComponent<DialogueInstance>();
    }

    void OnMouseDown()
    {


        // Hide the selection monkey toy
        GetComponent<SpriteRenderer>().enabled = false;

        // Select the blurred toys background
        backgroundController.SetBackground(1);

        // Set the focused monkey as the focused toy
        focusedToy.GetComponent<SpriteRenderer>().sprite = backgrounds[1];

        temp_color.a = 0;
        //focusedToy.GetComponent<SpriteRenderer>().color = temp_color;
        //fade_in = true;

        // Start the dialogue
        dialogue.startOn = 0;
        dialogue.enabled = true;
    }

    public void OnDialogueEnd(string aspect)
    {
        // Set no focused toy
        focusedToy.GetComponent<SpriteRenderer>().sprite = null;

        // Use the provided aspect on the HashTable (as its Key) to retrieve the result, and use that to load the relative background sprite
        int aspectEnding = Convert.ToInt32(Convert.ToBoolean(Score.good_endings[aspect]));
        backgroundController.SetBackground(Resources.LoadAll<Sprite>("Sprites/Aspects/" + aspect)[aspectEnding]);

        Camera.main.GetComponent<CameraController>().FadeOutFromBedScene();
    }

    void Update()
    {
        if (fade_in)
        {
            temp_color = focusedToy.GetComponent<SpriteRenderer>().color;
            temp_color.a = Mathf.Lerp(temp_color.a, 1, Time.deltaTime * fadespeed);
            Debug.Log(temp_color.a);
            focusedToy.GetComponent<SpriteRenderer>().color = temp_color;

            if (Mathf.Abs(temp_color.a - 1) < 0.05)
            {
                temp_color.a = 1;
                focusedToy.GetComponent<SpriteRenderer>().color = temp_color;
                fade_in = false;

            }
        }
    }
}