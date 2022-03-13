using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapIconMove : MonoBehaviour
{
    public GameObject robot;
    public float y;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(robot.transform.position.x, y, robot.transform.position.z);
        transform.eulerAngles = new Vector3(90, robot.transform.rotation.eulerAngles.y, 0);
    }
}
