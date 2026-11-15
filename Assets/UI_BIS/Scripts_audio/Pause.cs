using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class Pause : MonoBehaviour
{
    public GameObject panelPausa;
    public bool estaPausado = false;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (estaPausado) Reanudar();
            else Pausar();
        }
    }
public void Pausar()
    {
        panelPausa.SetActive(true);
        Time.timeScale = 0f;
        estaPausado = true;
    }
public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Prueba Enemigos");
    }
public void Reanudar()
    {
        panelPausa.SetActive(false);
        Time.timeScale = 1f;
        estaPausado = false;
    }
public void IrAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
