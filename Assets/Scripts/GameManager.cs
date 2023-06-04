using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    
    public GameObject player;
    public float health;

    public bool enteredFirstTemple, enteredSecondTemple, enteredThirdTemple;
    public bool firstTempleComplete, secondTempleComplete, thirdTempleComplete;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
