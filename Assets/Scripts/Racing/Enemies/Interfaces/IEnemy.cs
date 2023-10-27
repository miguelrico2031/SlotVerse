using UnityEngine;

namespace Interfaces
{
    public interface IEnemy
    {
        public GameObject GetGameObject();
        public void SetState(IState state);
        public IState GetState();

        public float GetWalkSpeed();

    }
}
