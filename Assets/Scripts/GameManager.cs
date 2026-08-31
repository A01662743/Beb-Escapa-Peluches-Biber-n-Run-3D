using System.Collections;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public float tiempo = 0f; //timmer
    public int n = 1; //contador de spawn consumibles
    private int ConsumObt = 0; //Consumibles obtenidos
    public int maxConsumibles = 3;
    public GameObject[] dangerPrefabs;
    public Consumable consumable; // Objetos consumibles
    public float xMin = 1f; //limite spawn x
    public float xMax = 38f; 
    public float zMin = 1f; //limite spawn z
    public float zMax = 18f;
    public float yPosition = 10f; // Altura spawn constante Y
    public float baseSpawnTime = 2.0f; // Tiempo base entre spawns
    private Coroutine timeCoroutine;
    public static event Action<float> Ganado;

    void Start()
    {
        // Iniciar el bucle de apariciones
        StartCoroutine(SpawnRoutine());
    }

    private void OnEnable()
    {
        //desuscripción de prevención
        Bebe.OnBebeTocado -= SlowDownTime;
        Consumable.OnObjetoTocado -= Ganar;
        // Nos suscribimos a la señal del objeto
        Bebe.OnBebeTocado += SlowDownTime;
        Consumable.OnObjetoTocado += Ganar;
    }

    private void OnDisable()
    {
        // Nos desuscribimos para evitar fugas de memoria
        Bebe.OnBebeTocado -= SlowDownTime;
        Consumable.OnObjetoTocado -= Ganar;
    }

    void Update() {
        tiempo += Time.deltaTime;
        if (tiempo >= (12f* n) && n <= maxConsumibles) {
            Vector3 randomPos = GenerateRandomPosition(1f);
            Consumable scriptConsumable = Instantiate(consumable, randomPos, Quaternion.identity);
            scriptConsumable.num = n-1;
            Debug.Log("Consumible " + n + " spawneado");
            n++;
        }
    }

    IEnumerator SpawnRoutine()
    {

        int x = 0;
        while (true)
        {
            // identificar objeto a spawnear
            int index = x % dangerPrefabs.Length;

            // 2. Generar posición  y rotaciones aleatorias
            Vector3 randomPos = GenerateRandomPosition(yPosition);
            Quaternion randomRotation = GenerateRandomRotation();

            // 3. Crear el objeto
            Instantiate(dangerPrefabs[index], randomPos, randomRotation);

            // 1. Esperar X segundos
            yield return new WaitForSeconds(baseSpawnTime* Mathf.Pow(0.9f, x)+ 0.2f);

            x++;
        }
    }

    Vector3 GenerateRandomPosition(float y)
    {
        float randomX = UnityEngine.Random.Range(xMin, xMax);
        float randomZ = UnityEngine.Random.Range(zMin, zMax);

        return new Vector3(randomX, y, randomZ);
    }

    Quaternion GenerateRandomRotation()
    {
        float randomRotation1 = UnityEngine.Random.Range(0f, 360f);
        float randomRotation2 = UnityEngine.Random.Range(0f, 360f);
        float randomRotation3 = UnityEngine.Random.Range(0f, 360f);
        return Quaternion.Euler(randomRotation1, randomRotation2, randomRotation3);
    }

    public void SlowDownTime(float duration)
    {
        if (timeCoroutine != null) StopCoroutine(timeCoroutine);
        timeCoroutine = StartCoroutine(SmoothTimeScaleToZero(duration));
    }

    public void Ganar(int num)
    {
        ConsumObt++;
        if (ConsumObt == maxConsumibles)
        {
            Ganado?.Invoke(10f);
            SlowDownTime(1f);
        }
    }

    private IEnumerator SmoothTimeScaleToZero(float duration)
    {
        float startScale = Time.timeScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // OBLIGATORIO: Usar unscaledDeltaTime porque Time.deltaTime se reduce conforme disminuye timeScale
            elapsed += Time.unscaledDeltaTime;
            
            float t = elapsed / duration;
            Time.timeScale = Mathf.Lerp(startScale, 0f, t);
            
            // Mantiene actualizado fixedDeltaTime para evitar saltos bruscos en las físicas (Rigidbodies)
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            yield return null;
        }

        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
    }

}
