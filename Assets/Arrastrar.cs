using UnityEngine;

public class Arrastrar : MonoBehaviour
{
    private bool isDragging;
    private Vector3 offset;
    private Camera camara;

    private void Start()
    {
        camara = Camera.main;
    }

    private void OnMouseUp()
    {
        // Termina el arrastre
        isDragging = false;
    }

    private void OnMouseDown()
    {
        // Inicia el arrastre
        isDragging = true;
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
}
