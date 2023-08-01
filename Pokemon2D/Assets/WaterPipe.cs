using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPipe : MonoBehaviour
{
    public Animator anim;
    

    public bool isflowing, isfinished, isInPlace;

    public GameObject waterPipeToTrigger;

    [HideInInspector] public WaterPipe Instance;

    private void Awake()
    {
        Instance= this;
    }
    private void Update()
    {
        if(isflowing)
        {
            StartFlow();
        }
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void StartNextBlock () => waterPipeToTrigger.GetComponent<WaterPipe>().StartFlow();

    void StartFlow()
    {
        if (isInPlace)
        {
            anim.SetBool("isPlaying", true);
            isflowing = true;
        } 

        if(!isInPlace)
        {
           
            anim.SetBool("isPlaying", false);
            isflowing = false;
            return;
        }

        
    }

   
}
