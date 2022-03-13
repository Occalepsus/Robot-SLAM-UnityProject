using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public List<GameObject> textsList;
    public float secondsBeforeDisplay;
    private bool called = false;

    void Update()
    {
        if (Input.anyKey)
        {
            called = false;
            foreach (var text in textsList)
            {
                text.SetActive(false);
            }
        }
        else if (!called)
        {
            StartCoroutine(Show());
        }
    }

    private IEnumerator Show()
    {
        called = true;
        float time = Time.time;
        while(!Input.anyKey && Time.time < time + secondsBeforeDisplay)
        {
            yield return new WaitForEndOfFrame();
        }
        if (!Input.anyKey)
        {
            foreach (var text in textsList)
            {
                text.SetActive(true);
            }
        }
        called = false;
    }

    public void Close()
    {
        Application.Quit();
    }
}
