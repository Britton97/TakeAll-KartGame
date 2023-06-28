using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveController : MonoBehaviour
{
    [SerializeField] float waveLength = 1;
    [SerializeField] Vector3 waveOffset = new Vector3(0,1,-1);
    [SerializeField] List<GameObject> waves = new List<GameObject>();
    [SerializeField] Material waveMaterial;
    [SerializeField] UnityEvent<Vector2> vector2Event;
    [HideInInspector] public KartController kartController;
    public float WaveLength
    {
        get { return waveLength; }
        set
        {
            if(value != waveLength)
            {
                waveLength = value;
                ChangeWaveLength();
            }
        }
    }

    private void OnValidate()
    {
        ChangeWaveLength();
        ChangeWaveOffset();
    }

    public void ReceiveInputVector2(Vector2 pInput)
    {

    }

    private void ChangeWaveLength()
    {
        foreach (GameObject wave in waves)
        {
            Vector3 newScale = new Vector3(wave.transform.localScale.x, wave.transform.localScale.y, waveLength);
            wave.transform.localScale = newScale;
        }
    }

    private void ChangeWaveOffset()
    {
        waveMaterial.SetVector("_DirectionalInfluence", waveOffset);
    }
}
