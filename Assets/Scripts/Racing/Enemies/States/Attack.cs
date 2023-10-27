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
            // Get dirección en la que estaba caminando
            throw new System.NotImplementedException();
        }

        public override void Exit()
        {
            // Continuar movimiento en dirección guardada
            // Parar animación
            throw new System.NotImplementedException();
        }

        public override void FixedUpdate()
        {
            // Ataque
            // Animación
            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            // Comprobar colisión (?????
            throw new System.NotImplementedException();
        }
    }
}