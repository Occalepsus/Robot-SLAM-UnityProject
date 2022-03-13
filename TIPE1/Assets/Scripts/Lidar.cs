using UnityEngine;

public class Lidar
{
    //On envoie des rayons parallèles au sol
    public static Bipoint[,] SendNewWaveHor(int height, int width, float distMax, float angleRange, float dH, float hOffset, Vector3 position, Vector3 rotation, Color color)
    {
        Bipoint[,] Data = Quadrillage.CreateEmptyMatrix(Bipoint.zero, height, width);
        float originalAngle = rotation.y - angleRange / 2f;
        float horAngle = angleRange / (width - 1);

        //Dans le sens de la hauteur
        for (int i = 0; i < height; i++)
        {
            Vector3 origine = position + new Vector3(0, hOffset + i * dH, 0);

            //Dans le sens de la largeur
            for (int j = 0; j < width; j++)
            {
                //On calcule l'angle horizontal
                float angle = j * horAngle + originalAngle;
                angle *= Mathf.Deg2Rad;
                Vector3 direction = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));

                //On envoie un rayon et on regarde le rayon résultant
                Bipoint ray = new Bipoint(origine, direction * distMax, false);
                ray = SendRay(ray);
                Debug.DrawRay(ray.origine, ray.direction, color, Time.deltaTime);

                //On ajoute la distance obtenue à Data
                Data[i, j] = ray;
            }
        }
        return Data;
    }

    //Envoie une vague de rayons de façon cônique
    public static Bipoint[,] SendNewWaveCone(int height, int width, float distMax, float horAngleRange, float vertAngleRange, float hOffset, Vector3 position, Vector3 rotation, Color color)
    {
        Bipoint[,] Data = Quadrillage.CreateEmptyMatrix(Bipoint.zero, height, width);
        float originalHAngle = rotation.y - horAngleRange / 2f;
        float originalVAngle = -rotation.x - vertAngleRange / 2f + 20;
        float horAngle = horAngleRange / (width - 1);
        float vertAngle = vertAngleRange / (height - 1);

        Vector3 origine = position + new Vector3(0, hOffset, 0);

        //Dans le sens de la hauteur
        for (int i = 0; i < height; i++)
        {
            float vAngle = i * vertAngle + originalVAngle;
            vAngle *= Mathf.Deg2Rad;
            //Dans le sens de la largeur
            for (int j = 0; j < width; j++)
            {
                //On calcule l'angle horizontal
                float hAngle = j * horAngle + originalHAngle;
                hAngle *= Mathf.Deg2Rad;
                Vector3 direction = new Vector3(Mathf.Sin(hAngle), Mathf.Sin(vAngle), Mathf.Cos(hAngle));

                //On envoie un rayon et on regarde le rayon résultant
                Bipoint ray = new Bipoint(origine, direction * distMax, false);
                ray = SendRay(ray);

                if ((i == 0 && j == 0) || (i == height - 1 && j == 0) || (i == 0 && j == width - 1) || (i == height - 1 && j == width - 1))
                {
                    Debug.DrawRay(ray.origine, ray.direction, color, Time.deltaTime);
                }

                //On ajoute la distance obtenue à Data
                Data[i, j] = ray;
            }
        }
        return Data;
    }


    //Calcule le trajet de ray en prennant en compte les colliders
    public static Bipoint SendRay(Bipoint ray)
    {
        float distMax = ray.magnitude;

        RaycastHit[] hitList;
        hitList = Physics.RaycastAll(ray.origine, ray.direction, distMax);

        RaycastHit hitMin = new RaycastHit { distance = distMax };

        foreach (RaycastHit hit in hitList)
        {
            if (hit.collider.gameObject.CompareTag("Obstacle") && hit.distance < hitMin.distance)
            {
                hitMin = hit;
            }
        }

        if (hitMin.distance < distMax)
        {
            ray.flèche = hitMin.point;
        }

        return ray;
    }


    public static float[,] CreateDepthTable(Bipoint[,] Data)
    {
        int height = Data.GetLength(0);
        int width = Data.GetLength(1);

        float[,] depthTable = new float[height, width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                depthTable[i, j] = Data[i, j].magnitude;
            }
        }
        return depthTable;
    }


    public static Texture2D EncodeDepthMap(float[,] depthTable, float distMax)
    {
        int height = depthTable.GetLength(0);
        int width = depthTable.GetLength(1);

        Texture2D depthMap = new Texture2D(width, height, TextureFormat.RGB24, false);

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                float color = 1 - depthTable[i, j] / distMax;
                depthMap.SetPixel(j, i, new Color(color, color, color));
            }
        }
        depthMap.Apply();
        return depthMap;
    }
}
