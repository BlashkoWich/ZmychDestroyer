using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAttack : MonoBehaviour
{
    [SerializeField] int DamageAmt = 1;
    [SerializeField] AudioSource MinionAudio;

    private void Start()
    {
        MinionAudio = GetComponentInParent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SaveScript.HealthAmt -= DamageAmt;
            other.transform.gameObject.GetComponent<PlayerMove>().SendMessage("GetHit");
            MinionAudio.Play();
        }
    }
}
