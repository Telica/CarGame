using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Describe las propiedades de los Items.
/// </summary>
public class Item : MonoBehaviour
{
    [Header ("Item Properties")]
    public GameObject prefab;
    public int itemScore;

    /// <summary>
    /// OnTrigger con el auto jugado, da puntaje y destruye el item de la escena.
    /// </summary>
    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            GameManagement.score += itemScore;
            Destroy(prefab);
        }
    }
}
