using UnityEngine;

public class GestionePiano : MonoBehaviour
{

    [SerializeField, Range(0, 1)] private int pianoIniziale = 0;

    private SpriteRenderer spriteRenderer;

    private int layerPlayerP0;
    private int layerPlayerP1;

    public int PianoCorrente { get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        layerPlayerP0 = LayerMask.NameToLayer("PlayerP0");
        layerPlayerP1 = LayerMask.NameToLayer("PlayerP1");

        if (spriteRenderer == null || layerPlayerP0 == -1 || layerPlayerP1 == -1)
        {
            Debug.LogError("GestionePiano: manca la SpriteRendere oppure una Physics Layer PlayerP0 o PlayerP1");
            enabled = false;
            return;
        }
        
        CambiaPiano(pianoIniziale);
    }

    public void CambiaPiano(int piano)
    {
        if (piano < 0 || piano > 1)
        {
            Debug.LogError("GestionePiano: il piano deve essere 0 o 1");
            return;
        }

        gameObject.layer = piano == 0 ? layerPlayerP0 : layerPlayerP1;

        spriteRenderer.sortingLayerName = piano == 0 ? "Personaggio_P0" : "Personaggio_P1";
        PianoCorrente = piano;

    }
}
