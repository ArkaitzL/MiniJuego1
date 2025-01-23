using UnityEngine;
using System;
using TMPro;

[Serializable] public class CLPersonaje
{
    [Header("Info")]
    [SerializeField] private bool local;
    [SerializeField] private bool ia;
    [SerializeField] private int vidaInicial;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI vidaTxt;
    [SerializeField] private TextMeshProUGUI escudoTxt;
    [SerializeField] private TextMeshProUGUI vidaMaxTxt;

    private int vida, escudo;

    private IA iaSP = new IA();

    public bool Local { get => local; set => local = value; }
    public bool Ia { get => ia; set => ia = value; }

    public void Iniciar()
    {
        vida = vidaInicial;

        vidaTxt.text = vida.ToString();
        escudoTxt.text = "0";
        vidaMaxTxt.text = $"/{vidaInicial}";
    }

    public void ActivarIA() 
    {
        // AQUI LA iA HACE SU MAGIA
        Debug.Log("IA...");
        Controlador.instancia.turnoSP.Pasar();
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
        const float proteccion = 0.4f; // Porcentaje de daño absorbido por el escudo

        // Calcular daño al escudo y vida con redondeo correcto
        int dañoEscudo = Mathf.RoundToInt(ataque * proteccion);
        int dañoVida = ataque - dañoEscudo;

        // Reducir el daño del escudo primero
        if (escudo >= dañoEscudo)
        {
            escudo -= dañoEscudo;
        }
        // Si el escudo no es suficiente, la diferencia se transfiere a la vida
        else
        {
            dañoVida += (dañoEscudo - escudo);
            escudo = 0;
        }

        // Reducir la vida
        vida -= dañoVida;
        if (vida < 0) vida = 0; // Evitar valores negativos

        // Actualizar la UI
        vidaTxt.text = vida.ToString();
        escudoTxt.text = escudo.ToString();
    }

}
