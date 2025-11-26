using UnityEngine;

public class AjustarFondo : MonoBehaviour
{
    void Start()
    {
        AjustarFondoACamara();
    }
    
    void AjustarFondoACamara()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) 
        {
            Debug.LogError("No hay SpriteRenderer en este objeto!");
            return;
        }
        
        // Resetear escala
        transform.localScale = Vector3.one;
        
        // Obtener el tamaño del sprite en unidades world
        float spriteWidth = sr.sprite.bounds.size.x;
        float spriteHeight = sr.sprite.bounds.size.y;
        
        // Obtener el tamaño de la pantalla de la cámara en unidades world
        Camera cam = Camera.main;
        float cameraHeight = 2f * cam.orthographicSize;
        float cameraWidth = cameraHeight * cam.aspect;
        
        // Calcular la escala necesaria para cubrir toda la pantalla
        float scaleX = cameraWidth / spriteWidth;
        float scaleY = cameraHeight / spriteHeight;
        
        // Usar la escala más grande para asegurar que cubra toda la pantalla
        float scale = Mathf.Max(scaleX, scaleY);
        
        transform.localScale = new Vector3(scale, scale, 1f);
        
        // Posicionar exactamente en el centro de la cámara
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z);
        
        Debug.Log($"Fondo ajustado - Escala: {scale}, Cámara: {cameraWidth}x{cameraHeight}, Sprite: {spriteWidth}x{spriteHeight}");
    }
}