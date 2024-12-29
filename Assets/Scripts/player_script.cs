using NUnit.Framework;
using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class player_script : MonoBehaviour
{
    public float bounce_speed; // The speed at which the ball bonces off platforms
    public float move_speed; // The speed at which the player moves when pressing arrow keys
    public int lives; // Number of lives the player has
    public Vector3 spawnpoint; // Place to reset the player to
    public float respawn_time; // Amount of time before player respawns
    public int move_times = 0; // Amount of times player can move at the start, is public so it can be set to zero after player lands on other platforms

    // Data on the player to make code more fluent
    private GameObject player;
    private Rigidbody rb;

    // Other variables
    private Vector3 death_place; // Default to null but will be set when player dies so particles can spawn
    private GameObject particles; // Reference to death particles
    private bool isPlayerDead = false; // Boolean defining whether player is dead
    private float respawn_time_copy; // Copies respawn time so it isn't lost when it is changed
    private GameObject highest_checkpoint; // The highest value checkpoint hit so far
    private List<GameObject> checkpoints = new List<GameObject>();

    private void Start()
    {
        player = this.gameObject;
        rb = GetComponent<Rigidbody>();
        respawn_time_copy = (respawn_time + 1) - 1;
        highest_checkpoint = GameObject.Find("Checkpoint (0)");
    }
    private void Update()
    {
        // Make player invisible if it is dead
        this.gameObject.GetComponent<Renderer>().enabled = !isPlayerDead;
        // If player is dead run timer to respawn
        if (isPlayerDead)
        {
            respawn_time -= Time.deltaTime;
        }
        // Movement system for moving after landing on a violet platform
        if (move_times > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                rb.AddForce(-bounce_speed * move_speed, 0, 0);
                move_times -= 1;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                rb.AddForce(bounce_speed * move_speed, 0, 0);
                move_times -= 1;
            } 
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb.AddForce(0, bounce_speed * move_speed, 0);
            move_times -= 1;
        }
        // Respawn player and spawn particles if player is dead
        if (lives == 0)
        {
            Debug.Log("You Died!");
            death_place = player.transform.position;
            particles = GameObject.Find("Death Particles");
            particles.transform.position = death_place;
            particles.GetComponent<ParticleSystem>().Play();
            lives = 1;
            isPlayerDead = true;
        }
        // Reset player velocity and reset player position
        if (respawn_time <= 0)
        {
            ResetPlayer();
        }
        // Reset scene if player presses enter
        if (Input.GetKey(KeyCode.Return))
        {
            ResetPlayer();
        }
    }
    private void Respawn(Vector3 respawn_pos)
    {
        /// Sets player position to the vector 3 passed into the function
        player.transform.position = respawn_pos;
    }


    public void OnWhitePlatformHit(float white_plat_angle, float speed = 1f)
    {
        /// Bounce the ball off the platform at depending on the angle of "white_plat_angle" and bounce it off at the speed of the float argument "speed"
        if (white_plat_angle < -90)
        {
            white_plat_angle += 180;
        }
        else if (white_plat_angle > 90)
        {
            white_plat_angle -= 180;
        }
        float xforce;
        float yforce = 90 - Mathf.Sqrt(white_plat_angle * white_plat_angle);
        if (yforce < 0)
        {
            yforce *= -1;
            xforce = -(90 - yforce);
        }
        else
        {
            xforce = 90 - yforce;
        }
        rb.AddForce(-xforce * bounce_speed * speed, yforce * bounce_speed * speed, 0);
    }

    public void OnVioletPlatformHit(int moveTimes)
    {
        // Sets the amount of times the player can move to the integer passed into the function
        move_times = moveTimes;
    }

    public void OnRedPlatformHit()
    {
        // Takes away a player life
        lives -= 1;
    }

    public void FlipPlatform(float rotation)
    {

    }
    // Changes player spawnpoint to checkpoint location, adds hit checkpoint to a list of checkpoints hit. Is called when player triggers a checkpoint
    public void HitCheckpoint(GameObject checkpoint_name)
    {
        checkpoints.Add(checkpoint_name);
        Vector3 point_pos = checkpoints[checkpoints.Count - 1].transform.position;
        spawnpoint = point_pos;
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    private void ResetPlayer()
    {
        /// Resets player
        rb.linearVelocity = new Vector3(0, 0, 0);
        Respawn(spawnpoint);
        isPlayerDead = false;
        respawn_time = respawn_time_copy;
        lives = 1;
    }
}