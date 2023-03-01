using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public string name;
    public string description;
    public string script;

    public User(string name, string description, string script)
    {
        this.name = name;
        this.description = description;
        this.script = script;
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
