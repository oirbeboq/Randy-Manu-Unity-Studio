using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public PlayerMovement player;
    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            player.LoadCheckPoint();
            Debug.Log("RESPAWN");   
        }
    }
}
