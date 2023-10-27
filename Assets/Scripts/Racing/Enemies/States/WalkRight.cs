using Interfaces;
using Unity;

namespace States
{
    public class WalkRight : AEnemyState
    {

        public WalkRight(IEnemy enemy) : base(enemy)
        {
        }

        public override void Enter()
        {
            // Get velocidad del enemigo
            throw new System.NotImplementedException();
        }

        public override void Exit()
        {
            // Parar movimiento
            // Parar animación
            throw new System.NotImplementedException();
        }

        public override void FixedUpdate()
        {
            // Transformar posición hacia la derecha
            // Animación
            // En momento aleatorio cambiar a modo de ataque
            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            // Comprobar colisión (?????
            throw new System.NotImplementedException();
        }
    }
}