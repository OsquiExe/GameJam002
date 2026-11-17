using UnityEngine;

public class GeneralMusic : MonoBehaviour
{

    public static GeneralMusic instancia;

    public AudioSource music;
    public AudioSource effects;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);

            //music = transform.Find("Music").GetComponent<AudioSource>();
            //effects = transform.Find("Efects").GetComponent<AudioSource>();

            if(music != null)
                music.volume = PlayerPrefs.GetFloat("VolumenMusica", 0.75f);

            if (effects != null)
                effects.volume = PlayerPrefs.GetFloat("VolumenEfectos", 0.50f);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void CambiarMusica(float value)
    {
        if (music != null)
        {
            music.volume = value;
            PlayerPrefs.SetFloat("VolumenMusica", value);
            PlayerPrefs.Save();
        }
           ;

    }
    public void CambiarEfectos(float value)
    {
        if(effects != null)
        {
            effects.volume = value;
            PlayerPrefs.SetFloat("VolumenEfectos", value);
            PlayerPrefs.Save();

        } 
    }
    public void ProbarEfecto()
    {
        if (effects != null && effects.clip != null)
        {
            effects.PlayOneShot(effects.clip);
        }
    }
}


