using UnityEngine;
using UnityEngine.InputSystem;

// Su taşıma aracının kontrolü. Tekerlekler WheelJoint2D ile kasaya bağlı.
public class AracKontrol : MonoBehaviour
{
    public WheelJoint2D arkaTeker;
    public WheelJoint2D onTeker;
    public float motorHizi = 1400f;   // derece/saniye
    public float motorGucu = 30f;     // tork
    public float havadaDonme = 3f;    // havadayken aracı çevirme
    public AudioSource motorSesi;
    public bool kontrolAcik = true;

    Rigidbody2D rb;
    float gaz;
    float mevcutHiz; // motor birden hızlanmasın, su dökülmesin

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        gaz = 0;
        if (kontrolAcik && Keyboard.current != null)
        {
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) gaz = 1;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) gaz = -1;
        }

        if (motorSesi != null)
            motorSesi.pitch = 0.8f + Mathf.Abs(rb.linearVelocity.x) / 8f;
    }

    void FixedUpdate()
    {
        // motor hızını yavaş yavaş artır
        mevcutHiz = Mathf.MoveTowards(mevcutHiz, -gaz * motorHizi, motorHizi * 0.6f * Time.fixedDeltaTime);
        MotorAyarla(arkaTeker);
        MotorAyarla(onTeker);

        // havada aracın burnunu biraz kaldırıp indirebilmek için
        rb.AddTorque(-gaz * havadaDonme);
    }

    void MotorAyarla(WheelJoint2D teker)
    {
        if (gaz == 0)
        {
            teker.useMotor = false;
            mevcutHiz = Mathf.MoveTowards(mevcutHiz, 0, motorHizi * 2f * Time.fixedDeltaTime);
            return;
        }
        JointMotor2D motor = teker.motor;
        motor.motorSpeed = mevcutHiz;
        motor.maxMotorTorque = motorGucu;
        teker.motor = motor;
        teker.useMotor = true;
    }

    public float HizKmSaat()
    {
        return rb.linearVelocity.magnitude * 3.6f;
    }
}
