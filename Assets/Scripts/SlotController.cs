using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotController : MonoBehaviour
{
    [SerializeField] private GameInfo _gameInfo;

    [SerializeField] private Rigidbody _gameModeRail;
    [SerializeField] private Rigidbody _npcRail;
    [SerializeField] private Rigidbody _settingRail;

    [SerializeField] private StopButton _stopButton;

    [SerializeField] private float _angularSpeed; //vel angular en grados / segundo


    private Rail[] _rails;

    private int _activeRailIndex;

    private void Awake()
    {
        _stopButton.Pressed.AddListener(StopRail);

        _rails = new Rail[]
        {
            new Rail(_gameModeRail),
            new Rail(_npcRail),
            new Rail(_settingRail)
        };
    }

    public void StartSlot()
    {
        foreach (var rail in _rails)
        {
            rail.Stopped = false;
            rail.Snapped = false;
        }

        _stopButton.EnableButton(true);
    }

    public void StopRail()
    {
        _rails[_activeRailIndex].Stopped = true;

        Vector3 eulers = _rails[_activeRailIndex].Rb.rotation.eulerAngles;

        if (eulers.z < 120) _rails[_activeRailIndex].TargetAngle = 60f;
        else if (eulers.z < 240) _rails[_activeRailIndex].TargetAngle = 180f;
        else _rails[_activeRailIndex].TargetAngle = 300f;
    }


    private void FixedUpdate()
    {
        Quaternion increment = Quaternion.identity;

        if (!_rails[0].Stopped || !_rails[1].Stopped || !_rails[2].Stopped)
            increment = Quaternion.AngleAxis(_angularSpeed * Time.fixedDeltaTime, Vector3.right);
       

        foreach(var rail in _rails) if (!rail.Stopped) rail.Rb.MoveRotation(increment * rail.Rb.rotation);

        //Debug.Log(_npcRail.rotation.eulerAngles.z);

        foreach (var rail in _rails)
        {
            if (rail.Stopped && !rail.Snapped)
            {
                float deltaAngle = Mathf.DeltaAngle(rail.Rb.rotation.eulerAngles.z, rail.TargetAngle);
                Vector3 newEulers = rail.Rb.rotation.eulerAngles;
                newEulers.z = Mathf.LerpAngle(rail.Rb.rotation.eulerAngles.z, rail.TargetAngle, 20* Time.fixedDeltaTime / deltaAngle);

                rail.Rb.rotation = Quaternion.Euler(newEulers);

                if(Mathf.DeltaAngle(rail.Rb.rotation.eulerAngles.z, rail.TargetAngle) < 0.1f)
                {
                    rail.Rb.rotation = 
                        Quaternion.Euler(rail.Rb.rotation.eulerAngles.x, rail.Rb.rotation.eulerAngles.y, rail.TargetAngle);

                    rail.Snapped = true;

                    _activeRailIndex++;
                    if (_activeRailIndex < _rails.Length) _stopButton.EnableButton(true);

                    else SetGame();
                }
            }

        }
    }

    private void SetGame()
    {
        switch(_rails[0].TargetAngle)
        {
            case 60f:
                _gameInfo.GameMode = GameMode.Shooter;
                break;

            case 180f:
                _gameInfo.GameMode = GameMode.Platformer;
                break;

            case 300:
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
    }
}


class Rail
{
    public Rigidbody Rb;
    public bool Stopped;
    public bool Snapped;
    public float TargetAngle;

    public Rail(Rigidbody rb)
    {
        Rb = rb;
        Stopped = true;
        Snapped = true;
    }
}

