using Godot;

public partial class HUD : Control
{
    private ProgressBar _hpBar;
    private ProgressBar _armorBar;
    private Player _player;

    public override void _Ready()
	{
		// O caminho agora inclui o container pai
		_hpBar    = GetNode<ProgressBar>("VBoxContainer/HPContainer/HPBar");
		_armorBar = GetNode<ProgressBar>("VBoxContainer/ArmorContainer/ArmorBar");
	}

	public override void _Process(double delta)
	{
		if (_player == null)
		{
			_player = GetTree().GetFirstNodeInGroup("Player") as Player;
			
			// Diagnóstico — remove depois
			GD.Print("HUD: à procura do player... encontrado: ", _player != null);

			if (_player == null) return;

			_hpBar.MaxValue    = _player.MaxHP;
			_armorBar.MaxValue = _player.MaxArmor;
		}

		_hpBar.Value    = _player.CurrentHP;
		_armorBar.Value = _player.CurrentArmor;
	}
}