using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Security.Cryptography.X509Certificates;
using static SceneManager;

public class Character : MonoBehaviour
{
    private CharacterController controller;
    private AudioSource audioSource;
    private Material material;
    public GameObject sweeper;

    private Vector3 ballPos;
    private float speed =0.5f;
    private float automaticSpeed =0.5f;
    private int disabler = 1;
    private bool groundLevel = true;
    private int laneNumber = 0;
    public int health = 3;
    public int score = 0;
    private bool gamdeMode = true; //true is normal
    public float spawnDistance;

    //Time Settings
    private float timeElapsed = 0;
    private int timeElapsedInteger = 0;
    private int lastTimeChanged = 0;
    private bool warning = true;
    private int previousSpawn = 0;
    private bool jumpAndroid = false;

    //SFX
    public AudioClip healSFX;
    public AudioClip scoreSFX;
    public AudioClip scoreDownSFX;
    public AudioClip damageSFX;
    public AudioClip deathSFX;
    public AudioClip colorSFX;
    public AudioClip jumpSFX;
    public AudioClip OST;
    public AudioClip MenuTheme;

    //Colors 
    public Material brbl;
    public Material yellow;
    public Material blue;

    //Game Objects (To Spawn)

    public GameObject healthOrb;
    public GameObject blueOrb;
    public GameObject yellowOrb;
    public GameObject purpleOrb;
    public GameObject wall1;
    public GameObject wall2;
    public GameObject wall3;
    public GameObject bottomPlate;
    public GameObject upperPlate;

    //Camera
    public Camera mainCamera;
    public Camera cameraUp;
    public Camera cameraDown;
    public Camera sideCamera;
    private Vector3 cameraPos;
    private bool wasTheLastCameraUp;
    private bool sideCameraActive = false;

    //Pause Menu nad HUD
    GameObject[] pauseObjects;
    private bool paused = false;
    public Text scoreHUD;
    public Text healthHUD;
    GameObject[] hudObjects;
    GameObject[] gameOverObjects;

    // Start is called before the first frame update
    //A & D Move Horizintal
    void Start()
    {
        controller = this.GetComponent<CharacterController>();
        audioSource = this.GetComponent<AudioSource>();
        this.gameObject.GetComponent<Renderer>().material = brbl;
        mainCamera.enabled = true;
        cameraUp.enabled = false;
        audioSource.PlayOneShot(OST, 0.3F);
        //Pause Data
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("showOnPause");
        hudObjects = GameObject.FindGameObjectsWithTag("hud");
        gameOverObjects = GameObject.FindGameObjectsWithTag("gameOver");
        hidePaused();
    }

    // Update is called once per frame
    void Update()
    {

        timeElapsed += Time.deltaTime;
        timeElapsedInteger = (int)Math.Round(timeElapsed);
        updateHUD();
        MoveCharacter();
        ChangeSphereColor();
        spawnObjects();
        cheats();
        audioSource.mute = SceneManager.muted;
        //updateSpeed();


    }

    private void updateHUD() { 
        healthHUD.text = "Health: "+health;
        scoreHUD.text = "Score: "+score;    
    }
    private void MoveCharacter()
    {
        pauseButton();
        if (!paused) {
            if (Input.GetButtonDown("Fire3") ) {
                
                if (!sideCameraActive) {
                    print("CAM S");
                    //if active disable
                    sideCameraActive = true;
                    setSideCamera(true);
                    
                }
                else
                {
                    sideCameraActive = false;
                    setSideCamera(false);
                    
                }
            }
            float horizontalMovment = (Input.GetAxis("Horizontal")+ Input.acceleration.x*2.0f) * speed;
            //float horizontalMovment = Input.acceleration.x * speed;
            ballPos = new Vector3(horizontalMovment, 0f, automaticSpeed);
            cameraPos = new Vector3(0f, 0f, automaticSpeed);
            if (groundLevel)
            {
                ballPos.y = 0f;
            }
            else
            {
                ballPos.y = 5f;
            }
            if (Input.GetButtonDown("Jump") || jumpAndroid)
            {
                if (groundLevel)
                {
                    mainCamera.enabled = false;
                    cameraUp.enabled = true && !sideCameraActive;
                    groundLevel = false;  // I am Down Going Up
                    gamdeMode = false;
                    audioSource.PlayOneShot(jumpSFX, 0.8F);
                }
                else
                {
                    mainCamera.enabled = true && !sideCameraActive;
                    cameraUp.enabled = false;
                    groundLevel = true;  // I am Up Going down
                    gamdeMode = true;
                    ballPos.y = -100f;
                    audioSource.PlayOneShot(jumpSFX, 0.8F);
                }
                jumpAndroid = false;
            }

            if (this.transform.position.x > 47.0f)
            {
                //print("Error in Location");
                ballPos.x = -1;
            }
            if (this.transform.position.x < -47.0f)
            {
                //print("Error in Location");
                ballPos.x = 1;
            }
            ballPos.x = ballPos.x * disabler;
            controller.Move(ballPos);
        
            sweeper.transform.Translate(new Vector3(0, 0, ballPos.z));

            //Update Camera Location

            mainCamera.transform.position = new Vector3(0f, 24.38f, this.transform.position.z - 25.0f);
            cameraUp.transform.position = new Vector3(0f, 45f, this.transform.position.z - 40.0f);
            sideCamera.transform.position = new Vector3(147f, 37f, this.transform.position.z - 120.0f);
        }
    }
    public void jump() {
        jumpAndroid = true;
    }
    public void changeCamera()
    {
        if (!sideCameraActive)
        {
            print("CAM S");
            //if active disable
            sideCameraActive = true;
            setSideCamera(true);

        }
        else
        {
            sideCameraActive = false;
            setSideCamera(false);

        }
    }

    public void pauseAndroid() {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            paused = true;
            showPaused();
            paused = true;

        }
        else if (Time.timeScale == 0)
        {
            Debug.Log("high");
            Time.timeScale = 1;
            paused = false;
            hidePaused();
            paused = false;
        }
    }
    private void spawnObjects() {
        
        Vector3 current_pos  = this.transform.position;
        int z_pos = (int)this.transform.position.z;
        float z_pos_float = this.transform.position.z;
        float y_pos = this.transform.position.y;
        
       // print("Z Pos is "+z_pos);
        
        if (z_pos>0 && z_pos%50==0 && z_pos != previousSpawn) { //spawn after 50 steps
            previousSpawn = z_pos;
            //Spawn Probability
            System.Random r = new System.Random();
            int downORup = r.Next(1,100);
            int wallORball = r.Next(1,100);
            int Lane = r.Next(1,3);

            float y_trans; 

            if (wallORball <= 50) {
                //Spawn a Wall

                //Check Wall Plate
                if(downORup <= 40) { 
                    //Wall should be spawned up
                    y_trans = 46.0f;
                }
                else {
                    //Wall should be spawned down
                    y_trans = 6.8f;
                }
                int whichWall = r.Next(1, 3);
                //print("Should Print Wall: "+whichWall);
                if (whichWall == 1) {
                    //Spawn Wall1 -44 0 44
                    GameObject tmp = Instantiate(wall1);
                    if (Lane % 2 == 0) {
                        //Spawn Mid
                        tmp.transform.position = new Vector3(20.0f, y_trans, spawnDistance+z_pos_float);
                    }
                    else { 
                        if(Lane == 3) {
                            //Spawn Right
                            tmp.transform.position = new Vector3(40.0f, y_trans, spawnDistance + z_pos_float);
                        }
                        else {
                            tmp.transform.position = new Vector3(10.0f, y_trans, spawnDistance + z_pos_float);
                        }
                    }
                    return;
                }
                else if (whichWall == 2) {
                    //Spawn Wall2 left x=-34 wall(right) x=12
                    GameObject tmp = Instantiate(wall2);
                    if (Lane == 1) {
                        //Right Biased
                        tmp.transform.position = new Vector3(-30.0f, y_trans, spawnDistance + z_pos_float);
                    }
                    else {
                        tmp.transform.position = new Vector3(12.0f, y_trans, spawnDistance + z_pos_float);
                    }

                    return;

                }
                else if (whichWall == 3) {
                    //Spawn Wall3 lol x=0
                    GameObject tmp = Instantiate(wall3);
                    tmp.transform.position = new Vector3(12.0f, y_trans, spawnDistance + z_pos_float);
                    return;
                }

            }
            else { 
                int colorint = r.Next(1,5); // blue prpl yllow HEALTH
                int howManyProb = r.Next(1, 100);
               // float [] xLane = {10.0f,20f,40.0f};
                float[] xLane = { -40.0f, 6.0f, 40.0f };
                int lane = r.Next(0, 2);
                float y_axis;
                if (downORup <= 40) {
                    //should be spawned up
                    y_axis = 50.0f;
                }
                else {
                    y_axis = 5.0f;
                }
                // Y IS 49 AND -1
                
                if (true) { 
                    //1 ORBS
                    if(colorint == 1) {
                        GameObject blueOrbtmp = Instantiate(blueOrb);
                        blueOrbtmp.transform.position = new Vector3(xLane[lane], y_axis, spawnDistance + z_pos_float);
                    }
                    else if (colorint == 2)
                    {
                        GameObject purpleOrbtmp = Instantiate(purpleOrb);
                        purpleOrbtmp.transform.position = new Vector3(xLane[lane], y_axis, spawnDistance + z_pos_float);
                    }
                    else if (colorint == 3)
                    {
                        GameObject yellowOrbtmp = Instantiate(yellowOrb);
                        yellowOrbtmp.transform.position = new Vector3(xLane[lane], y_axis, spawnDistance + z_pos_float);
                    }
                    else if (colorint == 4)
                    {
                        int risk = r.Next(0, 10);
                        if(risk%2 == 0) {
                            GameObject healthOrbtmp = Instantiate(healthOrb);
                            healthOrb.transform.position = new Vector3(xLane[lane], y_axis, spawnDistance + z_pos_float);
                        }
                        else {
                            GameObject yellowOrbtmp = Instantiate(yellowOrb);
                            yellowOrbtmp.transform.position = new Vector3(xLane[lane], y_axis, spawnDistance + z_pos_float);
                        }
                       
                    }
                }
                //Add New Plates
                GameObject tmp1 = Instantiate(bottomPlate);
                tmp1.transform.position = new Vector3(3.0f, 0, spawnDistance + z_pos_float+300.0f);
                //tmp1.transform.position = new Vector3(3.0f, 0, spawnDistance + z_pos_float+300.0f);
                GameObject tmp2 = Instantiate(upperPlate);
                tmp2.transform.position = new Vector3(3.0f, 2.0f, spawnDistance + z_pos_float + 300.0f);

            }
        }

        //print("Current Z is"+z_pos);
        //GameObject tmp = Instantiate(healthOrb);
        //tmp.transform.position = new Vector3(0.0f, tmp.transform.position.y, 0.0f);
    }
    private void updateSpeed() {
        if (score % 50 == 0 && score!=0) { 
            automaticSpeed = automaticSpeed * 1.1f * disabler; //Increase speed by 5%
            print("Automatic Speed is"+automaticSpeed);
        }
    }
    private void HealingPlayer(Collider other)
    {
        Destroy(other.gameObject);
        if (health != 3) {
            health++;
        }
        audioSource.PlayOneShot(healSFX,0.6F);
        //print(health);
    }
    private void destroy() {
        this.enabled= false; 
        automaticSpeed = 0;
        disabler = 0;

    }
    private void OrbTaken(Collider other)
    {
        //Assuming Normal Mode
        
            
        if (gamdeMode) {
            //print("Normal Mode");
            if (other.gameObject.GetComponent<Renderer>().material.color == this.gameObject.GetComponent<Renderer>().material.color)
            {
                audioSource.PlayOneShot(scoreSFX);
                score =score+10;
                updateSpeed();

            }
            else
            {
                audioSource.PlayOneShot(scoreDownSFX);
                score-=5;
                if (score <= 0)
                {
                    score = 0;
                }
            }
        }
        else {
            //print("Inverted Mode");
            if (other.gameObject.GetComponent<Renderer>().material.color == this.gameObject.GetComponent<Renderer>().material.color)
            {
                audioSource.PlayOneShot(scoreDownSFX);
                score-=5;
                if (score <= 0) { 
                    score=0;
                }
            }
            else
            {
                audioSource.PlayOneShot(scoreSFX);
                score = score+10;
                updateSpeed();
            }
        }
            
        //print("Score: "+score);
        
        Destroy(other.gameObject);
       
    }
    private void WallHit(Collider other) { 
        health = health -1;
        if (health == 0)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(deathSFX);
            //you died
            foreach (GameObject g in gameOverObjects)
            {
                g.SetActive(true);

            }
            
            audioSource.PlayOneShot(MenuTheme, 0.6F);
            destroy();
        }
        else {
            audioSource.PlayOneShot(damageSFX);
            Destroy(other.gameObject);
        }

    }
    public void OnTriggerEnter(Collider other)
    {
       // print("Hello");
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
    private void ChangeSphereColor() {
        if(timeElapsedInteger % 13 == 0 && lastTimeChanged-2 != timeElapsedInteger-2 && warning) {
            warning= false;
            //audioSource.PlayOneShot(colorSFX);
        }
        if (timeElapsedInteger % 15 == 0 && lastTimeChanged != timeElapsedInteger) {

            lastTimeChanged = timeElapsedInteger;
            // print("COlor change");
            print(lastTimeChanged);
            audioSource.PlayOneShot(colorSFX);
            System.Random rd = new System.Random();

            if (timeElapsedInteger>=15 && timeElapsed < 30) {
                //Blue Time
                this.gameObject.GetComponent<Renderer>().material = blue;
                warning = true;
            }
            else if (timeElapsedInteger+1 > 30 && timeElapsed < 45)
            {
                //Blue Time
                this.gameObject.GetComponent<Renderer>().material = yellow;
                warning = true;
            }
            else if (timeElapsedInteger >= 45 && timeElapsed < 60)
            {
                //Blue Time
                this.gameObject.GetComponent<Renderer>().material = brbl;
                timeElapsedInteger = 0;
                warning = true;
            }

           
            //Change Color
        }
    }
    
    public void setSideCamera( bool state) {
        if (state) {
            if (mainCamera.enabled) { 
                wasTheLastCameraUp = false;
            }
            else {
                wasTheLastCameraUp = false;
            }
            mainCamera.enabled = false;
            cameraUp.enabled = false;
        }
        else {
            if (wasTheLastCameraUp) {
                mainCamera.enabled = false;
                cameraUp.enabled = true;
            }
            else {
                mainCamera.enabled = true;
                cameraUp.enabled = false;
            }
        }
    }
    public void pauseButton() {

        //uses the p button to pause and unpause the game
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                paused = true;
                showPaused();
                
            }
            else if (Time.timeScale == 0)
            {
                Debug.Log("high");
                Time.timeScale = 1;
                paused = false;
                hidePaused();
                paused = false;
            }
        }
    }
    public void Reload()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
    public void Reload2()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    //controls the pausing of the scene
    public void pauseControl()
    {   
        print("CLicked");
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            paused = true;
            showPaused();
            paused = true;
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            paused = false;
            hidePaused();
            paused = false;
        }
    }
    //shows objects with ShowOnPause tag
    public void showPaused()
    {
        paused = true;
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(true);
            paused = true;

        }
        foreach (GameObject g in hudObjects)
        {   
            
            g.SetActive(false);
            paused = true;



        }
    }
    public void hidePaused()
    {
        paused = false;
        foreach (GameObject g in pauseObjects)
        {
            
            g.SetActive(false);
            paused = false;
        }
        foreach (GameObject g in hudObjects)
        {
            g.SetActive(true);
            paused = false;

        }
        foreach (GameObject g in gameOverObjects)
        {
            g.SetActive(false);
            paused = false;

        }

    }

    //loads inputted level
    public void MainMenu()
    {   print("Main Menu");
        Application.LoadLevel("MainMenu");
    }
    public void MainMenu2()
    {
        print("Main Menu");
        Application.LoadLevel("MainMenu");
    }
    private void cheats() {
        if (Input.GetKeyDown(KeyCode.R)) {

            System.Random r = new System.Random();
            int num = r.Next(1, 5);
            audioSource.PlayOneShot(colorSFX);
            if(num == 3) {
                this.gameObject.GetComponent<Renderer>().material = brbl;
            }
            if (num == 2) {
                this.gameObject.GetComponent<Renderer>().material = blue;
            }
            else {
                this.gameObject.GetComponent<Renderer>().material = yellow;
            }
            
        }
        if(Input.GetKeyDown(KeyCode.E)){
            if (health != 3) { 
                health++;
                audioSource.PlayOneShot(healSFX, 0.6F);
                updateHUD();
            }
        }
        if(Input.GetKeyDown(KeyCode.Q)){
            score +=10;
            updateSpeed();
        }
    }

}
