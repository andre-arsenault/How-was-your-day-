using UnityEngine;

public class MenuController : MonoBehaviour
{
    public enum MenuActions
    {
        None = 0,
        Start = 1,
        Credits = 2,
        Exit = 3
    }

    public MenuActions menuAction;

    void OnMouseDown()
    {
        if (menuAction == MenuActions.Start)
            Application.LoadLevel(1);
    }
}