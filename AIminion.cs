using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIminion : MonoBehaviour
{
    [SerializeField] GameObject PlayerTarget;
    [SerializeField] float RunSpeed = 2.3f;
    [SerializeField] float CrawlSpeed = 1.1f;
    [SerializeField] float DragSpeed = 1.1f;
    [SerializeField] float AttackDistance = 1.3f;
    [SerializeField] Collider MinionCol;
    [SerializeField] int MinionType = 1;
    [SerializeField] float RotationSpeed = 2.0f;
    [Tooltip("1=Running, 2=Crawl,3=Drag")]
    private float NavMinionSpeed;
    private NavMeshAgent Nav;
    [SerializeField] GameObject VampireSound;
    private Animator Anim;
    private float DistanceToPlayer;
    private bool CanMove = true;
    private NavMeshObstacle NavObstacle;
    private AnimatorStateInfo MinionInfo;
    // Start is called before the first frame update
    void Start()
    {
        Nav = GetComponent<NavMeshAgent>();
        PlayerTarget = GameObject.FindGameObjectWithTag("Player");
        Anim = GetComponent<Animator>();
        NavObstacle = GetComponent<NavMeshObstacle>();
        NavObstacle.enabled = false;
        if(MinionType == 1)
        {
            NavMinionSpeed = RunSpeed;
        }
        if (MinionType == 2)
        {
            Anim.SetLayerWeight(1, 1);
            NavMinionSpeed = CrawlSpeed;
        }
        if (MinionType == 3)
        {
            Anim.SetLayerWeight(2, 1);
            NavMinionSpeed = DragSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MinionInfo = Anim.GetCurrentAnimatorStateInfo(0);
        if (!MinionInfo.IsTag("Dead"))
            {
            DistanceToPlayer = Vector3.Distance(PlayerTarget.transform.position, transform.position);
            if (DistanceToPlayer < AttackDistance - 0.5)
            {
                Anim.SetBool("Attack", true);
                //MinionCol.enabled = true;
                //Nav.enabled = false;
                CanMove = false;
                //NavObstacle.enabled = true;
                Nav.isStopped = true;
                Vector3 Pos = (PlayerTarget.transform.position - transform.position).normalized;
                Quaternion PosRotation = Quaternion.LookRotation(new Vector3(Pos.x, 0, Pos.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, PosRotation, Time.deltaTime * RotationSpeed);
            }
            else if (DistanceToPlayer > AttackDistance - 0.5)
            {
                Anim.SetBool("Attack", false);
                //MinionCol.enabled = false;
                Nav.isStopped = false;
                //NavObstacle.enabled = false;
                //Nav.enabled = true;
                CanMove = true;
            }
            if (CanMove == true)
            {
                Nav.speed = NavMinionSpeed;
                Nav.SetDestination(PlayerTarget.transform.position);
            }
        }
        if(MinionInfo.IsTag("Dead"))
        {
            VampireSound.SetActive(false);
        }
    }
    public void MinionDeath()
    {
        if (!MinionInfo.IsTag("Dead"))
        {
            Anim.SetTrigger("Death");
            Nav.enabled = false;
            SaveScript.ScoreAmt += 1;
        }
    }
    public void MinionBurned()
    {
        if (!MinionInfo.IsTag("Dead"))
        {
                Anim.SetTrigger("Burned");
                Nav.enabled = false;
                SaveScript.ScoreAmt += 1;
        }
    }

    public void DestroyOnDeath()
    {
        StartCoroutine(WaitForDestroy());
    }
    IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(1.5f);
        SaveScript.MinionCount -= 1;
        Destroy(gameObject);
    }
}
