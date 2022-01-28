using Godot;
using Input;

namespace Player {
  public class Player : KinematicBody {
    public override void _PhysicsProcess(float delta) {
      var movement = Godot.Input.GetVector(
        ActionTypes.MoveLeft,
        ActionTypes.MoveRight,
        ActionTypes.MoveDown,
        ActionTypes.MoveUp);
      if (movement != Vector2.Zero)
        GD.Print($"Current vector is {movement}");
    }

    public override void _Input(InputEvent @event) {
      if (@event.IsActionPressed(ActionTypes.Jump))
        GD.Print("Jump pressed");
      else if (@event.IsActionPressed(ActionTypes.Interact))
        GD.Print("Interact");
    }
  }
}