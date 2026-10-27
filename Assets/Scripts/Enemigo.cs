using System.Collections;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [Header("Atributos del Enemigo (Vida y Daño)")]
    public int vidaMaxima = 3;
    private int vidaActual;
    public int dañoAlJugador = 1;

    [Header("Configuración de Rangos")]
    public float patrolSpeed = 1.5f;     // Velocidad al patrullar
    public float chaseSpeed = 2.5f;      // Velocidad al perseguir a Kalipa
    public float patrolDistance = 3.0f;  // Distancia a patrullar hacia cada lado
    public float detectionRange = 5.0f;  // Distancia para detectar a Kalipa y perseguir
    public float attackRange = 1.2f;     // Distancia para detenerse y golpear
    public float attackCooldown = 1.5f;  // Tiempo entre cada ataque

    [Header("Referencias")]
    public Transform player;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Variables internas de control
    private Vector2 startPos;
    private bool movingRight = true;
    private float nextAttackTime = 0f;
    private bool estaMuerto = false;

    private void Start()
    {
        vidaActual = vidaMaxima;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        startPos = transform.position;

        // Búsqueda automática del jugador por Tag si la casilla está vacía
        BuscarJugador();
    }

    private void Update()
    {
        if (estaMuerto) return;

        // Si no se asignó o perdió la referencia de Kalipa, la vuelve a buscar
        if (player == null || player == transform)
        {
            BuscarJugador();
            Patrullar();
            return;
        }

        // Calculamos la distancia 2D (ignorando el eje Z para evitar errores)
        Vector2 enemyPos2D = transform.position;
        Vector2 playerPos2D = player.position;
        float distanceToPlayer = Vector2.Distance(enemyPos2D, playerPos2D);

        // --- ESTADOS DE LA IA ---
        if (distanceToPlayer <= attackRange)
        {
            // 1. Estado: ATACAR
            if (Time.time >= nextAttackTime)
            {
                Atacar();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else if (distanceToPlayer <= detectionRange)
        {
            // 2. Estado: PERSEGUIR A KALIPA
            PerseguirJugador();
        }
        else
        {
            // 3. Estado: PATRULLAR (Kalipa está lejos)
            Patrullar();
        }
    }

    // --- COMPORTAMIENTOS DE LA IA ---

    private void Patrullar()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * patrolSpeed * Time.deltaTime;
            if (spriteRenderer != null) spriteRenderer.flipX = false;

            if (transform.position.x >= startPos.x + patrolDistance)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.position += Vector3.left * patrolSpeed * Time.deltaTime;
            if (spriteRenderer != null) spriteRenderer.flipX = true;

            if (transform.position.x <= startPos.x - patrolDistance)
            {
                movingRight = true;
            }
        }
    }

    private void PerseguirJugador()
    {
        Vector2 target = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, chaseSpeed * Time.deltaTime);

        // Voltear el sprite hacia Kalipa
        if (player.position.x > transform.position.x)
        {
            if (spriteRenderer != null) spriteRenderer.flipX = false;
        }
        else if (player.position.x < transform.position.x)
        {
            if (spriteRenderer != null) spriteRenderer.flipX = true;
        }
    }

    private void Atacar()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    private void BuscarJugador()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null && playerObj != gameObject)
        {
            player = playerObj.transform;
        }
    }

    // --- SISTEMA DE VIDA Y DAÑO ---

    public void RecibirDaño(int cantidad)
    {
        if (estaMuerto) return;

        vidaActual -= cantidad;
        Debug.Log(gameObject.name + " recibió daño. Vida restante: " + vidaActual);

        StartCoroutine(EfectoDaño());

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    private IEnumerator EfectoDaño()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.color = Color.white;
        }
    }

    private void Morir()
    {
        estaMuerto = true;
        Destroy(gameObject);
    }

    // --- DAÑO POR CONTACTO DIRECTO ---

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SaludJugador salud = collision.gameObject.GetComponent<SaludJugador>();
            if (salud != null)
            {
                salud.RecibirDaño(dañoAlJugador);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SaludJugador salud = collision.GetComponent<SaludJugador>();
            if (salud != null)
            {
                salud.RecibirDaño(dañoAlJugador);
            }
        }
    }
}