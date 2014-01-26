using System;
using UnityEngine;
using System.Linq;

public class ToyController : MonoBehaviour
{
    #region Fields, Properties and Constructor

    bool rotate = false;
    BackgroundController backgroundController;
    BackGroundSwitch bg_switch;
    DialogueInstance dialogue;
    float rotationAngle;
    GameObject focusedToy;
    System.Random rotationRandomizer;
    Sprite[] backgrounds;

    public Sprite temp;
    public String aspect;
    public int score;

    public AudioClip clickAudio;
    public float maxRotationAngle = 0.03f;

    public Sprite[] Backgrounds
    {
        get { return backgrounds; }
        set { backgrounds = value; }
    }



    public ToyController()
    {
        rotationRandomizer = new System.Random();
    }

    #endregion Fields, Properties and Constructor



    void Awake()
    {
        focusedToy = GameObject.Find("ToyFocused");
        backgrounds = Resources.LoadAll<Sprite>("Sprites/" + gameObject.name);

        // Inform the backgroundSwitch script on which toy is the focused one.
        GameObject.Find("GameLogic").GetComponent<BackGroundSwitch>().focused_toy = focusedToy;
        bg_switch = GameObject.Find("GameLogic").GetComponent<BackGroundSwitch>();
        bg_switch.focused_toy = focusedToy;
        dialogue = gameObject.GetComponent<DialogueInstance>();
    }

    void OnMouseDown()
    {
        // Hide the selected toy, and reset the counter
        GameObject[] toys = GameObject.FindGameObjectsWithTag("Toys");

        foreach (GameObject toy in toys)
        {
            toy.GetComponent<CursorChanger>().enabled = false;
            toy.GetComponent<CursorChanger>().ResetMouse();
            toy.GetComponent<SpriteRenderer>().enabled = false;
        }

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

    void Update()
    {
        if (rotate)
        {
            focusedToy.transform.Rotate(new Vector3(0f, 0f, rotationAngle) * (Time.deltaTime * 1000f));

            if (focusedToy.transform.rotation.z < rotationAngle)
                rotate = false;
        }
        else if (focusedToy.transform.rotation.z < 0f)
            focusedToy.transform.Rotate(new Vector3(0f, 0f, -rotationAngle) * (Time.deltaTime * 1000f));
        else if (focusedToy.transform.rotation.z > 0f)
            focusedToy.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
    }



    public void OnDialogueChosen()
    {
        if (clickAudio != null)
            focusedToy.GetComponent<AudioSource>().PlayOneShot(clickAudio);

        rotationAngle = Mathf.Max(-((float)rotationRandomizer.NextDouble() / 10f), -maxRotationAngle);
        rotate = true;
    }

    public void OnDialogueEnd(string aspect)
    {
        this.aspect = aspect;
        // Set no focused toy
        focusedToy.GetComponent<SpriteRenderer>().sprite = null;

        // 0 Neg 1 Pos
        score = Convert.ToInt32(Convert.ToBoolean(Score.good_endings[aspect]));
        //focusedToy.GetComponent<SpriteRenderer>().sprite = temp;

        //Notify the Background switch

        bg_switch.score = score;
        bg_switch.aspect = aspect;
        bg_switch.enter_dialogue = false;
        bg_switch.exit_dialogue = true;
    }
}