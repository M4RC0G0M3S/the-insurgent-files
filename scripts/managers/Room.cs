using Godot;

// Room.cs — Script de cada sala
// Calcula os limites da câmara automaticamente a partir do TileMapLayer

public partial class Room : Node2D
{
    public override void _Ready()
    {
        CallDeferred(nameof(ApplyCameraLimits));
    }

    private void ApplyCameraLimits()
    {
        Player player = GetTree().GetFirstNodeInGroup("Player") as Player;

        if (player == null)
        {
            GD.Print("ERRO: Player não encontrado");
            return;
        }

        // Godot 4.4 usa TileMapLayer em vez de TileMap
        TileMapLayer tileMapLayer = GetNodeOrNull<TileMapLayer>("TileMap/Chao");

        if (tileMapLayer != null)
        {
            Rect2I usedRect = tileMapLayer.GetUsedRect();
            Vector2I tileSize = tileMapLayer.TileSet.TileSize;

            int left   = usedRect.Position.X * tileSize.X;
            int top    = usedRect.Position.Y * tileSize.Y;
            int right  = (usedRect.Position.X + usedRect.Size.X) * tileSize.X;
            int bottom = (usedRect.Position.Y + usedRect.Size.Y) * tileSize.Y;

            int margin = 32;
            player.SetCameraLimits(left - margin, right + margin, top - margin, bottom + margin);
            GD.Print("Limites automáticos aplicados!");
        }
        else
        {
            GD.Print("TileMapLayer não encontrado — câmara sem limites");
        }
    }
}