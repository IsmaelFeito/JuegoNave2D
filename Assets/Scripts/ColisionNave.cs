using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ColisionNave : MonoBehaviour
{
    [Header("UI del Juego")]
    public TMP_Text textoVidas;
    public TMP_Text contadorPuntosTexto;
    public Slider healthBarSlider;
    
    [Header("Menú de Fin de Partida")]
    public GameObject menuFinPartida;
    public TMP_Text textoPuntuacionFinal;
    public TMP_Text textoNombreJugador;
    public TMP_Text textoMejoresPuntuaciones;
    
    [Header("Sistema de Partículas")]
    public ParticleSystem explosionEffect;
    
    [Header("Configuración")]
    public int maxHealth = 100;
    private int currentHealth;
    private int vidas = 3;
    private int contadorPuntos = 0;
    public bool juegoActivo = true;

    [Header("Dificultad")]
    private DifficultySettings configuracionDificultad;

    void Start()
    {
        // Asegurar que existe el ScoreManager
        AsegurarScoreManager();

        CargarConfiguracionDificultad();
        vidas = configuracionDificultad.vidasIniciales;
        maxHealth = configuracionDificultad.vidaJugador;
        currentHealth = maxHealth;
        
        // Verificación de partículas
        if (explosionEffect == null)
        {
            Debug.LogWarning("⚠️ ExplosionEffect no está asignado.");
        }
        
        // Inicialización normal
        currentHealth = maxHealth;
        ActualizarBarraVida();
        ActualizarTextoVidas();
        
        contadorPuntos = 0;
        if (contadorPuntosTexto != null)
            contadorPuntosTexto.text = "PUNTOS: " + contadorPuntos;
            
        if (menuFinPartida != null)
            menuFinPartida.SetActive(false);
    }

    private void CargarConfiguracionDificultad()
    {
        if (DifficultyManager.Instance != null)
        {
            configuracionDificultad = DifficultyManager.Instance.GetConfiguracionActual();
            Debug.Log($"🎮 Dificultad cargada: {DifficultyManager.Instance.GetDificultadActual()}");
        }
        else
        {
            // Configuración por defecto
            configuracionDificultad = new DifficultySettings();
            Debug.LogWarning("⚠️ Usando dificultad por defecto");
        }
    }

    // NUEVO MÉTODO: Asegurar que el ScoreManager existe
    private void AsegurarScoreManager()
    {
        if (ScoreManager.Instance == null)
        {
            Debug.Log("📊 Creando ScoreManager...");
            GameObject scoreObj = new GameObject("ScoreManager");
            scoreObj.AddComponent<ScoreManager>();
            
            // Verificar que se creó correctamente
            if (ScoreManager.Instance != null)
            {
                Debug.Log("✅ ScoreManager creado exitosamente");
            }
            else
            {
                Debug.LogError("❌ Fallo al crear ScoreManager");
            }
        }
        else
        {
            Debug.Log("✅ ScoreManager ya existe");
        }
    }

    // MÉTODO MEJORADO para el ranking
    private string ObtenerTextoMejoresPuntuaciones()
    {
        // Verificar que el ScoreManager existe
        if (ScoreManager.Instance == null)
        {
            Debug.LogError("❌ ScoreManager no disponible");
            return "Sistema de ranking no disponible\n\nPuntuación actual: " + contadorPuntos;
        }
        
        // Obtener las puntuaciones
        var mejores = ScoreManager.Instance.ObtenerMejoresPuntuaciones();
        
        if (mejores == null || mejores.Count == 0)
        {
            return "No hay puntuaciones guardadas\n\n¡Sé el primero!";
        }
        
        // Construir el texto del ranking
        string texto = "🏆 RANKING ACTUAL\n";
        texto += "----------------\n";
        
        for (int i = 0; i < mejores.Count; i++)
        {
            string nombre = string.IsNullOrEmpty(mejores[i].nombre) ? "Jugador" : mejores[i].nombre;
            texto += $"{i + 1}. {nombre}: {mejores[i].puntuacion} pts\n";
        }
        
        return texto;
    }

    // EL RESTO DE TUS MÉTODOS SE MANTIENEN IGUAL...
    private void PlayExplosionEffect(Vector3 position)
    {
        if (explosionEffect != null)
        {
            ParticleSystem explosion = Instantiate(explosionEffect, position, Quaternion.identity);
            explosion.Play();
            Destroy(explosion.gameObject, explosion.main.duration + 0.5f);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!juegoActivo) return;
        
        if (collision.CompareTag("UFO"))
        {
            int puntos = configuracionDificultad.puntosPorUFO;
            SumarPuntos(1);
            Destroy(collision.gameObject, 0.1f);
        }
        else if (collision.CompareTag("Asteroid"))
        {
            int dano = configuracionDificultad.danoAsteroide;
            RecibirDano(34);
            
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.ReproducirExplosionAsteroide();
            }
            
            PlayExplosionEffect(collision.transform.position);
            Destroy(collision.gameObject, 0.2f);
        }
    }

    private void ActualizarBarraVida()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
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

    private void FinDelJuego()
    {
        juegoActivo = false;
        Time.timeScale = 0f;
        
        Debug.Log("🎮 Fin del juego - Guardando puntuación: " + contadorPuntos);
        
        // Guardar la puntuación
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.GuardarPuntuacion(contadorPuntos);
            Debug.Log("✅ Puntuación guardada en ScoreManager");
        }
        else
        {
            Debug.LogError("❌ No se pudo guardar la puntuación - ScoreManager es null");
        }
        
        if (menuFinPartida != null)
        {
            menuFinPartida.SetActive(true);
            ActualizarMenuFinPartida();
        }
    }

    private void ActualizarMenuFinPartida()
    {
        // Puntuación actual
        if (textoPuntuacionFinal != null)
            textoPuntuacionFinal.text = $"Puntuación: {contadorPuntos}";
        
        // Nombre y edad del jugador
        string nombre = PlayerPrefs.GetString("NombreJugador", "Jugador");
        string edad = PlayerPrefs.GetString("EdadJugador", "0");
        
        if (textoNombreJugador != null)
            textoNombreJugador.text = $"{nombre} ({edad} años)";
        
        // Mejores puntuaciones
        if (textoMejoresPuntuaciones != null)
        {
            string rankingTexto = ObtenerTextoMejoresPuntuaciones();
            textoMejoresPuntuaciones.text = rankingTexto;
            Debug.Log("📋 Texto del ranking:\n" + rankingTexto);
        }
    }

    public void ReiniciarPartida()
    {
        ReproducirSonidoBoton();
        Time.timeScale = 1f;
        SceneManager.LoadScene("JuegoPrincipal");
    }
    
    public void IrAlMenuPrincipal()
    {
        ReproducirSonidoBoton();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }

    private void ReproducirSonidoBoton()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ReproducirClickBoton();
        }
    }

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
}