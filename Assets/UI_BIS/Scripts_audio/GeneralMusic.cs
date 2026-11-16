using UnityEngine;

public class GeneralMusic : MonoBehaviour
{

    public static GeneralMusic instancia;

    void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;

        DontDestroyOnLoad(gameObject);
    }
}


