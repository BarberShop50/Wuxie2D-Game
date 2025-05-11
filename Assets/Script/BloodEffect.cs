using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffect : MonoBehaviour
{
    // Start is called before the first frame update
    public float timeDestory;
    void Start()
    {
        Destroy(gameObject, timeDestory);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
