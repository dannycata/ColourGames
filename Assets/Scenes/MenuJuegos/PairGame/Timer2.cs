using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.EventSystems;

public class Timer2 : MonoBehaviour
{
	private Image timerBar;
	public float maxTime = 5f;
	public float timeLeft;
	[SerializeField] private GameObject panelcomienzo = null;
	[SerializeField] private GameObject menu = null;
	public static bool actualizar = false;
	private Button botoncomienzo;
	private Button botonmenu;
	private Text cuentaAtras = null;
	private string nombre;
	private int invisible=0;
	
	[SerializeField] private AudioClip sound = null;
	private AudioSource audioSource = null;
	
    public void Start()
    {
		nombre = PlayerPrefs.GetString("Nombre", "");
		Camera mainCamera = Camera.main;
		audioSource = mainCamera.GetComponent<AudioSource>();
		panelcomienzo.SetActive(true);
		menu.SetActive(false);
		maxTime = PlayerPrefs.GetFloat(nombre+"VelocidadPair", 40f);
		invisible = PlayerPrefs.GetInt(nombre+"TiempoInvisibleParejas", 0);
		actualizar=false;
		timerBar = GetComponent<Image> ();
		cuentaAtras = GameObject.Find("CuentaAtras").GetComponent<Text>();
		cuentaAtras.gameObject.SetActive(false);
		botoncomienzo = GameObject.Find("Empezar").GetComponent<Button>();
		botonmenu = GameObject.Find("BotonPausa").GetComponent<Button>();
		botonmenu.gameObject.SetActive(false);
		Color alpha = timerBar.color;
		alpha.a = 255f;
		timerBar.color = alpha;
		if (invisible == 1)
		{
			alpha.a = 0f;
			timerBar.color = alpha;
		}
		if (maxTime == 0)
		{
			maxTime=1800f;
			alpha.a = 0f;
			timerBar.color = alpha;
		}
		botoncomienzo.onClick.AddListener(CuentaAtras);
		ResetTimer();
	}
	
	public void OnPanelClicked()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			CuentaAtras();
        }
    }
	
	public void CuentaAtras()
	{
		audioSource.PlayOneShot(sound);
		botoncomienzo.gameObject.SetActive(false);
		Button botonvolver = GameObject.Find("VolverMenu").GetComponent<Button>();
		botonvolver.gameObject.SetActive(false);
		cuentaAtras.gameObject.SetActive(true);
		StartCoroutine(CuentaAtrasCoroutine());
	}
	
	IEnumerator CuentaAtrasCoroutine()
    {
        int numeroInicial = 3;
        for (int i = numeroInicial; i > 0; i--)
        {
			audioSource.PlayOneShot(sound);
            cuentaAtras.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
		panelcomienzo.SetActive(false);
		botonmenu.gameObject.SetActive(true);
		actualizar = true;
		Update();
    }

    void Update()
    {
		if(actualizar){
			if (timeLeft > 0){
				timeLeft -= Time.deltaTime;
				timerBar.fillAmount = timeLeft / maxTime;
			} else {
				//Time.timeScale = 0;
				Invoke("End",0.1f);
			}
		}
    }
	
	public void End()
	{
		SceneManager.LoadScene("FinJuego");
	}

    public void ResetTimer()
    {
		enabled=true;
        timeLeft = maxTime;
        timerBar.fillAmount = 1f;
        Time.timeScale = 1;
    }
	
	public void Stop()
    {
        enabled=false;
    }
}
