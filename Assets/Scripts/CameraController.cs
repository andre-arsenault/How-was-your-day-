using UnityEngine;

public class CameraController : MonoBehaviour
{
    float originalSize;

    void Awake()
    {
        originalSize = camera.orthographicSize;
    }

    void Update()
    {
        if (camera.orthographicSize < originalSize)
            camera.orthographicSize += (0.3f * Time.deltaTime);
        else if (camera.orthographicSize > originalSize)
            camera.orthographicSize = originalSize;
    }



    public void FadeOutFromBedScene()
    {
        camera.orthographicSize = 1.5f;
    }
}