using UnityEngine;

public enum NivelDificultad
{
    Facil,
    Normal,
    Dificil
}

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }
    
    [Header("Configuración por Dificultad")]
    public DifficultySettings facil;
    public DifficultySettings normal;
    public DifficultySettings dificil;
    
    private NivelDificultad dificultadActual = NivelDificultad.Normal;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SetDificultad(NivelDificultad nuevaDificultad)
    {
        dificultadActual = nuevaDificultad;
        Debug.Log($"🎯 Dificultad cambiada a: {nuevaDificultad}");
    }
    
    public DifficultySettings GetConfiguracionActual()
    {
        switch (dificultadActual)
        {
            case NivelDificultad.Facil: return facil;
            case NivelDificultad.Normal: return normal;
            case NivelDificultad.Dificil: return dificil;
            default: return normal;
        }
    }
    
    public NivelDificultad GetDificultadActual()
    {
        return dificultadActual;
    }
}

[System.Serializable]
public class DifficultySettings
{
    [Header("Velocidad de Enemigos")]
    public float velocidadUFOMin = 2f;
    public float velocidadUFOMax = 4f;
    public float velocidadAsteroideMin = 1f;
    public float velocidadAsteroideMax = 3f;
    
    [Header("Frecuencia de Generación")]
    public float tiempoEntreOleadasMin = 2f;
    public float tiempoEntreOleadasMax = 4f;
    public int maxEnemigosEnPantalla = 5;
    
    [Header("Daño y Vida")]
    public int danoAsteroide = 25;
    public int vidaJugador = 100;
    public int vidasIniciales = 3;
    
    [Header("Puntuación")]
    public int puntosPorUFO = 1;
    public int puntosExtraPorNivel = 0;
    
    [Header("Descripción")]
    public string descripcion = "";
}