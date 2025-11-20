using UnityEngine;
using System.Collections;
public class ControladorNave : MonoBehaviour
{
	public float velocidad = 1f;
	public Rigidbody2D playerRb;
	public float direcX, direcY;
	public Vector2 direc;
	public GameObject asteroide, ufo;
	public int ufosEnJuego;
	public int asteroideEnJuego;

	public float randomY;

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

	// public void GenerarPrefabs()
	// {
	// 	GameObject a1 = Instantiate(asteroide) as GameObject;
	// 	a1.transform.position = new Vector3(3f, 3f, 0);

	// 	GameObject a2 = Instantiate(asteroide) as GameObject;
	// 	a2.transform.position = new Vector3(3f, 3f, 0);

	// 	GameObject u1 = Instantiate(ufo) as GameObject;
	// 	u1.transform.position = new Vector3(1f, 3f, 0);

	// 	GameObject u2 = Instantiate(ufo) as GameObject;
	// 	u2.transform.position = new Vector3(1f, 3f, 0);

	// 	GameObject u3 = Instantiate(ufo) as GameObject;
	// 	u3.transform.position = new Vector3(3f, 3f, 0);

	// 	a1.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(-300f, 0f));
	// 	a2.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-5f, 0f);

	// 	u1.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(-150f, 0f));
	// 	u2.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-5f, 0f);
	// 	u3.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-7f, 0f);
	// }

	void Start(){

		ufosEnJuego = 0;
		// GenerarPrefabs();
		StartCoroutine(OleadasUfo());
		StartCoroutine(OleadasAsteroides());
	}

	void Update(){

		direcX = Input.GetAxisRaw("Horizontal");
		direcY = Input.GetAxisRaw("Vertical");

		direc = new Vector2(direcX, direcY).normalized;

		playerRb.linearVelocity = new Vector2(direcX * velocidad, direcY * velocidad);

		playerRb.position = new Vector2(Mathf.Clamp(playerRb.position.x, -7.54f, 7.54f), Mathf.Clamp(playerRb.position.y, -4.16f, 4.16f));

	}
	
}