using UnityEngine;
using System;
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

    void Awake()
    {
        backgrounds = Resources.LoadAll<Sprite>("Sprites/" + gameObject.name);
        backgroundController = GameObject.Find("ToysBackground").GetComponent<BackgroundController>();
        backgroundController.SetBackground(0);

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
    }
}