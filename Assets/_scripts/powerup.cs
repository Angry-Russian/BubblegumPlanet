using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class powerup : MonoBehaviour {

    enum types
    {
        a = 0,
        b = 1,
        c = 2,
        d = 3,
        e = 4
    }
    int m_type = 0;
	// Use this for initialization
	void Start () {
		
	}

    void Awake()
    {
        this.m_type = Random.Range(0, 5);
        powerupManager pwManager = GameObject.FindWithTag("powerupManager").GetComponent<powerupManager>();
        this.GetComponent<Renderer>().material = pwManager.getListMaterial()[m_type];
    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            effectPowerUp(other);
            other.GetComponent<ThirdPersonCharacter>().m_JumpPower = 25.0f;
            Destroy(this.gameObject);
        }
    }

    void effectPowerUp(Collider currentPlayer)
    {
        switch (this.m_type)
        {
            case 0:
            {
                break;
            }
            case 1:
            {
                break;
            }
            case 2:
            {
                break;
            }
            case 3:
            {
                break;
            }
            case 4:
            {
                break;
            }
        }
    }
}
