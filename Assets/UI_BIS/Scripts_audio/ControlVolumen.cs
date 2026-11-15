using UnityEngine;
using UnityEngine.UI;

public class ControlVolumen : MonoBehaviour
{
    public AudioSource music;
    public AudioSource efects;
    public Slider sliderMusic;
    public Slider sliderEfects;
    void Start()
    {
        music.volume = sliderMusic.value;
        efects.volume = sliderEfects.value;
    }
    public void CambiarMusica(float value)
    {
        music.volume = value;
    }
    public void CambiarEfectos(float value)
    {
        efects.volume = value;
        //para que se escuche al moverlo
        if (!efects.isPlaying)
            efects.Play();
    }
}
