using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class HUDPauseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image hudImage;
    private Animator animator;
    private bool isPaused = false;
    private static readonly int IsHoveredHash = Animator.StringToHash("IsHovered");
    public Sprite playSprite;
    public Sprite pauseSprite;
    private bool isGameEnded = false;
    public static event Action<bool> PausaPlay;

    void Start()
    {
        hudImage = GetComponent<Image>();
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.enabled = true;
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
    }

    public void OnEnable()
    {
        // Nos suscribimos a la señal del objeto
        Bebe.OnBebeTocado += _ => isGameEnded = true;
        GameManager.Ganado += _ =>isGameEnded = true;
    }

    public void OnDisable()
    {
        // Nos suscribimos a la señal del objeto
        Bebe.OnBebeTocado -= _ => isGameEnded = false;
        GameManager.Ganado -= _ =>isGameEnded = false;
    }

    // Detecta cuándo el mouse entra en la imagen
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (animator != null)
        {
            animator.SetBool(IsHoveredHash, true);
        }
    }

    // Detecta cuándo el mouse sale de la imagen
    public void OnPointerExit(PointerEventData eventData)
    {
        if (animator != null)
        {
            animator.SetBool(IsHoveredHash, false);
        }
    }

    // Detecta cuándo se hace click en la imagen
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isGameEnded) return; // No hacer nada si el juego ha terminado
        TogglePlayPause();
    }

    private void TogglePlayPause()
    {
        if (isPaused)
        {
            Debug.Log("Imagen Clickeada. Reanudando juego.");
            Time.timeScale = 1f;
            isPaused = false;
            if (hudImage == null) return;
            hudImage.sprite = pauseSprite;
        }
        else
        {
            Debug.Log("Imagen Clickeada. Pausando juego.");
            Time.timeScale = 0f; // detener tiempo global
            isPaused = true;
            if (hudImage == null) return;
            hudImage.sprite = playSprite;
        }
        PausaPlay?.Invoke(isPaused);
    }
}