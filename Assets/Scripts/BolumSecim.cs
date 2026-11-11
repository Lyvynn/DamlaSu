using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BolumSecim : MonoBehaviour
{
    public Button[] bolumButonlari;   // 5 tane
    public Text[] skorYazilari;
    public GameObject[] kilitler;
    public AudioSource tik;

    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("ses", 1f);
        int acik = PlayerPrefs.GetInt("acikBolum", 1);

        for (int i = 0; i < bolumButonlari.Length; i++)
        {
            int no = i + 1;
            bool acikMi = no <= acik;
            bolumButonlari[i].interactable = acikMi;
            kilitler[i].SetActive(!acikMi);

            int skor = PlayerPrefs.GetInt("skor" + no, 0);
            skorYazilari[i].text = skor > 0 ? "En iyi: " + skor : "";

            bolumButonlari[i].onClick.AddListener(() => BolumAc(no));
        }
    }

    void BolumAc(int no)
    {
        tik.Play();
        SceneManager.LoadScene("Bolum" + no);
    }

    public void Geri()
    {
        tik.Play();
        SceneManager.LoadScene("AnaMenu");
    }
}
