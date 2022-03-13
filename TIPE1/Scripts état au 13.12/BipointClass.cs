using System;
using UnityEngine;

public class Bipoint                                    //On définit le Bipoint, composé d'un Vector3 de départ et un Vector3 d'arrivée
{
    public Vector3 origine = Vector3.zero;
    public Vector3 flèche = Vector3.zero;

    public Bipoint() { }

    public Bipoint(Vector3 origine, Vector3 flèche)     //Définit le Bipoint
    {
        this.origine = origine;
        this.flèche = flèche;
    }

    public Vector3 direction                            //Renvoit le Vector3 direction de ce Bipoint
    {
        get => this.flèche - this.origine;
        set => this.direction = this.flèche - this.origine; //!!!! Penser à vérifier si on peut virer les this
    }

    public float magnitude                              //Renvoit le flottant de la norme de la direction de ce Bipoint
    {
        get => this.direction.magnitude;
    }

    public Bipoint normalize()                          //Renvoit un bipoint de même origine dont la direction a pour norme 1.
    {
        Bipoint normalizedBipoint = new Bipoint
        {
            origine = this.origine,
            direction = this.direction.normalized
        };
        return normalizedBipoint;
    }

    public override string ToString()                   //Sert à renvoyer un Bipoint en String pour le deboggage
    {
        return "Bipoint : (" + origine.ToString() + "), (" + flèche.ToString() + "))";
    }

                                                        //On définit les relations de comparaisons entre Bipoints
    public static bool operator == (Bipoint left, Bipoint right)
    {
        return (right.origine == left.origine && right.flèche == left.flèche);
    }
    public static bool operator != (Bipoint left, Bipoint right)
    {
        return (right.origine != left.origine || right.flèche != left.flèche);
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}