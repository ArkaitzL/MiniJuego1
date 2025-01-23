using UnityEngine;
using System;

[Serializable] public class CLPersonaje
{
    [SerializeField] private bool local;
    [SerializeField] private bool ia;

    private IA iaSP = new IA();

    public bool Local { get => local; set => local = value; }
    public bool Ia { get => ia; set => ia = value; }

    public void ActivarIA() 
    {
        // AQUI LA iA HACE SU MAGIA
        Debug.Log("IA...");
        Controlador.instancia.turnoSP.Pasar();
    }

    public void Accion(Dado dado) { 
    
    }

}
