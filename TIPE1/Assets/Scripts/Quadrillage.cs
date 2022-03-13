using System.Collections.Generic;
using UnityEngine;

public class Quadrillage
{
    public int zAxisNb;
    public int yAxisNb;
    public int xAxisNb;

    public float zScale = 1;
    public float yScale = 1;
    public float xScale = 1;

    //Crée un quadrillage vide
    public Quadrillage() { }

    //Crée un quadrillage en choisissant la taille
    public Quadrillage(int zAxisNb, int yAxisNb, int xAxisNb)
    {
        this.zAxisNb = zAxisNb;
        this.yAxisNb = yAxisNb;
        this.xAxisNb = xAxisNb;
    }

    //Crée un quadrillage complet
    public Quadrillage(int zAxisNb, int yAxisNb, int xAxisNb, float longueur, float hauteur, float largeur, bool useScalesInstead = false)
    {
        this.zAxisNb = zAxisNb;
        this.yAxisNb = yAxisNb;
        this.xAxisNb = xAxisNb;

        //Méthode avec les distances
        if (!useScalesInstead)
        {
            zScale = Mathf.Abs(longueur / zAxisNb);
            yScale = Mathf.Abs(hauteur / yAxisNb);
            xScale = Mathf.Abs(largeur / xAxisNb);
        }
        //Méthode avec les divisions
        else
        {
            zScale = Mathf.Abs(longueur);
            yScale = Mathf.Abs(hauteur);
            xScale = Mathf.Abs(largeur);
        }
    }

#pragma warning disable IDE1006
    //Donne les paramètres de taille
    public float longueur
    {
        get => zScale * zAxisNb;
        set => zScale = Mathf.Abs(longueur / zAxisNb);
    }

    public float hauteur
    {
        get => zScale * zAxisNb;
        set => zScale = Mathf.Abs(hauteur / zAxisNb);
    }

    public float largeur
    {
        get => xScale * xAxisNb;
        set => xScale = Mathf.Abs(largeur / zAxisNb);
    }
#pragma warning disable IDE1006

    //Transforme un Vector3 float en Vector3Int adapté au quadrillage. (i : axe -z, j : axe +x, k : axe +y)
    public Vector3Int Point(Vector3 vector)
    {
        int i = -vector.z != longueur ? (int)(-vector.z / zScale) : (int)(-vector.z / zScale) - 1;

        int j = vector.x != largeur ? (int)(vector.x / xScale) : (int)(vector.x - 1 / xScale) - 1;

        int k = (int)(vector.y / yScale);

        return new Vector3Int(i, j, k);
    }


    //Transpose dans le quadrillage tous les points que rencontre le bipoint (seulement sur le plan (x,z))
    public List<Vector3Int> Parcours(Bipoint bipoint)
    {
        Vector3Int origine = Point(bipoint.origine);
        Vector3Int flèche = Point(bipoint.flèche);

        int a = Mathf.Abs(flèche.x - origine.x);
        int b = Mathf.Abs(flèche.y - origine.y);

        int n = (int)Mathf.Sqrt(a * a + b * b);

        List<Vector3Int> parcours = new List<Vector3Int>();

        //Si la trajectoire n'est pas réduite à un point
        if (n != 0)
        {
            parcours.Add(Point(bipoint.origine + (1 / n) * bipoint.direction));

            //Sépare le bipoint en Bipoints plus courts
            for (float k = 2; k < n + 1; k++)
            {
                Vector3Int coord = Point(bipoint.origine + (k / n) * bipoint.direction);
                if (coord != parcours[parcours.Count - 1])
                {
                    parcours.Add(coord);
                }
            }
        }
        return parcours;
    }

    //Crée une matrice cubique de type T et de taille (n,p,q) de obj objets
    public static T[,,] CreateEmptyMatrix3<T>(T obj, int n, int p, int q)
    {
        T[,,] arr = new T[n, p, q];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < p; j++)
            {
                for (int k = 0; k < q; k++)
                {
                    arr[i, j, k] = obj;
                }
            }
        }
        return arr;
    }

    //Crée une matrice de type T et de taille (n,p) de obj objets
    public static T[,] CreateEmptyMatrix<T>(T obj, int n, int p)
    {
        T[,] arr = new T[n, p];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < p; j++)
            {
                arr[i, j] = obj;
            }
        }
        return arr;
    }

    //Crée un vecteur de type T et de taille n de obj objets
    public static T[] CreateEmptyArray<T>(T obj, int n)
    {
        T[] arr = new T[n];

        for (int i = 0; i < n; i++)
        {
            arr[i] = obj;
        }
        return arr;
    }
}
