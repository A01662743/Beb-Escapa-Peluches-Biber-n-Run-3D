using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class HUDReplayButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Animator animator;
    private static readonly int IsHoveredHash = Animator.StringToHash("IsHovered");

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.enabled = true;
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
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
        Time.timeScale = 1f;
        // 3. Recárgala usando su nombre o su índice (buildIndex)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}