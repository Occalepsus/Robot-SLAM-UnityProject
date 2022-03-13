using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GraphDisplay : MonoBehaviour
{
    private OccupancyMap occupancyMap;
    public PingManager pingManager;

    public RawImage oMapDisp;
    public RawImage dMapDisp;
    public RawImage cMapDisp;

    public Color32[] colors;

    public KeyCode update;
    public KeyCode coucheSup;
    public KeyCode coucheInf;
    public KeyCode agrOMap;
    public KeyCode agrDMap;

    public Vector2 oMapOffsetMax;
    public Vector2 oMapOffsetMin;
    public Vector3 oMapSideScale;

    public Vector3 oMapCenterPos;
    public Vector3 oMapCenterScale;

    public Vector2 dMapOffsetMax;
    public Vector2 dMapOffsetMin;
    public Vector3 dMapSideScale;

    public Vector3 dMapCenterPos;
    public Vector3 dMapCenterScale;

    private Texture2D[] graph;
    private int nbCouches;
    private int layer = -1;

    public int nbPerFrame;

    private Texture2D depthMap;

    public float colorDispTime;


    void Start()
    {
        occupancyMap = gameObject.GetComponent<OccupancyMap>();

        nbCouches = occupancyMap.size.y;

        graph = new Texture2D[nbCouches];

        for (int i = 0; i < nbCouches; i++)
        {
            graph[i] = new Texture2D(occupancyMap.size.z + 1, occupancyMap.size.x + 1, TextureFormat.RGB24, false);
        }

        ChangeLayer(true);
    }

    private void Update()
    {

        if (Input.GetKeyDown(update))
        {
            StartCoroutine(UpdateOccupationMap());
        }

        if (Input.GetKeyDown(coucheSup))
        {
            ChangeLayer(true);
        }
        else if (Input.GetKeyDown(coucheInf))
        {
            ChangeLayer(false);
        }

        if (Input.GetKeyDown(agrOMap))
        {
            EnlargeImage(oMapDisp, oMapCenterPos, oMapCenterScale);
        }
        else if (Input.GetKeyUp(agrOMap))
        {
            ReduceImage(oMapDisp, oMapOffsetMax, oMapOffsetMin, oMapSideScale);
        }

        if (Input.GetKeyDown(agrDMap))
        {
            EnlargeImage(dMapDisp, dMapCenterPos, dMapCenterScale);
        }
        else if (Input.GetKeyUp(agrDMap))
        {
            ReduceImage(dMapDisp, dMapOffsetMax, dMapOffsetMin, dMapSideScale);
        }
    }

    //Lit la carte de OccupancyMap pour la transposer dans la texture
    //On utilise une Coroutine pour éviter de faire lagger la simulation
    public IEnumerator UpdateOccupationMap()
    {
        Debug.Log("called");
        for (int couche = 0; couche < nbCouches; couche++)
        {
            Texture2D texture = graph[couche];
            texture.filterMode = FilterMode.Point;

            for (int z = 0; z < texture.height; z++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    texture.SetPixel(x, texture.height - z, colors[(int)occupancyMap.carte[z, x, couche]]);

                    //Tous les nbPerFrame points calculés, on change de frame pour éviter que la simulation ne ralentisse
                    if (((z + 1) * (x + 1)) % nbPerFrame == 0)
                    {
                        yield return null;
                        Debug.Log($"{x},{z}");
                    }
                }
            }

            texture.Apply(false);

            byte[] image = texture.EncodeToPNG();
            System.IO.File.WriteAllBytes($"couche {couche}.png", image);

            Debug.Log($"layer {couche} has been updated !");
        }
        Debug.Log("The map has been updated !");
        yield return null;
    }

    private void ChangeLayer(bool monter)
    {
        layer = monter ? layer + 1 : layer + nbCouches - 1;
        layer %= nbCouches;

        graph[layer].Apply();
        oMapDisp.texture = graph[layer];

        Debug.Log($"displayed layer : {layer}");
    }

    private void EnlargeImage(RawImage image, Vector3 pos, Vector3 scale)
    {
        image.transform.localPosition = pos;
        image.transform.localScale = scale;
    }

    private void ReduceImage(RawImage image, Vector2 offsetMax, Vector2 offsetMin, Vector3 scale)
    {
        image.transform.localScale = scale;
        image.rectTransform.offsetMax = offsetMax;
        image.rectTransform.offsetMin = offsetMin;
    }


    public void UpdateDepthMap(float[,] depthTable, float viewDistance)
    {
        depthMap = Lidar.EncodeDepthMap(depthTable, viewDistance);
        DisplayDepthMap(depthMap);
    }

    private void DisplayDepthMap(Texture2D heightMap)
    {
        dMapDisp.texture = heightMap;
    }

    public IEnumerator DispColorMap(Texture2D colorMap)
    {
        cMapDisp.texture = colorMap;
        cMapDisp.enabled = true;

        yield return new WaitForSeconds(colorDispTime);

        cMapDisp.enabled = false;
    }
}