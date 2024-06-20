using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public Animator ani;
    public Transform eje;

    public bool inground;
    private RaycastHit hit;
    public float distance;
    public Vector3 v3;

    public float attackCooldown = 2.0f; // Tiempo de cooldown para el ataque en segundos
    private float lastAttackTime; // Tiempo del último ataque
    public string attackAnimBoolName = "attack"; // Nombre del booleano de ataque en el Animator

    public float lanzamientoCooldown = 2.0f; // Tiempo de cooldown para el lanzamiento en segundos
    private float lastLanzamientoTime; // Tiempo del último lanzamiento
    public string lanzamientoAnimBoolName = "Lanzamiento"; // Nombre del booleano de lanzamiento en el Animator

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position + v3, Vector3.up * -1 * distance);
    }

    void Move()
    {
        Vector3 rotateTargetZ = eje.transform.forward;
        rotateTargetZ.y = 0;

        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotateTargetZ), 0.3f);
            var dir = transform.forward * speed * Time.fixedDeltaTime;
            dir.y = rb.velocity.y;
            rb.velocity = dir;
            ani.SetBool("run", true);
        }
        else
        {
            if (inground)
            {
                rb.velocity = Vector3.zero;
            }
            ani.SetBool("run", false);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotateTargetZ * -1), 0.3f);
            var dir = transform.forward * speed * Time.fixedDeltaTime;
            dir.y = rb.velocity.y;
            rb.velocity = dir;
            ani.SetBool("run", true);
        }

        Vector3 rotateTargetX = eje.transform.right;
        rotateTargetX.y = 0;

        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotateTargetX), 0.3f);
            var dir = transform.forward * speed * Time.fixedDeltaTime;
            dir.y = rb.velocity.y;
            rb.velocity = dir;
            ani.SetBool("run", true);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotateTargetX * -1), 0.3f);
            var dir = transform.forward * speed * Time.fixedDeltaTime;
            dir.y = rb.velocity.y;
            rb.velocity = dir;
            ani.SetBool("run", true);
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        if (Physics.Raycast(transform.position + v3, transform.up * -1, out hit, distance))
        {
            if (hit.collider.tag == "piso")
            {
                inground = true;
            }
        }
        else
        {
            inground = false;
        }

        // Detecta el clic izquierdo para activar la animación de ataque
        if (Input.GetMouseButtonDown(0)) // 0 es el botón izquierdo del ratón
        {
            float currentTime = Time.time;
            if (currentTime >= lastAttackTime + attackCooldown)
            {
                ani.SetBool(attackAnimBoolName, true);
                lastAttackTime = currentTime;
                StartCoroutine(ResetAttackAnimation());
            }
        }

        // Detecta si la tecla F ha sido presionada para activar la animación de lanzamiento
        if (Input.GetKeyDown(KeyCode.F))
        {
            float currentTime = Time.time;
            if (currentTime >= lastLanzamientoTime + lanzamientoCooldown)
            {
                ani.SetBool(lanzamientoAnimBoolName, true);
                lastLanzamientoTime = currentTime;
                StartCoroutine(ResetLanzamientoAnimation());
            }
        }
    }

    private IEnumerator ResetAttackAnimation()
    {
        // Espera el tiempo necesario para que la animación se complete
        yield return new WaitForSeconds(ani.GetCurrentAnimatorStateInfo(0).length);
        ani.SetBool(attackAnimBoolName, false);
    }

    private IEnumerator ResetLanzamientoAnimation()
    {
        // Espera el tiempo necesario para que la animación se complete
        yield return new WaitForSeconds(ani.GetCurrentAnimatorStateInfo(0).length);
        ani.SetBool(lanzamientoAnimBoolName, false);
    }
}
