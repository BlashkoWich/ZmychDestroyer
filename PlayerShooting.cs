using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] Transform MuzzleSpawn;
    [SerializeField] GameObject MuzzleFlash;
    [SerializeField] GameObject ImpactStone;
    [SerializeField] GameObject ImpactMetal;
    [SerializeField] AudioClip SingleShotSound;
    [SerializeField] AudioClip RapidShootSound;
    [SerializeField] float RapidDelay = 0.3f;
    [SerializeField] GameObject GrenadeSmoke;
    [SerializeField] AudioClip GrenadeSound;
    [SerializeField] GameObject GrenadeExplosion;
    [SerializeField] GameObject Flames;
    [SerializeField] AudioClip FlameSound;
    [SerializeField] AudioClip PickupFX;
    [SerializeField] GameObject BloodImapct;
    [SerializeField] float ImpactDistance = 0.01f;
    [SerializeField] LayerMask PlayerLayer;
    [SerializeField] LayerMask BarrelLayer;
    [SerializeField] GameObject Crosshair;

    private bool RapidPlay = true;
    private bool RapidShooting = true;
    private bool FireFuel = false;

    private AudioSource PlayerAudio;

    RaycastHit hit;
    void Start()
    {
        PlayerAudio = GetComponent<AudioSource>();
        Flames.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (SaveScript.WeaponID == 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(MuzzleFlash, MuzzleSpawn.position, MuzzleSpawn.rotation);

                SaveScript.AmmoAmt -= 1;

                PlayerAudio.clip = SingleShotSound;
                PlayerAudio.Play();

                Hits();
            }
        }
        if (SaveScript.WeaponID == 2)
        {
            if (Input.GetMouseButton(0))
            {
                Instantiate(MuzzleFlash, MuzzleSpawn.position, MuzzleSpawn.rotation);
                if (RapidPlay == true)
                {

                    PlayerAudio.clip = RapidShootSound;
                    RapidPlay = false;
                    PlayerAudio.loop = true;
                    PlayerAudio.volume = 0.1f;
                    PlayerAudio.pitch = 1f;
                    PlayerAudio.PlayDelayed(0.3f);
                }
                if(RapidShooting == true)
                {
                    RapidShooting = false;
                    StartCoroutine(RapidFire());
                }
            }
            if(Input.GetMouseButtonUp(0))
            {
                RapidPlay = true;
                PlayerAudio.Stop();
            }
        }
        if (SaveScript.WeaponID == 3)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(GrenadeSmoke, MuzzleSpawn.position, MuzzleSpawn.rotation);

                SaveScript.PickupAmmo -= 1;

                PlayerAudio.clip = GrenadeSound;
                PlayerAudio.loop = false;
                PlayerAudio.pitch = 1;
                PlayerAudio.Play();

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 1000))
                {
                    StartCoroutine(Grenade());
                }
                if (Physics.Raycast(ray, out hit, 1000))
                {
                    StartCoroutine(Grenade());
                }
            }
        }
        if (SaveScript.WeaponID == 4)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Flames.gameObject.SetActive(true);
                if (RapidPlay == true)
                {
                    FireFuel = true;
                    PlayerAudio.clip = FlameSound;
                    RapidPlay = false;
                    PlayerAudio.loop = true;
                    PlayerAudio.volume = 0.1f;
                    PlayerAudio.pitch = 0.1f;
                    PlayerAudio.PlayDelayed(0.3f);
                }
            }
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                Flames.gameObject.SetActive(false);
                if(RapidPlay == false)
                {
                    FireFuel = false;
                    PlayerAudio.Stop();
                    RapidPlay = true;
                }
            }
        }
        if(FireFuel == true)
        {
            SaveScript.PickupAmmo -= 1 * Time.deltaTime;
            if(SaveScript.PickupAmmo <= 0)
            {
                Flames.gameObject.SetActive(false);
                FireFuel = false;
                PlayerAudio.Stop();
            }
        }
    }
    void Hits()
    {
        Ray ray = Camera.main.ScreenPointToRay(Crosshair.transform.position);
        if(Physics.Raycast(ray, out hit, 1000, ~PlayerLayer))
        {
            if (hit.transform.tag == "Stone")
            {
                Instantiate(ImpactStone, hit.point, Quaternion.LookRotation(hit.normal));
            }
            if(hit.transform.tag == "Metal")
            {
                {
                    Instantiate(ImpactMetal, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
            if (hit.transform.tag == "Minion")
            {
                {
                    hit.transform.gameObject.SendMessageUpwards("MinionDeath");
                    Instantiate(BloodImapct, hit.point + hit.normal * ImpactDistance, Quaternion.LookRotation(hit.normal));
                }
            }
        }
        if (Physics.Raycast(ray, out hit, 1000, BarrelLayer))
            if (hit.transform.tag == "ExplodingBarrel")
            {
                {
                    hit.transform.gameObject.SendMessage("Explode");
                }
            }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RapidFire"))
        {
            SaveScript.WeaponID = 2;
            SaveScript.WeaponName = "Rapid Fire";
            SaveScript.PickupAmmo = 1500f;
            PickupSound();
            Destroy(other.gameObject, 0.2f);

        }
        if (other.gameObject.CompareTag("HealthPack") && SaveScript.HealthAmt < 100)
        {
            SaveScript.HealthAmt = 100;
            PickupSound();
            Destroy(other.gameObject, 0.2f);
        }
            if (other.gameObject.CompareTag("GrenadeAmmo"))
        {
            SaveScript.WeaponID = 3;
            SaveScript.WeaponName = "Grenade Launcher";
            SaveScript.PickupAmmo = 10f;
            PickupSound();
            Destroy(other.gameObject, 0.2f);
        }
        if (other.gameObject.CompareTag("Flamethrower"))
        {
            SaveScript.WeaponID = 4;
            SaveScript.WeaponName = "Flame Thrower";
            SaveScript.PickupAmmo = 100f;
            PickupSound();
            Destroy(other.gameObject, 0.2f);
        }
    }

    void PickupSound()
    {
        PlayerAudio.clip = PickupFX;
        PlayerAudio.loop = false;
        PlayerAudio.pitch = 1f;
        PlayerAudio.Play();
    }


    IEnumerator RapidFire()
    {
        yield return new WaitForSeconds(RapidDelay);

        SaveScript.PickupAmmo -= 1;
        Hits();
        RapidShooting = true;
    }
    IEnumerator Grenade()
    {
        yield return new WaitForSeconds(0.3f);

        Instantiate(GrenadeExplosion, hit.point, Quaternion.LookRotation(hit.normal));

    }
}
