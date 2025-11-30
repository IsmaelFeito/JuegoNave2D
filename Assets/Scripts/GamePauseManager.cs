using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GamePauseManager : MonoBehaviour
{
    [Header("Referencias del Menú de Pausa")]
    public GameObject menuPausa;
    public Button botonContinuar;
    public Button botonDificultad;  // NUEVO BOTÓN
    public Button botonMenuPrincipal;
    
    [Header("Configuración")]
    public KeyCode teclaPausa = KeyCode.P;
    public KeyCode teclaPausaAlternativa = KeyCode.Escape;
    
    private bool juegoPausado = false;

    void Start()
    {
        // Configurar botones
        if (botonContinuar != null)
            botonContinuar.onClick.AddListener(ReanudarJuego);
            
        // NUEVO: Botón Dificultad
        if (botonDificultad != null)
            botonDificultad.onClick.AddListener(IrAMenuDificultad);
            
        if (botonMenuPrincipal != null)
            botonMenuPrincipal.onClick.AddListener(IrAlMenuPrincipal);
        
        // Asegurar que el menú de pausa está oculto al empezar
        if (menuPausa != null)
            menuPausa.SetActive(false);
    }

    void Update()
    {
        // Detectar tecla de pausa
        if (Input.GetKeyDown(teclaPausa) || Input.GetKeyDown(teclaPausaAlternativa))
        {
            TogglePausa();
        }
    }

    public void TogglePausa()
    {
        juegoPausado = !juegoPausado;
        
        if (juegoPausado)
        {
            PausarJuego();
        }
        else
        {
            ReanudarJuego();
        }
    }

    public void PausarJuego()
    {
        juegoPausado = true;
        Time.timeScale = 0f;
        
        if (menuPausa != null)
            menuPausa.SetActive(true);
        
        // ACTIVAR MÚSICA DE PAUSA
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ActivarMusicaPausa();
        }
        
        Debug.Log("JUEGO EN PAUSA");
    }

    public void ReanudarJuego()
    {
        juegoPausado = false;
        Time.timeScale = 1f;
        
        if (menuPausa != null)
            menuPausa.SetActive(false);
        
        // DESACTIVAR MÚSICA DE PAUSA
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.DesactivarMusicaPausa();
        }
        
        Debug.Log("JUEGO REANUDADO");
    }

    // NUEVO MÉTODO: Ir a selección de dificultad desde pausa
    public void IrAMenuDificultad()
    {
        ReproducirSonidoBoton();
        
        // Reanudar el tiempo antes de cambiar de escena
        Time.timeScale = 1f;
        
        Debug.Log("🎯 Cambiando dificultad desde pausa...");
        SceneManager.LoadScene("MenuDificultad");
    }

    public void IrAMenuDatos()
    {
        ReproducirSonidoBoton();
        
        // Reanudar el tiempo antes de cambiar de escena
        Time.timeScale = 1f;
        
        Debug.Log("🎯 Cambiando dificultad desde pausa...");
        SceneManager.LoadScene("MenuDatos");
    }

    public void IrAlMenuPrincipal()
    {
        ReproducirSonidoBoton();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
    
    void ReproducirSonidoBoton()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ReproducirClickBoton();
        }
    }
    
    public bool EstaPausado()
    {
        return juegoPausado;
    }
}