using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerAnimation : MonoBehaviour
{
    PlatformerController mov;
    Animator anim;
    Animator transAnim;
    SpriteRenderer sr;

    public bool jump {private get; set;}
    bool backwards;

    void Start()
    {
        mov = GetComponent<PlatformerController>();
        anim = this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        transAnim = this.gameObject.transform.GetChild(0).GetComponent<Animator>();
        sr = anim.GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        CheckAnimState();
    }

    private void CheckAnimState()
    {
        anim.SetFloat("Vel X", mov.rb.velocity.magnitude);
        anim.SetFloat("Vel Y", mov.rb.velocity.y);

        if (!sr.flipX && mov.moveInput.x > 0)
        {
            sr.flipX = true;
            transAnim.SetTrigger("flip");
        }else if (sr.flipX && mov.moveInput.x < 0)
        {
            sr.flipX = false;
            transAnim.SetTrigger("flip");
        }
        if (!backwards && mov.moveInput.y > 0)
        {
            backwards = true;
            transAnim.SetTrigger("flip");
        } else if (backwards && mov.moveInput.y < 0)
        {
            backwards = false;
            transAnim.SetTrigger("flip");
        }
        anim.SetBool("backwards", backwards);

        if (jump)
        {
            anim.SetTrigger("jump");
            jump = false;
            return;
        }
    }
}
