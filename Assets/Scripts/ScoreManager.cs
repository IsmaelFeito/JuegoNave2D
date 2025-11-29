using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Puntuacion
{
    public string nombre;
    public int edad;
    public int puntuacion;
    public string fecha;

    public Puntuacion(string nombre, int edad, int puntuacion)
    {
        this.nombre = nombre;
        this.edad = edad;
        this.puntuacion = puntuacion;
        this.fecha = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
    }
}

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    
    private List<Puntuacion> mejoresPuntuaciones = new List<Puntuacion>();
    private const int MAX_PUNTUACIONES = 3;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CargarPuntuaciones();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Guardar nueva puntuación
    public void GuardarPuntuacion(int puntuacion)
    {
        string nombre = PlayerPrefs.GetString("NombreJugador", "Jugador");
        int edad = int.Parse(PlayerPrefs.GetString("EdadJugador", "0"));
        
        Puntuacion nuevaPuntuacion = new Puntuacion(nombre, edad, puntuacion);
        
        // Agregar a la lista
        mejoresPuntuaciones.Add(nuevaPuntuacion);
        
        // Ordenar por puntuación (mayor a menor) y mantener solo las mejores
        mejoresPuntuaciones = mejoresPuntuaciones
            .OrderByDescending(p => p.puntuacion)
            .Take(MAX_PUNTUACIONES)
            .ToList();
            
        GuardarEnPlayerPrefs();
    }

    // Obtener las mejores puntuaciones
    public List<Puntuacion> ObtenerMejoresPuntuaciones()
    {
        return mejoresPuntuaciones;
    }

    // Obtener la puntuación más alta
    public int ObtenerPuntuacionMaxima()
    {
        return mejoresPuntuaciones.Count > 0 ? mejoresPuntuaciones[0].puntuacion : 0;
    }

    // Guardar en PlayerPrefs
    private void GuardarEnPlayerPrefs()
    {
        for (int i = 0; i < mejoresPuntuaciones.Count; i++)
        {
            PlayerPrefs.SetString($"Puntuacion_{i}_Nombre", mejoresPuntuaciones[i].nombre);
            PlayerPrefs.SetInt($"Puntuacion_{i}_Edad", mejoresPuntuaciones[i].edad);
            PlayerPrefs.SetInt($"Puntuacion_{i}_Puntos", mejoresPuntuaciones[i].puntuacion);
            PlayerPrefs.SetString($"Puntuacion_{i}_Fecha", mejoresPuntuaciones[i].fecha);
        }
        
        PlayerPrefs.SetInt("TotalPuntuaciones", mejoresPuntuaciones.Count);
        PlayerPrefs.Save();
    }

    // Cargar desde PlayerPrefs
    private void CargarPuntuaciones()
    {
        mejoresPuntuaciones.Clear();
        int total = PlayerPrefs.GetInt("TotalPuntuaciones", 0);
        
        for (int i = 0; i < total; i++)
        {
            string nombre = PlayerPrefs.GetString($"Puntuacion_{i}_Nombre", "");
            int edad = PlayerPrefs.GetInt($"Puntuacion_{i}_Edad", 0);
            int puntos = PlayerPrefs.GetInt($"Puntuacion_{i}_Puntos", 0);
            string fecha = PlayerPrefs.GetString($"Puntuacion_{i}_Fecha", "");
            
            if (!string.IsNullOrEmpty(nombre))
            {
                Puntuacion puntuacion = new Puntuacion(nombre, edad, puntos);
                puntuacion.fecha = fecha;
                mejoresPuntuaciones.Add(puntuacion);
            }
        }
        
        // Ordenar
        mejoresPuntuaciones = mejoresPuntuaciones
            .OrderByDescending(p => p.puntuacion)
            .ToList();
    }

    // Limpiar todas las puntuaciones (para testing)
    public void LimpiarPuntuaciones()
    {
        mejoresPuntuaciones.Clear();
        PlayerPrefs.DeleteKey("TotalPuntuaciones");
        
        for (int i = 0; i < MAX_PUNTUACIONES; i++)
        {
            PlayerPrefs.DeleteKey($"Puntuacion_{i}_Nombre");
            PlayerPrefs.DeleteKey($"Puntuacion_{i}_Edad");
            PlayerPrefs.DeleteKey($"Puntuacion_{i}_Puntos");
            PlayerPrefs.DeleteKey($"Puntuacion_{i}_Fecha");
        }
        
        PlayerPrefs.Save();
    }
}