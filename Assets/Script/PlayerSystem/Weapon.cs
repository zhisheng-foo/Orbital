using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
The player's primary means of dealing damage to enemy classes
Has a base cooldown between attacks of 0.5f to prevent spam. Is a collidable object
that is controlled by unity's animator.
*/


public class Weapon : Collidable
{
    public int damagePoint = 1;
    public float pushForce = 5.0f;
    private SpriteRenderer spriteRenderer;
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

    private bool isPlayingSound = false; 
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
                StartCoroutine(Swing());
                lastSwing = Time.time;
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
                Vector3 pushDirection 
                = (coll.transform.position - transform.position).normalized * 2.0f;
                StartCoroutine(PushEnemy(coll.transform, pushDirection));
            }

            swinging = false;
        }
    }


    private IEnumerator PushEnemy(Transform enemyTransform, Vector3 pushDirection)
    {
        float pushedDistance = 1f;
        float pushDuration = 0.5f; 

        while (pushedDistance < pushForce)
        {
            float pushStep = pushForce * Time.deltaTime / pushDuration;
            enemyTransform.position += pushDirection * pushStep;
            pushedDistance += pushStep;
            yield return null;
        }
    }  

    private IEnumerator Swing()
    {    
        swinging = true;
        wepAnim.SetTrigger("Attack");
        playerAnim.SetBool(ATTACK_ANIMATION, true);
        weaponSoundEffect.PlayOneShot(receiveWeaponSound, receiveWeaponVolume);

        yield return new WaitForSeconds(0.001f);
        playerAnim.SetBool(ATTACK_ANIMATION, false);
        
    }  
}



