using UnityEngine;
using Interfaces;
using States;
using IState = Interfaces.IState;

namespace Enemies
{
    public class Squirrel : MonoBehaviour, IEnemy
    {

        public int WalkSpeed;

        private IState currentState;

        private Animator animator;

        // Start is called before the first frame update
        void Awake()
        {
            animator = gameObject.GetComponent<Animator>();
            SetState(new WalkLeft(this));
        }

        // Update is called once per frame
        void Update()
        {

        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public IState GetState()
        {
            return currentState;
        }

        public float GetWalkSpeed()
        {
            return WalkSpeed;
        }

        public void SetState(IState state)
        {
            if (currentState != null)
            {
                currentState.Exit();
            }
            
            currentState = state;
            currentState.Enter();
        }
    }
}
