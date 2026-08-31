using UnityEngine;

public class HUDConsumable : MonoBehaviour
{
    public Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        // Mantenemos el componente activo, pero la máquina estará en 'Idle'
        if (animator != null)
        {
            animator.enabled = true;
            animator.updateMode = AnimatorUpdateMode.UnscaledTime; // Útil para UI/Pausa
        }
    }
}