using UnityEngine;

namespace Interfaces
{
    public interface IEnemy
    {
        public GameObject GetGameObject();
        public void SetState();
        public IState GetState();

        //Movement
        public float WalkSpeed();
        public float WalkDistance();
        public void FlipDirection();

        //Actions
        public void HitPlayer();
        public void Die();
    }
}