using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZone : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Awake()
    {
        Camera.main.gameObject.TryGetComponent<CinemachineBrain>(out var brain);

        if (brain != null)
        {
            brain = Camera.main.gameObject.AddComponent<CinemachineBrain>();
        }
        brain.m_DefaultBlend.m_Time = 1;
        brain.m_ShowDebugText = true;

        cinemachineVirtualCamera = gameObject.AddComponent<CinemachineVirtualCamera>();
    }
}
