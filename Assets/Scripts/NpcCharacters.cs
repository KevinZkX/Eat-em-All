using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Random = UnityEngine.Random;
using Coroutine = System.Collections.IEnumerator;
using BTCoroutine = System.Collections.Generic.IEnumerator<BTNodeResult>;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(FieldOfView))]

public class NpcCharacters: Character {

    #region Public Variables

    public bool Target { get; set; }
    public bool Smaller { get; set; }
    public bool Foody { get; set; }
    public bool Hidden { get; set; }

    public Map map;

    public int skillID;

    public GameObject sesame;

    #endregion

    #region Private Variables

    [SerializeField] protected PathGenerator pathGenerator;

   
    protected FieldOfView fow;

    #endregion

    void Awake()
    {
        sesame = GameObject.Find("Sesame");
       
        rigidbody = GetComponent<Rigidbody>();
        fow = GetComponent<FieldOfView>();

        
    }

   

}
