using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : MonoBehaviour
{
    [SerializeField] float sinkDelay = 10f;
    float destroyHeight;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.CompareTag("Ragdoll")) {
            Invoke(nameof(StartSink), 5f);
        }
    }

    public void StartSink() {
        destroyHeight = Terrain.activeTerrain.SampleHeight(transform.position) - 5;
        Collider[] colList = transform.GetComponentsInChildren<Collider>();
        foreach (Collider c in colList) {
            Destroy(c);
        }
        InvokeRepeating(nameof(SinkIntoGround), sinkDelay, 0.1f);
    }

    void SinkIntoGround() {
        transform.Translate(0f, -0.001f, 0f);
        if (transform.position.y < destroyHeight) {
            Destroy(gameObject);
        }
    }
}
