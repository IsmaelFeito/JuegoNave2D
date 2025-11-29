using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [Header("MÚSICA DE FONDO")]
    public AudioClip musicaMenuPrincipal;
    public AudioClip musicaJuegoPrincipal;
    public AudioClip musicaPausa;
    
    [Header("EFECTOS DE SONIDO")]
    public AudioClip efectoExplosionAsteroide;
    public AudioClip efectoClickBoton;
    
    [Header("VOLUMEN")]
    [Range(0f, 1f)]
    public float volumenMusica = 0.5f;
    [Range(0f, 1f)]
    public float volumenEfectos = 0.7f;
    
    // AudioSources internos
    private AudioSource musicaFondoSource;
    private AudioSource efectosSonidoSource;
    
    // Singleton
    public static AudioManager Instance { get; private set; }
    
    private string escenaActual;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CrearAudioSources();
            ConfigurarAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GestionarMusicaPorEscena(SceneManager.GetActiveScene().name);
        SceneManager.sceneLoaded += OnEscenaCargada;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnEscenaCargada;
    }

    void CrearAudioSources()
    {
        // Crear AudioSource para música
        musicaFondoSource = gameObject.AddComponent<AudioSource>();
        musicaFondoSource.playOnAwake = false;
        musicaFondoSource.loop = true;
        
        // Crear AudioSource para efectos
        efectosSonidoSource = gameObject.AddComponent<AudioSource>();
        efectosSonidoSource.playOnAwake = false;
    }

    void ConfigurarAudioSources()
    {
        if (musicaFondoSource != null)
        {
            musicaFondoSource.volume = volumenMusica;
        }
        
        if (efectosSonidoSource != null)
        {
            efectosSonidoSource.volume = volumenEfectos;
        }
    }

    void OnEscenaCargada(Scene escena, LoadSceneMode modo)
    {
        GestionarMusicaPorEscena(escena.name);
    }

    void GestionarMusicaPorEscena(string nombreEscena)
    {
        escenaActual = nombreEscena;
        
        if (musicaFondoSource == null) return;
        
        switch (nombreEscena)
        {
            case "MenuPrincipal":
                CambiarMusica(musicaMenuPrincipal);
                break;
                
            case "JuegoPrincipal":
                CambiarMusica(musicaJuegoPrincipal);
                break;
                
            default:
                // Para cualquier otra escena, usar música del juego
                if (musicaJuegoPrincipal != null)
                    CambiarMusica(musicaJuegoPrincipal);
                break;
        }
    }

    // MÉTODOS PARA CAMBIAR MÚSICA
    public void CambiarMusica(AudioClip nuevaMusica)
    {
        if (musicaFondoSource == null || nuevaMusica == null) return;
        
        // Si ya está reproduciendo esta música, no hacer nada
        if (musicaFondoSource.clip == nuevaMusica && musicaFondoSource.isPlaying)
            return;
            
        musicaFondoSource.clip = nuevaMusica;
        musicaFondoSource.Play();
    }

    // MÉTODOS PARA EFECTOS DE SONIDO
    public void ReproducirExplosionAsteroide()
    {
        if (efectosSonidoSource != null && efectoExplosionAsteroide != null)
        {
            efectosSonidoSource.PlayOneShot(efectoExplosionAsteroide);
        }
    }

    public void ReproducirClickBoton()
    {
        if (efectosSonidoSource != null && efectoClickBoton != null)
        {
            efectosSonidoSource.PlayOneShot(efectoClickBoton);
        }
    }

    // MÉTODOS PARA CONTROL DE VOLUMEN
    public void SetVolumenMusica(float volumen)
    {
        volumenMusica = Mathf.Clamp01(volumen);
        if (musicaFondoSource != null)
            musicaFondoSource.volume = volumenMusica;
    }

    public void SetVolumenEfectos(float volumen)
    {
        volumenEfectos = Mathf.Clamp01(volumen);
        if (efectosSonidoSource != null)
            efectosSonidoSource.volume = volumenEfectos;
    }

    // MÉTODOS PARA PAUSA
    public void ActivarMusicaPausa()
    {
        if (musicaPausa != null)
        {
            CambiarMusica(musicaPausa);
        }
    }

    public void DesactivarMusicaPausa()
    {
        // Volver a la música correspondiente a la escena actual
        GestionarMusicaPorEscena(escenaActual);
    }

    
}