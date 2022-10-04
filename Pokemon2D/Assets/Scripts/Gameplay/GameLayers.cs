using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] LayerMask solidObjectsLayer;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask battleZoneLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask fovLayer;
    [SerializeField] LayerMask portalLayer;
    [SerializeField] LayerMask triggersLayer;
    [SerializeField] LayerMask ledgesLayer;
    [SerializeField] LayerMask waterLayer;



    public static GameLayers I { get;  set; }

    private void Awake()
    {
        I = this;
    }
    public LayerMask SolidLayer
    {
        get => solidObjectsLayer;
    }

    public LayerMask InteractableLayer
    {
        get => interactableLayer;
    }

    public LayerMask BattleZoneLayer
    {
        get => battleZoneLayer;
    }

    public LayerMask PlayerLayer
    {
        get => playerLayer;
    }

    public LayerMask FovLayer
    {
        get => fovLayer;
    }
    public LayerMask PortalLayer
    {
        get => portalLayer;
    }

    public LayerMask LedgesLayer => ledgesLayer;

    public LayerMask WaterLayer => waterLayer;


    public LayerMask TriggerableLayers
    {
        get => battleZoneLayer | fovLayer | portalLayer | triggersLayer | waterLayer; 
    }
}
