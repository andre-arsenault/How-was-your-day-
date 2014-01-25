using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    #region Fields & Constructors

    Sprite[] backgrounds;

    public Sprite[] Backgrounds
    {
        get { return backgrounds; }
        set { backgrounds = value; }
    }

    public BackgroundController()
    {
        backgrounds = new Sprite[1];
    }

    #endregion Fields & Constructors



    public void SetBackground(int index)
    {
        if (index >= 0 && index < backgrounds.Length)
            GetComponent<SpriteRenderer>().sprite = backgrounds[index];
    }

    void Awake()
    {
        backgrounds = Resources.LoadAll<Sprite>("Sprites/Backgrounds");
    }
}