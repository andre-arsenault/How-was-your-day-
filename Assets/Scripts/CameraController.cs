using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomOutSpeed = 0.3f;

    float originalSize;

    void Awake()
    {
        originalSize = camera.orthographicSize;
    }

    void Update()
    {
        if (camera.orthographicSize < originalSize)
            camera.orthographicSize += (zoomOutSpeed * Time.deltaTime);
        else if (camera.orthographicSize > originalSize)
            camera.orthographicSize = originalSize;
    }



    public void ZoomOutFromScene()
    {
        camera.orthographicSize = 1.5f;
    }
}