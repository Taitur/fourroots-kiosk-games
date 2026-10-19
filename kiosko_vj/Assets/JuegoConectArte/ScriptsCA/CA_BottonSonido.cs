using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CA_ButtonSound : MonoBehaviour
{
    public enum SoundType { Sound1, Sound2 }

    [SerializeField] private SoundType sound = SoundType.Sound1;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Play);
    }

    private void Play()
    {
        if (CA_SFX.Instance == null) return;

        if (sound == SoundType.Sound1) CA_SFX.Instance.PlayButtonSound1();
        else CA_SFX.Instance.PlayButtonSound2();
    }
}