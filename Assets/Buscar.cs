using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

[Serializable]
public class Buscar
{
    [SerializeField] private int sumar = 10; // Valor objetivo de la suma

    private Dictionary<float, Dictionary<float, Dado>> tablero; // Tablero completo
    private List<float> xIndices; // Índices X ordenados
    private List<float> yIndices; // Índices Y ordenados

    public void Inicializar(Dictionary<float, Dictionary<float, Dado>> tablero)
    {
        this.tablero = tablero;

        // Extraer y ordenar los índices X e Y del tablero
        xIndices = tablero.Keys.OrderBy(x => x).ToList();
        yIndices = tablero.FirstOrDefault().Value?.Keys.OrderBy(y => y).ToList();
    }

    public List<Dado> ComprobarSuma(Vector2 posicionInicial)
    {
        List<Dado> resultado = new List<Dado>();

        if (yIndices == null || tablero == null)
        {
            Debug.LogWarning("El tablero no está inicializado.");
            return resultado;
        }

        // Buscar en las 4 direcciones principales y las diagonales
        resultado.AddRange(BuscarEnDireccion(posicionInicial, 1, 0));  // Horizontal derecha
        resultado.AddRange(BuscarEnDireccion(posicionInicial, 0, 1));  // Vertical arriba
        resultado.AddRange(BuscarEnDireccion(posicionInicial, 1, 1));  // Diagonal ↘
        resultado.AddRange(BuscarEnDireccion(posicionInicial, 1, -1)); // Diagonal ↗

        return resultado.Distinct().ToList(); // Eliminar duplicados si los hay
    }

    private List<Dado> BuscarEnDireccion(Vector2 posicionInicial, int dirX, int dirY)
    {
        List<Dado> resultado = new List<Dado>();
        List<Dado> temporal = new List<Dado>();

        float xActual = posicionInicial.x;
        float yActual = posicionInicial.y;

        // Buscar dados en ambas direcciones
        List<Dado> direccionPositiva = BuscarEnUnaDireccion(posicionInicial, dirX, dirY);
        List<Dado> direccionNegativa = BuscarEnUnaDireccion(posicionInicial, -dirX, -dirY);

        // Combinar los resultados de ambas direcciones
        temporal.AddRange(direccionNegativa);
        temporal.Add(GetDadoEnPosicion(posicionInicial)); // Incluir el dado en la posición inicial
        temporal.AddRange(direccionPositiva);

        // Filtrar combinaciones válidas
        for (int i = 0; i < temporal.Count; i++)
        {
            int sumaParcial = 0;
            List<Dado> combinacion = new List<Dado>();

            for (int j = i; j < temporal.Count; j++)
            {
                Dado dadoActual = temporal[j];

                if (dadoActual == null)
                {
                    break; // Interrumpir si no hay dado (conexión rota)
                }

                sumaParcial += dadoActual.puntuacion;
                combinacion.Add(dadoActual);

                if (sumaParcial == sumar && combinacion.Contains(GetDadoEnPosicion(posicionInicial)))
                {
                    resultado.AddRange(combinacion);
                    break; // Termina la búsqueda para esta combinación
                }

                if (sumaParcial > sumar)
                {
                    break; // Deja de sumar si la suma parcial excede el valor objetivo
                }
            }
        }

        return resultado;
    }

    private List<Dado> BuscarEnUnaDireccion(Vector2 posicionInicial, int dirX, int dirY)
    {
        List<Dado> acumulador = new List<Dado>();

        float xActual = posicionInicial.x;
        float yActual = posicionInicial.y;

        while (true)
        {
            // Mover en la dirección indicada
            xActual = ObtenerSiguienteIndice(xIndices, xActual, dirX);
            yActual = ObtenerSiguienteIndice(yIndices, yActual, dirY);

            // Verificar si la posición existe en el tablero
            if (!tablero.ContainsKey(xActual) || !tablero[xActual].ContainsKey(yActual))
            {
                break;
            }

            Dado dadoActual = tablero[xActual][yActual];
            if (dadoActual == null)
            {
                break; // Interrumpir si no hay dado (conexión rota)
            }

            acumulador.Add(dadoActual);
        }

        return acumulador;
    }

    private Dado GetDadoEnPosicion(Vector2 posicion)
    {
        float x = posicion.x;
        float y = posicion.y;

        if (tablero.ContainsKey(x) && tablero[x].ContainsKey(y))
        {
            return tablero[x][y];
        }

        return null;
    }

    // Obtiene el siguiente índice en una lista ordenada, considerando la dirección
    private float ObtenerSiguienteIndice(List<float> indices, float valorActual, int direccion)
    {
        int index = indices.IndexOf(valorActual);
        if (index == -1) return float.NaN;

        int nuevoIndex = index + direccion;
        if (nuevoIndex < 0 || nuevoIndex >= indices.Count) return float.NaN;

        return indices[nuevoIndex];
    }
}
