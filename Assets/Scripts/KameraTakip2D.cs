using UnityEngine;

public class KameraTakip2D : MonoBehaviour
{
    public Transform hedef;
    public Vector3 ofset = new Vector3(4f, 1.5f, -10f);
    public float yumusaklik = 4f;

    void LateUpdate()
    {
        if (hedef == null) return;
        Vector3 yer = hedef.position + ofset;
        transform.position = Vector3.Lerp(transform.position, yer, yumusaklik * Time.deltaTime);
    }
}
