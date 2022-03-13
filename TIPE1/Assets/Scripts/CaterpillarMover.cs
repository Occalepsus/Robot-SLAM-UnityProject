using UnityEngine;

public class CaterpillarMover : MonoBehaviour
{
    private Rigidbody robotRigidbody;
    private Collider whCollider;
    private RobotController robotController;

    public Vector3 normalVector;
    private Vector3 wheelsRotationVector;
    private float powerMax;
    private float turnRate;
    private float rearRate;

#pragma warning disable IDE0051
    // Start is called before the first frame update
    void Start()
    {
        robotRigidbody = gameObject.GetComponentInParent<Rigidbody>();
        whCollider = gameObject.GetComponent<Collider>();
        robotController = gameObject.GetComponentInParent<RobotController>();

        powerMax = robotController.powerMax;
        turnRate = robotController.turnRate;
        rearRate = robotController.rearRate;
    }

    // Update is called once per frame
    void Update()
    {
        wheelsRotationVector = Vector3.Cross(normalVector, robotRigidbody.transform.forward);
    }
#pragma warning restore IDE0051

    // Applique au point de contact voulu une force power
    public void Move(ContactPoint contact, Side side, Color color)
    {
        float power = powerMax * (robotController.inputY + turnRate * (int)side * robotController.inputX);

        if (power < 0) { power *= rearRate; }

        if (whCollider == contact.thisCollider)
        {
            Vector3 tractionForceAtContact = Vector3.Cross(wheelsRotationVector, contact.normal) * power;
            robotRigidbody.AddForceAtPosition(tractionForceAtContact, contact.point, ForceMode.Force);

            Debug.DrawRay(contact.point, tractionForceAtContact, color);
        }
    }
}
