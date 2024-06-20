using System.Collections;
using UnityEngine;

public class SpawnHacha : MonoBehaviour
{
    public GameObject objectToSpawn; // El objeto que quieres instanciar
    public float cooldown = 2.0f; // Tiempo de cooldown en segundos
    public Animator animator; // Referencia al Animator
    public string animBoolName = "Lanzamiento"; // Nombre del booleano en el Animator

    private float lastSpawnTime; // Tiempo del �ltimo spawn

    void Update()
    {
        // Detecta si la tecla F ha sido presionada
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Obt�n el tiempo actual
            float currentTime = Time.time;

            // Comprueba si ha pasado suficiente tiempo desde el �ltimo spawn
            if (currentTime >= lastSpawnTime + cooldown)
            {
                // Instancia el objeto en la posici�n y rotaci�n del GameObject actual
                Instantiate(objectToSpawn, transform.position, transform.rotation);

                // Actualiza el tiempo del �ltimo spawn
                lastSpawnTime = currentTime;

                // Activa la animaci�n de lanzamiento
                animator.SetBool(animBoolName, true);

                // Desactiva la animaci�n despu�s de un frame para permitir futuras activaciones
                StartCoroutine(ResetAnimationBool());
            }
        }
    }

    private IEnumerator ResetAnimationBool()
    {
        // Espera un frame
        yield return null;
        // Desactiva el booleano en el Animator
        animator.SetBool(animBoolName, false);
    }
}
