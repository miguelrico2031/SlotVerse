using UnityEngine;

namespace Interfaces
{
    public abstract class AEnemyState
    {
        protected IEnemy _enemy;

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