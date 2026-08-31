using UnityEngine;
using UnityEngine.UI; // Necesario para trabajar con UI.Image
using System;

public class HUDColorController : MonoBehaviour
{
    // Arrastra el objeto de la imagen del HUD desde el Inspector a esta casilla
    public Image[] imagenHUD;
    private Animator animator;

    private void OnEnable()
    {
        // Nos suscribimos a la señal del objeto
        Consumable.OnObjetoTocado += MaximizarColor;
    }

    private void OnDisable()
    {
        // Nos desuscribimos para evitar fugas de memoria
        Consumable.OnObjetoTocado -= MaximizarColor;
    }

    // Mostrar biberón HUD (100% visible)
    public void MaximizarColor(int cantidad)
    {
        if (imagenHUD[cantidad] != null)
        {
            imagenHUD[cantidad].color = new Color32(255, 255, 255, 255);
            animator = imagenHUD[cantidad].GetComponent<Animator>();
            if (animator != null){
                animator.SetTrigger("Start");
            }
        }
    }

    // Cambiar a cualquier otro color por código de 0 a 255
    public void CambiarColorPersonalizado(byte r, byte g, byte b)
    {
        // Se preserva la transparencia actual de la imagen
        byte alphaActual = (byte)(imagenHUD[0].color.a * 255);
        imagenHUD[0].color = new Color32(r, g, b, alphaActual);
    }
}