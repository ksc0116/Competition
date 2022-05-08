using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private static CameraShake instance;
    public static CameraShake Instance => instance;

    private float shakeTime;
    private float shakeIntensity;

    private CameraController cameraController;
    public CameraShake()
    {
        instance = this;
    }
    private void Awake()
    {
        cameraController=GetComponent<CameraController>();
    }
    public void OnShakeCamera(float shakeTime=1.0f,float shakeIntensity = 0.1f,bool isPlus=true)
    {
        this.shakeTime = shakeTime;
        this.shakeIntensity = shakeIntensity;

        StopCoroutine("ShakeByRotation");
        StartCoroutine("ShakeByRotation",isPlus);
    }

    private IEnumerator ShakeByRotation(bool isPlus)
    {
        cameraController.isOnShake = true;

        Vector3 startRotation = transform.eulerAngles;

        float power = 10f;

        while (shakeTime > 0.0f)
        {
            float x = isPlus==true ? 20 : -20;
            float y = 0;
            float z = 0;

            transform.rotation = Quaternion.Euler(startRotation + new Vector3(x, y, z) * shakeIntensity * power);

            shakeTime -= Time.deltaTime;

            yield return null;
        }
        transform.rotation=Quaternion.Euler(startRotation);
        cameraController.isOnShake = false;
    }
}
