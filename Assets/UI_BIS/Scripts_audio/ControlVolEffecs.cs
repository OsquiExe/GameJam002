using UnityEngine;
using UnityEngine.UI;

public class ControlVolumenEfectos : MonoBehaviour
{
    public Slider sliderEfectos;

    void Start()
    {
        if (sliderEfectos == null)
        {
            sliderEfectos = GameObject.Find("Slider_Efectos").GetComponent<Slider>();
        }

        if (sliderEfectos == null) return;

        float volumenGuardado = 0.5f;
        if (PlayerPrefs.HasKey("VolumenEfectos"))
            volumenGuardado = PlayerPrefs.GetFloat("VolumenEfectos");

        sliderEfectos.value = volumenGuardado;

        if (EffectsManager.instancia != null)
            EffectsManager.instancia.CambiarVolumenEfectos(volumenGuardado);

        sliderEfectos.onValueChanged.RemoveAllListeners();
        sliderEfectos.onValueChanged.AddListener(CambiarVolumenEfectos);
    }

    public void CambiarVolumenEfectos(float nuevoVolumen)
    {
        if (EffectsManager.instancia != null)
            EffectsManager.instancia.CambiarVolumenEfectos(nuevoVolumen);

        PlayerPrefs.SetFloat("VolumenEfectos", nuevoVolumen);
        PlayerPrefs.Save();
    }
}