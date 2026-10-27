using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Singleton para persistencia y referencia global
    public static PlayerController Instance;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    [Header("Configuración de Movimiento")]
    public float velocidad = 4.5f;
    public float fuerzaSalto = 7f;

    [Header("Detección de Suelo")]
    public Transform detectorSuelo;
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;
    private bool enSuelo;

    [Header("Configuración de Ataque")]
    public Transform puntoAtaque;
    public float radioAtaque = 0.8f;
    public LayerMask capaEnemigo;
    public int dañoAtaque = 1;

    private float movimientoX;
    private bool solicitarSalto;
    private bool solicitarAtaque;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Evita que se destruya al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Evita duplicados al regresar a la escena
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }

    void Update()
    {
        // 1. Capturar entradas
        movimientoX = Input.GetAxisRaw("Horizontal");

        // SOLO PERMITIR SALTO SI ESTÁ EN EL SUELO Y SU VELOCIDAD VERTICAL ES CASI 0 (NO ESTÁ SUBIENDO/CAYENDO)
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo && Mathf.Abs(rb.linearVelocity.y) < 0.1f)
        {
            solicitarSalto = true;
        }

        if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0))
        {
            solicitarAtaque = true;
            Atacar(); // Ejecuta la lógica del golpe
        }

        // 2. Voltear sprite y ajustar el punto de ataque
        if (movimientoX > 0)
        {
            spriteRenderer.flipX = false;
            AjustarPuntoAtaque(1);
        }
        else if (movimientoX < 0)
        {
            spriteRenderer.flipX = true;
            AjustarPuntoAtaque(-1);
        }

        // 3. Control de animaciones
        if (anim != null)
        {
            anim.SetFloat("MovimientoX", Mathf.Abs(movimientoX));
            anim.SetBool("EnSuelo", enSuelo);
            anim.SetFloat("VelocidadY", rb.linearVelocity.y);

            if (solicitarAtaque)
            {
                anim.SetTrigger("Ataque");
                solicitarAtaque = false;
            }
        }
    }

    void Atacar()
    {
        if (puntoAtaque == null) return;

        // Detectar enemigos dentro del rango de ataque
        Collider2D[] enemigosGolpeados = Physics2D.OverlapCircleAll(puntoAtaque.position, radioAtaque, capaEnemigo);

        foreach (Collider2D enemigo in enemigosGolpeados)
        {
            Enemigo scriptEnemigo = enemigo.GetComponent<Enemigo>();
            if (scriptEnemigo != null)
            {
                scriptEnemigo.RecibirDaño(dañoAtaque);
            }
        }
    }

    private void AjustarPuntoAtaque(int direccion)
    {
        if (puntoAtaque != null)
        {
            // Mantiene la distancia en X del punto de ataque orientada hacia donde mira Kalipa
            float offsetX = puntoAtaque.localPosition.x == 0 ? 0.8f : Mathf.Abs(puntoAtaque.localPosition.x);
            puntoAtaque.localPosition = new Vector3(offsetX * direccion, puntoAtaque.localPosition.y, puntoAtaque.localPosition.z);
        }
    }

    void FixedUpdate()
    {
        // 1. Detección de suelo rigurosa (ignora el collider de Kalipa)
        ComprobarSuelo();

        // 2. Movimiento horizontal
        rb.linearVelocity = new Vector2(movimientoX * velocidad, rb.linearVelocity.y);

        // 3. Salto
        if (solicitarSalto)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            
            if (anim != null)
            {
                anim.SetTrigger("Salto");
            }

            enSuelo = false;
            solicitarSalto = false;
        }
    }

    private void ComprobarSuelo()
    {
        if (detectorSuelo == null) return;

        Collider2D[] colisionadores = Physics2D.OverlapCircleAll(detectorSuelo.position, radioSuelo, capaSuelo);
        enSuelo = false;

        foreach (Collider2D col in colisionadores)
        {
            // Ignorar el propio collider de Kalipa
            if (col.gameObject != gameObject)
            {
                enSuelo = true;
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (detectorSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(detectorSuelo.position, radioSuelo);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (puntoAtaque != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(puntoAtaque.position, radioAtaque);
        }
    }
}