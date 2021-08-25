using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   
    [Header("Player")]
    [SerializeField] float moveSpeed;
    [SerializeField] bool mousePlayer;
    [SerializeField] float health=500;
    [SerializeField] GameObject destroyParticle;


    [Header("Laser")]
    [SerializeField] GameObject laserProjectile;

    [SerializeField] AudioClip laserSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;

    [SerializeField] Level level;


    float FireCooldown =0;

    //Var init
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    float screenUnitWidth = 9f;
    float screenUnitHeight = 16f;

    // Start is called before the first frame update
    void Start()
    {
        SetupBoundaries();     
    }

    private void SetupBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0.02f, 0, 0)).x;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(0.98f, 0, 0)).x;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0.10f, 0)).y;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 0.98f, 0)).y;
    }

    // Called once per frame
    void Update()
    {
        Move();
        FireLaser();
    }

    private void Move()
    {
        //Ship controls
        if (!mousePlayer)
        {
            var deltaX = Input.GetAxisRaw("Horizontal") * Time.deltaTime * moveSpeed;
            var deltaY = Input.GetAxisRaw("Vertical") * Time.deltaTime * moveSpeed;
            var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
            var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
            transform.position = new Vector2(newXPos, newYPos);

            //Ship rotation while moving horizontally
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, shipRotation(deltaX), transform.eulerAngles.z);
        }
        else
        {
            

            float mouseXPosInUnits = (Input.mousePosition.x / Screen.width * screenUnitWidth);
            float mouseYPosInUnits = ((Input.mousePosition.y) / Screen.height * screenUnitHeight);

            Vector3 dirVector = new Vector3(mouseXPosInUnits - transform.position.x, mouseYPosInUnits - transform.position.y - 16,0);
            if(dirVector.magnitude > 1.0f)
            {
                dirVector.Normalize();
                float maxComponent = Mathf.Max(Mathf.Abs(dirVector.x), Mathf.Abs(dirVector.y));
                dirVector = dirVector / maxComponent;
            }
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x + (dirVector.x * Time.deltaTime * moveSpeed), xMin, xMax),
                Mathf.Clamp(transform.position.y + (dirVector.y * Time.deltaTime * moveSpeed), yMin, yMax), 
                0);

            //Debug.Log(Input.mousePosition.y + "    " + mouseYPosInUnits);

            //Ship rotation while moving horizontally
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, shipRotation(dirVector.x), transform.eulerAngles.z);
        }
        
    }

    private float shipRotation(float var)
    {
        if (!mousePlayer)
        {
            if (var == 0)
            {
                return 0;
            }
            else
            {
                return 45 * Mathf.Sign(var);
            }
        }
        else
        {
            return 40 * var;
        }
        
    }

    private void FireLaser()
    {
        if (Input.GetButton("Fire1") && FireCooldown<=0 && !mousePlayer)
        {
            GameObject laser = Instantiate(
                laserProjectile, 
                transform.position, 
                Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 10.0f);
            AudioSource.PlayClipAtPoint(laserSound, Camera.main.transform.position,0.75f);

            FireCooldown = 30;
        }
        else if(Input.GetButton("Fire2") && FireCooldown <= 0 && mousePlayer)
        {
            GameObject laser = Instantiate(
                laserProjectile,
                transform.position,
                Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 10.0f);
            AudioSource.PlayClipAtPoint(laserSound, Camera.main.transform.position,0.75f);

            FireCooldown = 30;
        }
        else if(FireCooldown > 0)
        {
            FireCooldown= FireCooldown- 1*Time.deltaTime * 150;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {       
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if(!damageDealer) { return; }
        ProcessHit(damageDealer);

    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        damageDealer.Hit();
        health -= damageDealer.GetDamage();

        if(damageDealer.GetComponent<Enemy>())
        {
            damageDealer.GetComponent<Enemy>().SelfDestruct();
        }

        

        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f, 1);
        StartCoroutine(Recolor());

        if (health <= 0)
        {
            SelfDestruct();
        }
    }

    private IEnumerator Recolor()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    private void SelfDestruct()
    {
        Destroy(gameObject);
        GameObject vfx = Instantiate(destroyParticle, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        FindObjectOfType<EnemySpawner>().Players--;
        //Debug.Log(FindObjectOfType<EnemySpawner>().Players);
        
        if (FindObjectOfType<EnemySpawner>().Players == 0)
        {
            FindObjectOfType<Level>().LoadGameOverScene();
        }
        
    }

    public float GetHealthPercent()
    {
        return health / 500;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
    }
}
