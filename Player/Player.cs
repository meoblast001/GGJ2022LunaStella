using System;
using Godot;
using Input;

namespace Player {
  public class Player : KinematicBody {
    [Export] private float playerSpeed;
    [Export] private float jumpSpeed;

    private float gravity;
    private float yVelocity = 0;

    public override void _Ready() {
      this.gravity = -((float) ProjectSettings.GetSetting("physics/3d/default_gravity"));
    }

    public override void _PhysicsProcess(float delta) {
      var movement = Godot.Input.GetVector(
        ActionTypes.MoveLeft,
        ActionTypes.MoveRight,
        ActionTypes.MoveDown,
        ActionTypes.MoveUp);
      yVelocity = Math.Max(yVelocity + delta * gravity, gravity);
      MoveAndSlide(new Vector3(movement.x * this.playerSpeed, yVelocity, 0f), Vector3.Up);
    }

    public override void _Input(InputEvent @event) {
      if (@event.IsActionPressed(ActionTypes.Jump))
        Jump();
      else if (@event.IsActionPressed(ActionTypes.Interact))
        GD.Print("Interact");
    }

    private void Jump() {
      if (IsOnFloor())
        this.yVelocity = this.jumpSpeed;
    }
  }
}