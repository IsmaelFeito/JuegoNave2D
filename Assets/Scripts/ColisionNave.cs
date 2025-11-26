using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ColisionNave : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text textoVidas;
    public TMP_Text contadorPuntosTexto;
    public Slider healthBarSlider;
    
    [Header("Menú Fin de Partida")]
    public GameObject menuFinPartida;
    public TMP_Text puntuacionFinalTexto;
    
    [Header("Configuración de Vida")]
    public int maxHealth = 100;
    private int currentHealth;
    private int vidas = 3;
    private int contadorPuntos = 0;
    private bool juegoActivo = true;

    void Start()
    {
        // Inicializar vida y UI
        currentHealth = maxHealth;
        ActualizarBarraVida();
        ActualizarTextoVidas();
        
        contadorPuntos = 0;
        if (contadorPuntosTexto != null)
            contadorPuntosTexto.text = "PUNTOS: " + contadorPuntos;
            
        // Ocultar menú de fin de partida
        if (menuFinPartida != null)
            menuFinPartida.SetActive(false);
    }

    // MÉTODOS PARA LA BARRA DE VIDA (los que ya tienes)
    private void ActualizarBarraVida()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
            ActualizarColorBarraVida();
        }
    }
    
    private void ActualizarColorBarraVida()
    {
        if (healthBarSlider == null) return;
        
        float healthPercent = (float)currentHealth / maxHealth;
        Image fillImage = healthBarSlider.fillRect.GetComponent<Image>();
        
        if (fillImage != null)
        {
            if (healthPercent > 0.6f)
                fillImage.color = Color.green;
            else if (healthPercent > 0.3f)
                fillImage.color = Color.yellow;
            else
                fillImage.color = Color.red;
        }
    }
    
    public void RecibirDano(int dano)
    {
        if (!juegoActivo) return;
        
        currentHealth -= dano;
        if (currentHealth < 0) currentHealth = 0;
        
        ActualizarBarraVida();
        
        if (currentHealth <= 0)
        {
            PerderVidaCompleta();
        }
    }
    
    private void PerderVidaCompleta()
    {
        if (vidas > 0)
        {
            vidas--;
            currentHealth = maxHealth;
            ActualizarBarraVida();
            ActualizarTextoVidas();
        }
        
        if (vidas <= 0)
        {
            FinDelJuego();
        }
    }

    // NUEVO: MÉTODO PARA FIN DEL JUEGO
    private void FinDelJuego()
    {
        juegoActivo = false;
        
        // Parar el tiempo del juego
        Time.timeScale = 0f;
        
        // Mostrar menú de fin de partida
        if (menuFinPartida != null)
        {
            menuFinPartida.SetActive(true);
            
            // Actualizar texto de puntuación final
            if (puntuacionFinalTexto != null)
                puntuacionFinalTexto.text = contadorPuntos.ToString();
        }
        
        Debug.Log("JUEGO TERMINADO - Puntuación: " + contadorPuntos);
    }

    // MÉTODOS PARA LOS BOTONES
    public void ReiniciarPartida()
    {
        // Reanudar el tiempo
        Time.timeScale = 1f;
        
        // Recargar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void IrAlMenuPrincipal()
    {
        // Reanudar el tiempo antes de cambiar de escena
        Time.timeScale = 1f;
        
        // Cargar la escena del menú principal (asegúrate de que existe)
        // SceneManager.LoadScene("MenuPrincipal");
        // O si no tienes menú principal, recarga la escena actual:
        SceneManager.LoadScene(0); // Escena 0 = menú principal
    }

    // Tus métodos existentes...
    private string generarTextoVidas(int vidas)
    {
        if (vidas <= 0) return "<//3";
        string v = "";
        for (int i = 0; i < vidas; i++)
            v += "<3";
        return v;
    }

    private void ActualizarTextoVidas()
    {
        if (textoVidas != null)
            textoVidas.text = generarTextoVidas(vidas);
    }

    public void SumarPuntos(int puntos)
    {
        if (!juegoActivo) return;
        
        contadorPuntos += puntos;
        if (contadorPuntosTexto != null)
            contadorPuntosTexto.text = "PUNTOS: " + contadorPuntos;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!juegoActivo) return;
        
        if (collision.CompareTag("UFO"))
        {
            SumarPuntos(1);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Asteroid"))
        {
            RecibirDano(34);
            Destroy(collision.gameObject);
        }
    }
}