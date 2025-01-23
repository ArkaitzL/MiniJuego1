using UnityEngine;
using System;
using Random = UnityEngine.Random;

[Serializable] public class Turno
{
    [SerializeField] private CLPersonaje[] personajes;
    int turno;

    public CLPersonaje Personaje { get => personajes[turno]; }

    public void Iniciar()
    {
        // Inicia los turnos
        turno = Random.Range(0, personajes.Length);
        Accion();
    }

    public void Pasar() 
    {
        turno = (turno + 1) % personajes.Length;
        Accion();
    }

    private void Accion() 
    {
        Debug.Log(turno);
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

    public CLPersonaje ProximoJugador => personajes[(turno + 1) % personajes.Length];

}
