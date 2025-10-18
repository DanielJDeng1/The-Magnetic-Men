using System;
using System.Collections.Generic;
using UnityEngine;

namespace TarodevController
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField] private ScriptableStats _stats;
        private Rigidbody2D _rb;
        private CapsuleCollider2D _col;
        private FrameInput _frameInput;
        private Vector2 _frameVelocity;
        private bool _cachedQueryStartInColliders;

        #region Interface

        public Vector2 FrameInput => _frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        #endregion

        private float _time;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<CapsuleCollider2D>();

            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;

            isActive = false;

            magnetismRadius.gameObject.SetActive(false);

            StartMagnetism();
        }

        private void Update()
        {
            _time += Time.deltaTime;
            GatherInput();
        }

        private void GatherInput()
        {
            _frameInput = new FrameInput
            {
                JumpDown = /*Input.GetButtonDown("Jump") ||*/ Input.GetKeyDown(_stats.JumpKey),
                JumpHeld = /*Input.GetButton("Jump") ||*/ Input.GetKey(_stats.JumpKey),

                Move = new Vector2(Convert.ToInt32(Input.GetKey(_stats.RightKey)) - Convert.ToInt32(Input.GetKey(_stats.LeftKey)), Input.GetAxisRaw("Vertical")),

                ToggleMagnetism = Input.GetKeyDown(_stats.ToggleMagnetism)
            };

            if (_stats.SnapInput)
            {
                _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
                _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
            }

            if (_frameInput.JumpDown)
            {
                _jumpToConsume = true;
                _timeJumpWasPressed = _time;
            }

            if (_frameInput.ToggleMagnetism){
                isActive = !isActive;
                magnetismRadius.gameObject.SetActive(isActive);
            }
        }

        private void FixedUpdate()
        {
            

            CheckCollisions();

            HandleJump();
            HandleDirection();
            HandleGravity();

            HandleMagnetism();
            
            ApplyMovement();

        }

        #region Collisions
        
        private float _frameLeftGrounded = float.MinValue;
        private bool _grounded;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            // Ground and Ceiling
            bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);
            bool ceilingHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.up, _stats.GrounderDistance, ~_stats.PlayerLayer);

            // Hit a Ceiling
            if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

            // Landed on the Ground
            if (!_grounded && groundHit)
            {
                _grounded = true;
                _coyoteUsable = true;
                _bufferedJumpUsable = true;
                _endedJumpEarly = false;
                GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
            }
            // Left the Ground
            else if (_grounded && !groundHit)
            {
                _grounded = false;
                _frameLeftGrounded = _time;
                GroundedChanged?.Invoke(false, 0);
            }

            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        #endregion


        #region Jumping

        private bool _jumpToConsume;
        private bool _bufferedJumpUsable;
        private bool _endedJumpEarly;
        private bool _coyoteUsable;
        private float _timeJumpWasPressed;

        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;
        private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;

        private void HandleJump()
        {
            if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.velocity.y > 0 && !isInBounce) _endedJumpEarly = true;

            if (!_jumpToConsume && !HasBufferedJump) return;

            if (_grounded || CanUseCoyote) ExecuteJump();

            _jumpToConsume = false;
        }

        private void ExecuteJump()
        {
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            _frameVelocity.y = _stats.JumpPower;
            Jumped?.Invoke();
        }

        #endregion

        #region Horizontal

        Vector2 velocityIncrease = Vector2.zero;

        bool iceEffect = false;

        public void IceEnable(bool b){
            iceEffect = b;
        }

        private void HandleDirection()
        {

            if (_frameInput.Move.x == 0)
            {
                var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;

                if (iceEffect){
                    deceleration *= 0.5f;
                }
                
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                //var maxSpeed = _grounded? _stats.MaxSpeed: 30f;
                float accelMultiplier = 1f;
                float maxSpeedMultiplier = 1f;
                if (iceEffect){
                    accelMultiplier = 1.5f;
                    maxSpeedMultiplier = 2f;
                }

                
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed * maxSpeedMultiplier, 
                    _stats.Acceleration * Time.fixedDeltaTime * accelMultiplier);
            }


        }

        #endregion

        #region Gravity

        private void HandleGravity()
        {

            if (isPulled)
                return;

            if (_frameVelocity.y <= 0f)
                isInBounce = false;

            if (_grounded && _frameVelocity.y <= 0f)
            {
                _frameVelocity.y = _stats.GroundingForce;
            }
            else
            {
                var inAirGravity = _stats.FallAcceleration;
                if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }

        #endregion

        #region Magnetism

        public bool isPositive = false;

        public bool isActive = true;

        bool isPulled = false;

        void HandleMagnetism(){
            isPulled = false;
            if (!isActive){
                for (int i = lines.Count - 1; i >= 0; i--){
                    GameObject temp = lines[i];
                    lines.Remove(lines[i]);
                    Destroy(temp);
                }
                return;
            }

            List<GameObject> list = magnetismRadius.inRadius;

            for (int i = 0; i < list.Count; i++){
                if (i + 1> lines.Count){
                    lines.Add(InstantiateLine());
                }

                GameObject obj = list[i];
                MagneticObject magnet = obj.GetComponent<MagneticObject>();
                
                if (magnet.isStaticObject){
                    if (magnet.isPositive != isPositive){
                        MagnetVelocity(obj.GetComponent<Collider2D>().ClosestPoint(transform.position));
                        DrawLine(transform.position, obj.GetComponent<Collider2D>().ClosestPoint(transform.position), i);
                        isPulled = true;
                    }
                    else{
                        DrawLine(obj.GetComponent<Collider2D>().ClosestPoint(transform.position), transform.position, i);
                        _frameVelocity = ((Vector2)transform.position - obj.GetComponent<Collider2D>().ClosestPoint(transform.position)).normalized * 20f;
                        
                    }
                }
                else{
                    if (magnet.isPositive != isPositive){
                        obj.GetComponent<Rigidbody2D>().velocity += ((Vector2) transform.position - obj.GetComponent<Collider2D>().ClosestPoint(transform.position)).normalized/obj.GetComponent<Rigidbody2D>().mass * 5f;
                        DrawLine(obj.GetComponent<Collider2D>().ClosestPoint(transform.position), transform.position, i);
                    }
                    else{
                        obj.GetComponent<Rigidbody2D>().velocity += (obj.GetComponent<Collider2D>().ClosestPoint(transform.position) - (Vector2) transform.position).normalized/obj.GetComponent<Rigidbody2D>().mass * 5f;
                        DrawLine(transform.position, obj.GetComponent<Collider2D>().ClosestPoint(transform.position), i);
                    }
                }
                if (magnet.isPositive != isPositive){
                    if (isPositive){
                        lines[i].GetComponent<LineRenderer>().startColor = Color.red;
                        lines[i].GetComponent<LineRenderer>().endColor = Color.blue;
                    }
                    else{
                        lines[i].GetComponent<LineRenderer>().startColor = Color.blue;
                        lines[i].GetComponent<LineRenderer>().endColor = Color.red;
                    }
                }
                else{
                    if (isPositive){
                        lines[i].GetComponent<LineRenderer>().startColor = Color.red;
                        lines[i].GetComponent<LineRenderer>().endColor = Color.red;
                    }
                    else{
                        lines[i].GetComponent<LineRenderer>().startColor = Color.blue;
                        lines[i].GetComponent<LineRenderer>().endColor = Color.blue;
                    }
                }
            }

            for (int i = lines.Count - 1; i >= list.Count; i--){
                GameObject temp = lines[i];
                lines.Remove(lines[i]);
                Destroy(temp);

            }
            
        }

        [SerializeField] MagnetismRadius magnetismRadius;

        [SerializeField] GameObject drawingPrefab;

        GameObject drawing;

        private LineRenderer lineRenderer;
        public Material material;
        private Color randomColor;

        List<GameObject> lines;

        GameObject InstantiateLine(){
            drawing = Instantiate(drawingPrefab);
            lineRenderer = drawing.GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = 0.15f;
            lineRenderer.endWidth = 0.15f;
            Randomize();
            lineRenderer.startColor = randomColor;
            lineRenderer.endColor = randomColor;
            return drawing;
        }

        void StartMagnetism(){
            lines = new List<GameObject>();
            /*drawing = Instantiate(drawingPrefab);
            lineRenderer = drawing.GetComponent<LineRenderer>();
            lineRenderer.positionCount = 0;
            lineRenderer.startWidth = 0.15f;
            lineRenderer.endWidth = 0.15f;
            Randomize();
            lineRenderer.startColor = randomColor;
            lineRenderer.endColor = randomColor;*/
        }

        /*void HandleMagnetism(){
            if (magnetismRadius.other != null){
                PlayerController other = magnetismRadius.other.GetComponent<PlayerController>();

                if (!(isPositive && other.isPositive) && !(!isPositive && !other.isPositive)){
                    drawing.SetActive(true);
                    Vector2 midPoint = FindMidpoint(gameObject.transform.position, other.gameObject.transform.position);
                    DrawLine((Vector2)transform.position + new Vector2(0f, 0.5f), (Vector2)midPoint + new Vector2(0f, 0.5f));

                    MagnetVelocity(midPoint);
                }
                else{
                    drawing.SetActive(false);
                }
            }
            else{
                drawing.SetActive(false);
            }
        }*/

        void MagnetVelocity(Vector2 positionTo){
            _frameVelocity = (positionTo - (Vector2)transform.position).normalized * 15f;
        }

        /*public void MagnetVelocity(Vector2 vel){
            _frameVelocity += vel;

            if (_frameVelocity.x < 0)
                _frameVelocity.x = Mathf.Max(_frameVelocity.x, -24f);
            else
                _frameVelocity.x = Mathf.Min(_frameVelocity.x, 24f);

            if (_frameVelocity.y < 0)
                _frameVelocity.y = Mathf.Max(_frameVelocity.y, -40f);
            else
                _frameVelocity.y = Mathf.Min(_frameVelocity.y, 40f);
        }*/

        Vector2 FindMidpoint(Vector2 v1, Vector2 v2){
            return new Vector2((v1.x + v2.x)/2, (v1.y + v2.y)/2);
        }

        void DrawLine(Vector2 pos1, Vector2 pos2, int index){
            LineRenderer lr = lines[index].GetComponent<LineRenderer>();
            lr.SetPosition(0, pos1);
            lr.SetPosition(1, pos2);
            //Debug.Log("done");
        }

        void Randomize()
        {
            int randomInt = UnityEngine.Random.Range(1, 5); 

            switch (randomInt)
            {
                case 4:
                    material.SetFloat("width", 0.5f); 
                    material.SetFloat("heigth", 0.1f); 
                    randomColor = Color.yellow;
                    break;
                case 3:
                    material.SetFloat("width", 0.5f); 
                    material.SetFloat("heigth", 1f); 
                    randomColor = Color.cyan;
                    break;
                case 2:
                    material.SetFloat("width", 0.75f); 
                    material.SetFloat("heigth", 0.1f); 
                    randomColor = Color.green;
                    break;      
                case 1:
                    material.SetFloat("width", 0.4f); 
                    material.SetFloat("heigth", 0.8f); 
                    randomColor = Color.red;
                    break;
            }
        
        }

        #endregion

        public Vector2 GetFrameVelocity(){
            return _frameVelocity;
        }

        public void SetVelocity(Vector2 vell)
        {
            _frameVelocity = vell;
        }

        bool isInBounce = false;
        public void BouncyJump(float bounciness){
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            _frameVelocity.y = bounciness;
            isInBounce = true;
            

            _jumpToConsume = false;
        }

        private void ApplyMovement(){
             _rb.velocity = _frameVelocity + velocityIncrease;
             velocityIncrease = Vector2.zero;
        }

        public void IncreaseVelocity(Vector2 increase){
            velocityIncrease += increase;
        }   

    }

    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public Vector2 Move;

        public bool ToggleMagnetism;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
        public Vector2 FrameInput { get; }
    }
}