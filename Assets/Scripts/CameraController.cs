﻿using UnityEngine;

public class CameraController : MonoBehaviour
{
    bool initializing = true;

    public float originalSize;
    public float zoomOutSpeed = 0.3f;

    void Awake()
    {
        // Initial setup - Step-out the camera to fit all the width of the screen
        int stepOutCount = 0;
        GameObject background = GameObject.Find("ToysBackground");

        while (camera.WorldToViewportPoint(background.renderer.bounds.extents).x >= 1f && stepOutCount < 10)
        {
            camera.orthographicSize += 0.5f;
            zoomOutSpeed += 0.15f;
            stepOutCount++;
        }

        if (stepOutCount > 0)
            Debug.Log("Camera has been stepped-out " + stepOutCount + "x");

        originalSize = camera.orthographicSize;
        initializing = false;
    }

    void Update()
    {
        if (initializing)
            return;

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