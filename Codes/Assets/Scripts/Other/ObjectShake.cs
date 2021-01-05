using UnityEngine;
using System.Collections;

public class ObjectShake : MonoBehaviour
{
    /*
     * Modified code from https://gist.github.com/ftvs/5822103
     * - Change from update to co-routine
     */

    [Tooltip("How long the object should shake for.")]
    public float shakeDuration = 1f;

    [Tooltip("Amplitude of the shake. A larger value shakes the camera harder.")]
    public float shakeIntensity = 0.7f;

    [Tooltip("Uses realtime, independent from time scale")]
    public bool useRealTime = true;

    public GameObject objectToShake;
    private Transform objTransform;

    private float internalShakeDuration; // use to time countdown
    private Vector3 originalPos; // use to reset camera position

    private void Awake()
    {
        objTransform = objectToShake.transform;
        originalPos = objectToShake.transform.localPosition;
    }

    private void OnEnable() { }

    private void OnDisable()
    {
        StopObjectShake();
    }

    public void StopObjectShake()
    {
        if (InternalShakeCameraRef != null)
        {
            StopCoroutine(InternalShakeCameraRef);
            internalShakeDuration = 0f;
            objTransform.position = originalPos;
        }
    }

    public void ShakeObject()
    {
        InternalShakeCameraRef = InternalShakeCamera();
        StartCoroutine(InternalShakeCameraRef);
    }

    private IEnumerator InternalShakeCameraRef;
    private IEnumerator InternalShakeCamera()
    {
        internalShakeDuration = shakeDuration;

        // If condition is put here for performances
        // So does not need to evaluate every frame
        if (useRealTime)
        {
            while (internalShakeDuration > 0)
            {
                objTransform.localPosition = originalPos + Random.insideUnitSphere * shakeIntensity;
                internalShakeDuration -= Time.unscaledDeltaTime;
                yield return null;
            }
        }
        else
        {
            while (internalShakeDuration > 0)
            {
                objTransform.localPosition = originalPos + Random.insideUnitSphere * shakeIntensity;
                internalShakeDuration -= Time.deltaTime;
                yield return null;
            }
        }

        internalShakeDuration = 0f;
        objTransform.localPosition = originalPos;
    }
}