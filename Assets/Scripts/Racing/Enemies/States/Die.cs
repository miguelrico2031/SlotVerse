using Interfaces;
using Unity;

namespace States
{
    public class Die : AEnemyState
    {

        public Die(IEnemy enemy) : base(enemy)
        {
        }

        public override void Enter()
        {
            throw new System.NotImplementedException();
        }

        public override void Exit()
        {
            // Destruir gameobject enemigo
            // Parar animaci�n
            throw new System.NotImplementedException();
        }

        public override void FixedUpdate()
        {
            // Animaci�n
            // Hacer da�o y efectos al enemigo
            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}