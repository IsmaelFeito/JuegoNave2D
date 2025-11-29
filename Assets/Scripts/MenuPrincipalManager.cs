using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
    public Button botonJugar;
    public Button botonDificultad;  // NUEVO BOTÓN
    public Button botonConfigurarJugador;
    public Button botonSalir;

    void Start()
    {
        // Configurar botón JUGAR
        if (botonJugar != null)
            botonJugar.onClick.AddListener(() => {
                ReproducirSonidoBoton();
                Jugar();
            });
        
        // NUEVO: Configurar botón DIFICULTAD
        if (botonDificultad != null)
            botonDificultad.onClick.AddListener(() => {
                ReproducirSonidoBoton();
                IrAMenuDificultad();
            });
            
        // Configurar botón CONFIGURAR JUGADOR
        if (botonConfigurarJugador != null)
            botonConfigurarJugador.onClick.AddListener(() => {
                ReproducirSonidoBoton();
                IrAMenuDatos();
            });
            
        // Configurar botón SALIR
        if (botonSalir != null)
            botonSalir.onClick.AddListener(() => {
                ReproducirSonidoBoton();
                Salir();
            });
    }

    void ReproducirSonidoBoton()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ReproducirClickBoton();
        }
    }

    public void Jugar()
    {
        SceneManager.LoadScene("JuegoPrincipal");
    }

    // NUEVO MÉTODO: Ir a selección de dificultad
    public void IrAMenuDificultad()
    {
        Debug.Log("🎯 Yendo a selección de dificultad...");
        SceneManager.LoadScene("MenuDificultad");
    }

    public void IrAMenuDatos()
    {
        Debug.Log("👤 Cargando configuración de jugador...");
        SceneManager.LoadScene("MenuDatos");
    }

    public void Salir()
    {
        Debug.Log("👋 Saliendo del juego...");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}