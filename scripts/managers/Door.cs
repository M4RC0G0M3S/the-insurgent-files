using Godot;

// Door.cs — Script da porta
// Nó raiz: StaticBody2D
// Responsável por: abrir/fechar porta quando o jogador prime E

public partial class Door : StaticBody2D
{
    // Frames do tileset para cada estado da porta
    [Export] public int FrameFechada = 45;  // sprite porta fechada
    [Export] public int FrameAberta = 21;   // sprite porta aberta

    // Referências aos nós filhos
    private Sprite2D _sprite;
    private CollisionShape2D _collision;

    // Estado atual da porta
    private bool _estaAberta = false;

    // O jogador está perto o suficiente para interagir?
    private bool _jogadorPerto = false;

    public override void _Ready()
    {
        // GetNode busca um nó filho pelo nome
        _sprite = GetNode<Sprite2D>("Sprite2D");
        _collision = GetNode<CollisionShape2D>("CollisionShape2D");

        // Liga os sinais do Area2D
        // Sinais são eventos — "alguém entrou na área", "alguém saiu"
        Area2D area = GetNode<Area2D>("Area2D");
        area.BodyEntered += OnBodyEntered;
        area.BodyExited += OnBodyExited;
    }

    public override void _Input(InputEvent @event)
    {
        // Só abre/fecha se o jogador estiver perto E premir E
        if (_jogadorPerto && @event.IsActionPressed("interact"))
        {
            ToggleDoor();
        }
    }

    private void ToggleDoor()
    {
        _estaAberta = !_estaAberta;

        if (_estaAberta)
        {
            // Abre a porta — muda sprite e desativa colisão
            _sprite.Frame = FrameAberta;
            _collision.Disabled = true;
            GD.Print("Porta aberta!");
        }
        else
        {
            // Fecha a porta — muda sprite e ativa colisão
            _sprite.Frame = FrameFechada;
            _collision.Disabled = false;
            GD.Print("Porta fechada!");
        }
    }

    // Chamado quando um corpo entra na Area2D
    private void OnBodyEntered(Node2D body)
    {
        if (body.Name == "Player")
        {
            _jogadorPerto = true;
            GD.Print("Perto da porta — prime E para interagir");
        }
    }

    // Chamado quando um corpo sai da Area2D
    private void OnBodyExited(Node2D body)
    {
        if (body.Name == "Player")
        {
            _jogadorPerto = false;
        }
    }
}
