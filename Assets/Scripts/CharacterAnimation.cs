using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterAnimation : MonoBehaviour
{

    public static Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public static void attackAnimation()
    {
        anim.SetBool("isAttacking", true);
    }

    public static void stopAttackAnimation()
    {
        anim.SetBool("isAttacking", false);
    }
}
