using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


// Add this component to your Cinemachine Virtual Camera to have it shake when calling its ShakeCamera methods.

public class CinemachineCameraShaker : MonoBehaviour
{
    // the amplitude of the camera's noise when it's idle
    public float IdleAmplitude = 0.1f;
    // the frequency of the camera's noise when it's idle
    public float IdleFrequency = 1f;

    
    public float DefaultShakeAmplitude = .5f;
    
    public float DefaultShakeFrequency = 10f;

    protected Vector3 _initialPosition;
    protected Quaternion _initialRotation;

    protected Cinemachine.CinemachineBasicMultiChannelPerlin _perlin;
    protected Cinemachine.CinemachineVirtualCamera _virtualCamera;

    
    protected virtual void Awake()
    {
        _virtualCamera = GameObject.FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();     
        _perlin = _virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    
    //  reset camera to apply base amplitude and frequency
    
    protected virtual void Start()
    {
        CameraReset();
    }

    
    // method to shake the camera for the specified duration (in seconds) with the default amplitude and frequency
    
    
    public virtual void ShakeCamera(float duration)
    {
        StartCoroutine(ShakeCameraCo(duration, DefaultShakeAmplitude, DefaultShakeFrequency));
    }

    
    // method to shake the camera for the specified duration (in seconds), amplitude and frequency
   
    public virtual void ShakeCamera(float duration, float amplitude, float frequency)
    {
        StartCoroutine(ShakeCameraCo(duration, amplitude, frequency));
    }

    
    // This coroutine will shake the camera co.

    protected virtual IEnumerator ShakeCameraCo(float duration, float amplitude, float frequency)
    {
        _perlin.m_AmplitudeGain = amplitude;
        _perlin.m_FrequencyGain = frequency;
        yield return new WaitForSeconds(duration);
        CameraReset();
    }


    // Resets the camera's noise values to their idle values

    public virtual void CameraReset()
    {
        _perlin.m_AmplitudeGain = IdleAmplitude;
        _perlin.m_FrequencyGain = IdleFrequency;
    }

}
