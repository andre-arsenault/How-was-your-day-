using UnityEngine;
using System.Collections;

public class CreditsController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        float pixelRatio = (Camera.main.orthographicSize * 2) / Camera.main.pixelHeight;
        transform.localScale = new Vector3(pixelRatio * 10f, pixelRatio * 10f, pixelRatio * 10f);
    }

    // Update is called once per frame
    void Update()
    {
    }
}