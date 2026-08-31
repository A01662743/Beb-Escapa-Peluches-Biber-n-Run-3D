using System.Collections;
using UnityEngine;

public class HUDTransition : MonoBehaviour
{
     public RectTransform playHUD;
     public CanvasGroup playCanvasGroup;

     public RectTransform loseHUD;
     public CanvasGroup loseCanvasGroup;

     public RectTransform winHUD;
     public CanvasGroup winCanvasGroup;

     public RectTransform pauseHUD;
     public CanvasGroup pauseCanvasGroup;

     public RectTransform startHUD;
     public CanvasGroup startCanvasGroup;
     private float targetMaxScale = 2.5f;

    private void Awake()
    {
        // 1. Pausar el tiempo del juego antes de que inicie cualquier Frame
        Time.timeScale = 0f;
        startHUD.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        Bebe.OnBebeTocado += LoseHUDSwitch;
        HUDPauseButton.PausaPlay += PausePlay;
        GameManager.Ganado += EstadoGanar;
        HUDStartAnim.Play += EstadoGanarIniciar;
    }

    private void OnDisable()
    {
        Bebe.OnBebeTocado -= LoseHUDSwitch;
        HUDPauseButton.PausaPlay -= PausePlay;
        GameManager.Ganado -= EstadoGanar;
        HUDStartAnim.Play -= EstadoGanarIniciar;
    }

    // Coordinador: ejecuta ambas animaciones simultáneamente
    public void LoseHUDSwitch(float duration)
    {
        StartCoroutine(HideHUD(playHUD, playCanvasGroup, duration));
        StartCoroutine(ShowHUD(loseHUD, loseCanvasGroup, duration));
    }

    // FUNCIÓN 1: Agrandar y desvanecer cualquier HUD saliente
    public IEnumerator HideHUD(RectTransform hudTransform, CanvasGroup canvasGroup, float duration)
    {
        float elapsed = 0f;
        Vector3 initialScale = hudTransform.localScale;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            hudTransform.localScale = Vector3.Lerp(initialScale, Vector3.one * targetMaxScale, smoothT);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, smoothT);

            yield return null;
        }

        canvasGroup.alpha = 0f;
        hudTransform.gameObject.SetActive(false);
    }

    // FUNCIÓN 2: Emerger cualquier HUD entrante desde el centro
    public IEnumerator ShowHUD(RectTransform hudTransform, CanvasGroup canvasGroup, float duration)
    {
        hudTransform.gameObject.SetActive(true);
        hudTransform.localScale = Vector3.zero;
        canvasGroup.alpha = 0f;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            hudTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, smoothT);
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, smoothT);

            yield return null;
        }

        hudTransform.localScale = Vector3.one;
        canvasGroup.alpha = 1f;
    }

    private void PausePlay(bool IsPaused)
    {
        if (IsPaused)
        {
            // StartCoroutine(ShowHUD(pauseHUD, pauseCanvasGroup, 0.5f));
            pauseHUD.gameObject.SetActive(true);
        }
        else
        {
            pauseHUD.gameObject.SetActive(false);
        }
    }

    private void EstadoGanar(float duracion)
    {
        StartCoroutine(EstadoGanar2(duracion));
    }

    private IEnumerator EstadoGanar2(float duracion)
    {
        StartCoroutine(HideHUD(playHUD, playCanvasGroup, duracion/8));
        yield return new WaitForSecondsRealtime(duracion -(duracion/8));
        StartCoroutine(ShowHUD(winHUD, winCanvasGroup, duracion/8));
    }

    private void EstadoGanarIniciar()
    {
        StartCoroutine(HideHUD(startHUD, startCanvasGroup, 1f));
        StartCoroutine(ShowHUD(playHUD, playCanvasGroup, 1f));
        Time.timeScale = 1f;
    }

}