using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Controlador : MonoBehaviour
{

    [SerializeField] private Generador generador = new Generador();
    private HashSet<Dado> dadosL;
    private HashSet<Dado> dadosR;

    private void Start()
    {
        Generar();
    }

    // Genera el escenario
    private async void Generar() 
    {
        // Cajones
        var tareas = new[] {
            generador.IniciarDados(1),
            generador.IniciarDados(-1)
        };

        var resultados = await Task.WhenAll(tareas);

        // Asignar los resultados a los HashSet correspondientes
        dadosL = new HashSet<Dado>(resultados[0]);
        dadosR = new HashSet<Dado>(resultados[1]);
    }

}
