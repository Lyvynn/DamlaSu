using System.Collections.Generic;
using UnityEngine;

// Kasadaki suyu küçük damlalarla (Rigidbody2D) yapıyoruz,
// böylece su çalkalanınca aracın ağırlık merkezi de değişiyor.
public class SuTanki : MonoBehaviour
{
    public GameObject damlaPrefab;
    public int satir = 3;
    public int sutun = 9;
    public float damlaArasi = 0.165f;
    public Vector2 tankMerkezi = new Vector2(-0.425f, 0.235f); // aracın içinde tankın yeri
    public Vector2 tankBoyutu = new Vector2(1.55f, 0.73f);
    public AudioSource suSesi;

    List<Transform> damlalar = new List<Transform>();
    List<bool> dokuldu = new List<bool>();
    int toplam;
    int dokulen;

    void Start()
    {
        Vector2 bas = tankMerkezi + new Vector2(-tankBoyutu.x / 2 + 0.1f, -tankBoyutu.y / 2 + 0.1f);
        for (int y = 0; y < satir; y++)
        {
            for (int x = 0; x < sutun; x++)
            {
                Vector3 yer = transform.TransformPoint(bas + new Vector2(x * damlaArasi, y * damlaArasi));
                GameObject d = Instantiate(damlaPrefab, yer, Quaternion.identity);
                damlalar.Add(d.transform);
                dokuldu.Add(false);
            }
        }
        toplam = damlalar.Count;
    }

    void Update()
    {
        for (int i = 0; i < damlalar.Count; i++)
        {
            if (dokuldu[i]) continue;
            Vector2 yerel = transform.InverseTransformPoint(damlalar[i].position);
            Vector2 fark = yerel - tankMerkezi;
            // tankın dışına çok uzaklaştıysa döküldü say
            if (Mathf.Abs(fark.x) > tankBoyutu.x / 2 + 0.3f || fark.y < -tankBoyutu.y / 2 - 0.2f || fark.y > tankBoyutu.y / 2 + 1.5f)
            {
                dokuldu[i] = true;
                dokulen++;
                if (suSesi != null && !suSesi.isPlaying) suSesi.Play();
                Destroy(damlalar[i].gameObject, 4f);
            }
        }
    }

    public float KalanYuzde()
    {
        if (toplam == 0) return 100;
        return (toplam - dokulen) * 100f / toplam;
    }
}
