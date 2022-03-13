using Packages.Rider.Editor.UnitTesting;
using System;
using UnityEngine;
using System.Collections.Generic;

public class Quadrillage
{
	public int height;
	public int width;

	public float hScale;
	public float wScale;	

	public Quadrillage() { }

	public Quadrillage(int height, int width)
    {
		this.height = height;
		this.width = width;
    }

	public Quadrillage(int height, int width, float longueur, float largeur, bool useScalesInstead = false)
	{
		this.height = height;
		this.width = width;

		if (! useScalesInstead) //Méthode avec les distances
		{
			hScale = Math.Abs(longueur / height);
            wScale = Math.Abs(largeur / width);
		}
        else					//Méthode avec les divisions
        {
			hScale = Math.Abs(longueur);
			wScale = Math.Abs(largeur);
        }
	}

	public float longueur
    {
		get => hScale * height;
		set => hScale = Math.Abs(longueur / height);
    }
	
	public float largeur
    {
		get => wScale * width;
		set => wScale = Math.Abs(largeur / height);
    }

	public (int,int) Point(Vector3 vector)
    {
		int i = -vector.z != longueur ? (int)(-vector.z / hScale) : (int)(-vector.z / hScale) - 1;

        int j = vector.x != largeur ? (int)(vector.x / wScale) : (int)(vector.x - 1 / wScale) - 1;

        return (i, j);
    }

	public List<(int, int)> Parcours(Bipoint bipoint)
    {
        (int, int) origine = Point(bipoint.origine);
        (int, int) flèche = Point(bipoint.flèche);

		int a = Math.Abs(flèche.Item1 - origine.Item1);
		int b = Math.Abs(flèche.Item2 - origine.Item2);

		int n = (int) Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));

		List<(int, int)> parcours = new List<(int, int)>();

		if (n != 0)
        {
			parcours.Add(Point(bipoint.origine + (1 / n) * bipoint.direction));
			for (float k = 2; k < n+1; k++)
	        {
				(int, int) coord = Point(bipoint.origine + (k / n) * bipoint.direction);
				if (coord != parcours[parcours.Count - 1])
			    {
					parcours.Add(coord);
			    }
		    }
        }
		return parcours;
    }

	public static T[,] CreateEmptyMatrix<T>(T obj, int n, int p)     //Crée une matrice de type T et de taille (n,p) de obj objets
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

	public static T[] CreateEmptyArray<T>(T obj, int n)				//Crée un vecteur de type T et de taille n de obj objets
	{
		T[] arr = new T[n];

		for (int i = 0; i < n; i++)
		{
			arr[i] = obj;
		}
		return arr;
	}
}
