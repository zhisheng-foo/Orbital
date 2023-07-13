using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Weapon : Collidable
{
    public int damagePoint = 1;
    public float pushForce = 5.0f;

    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    //Internal Cooldown for swing, currently set at 0.5seconds
    private float cooldown = 0.5f;
    private float lastSwing;
    private Animator wepAnim;

    public bool swinging;

    public Player player;
    private Animator playerAnim;
    private string ATTACK_ANIMATION = "Attack";

    [SerializeField]
    public AudioSource weaponSoundEffect;

    [SerializeField]
    private AudioClip receiveWeaponSound;

    [SerializeField]
    private float receiveWeaponVolume = 0.7f;


    protected override void Start()
    {
        base.Start();
        weaponSoundEffect = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnim = player.GetComponent<Animator>();
        wepAnim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space) && SceneManager.GetActiveScene().name != "Start Game" 
            && SceneManager.GetActiveScene().name != "Victory 1 - 0")
        {
            if (Time.time - lastSwing > cooldown)
            {

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    wepAnim.SetTrigger("Attack");
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    wepAnim.SetTrigger("Attack");
                }
                else
                {
                    wepAnim.SetTrigger("Attack");
                }
                lastSwing = Time.time;
                StartCoroutine(Swing());
            }
        }
    }


    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter" && swinging == true)
        {
            if (coll.name == "Player")
            {
                return;
            }

            Damage dmg = new Damage()
            {
                damageAmount = damagePoint,
                origin = transform.position,
                pushForce = pushForce
            };

            coll.SendMessage("ReceiveDamage", dmg);

            if (coll.name != "Boss" && coll.name != "Boss_2" && coll.name != "Boss_3")
            {
                // Apply the push force to the enemy over time
                Vector3 pushDirection = (coll.transform.position - transform.position).normalized * 2.0f;
                StartCoroutine(PushEnemy(coll.transform, pushDirection));
            }

            swinging = false;
        }
    }


    private IEnumerator PushEnemy(Transform enemyTransform, Vector3 pushDirection)
    {
        float pushedDistance = 1f;
        float pushDuration = 0.5f; // Adjust the duration as needed

        while (pushedDistance < pushForce)
        {
            float pushStep = pushForce * Time.deltaTime / pushDuration;
            enemyTransform.position += pushDirection * pushStep;
            pushedDistance += pushStep;
            yield return null;
        }
    }


    private bool isPlayingSound = false; 

    private IEnumerator Swing()
    {
        playerAnim.SetBool(ATTACK_ANIMATION, true);
        swinging = true;

        if (!isPlayingSound) 
            StartCoroutine(PlayWeaponSound());

        yield return new WaitForSeconds(1.2f);

        playerAnim.SetBool(ATTACK_ANIMATION, false);
    }

    private IEnumerator PlayWeaponSound()
    {
        isPlayingSound = true; 

        // Play the receive damage sound
        weaponSoundEffect.PlayOneShot(receiveWeaponSound, receiveWeaponVolume);

        yield return new WaitForSeconds(receiveWeaponSound.length);

        isPlayingSound = false;
    }
}



