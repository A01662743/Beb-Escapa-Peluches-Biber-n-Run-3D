using System.Collections;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    // Se ejecuta al collisionar
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Mat"))
        {
            StartCoroutine(WaitAndDestroy());
        }
    }
    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(1.0f); // Espera 1 segundos antes de destruir el objeto
        Destroy(gameObject); // Destruye este objeto
    }

}