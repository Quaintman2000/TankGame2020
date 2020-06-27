using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankData : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float health;
    public float moveSpeed = 5.0f;
    public float reverseSpeed = -5.0f;
    public float rotateSpeed = 30.0f;
    public float score = 0;
    public float pointsForKill = 10;
    public float fireRate;
}
