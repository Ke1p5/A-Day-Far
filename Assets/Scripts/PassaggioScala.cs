using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PassaggioScala : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private int pianoDesinazione;
    private void Awake()
    {
        if (!GetComponent<BoxCollider2D>().isTrigger)
        {
            Debug.LogError("PassaggioScala: attiva Is Trigger sul BoxCollider2D.", this);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GestionePiano giocatore = collision.GetComponentInParent<GestionePiano>();

        if (giocatore != null || !giocatore.enabled)
        {
            return;
        }

        if (giocatore.PianoCorrente != pianoDesinazione)
        {
            return;    
        }

        giocatore.CambiaPiano(pianoDesinazione);
    }
}
