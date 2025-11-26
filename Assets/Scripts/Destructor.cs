using UnityEngine;

public class Destructor : MonoBehaviour
{
    [Header("Configuración")]
    public bool destruirSoloEnemigos = true;
    public string[] tagsParaDestruir = { "UFO", "Asteroid", "Enemy" };
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificar si debemos destruir este objeto
        if (DebeDestruirse(collision.gameObject))
        {
            Debug.Log($"Destruyendo objeto: {collision.gameObject.name}");
            Destroy(collision.gameObject);
        }
    }
    
    private bool DebeDestruirse(GameObject obj)
    {
        // Si no estamos filtrando por tags, destruir todo
        if (!destruirSoloEnemigos)
            return true;
            
        // Verificar si el objeto tiene uno de los tags permitidos
        foreach (string tag in tagsParaDestruir)
        {
            if (obj.CompareTag(tag))
                return true;
        }
        
        return false;
    }
}