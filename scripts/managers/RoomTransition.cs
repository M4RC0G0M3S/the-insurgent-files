using Godot;

// RoomTransition.cs — Trigger de transição entre salas
// Nó raiz: Area2D
// Quando o jogador entra na área, muda para a cena destino

public partial class RoomTransition : Area2D
{
    // Caminho para a cena destino — editável no Inspector
    // Exemplo: "res://scenes/levels/test_level_2.tscn"
    [Export] public string TargetScene = "";

    // Posição onde o jogador vai aparecer na nova sala
    [Export] public Vector2 SpawnPosition = Vector2.Zero;

    public override void _Ready()
    {
        // Liga o sinal — quando um corpo entra na área chama OnBodyEntered
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        // Só o Player ativa a transição
        if (body.IsInGroup("Player") && TargetScene != "")
        {
            GD.Print("A mudar para: ", TargetScene);

            // Guarda a posição de spawn numa variável global
            // para o player aparecer no sítio certo na nova sala
            TransitionData.SpawnPosition = SpawnPosition;

            // ChangeSceneToFile() carrega a nova cena e destroi a atual
            GetTree().ChangeSceneToFile(TargetScene);
        }
    }
}
