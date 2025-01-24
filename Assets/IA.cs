using System.Collections.Generic;
using UnityEngine;

public class IA
{
    public void Nivel1(CLPersonaje personaje) 
    {
        Debug.Log("IA...");

        // Busca si alguna opcion suma puntos
        (Vector2? pos, Dado dado) dataSuma = BuscarSuma(personaje);
        if (dataSuma.pos != null)
        {
            Colocar((Vector2)dataSuma.pos, dataSuma.dado);
            return;
        }

        // Pone uno al azar
        (Vector2 pos, Dado dado) dataRandom = BuscarRandom(personaje);
        Colocar(dataRandom.pos, dataRandom.dado);
    }

    private async void Colocar(Vector2 p, Dado dado) 
    {
        Controlador.instancia.Tablero[p.x][p.y] = dado;
        await dado.transform.GetComponent<Arrastrar>().AnimMover(new Vector3(p.x, p.y, 0));

        Controlador.instancia.turnoSP.Pasar(dado);
    }

    private (Vector2 pos, Dado dado) BuscarRandom(CLPersonaje personaje)
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
                Dado dadoNull = entryY.Value;

                if (dadoNull == null)
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

        // Seleccion un dado aleatorio del jugador
        int random = Random.Range(0, personaje.Dados.Count);

        List<Dado> listaDados = personaje.Dados.ToList();
        Dado dado = listaDados[random];

        // Si no hay nulos, retorna un valor por defecto
        return (posicion, dado);
    }

    private (Vector2? pos, Dado dado) BuscarSuma(CLPersonaje personaje) 
    {
        // Busca si alguna opcion suma puntos
        // ...

        return (null, null);
    }
}
