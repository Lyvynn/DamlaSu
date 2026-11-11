using System.Collections.Generic;
using UnityEngine;

// Bölümün arazisini oluşturur. Her bölümde değerleri farklı veriyoruz
// (yükseklik, sıklık, tümsek) böylece bölümler gittikçe zorlaşıyor.
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(EdgeCollider2D))]
public class Arazi : MonoBehaviour
{
    public float uzunluk = 200f;
    public float yukseklik = 2f;
    public float siklik = 0.04f;
    public float tumsek = 0f;
    public float tohum = 10f;
    public float aralik = 0.5f;
    public LineRenderer cimen;

    void Awake()
    {
        Olustur();
    }

    public float YukseklikAl(float x)
    {
        float basla = Mathf.Clamp01((x - 15f) / 20f);      // başta düz alan
        float bitir = Mathf.Clamp01((uzunluk - 20f - x) / 15f); // sonda düz alan (teslim noktası)
        float zorlasma = 0.6f + 0.4f * (x / uzunluk);       // bölümün sonuna doğru daha engebeli

        float tepe = (Mathf.PerlinNoise(x * siklik + tohum, 0.3f) * 2f - 1f) * yukseklik;
        float kucukTumsek = (Mathf.PerlinNoise(x * 0.6f + tohum * 2f, 0.7f) * 2f - 1f) * tumsek;

        return (tepe + kucukTumsek) * basla * bitir * zorlasma;
    }

    public void Olustur()
    {
        List<Vector2> noktalar = new List<Vector2>();
        for (float x = -30; x <= uzunluk + 30; x += aralik)
            noktalar.Add(new Vector2(x, YukseklikAl(Mathf.Clamp(x, 0, uzunluk))));

        GetComponent<EdgeCollider2D>().SetPoints(noktalar);

        // mesh (toprak)
        Mesh mesh = new Mesh();
        Vector3[] v = new Vector3[noktalar.Count * 2];
        Color[] renk = new Color[v.Length];
        int[] ucgen = new int[(noktalar.Count - 1) * 6];
        for (int i = 0; i < noktalar.Count; i++)
        {
            v[i * 2] = noktalar[i];
            v[i * 2 + 1] = new Vector3(noktalar[i].x, -25f, 0);
            renk[i * 2] = new Color(0.55f, 0.38f, 0.2f);
            renk[i * 2 + 1] = new Color(0.3f, 0.2f, 0.1f);
        }
        for (int i = 0; i < noktalar.Count - 1; i++)
        {
            int a = i * 2;
            ucgen[i * 6] = a; ucgen[i * 6 + 1] = a + 2; ucgen[i * 6 + 2] = a + 1;
            ucgen[i * 6 + 3] = a + 1; ucgen[i * 6 + 4] = a + 2; ucgen[i * 6 + 5] = a + 3;
        }
        mesh.vertices = v;
        mesh.colors = renk;
        mesh.triangles = ucgen;
        mesh.RecalculateBounds();
        GetComponent<MeshFilter>().sharedMesh = mesh;

        // üstteki çimen çizgisi
        if (cimen != null)
        {
            cimen.positionCount = noktalar.Count;
            for (int i = 0; i < noktalar.Count; i++)
                cimen.SetPosition(i, new Vector3(noktalar[i].x, noktalar[i].y, -0.1f));
        }
    }
}
