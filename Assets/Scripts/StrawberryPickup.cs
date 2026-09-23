using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class StrawberryPickup : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            Destroy(gameObject);

        }
    }
}
