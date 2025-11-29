using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MenuFinPartida : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject menuFinPartida;
    public TMP_Text textoPuntuacionActual;
    public TMP_Text textoNombreJugador;
    public TMP_Text textoMejoresPuntuaciones;
    
    [Header("Botones")]
    public Button botonReintentar;
    public Button botonMenuPrincipal;

    private int puntuacionFinal;

    void Start()
    {
        // Configurar botones
        if (botonReintentar != null)
            botonReintentar.onClick.AddListener(Reintentar);
            
        if (botonMenuPrincipal != null)
            botonMenuPrincipal.onClick.AddListener(IrAlMenuPrincipal);
        
        // Ocultar menú al inicio
        if (menuFinPartida != null)
            menuFinPartida.SetActive(false);
    }

    // Mostrar menú de fin de partida
    public void MostrarMenuFinPartida(int puntuacion)
    {
        puntuacionFinal = puntuacion;
        
        // Guardar la puntuación
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.GuardarPuntuacion(puntuacion);
        }
        
        // Actualizar UI
        ActualizarUI();
        
        // Mostrar menú
        if (menuFinPartida != null)
            menuFinPartida.SetActive(true);
        
        // Pausar el juego
        Time.timeScale = 0f;
    }

    // Actualizar la interfaz
    private void ActualizarUI()
    {
        // Puntuación actual
        if (textoPuntuacionActual != null)
            textoPuntuacionActual.text = $"Puntuación: {puntuacionFinal}";
        
        // Nombre y edad del jugador
        string nombre = PlayerPrefs.GetString("NombreJugador", "Jugador");
        string edad = PlayerPrefs.GetString("EdadJugador", "0");
        
        if (textoNombreJugador != null)
            textoNombreJugador.text = $"{nombre} ({edad} años)";
        
        // Mejores puntuaciones
        if (textoMejoresPuntuaciones != null)
            textoMejoresPuntuaciones.text = ObtenerTextoMejoresPuntuaciones();
    }

    // Generar texto de mejores puntuaciones
    private string ObtenerTextoMejoresPuntuaciones()
    {
        if (ScoreManager.Instance == null)
            return "Cargando...";
        
        List<Puntuacion> mejores = ScoreManager.Instance.ObtenerMejoresPuntuaciones();
        
        if (mejores.Count == 0)
            return "No hay puntuaciones guardadas";
        
        string texto = "🏆 MEJORES PUNTUACIONES 🏆\n\n";
        
        for (int i = 0; i < mejores.Count; i++)
        {
            string posicion = "";
            switch (i)
            {
                case 0: posicion = "🥇"; break;
                case 1: posicion = "🥈"; break;
                case 2: posicion = "🥉"; break;
            }
            
            texto += $"{posicion} {mejores[i].nombre} - {mejores[i].puntuacion} pts\n";
            texto += $"   Edad: {mejores[i].edad} - {mejores[i].fecha}\n\n";
        }
        
        return texto;
    }

    // Métodos de botones
    public void Reintentar()
    {
        ReproducirSonidoBoton();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
}