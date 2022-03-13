using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ping : MonoBehaviour
{
    private GameObject robot;
    private GameObject simManager;
    private PingManager pingManager;
    private Mapper mapper;
    private Rigidbody rb;

    public float speed;
    public int column;
    public int height;
    public int wave;

    private Vector3 posInit;

    void Start()
    {
        robot = GameObject.Find("Robot");               //On assigne les références des objets
        simManager = GameObject.Find("SimManager");
        pingManager = robot.GetComponent<PingManager>();
        mapper = simManager.GetComponent<Mapper>();
        rb = gameObject.GetComponent<Rigidbody>();

        posInit = gameObject.transform.position;

        rb.AddRelativeForce(Vector3.forward * speed, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider obstacle)      //Si le Ping rencontre un collider
    {
        if (obstacle.tag == "Obstacle")                 //Test du tag du collider (pour pas taper le robot !!)
        {
            Bipoint trajectory = new Bipoint(mapper.DansCardillage(posInit), mapper.DansCardillage(transform.position));

            pingManager.Hit(column, height, wave, trajectory);

            Destroy(gameObject);
            //rb.AddRelativeForce(-Vector3.forward * speed, ForceMode.VelocityChange);
        }
    }
}
