using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;


public class HUDStartAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    private Image hudImage;
    private Animator animator;
    private static readonly int IsHoveredHash = Animator.StringToHash("IsHovered");
    public static event Action Play;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    public void OnPointerClick(PointerEventData eventData)
    {
        Play?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
