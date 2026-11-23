using UnityEngine;

public class StatistichePersonaggio : MonoBehaviour
{
    [Header("Attributi")]
    [SerializeField] private int vitaMassima = 100;
    [SerializeField] private int forza = 10;
    [SerializeField] private int difesa = 5;

    public int VitaAttuale { get; private set; }
    public int VitaMassima {
        get { return vitaMassima; } 
    }

    public void Awake()
    {
        vitaMassima = Mathf.Max(1, vitaMassima);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
        VitaAttuale = vitaMassima;
    }               
    
    public void SubisciDanno(int danno)
    {
        if (danno <= 0)
        {
            Debug.LogWarning("SubisciDanno: il danno deve essere maggiore di zero");
            return;
        }
        
        VitaAttuale = Mathf.Max(0, VitaAttuale - danno);
        Debug.Log(VitaAttuale);
    }

    public void Cura(int quantita)
    {
        if (quantita <= 0)
        {
            Debug.LogWarning("Cura: la quantità deve essere maggiore di zero");
            return;
        }

        VitaAttuale = Mathf.Min(VitaMassima, VitaAttuale + quantita);
        Debug.Log(VitaAttuale);
    }

    [ContextMenu("Test - Subisci 20 di danno")]
    private void TestDanno()
    {
        SubisciDanno(20);
    }

    [ContextMenu("Test - Cura 20 punti")]
    private void TestCura()
    {
        Cura(20);
    }
}
