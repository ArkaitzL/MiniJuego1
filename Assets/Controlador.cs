using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Controlador : MonoBehaviour
{

    [SerializeField] public Generador generadorSP = new Generador();
    [SerializeField] public Tablero tableroSP = new Tablero();
    [SerializeField] public Buscar buscarSP = new Buscar();
    [SerializeField] public Turno turnoSP = new Turno();

    private Dictionary<float, Dictionary<float, Dado>> tablero;

    private Usado usando = null;

    public static Controlador instancia;

    public Dictionary<float, Dictionary<float, Dado>> Tablero { get => tablero; set => tablero = value; }

    private void Awake()
    {
        // Crea una instancia publica y unica paar que sea accesible desde cualquier lado
        if (instancia != null) {
            Debug.LogWarning($"Multiples instancias de controlador, puedes eliminar la de {gameObject.name}");
            return;
        }

        instancia = this;
    }

    private void Start()
    {
        // Genera el escenario
        Generar();
        TableroG();

        // Iniciar los personaje
        foreach (CLPersonaje personaje in turnoSP.Personajes)
        {
            personaje.Iniciar();
        }
    }

    // Genera los cajones
    private async void Generar()
    {
        // Cajones
        List<(CLPersonaje personaje, Task<HashSet<Dado>> tarea)> tareas = new List<(CLPersonaje, Task<HashSet<Dado>>)>();

        foreach (CLPersonaje personaje in turnoSP.Personajes)
        {
            // Generar posiciones
            var resultado = generadorSP.IniciarDados(personaje.Posicion, personaje.Local, personaje.Lado);
            tareas.Add((personaje, resultado));

            // Detectar nuevo elemento en la lista
            personaje.Dados.OnElementAdded += OnAdd;
        }

        // Esperar a que todas las tareas se completen
        await Task.WhenAll(tareas.Select(t => t.tarea));

        // Asignar los resultados a los HashSet correspondientes
        foreach (var (personaje, tarea) in tareas)
        {
            var dados = await tarea; // Obtener el resultado de la tarea
            personaje.Dados.New(dados); // Asignar al HashSet del personaje
        }

        // Inicia el juego
        turnoSP.Iniciar();
    }

    // Generar el tablero
    private void TableroG() 
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

    // Jugar Turno al darle al BOTON PLAY
    public void PasarTurno() 
    {
        if (usando == null) return;
        if (!turnoSP.Personaje.Local) return;

        // Pasar de turno
        turnoSP.Pasar(usando.dado);

        // Quitarlo de la lista
        usando.lista.Remove(usando.dado);

        // Fijarlo al tablero
        usando.arrastrar.Arrastrable = false;

        usando = null;
    }

    // Comprueba si se puede sumar y pasa
    public async Task Sumar(Dado dado)
    {
        // Quitarlo de la lista
        turnoSP.Personaje.Dados.Remove(dado);

        // Cambiarlo de lista
        Vector3 p = dado.transform.position;
        tablero[p.x][p.y] = dado;

        // Buscar sumas
        buscarSP.Inicializar(tablero);
        List<Dado> dados = buscarSP.ComprobarSuma(new Vector2(p.x, p.y));

        if (dados.Count > 0)
        {
            foreach (Dado d in dados)
            {
                // Quitar dado del tablero
                tablero[d.transform.position.x][d.transform.position.y] = null;
                Destroy(d.transform.gameObject);

                // ANIMAR .......

                // Accion
                turnoSP.Personaje.Accion(d);
            }
        }

        // Crear nuevo dado en la lista
        Transform dadoTR = dado.transform;
        bool arrastrable = turnoSP.Personaje.Local;
        Dado nuevoDado = await generadorSP.CrearDado(
            dado.posicionInicial,
            dadoTR.name,
            dadoTR.parent,
            arrastrable
        );
        turnoSP.Personaje.Dados.Add(nuevoDado);
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
