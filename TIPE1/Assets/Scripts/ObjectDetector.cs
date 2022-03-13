using System.Collections.Generic;
using UnityEngine;

public enum Shape
{
    Nsp,
    Cube,
    Rectangle,
    Boule,
    Cylindre,
}

public struct Obstacle
{
    public Vector2Int centerPosition;
    public float size;
    public Shape forme;

    public Obstacle(Vector2Int position, float size)
    {
        this.centerPosition = position;
        this.size = size;
        this.forme = Shape.Nsp;
    }

    public Obstacle(Vector2Int position, float size, Shape forme)
    {
        this.centerPosition = position;
        this.size = size;
        this.forme = forme;
    }
}


public class ObjectDetector : MonoBehaviour
{
    public PingManager pingManager;
    public GraphDisplay graphDisplay;

    public Vector2Int depthMapSize;
    public float viewDistance;
    public float hAngleRange;
    public float vAngleRange;
    public float hOffset;

    public Bipoint[,] Data;
    public float[,] depthTable;
    public List<Obstacle> obstacles;

    public bool render;

    public Color[] colors;
    public float deltaDist;
    public KeyCode takeColorMap;
    public bool save;


    void Update()
    {
        Data = Lidar.SendNewWaveCone(depthMapSize.x, depthMapSize.y, viewDistance, hAngleRange, vAngleRange, hOffset,
            pingManager.transform.position, pingManager.transform.rotation.eulerAngles, Color.gray);
        depthTable = Lidar.CreateDepthTable(Data);

        if (render)
        {
            graphDisplay.UpdateDepthMap(depthTable, viewDistance);
        }

        if (Input.GetKeyDown(takeColorMap))
        {
            FindObstacles(depthTable);
        }
    }

    private List<Vector2Int> FindNeighbouring(float[,] depthTable, Vector2Int pixel, float deltaDist)
    {
        int height = Data.GetLength(0);
        int width = Data.GetLength(1);

        float dist = depthTable[pixel.x, pixel.y];

        List<Vector2Int> neighbours = new List<Vector2Int>();

        if (pixel.x > 0 && Mathf.Abs(depthTable[pixel.x - 1, pixel.y] - dist) < deltaDist)
        {
            neighbours.Add(new Vector2Int(pixel.x - 1, pixel.y));
        }
        if (pixel.x < height - 1 && Mathf.Abs(depthTable[pixel.x + 1, pixel.y] - dist) < deltaDist)
        {
            neighbours.Add(new Vector2Int(pixel.x + 1, pixel.y));
        }
        if (pixel.y > 0 && Mathf.Abs(depthTable[pixel.x, pixel.y - 1] - dist) < deltaDist)
        {
            neighbours.Add(new Vector2Int(pixel.x, pixel.y - 1));
        }
        if (pixel.y < width - 1 && Mathf.Abs(depthTable[pixel.x, pixel.y + 1] - dist) < deltaDist)
        {
            neighbours.Add(new Vector2Int(pixel.x, pixel.y + 1));
        }
        return neighbours;
    }


    //A partir d'un pixel, trouve tous ces voisins de sa composante connexe
    private List<Vector2Int> FindConnex(float[,] depthTable, int[,] coloration, int shapeColor, Vector2Int originalPixel, float deltaDist)
    {
        if (coloration[originalPixel.x, originalPixel.y] != 0)
        {
            throw new System.ArgumentException("This pixel is already colored");
        }

        List<Vector2Int> neighbours = new List<Vector2Int> { originalPixel };
        coloration[originalPixel.x, originalPixel.y] = shapeColor;

        int next = 0;

        //On regarde tous les voisins trouvés dans la composante connexe, on s'arrête quand il n'y en a plus
        while (next < neighbours.Count)
        {
            //On regarde les voisins du prochain pixel
            foreach (Vector2Int pixel in FindNeighbouring(depthTable, neighbours[next], deltaDist))
            {
                if (coloration[pixel.x, pixel.y] == 0)
                {
                    coloration[pixel.x, pixel.y] = shapeColor;
                    neighbours.Add(pixel);
                }
                else if (coloration[pixel.x, pixel.y] < shapeColor)
                {
                    throw new System.ArgumentException("Case déjà colorée");
                }
            }
            next++;
        }
        return neighbours;
    }


    public void FindObstacles(float[,] depthTable)
    {
        int height = depthTable.GetLength(0);
        int width = depthTable.GetLength(1);

        int[,] coloration = new int[height, width];
        int c = 2;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (depthTable[i, j] >= viewDistance - deltaDist)
                {
                    coloration[i, j] = 1;
                }

                else if (coloration[i, j] == 0)
                {
                    FindConnex(depthTable, coloration, c, new Vector2Int(i, j), deltaDist);
                    c++;
                    Debug.Log(c);
                }
            }
        }
        ColorMapToPNG(coloration);
    }


    private void ColorMapToPNG(int[,] coloration)
    {
        int height = coloration.GetLength(0);
        int width = coloration.GetLength(1);

        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, true) { filterMode = FilterMode.Point };

        for (int i = 0; i < texture.height; i++)
        {
            for (int j = 0; j < texture.width; j++)
            {
                try
                {
                    texture.SetPixel(j, i, colors[coloration[i, j] - 1]);
                }
                catch (System.IndexOutOfRangeException) { Debug.Log("Not enough colors"); }
            }
        }

        texture.Apply(false);

        if (save)
        {
            byte[] image = texture.EncodeToPNG();
            //System.IO.File.WriteAllBytes("ColorMap.png", image);

            Debug.Log("The ColorMap has been successfully created");
        }
        else
        {
            StartCoroutine(graphDisplay.DispColorMap(texture));
        }
    }
}
