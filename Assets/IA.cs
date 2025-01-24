using System.Collections.Generic;
using UnityEngine;

public class IA
{
    public void Nivel1() 
    {
        Debug.Log("IA...");

        // Busca si alguna opcion suma puntos
        (Vector2? pos, Dado dado) dataSuma = BuscarSuma();
        if (dataSuma.pos != null)
        {
            Colocar((Vector2)dataSuma.pos, dataSuma.dado);
            return;
        }

        // Pone uno al azar
        (Vector2 pos, Dado dado) dataRandom = BuscarRandom();
        Colocar(dataRandom.pos, dataRandom.dado);
    }

    private void Colocar(Vector2 p, Dado dado) 
    {
        Controlador.instancia.Tablero[p.x][p.y] = dado;
    }

    private (Vector2 pos, Dado dado) BuscarRandom()
    {
        // Busca si alguna opción suma puntos
        Dictionary<float, Dictionary<float, Dado>> tablero = Controlador.instancia.Tablero;

        // Lista para almacenar las posiciones de los Dados nulos
        List<Vector2> dadosNulos = new List<Vector2>();

        foreach (var entryX in tablero)
        {
            float x = entryX.Key;
            foreach (var entryY in entryX.Value)
            {
                float y = entryY.Key;
                Dado dado = entryY.Value;

                if (dado == null)
                {
                    dadosNulos.Add(new Vector2(x, y));
                }
            }
        }

        // Selecciona un dado aleatorio de la lista de nulos
        Vector2 posicion = new Vector2();

        if (dadosNulos.Count > 0)
        {
            var randomIndex = Random.Range(0, dadosNulos.Count);
            posicion = dadosNulos[randomIndex];
        }

        if (true)
        {

        }

        // Si no hay nulos, retorna un valor por defecto
        return (posicion, new Dado());
    }

    private (Vector2? pos, Dado dado) BuscarSuma() 
    {
        // Busca si alguna opcion suma puntos
        // ...

        return (null, null);
    }
}
