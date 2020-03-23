using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureHunterInventory : MonoBehaviour
{
    public List<Collectible> inv;
     // Start is called before the first frame update
    void Start()
    {
        inv = new List<Collectible>();
    }
}
