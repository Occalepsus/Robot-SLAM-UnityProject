using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera[] cameras;
    public int currentCamera;
    public KeyCode nextKey;

    void Start()
    {
        UpdateEnabled();
    }

    void Update()
    {
        if (Input.GetKeyDown(nextKey))
        {
            currentCamera = (currentCamera + 1) % cameras.Length;
            UpdateEnabled();
        }
    }

    void UpdateEnabled()
    {
        for (int i = 0; i < cameras.Length; ++i)
        {
            cameras[i].enabled = (i == currentCamera);
        }
    }
}

// @ Jesse Anders sur https://forum.unity.com/threads/how-do-i-choose-camera-to-play-from.81695/ (modifié)
