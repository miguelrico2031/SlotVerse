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
            // Parar animaci�n
            throw new System.NotImplementedException();
        }

        public override void FixedUpdate()
        {
            // Transformar posici�n hacia la derecha
            // Animaci�n
            // En momento aleatorio cambiar a modo de ataque
            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            // Comprobar colisi�n (?????
            throw new System.NotImplementedException();
        }
    }
}