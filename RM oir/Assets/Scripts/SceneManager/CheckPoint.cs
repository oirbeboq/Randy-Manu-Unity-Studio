using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform checkPoint;
    public PlayerMovement player;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            player.checkPoint = checkPoint.position;
            Debug.Log("SAVED");
        }
    }
}
