using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class StrawberryPickup : MonoBehaviour
{
    [SerializeField] private GameObject strawberryBomb;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerQuest>().AddStrawberry();
            Instantiate(strawberryBomb, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
