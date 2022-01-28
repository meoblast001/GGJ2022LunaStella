using Godot;
using Input;

namespace Player {
  public class Player : KinematicBody {
    [Export] private float playerSpeed;

    private float gravity;

    public override void _Ready() {
      this.gravity = -((float) ProjectSettings.GetSetting("physics/3d/default_gravity"));
    }

    public override void _PhysicsProcess(float delta) {
      var movement = Godot.Input.GetVector(
        ActionTypes.MoveLeft,
        ActionTypes.MoveRight,
        ActionTypes.MoveDown,
        ActionTypes.MoveUp);
      MoveAndSlide(new Vector3(movement.x * this.playerSpeed, gravity, 0f), Vector3.Up);
    }

    public override void _Input(InputEvent @event) {
      if (@event.IsActionPressed(ActionTypes.Jump))
        GD.Print("Jump pressed");
      else if (@event.IsActionPressed(ActionTypes.Interact))
        GD.Print("Interact");
    }
  }
}