using UnityEngine;

public class Osef : MonoBehaviour
{
    public float sizeInit;
    public float grow;
    public float speed;
    public float hspeed;

    public GameObject boule;

    // Start is called before the first frame update
    void Start()
    {
        boule.transform.localScale = new Vector3(sizeInit, sizeInit, sizeInit);
    }

    // Update is called once per frame
    void Update()
    {
        float nSpeed = Input.GetAxis("Vertical") * speed;
        float rotate = Input.GetAxis("Horizontal") * hspeed;
        transform.Rotate(new Vector3(0, 1, 0) * rotate);
        transform.Translate(Vector3.forward * nSpeed);
        Debug.Log(Vector3.forward * nSpeed);

        float nscale = boule.transform.localScale.x + grow * Mathf.Abs(Input.GetAxis("Vertical"));
        boule.transform.localScale = new Vector3(nscale, nscale, nscale);
    }
}
