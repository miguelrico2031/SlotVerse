using UnityEngine;

namespace Interfaces
{
    public abstract class AEnemyState : IState
    {
        protected IEnemy _enemy;

        public int Health; //salud
        public int Damage; //daño que hacen sus ataques
        public float MoveSpeed; //vel de movimiento
        public float DeadTime; //tiempo de estar muerto antes de desaparecer
        public Color DamageColor = Color.red; //color del que se pintan los enemigos al ser dañad

        public AEnemyState(IEnemy enemy)
        {
            this._enemy = enemy;
        }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update();
        public abstract void FixedUpdate();
    }
}
