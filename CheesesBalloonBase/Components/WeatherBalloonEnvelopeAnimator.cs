using UnityEngine;

namespace CheeseMods.CheesesBalloonBase.Components;

public class WeatherBalloonEnvelopeAnimator : MonoBehaviour
{
    public Health health;
    public GameObject envelopeGo;
    public Transform envelopeTf;

    public float envelopeDefaultSize;
    public float envelopeMinSize;
    public float envelopeMaxSize;

    public float minAltitude = 0;
    public float maxAltitude = 12000;

    private void Update()
    {
        bool alive = !health.isDead;
        if (alive != envelopeGo.activeSelf)
        {
            envelopeGo.SetActive(alive);
        }

        if (alive)
        {
            float altitude = envelopeTf.position.y - WaterPhysics.waterHeight;
            float currentSize = Mathf.Lerp(envelopeMinSize, envelopeMaxSize, Mathf.InverseLerp(minAltitude, maxAltitude, altitude));
            envelopeTf.localScale = Vector3.one * (currentSize / envelopeDefaultSize);
        }
    }
}
