using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlay : MonoBehaviour
{
    // Start is called before the first frame update

    public Animation anima;

    void PlayAnimation()
    {
        anima.Play("TransitionAnimation");
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
