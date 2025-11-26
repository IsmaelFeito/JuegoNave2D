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

    // Métodos de velocidad (existentes)
    public void Lento()
    {
        Time.timeScale = 0.25f;
        Debug.Log("Modo LENTO activado");
    }

    public void Rapido()
    {
        Time.timeScale = 2f;
        Debug.Log("Modo RÁPIDO activado");
    }

    // Generación de enemigos (existentes)
    public void GenerarAsteroide()
    {
        randomY = Random.Range(-4.16f, 4.16f);
        GameObject u = Instantiate(asteroide) as GameObject;
        u.transform.position = new Vector3(12f, randomY, 0f);
        u.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-2f, 0f);
        asteroideEnJuego++;
    }

    IEnumerator OleadasAsteroides()
    {
        while (true)
        {
            float tiempo = Random.Range(1, 3);
            yield return new WaitForSeconds(tiempo);

            if (asteroideEnJuego < 10)
            {
                GenerarAsteroide();
            }
        }
    }

    public void GenerarUfo()
    {
        randomY = Random.Range(-4.16f, 4.16f);
        GameObject u = Instantiate(ufo) as GameObject;
        u.transform.position = new Vector3(12f, randomY, 0f);
        u.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(-250f, 0f));
        ufosEnJuego++;
    }
    
    IEnumerator OleadasUfo()
    {
        while (true){
            float tiempo = Random.Range(1, 3);
            yield return new WaitForSeconds(tiempo);

            if (ufosEnJuego < 10){
                GenerarUfo();
            }
        }
    }

    // SISTEMA DE IMÁN CORREGIDO
    public void ActivarImán()
    {
        // Solo permitir si está listo y tiene usos
        if (magnetState != MagnetState.Ready || magnetUsesRemaining <= 0)
            return;
            
        if (magnetController != null)
        {
            magnetController.SetMagnetActive(true);
            magnetState = MagnetState.Active;
            magnetTimer = magnetDuration;
            magnetUsesRemaining--;
            
            Debug.Log($"Imán activado. Usos restantes: {magnetUsesRemaining}");
            ActualizarUIImán();
        }
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
                    // Terminar imán activo
                    if (magnetController != null)
                        magnetController.SetMagnetActive(false);
                    
                    // Iniciar cooldown si aún quedan usos
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
            
        // Actualizar texto según estado
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
        
        // Actualizar interactividad del botón
        magnetButton.interactable = (magnetState == MagnetState.Ready && magnetUsesRemaining > 0);
        
        // Cambiar color del botón
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
    
    // Método para recargar usos del imán
    public void RecargarImán(int usos = 1)
    {
        magnetUsesRemaining = Mathf.Min(magnetUsesRemaining + usos, maxMagnetUses);
        
        // Si estaba sin usos y ahora tiene, cambiar a Ready
        if (magnetState == MagnetState.NoUses && magnetUsesRemaining > 0)
        {
            magnetState = MagnetState.Ready;
        }
        
        ActualizarUIImán();
        Debug.Log($"Usos de imán recargados. Total: {magnetUsesRemaining}");
    }
    
    // Método para resetear el imán
    public void ResetearImán()
    {
        if (magnetController != null)
        {
            magnetController.SetMagnetActive(false);
        }
        
        magnetState = MagnetState.Ready;
        magnetUsesRemaining = maxMagnetUses;
        ActualizarUIImán();
    }

    void Start()
    {
        ufosEnJuego = 0;
        asteroideEnJuego = 0;
        
        // Inicializar sistema de imán
        magnetUsesRemaining = maxMagnetUses;
        magnetState = MagnetState.Ready;
        
        // Configurar el botón del imán si existe
        if (magnetButton != null)
        {
            magnetButton.onClick.AddListener(ActivarImán);
        }
        
        ActualizarUIImán();
        
        // Iniciar generación de enemigos
        StartCoroutine(OleadasUfo());
        StartCoroutine(OleadasAsteroides());
    }

    void Update()
    {
        // Movimiento de la nave (existente)
        direcX = Input.GetAxisRaw("Horizontal");
        direcY = Input.GetAxisRaw("Vertical");
        direc = new Vector2(direcX, direcY).normalized;
        playerRb.linearVelocity = new Vector2(direcX * velocidad, direcY * velocidad);
        playerRb.position = new Vector2(Mathf.Clamp(playerRb.position.x, -7.54f, 7.54f), Mathf.Clamp(playerRb.position.y, -4.16f, 4.16f));

        // Actualizar temporizadores del imán
        ActualizarTemporizadoresImán();

        // Debug con tecla M - solo funciona si está listo
        if (Input.GetKeyDown(KeyCode.M) && magnetState == MagnetState.Ready && magnetUsesRemaining > 0)
        {
            Debug.Log("Activando imán con tecla M");
            ActivarImán();
        }
    }
}