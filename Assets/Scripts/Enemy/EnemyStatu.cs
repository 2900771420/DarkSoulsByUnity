using System.Collections;
using System.Collections.Generic;
using LostLight;
using UnityEngine;

public class EnemyStatu : MonoBehaviour
{
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;


    Animator animator;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;

    }
    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;

        animator.Play("damage");

        if (currentHealth <= 0)
        {
            animator.Play("death");
            currentHealth = 0;

        }
    }
}
