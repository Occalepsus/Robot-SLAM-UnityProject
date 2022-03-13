using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TakePicture : MonoBehaviour
{
    private PictureManager picManager;
    public RenderTexture renderTex;

    public KeyCode takeScreenshot;


    public const string rootName = @"\pic_";
    public const string path = @"C:\Users\jujuj\OneDrive\Documents\Perso\Programmes\TIPE1\Pictures";

    private void Start()
    {
        picManager = gameObject.GetComponent<PictureManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(takeScreenshot))
        {
            SavePicture();
        }
    }

    //Take and save a Screenshot
    private void SavePicture()
    {
        DateTime date = DateTime.Now;
        string name = rootName + date.ToShortDateString();

        Texture2D screenShot = new Texture2D(renderTex.width, renderTex.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTex;
        screenShot.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);

        byte[] image = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(path + name + ".png", image);

        picManager.AddPictureName(name);
    }
}
