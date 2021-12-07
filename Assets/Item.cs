using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Text Label = default;

    public string Rank { get; set; } = default;
    public string Name { get; set; } = default;

    private void Update()
    {
        this.Label.text = $"[{Rank}] {Name}";
    }
}
