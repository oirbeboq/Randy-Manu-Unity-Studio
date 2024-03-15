using UnityEngine;

public class MoveCamera : MonoBehaviour 
{

    [SerializeField] public Transform cameraPosition = null;

    void Update() {
        transform.position = cameraPosition.position;
    }
}
