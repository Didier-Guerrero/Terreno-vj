using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public Animator ani;
    public Quaternion angulo;
    public float grado;

    public GameObject target;

    // Variables de velocidad
    public float walkSpeed = 0.5f; // Velocidad de caminar
    public float runSpeed = 1.0f;  // Velocidad de correr
    public float rotationSpeed = 1.0f; // Velocidad de rotación

    // Variables de vida
    public int vida = 3;

    // Variable para controlar si el enemigo está muerto
    private bool isDead = false;

    // Prefab de partículas
    public GameObject particleEffect;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.Find("I_1_Brown");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            comportamiento();
        }
    }

    public void comportamiento()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > 5)
        {
            ani.SetBool("run", false);
            cronometro += 1 * Time.deltaTime;
            if (cronometro >= 4)
            {
                rutina = Random.Range(0, 2);
                cronometro = 0;
            }
            switch (rutina)
            {
                case 0:
                    ani.SetBool("walk", false);
                    break;
                case 1:
                    grado = Random.Range(0, 360);
                    angulo = Quaternion.Euler(0, grado, 0);
                    rutina++;
                    break;
                case 2:
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, rotationSpeed * Time.deltaTime);
                    transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
                    ani.SetBool("walk", true);
                    break;
            }
        }
        else
        {
            var lookpos = target.transform.position - transform.position;
            lookpos.y = 0;
            var rotation = Quaternion.LookRotation(lookpos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            ani.SetBool("walk", false);
            ani.SetBool("run", true);
            transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "hacha" || collision.gameObject.tag == "arma")
        {
            Debug.Log("impacto");
            vida--;

            // Instanciar partículas en el punto de impacto
            Instantiate(particleEffect, collision.contacts[0].point, Quaternion.identity);

            if (vida <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        isDead = true;
        ani.SetBool("die", true);
        ani.SetBool("walk", false);
        ani.SetBool("run", false);
        // Puedes agregar otros efectos aquí, como detener el movimiento, sonido de muerte, etc.
    }
}
