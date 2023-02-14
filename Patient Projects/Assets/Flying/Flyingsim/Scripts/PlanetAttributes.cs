using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetAttributes : MonoBehaviour
{
    [Header("General planet att. info")]
    [SerializeField] public string planetName;

    [Header("Landscape info")]
    [SerializeField] public bool mountain;
    [SerializeField] public bool oceans;
    [SerializeField] public bool forest;
    [SerializeField] public bool deserts;
    [SerializeField] public bool anomlyZones;

    [Header("Resource info")]
    [SerializeField] public bool stone;
    [SerializeField] public bool wood;
    [SerializeField] public bool iron;
    [SerializeField] public bool anomlyMaterial;

}
