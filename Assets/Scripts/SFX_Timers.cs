using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Timers : MonoBehaviour
{
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public float Timer = 5f; // Intervalo de tiempo en segundos

    void Start()
    {
        // Invoca repetidamente la función Repetir cada 'intervaloDeTiempo' segundos
        InvokeRepeating("Repetir", 30, Timer);
    }

    void Repetir()
    {
        StartCoroutine(Audio(audioSource1));
        StartCoroutine(Audio(audioSource2));
    }

    IEnumerator Audio(AudioSource audioSource)
    {
        // Reproduce el audio
        audioSource.Play();

        // Espera hasta que el audio haya terminado de reproducirse
        yield return new WaitForSeconds(audioSource.clip.length);

        // Detiene la reproducción
        audioSource.Stop();
    }
}