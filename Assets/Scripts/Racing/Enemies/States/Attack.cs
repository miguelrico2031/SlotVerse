using Interfaces;
using Unity;

namespace States
{
    public class Attack : AEnemyState
    {

        public Attack(IEnemy enemy) : base(enemy)
        {
        }

        public override void Enter()
        {
            // Get direcci�n en la que estaba caminando
            throw new System.NotImplementedException();
        }

        public override void Exit()
        {
            // Continuar movimiento en direcci�n guardada
            // Parar animaci�n
            throw new System.NotImplementedException();
        }

        public override void FixedUpdate()
        {
            // Ataque
            // Animaci�n
            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            // Comprobar colisi�n (?????
            throw new System.NotImplementedException();
        }
    }
}