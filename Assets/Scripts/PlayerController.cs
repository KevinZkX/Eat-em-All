using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(MouseSkill))]
[RequireComponent(typeof(RabbitSkill))]
[RequireComponent(typeof(FragSkill))]
[RequireComponent(typeof(SkunkSkill))]
public class PlayerController : Character {

    public Camera mainCamera;
    public float speed = 5.0f;
    bool InAir = false;
    public Vector3 jump = new Vector3(0, 4, 0);
    Skills[] skills;
    MouseSkill mouseSkill;
    RabbitSkill rabbitSkill;
    FragSkill fragSkill;
    public Transform cheese;
    public PathGenerator pathGenerator;
    public bool canJump;
    Skills active_skill;
    public float vertical;
    public float horizontal;
    public Image detection_bar;
    public float avoidence_factor;
    float trigger = 20;
    int easterEggs;
    public GameObject current_map;
    public GameObject spawn_point;
    public bool game_over;

    //For UI
    SesameHealth health;
    public Transform UI_references;

    bool isDead = false;
    bool slowDown = false;
    float slowDownTimer;
    public static bool isRunning;
    public static bool isWalking;

    public ParticleSystem blood;

    //For Audio

    AudioSource ad_footstep;
    AudioSource ad_meow;
    AudioSource ad_pickup;
    AudioSource ad_lose;
    AudioSource ad_skill;
    AudioSource ad_win;
    AudioSource water_ad;
    AudioSource water1_ad;
    // Use this for initialization
    void Start()
    {            
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        scales = transform.localScale;
        mouseSkill = GetComponent<MouseSkill>();
        rabbitSkill = GetComponent<RabbitSkill>();
        fragSkill = GetComponent<FragSkill>();
        mouseSkill.enabled = false;
        rabbitSkill.enabled = false;
        fragSkill.enabled = false;
        skills = GetComponents<Skills>();
        health = GetComponent<SesameHealth>();
        spawn_point = new GameObject();

        GameObject[] nodes = GameObject.FindGameObjectsWithTag("PathNode");
        foreach (GameObject go in nodes)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), go.GetComponent<Collider>());
        }
        foreach (GameObject go in Rabbit.rabbitNode)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), go.GetComponent<Collider>());
        }

        //audio
        AudioSource[] ad_list = this.GetComponents<AudioSource>();
        ad_footstep = ad_list[0];
        ad_meow = ad_list[1];
        ad_pickup = ad_list[2];
        ad_lose = ad_list[3];
        ad_skill = ad_list[4];
        ad_win = ad_list[5];
        water_ad = ad_list[6];
        water1_ad = ad_list[7];
    }

    void FixedUpdate()
    {
        if (!health.isDead())
        {
            Vector3 temp = Camera.main.transform.forward;
            transform.forward = new Vector3(temp.x, 0, temp.z);
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
            if(Input.GetKeyDown("w"))
            {
                ad_footstep.Play();
            }
            //if (vertical != 0 || horizontal != 0)
            //{
            //   ad_footstep.;
            //}

            if (speed == 5 && (horizontal != 0 || vertical != 0))
            {
                isRunning = true;
            }
            else if (speed == 2 && (horizontal != 0 || vertical != 0))
            {
                isWalking = true;
            }
            else
            {
                isRunning = false;
                isWalking = false;
            }

            vertical *= Time.deltaTime * speed;
            horizontal *= Time.deltaTime * speed;

            transform.Translate(horizontal, 0, vertical);

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Debug.Log("Avoid");
                transform.Translate(new Vector3(horizontal, 0, vertical).normalized * avoidence_factor);
            }

            if (canJump && !InAir && Input.GetKeyDown(KeyCode.Space))
            {
                rigidbody.AddForce(jump, ForceMode.Impulse);
                // ad_footstep.Stop();
                InAir = true;
            }
        }
        else
        {
            if (game_over == false)
            {
                isDead = true;
                health.died = false;
                health.resetHealth(100);
                health.resetHunger(100);
                transform.position = spawn_point.transform.position;
                UI_references.GetComponent<UI_References>().resetCounterTimer();
                isDead = false;
            }
            else
            {
                game_over = true;
            }
        }


        detection_bar.transform.localScale = new Vector3(Wolf.soundDetection / trigger, 0.9f, 0);
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 2);
        if (!health.isDead())
        {
            if (slowDown)
            {
                slowDownTimer += Time.deltaTime;
                speed = 2;
                if (slowDownTimer > 5)
                {
                    speed = 5;
                    slowDown = false;
                }
            }

            useSkill();
            UseCheese();
            attack();
        }
        else
        {
            isDead = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
    }

    void OnCollisionEnter(Collision other)
    {

        if (other.collider.tag == "Ground" || other.collider.tag == "Water")
        {
            current_map = other.collider.transform.parent.gameObject;
            //Debug.Log(current_map.name);
            if (other.collider.transform.parent.name.Contains("Mouse") 
                || other.collider.transform.parent.name.Contains("Frog")
                || other.collider.transform.parent.name.Contains("Rabbit")
                || other.collider.transform.parent.name.Contains("Wolf"))
            {
                spawn_point = current_map.transform.Find("Spawn Point").gameObject;
            }
        }
        if (other.collider.tag == "Key")
        {
            ad_pickup.Play();
            ad_skill.PlayDelayed(0.5f);
        }

        if (other.collider.tag == "Npc" || other.collider.tag == "Frog" || other.collider.tag == "Skunk")
        {
            ApplySkills(other.collider.gameObject.GetComponent<NpcCharacters>().skillID);
            Destroy(other.collider.gameObject);
            transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
            GetComponent<BoxCollider>().size += new Vector3(0.02f, 0.02f, 0.02f);
            health.incrementHunger(10f);
        }

        if(other.collider.tag == "Wolf" && other.gameObject.GetComponent<Wolf>().wolfState == WolfState.Flee)
        {
            other.gameObject.GetComponent<Wolf>().health_point -= 1;
            
            ParticleSystem temp = Instantiate(blood, other.contacts[0].point, Quaternion.identity);
            
            Destroy(temp, 1);
        }
        if (other.collider.tag == "MovingPlatform")
        {
            transform.parent = other.gameObject.transform;
        }

        if (other.collider.name.Contains("Cheese"))
        {
            ad_pickup.Play();
            
            cheese = other.collider.transform;
            cheese.tag = "Cheese";
            cheese.gameObject.GetComponent<PathNode>().enabled = false;
            //Destroy(other.collider.GetComponent<HingeJoint>());
            other.collider.transform.parent = gameObject.transform;
            Monsters.cheese = null;
            UI_references.GetComponent<UI_References>().cheeseTut();
        }

        if (other.collider.name.Contains("Easter Egg"))
        {
            ad_pickup.Play();
            Destroy(other.gameObject);
            easterEggs += 1;
            if (easterEggs == 2)
            {
                GameObject.Find("Big Carrot").GetComponent<Animator>().SetBool("EggsCollected", true);
                Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("Boundary1").GetComponent<Collider>());
            }
        }

        //if (other.collider.tag == "MovingCarrot")
        //{ 
            
        //    other.collider.transform.parent = transform;
        //}

        //if(other.collider.tag == "Water")
        //{
        //    Debug.Log("Water");
        //    ad_meow.Play();
        //    ad_lose.Play();
        //    health.GetComponent<SesameHealth>().decrementHealth(10);
        //}

        InAir = false;
    }

    void OnCollisionStay(Collision col)
    {
        if (col.collider.tag == "MovingPlatform")
        {
            transform.parent = col.transform;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        if (col.collider.tag == "Water")
        {
            Debug.Log("Water");
            water_ad.Play();
            water1_ad.PlayDelayed(0.5f);
            ad_meow.PlayDelayed(0.7f);
            ad_lose.PlayDelayed(1f);
            health.GetComponent<SesameHealth>().decrementHealth(0.1f);
        }
    }
    void OnTriggerStay(Collider col)
    {
        
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.collider.tag == "MovingPlatform")
        {
            transform.parent = null;
        }
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.name);
        if (other.name.Contains("Smoke_Explosion_Loop_01"))
        {
            slowDown = true;
            slowDownTimer = 0;
        }
    }

    void ApplySkills(int skillID)
    {
        Debug.Log("Skill ID: " + skillID);
        foreach (Skills sk in skills)
        {
            if (sk.SkillId == skillID)
            {
                sk.enabled = true;
                if(skillID == 1) //Mouse
                {
                    UI_references.GetComponent<UI_References>().showSkill("mouse");
                    active_skill = sk;
                }
                else if (skillID == 2) //Frog
                {
                    //UI_references.GetComponent<UI_References>().showSkill("frog");
                    sk.Skill();
                }
                else if(skillID == 3) //Rabbit
                {
                    //UI_references.GetComponent<UI_References>().showSkill("rabbit");
                    sk.Skill();
                }
                else if(skillID == 5) //Skunk
                {
                    UI_references.GetComponent<UI_References>().showSkill("skunk");
                    active_skill = sk;
                }
                else
                    active_skill = sk;
            }
            else
                sk.enabled = false;
        }
    }

    void attack()
    {
        if (Input.GetMouseButtonDown(1))
        {
            rigidbody.AddForce(transform.forward * 500);
        }
    }
    void useSkill()
    {
        if (active_skill && Input.GetKeyDown(KeyCode.F))
        {
            ad_skill.Play();
            transform.parent = null;
            ad_skill.Play();
            UI_references.GetComponent<UI_References>().disablePopup();
            active_skill.Skill();
        }
    }

    void UseCheese()
    {
        if (cheese != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ad_pickup.Play();
                cheese.parent = null;
                UI_references.GetComponent<UI_References>().disablePopup();
                cheese.tag = "PathNode";
                cheese.GetComponent<PathNode>().enabled = true;
                cheese.GetComponent<Collider>().enabled = true;
                cheese.gameObject.layer = 8;
                Monsters.cheese = cheese.gameObject;
                pathGenerator.GenerateNewNodeNetwork();
            }
        }
    }

    public bool sesameDied()
    {
        return isDead;
    }
}
