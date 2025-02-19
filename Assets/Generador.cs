using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Threading.Tasks;

[Serializable] public class Generador
{
    [SerializeField] private int dadosCantidad = 3;
    [SerializeField] private float dadosGap = 0.5f;
    [SerializeField] private float generacionEspera= 0.5f;

    [SerializeField] private GameObject dado2D;

    const int NUM_MAX_DADO = 6;

    public async Task<HashSet<Dado>> IniciarDados(Vector2 posicion, bool local, int lado)
    {
        // Validar que el prefab exista
        if (dado2D == null) {
            Debug.LogError("El prefab 'dado2D' no est� asignado.");
            return null;
        }

        // Crear el padre y posici�n inicial
        GameObject padre = new GameObject($"Jugador");
        Vector2 posicionInicial = posicion;

        // Almacenar los dados generados
        HashSet<Dado> dadosSet = new HashSet<Dado>();

        for (int i = 0; i < dadosCantidad; i++)
        {
            // Calcular la posici�n del dado actual
            Vector2 posicionDado = new Vector2(
                posicionInicial.x + (i * (1 + dadosGap)) * lado,
                posicionInicial.y
            );

            // Crear el dado asincr�nicamente
            Dado nuevoDado = await CrearDado(posicionDado, $"Dado-{i}", padre.transform, local);

            // A�adir el dado al conjunto
            dadosSet.Add(nuevoDado);

            // Esperar antes de crear el siguiente
            await Task.Delay(Mathf.RoundToInt(generacionEspera * 1000));
        }

        return dadosSet;
    }

    public async Task<Dado> CrearDado(Vector2 posicion, string nombre, Transform parent, bool jugador)
    {
        // Instanciar el GameObject del dado
        GameObject dadoGO = GameObject.Instantiate(dado2D);
        dadoGO.transform.position = new Vector3(posicion.x, posicion.y, 0);
        dadoGO.transform.SetParent(parent);
        dadoGO.name = nombre;

        // Elegir un palo random
        Array valores = Enum.GetValues(typeof(Palo));
        int indice = Random.Range(0, valores.Length);
        Palo palo = (Palo)valores.GetValue(indice);

        // Crear los datos del dado
        Dado datos = new Dado  {
            puntuacion = Random.Range(1, NUM_MAX_DADO + 1),
            posicionInicial = dadoGO.transform.position,
            palo = palo,
            transform = dadoGO.transform
        };

        // Pintar el dado
        dadoGO.GetComponent<UIDado>().Pintar(datos);

        // Permitir arrastrar o no
        dadoGO.GetComponent<Arrastrar>().Arrastrable = jugador;


        // Animacion
        // await ....

        return datos;
    }
}
