using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//clase que controla la logica de la palanca, el boton de parar, y los carriles de la tragaperras
public class SlotController : MonoBehaviour
{
    [SerializeField] private GameInfo _gameInfo; //info del juego para escribirla con los resultados

    //rigidbodies de los 3 railes de la tragaperras
    [SerializeField] private Rigidbody _gameModeRail;
    [SerializeField] private Rigidbody _npcRail;
    [SerializeField] private Rigidbody _settingRail;

    //3 botones frontales de la tragaperras
    [SerializeField] private MenuButton _stopButton, _quitButton, _optionsButton;

    [SerializeField] private float _angularSpeed; //velocidad angular en grados / segundo de los carriles


    private Rail[] _rails; //array de los 3 carriles

    private int _activeRailIndex; //indice del rail activo, es decir, el que sera detenido con el boton parar

    //clase rail que guarda el estado e informacion relevante de cada rail
    class Rail
    {
        public Rigidbody Rb; //referencia al rigidbody (para rotarlo y pararlo)
        public bool Stopped; //true si se esta deteniendo o esta detenido
        public bool Snapped; //true si esta detenido y ajustado al marco
        public float TargetAngle; //angulo que tiene que tener para estar Snapped una vez que esta Stopped

        public Rail(Rigidbody rb)
        {
            Rb = rb;
            Stopped = true;
            Snapped = true;
        }
    }

    private void Awake()
    {
        //inicalizar los 3 objetos railes en el array a partir de los rigidbodies
        _rails = new Rail[]
        {
            new Rail(_gameModeRail),
            new Rail(_npcRail),
            new Rail(_settingRail)
        };
    }

    private void Start()
    {
        //suscribirse al evento del boton de parar, para parar el rail activo
        _stopButton.Pressed.AddListener(StopRail);
        _stopButton.DisableButton(); //inicialmente desactivamos el boton de parar hasta que se pulse la palanca
    }

    //funcion llamada por el evento de pulsar la palanca, que hace iniciar a girar a los 3 carriles
    public void StartSlot()
    {
        //cada carril deja de estar stopped y snapped, lo que hace que en el FixedUpdate sean rotados
        foreach (var rail in _rails)
        {
            rail.Stopped = false;
            rail.Snapped = false;
        }

        _stopButton.EnableButton(); //activamos el boton de parar el rail

        _optionsButton.DisableButton(); //desactivamos el boton de opciones
    }

    //funcion llamada por el evento de pulsar el boton de parar, que hace detenerse al rail activo
    public void StopRail(MenuButton button)
    {
        //detengo el movimiento del rail activo cambiando su estado
        _rails[_activeRailIndex].Stopped = true;

        //obtencion de la rotacion del rail activo en eulers
        Vector3 eulers = _rails[_activeRailIndex].Rb.rotation.eulerAngles;

        //calculo del angulo target del primer carril
        if(_activeRailIndex == 0)
        {
            if (eulers.z < 135 || eulers.z > 315) _rails[0].TargetAngle = 45f;
            else _rails[0].TargetAngle = 225f;

            return;
        }

        //calculo del angulo target del segundo o tercer rail
        if (eulers.z < 120) _rails[_activeRailIndex].TargetAngle = 60f;
        else if (eulers.z < 240) _rails[_activeRailIndex].TargetAngle = 180f;
        else _rails[_activeRailIndex].TargetAngle = 300f;
    }

    //aqui se rotan los railes dependiendo de su estado, usando sus rigidbodies
    private void FixedUpdate()
    {
        //variable para calcular el incremento de la rotacion
        Quaternion increment = Quaternion.identity;

        //si al menos 1 de los 3 railes esta girando y no Stopped, se calcula el incremento
        //con la velocidad angular, y el eje de rotacion de los 3 carriles, que es el (1, 0, 0)
        if (!_rails[0].Stopped || !_rails[1].Stopped || !_rails[2].Stopped)
            increment = Quaternion.AngleAxis(_angularSpeed * Time.fixedDeltaTime, Vector3.right);
       
       

           
        foreach (var rail in _rails)
        {
            //despues de calcularse el incremento, se le aplica al rigidbody de cada carril que no este Stopped
            if (!rail.Stopped) rail.Rb.MoveRotation(increment * rail.Rb.rotation);

            //si el rail esta Stopped pero no Snapped, hay que rotarlo hasta alcanzar el angulo target
            else if (!rail.Snapped)
            {
                //distancia angular entre el angulo actual y el angulo target o destino
                float deltaAngle = Mathf.DeltaAngle(rail.Rb.rotation.eulerAngles.z, rail.TargetAngle);
                
                //Vector de la nueva rotacion del rail, que es la actual salvo por la z
                //Que es el eje en el que hay que rotarlo hasta que alcance el angulo destino
                //La rotacion se hace interpolando linealmente el angulo actual con el angulo objetivo,
                //usando una velocidad hardcodeada 40 (perdon xdd) dividiendola entre la distancia angular 
                Vector3 newEulers = rail.Rb.rotation.eulerAngles;
                newEulers.z = Mathf.LerpAngle(rail.Rb.rotation.eulerAngles.z, rail.TargetAngle, 40* Time.fixedDeltaTime / deltaAngle);

                //asignar la nueva rotacion interpolada al rigidbody
                rail.Rb.rotation = Quaternion.Euler(newEulers);

                //comprobar si la distancia angular actual es virtualmente 0 (el angulo target ha sido alcanzado)
                if(Mathf.DeltaAngle(rail.Rb.rotation.eulerAngles.z, rail.TargetAngle) < 0.1f)
                {
                    //si se alcanzo el angulo target lo suficiente (0.1 de error), se asigna la rotacion al
                    //angulo target exactamente
                    rail.Rb.rotation = 
                        Quaternion.Euler(rail.Rb.rotation.eulerAngles.x, rail.Rb.rotation.eulerAngles.y, rail.TargetAngle);

                    //se cambia el estado a Snapped para que no siga siendo rotado
                    rail.Snapped = true;

                    _activeRailIndex++; //se avanza de rail activo

                    //si no estamos en el ultimo rail, se vuelve a activar el boton de parar
                    if (_activeRailIndex < _rails.Length) _stopButton.EnableButton();
                    //sino, empezamos la partida que corresponda
                    else SetGame();
                }
            }

        }
    }

    //funcion que escribe en el GameInfo los datos obtenidos en la tragaperras, y carga la escena del juego obtenido
    private void SetGame()
    {
        switch(_rails[0].TargetAngle)
        {
            case 45:
                _gameInfo.GameMode = GameMode.Shooter;
                break;

            case 225:
                _gameInfo.GameMode = GameMode.Racing;
                break;
        }

        switch (_rails[1].TargetAngle)
        {
            case 60f:
                _gameInfo.NPC = NPC.Squirrel;
                break;

            case 180f:
                _gameInfo.NPC = NPC.Hedgehog;
                break;

            case 300:
                _gameInfo.NPC = NPC.Monkey;
                break;
        }

        switch (_rails[2].TargetAngle)
        {
            case 60f:
                _gameInfo.Setting = Setting.Halloween;
                break;

            case 180f:
                _gameInfo.Setting = Setting.Futuristic;
                break;

            case 300:
                _gameInfo.Setting = Setting.Beach;
                break;
        }

        Debug.Log(_gameInfo.GameMode.ToString());
        Debug.Log(_gameInfo.NPC.ToString());
        Debug.Log(_gameInfo.Setting.ToString());

        if(_gameInfo.GameMode == GameMode.Shooter) SceneManager.LoadScene("Shooter");
        if (_gameInfo.GameMode == GameMode.Racing) SceneManager.LoadScene("Racing");

    }

}




