using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skills : MonoBehaviour {
    [Tooltip("The unique id of the skill")]
    public int SkillId;
    [Tooltip("The unique name of the skill")]
    public string SkillName;
    protected GameObject attatched_gameObject;
    protected Transform attatched_transform;
    protected Rigidbody attatched_rigidbody;
    public PlayerController attatched_player;
    public Image skillImage;
    public Text skillName;

    private void Start()
    {
        attatched_gameObject = gameObject;
        attatched_transform = transform;
        attatched_rigidbody = GetComponent<Rigidbody>();
        attatched_player = GetComponent<PlayerController>();
    }

    public abstract void Skill();
}
