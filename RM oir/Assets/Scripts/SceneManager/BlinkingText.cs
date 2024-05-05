using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingText : MonoBehaviour
{
    public GameObject text;
    public float speed = 0.3f;
    private void Start()
    {
        InvokeRepeating("StartBlinking", 1f, speed);
    }

    void StartBlinking()
    {
        text.SetActive(!text.activeInHierarchy);
    }
}
