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
        // Busca si alguna opcion suma puntos
        // ...

        return (Vector2.zero, new Dado());
    }

    private (Vector2? pos, Dado dado) BuscarSuma() 
    {
        // Busca si alguna opcion suma puntos
        // ...

        return (null, null);
    }
}
