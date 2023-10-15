using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//clase abstracta para que herede una clase por cada tipo de enemigo
//que solo tiene una instancia, en la que se guardan los stats comunes y espec�ficos de 
//cada enemigo
public abstract class ShooterEnemyStats : ScriptableObject
{
    public int Health; //salud
    public int Damage; //da�o que hacen sus ataques
    public float MoveSpeed; //vel de movimiento
    public float NextWaypointDistance; //distancia a partir de la cual avanza de nodo en el pathfinding
    public float KnockbackForce; //fuerza del knockback al jugador
    public float KnockbackDuration; //duraci�n del knockback al jugador
    
}
