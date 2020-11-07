using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController controller;
    private AudioSource audioSource;
    private Material material;

    private Vector3 ballPos;
    public float speed;
    public float automaticSpeed;
    private bool groundLevel = true;
    private int laneNumber = 0;
    public int health = 3;
    public int score = 0;
    public bool gamdeMode = false;
    //SFX
    public AudioClip healSFX;
    public AudioClip scoreSFX;
    public AudioClip scoreDownSFX;
    public AudioClip damageSFX;

    // Start is called before the first frame update
    //A & D Move Horizintal
    void Start()
    {
        controller = this.GetComponent<CharacterController>();
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        float horizontalMovment = Input.GetAxis("Horizontal") * speed;
        ballPos = new Vector3(horizontalMovment, 0f, automaticSpeed);
        if (groundLevel)
        {
            ballPos.y = 0f;
        }
        else
        {
            ballPos.y = 5f;
        }
        if (Input.GetButtonDown("Jump"))
        {
            if (groundLevel)
            {
                groundLevel = false;  // I am Down Going Up
            }
            else
            {
                groundLevel = true;  // I am Up Going down
                ballPos.y = -100f;
            }

        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (laneNumber == 0 || laneNumber == 1)
            {
                //you can move left
                laneNumber--;
                ballPos.x = -40f;
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            if (laneNumber == 0 || laneNumber == -1)
            {
                //you can move right
                laneNumber++;
                ballPos.x = 40f;
            }

        }
        controller.Move(ballPos);
    }

    private void HealingPlayer(Collider other)
    {
        Destroy(other.gameObject);
        health++;
        audioSource.PlayOneShot(healSFX);
        print(health);
    }
    private void OrbTaken(Collider other)
    {
        //Assuming Normal Mode
        
            
        if (gamdeMode) {
            print("Normal Mode");
            if (other.gameObject.GetComponent<Renderer>().material.color == this.gameObject.GetComponent<Renderer>().material.color)
            {
                audioSource.PlayOneShot(scoreSFX);

            }
            else
            {
                audioSource.PlayOneShot(scoreDownSFX);
            }
        }
        else {
            print("Inverted Mode");
            if (other.gameObject.GetComponent<Renderer>().material.color == this.gameObject.GetComponent<Renderer>().material.color)
            {
                audioSource.PlayOneShot(scoreDownSFX);
            }
            else
            {
                audioSource.PlayOneShot(scoreSFX);
            }
        }
            
        
        Destroy(other.gameObject);
       
    }
    private void WallHit(Collider other) { 
        health--;
        audioSource.PlayOneShot(damageSFX);
        Destroy(other.gameObject);
    }
    public void OnTriggerEnter(Collider other)
    {
        print("Hello");
        print(gamdeMode);
        if (other.gameObject.CompareTag("health"))
        {
            HealingPlayer(other);
        }
         if (other.gameObject.CompareTag("socreOrb"))
        { 
                OrbTaken(other);
            
        }
        if (other.gameObject.CompareTag("wall"))
        {
            WallHit(other);

        }

    }

}
