using System;
using System.Collections.Generic;
using UnityEngine;

//Les différents types de case possibles
public enum MCode
{
    Nsp = 0,
    Vide = 1,
    Surface = 2,
}

public class OccupancyMap : MonoBehaviour
{
    public Vector3Int size;

    public Vector3 limitUp;
    public Vector3 limitDown;

    public Quadrillage quadrillage;

    public MCode[,,] carte;

#pragma warning disable IDE0051
    void Start()
    {
        quadrillage = new Quadrillage
            (size.z, size.y, size.x, limitUp.z - limitDown.z, limitUp.y - limitDown.y, limitUp.x - limitDown.x, false);

        carte = Quadrillage.CreateEmptyMatrix3<MCode>(MCode.Nsp, size.z + 1, size.x + 1, size.y);
    }
#pragma warning restore IDE0051

    //Lit les trajectoires reçues pour les mettre sur la carte
    public void UpdateMap(Bipoint[,] Data)
    {
        //On ajoute chaque parcours dans une liste
        foreach (Bipoint ray in Data)
        {
            List<Vector3Int> parcours = quadrillage.Parcours(DansQuadrillage(ray));

            try
            {
                foreach (Vector3Int place in parcours)
                {
                    carte[place.x, place.y, place.z] = MCode.Vide;
                }
            }
            catch (ArgumentOutOfRangeException) { }
            catch (IndexOutOfRangeException) { }
            finally { }
        }
    }

    //Transforme un Vector3 centré en 0 en Vector3 centré au début de Quadrillage.
    public Vector3 DansQuadrillage(Vector3 vector3)
    {
        return vector3 - new Vector3(limitDown.x, limitDown.y, limitUp.z);
    }

    //Transforme un Bipoint centré en 0 en Bipoint centré au début de Quadrillage.
    public Bipoint DansQuadrillage(Bipoint bipoint)
    {
        return new Bipoint(DansQuadrillage(bipoint.origine), DansQuadrillage(bipoint.flèche));
    }
}
