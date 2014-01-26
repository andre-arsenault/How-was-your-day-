using System.Linq;
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

    public void SetBackground(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }



    public void Awake()
    {
        backgrounds = Resources.LoadAll<Sprite>("Sprites/Backgrounds");
    }



    public void ReactivateHiddenToys()
    {
        GameObject[] toys = GameObject.FindGameObjectsWithTag("Toys").Where(t => string.IsNullOrEmpty(t.GetComponent<ToyController>().aspect)).ToArray();

        foreach (GameObject toy in toys)
        {
            toy.GetComponent<CursorChanger>().SetMouse();
            toy.GetComponent<SpriteRenderer>().enabled = true;

            GameObject shadow = GameObject.Find(toy.name.Replace("Toy", "Shadow"));
            if (shadow != null)
                shadow.GetComponent<SpriteRenderer>().enabled = true;

            toy.GetComponent<CursorChanger>().enabled = true;
            toy.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}