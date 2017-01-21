using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
    private string id { get; set; }
    private string displayName { get; set; }
    private string color { get; set; }
    private bool isDead { get; set; }
    private List<Jump> listOfJumps { get; set; }

    public Player()
    {
        this.isDead = false;
        this.listOfJumps = new List<Jump>();
    }
}
