using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    public Texture2D cursorTexture;
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
            Cursor.SetCursor(cursorTexture, cursorHotSpot, cursorMode);
            isActive = true;
        }
    }

    void OnMouseExit()
    {
        ResetMouse();
        isActive = false;
    }

    public void ResetMouse()
    {
        Cursor.SetCursor(null, cursorHotSpot, cursorMode);
    }
}