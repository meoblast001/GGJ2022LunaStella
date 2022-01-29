using Godot;

namespace HUD {
  public class Hud : Control {
    [Export] private NodePath levelCompleteLabelPath;

    private Label levelCompleteLabel;

    public static Hud Singleton { get; private set; }

    public override void _Ready() {
      this.levelCompleteLabel = GetNode<Label>(this.levelCompleteLabelPath);

      this.levelCompleteLabel.Hide();

      Singleton = this;
    }

    public bool IsLevelCompleteLabelVisible {
      set {
        if (value)
          this.levelCompleteLabel.Show();
        else
          this.levelCompleteLabel.Hide();
      }
    }
  }
}
