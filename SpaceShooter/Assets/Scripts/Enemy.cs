using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] float attackSpeed = 80;
    [SerializeField] bool randomizeFire = true;
    [SerializeField] float shotCounter;

    [SerializeField] GameObject projectile;
    [SerializeField] GameObject destroyParticle;

    [SerializeField] AudioClip deathSound;
    [SerializeField][Range(0,1)] float deathSoundVolume = 0.75f;

    [SerializeField] AudioClip laserSound;

    float fullhp;

    //float lifetime=0;

    // Start is called before the first frame update
    void Start()
    {
        fullhp = health;
        shotCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        IncreaseCounter();       
    }

    private void IncreaseCounter()
    {
        
        if (UnityEngine.Random.Range(0,1.0f) >= 0.8 || !randomizeFire)
        {
            shotCounter=shotCounter+1* Time.deltaTime * 150;
            if (shotCounter >= attackSpeed)
            {
                Fire();
                shotCounter = 0;
            }
        }
    }

    private void Fire()
    {
        //Debug.Log("Firing!" + " DeltaTime: " + Time.deltaTime);

        GameObject laser = Instantiate(
            projectile,
            transform.position,
            Quaternion.identity) as GameObject;

        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -10.0f);
        AudioSource.PlayClipAtPoint(laserSound, Camera.main.transform.position,0.55f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);

    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        damageDealer.Hit();
        health -= damageDealer.GetDamage();

        gameObject.GetComponent<SpriteRenderer>().color = new Color(1,0.5f,0.5f,1);
        StartCoroutine(Recolor());

        if (health <= 0)
        {
            SelfDestruct();
        }
    }

    private IEnumerator Recolor()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
    }

    public void SelfDestruct()
    {
        float PointReward = Mathf.Pow(1.05f, FindObjectOfType<EnemySpawner>().Waves) * fullhp;  
        GameObject vfx = Instantiate(destroyParticle, transform.position, transform.rotation);
        //Debug.Log(FindObjectOfType<EnemySpawner>().Waves+"|"+ Mathf.Pow(1.05f, FindObjectOfType<EnemySpawner>().Waves)+"|"+PointReward);
        FindObjectOfType<GameState>().UpdateScore((int)PointReward);
        Destroy(gameObject);
        Destroy(vfx, 2);
        AudioSource.PlayClipAtPoint(deathSound,Camera.main.transform.position,deathSoundVolume);
        FindObjectOfType<EnemySpawner>().Counter--;

        
    }
}
