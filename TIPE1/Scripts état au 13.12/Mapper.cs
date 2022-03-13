using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapper : MonoBehaviour
{
    public int sizeH;
    public int sizeL;

    public float limitxUp;
    public float limitxDown;
    public float limitzUp;
    public float limitzDown;

    private int heightMax;

    public Quadrillage quadrillage;
    private PingManager pingManager;

    public int[,] carte;

    void Start()
    {
        quadrillage = new Quadrillage(sizeH, sizeL, limitzUp - limitzDown, limitxUp - limitxDown, false);
        pingManager = GameObject.Find("Robot").GetComponent<PingManager>();

        carte = Quadrillage.CreateEmptyMatrix<int>(pingManager.nbVert, sizeH + 1, sizeL + 1);

        heightMax = pingManager.nbVert;
    }

    public void UpdateMap(int column, int wave)
    {
        List<List<(int, int)>> columnList = new List<List<(int, int)>>();

        for (int height = 0; height < heightMax; height++)                            //On ajoute chaque parcours dans une liste
        {
            List<(int, int)> parcours = quadrillage.Parcours(pingManager.pingTable[wave][column, height]);
            columnList.Add(parcours);
        }

        int parcoursIdx = 0;

        for (int height = 0; height < heightMax; height++)                            //Pour chaque couche
        {
            while (columnList[height].Count > parcoursIdx)
            {
                if (carte[columnList[height][parcoursIdx].Item1, columnList[height][parcoursIdx].Item2] > height)
                {
                    carte[columnList[height][parcoursIdx].Item1, columnList[height][parcoursIdx].Item2] = height;
                }
                parcoursIdx++;
            }
        }
    }

    public Vector3 DansCardillage(Vector3 vector3)
    {
        return vector3 - new Vector3(limitxDown, 0, limitzUp);
    }
}
