using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterSquirrelStats : ShooterEnemyStats
{
    public float BulletSpeed; //velocidad de la bala
    public float BulletLifeTime; //tiempo que está la bala antes de destruirse sola
    public float ShootDistance; //distancia del jugador a partir de la que empieza a disparar
    public float StopDistance; //distancia del jugador a partir de la que deja de acercarse
    public float ShootCooldown; //segundos entre cada disparo
}
