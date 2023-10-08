using UnityEngine;

//Interfaz implementada por todos los objetos que puedan ser atacados por un enemigo
public interface IEnemyTarget
{
    public void Hit(int damage, Vector2 enemyPosition);
}
