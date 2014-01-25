using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 cursorHotSpot = Vector2.zero;

    /// <summary>
    /// This boolean value will represent whether the mouse is hovering over the
    /// object collider.
    /// </summary>
    bool isActive;

    DialogueInstance dialogue;

    void Awake()
    {
        Screen.showCursor = true;
        isActive = false;

        dialogue = gameObject.GetComponent<DialogueInstance>();
        //Debug.Log(dialogue.GetType().ToString());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isActive)
        {
            /* startOn is the node we want to initiate the dialogue it will be different for every object
             * It's propably to best to have different DialogueInstance.js for different toys instead of having one
             * huge dialogue Tree
             * 
             * if ( this.name.Equals("Test_toy"))
             * dialogue.startOn = 0;
             * 
             * dialogue.enabled = true;
             */

            Debug.Log("I have been clicked on...");
        }
    }

    void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, cursorHotSpot, cursorMode);
        isActive = true;
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, cursorHotSpot, cursorMode);
        isActive = false;
    }
}