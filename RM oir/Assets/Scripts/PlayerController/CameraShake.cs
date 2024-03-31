using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    public void Awake()
    {
        instance = this; 
    }

    private void OnShake(float duration, float strengthh)
    {
        transform.DOShakePosition(duration, strengthh);
        transform.DOShakeRotation(duration, strengthh);
    }

    public static void Shake(float duration, float strengthh)
    {
        instance.OnShake(duration, strengthh);
    }
}
