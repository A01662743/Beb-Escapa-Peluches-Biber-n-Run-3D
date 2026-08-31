using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class HUDPausaAnim : MonoBehaviour
{

    private Animator animator;
    public Image[] PauseImages;
    private float offset = 0.2f;
    private float loopOffset = 2f;
    private Coroutine coroutine;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        Reset(PauseImages);
        coroutine = StartCoroutine(Jump(PauseImages));
    }

    private void OnDisable()
    {
        // 1. Detener el bucle para que no siga ejecutándose en segundo plano
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Jump(Image[] images)
    {
        while (true){
            for (int x = 0; x < images.Length; x++)
            {
                if (images[x] != null)
                {
                    animator = images[x].GetComponent<Animator>();
                    if (animator != null){
                        animator.SetTrigger("Start");
                    }
                    yield return new WaitForSecondsRealtime(offset);
                }
            }
            yield return new WaitForSecondsRealtime(loopOffset);
        }
    }

    private void Reset(Image[] images)
    {
        for (int x = 0; x < images.Length; x++)
        {
            if (images[x] != null)
            {
                animator = images[x].GetComponent<Animator>();
                if (animator != null){
                    animator.ResetTrigger("Start"); // Limpia disparadores pendientes
                    animator.Play("DefaultPauseLetters", 0, 0f);// manda a Default anim
                }
            }
        }
    }
}
