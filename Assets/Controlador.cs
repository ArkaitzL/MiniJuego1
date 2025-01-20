using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Controlador : MonoBehaviour
{

    [SerializeField] private Generador generador = new Generador();
    private ObservableHashSet<Dado> dadosL = new ObservableHashSet<Dado>();
    private ObservableHashSet<Dado> dadosR = new ObservableHashSet<Dado>();

    private void Start()
    {
        // Genera el escenario
        Generar();

        // Detectar nuevo elemento en la lista
        dadosL.OnElementAdded += OnAdd;
        dadosL.OnElementAdded += OnAdd;
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
        dadosL = new ObservableHashSet<Dado>(resultados[0]);
        dadosR = new ObservableHashSet<Dado>(resultados[1]);
    }

    // Detectar nuevo elemento en la lista
    private void OnAdd(Dado dado) 
    {
        Arrastrar arrastrar = dado.transform.GetComponent<Arrastrar>();
        if (arrastrar == null) return;

        // Funcion que se ejecuta al terminar de mover el dado
        arrastrar.Callback = () =>
        {

        };
    }
}
