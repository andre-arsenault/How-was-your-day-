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

    void Awake()
    {
        backgrounds = Resources.LoadAll<Sprite>("Sprites/" + gameObject.name);

        dialogue = gameObject.GetComponent<DialogueInstance>();
    }

    void OnMouseDown()
    {
        // Hide the selection monkey toy
        GetComponent<SpriteRenderer>().enabled = false;

        // Select the blurred toys background
        GameObject.Find("ToysBackground").GetComponent<ToysBackgroundController>().SetBackground(0);

        // Set the focused monkey as the focused toy
        GameObject.Find("ToyFocused").GetComponent<SpriteRenderer>().sprite = backgrounds[0];

        // Start the dialogue
        dialogue.startOn = 0;
        dialogue.enabled = true;
    }
}