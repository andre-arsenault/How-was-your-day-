using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomOutSpeed = 0.3f;

    public float originalSize;

    void Awake()
    {
        originalSize = camera.orthographicSize;
    }

    void Update()
    {

        if (camera.orthographicSize < 3.45){
			camera.orthographicSize += (zoomOutSpeed * Time.deltaTime);
			Debug.Log("mpika");
		}
           
        else if (camera.orthographicSize > originalSize)
            camera.orthographicSize = originalSize;
    }



    public void ZoomOutFromScene()
    {
        camera.orthographicSize = 1.5f;
    }
}