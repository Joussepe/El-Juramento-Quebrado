using JetBrains.Annotations;
using UnityEngine;

public class EnemiScript : MonoBehaviour
{
    public Transform Caballero;
    public float speed = 2f;
    public float detectionRange = 5f;
    public float fuerzaRebote = 7f;
    public int vida = 3;

    private Rigidbody2D rb;
    private Vector3 originalScale;
    private bool recibiendoDanio;
    private bool playerVivo;
    private bool muerto;
    private Animator anim;

    void Start()
    {
        playerVivo = true;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // ✅ Añadir esta línea
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (playerVivo && !muerto)
        {
            Movimiento();
        }
        anim.SetBool("muerto", muerto);
    }

    private void Movimiento()
    {
        if (Caballero == null) return;

        float distance = Vector2.Distance(transform.position, Caballero.position);

        Vector3 dir = Caballero.position - transform.position;
        if (dir.x >= 0f)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

        if (distance <= detectionRange)
        {
            Vector3 target = new Vector3(Caballero.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Caballero"))
        {
            Vector2 direccionDanio = new Vector2(transform.position.x, 0);
            CaballeroMovement playerScript = collision.gameObject.GetComponent<CaballeroMovement>();

            playerScript.RecibeDanio(direccionDanio, 1);
            playerVivo = !playerScript.muerto;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Espada"))
        {
            Vector2 direccionDanio = new Vector2(collision.transform.position.x, 0);
            RecibeDanio(direccionDanio, 1);
        }
    }

    public void RecibeDanio(Vector2 direccion, int cantDanio)
    {
        if (!recibiendoDanio)
        {
            vida -= cantDanio;
            recibiendoDanio = true;
            
            if(vida <= 0)
            {
                muerto = true;
            }
            else
            {
                Vector2 rebote = new Vector2(transform.position.x - direccion.x, 0.5f).normalized;
                rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse);

                Invoke(nameof(ResetDanio), 0.2f);
            }
        }
    }

    public void ResetDanio()
    {
        recibiendoDanio = false;
    }

    public void EliminarCuerpo()
    {
        Destroy(gameObject);
    }
}
