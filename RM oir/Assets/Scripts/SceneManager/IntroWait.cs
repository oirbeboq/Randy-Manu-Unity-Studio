using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroWait : MonoBehaviour
{
    public float waitTime = 5f;
    public string Scenename;

    private void Start()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(Scenename);
    }
}
