using UnityEngine;

// DADO: Todo lo que compoen el dado
public class Dado 
{
    public Transform transform;
    public int puntuacion;
    public PaloValor palo;
}

//PALO: El palo del dado
public class Palo
{
    public static PaloValor espada = new Espada();
    public static PaloValor corazon = new Corazon();
    public static PaloValor escudo = new Escudo();

    private static readonly PaloValor[] acciones = {
        espada,
        corazon,
        escudo
    };

    public static PaloValor RandomValue() 
    {
        int num =  Random.Range(0, acciones.Length);
        return acciones[num];
    }
}

public abstract class PaloValor
{
    public abstract int Index { get; }
    public abstract void Ejecutar(CLPersonaje personaje);
}

public class Espada : PaloValor
{
    public override int Index => 0;
    public override void Ejecutar(CLPersonaje personaje)
    {

    }
}

public class Corazon : PaloValor
{
    public override int Index => 1;
    public override void Ejecutar(CLPersonaje personaje)
    {

    }
}

public class Escudo : PaloValor
{
    public override int Index => 2;
    public override void Ejecutar(CLPersonaje personaje)
    {

    }
}

