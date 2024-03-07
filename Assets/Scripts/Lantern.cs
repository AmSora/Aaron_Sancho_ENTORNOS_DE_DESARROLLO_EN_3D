using UnityEngine;
using TMPro;

public class Lantern : MonoBehaviour
{
    public Transform handTransform; // Referencia al transform del objeto de la mano del jugador
    public float pickupRange = 2f; // Rango m�ximo de interacci�n con la linterna
    public float dropDistance = 2f; // Rango m�ximo de interacci�n con la linterna
    public KeyCode pickUpKey = KeyCode.E; // Tecla para recoger la linterna
    public KeyCode dropKey = KeyCode.F; // Tecla para soltar la linterna
    public TMP_Text pickupText; // Cambiado el tipo de variable a TMPro.TextMeshProUGUI

    private GameObject currentLantern; // La linterna actual que el jugador est� sosteniendo

    void Update()
    {
        // Si el jugador presiona la tecla asignada para recoger la linterna, la recogemos
        if (Input.GetKeyDown(pickUpKey) && currentLantern == null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRange);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Lantern"))
                {
                    PickUpLantern(collider.gameObject);
                    break;
                }
            }
        }
        if (Input.GetKeyDown(dropKey) && currentLantern != null)
        {
            DropLantern();
        }
    }

    void PickUpLantern(GameObject lantern)
    {
        currentLantern = lantern;
        currentLantern.transform.parent = handTransform; // Hacemos que la linterna sea un hijo de la mano
        currentLantern.transform.localPosition = Vector3.zero; // Ajustamos la posici�n local a cero
        currentLantern.transform.localRotation = Quaternion.identity; // Ajustamos la rotaci�n local a la identidad
        //currentLantern.GetComponent<BoxCollider>().enabled = false; // Desactivamos el BoxCollider de la linterna
        currentLantern.GetComponent<Rigidbody>().isKinematic = true; // Desactivamos la f�sica de la linterna

        // Desactivar el objeto de texto de recolecci�n
        pickupText.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lantern"))
        {
            // Activar el objeto de texto de recolecci�n
            Debug.Log("Entered lantern range");
            pickupText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Lantern"))
        {
            // Desactivar el objeto de texto de recolecci�n
            Debug.Log("Exited lantern range");
            pickupText.gameObject.SetActive(false);
        }
    }

    void DropLantern()
    {
        // Obtener la posici�n de la mano del jugador
        Vector3 handPosition = handTransform.position;

        // Obtener la direcci�n de la vista del jugador
        Vector3 viewDirection = handTransform.forward;

        // Dibujar el raycast en la escena
        Debug.DrawRay(handPosition, viewDirection * dropDistance, Color.red, 2f);

        // Lanzamos un raycast desde la posici�n de la mano del jugador
        RaycastHit hit;
        if (Physics.Raycast(handPosition, viewDirection, out hit, dropDistance))
        {
            // Si el raycast golpea un objeto, dejamos la linterna en la posici�n de impacto
            currentLantern.transform.parent = null; // Eliminamos la relaci�n de padre-hijo
            currentLantern.transform.position = hit.point - viewDirection * 0.1f; // Ajustamos la posici�n para evitar traspasar el objeto
        }
        else
        {
            // Si el raycast no golpea ning�n objeto, dejamos la linterna un poco m�s adelante
            currentLantern.transform.parent = null; // Eliminamos la relaci�n de padre-hijo
            currentLantern.transform.position = handPosition * dropDistance;
        }

        //currentLantern.GetComponent<BoxCollider>().enabled = true; // Activamos el BoxCollider de la linterna
        currentLantern.GetComponent<Rigidbody>().isKinematic = false; // Activamos la f�sica de la linterna
        currentLantern = null; // El jugador ya no est� sosteniendo la linterna
    }
}
