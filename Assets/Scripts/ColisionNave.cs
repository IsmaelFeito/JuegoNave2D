using UnityEngine;
using TMPro;

public class ColisionNave : MonoBehaviour
{
	private int contadorPuntos;
	public TMP_Text contadorPuntosTexto;
	private static int vidas = 3;
	public TMP_Text textoVidas;
	private static final MAX_VIDAS = 5;

	void Start()
	{
		contadorPuntos = 0;
		if (contadorPuntosTexto != null)
			contadorPuntosTexto.text = "PUNTOS: " + contadorPuntos.ToString();

		if (textoVidas != null)
			textoVidas.text = generarTextoVidas(vidas);
		"VIDAS: " + vidas.ToString();
	}

	private string generarTextoVidas(int vidas)
	{
		if (vidas <= MAX_VIDAS){
			if (vidas == 0)
				return "<//3";
			else if (vidas == 1)
				return "<3";
			else if (vidas == 2)
				return "<3<3";
			else if (vidas == 3)
				return "<3<3<3";
			else if (vidas == 4)
				return "<3<3<3<3";
			else if (vidas == 5)
				return "<3<3<3<3<3";
			// else if (vidas <= -1)
				//acabar el juego
		}
	}
	public void SumarPuntos(int puntos)
	{
		contadorPuntos += puntos;
		if (contadorPuntosTexto != null)
		{
			contadorPuntosTexto.text = "PUNTOS: " + contadorPuntos.ToString();
			Debug.Log("Puntos sumados! Total: " + contadorPuntos);
		}
	}

	public void RestarVidasYPuntos(int vidas, int puntos)
	{
		contadorPuntos -= puntos;
		if (contadorPuntosTexto != null)
		{
			contadorPuntosTexto.text = "PUNTOS: " + contadorPuntos.ToString();
			Debug.Log("Puntos sumados! Total: " + contadorPuntos);
		}
	}

	// // Colisiones NORMALES (con física) / DEBUGEO
	// public void OnCollisionEnter2D(Collision2D collision)
	// {
	// 	Debug.Log("¡CHOQUE NORMAL con: " + collision.gameObject.name);
	// }

	// public void OnCollisionStay2D(Collision2D collision)
	// {
	// 	Debug.Log("Sigo chocando con: " + collision.gameObject.name);
	// }

	// public void OnCollisionExit2D(Collision2D collision)
	// {
	// 	Debug.Log("Dejé de chocar con: " + collision.gameObject.name);
	// }


	// Colisiones TRIGGER (sin física)
	public void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("¡TRIGGER con: " + collision.gameObject.name);

		if (collision.CompareTag("UFO"))
		{
			Debug.Log("¡Has chocado con un UFO! +1 punto");
			SumarPuntos(1);
		}
		else if (collision.CompareTag("Asteroide"))
		{
			Debug.Log("¡Cuidado! Asteroide -1 vida");
			RestarVidasYPuntos(1);
		}
	}

	public void OnTriggerStay2D(Collider2D collision)
	{
		Debug.Log("Sigo en trigger con: " + collision.gameObject.name);
	}

	public void OnTriggerExit2D(Collider2D collision)
	{
		Debug.Log("Salí del trigger de: " + collision.gameObject.name);
	}
}