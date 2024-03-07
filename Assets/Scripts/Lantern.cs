using UnityEngine;
using TMPro;

public class Lantern : MonoBehaviour
{
    public Transform handTransform; // Referencia al transform del objeto de la mano del jugador
    public float pickupRange = 2f; // Rango máximo de interacción con la linterna
    public float dropDistance = 2f; // Rango máximo de interacción con la linterna
    public KeyCode pickUpKey = KeyCode.E; // Tecla para recoger la linterna
    public KeyCode dropKey = KeyCode.F; // Tecla para soltar la linterna
    public TMP_Text pickupText; // Cambiado el tipo de variable a TMPro.TextMeshProUGUI

    private GameObject currentLantern; // La linterna actual que el jugador está sosteniendo

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
        currentLantern.transform.localPosition = Vector3.zero; // Ajustamos la posición local a cero
        currentLantern.transform.localRotation = Quaternion.identity; // Ajustamos la rotación local a la identidad
        //currentLantern.GetComponent<BoxCollider>().enabled = false; // Desactivamos el BoxCollider de la linterna
        currentLantern.GetComponent<Rigidbody>().isKinematic = true; // Desactivamos la física de la linterna

        // Desactivar el objeto de texto de recolección
        pickupText.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lantern"))
        {
            // Activar el objeto de texto de recolección
            Debug.Log("Entered lantern range");
            pickupText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Lantern"))
        {
            // Desactivar el objeto de texto de recolección
            Debug.Log("Exited lantern range");
            pickupText.gameObject.SetActive(false);
        }
    }

    void DropLantern()
    {
        // Obtener la posición de la mano del jugador
        Vector3 handPosition = handTransform.position;

        // Obtener la dirección de la vista del jugador
        Vector3 viewDirection = handTransform.forward;

        // Dibujar el raycast en la escena
        Debug.DrawRay(handPosition, viewDirection * dropDistance, Color.red, 2f);

        // Lanzamos un raycast desde la posición de la mano del jugador
        RaycastHit hit;
        if (Physics.Raycast(handPosition, viewDirection, out hit, dropDistance))
        {
            // Si el raycast golpea un objeto, dejamos la linterna en la posición de impacto
            currentLantern.transform.parent = null; // Eliminamos la relación de padre-hijo
            currentLantern.transform.position = hit.point - viewDirection * 0.1f; // Ajustamos la posición para evitar traspasar el objeto
        }
        else
        {
            // Si el raycast no golpea ningún objeto, dejamos la linterna un poco más adelante
            currentLantern.transform.parent = null; // Eliminamos la relación de padre-hijo
            currentLantern.transform.position = handPosition * dropDistance;
        }

        //currentLantern.GetComponent<BoxCollider>().enabled = true; // Activamos el BoxCollider de la linterna
        currentLantern.GetComponent<Rigidbody>().isKinematic = false; // Activamos la física de la linterna
        currentLantern = null; // El jugador ya no está sosteniendo la linterna
    }
}
