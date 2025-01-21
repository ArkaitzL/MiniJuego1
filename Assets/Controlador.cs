using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Controlador : MonoBehaviour
{

    [SerializeField] private Generador generador = new Generador();

    private ObservableHashSet<Dado> dadosL = new ObservableHashSet<Dado>();
    private ObservableHashSet<Dado> dadosR = new ObservableHashSet<Dado>();
    private Usado usando = null;

    private void Start()
    {
        // Detectar nuevo elemento en la lista
        dadosL.OnElementAdded += OnAdd;
        dadosR.OnElementAdded += OnAdd;

        // Genera el escenario
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
        dadosL.New(resultados[0]);
        dadosR.New(resultados[1]);
    }

    // Detectar nuevo elemento en la lista
    private void OnAdd(Dado dado) 
    {
        Arrastrar arrastrar = dado.transform.GetComponent<Arrastrar>();
        if (arrastrar == null) return;

        // Funcion que se ejecuta al terminar de mover el dado
        arrastrar.Callback = () =>
        {
            if (usando != null) usando.arrastrar.PosicionInicial();

            usando = new Usado(
                dado,
                arrastrar
            );
        };
    }

    public class Usado {
        public Dado dado;
        public Arrastrar arrastrar;

        public Usado(Dado dado, Arrastrar arrastrar)
        {
            this.dado = dado;
            this.arrastrar = arrastrar;
        }
    }
}
