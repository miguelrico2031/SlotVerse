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
            // Parar animación
            throw new System.NotImplementedException();
        }

        public override void FixedUpdate()
        {
            // Animación
            // Hacer daño y efectos al enemigo
            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}