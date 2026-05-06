using Godot;
using System;

// Door.cs — Script da porta
// Nó raiz: StaticBody2D
// Responsável por: abrir/fechar porta e transição entre salas

public partial class Door : StaticBody2D
{
    [Export] public int FrameFechada = 21;
    [Export] public int FrameAberta = 45;

    // Se tiver caminho definido, esta porta leva a outra sala
    // Deixa vazio se for uma porta normal sem transição
    [Export] public string TargetScene = "";

    // Posição onde o player aparece na nova sala
    [Export] public Vector2 SpawnPosition = Vector2.Zero;

    private Sprite2D _sprite;
    private CollisionShape2D _collision;
    private bool _estaAberta = false;
    private bool _jogadorPerto = false;

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite2D");
        _collision = GetNode<CollisionShape2D>("CollisionShape2D");

        Area2D area = GetNode<Area2D>("Area2D");
        area.BodyEntered += OnBodyEntered;
        area.BodyExited += OnBodyExited;
    }

    public override void _Process(double delta)
    {
        if (_jogadorPerto && Input.IsActionJustPressed("interact"))
        {
            ToggleDoor();

            if (_estaAberta && !string.IsNullOrWhiteSpace(TargetScene))
            {
                TryChangeScene();
            }
        }
    }

    private void TryChangeScene()
    {
        string scenePath = ResolveScenePath(TargetScene);

        if (!ResourceLoader.Exists(scenePath))
        {
            GD.PrintErr("Cena de destino nao encontrada: ", scenePath, " | TargetScene: ", TargetScene);
            return;
        }

        GD.Print("A mudar para: ", scenePath);
        TransitionData.SpawnPosition = SpawnPosition;

        Error err = GetTree().ChangeSceneToFile(scenePath);
        if (err != Error.Ok)
        {
            GD.PrintErr("Falha ao mudar de cena: ", err);
        }
    }

    private string ResolveScenePath(string target)
    {
        string trimmed = target.Trim();
        if (trimmed.StartsWith("res://", StringComparison.Ordinal))
        {
            return trimmed;
        }

        if (!trimmed.EndsWith(".tscn", StringComparison.OrdinalIgnoreCase))
        {
            trimmed += ".tscn";
        }

        string levelsPath = "res://scenes/levels/" + trimmed;
        if (ResourceLoader.Exists(levelsPath))
        {
            return levelsPath;
        }

        string rootPath = "res://" + trimmed;
        if (ResourceLoader.Exists(rootPath))
        {
            return rootPath;
        }

        return levelsPath;
    }

    private void ToggleDoor()
    {
        _estaAberta = !_estaAberta;

        if (_estaAberta)
        {
            _sprite.Frame = FrameAberta;
            _collision.Disabled = true;
            GD.Print("Porta aberta!");
        }
        else
        {
            _sprite.Frame = FrameFechada;
            _collision.Disabled = false;
            GD.Print("Porta fechada!");
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        if (IsPlayer(body))
        {
            _jogadorPerto = true;
            GD.Print("Perto da porta — prime E para interagir");
        }
    }

    private void OnBodyExited(Node2D body)
    {
        if (IsPlayer(body))
        {
            _jogadorPerto = false;
        }
    }

    private bool IsPlayer(Node2D body)
    {
        return body.IsInGroup("Player") || body is Player;
    }
}