using UnityEngine;

public class MoveBridge : MonoBehaviour
{
    [SerializeField] private GameObject button;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetTrigger("MoveBridge");
            button.SetActive(false);
        }
    }
}
