using Godot;
using HUD;

public class Goal : Area {
  [Export] private NodePath timerPath;

  private Timer timer;
  private bool goalReached = false;

  public override void _Ready() {
    this.timer = GetNode<Timer>(this.timerPath);

    this.timer.Connect("timeout", this, nameof(OnTimerExpired));
  }

  public void OnBodyEnter(Node node) {
    if (this.goalReached || !node.IsInGroup(Groups.Player))
      return;
    this.goalReached = true;
    Hud.Singleton.IsLevelCompleteLabelVisible = true;
    this.timer.Start();
  }

  private void OnTimerExpired() {
    Hud.Singleton.IsLevelCompleteLabelVisible = false;
    GetTree().Quit();
  }
}
