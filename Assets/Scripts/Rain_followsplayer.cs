using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain_followsplayer : MonoBehaviour
{
    public GameObject character; // Referencia al GameObject que seguir� la lluvia

    void LateUpdate()
    {
        // Obtenemos la posici�n del personaje
        Vector3 characterPosition = character.transform.position;

        // Mantenemos la altura de la lluvia pero actualizamos su posici�n en el plano XZ
        transform.position = new Vector3(characterPosition.x, transform.position.y, characterPosition.z);
    }
}