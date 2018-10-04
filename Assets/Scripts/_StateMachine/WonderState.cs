using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

public class WonderState : NpcState<Monsters>
{
    private static WonderState _instance;
    //just one
    private WonderState()
    {
        if(_instance !=null)
        {
            return;
        }
        _instance = this;
    }
    public static WonderState Instance
    {
        get
        {
            if (_instance == null)
            {
                new WonderState();
            }
            return _instance;
        }
    }
    public override void EnterState(Monsters _owner)
    {
        Debug.Log(_owner.ToString() + "Enter Wonder state");
    }

    public override void ExitState(Monsters _owner)
    {
        Debug.Log(_owner.ToString() + "Exit Wonder state");
    }

    public override void UpdateState(Monsters _owner)
    {
        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
