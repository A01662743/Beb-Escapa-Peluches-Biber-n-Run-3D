using UnityEngine;
using UnityEngine.UI; // Necesario para trabajar con UI.Image
using System;
using UnityEngine.EventSystems;
using System.Collections;

public class PopOutIn : MonoBehaviour
{

    public Image[] LoseImages;
    public Image[] PauseImages;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        BebeTocadoHandeler(3.0f);
    }

    // Update is called once per frame
    private void BebeTocadoHandeler(float duracion)
    {
        Debug.Log("Detectado fin en HUD transision");
        StartCoroutine(Pop(duracion, LoseImages));
    }

    private IEnumerator Pop(float duracion, Image[] images)
    {
        yield return new WaitForSecondsRealtime(duracion/3f);
        for (int x = 0; x < images.Length; x++)
        {
            if (images[x] != null)
            {
                animator = images[x].GetComponent<Animator>();
                if (animator != null){
                    animator.SetTrigger("Start");
                }
                yield return new WaitForSecondsRealtime(0.2f);
            }
        }
         yield return null;
    }
}
