using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ControlVolumen : MonoBehaviour
{

    public Slider sliderMusic;
    public Slider sliderEfects;

    void OnEnable()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (sliderMusic != null)
        {
            sliderMusic.value = PlayerPrefs.GetFloat("VolumenMusica", 0.75f);
            sliderMusic.onValueChanged.RemoveAllListeners();
            sliderMusic.onValueChanged.AddListener(CambiarMusica);
        }

        if (sliderEfects != null)
        {
            sliderEfects.value = PlayerPrefs.GetFloat("VolumenEfectos", 0.50f);
            sliderEfects.onValueChanged.RemoveAllListeners();
            sliderEfects.onValueChanged.AddListener(CambiarEfectos);

            EventTrigger trigger = sliderEfects.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = sliderEfects.gameObject.AddComponent<EventTrigger>();

            trigger.triggers.Clear();

            EventTrigger.Entry soltar = new EventTrigger.Entry();
            soltar.eventID = EventTriggerType.PointerUp;
            soltar.callback.AddListener((data) => { SoltarEfectos(); });
            trigger.triggers.Add(soltar);
        }
    }
    public void CambiarMusica(float value)
    {
       if (GeneralMusic.instancia != null)
            GeneralMusic.instancia.CambiarMusica(value);
    }
    public void CambiarEfectos(float value)
    {
    if (GeneralMusic.instancia != null)
        GeneralMusic.instancia.CambiarEfectos(value);

    }
    public void SoltarEfectos()
    {
        if (GeneralMusic.instancia != null)
            GeneralMusic.instancia.ProbarEfecto();
    }
}
