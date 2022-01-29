using Godot;

namespace HUD {
  public class Hud : Control {
    [Export] private NodePath levelCompleteLabelPath;
    [Export] private NodePath playerIsAttractingPath;

    private Label levelCompleteLabel;
    private Label playerIsAttracting;

    public static Hud Singleton { get; private set; }

    public override void _Ready() {
      this.levelCompleteLabel = GetNode<Label>(this.levelCompleteLabelPath);
      this.playerIsAttracting = GetNode<Label>(this.playerIsAttractingPath);

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

    public bool PlayerIsAttracting {
      set => this.playerIsAttracting.Text = value
        ? "Player magnetic"
        : "Player NOT magnetic";
    }
  }
}
