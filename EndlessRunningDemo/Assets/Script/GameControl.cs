﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    public bool isOver = false;
    public float Volume;
    public GameObject gameOverText;
    public static GameControl instance;
    public Text scoreText;
    public float scrollSpeed = -1.5f;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Rigidbody2D rgbd;

    private int score;
    private string deviceName;
    private AudioClip micRecord;

	// Use this for initialization
	void Awake () {
        if(instance == null)
        {
            instance = this;
        }else if(instance != this)
        {
            Destroy(this);
        }
	}

	void Start()
	{
        deviceName = Microphone.devices[0];
        micRecord = Microphone.Start(deviceName, true, 999, 44100);
	}

	// Update is called once per frame
	void Update () {
        if(isOver == true && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }else if(isOver == false)
        {
            Volume = MaxVolume();
        }
	}

    public void BirdScore()
    {
        if(isOver == true)
        {
            return;
        }
        score++;
        scoreText.text = "Score:" + score.ToString();
    }

    public void PlayerDie()
    {
        gameOverText.SetActive(true);
        isOver = true;
    }

    public float MaxVolume()
    {
        float maxVolume = 0;
        float[] volumeData = new float[128];
        int offset = Microphone.GetPosition(deviceName) - 128 + 1;
        if(offset < 0)
        {
            return 0;
        }

        micRecord.GetData(volumeData, offset);

        for (int i = 0; i < volumeData.Length; i++)
        {
            float tempVolume = volumeData[i] * 1.5f;

            if(maxVolume < tempVolume)
            {
                maxVolume = tempVolume;
            }
        }
        return maxVolume;
    }
}