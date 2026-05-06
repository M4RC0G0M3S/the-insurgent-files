using Godot;

// AmbientLight.cs — Controla a luz ambiente de cada sala
// Nó raiz: CanvasModulate
// O CanvasModulate multiplica a sua cor por TUDO no canvas
// Branco (1,1,1) = sem efeito | Preto (0,0,0) = escuridão total

public partial class AmbientLight : CanvasModulate
{
    // Cor ambiente da sala — editável no Inspector
    // Experimenta: Color(0.1, 0.1, 0.15) para azul-escuro noturno
    [Export] public Color AmbientColor = new Color(0.15f, 0.15f, 0.2f);

    // Tempo em segundos para a luz fazer fade ao entrar na sala
    [Export] public float FadeInDuration = 0.8f;

    public override void _Ready()
    {
        // Começa com escuridão total e faz fade para a cor ambiente
        Color = Colors.Black;
        FadeToAmbient();
    }

    // Anima a transição de preto para a cor ambiente da sala
    private void FadeToAmbient()
    {
        // Cria um Tween — ferramenta do Godot para animações de valor
        Tween tween = CreateTween();

        // TweenProperty anima a propriedade "color" deste nó
        // De: Colors.Black  →  Para: AmbientColor
        // Durante: FadeInDuration segundos
        tween.TweenProperty(this, "color", AmbientColor, FadeInDuration)
             .SetTrans(Tween.TransitionType.Sine)   // curva suave
             .SetEase(Tween.EaseType.Out);           // desacelera no fim
    }

    // Permite mudar a cor em tempo de execução (útil para eventos futuros)
    // Exemplo: luz vermelha de alarme, flash de explosão
    public void SetColor(Color newColor, float duration = 0.5f)
    {
        Tween tween = CreateTween();
        tween.TweenProperty(this, "color", newColor, duration)
             .SetTrans(Tween.TransitionType.Sine)
             .SetEase(Tween.EaseType.InOut);
    }

    // Atalhos prontos a usar
    public void SetDark()   => SetColor(new Color(0.1f, 0.1f, 0.15f));
    public void SetNormal() => SetColor(new Color(0.15f, 0.15f, 0.2f));
    public void SetAlarm()  => SetColor(new Color(0.3f, 0.05f, 0.05f));
}