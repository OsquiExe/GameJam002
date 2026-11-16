
using UnityEngine;
using UnityEngine.UI;

public class ControlVolumen : MonoBehaviour
{
    public Slider sliderVolumen;
    public AudioSource musica;

    private void Start()
    {
        float volumenGuardado = 0.7f;

        if (PlayerPrefs.HasKey("VolumenMusica"))
        {
            volumenGuardado = PlayerPrefs.GetFloat("VolumenMusica");
        }

        sliderVolumen.value = volumenGuardado;
        musica.volume = volumenGuardado;

        sliderVolumen.onValueChanged.AddListener(CambiarVolumen);

    }

    public void CambiarVolumen(float newVolumen)
    {
        musica.volume = newVolumen;
        PlayerPrefs.SetFloat("VolumenMusica", newVolumen);
        PlayerPrefs.Save();

    }
}
