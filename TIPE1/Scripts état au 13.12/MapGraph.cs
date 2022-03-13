using System;
using UnityEngine;
using UnityEngine.Windows;

public class MapGraph : MonoBehaviour
{
    private Mapper mapper;
    private Quadrillage quadrillage;
    public GameObject tileObject;
    private MeshRenderer rend;

    public Color32[] colors;

    public KeyCode update;

    void Start()
    {
        mapper = gameObject.GetComponent<Mapper>();
        quadrillage = mapper.quadrillage;

        rend = tileObject.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(update))
        {
            UpdateMap();
        }
    }

    public void UpdateMap()
    {
        var texture = new Texture2D(mapper.sizeH + 1, mapper.sizeL + 1, TextureFormat.RGB24, false);
        texture.filterMode = FilterMode.Point;

        rend.material.mainTexture = texture;

        for (int h = 0; h < texture.height; h++)
        {
            for (int w = 0; w < texture.width; w++)
            {
                texture.SetPixel(texture.width - w, h, colors[mapper.carte[h, w]]);
            }
        }

        texture.Apply();

        byte[] image = texture.EncodeToPNG();
        File.WriteAllBytes("image.png", image);

        Debug.Log("The map has been updated !");
    }
}