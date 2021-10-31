using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawn : MonoBehaviour
{
    [SerializeField] GameObject RunningMinion;
    [SerializeField] Transform SpawnPlace;
    private bool CanSpawn = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SaveScript.MinionCount <= 10)
        {
            if (CanSpawn == true)
            {
                CanSpawn = false;
                StartCoroutine(Spawning());
            }
        }
        IEnumerator Spawning()
        {
            yield return new WaitForSeconds(0.1f);
            Instantiate(RunningMinion, SpawnPlace.position, SpawnPlace.rotation);
            SaveScript.MinionCount += 1;
            yield return new WaitForSeconds(1.5f);
            Instantiate(RunningMinion, SpawnPlace.position, SpawnPlace.rotation);
            SaveScript.MinionCount += 1;
            yield return new WaitForSeconds(2f);
            Instantiate(RunningMinion, SpawnPlace.position, SpawnPlace.rotation);
            SaveScript.MinionCount += 1;
            yield return new WaitForSeconds(2.5f);
            Instantiate(RunningMinion, SpawnPlace.position, SpawnPlace.rotation);
            SaveScript.MinionCount += 1;
            yield return new WaitForSeconds(1f);
            Instantiate(RunningMinion, SpawnPlace.position, SpawnPlace.rotation);
            SaveScript.MinionCount += 1;
            yield return new WaitForSeconds(3f);
            Instantiate(RunningMinion, SpawnPlace.position, SpawnPlace.rotation);
            SaveScript.MinionCount += 1;
            yield return new WaitForSeconds(3.5f);
            Instantiate(RunningMinion, SpawnPlace.position, SpawnPlace.rotation);
            SaveScript.MinionCount += 1;
            CanSpawn = true;
        }
    }
}
