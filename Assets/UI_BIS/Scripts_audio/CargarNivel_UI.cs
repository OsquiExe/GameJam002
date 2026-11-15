using UnityEngine;
using UnityEngine.SceneManagement;

public class CargarNivel_UI : MonoBehaviour
{
   
public void CargarFacil()
    {
        PlayerPrefs.SetString("Dificultad", "Facil");
        SceneManager.LoadScene("Prueba Enemigos");
    }
}
