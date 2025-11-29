using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuDificultadManager : MonoBehaviour
{
    [Header("Botones de Dificultad")]
    public Button botonFacil;
    public Button botonNormal;
    public Button botonDificil;
    public Button botonVolver;
    
    [Header("UI")]
    public TMP_Text textoDescripcion;

    private void Start()
    {
        // Configurar botones
        botonFacil.onClick.AddListener(() => SeleccionarDificultad(0));
        botonNormal.onClick.AddListener(() => SeleccionarDificultad(1));
        botonDificil.onClick.AddListener(() => SeleccionarDificultad(2));
        botonVolver.onClick.AddListener(Volver);
        
        // Mostrar dificultad actual
        MostrarDificultadActual();
    }
    
    public void SeleccionarDificultad(int nivelDificultad)
    {
        ReproducirSonidoBoton();
        
        // Guardar dificultad en PlayerPrefs
        PlayerPrefs.SetInt("Dificultad", nivelDificultad);
        PlayerPrefs.Save();
        
        Debug.Log($"✅ Dificultad guardada: {ObtenerNombreDificultad(nivelDificultad)}");
        
        // Mostrar confirmación
        if (textoDescripcion != null)
        {
            textoDescripcion.text = $"¡Dificultad {ObtenerNombreDificultad(nivelDificultad)} seleccionada!\n\nPulsa VOLVER para aplicar los cambios.";
        }
    }
    
    private void MostrarDificultadActual()
    {
        int dificultadActual = PlayerPrefs.GetInt("Dificultad", 1);
        
        if (textoDescripcion != null)
        {
            textoDescripcion.text = $"Dificultad actual: {ObtenerNombreDificultad(dificultadActual)}\n\nSelecciona una nueva dificultad:";
        }
    }
    
    private string ObtenerNombreDificultad(int nivel)
    {
        switch (nivel)
        {
            case 0: return "FÁCIL";
            case 1: return "NORMAL";
            case 2: return "DIFÍCIL";
            default: return "NORMAL";
        }
    }
    
    public void Volver()
    {
        ReproducirSonidoBoton();
        
        int dificultadSeleccionada = PlayerPrefs.GetInt("Dificultad", 1);
        Debug.Log($"🎯 Volviendo con dificultad: {ObtenerNombreDificultad(dificultadSeleccionada)}");
        
        // Volver al menú principal
        SceneManager.LoadScene("MenuPrincipal");
    }
    
    // Método opcional para volver directamente al juego
    public void AplicarYVolverAlJuego()
    {
        ReproducirSonidoBoton();
        
        int dificultadSeleccionada = PlayerPrefs.GetInt("Dificultad", 1);
        Debug.Log($"🎯 Aplicando dificultad: {ObtenerNombreDificultad(dificultadSeleccionada)}");
        
        // Volver directamente al juego
        SceneManager.LoadScene("JuegoPrincipal");
    }
    
    private void ReproducirSonidoBoton()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ReproducirClickBoton();
        }
    }
}