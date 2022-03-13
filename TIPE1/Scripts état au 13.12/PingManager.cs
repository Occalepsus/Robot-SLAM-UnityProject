using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PingManager : MonoBehaviour
{
    private GameObject robot;
    public GameObject pingPrefab;

    public GameObject simManager;
    private Mapper mapper;

    public int nbHor;                                   //nbHor correspond au nombre de ping envoyés sur le plan (xOz)
    public int nbVert;                                  //nbVer correspond au nombre de ping envoyés selon l'axe (Oy)
    public float hOffset;
    public float dH;
    public float dTheta;

    private bool input0;
    public KeyCode launch;
    public bool forcePing = false;

    public int wave = 0;
    public List<Bipoint[,]> pingTable = new List<Bipoint[,]>();
    private readonly Bipoint nullBipoint = new Bipoint(Vector3.zero, Vector3.zero);

    void Start()
    {
        robot = gameObject;
        mapper = simManager.gameObject.GetComponent<Mapper>();
    }

    void Update()
    {
        input0 = Input.GetKeyDown(launch);
        if (input0 || forcePing)
        {
            SendNewPing(nbHor, nbVert, dH, dTheta, wave);
            wave++;
            forcePing = false;
        }
    }
    public void SendNewPing(int nbHor, int nbVert, float dH, float dTheta, int newWave)
    {
        pingTable.Add(Quadrillage.CreateEmptyMatrix(new Bipoint(), nbHor, nbVert));         //On crée une nouvelle matrice qu'on ajoute au tableau pingTable

        float thetaInit;

        if (nbHor % 2 == 0)
        {
            thetaInit = -(nbHor / 2) * dTheta + dTheta/2;
        }
        else
        {
            thetaInit = -((nbHor - 1) / 2) * dTheta;
        }

        for (int i = 0; i < nbHor; i++)
        {
            for (int j = 0; j < nbVert; j++)
            {
                GameObject pingObject = Instantiate(pingPrefab, 
                    robot.transform.position + new Vector3(0, hOffset + j * dH, 0),
                    Quaternion.Euler(new Vector3(0, thetaInit + i * dTheta))*robot.transform.rotation);  //TEMPORAIRE !
                Ping ping = pingObject.GetComponent<Ping>();

                pingObject.transform.parent = simManager.transform;

                ping.column = i;                             //On assigne les paramètres de chaque Ping
                ping.height = j;
                ping.wave = newWave;
            }
        }
    }

    public void Hit(int i, int j, int wave, Bipoint trajectory)
    {
        pingTable[wave][i, j] = trajectory;                 //On stocke le Bipoint dans notre Table en attendant de le traiter

        bool E = true;
        int k = 0;
        while (E && k < nbVert)                             //On regarde si tous les pings de la colonne sont arrivés.
        {
            E = pingTable[wave][i, k] != nullBipoint;
            k++;
        }

        if (E)                                              //Si tous les pings de la colonne sont arrivés, on appelle UpdateMap
        {
            //Debug.Log("Ok, on est bon sur " + i.ToString() + wave.ToString());          
            mapper.UpdateMap(i, wave);
        }
    } 
}
