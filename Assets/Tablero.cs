using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable] public class Tablero
{

    public Dictionary<float, Dictionary<float, Dado>> Generar()
    {
        // Encuentra todos los GameObjects con la etiqueta de casilla
        GameObject[] casillas = GameObject.FindGameObjectsWithTag(IDs.CASILLA_ID);

        if (casillas.Length == 0)
        {
            Debug.LogWarning("No se encontraron casillas.");
            return null;
        }

        // Extraer las posiciones y ordenarlas
        List<Vector2> posiciones = new List<Vector2>();
        foreach (GameObject casilla in casillas)
        {
            posiciones.Add(casilla.transform.position);
        }

        // Ordenar por Y descendente, luego por X ascendente
        posiciones.Sort((a, b) =>
        {
            if (a.y != b.y) return b.y.CompareTo(a.y); // Y de mayor a menor
            return a.x.CompareTo(b.x);                // X de menor a mayor
        });

        // Crear el diccionario
        var tablero = new Dictionary<float, Dictionary<float, Dado>>();

        foreach (Vector2 posicion in posiciones)
        {
            // Verifica si ya existe el diccionario interno para la posición X
            if (!tablero.ContainsKey(posicion.x))
            {
                tablero[posicion.x] = new Dictionary<float, Dado>();
            }

            // Añade la posición Y al diccionario interno con un valor inicial de null
            tablero[posicion.x][posicion.y] = null;
        }

        return tablero;
    }

}
