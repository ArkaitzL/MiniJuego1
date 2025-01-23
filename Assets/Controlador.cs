using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Controlador : MonoBehaviour
{

    [SerializeField] private Generador generadorSP = new Generador();
    [SerializeField] private Tablero tableroSP = new Tablero();
    [SerializeField] private Buscar buscar = new Buscar();

    private Dictionary<float, Dictionary<float, Dado>> tablero;

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
        Tablero();
    }

    // Genera los cajones
    private async void Generar() 
    {
        // Cajones
        var tareas = new[] {
            generadorSP.IniciarDados(1),
            generadorSP.IniciarDados(-1)
        };

        var resultados = await Task.WhenAll(tareas);

        // Asignar los resultados a los HashSet correspondientes
        dadosL.New(resultados[0]);
        dadosR.New(resultados[1]);
    }

    // Generar el tablero
    private void Tablero() 
    {
        // Tablero
        tablero = tableroSP.Generar();
    }

    // Detectar nuevo elemento en la lista
    private void OnAdd(Dado dado, ObservableHashSet<Dado> lista) 
    {
        Arrastrar arrastrar = dado.transform.GetComponent<Arrastrar>();
        if (arrastrar == null) return;

        // Funcion que se ejecuta al terminar de mover el dado
        arrastrar.Callback = () =>
        {
            if (usando != null) usando.arrastrar.PosicionInicial();

            usando = new Usado(
                dado,
                arrastrar,
                lista
            );
        };

        arrastrar.Iniciado = true;
    }

    // Jugar Turno
    public async void Turno() 
    {
        if (usando == null) return;

        // Cambiarlo de lista
        usando.lista.Remove(usando.dado);
        Vector3 p = usando.dado.transform.position;
        tablero[p.x][p.y] = usando.dado;

        // Buscar sumas
        buscar.Inicializar(tablero);
        List<Dado> dados = buscar.ComprobarSuma(new Vector2(p.x, p.y));
        Debug.Log("DADOS: " + dados.Count);

        if (dados.Count > 0)
        {
            foreach (Dado d in dados)
            {
                // Quitar dado del tablero
                tablero[d.transform.position.x][d.transform.position.y] = null;
                Destroy(d.transform.gameObject);
                // ANIMAR .......

                // Accion
                //d.palo.Ejecutar( , d.puntuacion);
            }
        }

        // Crear nuevo dado en la lista
        Transform dadoTR = usando.dado.transform;
        bool arrastrable = usando.arrastrar.Arrastrable;
        Dado dado = await generadorSP.CrearDado(
            usando.dado.posicionInicial,
            dadoTR.name,
            dadoTR.parent,
            arrastrable
         );
        usando.lista.Add(dado);

        // Fijarlo al tablero
        usando.arrastrar.Arrastrable = false;

        // Pasar turno
        // ...

        usando = null;
    }

    public class Usado {
        public Dado dado;
        public Arrastrar arrastrar;
        public ObservableHashSet<Dado> lista;

        public Usado(Dado dado, Arrastrar arrastrar, ObservableHashSet<Dado> lista)
        {
            this.dado = dado;
            this.arrastrar = arrastrar;
            this.lista = lista;
        }
    }
}
