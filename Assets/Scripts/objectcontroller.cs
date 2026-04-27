using UnityEngine;

public class ObjectMotionController : MonoBehaviour
{
    public enum MotionMode
    {
        Move,

        Rotate,

        MoveAndRotate
    }

    public enum LoopMode
    {
        Once,
        Loop,
        PingPong
    }

    public enum TickMode
    {
        Update,
        FixedUpdate,

        LateUpdate
    }

    [Header("General")]

    [SerializeField] private MotionMode motionMode = MotionMode.MoveAndRotate;

    [SerializeField] private LoopMode loopmode = LoopMode.Loop;

    [SerializeField] private TickMode tickMode = TickMode.Update;

    [SerializeField] private bool playOnStart = true;

    [SerializeField] private bool useLocalSpace = true;

    [SerializeField] private bool useUnscaledTime = false;

    [SerializeField] private AnimationCurve interpolationCurve = AnimationCurve.Linear(0f,0f,1f,1f);

    [Header("Move")]

    [SerializeField] private Vector3 moveOffset = new Vector3(2f, 0f, 0f);
    [SerializeField] private float moveDuration = 2f;

    [SerializeField] private bool startModeFromPositiveOffset = true;

    [Header("Rotate")]

    [SerializeField] private Vector3 rotationOffsetEuler = new Vector3(0f,0f,90f);

    [SerializeField] private float rotateDuration = 2f;

    private Vector3 initialPosition;

    private Quaternion initialRotation;

    private float moveElapsed;

    private float rotateElapsed;

    private bool moveForward = true;

    private bool rotateForward = true;

    private bool moveCompleted;

    private bool rotateCompleted;

    private bool isPlaying;

    private void OnEnable()
    {
        if (playOnStart)
        {
            Play();
        }
    }

    private void Update()
    {
        if(tickMode == TickMode.Update)
        {
            Tick(GetDeltaTime(false));
        }
    }

    private void FixedUpdate()
    {
        if(tickMode == TickMode.FixedUpdate)
        {
            Tick(GetDeltaTime(true));
        }
    }

    private void LateUpdate()
    {
        if(tickMode == TickMode.LateUpdate)
        {
            if(tickMode == TickMode.LateUpdate)
            {
                Tick(GetDeltaTime(false));
            }
        }
    }

    public void Play()
    {
        isPlaying= true;
    }

    public void Pause()
    {
        isPlaying = false;
    }

    public void StopAndReset()
    {
        isPlaying = false;

        moveElapsed = 0f;

        rotateElapsed= 0f;

        rotateForward = true;

        moveCompleted = false;

        rotateCompleted = false;

        //mehhhhh
    }

    private float GetDeltaTime(bool isFixedStep)
    {
        if (!useUnscaledTime)
        {
            return isFixedStep ? Time.fixedDeltaTime : Time.deltaTime;
        }

        return isFixedStep ? Time.fixedUnscaledDeltaTime : Time.unscaledDeltaTime;
    }

    private void Getthecurrentposition()
    {
        initialPosition = useLocalSpace ? transform.localPosition : transform.position;
        initialRotation = useLocalSpace ? transform.localRotation : transform.rotation;
    }

    public void Gettheposition()
    {
        Getthecurrentposition();
    }

    private bool UsesMove()
    {
        return motionMode == MotionMode.Move|| motionMode == MotionMode.MoveAndRotate;
    }

    private void Tick(float deltaTime)
    {
        
    }
} 