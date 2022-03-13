using UnityEngine;

public enum Side
{
    Left = 1,
    Right = -1,
}


public class RobotController : MonoBehaviour
{
    private Rigidbody rbRigidbody;
    public GameObject centerOfMass;
    public CaterpillarMover lCaterpillar;
    public CaterpillarMover rCaterpillar;

    public float inputX;
    public float inputY;

    public float powerMax;
    public float turnRate;
    public float rearRate;

#pragma warning disable IDE0051
    private void Start()
    {
        rbRigidbody = gameObject.GetComponent<Rigidbody>();

        rbRigidbody.centerOfMass = centerOfMass.transform.localPosition;
    }

    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        if (inputX != 0 || inputY != 0)
        {
            rbRigidbody.WakeUp();
        }
    }
#pragma warning disable IDE0051

    private void OnCollisionStay(Collision collision)
    {
        //Pour chaque point de contact, appliquer une force au niveau du point d'application normalement à la surface du collider
        foreach (ContactPoint contact in collision.contacts)
        {
            lCaterpillar.Move(contact, Side.Left, Color.blue);
            rCaterpillar.Move(contact, Side.Right, Color.red);
        }
    }
}