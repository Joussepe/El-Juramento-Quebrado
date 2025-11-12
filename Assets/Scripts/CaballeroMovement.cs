using UnityEngine;
using UnityEngine.InputSystem; // Importante

public class CaballeroMovement : MonoBehaviour
{
    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private float Horizontal;
    private bool Grounded;
    private bool recibiendoDanio;
    public bool muerto;

    public float Speed; // velocidad configurable en el inspector
    public float JumpForce; // fuerza de salto
    public float ReboteForce; //fuerza de rebote
    public int vida = 3;


    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Mantener el parámetro "muerto" sincronizado en el Animator
        Animator.SetBool("muerto", muerto);

        if (!muerto)
        {
            // Guardar el input en la variable de clase
            Horizontal = Input.GetAxisRaw("Horizontal");
            // Hacemos que gire el personaje a donde camina
            if (Horizontal < 0.0f) transform.localScale = new Vector3(-3.5f, 3.5f, 1.0f);
            else if (Horizontal > 0.0f) transform.localScale = new Vector3(3.5f, 3.5f, 1.0f);

            Animator.SetBool("Caminando", Horizontal != 0.0f);
            Animator.SetBool("recibeDanio", recibiendoDanio);

            Debug.DrawRay(transform.position, Vector3.down * 0.55f, Color.red);
            if (Physics2D.Raycast(transform.position, Vector3.down, 0.55f).collider != null)
            {
                Grounded = true;
            }
            else Grounded = false;

            if (Input.GetKeyDown(KeyCode.W) && Grounded)
            {
                Jump();
            }

            //Atacar
            if (Input.GetMouseButtonDown(0))
            {
                Animator.SetBool("Atacando", true);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Animator.SetBool("Atacando", false);
            }
        }
        else
        {
            // Si está muerto, aseguramos que no haya velocidad ni animación de caminar
            Rigidbody2D.linearVelocity = Vector2.zero;
            Animator.SetBool("Caminando", false);
        }
    }

    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
    }

    public void RecibeDanio(Vector2 direccion, int cantDanio)
    {
        if (!recibiendoDanio)
        {
            recibiendoDanio = true;
            vida -= cantDanio;
            if (vida <= 0)
            {
                muerto = true;
                Rigidbody2D.linearVelocity = Vector2.zero; // Detiene todo movimiento
                Animator.SetBool("Caminando", false);      // Detiene la animación de caminar si estaba activa
                // ya dejamos el Animator.SetBool("muerto", muerto) en Update para reflejarlo
            }
            if (!muerto)
            {
                Vector2 rebote = new Vector2(transform.position.x - direccion.x, 0.5f).normalized;
                Rigidbody2D.AddForce(rebote * ReboteForce, ForceMode2D.Impulse);
            }
        }
    }

    public void DesactivaDanio()
    {
        recibiendoDanio = false;
        Rigidbody2D.linearVelocity = Vector2.zero;
    }

    void FixedUpdate()
    {
        // Usar linearVelocity (reemplazo moderno de velocity)
        Rigidbody2D.linearVelocity = new Vector2(Horizontal * Speed, Rigidbody2D.linearVelocity.y);
    }
}
