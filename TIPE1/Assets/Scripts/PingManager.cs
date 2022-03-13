using System.Collections.Generic;
using UnityEngine;

public struct RayInfo
{
    public Bipoint ray;
    public bool touched;

    public RayInfo(Bipoint ray, bool touched)
    {
        this.ray = ray;
        this.touched = touched;
    }
}


public class PingManager : MonoBehaviour
{
    public GameObject simManager;
    private OccupancyMap occupancyMap;

    public int nbHor;                                   //nbHor correspond au nombre de rayons envoyés sur le plan (xOz)
    public int nbVert;                                  //nbVer correspond au nombre de rayons envoyés selon l'axe (Oy)
    public float hOffset;
    public float dH;
    public float angleRange;

    public float distMax;

    public Bipoint[,] Data;
    public List<RayInfo[,]> pingTable = new List<RayInfo[,]>();

    private Vector3 lastPos;
    private Quaternion lastRot;

#pragma warning disable IDE0051
    void Start()
    {
        occupancyMap = simManager.gameObject.GetComponent<OccupancyMap>();
        lastPos = transform.position;
        lastRot = transform.rotation;

        Data = Lidar.SendNewWaveHor
                (nbVert, nbHor, distMax, angleRange, dH, hOffset, transform.position, transform.rotation.eulerAngles, Color.green);
    }

    void Update()
    {
        if (lastPos != transform.position || lastRot != transform.rotation)
        {
            Data = Lidar.SendNewWaveHor
                (nbVert, nbHor, distMax, angleRange, dH, hOffset, transform.position, transform.rotation.eulerAngles, Color.green);
            occupancyMap.UpdateMap(Data);

            lastPos = transform.position;
            lastRot = transform.rotation;
        }
    }
#pragma warning restore IDE0051
}
