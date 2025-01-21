using System;
using System.Threading.Tasks;
using UnityEngine;

public class Arrastrar : MonoBehaviour
{
    [SerializeField] private LayerMask mesaLayer; 
    [SerializeField] private AnimationCurve movimientoCurve;
    [SerializeField] private float tiempoAnimacion = 0.5f;

    private Action callback;
    private bool isDragging;
    private Vector3 offset, posicionInicial;
    private Camera camara;


    const string CASILLA_ID = "Casilla";

    public Action Callback { get => callback; set => callback = value; }

    private void Start()
    {
        camara = Camera.main;
    }

    private void OnMouseUp()
    {
        isDragging = false;

        // Detecta si se solt� en una posici�n v�lida
        Collider2D hitCollider = Physics2D.OverlapPoint(transform.position, mesaLayer);

        if (hitCollider != null && hitCollider.CompareTag(CASILLA_ID))
        {
            // Si est� en una posici�n v�lida, animar hacia el destino
            AnimMover(hitCollider.transform.position);

            // Avisa de que ya est� movido
            Callback?.Invoke();
            return;
        }

        // Si no est� en una posici�n v�lida, regresar a la posici�n inicial con animaci�n
        PosicionInicial();
    }

    private void OnMouseDown()
    {
        // Inicia el arrastre
        isDragging = true;
        posicionInicial = transform.position;

        // Calcula el offset entre el objeto y el cursor
        offset = transform.position - Mover();
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;

        // Actualiza la posici�n del objeto al seguir el cursor
        transform.position = Mover() + offset;
    }

    private Vector3 Mover()
    {
        // Obtiene la posici�n del cursor en el mundo
        Vector3 mousePos = camara.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; 
        return mousePos;
    }

    public void PosicionInicial() {
        AnimMover(posicionInicial);
    }

    private async void AnimMover(Vector3 destino)
    {
        float tiempoTranscurrido = 0;
        Vector3 posicionInicial = transform.position;

        while (tiempoTranscurrido < tiempoAnimacion)
        {
            tiempoTranscurrido += Time.deltaTime;
            float t = tiempoTranscurrido / tiempoAnimacion;

            // Aplicar la curva de animaci�n
            float curvaT = movimientoCurve.Evaluate(t);
            transform.position = Vector3.Lerp(posicionInicial, destino, curvaT);

            await Task.Yield(); 
        }

        // Asegurarse de que termina en la posici�n exacta
        transform.position = destino;
    }
}
