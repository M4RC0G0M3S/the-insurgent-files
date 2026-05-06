using Godot;

// TransitionData.cs — Dados partilhados entre transições de cenas
// Classe estática — não precisa de ser instanciada
// Guarda informação que precisa de persistir entre cenas

public static class TransitionData
{
    // Posição onde o player vai aparecer na nova sala
    public static Vector2 SpawnPosition = Vector2.Zero;
}