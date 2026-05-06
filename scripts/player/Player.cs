using Godot;

// Player.cs — Script principal do jogador
// Nó raiz: CharacterBody2D
// Este script controla: movimento, rotação para o rato, HP e Armor

public partial class Player : CharacterBody2D
{
    [Export] public float MoveSpeed = 150f;
    [Export] public float SprintSpeed = 280f;
    [Export] public int MaxHP = 100;
    [Export] public int MaxArmor = 50;

    private int _currentHP;
    private int _currentArmor;

    public override void _Ready()
    {
        _currentHP = MaxHP;
        _currentArmor = MaxArmor;
        GD.Print("Player iniciado | HP: ", _currentHP, " | Armor: ", _currentArmor);
    }

    public override void _PhysicsProcess(double delta)
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        float speed = Input.IsActionPressed("sprint") ? SprintSpeed : MoveSpeed;
        Velocity = direction * speed;
        MoveAndSlide();
    }

    private void HandleRotation()
    {
        Vector2 mousePos = GetGlobalMousePosition();
        Sprite2D sprite = GetNode<Sprite2D>("Sprite2D");

        if (mousePos.X < GlobalPosition.X)
        {
            // Rato à esquerda — flipa o sprite e corrige a rotação
            sprite.FlipH = true;
            LookAt(mousePos);
            Rotation += Mathf.Pi; // adiciona 180° para compensar o flip
        }
        else
        {
            // Rato à direita — comportamento normal
            sprite.FlipH = false;
            LookAt(mousePos);
        }
    }

    public void TakeDamage(int damage)
    {
        if (_currentArmor > 0)
        {
            int absorbed = Mathf.Min(_currentArmor, damage);
            _currentArmor -= absorbed;
            damage -= absorbed;
        }

        _currentHP = Mathf.Max(_currentHP - damage, 0);
        GD.Print("Dano! HP: ", _currentHP, " | Armor: ", _currentArmor);

        if (_currentHP <= 0) Die();
    }

    private void Die()
    {
        GD.Print("Player morreu.");
    }

    public void SetCameraLimits(int left, int right, int top, int bottom)
    {
        Camera2D camera = GetNode<Camera2D>("Camera2D");
        camera.LimitLeft = left;
        camera.LimitRight = right;
        camera.LimitTop = top;
        camera.LimitBottom = bottom;
    }
}