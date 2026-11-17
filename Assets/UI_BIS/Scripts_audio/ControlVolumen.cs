using UnityEngine;
using UnityEngine.UI;

public class ControlVolumen : MonoBehaviour
{
    public Slider sliderVolumen;
    public AudioSource musica;

    void Start()
    {
        if (sliderVolumen == null)
        {
            sliderVolumen = GameObject.Find("Slider_Volumen").GetComponent<Slider>();
        }

        // Aquí está la clave: busca la música que NO se destruyó
        GeneralMusic fondo = FindFirstObjectByType<GeneralMusic>();
        if (fondo != null)
        {
            musica = fondo.GetComponent<AudioSource>();
        }

        if (sliderVolumen == null || musica == null) return;

        float volumenGuardado = 0.7f;
        if (PlayerPrefs.HasKey("VolumenMusica"))
            volumenGuardado = PlayerPrefs.GetFloat("VolumenMusica");

        sliderVolumen.value = volumenGuardado;
        musica.volume = volumenGuardado;

        sliderVolumen.onValueChanged.RemoveAllListeners();
        sliderVolumen.onValueChanged.AddListener(CambiarVolumen);
    }

    public void CambiarVolumen(float nuevoVolumen)
    {
        // Si la música que teníamos ya se destruyó, la vuelve a buscar
        if (musica == null)
        {
            GeneralMusic fondo = FindFirstObjectByType<GeneralMusic>();
            if (fondo != null)
                musica = fondo.GetComponent<AudioSource>();
            else
                return;
        }

        musica.volume = nuevoVolumen;
        PlayerPrefs.SetFloat("VolumenMusica", nuevoVolumen);
        PlayerPrefs.Save();
    }
}