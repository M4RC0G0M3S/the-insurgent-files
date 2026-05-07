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

    public int CurrentHP    { get; private set; }
    public int CurrentArmor { get; private set; }

    public override void _Ready()
    {
        // Se TransitionData tem valores guardados, usa-os
        // Senão começa com valores máximos (primeira vez que o jogo corre)
        CurrentHP    = TransitionData.CurrentHP    == -1 ? MaxHP    : TransitionData.CurrentHP;
        CurrentArmor = TransitionData.CurrentArmor == -1 ? MaxArmor : TransitionData.CurrentArmor;

        GD.Print("Player iniciado | HP: ", CurrentHP, " | Armor: ", CurrentArmor);
    }

    public override void _PhysicsProcess(double delta)
    {
        HandleMovement();
        HandleRotation();
    }

    public override void _Process(double delta)
    {
        // Teste temporário — remove depois
        if (Input.IsKeyPressed(Key.T))
            TakeDamage(1);
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
            sprite.FlipH = true;
            LookAt(mousePos);
            Rotation += Mathf.Pi;
        }
        else
        {
            sprite.FlipH = false;
            LookAt(mousePos);
        }
    }

    public void TakeDamage(int damage)
    {
        if (CurrentArmor > 0)
        {
            int absorbed = Mathf.Min(CurrentArmor, damage);
            CurrentArmor -= absorbed;
            damage -= absorbed;
        }

        CurrentHP = Mathf.Max(CurrentHP - damage, 0);

        // Guarda sempre que o estado muda
        TransitionData.CurrentHP    = CurrentHP;
        TransitionData.CurrentArmor = CurrentArmor;

        GD.Print("Dano! HP: ", CurrentHP, " | Armor: ", CurrentArmor);

        if (CurrentHP <= 0) Die();
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