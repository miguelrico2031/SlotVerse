//Nombrado interfaz: I + S (shooter) + nombre interfaz
//Interfaz que implementan todos los scripts del enemigo cuyo estado se necesite resetear
//al hacer object pooling con ellos
public interface ISSpawnableEnemy
{
    public void Reset();
}
