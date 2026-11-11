using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

// Bölüm içindeki oyun akışı: gösterge, pause, bölüm sonu
public class SeviyeYonetici : MonoBehaviour
{
    public int bolumNo = 1;
    public float hedefYuzde = 50f;   // en az bu kadar su teslim edilmeli
    public float bitisX = 180f;
    public float hedefSure = 60f;

    public AracKontrol arac;
    public SuTanki tank;

    [Header("UI")]
    public Text sureText;
    public Text bolumText;
    public GameObject pausePaneli;
    public GameObject sonucPaneli;
    public Text sonucBaslik;
    public Text sonucBilgi;
    public Image[] yildizlar;
    public Button sonrakiButon;

    [Header("Ses")]
    public AudioSource sesKaynagi;
    public AudioClip basariSesi;
    public AudioClip basarisizSesi;
    public AudioClip tikSesi;

    float sure;
    float devrikSure;
    bool bitti;
    bool durdu;

    void Start()
    {
        Time.timeScale = 1;
        AudioListener.volume = PlayerPrefs.GetFloat("ses", 1f);
        pausePaneli.SetActive(false);
        sonucPaneli.SetActive(false);
        bolumText.text = "BÖLÜM " + bolumNo + "   (hedef: %" + hedefYuzde + " su)";
    }

    void Update()
    {
        if (bitti) return;

        if (Keyboard.current != null && (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame))
        {
            if (durdu) Devam(); else Durdur();
        }
        if (durdu) return;

        sure += Time.deltaTime;

        float su = tank.KalanYuzde();
        sureText.text = "Süre: " + sure.ToString("0.0");

        // kontroller
        if (su < hedefYuzde)
        {
            BolumBitti(false, "Çok fazla su döküldü!");
        }
        else if (arac.transform.position.x >= bitisX)
        {
            BolumBitti(true, "Su başarıyla teslim edildi!");
        }

        if (arac.transform.up.y < -0.1f) devrikSure += Time.deltaTime;
        else devrikSure = 0;
        if (devrikSure > 2f) BolumBitti(false, "Araç devrildi!");
    }

    void BolumBitti(bool basarili, string mesaj)
    {
        bitti = true;
        arac.kontrolAcik = false;
        sonucPaneli.SetActive(true);

        float su = tank.KalanYuzde();
        int skor = 0;
        int yildiz = 0;

        if (basarili)
        {
            skor = Mathf.RoundToInt(su * 10 + Mathf.Max(0, hedefSure - sure) * 5);
            yildiz = 1;
            if (su >= 75) yildiz = 2;
            if (su >= 90) yildiz = 3;

            // sonraki bölümü aç, en iyi skoru kaydet
            if (PlayerPrefs.GetInt("acikBolum", 1) < bolumNo + 1)
                PlayerPrefs.SetInt("acikBolum", bolumNo + 1);
            if (skor > PlayerPrefs.GetInt("skor" + bolumNo, 0))
                PlayerPrefs.SetInt("skor" + bolumNo, skor);
            PlayerPrefs.Save();

            sonucBaslik.text = "BAŞARILI!";
            sonucBaslik.color = new Color(0.3f, 0.85f, 0.4f);
            sesKaynagi.PlayOneShot(basariSesi);
        }
        else
        {
            sonucBaslik.text = "BAŞARISIZ";
            sonucBaslik.color = new Color(1f, 0.4f, 0.35f);
            sesKaynagi.PlayOneShot(basarisizSesi);
        }

        sonucBilgi.text = mesaj + "\n\nTeslim edilen su: %" + Mathf.RoundToInt(su) +
                          "\nSüre: " + sure.ToString("0.0") + " sn\nSkor: " + skor;

        for (int i = 0; i < yildizlar.Length; i++)
            yildizlar[i].color = i < yildiz ? new Color(1f, 0.85f, 0.2f) : new Color(1, 1, 1, 0.25f);

        sonrakiButon.gameObject.SetActive(basarili && bolumNo < 5);
    }

    public void Durdur()
    {
        durdu = true;
        Time.timeScale = 0;
        pausePaneli.SetActive(true);
        Tik();
    }

    public void Devam()
    {
        durdu = false;
        Time.timeScale = 1;
        pausePaneli.SetActive(false);
        Tik();
    }

    public void YenidenBaslat()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AnaMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("AnaMenu");
    }

    public void SonrakiBolum()
    {
        SceneManager.LoadScene("Bolum" + (bolumNo + 1));
    }

    void Tik()
    {
        if (tikSesi != null) sesKaynagi.PlayOneShot(tikSesi);
    }
}
