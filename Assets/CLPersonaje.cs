using UnityEngine;
using System;
using TMPro;

[Serializable] public class CLPersonaje
{
    [Header("Info")]
    [SerializeField] private Vector2 posicion;
    [SerializeField] private int lado;
    [SerializeField] private bool local;
    [SerializeField] private bool ia;
    [Header("Estadisticas")]
    [SerializeField] private int vidaInicial;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI vidaTxt;
    [SerializeField] private TextMeshProUGUI escudoTxt;
    [SerializeField] private TextMeshProUGUI vidaMaxTxt;

    private ObservableHashSet<Dado> dados = new ObservableHashSet<Dado>();
    private int vida, escudo;

    private IA iaSP = new IA();

    public ObservableHashSet<Dado> Dados { get => dados; set => dados = value; }
    public Vector2 Posicion { get => posicion; set => posicion = value; }
    public bool Local { get => local; set => local = value; }
    public bool Ia { get => ia; set => ia = value; }
    public int Lado { get => lado; set => lado = value; }

    public void Iniciar()
    {
        vida = vidaInicial;

        vidaTxt.text = vida.ToString();
        escudoTxt.text = "0";
        vidaMaxTxt.text = $"/{vidaInicial}";
    }

    public void ActivarIA() 
    {
        // Cosas de IA y tal
        iaSP.Nivel1(this);
    }

    public void Accion(Dado dado) 
    {
        dado.AccionPalo(Palo.espada, () => 
        {
            // ANIMAR ...
            Controlador.instancia.turnoSP.OtroJugador().Espada(dado.puntuacion);
        });
        dado.AccionPalo(Palo.corazon, () => 
        {
            // ANIMAR ...
            vida += dado.puntuacion;
            if (vida > vidaInicial) vida = vidaInicial;
            vidaTxt.text = vida.ToString();
        });
        dado.AccionPalo(Palo.escudo, () => 
        {
            // ANIMAR ...
            escudo += dado.puntuacion;
            escudoTxt.text = escudo.ToString();
        });
    }

    public void Espada(int ataque) 
    {
        const float proteccion = 0.4f; // Porcentaje de da�o absorbido por el escudo

        // Calcular da�o al escudo y vida con redondeo correcto
        int da�oEscudo = Mathf.RoundToInt(ataque * proteccion);
        int da�oVida = ataque - da�oEscudo;

        // Reducir el da�o del escudo primero
        if (escudo >= da�oEscudo)
        {
            escudo -= da�oEscudo;
        }
        // Si el escudo no es suficiente, la diferencia se transfiere a la vida
        else
        {
            da�oVida += (da�oEscudo - escudo);
            escudo = 0;
        }

        // Reducir la vida
        vida -= da�oVida;
        if (vida < 0) vida = 0; // Evitar valores negativos

        // Actualizar la UI
        vidaTxt.text = vida.ToString();
        escudoTxt.text = escudo.ToString();
    }

}
