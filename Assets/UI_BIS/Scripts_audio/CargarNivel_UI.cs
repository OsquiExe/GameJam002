using UnityEngine;
using UnityEngine.SceneManagement;

public class CargarNivel_UI : MonoBehaviour
{
   
    public void CargarFacil()
    {
        PlayerPrefs.SetInt("difficulty", 1);
        SceneManager.LoadScene("Gameplay");
    }
    public void CargarNormal()
    {
        PlayerPrefs.SetInt("difficulty", 2);
        SceneManager.LoadScene("Gameplay");
    }
    public void CargarDificil()
    {
        PlayerPrefs.SetInt("difficulty", 3);
        SceneManager.LoadScene("Gameplay");
    }
}
