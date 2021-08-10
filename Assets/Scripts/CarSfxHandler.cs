using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSfxHandler : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource carScreechAudioSource;
    public AudioSource carEngineAudioSource;
    public AudioSource carHitAudioSource;

    float desiredEnginePitch = 0.5f;
    float carScreechPitch = 0.5f;
    CarController carController;

    void Awake()
    {
        carController = GetComponentInParent<CarController>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateCarEngineSFX();
        UpdateCarScreechSFX();
    }

    void UpdateCarEngineSFX()
    {
        float velocityMagnitude = carController.GetVelocityMagnitude();
        float desiredEngineVolume = velocityMagnitude * 0.005f;

        desiredEngineVolume = Mathf.Clamp(desiredEngineVolume, 0.1f, 0.5f);
        carEngineAudioSource.volume = Mathf.Lerp(carEngineAudioSource.volume, desiredEngineVolume, Time.deltaTime * 10);

        desiredEnginePitch = velocityMagnitude * 0.02f;
        desiredEnginePitch = Mathf.Clamp(desiredEnginePitch, 0.5f, 2.0f);
        carEngineAudioSource.pitch = Mathf.Lerp(carEngineAudioSource.pitch, desiredEnginePitch, Time.deltaTime * 1.5f);
    }

    void UpdateCarScreechSFX()
    {
        if (carController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            if (isBraking)
            {
                carScreechAudioSource.volume = Mathf.Lerp(carScreechAudioSource.volume, 0.05f, Time.deltaTime * 10);
                carScreechPitch = Mathf.Lerp(carScreechPitch, 0.5f, Time.deltaTime * 10);
            }
            else
            {
                carScreechAudioSource.volume = Mathf.Abs(lateralVelocity) * 0.002f;
                carScreechPitch = Mathf.Abs(lateralVelocity) * 0.001f;
            }
        }
        else carScreechAudioSource.volume = Mathf.Lerp(carScreechAudioSource.volume, 0, Time.deltaTime * 10);
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        float relativeVelocity = collision2D.relativeVelocity.magnitude;
        float volume = relativeVelocity * 0.0005f;

        carHitAudioSource.volume = volume;
        carHitAudioSource.pitch = Random.Range(0.95f, 1.05f);

        if (!carHitAudioSource.isPlaying)
            carHitAudioSource.Play();
    }
}
