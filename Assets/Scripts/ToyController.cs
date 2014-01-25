using System;
using UnityEngine;

public class ToyController : MonoBehaviour
{
    #region Fields, Properties and Constructor

    BackgroundController backgroundController;
    BackGroundSwitch bg_switch;
    DialogueInstance dialogue;
    GameObject focusedToy;
    System.Random rotationRandomizer;
    Sprite[] backgrounds;

    public Sprite temp;
    public String aspect;
    public int score;

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

        // Inform the backgroundSwitch script on which toy is the focused one.
        GameObject.Find("GameLogic").GetComponent<BackGroundSwitch>().focused_toy = focusedToy;
        bg_switch = GameObject.Find("GameLogic").GetComponent<BackGroundSwitch>();
        bg_switch.focused_toy = focusedToy;
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

    float rotationAngle;
    bool rotate = false;

    public void OnDialogueChosen()
    {
        rotationAngle = -((float)rotationRandomizer.NextDouble() / 10f);
        rotate = true;
        Debug.Log("Rotation angle:" + rotationAngle);
    }

    void Update()
    {
        if (rotate)
        {
            if (rotationAngle > 0f && focusedToy.transform.rotation.z < rotationAngle)
            {
                rotate = false;
                Debug.LogWarning("STOP");
            }
            else if (rotationAngle > 0f)
                Debug.Log(focusedToy.transform.rotation.z < rotationAngle);
            else if (rotationAngle == 0f)
                Debug.Log("?");

            focusedToy.transform.RotateAround(Vector3.zero, Vector3.back, 1f * Time.deltaTime);

            //Debug.Log("Z ROT: " + focusedToy.transform.rotation.z + " ROT ANGLE: " + rotationAngle);
        }

        //    focusedToy.transform.rotation = new Quaternion(0f, 0f, focusedToy.transform.rotation.z + (rotationAngle * Time.deltaTime), 0f);
        //    Debug.Log("In i go...");
        //}
        //else if (rotate)
        //{
        //    focusedToy.transform.rotation = new Quaternion(0f, 0f, 180f, 0f);
        //    rotate = false;
        //    Debug.Log("Off i go...");
        //}
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