using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class RobotController : MonoBehaviour
{
    private Rigidbody rbRigidbody;
    private GameObject centerOfMass;

    public float inputX;
    public float inputY;

    public float maxSpeed;
    public float accelerationRate;
    public float turnRate;

    private void Start()
    {
        rbRigidbody = gameObject.GetComponent<Rigidbody>();
        centerOfMass = gameObject.transform.GetChild(0).gameObject;

        rbRigidbody.centerOfMass = centerOfMass.transform.localPosition;
    }

    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
    }

    void LateUpdate()
    {
        Move();
        Turn();
    }

    private void Move()
    {
        float acceleration = accelerationRate * (maxSpeed - rbRigidbody.velocity.magnitude);
        rbRigidbody.AddForce(gameObject.transform.forward * acceleration * inputY, ForceMode.Acceleration);
    }

    private void Turn()
    {
        rbRigidbody.transform.Rotate(new Vector3(0, turnRate * inputX, 0));
    }
}