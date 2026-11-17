using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager instancia;
    public AudioSource audioEfectos;

    void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        instancia = this;
        DontDestroyOnLoad(gameObject);

        if (audioEfectos == null)
            audioEfectos = GetComponent<AudioSource>();
    }

    public void ReproducirEfecto(AudioClip clip)
    {
        if (clip != null && audioEfectos != null)
            audioEfectos.PlayOneShot(clip);
    }

    public void CambiarVolumenEfectos(float nuevoVolumen)
    {
        if (audioEfectos != null)
            audioEfectos.volume = nuevoVolumen;
    }
}