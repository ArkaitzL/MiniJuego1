using UnityEngine;

public class UIDado : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] puntos;
    [SerializeField] private Sprite[] palosSP; 
    [SerializeField] private SpriteRenderer palo;

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
        // Palo
        palo.sprite = palosSP[dado.palo.Index];

        // Cantidad
        foreach (int v in valores[dado.puntuacion-1])  {
            puntos[v].enabled = true;
        }
    }

}
