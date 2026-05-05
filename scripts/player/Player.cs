using Godot;

// Player.cs — Script principal do jogador
// Nó raiz: CharacterBody2D
// Este script controla: movimento, rotação para o rato, HP e Armor

public partial class Player : CharacterBody2D
{
    // [Export] torna a variável editável no Inspector do Godot
    // Assim podes mudar os valores sem tocar no código
    [Export] public float MoveSpeed = 150f;
    [Export] public float SprintSpeed = 280f;
    [Export] public int MaxHP = 100;
    [Export] public int MaxArmor = 50;

    // Variáveis privadas — só usadas dentro deste script
    private int _currentHP;
    private int _currentArmor;

    // _Ready() é chamado UMA VEZ quando a cena carrega
    // É aqui que inicializamos os valores de HP e Armor
    public override void _Ready()
    {
        _currentHP = MaxHP;
        _currentArmor = MaxArmor;
        GD.Print("Player iniciado | HP: ", _currentHP, " | Armor: ", _currentArmor);
    }

    // _PhysicsProcess() é chamado 60 vezes por segundo
    // Tudo o que seja movimento vai sempre aqui
    // 'delta' = tempo desde o último frame (garante movimento suave)
    public override void _PhysicsProcess(double delta)
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        // Input.GetVector lê WASD ou setas — devolve Vector2 normalizado
        // Normalizado significa que andar na diagonal não é mais rápido
        Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");

        // Sprint com Shift
        float speed = Input.IsActionPressed("sprint") ? SprintSpeed : MoveSpeed;

        // Velocity é a propriedade do CharacterBody2D
        // MoveAndSlide() aplica o movimento e resolve colisões automaticamente
        Velocity = direction * speed;
        MoveAndSlide();
    }

    private void HandleRotation()
    {
        // O jogador roda para apontar sempre para o rato
        // GetGlobalMousePosition() devolve a posição do rato no mundo do jogo
        LookAt(GetGlobalMousePosition());
    }

    // Chamado quando o jogador recebe dano
    // O Armor absorve primeiro, depois o HP
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
        // Vamos expandir isto na F3
    }
}
