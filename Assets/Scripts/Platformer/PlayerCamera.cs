using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform cam;
    Animator anim;

    public bool to2d;
    public bool to3d;
    void Start() {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        cam = GetComponent<Transform>();
        anim = cam.GetChild(0).GetComponent<Animator>();
    }

    void Update() {
        cam.position = new Vector3(player.position.x, player.position.y + 4, player.position.z - 10);
    }

    void LateUpdate() {
       AnimCheck();
    }

    void AnimCheck() {
        if (to2d) {
            anim.SetTrigger("to2d");
            to2d = false;
            return;
        } else if (to3d) {
            anim.SetTrigger("to3d");
            to3d = false;
            
        }
    }
}
