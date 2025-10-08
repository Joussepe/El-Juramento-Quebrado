using UnityEngine;
using UnityEngine.InputSystem; // Importante

public class CaballeroMovement : MonoBehaviour
{
    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private float Horizontal;
    private bool Grounded;

    public float Speed; // velocidad configurable en el inspector
    public float JumpForce; // fuerza de salto

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Guardar el input en la variable de clase
        Horizontal = Input.GetAxisRaw("Horizontal");
        // Hacemos que gire el personaje a donde camina
        if (Horizontal < 0.0f) transform.localScale = new Vector3(-3.5f, 3.5f, 1.0f);
        else if (Horizontal > 0.0f) transform.localScale = new Vector3(3.5f, 3.5f, 1.0f);

        Animator.SetBool("Caminando", Horizontal != 0.0f);

        Debug.DrawRay(transform.position, Vector3.down * 0.55f, Color.red);
        if(Physics2D.Raycast(transform.position,Vector3.down, 0.55f).collider != null)
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

    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
    }

    void FixedUpdate()
    {
        // Usar linearVelocity (reemplazo moderno de velocity)
        Rigidbody2D.linearVelocity = new Vector2(Horizontal * Speed, Rigidbody2D.linearVelocity.y);
    }
}
