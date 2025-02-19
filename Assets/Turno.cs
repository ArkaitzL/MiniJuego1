using UnityEngine;
using System;
using Random = UnityEngine.Random;

[Serializable] public class Turno
{
    [SerializeField] private CLPersonaje[] personajes;
    int turno;

    public CLPersonaje[] Personajes { get => personajes; }
    public CLPersonaje Personaje { get => personajes[turno]; }

    public void Iniciar()
    {
        // Inicia los turnos
        turno = Random.Range(0, personajes.Length);
        Accion();
    }

    public async void Pasar(Dado dado) 
    {
        await Controlador.instancia.Sumar(dado);

        turno = (turno + 1) % personajes.Length;
        Accion();
    }

    private void Accion() 
    {
        // Si es el turno del jugador local 
        if (personajes[turno].Local)
        {
            // ANIMAR ...
        }
        // Si usa IA el personaje
        if (personajes[turno].Ia)
        {
            personajes[turno].ActivarIA();
        }
    }

    public CLPersonaje OtroJugador() => personajes[(turno + 1) % personajes.Length];

}
