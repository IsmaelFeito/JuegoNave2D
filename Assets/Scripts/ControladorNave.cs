using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class ControladorNave : MonoBehaviour
{
    [Header("Configuración Movimiento")]
    public float velocidad = 1f;
    public Rigidbody2D playerRb;
    public float direcX, direcY;
    public Vector2 direc;

    [Header("Sistema de Velocidad Global")]
    public float velocidadLento = 0.25f;    // Time.timeScale para lento
    public float velocidadNormal = 1f;      // Time.timeScale para normal  
    public float velocidadRapido = 2f;      // Time.timeScale para rápido

    [Header("Prefabs Enemigos")]
    public GameObject asteroide, ufo;
    public int ufosEnJuego;
    public int asteroideEnJuego;
    public float randomY;

    [Header("Sistema de Imán")]
    public MagnetController magnetController;
    public Button magnetButton;
    public TMP_Text magnetButtonText;
    
    [Header("Configuración Imán")]
    public float magnetDuration = 5f;
    public float magnetCooldown = 10f;
    public int maxMagnetUses = 3;
    
    // Estados del imán
    private enum MagnetState { Ready, Active, Cooldown, NoUses }
    private MagnetState magnetState = MagnetState.Ready;
    private int magnetUsesRemaining;
    private float magnetTimer;
    private float cooldownTimer;

    // Configuraciones por dificultad
    private float[] velocidadUFO = { 200f, 250f, 300f };
    private float[] velocidadAsteroide = { 1.5f, 2f, 3f };
    private float[] tiempoMinOleadas = { 2f, 1f, 0.5f };
    private float[] tiempoMaxOleadas = { 4f, 3f, 2f };

    void Start()
    {
        ufosEnJuego = 0;
        asteroideEnJuego = 0;
        
        // Inicializar sistema de velocidad - EMPEZAR CON VELOCIDAD NORMAL
        Time.timeScale = velocidadNormal;
        
        // Inicializar sistema de imán
        magnetUsesRemaining = maxMagnetUses;
        magnetState = MagnetState.Ready;
        
        // Configurar el botón del imán si existe
        if (magnetButton != null)
        {
            magnetButton.onClick.AddListener(ActivarImán);
        }
        
        ActualizarUIImán();
        
        // Iniciar generación de enemigos CON DIFICULTAD
        StartCoroutine(OleadasUfo());
        StartCoroutine(OleadasAsteroides());
        
        // Mostrar dificultad actual
        int dificultad = PlayerPrefs.GetInt("Dificultad", 1);
        Debug.Log($"🎮 Iniciando juego con dificultad: {ObtenerNombreDificultad(dificultad)}");
    }

    void Update()
    {
        // MOVIMIENTO ORIGINAL - Time.timeScale ya afecta a todo automáticamente
        direcX = Input.GetAxisRaw("Horizontal");
        direcY = Input.GetAxisRaw("Vertical");
        direc = new Vector2(direcX, direcY).normalized;

        playerRb.linearVelocity = new Vector2(direc.x * velocidad, direc.y * velocidad);
        playerRb.position = new Vector2(Mathf.Clamp(playerRb.position.x, -7.54f, 7.54f), Mathf.Clamp(playerRb.position.y, -4.16f, 4.16f));

        ActualizarTemporizadoresImán();

        if (Input.GetKeyDown(KeyCode.M) && magnetState == MagnetState.Ready && magnetUsesRemaining > 0)
        {
            Debug.Log("Activando imán con tecla M");
            ActivarImán();
        }
    }

    // MÉTODOS DE VELOCIDAD GLOBAL - AFECTAN A TODO EL JUEGO
    public void Lento()
    {
        Time.timeScale = velocidadLento;
        Debug.Log($"🐢 Modo LENTO activado - Time.timeScale: {Time.timeScale}");
    }

    public void Rapido()
    {
        Time.timeScale = velocidadRapido;
        Debug.Log($"🐇 Modo RÁPIDO activado - Time.timeScale: {Time.timeScale}");
    }

    public void Normal()
    {
        Time.timeScale = velocidadNormal;
        Debug.Log($"⚡ Velocidad NORMAL - Time.timeScale: {Time.timeScale}");
    }

    // SISTEMA DE IMÁN (se mantiene igual)
    public void ActivarImán()
    {
        if (magnetState != MagnetState.Ready || magnetUsesRemaining <= 0)
            return;
            
        if (magnetController != null)
        {
            // Verificar que el MagnetController esté configurado correctamente
            if (!IsMagnetControllerValid())
            {
                Debug.LogError("❌ No se puede activar el imán: MagnetController no está configurado correctamente");
                return;
            }
            
            magnetController.SetMagnetActive(true);
            magnetState = MagnetState.Active;
            magnetTimer = magnetDuration;
            magnetUsesRemaining--;
            
            Debug.Log($"🧲 Imán activado. Usos restantes: {magnetUsesRemaining}");
            ActualizarUIImán();
        }
        else
        {
            Debug.LogError("❌ MagnetController no asignado en el Inspector");
        }
    }

    // Nuevo método para validar el MagnetController
    private bool IsMagnetControllerValid()
    {
        if (magnetController == null)
        {
            Debug.LogError("❌ MagnetController es null");
            return false;
        }
        
        // Verificar que tenga los componentes necesarios
        CircleCollider2D collider = magnetController.GetComponent<CircleCollider2D>();
        if (collider == null)
        {
            Debug.LogError("❌ MagnetController no tiene CircleCollider2D");
            return false;
        }
        
        return true;
    }
    
    void ActualizarTemporizadoresImán()
    {
        switch (magnetState)
        {
            case MagnetState.Active:
                magnetTimer -= Time.deltaTime;
                ActualizarUIImán();
                
                if (magnetTimer <= 0)
                {
                    if (magnetController != null)
                        magnetController.SetMagnetActive(false);
                    
                    if (magnetUsesRemaining > 0)
                    {
                        magnetState = MagnetState.Cooldown;
                        cooldownTimer = magnetCooldown;
                    }
                    else
                    {
                        magnetState = MagnetState.NoUses;
                    }
                    
                    ActualizarUIImán();
                    Debug.Log("Imán desactivado");
                }
                break;
                
            case MagnetState.Cooldown:
                cooldownTimer -= Time.deltaTime;
                ActualizarUIImán();
                
                if (cooldownTimer <= 0)
                {
                    magnetState = MagnetState.Ready;
                    ActualizarUIImán();
                    Debug.Log("Imán listo para usar");
                }
                break;
        }
    }
    
    void ActualizarUIImán()
    {
        if (magnetButtonText == null || magnetButton == null)
            return;
            
        switch (magnetState)
        {
            case MagnetState.Ready:
                magnetButtonText.text = $"IMÁN ({magnetUsesRemaining})";
                break;
            case MagnetState.Active:
                magnetButtonText.text = $"ACTIVO: {magnetTimer:F1}s";
                break;
            case MagnetState.Cooldown:
                magnetButtonText.text = $"CD: {cooldownTimer:F1}s";
                break;
            case MagnetState.NoUses:
                magnetButtonText.text = "SIN USOS";
                break;
        }
        
        magnetButton.interactable = (magnetState == MagnetState.Ready && magnetUsesRemaining > 0);
        
        Image buttonImage = magnetButton.GetComponent<Image>();
        if (buttonImage != null)
        {
            switch (magnetState)
            {
                case MagnetState.Ready:
                    buttonImage.color = magnetUsesRemaining > 0 ? Color.white : Color.gray;
                    break;
                case MagnetState.Active:
                    buttonImage.color = Color.green;
                    break;
                case MagnetState.Cooldown:
                    buttonImage.color = Color.red;
                    break;
                case MagnetState.NoUses:
                    buttonImage.color = Color.gray;
                    break;
            }
        }
    }

    // GENERACIÓN DE ENEMIGOS CON DIFICULTAD (se mantiene igual)
    public void GenerarAsteroide()
    {
        int dificultad = PlayerPrefs.GetInt("Dificultad", 1);
        
        randomY = Random.Range(-4.16f, 4.16f);
        GameObject u = Instantiate(asteroide) as GameObject;
        u.transform.position = new Vector3(12f, randomY, 0f);
        
        float velocidadAst = velocidadAsteroide[dificultad];
        u.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-velocidadAst, 0f);
        
        asteroideEnJuego++;
    }

    IEnumerator OleadasAsteroides()
    {
        while (true)
        {
            int dificultad = PlayerPrefs.GetInt("Dificultad", 1);
            
            float tiempoMin = tiempoMinOleadas[dificultad];
            float tiempoMax = tiempoMaxOleadas[dificultad];
            float tiempo = Random.Range(tiempoMin, tiempoMax);
            
            yield return new WaitForSeconds(tiempo);

            if (asteroideEnJuego < 10)
            {
                GenerarAsteroide();
            }
        }
    }

    public void GenerarUfo()
    {
        int dificultad = PlayerPrefs.GetInt("Dificultad", 1);
        
        randomY = Random.Range(-4.16f, 4.16f);
        GameObject u = Instantiate(ufo) as GameObject;
        u.transform.position = new Vector3(12f, randomY, 0f);
        
        float fuerza = velocidadUFO[dificultad];
        u.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(-fuerza, 0f));
        
        ufosEnJuego++;
    }
    
    IEnumerator OleadasUfo()
    {
        while (true)
        {
            int dificultad = PlayerPrefs.GetInt("Dificultad", 1);
            
            float tiempoMin = tiempoMinOleadas[dificultad];
            float tiempoMax = tiempoMaxOleadas[dificultad];
            float tiempo = Random.Range(tiempoMin, tiempoMax);
            
            yield return new WaitForSeconds(tiempo);

            if (ufosEnJuego < 10)
            {
                GenerarUfo();
            }
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
}