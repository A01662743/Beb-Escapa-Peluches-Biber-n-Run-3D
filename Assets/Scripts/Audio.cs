using System.Collections;
using UnityEngine;
using System;

public class Audio : MonoBehaviour
{
    private AudioSource targetAudioSource;
    private Coroutine pitchCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        // Nos suscribimos a la señal del objeto
        Bebe.OnBebeTocado += SlowDownPitch;
    }

    private void OnDisable()
    {
        // Nos desuscribimos para evitar fugas de memoria
        Bebe.OnBebeTocado -= SlowDownPitch;
    }

    public void SlowDownPitch(float duration)
    {
        if (targetAudioSource == null) return;
        if (pitchCoroutine != null) StopCoroutine(pitchCoroutine);
        pitchCoroutine = StartCoroutine(SmoothPitchToZero(duration));
    }

    private IEnumerator SmoothPitchToZero(float duration)
    {
        float startPitch = targetAudioSource.pitch;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            
            float t = elapsed / duration;
            targetAudioSource.pitch = Mathf.Lerp(startPitch, 0f, t);

            yield return null;
        }

        targetAudioSource.pitch = 0f;
    }

}
