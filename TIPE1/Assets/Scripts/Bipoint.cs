using UnityEngine;

public struct Bipoint
{
    public Vector3 origine;
    public Vector3 flèche;

    //On définit le Bipoint, composé d'un Vector3 de départ et un Vector3 d'arrivée
    public Bipoint(Vector3 origine, Vector3 flèche, bool usingflèche = true)
    {
        this.origine = origine;
        this.flèche = usingflèche ? flèche : origine + flèche;
    }

#pragma warning disable IDE1006
    public static Bipoint zero
    {
        get => new Bipoint(Vector3.zero, Vector3.zero);
    }

    //Renvoit le Vector3 direction de ce Bipoint
    public Vector3 direction
    {
        get => this.flèche - this.origine;
        set => this.flèche = this.origine + this.direction;
    }

    //Renvoit le flottant de la norme de la direction de ce Bipoint
    public float magnitude
    {
        get => this.direction.magnitude;
    }
#pragma warning restore IDE1006

    //Renvoit un bipoint de même origine dont la direction a pour norme 1
    public Bipoint Normalize()
    {
        Bipoint normalizedBipoint = new Bipoint
        {
            origine = this.origine,
            direction = this.direction.normalized
        };
        return normalizedBipoint;
    }

    //Copie le Bipoint
    public Bipoint Copy()
    {
        return new Bipoint(origine, flèche);
    }

    //Sert à renvoyer un Bipoint en String pour le deboggage
    public override string ToString()
    {
        return "Bipoint : (" + origine.ToString() + "), (" + flèche.ToString() + "))";
    }

    //Transforme un Bipoint en Ray
    public Ray ToRay()
    {
        return new Ray(this.origine, this.direction);
    }

    //On définit les relations de comparaisons entre Bipoints
    public static bool operator ==(Bipoint left, Bipoint right)
    {
        return (right.origine == left.origine && right.flèche == left.flèche);
    }
    public static bool operator !=(Bipoint left, Bipoint right)
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