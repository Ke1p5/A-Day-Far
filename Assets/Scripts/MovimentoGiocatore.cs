using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class MovimentoGiocatore : MonoBehaviour
{

    [SerializeField] private float velocita = 5f;

    private Rigidbody2D corpo;
    private Animator animatore;
    private SpriteRenderer spriteRenderer;
    private InputAction azioneMovimento;
    private Vector2 direzione;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        corpo = GetComponent<Rigidbody2D>();
        animatore = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        azioneMovimento = InputSystem.actions.FindAction("Player/Move", true);
    }



    // Update is called once per frame
    private void Update()
    {
        direzione = azioneMovimento.ReadValue<Vector2>();
        direzione = Vector2.ClampMagnitude(direzione, 1f);

        bool inMovimento = direzione.sqrMagnitude > 0.001f;
        if (inMovimento)
        {
            Vector2 orientamento = direzione.normalized;
            animatore.SetFloat("MoveX", orientamento.x);
            animatore.SetFloat("MoveY", orientamento.y);
        }

        animatore.SetBool("IsMoving", inMovimento);

        if (direzione.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (direzione.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void FixedUpdate()
    {
        corpo.linearVelocity = direzione * velocita;
    }
}
