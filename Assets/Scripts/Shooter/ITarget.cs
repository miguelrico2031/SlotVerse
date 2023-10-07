using UnityEngine.Events;
//interfaz para todos los objetos que vayan a poder ser golpeados por una bala (Bullet)
public interface ITarget
{
    //IMPORTANTE: al final de la implementacion de este metodo, hay que llamar al metodo DestroyBullet()
    //de la bala pasada como argumento
    public void Hit(Bullet bullet);
}
