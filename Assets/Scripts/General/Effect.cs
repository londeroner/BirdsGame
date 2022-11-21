using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Effect : MonoBehaviour
{

    public VisualEffect effect;
    // Start is called before the first frame update
    void Start()
    {
        effect.pause = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
