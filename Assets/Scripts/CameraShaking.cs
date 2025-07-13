using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaking : MonoBehaviour
{

    //MOVER TODO ESTO AL CAMERA Y CONTROLARLO DESDE ALLI

    //https://gist.github.com/ftvs/5822103

    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    private Transform camTransform;

    // How long the object should shake for.
    public float shakeDuration = 0f;
    private float initShakeDuration;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    void Awake()
    {
        initShakeDuration = shakeDuration;

        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
        this.enabled = false;
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;

    }

    void Update()
    {
        BombShake();
    }

    private void BombShake()
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = initShakeDuration;
            camTransform.localPosition = originalPos;
            this.enabled = false;
        }
    }
}
