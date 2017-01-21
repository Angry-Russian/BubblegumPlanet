using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerupManager : MonoBehaviour {

    [SerializeField] Material[] listMaterial = new Material[5];
    [SerializeField] GameObject powerUpPrefab;
    float x;
    float y;
    float z;
    Vector3 pos;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Random.Range(0, 5000) == 100)
        {
            x = Random.Range(-25, 26);
            y = 1;
            z = Random.Range(-25, 26);
            pos = new Vector3(x, y, z);
            Instantiate(powerUpPrefab, pos, Quaternion.identity);
        }
            
	}

    public Material[] getListMaterial()
    {
        return listMaterial;
    }
}
