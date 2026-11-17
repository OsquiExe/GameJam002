using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ControlVolumen : MonoBehaviour
{
    public Slider sliderVolumen;
    public Slider sliderSFX;
    public AudioMixer myMixer;

    void Start()
    {
        if (sliderVolumen == null)
        {
            GameObject go = GameObject.Find("Slider_Volumen");
            if (go != null) sliderVolumen = go.GetComponent<Slider>();
        }
        if (sliderSFX == null)
        {
            GameObject go = GameObject.Find("Slider_SFX");
            if (go != null) sliderSFX = go.GetComponent<Slider>();
        }

        float volMusica = PlayerPrefs.GetFloat("VolumenMusica", 0.7f);
        float volSFX = PlayerPrefs.GetFloat("VolumenSFX", 0.7f);

        if (sliderVolumen != null)
        {
            sliderVolumen.SetValueWithoutNotify(volMusica);
            sliderVolumen.onValueChanged.RemoveAllListeners();
            sliderVolumen.onValueChanged.AddListener(CambiarVolumen);
        }

        if (sliderSFX != null)
        {
            sliderSFX.SetValueWithoutNotify(volSFX);
            sliderSFX.onValueChanged.RemoveAllListeners();
            sliderSFX.onValueChanged.AddListener(CambiarVolumenSFX);
        }

        AplicarAlMixer("Musica", volMusica);
        AplicarAlMixer("SFX", volSFX);
    }

    public void CambiarVolumen(float v)
    {
        AplicarAlMixer("Musica", v);
        PlayerPrefs.SetFloat("VolumenMusica", v);
        PlayerPrefs.Save();
    }

    public void CambiarVolumenSFX(float v)
    {
        AplicarAlMixer("SFX", v);
        PlayerPrefs.SetFloat("VolumenSFX", v);
        PlayerPrefs.Save();
    }

    private void AplicarAlMixer(string parametro, float v)
    {
        bool ok = myMixer.SetFloat(parametro, Mathf.Log10(Mathf.Max(v, 0.0001f)) * 20);
        if (!ok) Debug.LogWarning($"El parámetro '{parametro}' no existe en el AudioMixer");
    }
}