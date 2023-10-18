using UnityEngine;

//Interfaz implementada por todos los objetos que puedan ser atacados por un enemigo
public interface IEnemyTarget
{
    public void Hit(EnemyAttackInfo attackInfo);
}
