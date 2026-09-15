using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;
    void Start()
    {
        Destroy(gameObject, timeToDestroy);

    }


}
