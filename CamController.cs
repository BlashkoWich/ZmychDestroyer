using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    private Animator Anim;
    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Anim.SetBool("AimCam", true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            Anim.SetBool("AimCam", false);
        }
    }
}
