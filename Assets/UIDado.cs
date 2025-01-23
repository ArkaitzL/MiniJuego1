using UnityEngine;

public class UIDado : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] puntosRend;
    [SerializeField] private SpriteRenderer paloRend;
    [SerializeField] private Sprite corazon, espada, escudo; 

    private static readonly int[][] valores = {
        new int[] { 0 },
        new int[] { 1, 2 },
        new int[] { 0, 1, 2 },
        new int[] { 1, 2, 3, 4 },
        new int[] { 0, 1, 2, 3, 4 },
        new int[] { 1, 2, 3, 4, 5, 6 },
    };

    public void Pintar(Dado dado) 
    {

        dado.AccionPalo(Palo.espada, () => { paloRend.sprite = espada; });
        dado.AccionPalo(Palo.corazon, () => { paloRend.sprite = corazon; });
        dado.AccionPalo(Palo.escudo, () => { paloRend.sprite = escudo; });

        // Cantidad
        foreach (int v in valores[dado.puntuacion-1])  {
            puntosRend[v].enabled = true;
        }
    }

}
