using System.Collections;
using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    public Texture2D cursorTexture, cursorSelectionTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 cursorHotSpot = Vector2.zero;
    public float scaleIncrease = 0.5f;
    public float scaleSpeed = 1f;

    /// <summary>
    /// This boolean value will represent whether the mouse is hovering over the
    /// object collider.
    /// </summary>
    bool isActive;

    Vector3 originalSize;

    void Awake()
    {
        Screen.showCursor = true;
        isActive = false;

        originalSize = transform.localScale;

        /*
         * The setting of the initial cursor is being set as a Co-Routine as a 'hack'
         * to get the mouse to actually change its cursor.
         */
        StartCoroutine(SetInitialCursor());
    }

    void Update()
    {
        if (isActive)
        {
            if (transform.localScale.x < (originalSize.x + scaleIncrease))
                transform.localScale = new Vector3(transform.localScale.x + (scaleIncrease * Time.deltaTime * scaleSpeed), transform.localScale.y + (scaleIncrease * Time.deltaTime * scaleSpeed), transform.localScale.z);
        }
        else if (originalSize.x < transform.localScale.x)
            transform.localScale = new Vector3(transform.localScale.x - (scaleIncrease * Time.deltaTime * scaleSpeed), transform.localScale.y - (scaleIncrease * Time.deltaTime * scaleSpeed), transform.localScale.z);
    }

    void OnMouseEnter()
    {
        if (this.enabled)
        {
            Cursor.SetCursor(cursorSelectionTexture, cursorHotSpot, cursorMode);
            isActive = true;
        }
    }

    void OnMouseExit()
    {
        if (this.enabled)
        {
            Cursor.SetCursor(cursorTexture, cursorHotSpot, cursorMode);
            isActive = false;
        }
    }



    public void ResetMouse()
    {
        isActive = false;
        Cursor.SetCursor(null, cursorHotSpot, cursorMode);
    }

    /// <summary>
    /// This is only meant to be called as a Co-Routine the first time the application
    /// is run.
    /// </summary>
    private IEnumerator SetInitialCursor()
    {
        yield return null;
        OnMouseExit();
    }
}