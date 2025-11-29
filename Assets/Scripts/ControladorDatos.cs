using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ControladorDatos : MonoBehaviour
{
    public TMP_InputField campoNombre;
    public TMP_InputField campoEdad;

    void Start()
    {
        // Cargar datos guardados si existen
        if (PlayerPrefs.HasKey("NombreJugador"))
        {
            campoNombre.text = PlayerPrefs.GetString("NombreJugador");
        }
        if (PlayerPrefs.HasKey("EdadJugador"))
        {
            campoEdad.text = PlayerPrefs.GetString("EdadJugador");
        }
    }

    // MÉTODO PRINCIPAL: Guardar y volver al menú principal
    public void GuardarYVolverAlMenuPrincipal()
    {
        string nombre = campoNombre.text;
        string edad = campoEdad.text;

        // Validar datos
        if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(edad))
        {
            Debug.Log("❌ Por favor, completa todos los campos");
            return;
        }

        // Guardar datos
        PlayerPrefs.SetString("NombreJugador", nombre);
        PlayerPrefs.SetString("EdadJugador", edad);
        PlayerPrefs.Save();

        Debug.Log("✅ Datos guardados: " + nombre + ", " + edad + " años");
        
        // Reproducir sonido de confirmación
        ReproducirSonidoBoton();
        
        // Volver al menú principal
        SceneManager.LoadScene("MenuPrincipal");
    }

    void ReproducirSonidoBoton()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ReproducirClickBoton();
        }
    }

    // MÉTODO ALTERNATIVO: Solo para limpiar campos
    public void LimpiarCampos()
    {
        campoNombre.text = "";
        campoEdad.text = "";
        Debug.Log("🧹 Campos limpiados");
    }
}