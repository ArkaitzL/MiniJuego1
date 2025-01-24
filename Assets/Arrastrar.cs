using System;
using System.Threading.Tasks;
using UnityEngine;

public class Arrastrar : MonoBehaviour
{
    [SerializeField] private LayerMask mesaLayer; 
    [SerializeField] private AnimationCurve movimientoCurve;
    [SerializeField] private float tiempoAnimacion = 0.5f;

    private bool arrastrable = true;
    private bool iniciado = false;
    private Action callback;
    private bool isDragging;
    private Vector3 offset, posicionInicial;
    private Camera camara;
    private Animator animator;


    public bool Arrastrable { get => arrastrable; set => arrastrable = value; }
    public Action Callback { get => callback; set => callback = value; }
    public bool Iniciado { get => iniciado; set => iniciado = value; }

    private void Start()
    {
        camara = Camera.main;
        animator = GetComponent<Animator>();
    }

    private void OnMouseUp()
    {
        if (!arrastrable || !iniciado) return;

        isDragging = false;

        // Detecta si se soltó en una posición válida
        Collider2D hitCollider = Physics2D.OverlapPoint(transform.position, mesaLayer);

        if (hitCollider != null && hitCollider.CompareTag(IDs.CASILLA_ID))
        {
            // Si está en una posición válida, animar hacia el destino
            AnimMover(hitCollider.transform.position);
            animator.SetBool("Moviendo", false);

            // Avisa de que ya está movido
            Callback?.Invoke();
            return;
        }

        // Si no está en una posición válida, regresar a la posición inicial con animación
        PosicionInicial();
        animator.SetBool("Moviendo", false);
    }

    private void OnMouseDown()
    {
        if (!arrastrable || !iniciado) return;

        // Inicia el arrastre
        isDragging = true;
        posicionInicial = transform.position;

        // Calcula el offset entre el objeto y el cursor
        offset = transform.position - Mover();

        animator.SetBool("Moviendo", true);
    }

    private void OnMouseDrag()
    {
        if (!arrastrable || !iniciado) return;
        if (!isDragging) return;

        // Actualiza la posición del objeto al seguir el cursor
        transform.position = Mover() + offset;
    }

    private Vector3 Mover()
    {
        // Obtiene la posición del cursor en el mundo
        Vector3 mousePos = camara.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; 
        return mousePos;
    }

    public async void PosicionInicial() {
        await  AnimMover(posicionInicial);
    }

    public async Task AnimMover(Vector3 destino)
    {
        float tiempoTranscurrido = 0;
        Vector3 posicionInicial = transform.position;

        while (tiempoTranscurrido < tiempoAnimacion)
        {
            tiempoTranscurrido += Time.deltaTime;
            float t = tiempoTranscurrido / tiempoAnimacion;

            // Aplicar la curva de animación
            float curvaT = movimientoCurve.Evaluate(t);
            transform.position = Vector3.Lerp(posicionInicial, destino, curvaT);

            await Task.Yield();
        }

        // Asegurarse de que termina en la posición exacta
        transform.position = destino;
    }
}
