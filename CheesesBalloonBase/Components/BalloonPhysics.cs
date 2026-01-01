using UnityEngine;
using UnityEngine.PlayerLoop;

namespace CheeseMods.CheesesBalloonBase.Components;

public class BalloonPhysics : MonoBehaviour, IWindReceiver
{
    public Rigidbody rb;
    public GameObject go;
    public Transform tf;
    public Health health;

    public Transform com;
    public Transform col;

    private Vector3 windVel;

    public float maxClimbSpeed;
    public float minClimbSpeed;
    public float maxMagicTraverseSpeed;

    public float climbAcceleration;
    public float decendAcceleration;
    public float magicTraverseAcceleration;

    private float targetAlt;
    private float targetVerticalSpeed;

    private Vector3 targetVel;
    private Vector3D targetPos;
    private bool move;

    public float maxSpinSpeed;
    public float maxSpinAcceleration;
    private float spinSpeed;
    public float minSpinTime;
    public float maxSpinTime;
    private float timer;

    public float terminalVelocity;

    private void OnEnable()
    {
        WindVolumes.RegisterWindReceiver(this);
    }

    private void OnDisable()
    {
        WindVolumes.UnregisterWindReceiver(this);
    }

    private void Start()
    {
        rb.centerOfMass = com.localPosition;
    }

    public void SetTargetAlt(float targetAlt)
    {
        this.targetAlt = targetAlt;
    }

    public void SetTargetPos(Vector3D targetPos)
    {
        move = true;
        this.targetPos = targetPos;
    }

    public void Drift()
    {
        move = false;
    }

    private void FixedUpdate()
    {
        if (!health.isDead)
        {
            UpdateMovement();
        }
        else
        {
            UpdateDeadPhysics();
        }
    }

    private void UpdateMovement()
    {
        float currentAlt = tf.position.y - WaterPhysics.waterHeight;
        targetVerticalSpeed = (targetAlt - currentAlt) * (1f / 100f);

        if (move)
        {
            Vector3 offset = VTMapManager.GlobalToWorldPoint(targetPos) - tf.position;
            targetVel = offset * (1f / 100f);
            targetVel.y = 0;
            targetVel = Vector3.ClampMagnitude(targetVel, maxMagicTraverseSpeed);
        }
        else
        {
            targetVel = windVel;
            targetVel.y = 0;
        }

        UpdatePhysics();
        UpdateSpinning();
    }

    private void UpdatePhysics()
    {
        Vector3 acceleration = -Physics.gravity;

        targetVerticalSpeed = Mathf.Clamp(targetVerticalSpeed, -minClimbSpeed, maxClimbSpeed);
        acceleration += Vector3.up * Mathf.Clamp(targetVerticalSpeed - rb.velocity.y, -decendAcceleration, climbAcceleration);
        Vector3 velOffset = targetVel - rb.velocity;
        velOffset.y = 0;
        acceleration += Vector3.ClampMagnitude(velOffset, magicTraverseAcceleration);

        rb.AddForceAtPosition(acceleration, col.position);
    }

    private void UpdateSpinning()
    {
        timer -= Time.fixedDeltaTime;
        if (timer < 0)
        {
            timer = Random.Range(minSpinTime, maxSpinTime);
            spinSpeed = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        }

        float currentSpinSpeed = rb.angularVelocity.y;
        float targetSpinSpeed = spinSpeed * Mathf.Deg2Rad;

        rb.AddTorque(Vector3.up * Mathf.Clamp((targetSpinSpeed - currentSpinSpeed) / Time.fixedDeltaTime, -maxSpinAcceleration * Mathf.Deg2Rad, maxSpinAcceleration * Mathf.Deg2Rad));
    }

    private void UpdateDeadPhysics()
    {
        Vector3 airspeed = rb.velocity - windVel;
        if (airspeed.magnitude > terminalVelocity)
        {
            rb.velocity = airspeed.normalized * terminalVelocity + windVel;
        }
        if (tf.position.y < WaterPhysics.waterHeight)
        {
            Disapear();
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (health.isDead)
        {
            Disapear();
        }
    }

    private void Disapear()
    {
        go.SetActive(false);
        ExplosionManager.instance.CreateExplosionEffect(ExplosionManager.ExplosionTypes.Medium, tf.position, Vector3.up);
    }

    public void SetWind(Vector3 w)
    {
        windVel = w;
    }

    public Vector3 GetPosition()
    {
        return tf.position;
    }
}
