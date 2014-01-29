using System;
using System.Linq;
using UnityEngine;

public class ToyController : MonoBehaviour
{
    #region Fields, Properties and Constructor

    #region Toy Rotation Variables

    public enum RotationStates
    {
        None = 0,
        Forward = 1,
        Backward = 2,
        Rewinding = 4,
        Stopping = 8
    }

    RotationStates rotationState = RotationStates.None;
    float rotationAngle;

    [SerializeField]
    float maxRotationAngle = 0.03f;

    public float MaxRotationAngle
    {
        get { return maxRotationAngle; }
        set
        {
            if (value >= 0.001f && value <= 0.1f)
                maxRotationAngle = value;
        }
    }

    #endregion Toy Rotation Variables

    BackgroundController backgroundController;
    BackGroundSwitch bg_switch;
    DialogueInstance dialogue;
    GameObject focusedToy;
    Sprite[] backgrounds;

    public Sprite temp;
    public String aspect;
    public int score;

    public AudioClip clickAudio;

    public Sprite[] Backgrounds
    {
        get { return backgrounds; }
        set { backgrounds = value; }
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
        GameObject[] toys = GameObject.FindGameObjectsWithTag("Toys").Where(t => string.IsNullOrEmpty(t.GetComponent<ToyController>().aspect)).ToArray();

        foreach (GameObject toy in toys)
        {
            toy.GetComponent<CursorChanger>().enabled = false;
            toy.GetComponent<CursorChanger>().ResetMouse();
            toy.GetComponent<SpriteRenderer>().enabled = false;
            toy.GetComponent<BoxCollider2D>().enabled = false;

            GameObject shadow = GameObject.Find(toy.name.Replace("Toy", "Shadow"));
            if (shadow != null)
                shadow.GetComponent<SpriteRenderer>().enabled = false;
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
        //bg_switch.exit_dialogue = false;
        bg_switch.enter_dialogue = true;
    }

    void Update()
    {
        if (rotationState == RotationStates.Forward || rotationState == RotationStates.Rewinding)
        {
            focusedToy.transform.Rotate(Vector3.back, Time.deltaTime * 30f);
            //Debug.Log(">> " + focusedToy.transform.rotation.z + " < " + -rotationAngle + " : " + (focusedToy.transform.rotation.z < rotationAngle));

            if (rotationState == RotationStates.Forward)
            {
                if (focusedToy.transform.rotation.z < -rotationAngle)
                    rotationState = RotationStates.Backward;
            }
            else if (rotationState == RotationStates.Rewinding)
            {
                if (focusedToy.transform.rotation.z < 0f)
                    rotationState = RotationStates.Stopping;
            }
        }
        else if (rotationState == RotationStates.Backward)
        {
            focusedToy.transform.Rotate(Vector3.forward, Time.deltaTime * 30f);
            //Debug.Log("<< " + focusedToy.transform.rotation.z + " > " + (rotationAngle * 2) + " : " + (focusedToy.transform.rotation.z > (rotationAngle * 2)));

            if (focusedToy.transform.rotation.z > (rotationAngle * 2))
                rotationState = RotationStates.Rewinding;
        }
        else if (rotationState == RotationStates.Stopping)
        {
            rotationState = RotationStates.None;

            focusedToy.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
    }



    public void OnDialogueChosen()
    {
        if (clickAudio != null)
            focusedToy.GetComponent<AudioSource>().PlayOneShot(clickAudio);

        rotationAngle = Mathf.Max(Mathf.Min(UnityEngine.Random.value / 20f, maxRotationAngle), 0.01f);
        rotationState = RotationStates.Forward;
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
        //bg_switch.enter_dialogue = false;
        bg_switch.exit_dialogue = true;
    }
}