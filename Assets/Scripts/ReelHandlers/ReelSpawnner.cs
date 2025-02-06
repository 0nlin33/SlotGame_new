using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelSpawnner : MonoBehaviour
{
    public List<Symbols> reelSymbols;

    public int radius = 5;
    
    public bool spawnReel = true;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (spawnReel)
        {
            SpawnReelSymbols();
        }
    }

    void SpawnReelSymbols()
    {
        if (reelSymbols == null || reelSymbols.Count == 0)
        {
            Debug.LogError("No Reel Symbols Found");
        }
        
        int numberOfSymbols = reelSymbols.Count;
        float angleIncrease = 360 / numberOfSymbols;

        for (int i = 0; i < numberOfSymbols; i++)
        {
            float angle = angleIncrease * i;
            
            float radians = angle * Mathf.Deg2Rad;

            Vector3 spawnPosition =
                transform.position + new Vector3(0, Mathf.Cos(radians), Mathf.Sin(radians) ) * radius;
            
            Symbols spawnedSymbol = Instantiate(reelSymbols[i], spawnPosition, Quaternion.identity);
            
            spawnedSymbol.transform.parent = transform;
            
            spawnedSymbol.transform.LookAt(transform.position);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
