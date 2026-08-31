using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class AnimacionCamaraOrbit : MonoBehaviour
{
    private CinemachineCamera virtualCamera;
    private CinemachineInputAxisController axisController;
    private float panTotal = 540f;
    private float tiltTotal = -20f;

    private CinemachineOrbitalFollow orbitalFollow;
    private int botonMouse = 0;
    private float duracionTransicion = 1f;
    private float anguloInicialX = 180.0f;
    private float anguloInicialY = -20.0f;
    private float anguloPlayY = 25f;
    private float anguloPlayX = 0.0f;
    private float radioInicial = 1.0f;
    private float radioNormal = 3.0f;

    private void Awake()
    {
        if (virtualCamera == null) 
            virtualCamera = GetComponent<CinemachineCamera>();

        if (axisController == null) 
            axisController = GetComponent<CinemachineInputAxisController>();

        // Obtenemos el Orbital Follow que controla el giro alrededor del target
        if (virtualCamera != null)
            orbitalFollow = virtualCamera.GetComponent<CinemachineOrbitalFollow>();

        // 3. Ignorar/Desactivar el recentrado al instanciarse o iniciar
        CamaraInicio();
    }

    public void CamaraInicio()
    {
        if (orbitalFollow == null) return;

        // 1. Apagar Recentering al inicio
        orbitalFollow.HorizontalAxis.Recentering.Enabled = false;

        // 2. Colocar la cámara en la distancia/radio de inicio
        orbitalFollow.Radius = radioInicial;

        // 3. Ajustar ángulo horizontal y vertical inicial
        orbitalFollow.HorizontalAxis.Value = anguloInicialX;
        orbitalFollow.VerticalAxis.Value = anguloInicialY;
    }

    public void ActivarCamaraNormal()
    {
        if (orbitalFollow == null) return;

        // Transición suave del radio y activación del recentrado
        StartCoroutine(RutinaRestablecerCamara());
    }

    private IEnumerator RutinaRestablecerCamara()
    {
        
        float tiempo = 0f;
        float radioActual = orbitalFollow.Radius;

        // 2. Animar el alejamiento progresivo hasta el radio normal
        while (tiempo < duracionTransicion)
        {
            tiempo += Time.unscaledDeltaTime; // Usa unscaledDeltaTime por si el juego está pausado
            float t = Mathf.Clamp01(tiempo / duracionTransicion);
            float tSuave = Mathf.SmoothStep(0f, 1f, t);

            orbitalFollow.Radius = Mathf.Lerp(radioActual, radioNormal, tSuave);
            orbitalFollow.VerticalAxis.Value = Mathf.Lerp(anguloInicialY, anguloPlayY, tSuave);
            orbitalFollow.HorizontalAxis.Value = Mathf.Lerp(anguloInicialX, anguloPlayX, tSuave);

            yield return null;
        }

        orbitalFollow.VerticalAxis.Value = anguloPlayY;
        orbitalFollow.Radius = radioNormal;
        // 1. Activar el Recentering automático
        orbitalFollow.HorizontalAxis.Recentering.Enabled = true;
    }

    private void OnEnable()
    {
        HUDStartAnim.Play += ActivarCamaraNormal;
        GameManager.Ganado += IniciarGiroAnimado;
    }

    private void OnDisable()
    {
        HUDStartAnim.Play -= ActivarCamaraNormal;
        GameManager.Ganado -= IniciarGiroAnimado;
    }

    private void Update()
    {
        if (axisController != null)
        {
            // Solo activa la lectura del mousepad MIENTRAS mantengas el clic presionado
            axisController.enabled = Input.GetMouseButton(botonMouse);
        }
    }

    public void IniciarGiroAnimado(float duracion)
    {
        StartCoroutine(RutinaGiro(duracion));
    }

    private IEnumerator RutinaGiro(float duracion)
    {

        if (orbitalFollow != null)
        {
            orbitalFollow.HorizontalAxis.Recentering.Enabled = false;
        }

        // 1. Desactivar el control por mouse/mousepad
        if (axisController != null)
        {
            axisController.enabled = false;
        }

        // Validación de seguridad
        if (orbitalFollow == null)
        {
            Debug.LogError("Error: No se encontró el componente CinemachineOrbitalFollow en la cámara.");
            yield break;
        }

        float tiempoTranscurrido = 0f;

        // Leemos el eje horizontal actual desde el Orbital Follow
        float panInicial = orbitalFollow.HorizontalAxis.Value;
        float panObjetivo = panInicial + panTotal;

        float tiltInicial = orbitalFollow.HorizontalAxis.Value;
        float tiltObjetivo = tiltInicial + tiltTotal;

        // 2. Transición de rotación
        while (tiempoTranscurrido < duracion)
        {
            tiempoTranscurrido += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(tiempoTranscurrido / (duracion - (duracion/9)));
            float tSuave = Mathf.SmoothStep(0f, 1f, t);

            // Actualizamos la posición del eje horizontal de la órbita
            orbitalFollow.HorizontalAxis.Value = Mathf.Lerp(panInicial, panObjetivo, tSuave);
            orbitalFollow.VerticalAxis.Value = Mathf.Lerp(tiltInicial, tiltObjetivo, tSuave);
            orbitalFollow.Radius = Mathf.Lerp(radioNormal, radioInicial, tSuave);

            yield return null;
        }

        orbitalFollow.HorizontalAxis.Value = panObjetivo;
        orbitalFollow.VerticalAxis.Value = tiltObjetivo;
        orbitalFollow.Radius = radioInicial;

        // 3. Restaurar control si fuera necesario
        if (axisController != null && Input.GetMouseButton(0))
        {
            axisController.enabled = true;
        }
    }
}