using System.Collections;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Bebe : MonoBehaviour
{
    public float speed = 5.0f;
    public float turnSpeed = 140.0f;
    public float forwardInput;
    public float horizontalInput;
    public Animator animator;
    private float duration = 3f;
    public static event Action<float> OnBebeTocado;
    private Action suscripcionGanar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Collider>().enabled = true;
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = true;
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
    }

    private void OnEnable()
    {
        GameManager.Ganado += EstadoGanar;
        HUDStartAnim.Play += EstadoStart;
    }

    private void OnDisable()
    {
        GameManager.Ganado -= EstadoGanar;
        HUDStartAnim.Play -= EstadoStart;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");
        
        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);

        if (animator != null)
        {
            animator.SetFloat("SpeedMultiplier", forwardInput);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Danger"))
        {
            OnBebeTocado?.Invoke(duration);
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider, true);

            GameObject Consumable = GameObject.FindWithTag("Consumable");
            if (Consumable != null)
            {
                Collider colliderConsumable = Consumable.GetComponent<Collider>();

                // 3. Le indicas a Unity que ignore la colisión entre tu collider y el de B
                if (colliderConsumable != null)
                {
                    Physics.IgnoreCollision(GetComponent<Collider>(), colliderConsumable, true);
                }
            }
        }
    }

    private void EstadoGanar(float duracion)
    {
        GetComponent<Animator>().SetFloat("WinSpeed", 7/duracion);
        GetComponent<Animator>().SetTrigger("Win");
        StartCoroutine(EstadoGanar2(duracion));
        return;
    }

    private IEnumerator EstadoGanar2(float dur)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        GetComponent<Collider>().enabled = false;

        Vector3 posicionInicial = transform.position;
        Vector3 posicionObjetivo = new Vector3(posicionInicial.x, 15f, posicionInicial.z);
        float tiempo = 0f;

        while (tiempo < 1f)
        {
            tiempo += Time.unscaledDeltaTime/dur;
            
            // Interpolación lineal entre la posición inicial y el destino
            transform.position = Vector3.Lerp(posicionInicial, posicionObjetivo, tiempo);
            
            yield return null; // Espera al siguiente fotograma
        }

        // Asegura que quede exactamente en Y = 10 al finalizar
        transform.position = posicionObjetivo;
    }

    private void EstadoStart()
    {
        GetComponent<Animator>().SetTrigger("Start");
    }

}