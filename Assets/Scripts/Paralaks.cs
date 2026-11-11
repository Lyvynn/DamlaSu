using UnityEngine;

// arka plandaki tepeler ve bulutlar kameradan yavaş kaysın
public class Paralaks : MonoBehaviour
{
    public float oran = 0.8f;
    Transform kamera;
    Vector3 ilkYer;
    Vector3 kameraIlk;

    void Start()
    {
        kamera = Camera.main.transform;
        ilkYer = transform.position;
        kameraIlk = kamera.position;
    }

    void LateUpdate()
    {
        Vector3 fark = kamera.position - kameraIlk;
        transform.position = new Vector3(ilkYer.x + fark.x * oran, ilkYer.y + fark.y * oran, ilkYer.z);
    }
}
