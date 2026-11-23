using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))
]
public class MovimentoNPC : MonoBehaviour
{
    [Header("Comportamento")]
    [SerializeField] private ModalitaNPC modalita = ModalitaNPC.Fermo;
    [SerializeField] private float velocita = 2.5f;
    [SerializeField] private Vector2 direzioneIniziale = Vector2.down;

    [Header("Pattugliamento")]
    [SerializeField] private Transform[] puntiPattuglia;

    [Header("Movimemnto Casuale")]
    [SerializeField] private float raggioMovimento = 3f;
    [SerializeField] private float raggioControlloOstacoli = 0.25f;
    [SerializeField] private LayerMask ostacoli;

    [Header("Pause")]
    [SerializeField] private float pausaMinima = 1f;
    [SerializeField] private float pausaMassima = 3f;

    [Header("Precisione")]
    [SerializeField] private float distanzaArrivo = 0.1f;

    private Rigidbody2D corpo;
    private Animator animatore;
    private SpriteRenderer spriteRenderer;

    private int indicePunto = 0;
    private Vector2 centroMovimento;
    private Vector2 destinazioneCasuale;
    private bool destinazioneValida = false;
    private float tempoPausa = 0f;
    private Vector2 direzioneGuardata;

    private void Awake()
    {
        corpo = GetComponent<Rigidbody2D>();
        animatore = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (corpo == null || animatore == null)
        {
            Debug.LogError("MovimentoNPC: manca il Rigidbody2D oppure l'Animator");
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        centroMovimento = corpo.position;
        if (direzioneIniziale.sqrMagnitude > 0.1f)
        {
            direzioneGuardata = direzioneIniziale.normalized;
        } else
        {
            direzioneGuardata = Vector2.down;
        }

        AggiornaAnimazione(Vector2.zero, true);
    }

    private void FixedUpdate()
    {

        if (tempoPausa > 0f)
        {
            tempoPausa -= Time.fixedDeltaTime;
            FermaNPC();
            return;
        }

        switch (modalita)
        {
            case ModalitaNPC.Fermo:
                FermaNPC();
                break;
            case ModalitaNPC.Pattuglia:
                GestisciPattuglia();
                break;
            case ModalitaNPC.Casuale:
                GestisciMovimentoCasuale();
                break;
        }
        
        if (direzioneGuardata.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (direzioneGuardata.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void GestisciPattuglia()
    {
        if (puntiPattuglia == null || puntiPattuglia.Length == 0)
        {
            FermaNPC();
            return;
        }

        if (puntiPattuglia[indicePunto] == null)
        {
            Debug.LogError("MovimentoNPC: il punto di pattuglia all'indice " + indicePunto + " è null");
            indicePunto = (indicePunto + 1) % puntiPattuglia.Length;
            FermaNPC();
            return;
        }

        Vector2 destinazione = puntiPattuglia[indicePunto].position;

        if (MuoviVerso(destinazione))
        {
            indicePunto = (indicePunto + 1) % puntiPattuglia.Length;
            AvviaPausa();
        }
    }

    private void GestisciMovimentoCasuale()
    {
        if (!destinazioneValida)
        {
            ScegliDestinazioneCasuale();

            if (!destinazioneValida)
            {
                tempoPausa = 1f;
                FermaNPC();
                return;
            }
        }

        if (MuoviVerso(destinazioneCasuale))
        {
            destinazioneValida = false;
            AvviaPausa();
        }
    }

    private void ScegliDestinazioneCasuale()
    {
        for (int tentativi = 0; tentativi < 10; tentativi++)
        {
            Vector2 punto = centroMovimento + Random.insideUnitCircle * raggioMovimento;
            Vector2 spostamento = punto - corpo.position;

            if (spostamento.magnitude < 0.05f)
            {
                continue;
            }
            
            RaycastHit2D ostacolo = Physics2D.CircleCast(
                corpo.position, 
                raggioControlloOstacoli, 
                spostamento.normalized, 
                spostamento.magnitude,
                ostacoli
            );

            if (ostacolo.collider != null)
            {
                continue;
            }

            destinazioneCasuale = punto;
            destinazioneValida = true;
            return;
        }

        destinazioneValida = false;
    }

    private bool MuoviVerso(Vector2 destinazione)
    {
        Vector2 spostamento = destinazione - corpo.position;
        float distanza = spostamento.magnitude;

        if (distanza <= distanzaArrivo)
        {
            FermaNPC();
            return true;
        }

        Vector2 direzione = spostamento.normalized;
        float velocitaEffettiva = Mathf.Min(
            velocita,
            distanza / Time.fixedDeltaTime
        );
        corpo.linearVelocity = direzione * velocitaEffettiva;
        AggiornaAnimazione(direzione, true);
        return false;
    }

    private void FermaNPC()
    {
        corpo.linearVelocity = Vector2.zero;
        AggiornaAnimazione(Vector2.zero, false);
    }
    
    private void AvviaPausa()
    {
        tempoPausa = Random.Range(pausaMinima, pausaMassima);
        FermaNPC();
    }

    private void AggiornaAnimazione(Vector2 direzione, bool cammina)
    {
        if (cammina && direzione.sqrMagnitude > 0.001f)
        {
            direzioneGuardata = direzione.normalized;
        }

        animatore.SetFloat("DirezioneX", direzioneGuardata.x);
        animatore.SetFloat("DirezioneY", direzioneGuardata.y);
        animatore.SetBool("Cammina", cammina);
    }

}
