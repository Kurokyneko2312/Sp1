using UnityEngine;
using TMPro;

public class PlayerQuest : MonoBehaviour
{
    [SerializeField] private int strawberriesToCollect = 10;
    [SerializeField] private TMP_Text strawberryText;
    [SerializeField] private AudioClip pickupSoundEffect;

    private int strawberries = 0;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        strawberryText.text = strawberries.ToString();
    }

    public void AddStrawberry()
    {
        strawberries++;
        strawberryText.text = strawberries.ToString();

        if (audioSource != null && pickupSoundEffect != null)
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(pickupSoundEffect);
        }
    }

    public int GetStrawberries()
    {
        return strawberries;
    }

    public int GetStrawberriesToCollect()
    {
        return strawberriesToCollect;
    }
}