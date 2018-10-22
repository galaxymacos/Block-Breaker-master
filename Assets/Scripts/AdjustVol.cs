using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AdjustVol : MonoBehaviour
{
    [SerializeField]private AudioMixer audioM;

    [SerializeField] private string audioControlTarget;
	// Use this for initialization
    private Slider volController;
	void Start ()
	{
	    volController = GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    audioM.SetFloat(audioControlTarget, volController.value);
	}

    
}
