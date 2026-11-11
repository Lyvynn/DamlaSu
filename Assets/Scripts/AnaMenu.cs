using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AnaMenu : MonoBehaviour
{
    public GameObject ayarlarPaneli;
    public Slider sesSlider;
    public AudioSource tik;

    void Start()
    {
        Time.timeScale = 1;
        ayarlarPaneli.SetActive(false);
        sesSlider.value = PlayerPrefs.GetFloat("ses", 1f);
        AudioListener.volume = sesSlider.value;
    }

    public void Oyna()
    {
        tik.Play();
        SceneManager.LoadScene("BolumSecim");
    }

    public void AyarlariAc()
    {
        tik.Play();
        ayarlarPaneli.SetActive(true);
    }

    public void AyarlariKapat()
    {
        tik.Play();
        PlayerPrefs.SetFloat("ses", sesSlider.value);
        PlayerPrefs.Save();
        ayarlarPaneli.SetActive(false);
    }

    public void SesDegisti(float deger)
    {
        AudioListener.volume = deger;
    }

    public void IlerlemeyiSifirla()
    {
        tik.Play();
        float ses = PlayerPrefs.GetFloat("ses", 1f);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("ses", ses);
    }

    public void Cikis()
    {
        Debug.Log("Oyundan çıkılıyor");
        Application.Quit();
    }
}
