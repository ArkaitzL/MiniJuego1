using System;
using System.Collections.Generic;
using UnityEngine;

// DADO: Todo lo que compoen el dado
public class Dado 
{
    public Transform transform;
    public Vector2 posicionInicial;
    public int puntuacion;
    public Palo palo;

    public bool AccionPalo(Palo palo, Action accion) 
    {
        if (this.palo != palo) return false;

        accion?.Invoke();
        return false;
    }

    public override bool Equals(object obj)
    {
        return obj is Dado dado &&
               EqualityComparer<Transform>.Default.Equals(transform, dado.transform) &&
               posicionInicial.Equals(dado.posicionInicial) &&
               puntuacion == dado.puntuacion &&
               palo == dado.palo;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(transform, posicionInicial, puntuacion, palo);
    }
}

//PALO: El palo del dado
public enum Palo
{
    espada,
    corazon,
    escudo
}