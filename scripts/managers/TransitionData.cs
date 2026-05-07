using Godot;

// TransitionData.cs — Dados partilhados entre transições de cenas
// Classe estática — não precisa de ser instanciada
// Guarda informação que precisa de persistir entre cenas

public static class TransitionData
{
    public static Vector2 SpawnPosition = Vector2.Zero;

    // Guarda o estado do player entre salas
    // -1 significa "não inicializado ainda" — primeira vez que o jogo corre
    public static int CurrentHP    = -1;
    public static int CurrentArmor = -1;
}