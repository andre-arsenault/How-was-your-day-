using System.Collections;
using System.Linq;
using UnityEngine;

/*
 * This is where we make the background switches.
 * 
 * There are 4 GameObjects with the different backgrounds. These are 
 * 
 * -Normal
 * -Blur
 * -Ending
 * -Black
 * 
 *  All the backgrounds are standard except the ending we arrange it dynamicaly when the dialogue ends. 
 * 
 *  There are practically 3 "states" for this script.
 * 
 *  -Enter Dialogue
 *  -Exit Dialogue 
 *  -End
 *  
 *  There are many boolean variables, but I wanted to make the Update as light as possible. 
 * 
 */
public class BackGroundSwitch : MonoBehaviour
{

    SpriteRenderer back_ground_norm;
    SpriteRenderer back_ground_blur;
    SpriteRenderer back_ground_end;
    SpriteRenderer back_ground_black;
    SpriteRenderer back_ground_message;
    SpriteRenderer back_ground_finale;
    SpriteRenderer back_ground_credits;
    SpriteRenderer back_ground_credits2;

    float fadespeed = 2f;
    Color temp_color;

    public bool enter_dialogue = false;
    public bool exit_dialogue = false;
    bool bg_loaded = false;
    public GameObject focused_toy;

    public int score;
    public string aspect;
    public float camera_original;

    bool end;
    bool wait = true;
    bool wait_again = true;
    bool show_credits_first = true;


    public bool finale;
    bool message_wait = true;
    bool finale_Wait = true;
    bool message_first_time = true;

    bool stop_coroutine = false;

    bool zoom_out = false;
    float time;
    public float delay_end = 3f;
    public float delay_black = 5f;
    public float delay_black_for_message = 2f;
    public float keep_message = 5f;
    public float keep_finale = 6f;
    public bool show_credits_second = true;

    void Awake()
    {

        //Get the Sprite Renderers for both backgrounds

        back_ground_norm = GameObject.Find("ToysBackground").GetComponent<SpriteRenderer>();
        back_ground_blur = GameObject.Find("ToysBackground_blur").GetComponent<SpriteRenderer>();
        back_ground_end = GameObject.Find("ToysBackground_end").GetComponent<SpriteRenderer>();
        back_ground_black = GameObject.Find("ToysBackground_black").GetComponent<SpriteRenderer>();
        back_ground_message = GameObject.Find("ToysBackground_message").GetComponent<SpriteRenderer>();
        back_ground_finale = GameObject.Find("ToysBackground_finale").GetComponent<SpriteRenderer>();
        back_ground_credits = GameObject.Find("ToysBackground_credits").GetComponent<SpriteRenderer>();
        back_ground_credits2 = GameObject.Find("ToysBackground_credits2").GetComponent<SpriteRenderer>();

        //Set all the other backGrounds to be transparent
        temp_color = back_ground_blur.color;
        temp_color.a = 0;
        back_ground_blur.color = temp_color;
        back_ground_end.color = temp_color;
        back_ground_black.color = temp_color;
        back_ground_message.color = temp_color;
        back_ground_finale.color = temp_color;
        back_ground_credits.color = temp_color;
        back_ground_credits2.color = temp_color;

    }

    // Use this for initialization
    void Start()
    {
        camera_original = Camera.main.GetComponent<CameraController>().originalSize;
    }


    void Update()
    {
        finale = GameObject.FindGameObjectsWithTag("Toys").All(t => !string.IsNullOrEmpty(t.GetComponent<ToyController>().aspect));

        //The simpler state we go from Normal -> Blur background. We also make the corresponding animal appear
        //We get the information about the focused animal from the ToyController.cs script.
        if (enter_dialogue)
        {
            FadeIn(back_ground_blur);
            FadeOut(back_ground_norm);
            FadeIn(focused_toy.GetComponent<SpriteRenderer>());
        }
        //Exit the dialogue.

        if (exit_dialogue)
        {
            /* The CameraController.cs script can be a little tricky. It detects whether the camera is off position
             * and slowly brings it back. We zoom the camera in and then we deactivate the screen because 
             * firstly we want the background to change from Blur -> Ending and then we reactivate it (so that the zoom out may commence). 
             * 
             */
            if (!zoom_out)
            {
                Camera.main.GetComponent<CameraController>().enabled = false;
                Camera.main.orthographicSize = 1.5f;
                zoom_out = true;
            }

            //We load the appropriate ending background. All the booleans are there to make the Update() lighter.

            if (!bg_loaded)
            {
                back_ground_end.sprite = Resources.LoadAll<Sprite>("Sprites/Aspects/" + aspect)[score];
                bg_loaded = true;
            }

            //We move from Blur -> Ending 
            FadeOut(back_ground_blur);
            FadeOut(focused_toy.GetComponent<SpriteRenderer>());
            FadeIn(back_ground_end);

            //When the fading is complete we reactivate the camera in order for the zoom out to commence. We also activate the "end" state.
            if (back_ground_end.color.a > 0.95)
            {

                Camera.main.GetComponent<CameraController>().enabled = true;
                if (Camera.main.orthographicSize > 3.4f)
                {
                    end = true;
                    exit_dialogue = false;
                }
            }
        }

        /*
         *  Here is the end state. We have a couroutine so that there is a small pause sothat the player can look the ending scene. Then
         *  we fade to normal and we reset the states.
         */
        if (end)
        {
            if (!finale)
            {
                StartCoroutine("Start_Ending");
                if (back_ground_black.color.a == 1)
                {
                    Screen.showCursor = false;
                    Screen.lockCursor = true;

                    StartCoroutine("End_Ending");
                    if (back_ground_norm.color.a == 1)
                    {
                        //Reset everything
                        end = false;
                        wait = true;
                        enter_dialogue = false;
                        exit_dialogue = false;
                        zoom_out = false;
                        bg_loaded = false;

                        temp_color.a = 0;
                        back_ground_blur.color = temp_color;
                        back_ground_end.color = temp_color;
                        back_ground_black.color = temp_color;

                        Screen.lockCursor = false;
                        Screen.showCursor = true;
                    }
                }
            }



            if (finale)
            {
                //Debug.Log("Hello");
                StartCoroutine("End_Ending_for_message");

                if (back_ground_message.color.a == 1)
                {
                    stop_coroutine = true;
                    StartCoroutine("Keep_Message");
                }

                if (back_ground_finale.color.a == 1)
                {
                    //end = false;
                    StartCoroutine("ShowCredits");
                }
            }
        }
    }



    void FadeIn(SpriteRenderer fadein_sprite)
    {
        temp_color = fadein_sprite.color;
        temp_color.a = Mathf.Lerp(temp_color.a, 1, Time.deltaTime * fadespeed);

        fadein_sprite.color = temp_color;

        if (temp_color.a > 0.95)
        {
            temp_color.a = 1;
            fadein_sprite.color = temp_color;
        }
    }

    void FadeOut(SpriteRenderer fadeout_sprite)
    {
        temp_color = fadeout_sprite.color;
        temp_color.a = Mathf.Lerp(temp_color.a, 0, Time.deltaTime * fadespeed);

        fadeout_sprite.color = temp_color;

        if (temp_color.a < 0.05)
        {

            temp_color.a = 0;
            fadeout_sprite.color = temp_color;
        }
    }

    IEnumerator Start_Ending()
    {
        if (wait)
        {
            yield return new WaitForSeconds(delay_end);
            wait = false;
        }
        FadeIn(back_ground_black);
        FadeOut(back_ground_end);

        yield return null;
    }

    IEnumerator End_Ending()
    {
        if (wait_again)
        {
            yield return new WaitForSeconds(delay_black);
            wait_again = false;
        }

        if (back_ground_norm.color.a > 0.5f)
            GameObject.Find("ToysBackground").GetComponent<BackgroundController>().ReactivateHiddenToys();

        FadeOut(back_ground_black);
        FadeIn(back_ground_norm);

        yield return null;
    }

    IEnumerator End_Ending_for_message()
    {
        if (!stop_coroutine)
        {
            if (message_wait)
            {
                yield return new WaitForSeconds(delay_black_for_message);
                message_wait = false;
            }

            FadeOut(back_ground_end);
            FadeIn(back_ground_message);
        }

        yield return null;
    }

    IEnumerator Keep_Message()
    {

        yield return new WaitForSeconds(4);
        temp_color.a = 0;
        back_ground_blur.color = temp_color;
        back_ground_end.color = temp_color;
        back_ground_black.color = temp_color;
        back_ground_message.color = temp_color;

        back_ground_credits.color = temp_color;
        back_ground_credits2.color = temp_color;
        temp_color.a = 1;
        back_ground_finale.color = temp_color;


        /*
        if (message_first_time)
        {
            yield return new WaitForSeconds(keep_message);
            message_first_time = false;
        }

        /*
        if ( back_ground_finale.color.a == 1 ) {
            yield return null;
			
        }

        FadeOut(back_ground_message);
        FadeIn(back_ground_finale);

        yield return null;
        */
    }

    IEnumerator ShowCredits()
    {
        if (show_credits_first)
        {
            yield return new WaitForSeconds(keep_finale);
            show_credits_first = false;
            gameObject.audio.Stop();
        }


        FadeOut(back_ground_finale);
        FadeIn(back_ground_credits);


        if (back_ground_credits.color.a == 1)
        {

            StartCoroutine("ShowCredits2");
        }

        yield return null;

    }

    IEnumerator ShowCredits2()
    {
        yield return new WaitForSeconds(4);
        temp_color.a = 0;
        back_ground_blur.color = temp_color;
        back_ground_end.color = temp_color;
        back_ground_black.color = temp_color;
        back_ground_message.color = temp_color;
        back_ground_finale.color = temp_color;
        back_ground_credits.color = temp_color;

        temp_color.a = 1;
        back_ground_credits2.color = temp_color;

        yield return new WaitForSeconds(4);
        Application.LoadLevel("MenuScene");

        /*
        if (show_credits_second)
        {
            yield return new WaitForSeconds(4);
            show_credits_second = false;
        }

        FadeOut(back_ground_credits);
        FadeIn(back_ground_credits2);

        if (back_ground_credits2.color.a == 1)
        {
            yield return new WaitForSeconds(4);
            Application.LoadLevel("MenuScene");

        }

        yield return null;*/

    }
}