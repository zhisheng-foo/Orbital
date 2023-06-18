using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Weapon : Collidable
{
    public int damagePoint = 1;
    public float pushForce = 3.0f;

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

        if (Input.GetKeyDown(KeyCode.Space) && SceneManager.GetActiveScene().name != "Start Game")
        {   
            if(Time.time - lastSwing > cooldown)
            {
                
                if(Input.GetKeyDown(KeyCode.DownArrow))
                {
                    wepAnim.SetTrigger("AttackDown");
                }
                else if(Input.GetKeyDown(KeyCode.UpArrow))
                {
                    wepAnim.SetTrigger("AttackUp");
                }
                else
                {
                    wepAnim.SetTrigger("AttackRight");   
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
            swinging = false;
        }
    }

    private bool isPlayingSound = false; // Flag to check if the sound effect is playing

    private IEnumerator Swing()
    {   
        playerAnim.SetBool(ATTACK_ANIMATION, true);
        swinging = true;

        if (!isPlayingSound) // Check if the sound effect is already playing
            StartCoroutine(PlayWeaponSound());

        yield return new WaitForSeconds(1.2f);

        playerAnim.SetBool(ATTACK_ANIMATION, false);
    } 

    private IEnumerator PlayWeaponSound()
    {
        isPlayingSound = true; // Set the flag to indicate that the sound effect is playing

        // Play the receive damage sound
        weaponSoundEffect.PlayOneShot(receiveWeaponSound, receiveWeaponVolume);

        yield return new WaitForSeconds(receiveWeaponSound.length);

        isPlayingSound = false; // Reset the flag after the sound effect has finished playing
    }
}

