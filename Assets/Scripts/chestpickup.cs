using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ChestPickup : MonoBehaviour
{
    [SerializeField] private GameObject CoinBurst;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerQuest>().AddStrawberry();
            Instantiate(CoinBurst, transform.position, Quaternion.identity);
            
        }
    }
}
