using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieScript : MonoBehaviour
{
    [SerializeField] Material DissolveMat;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material = DissolveMat;
        GetComponent<SpawnEffect>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
