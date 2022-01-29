using System;
using Godot;
using Input;
using Magnetism;

namespace Player {
  public class Player : KinematicBody {
    private enum AnimationState {
      Idle,
      Running,
      Jump
    }

    [Export] private float playerSpeed;
    [Export] private float jumpSpeed;
    [Export] private NodePath magneticAreaPath;
    [Export] private NodePath meshPath;

    private float gravity;
    private float yVelocity = 0;
    private Area magneticArea;
    private Spatial mesh;
    private AnimationPlayer animationPlayer;
    private AnimationState animationState = (AnimationState)(-1);

    public override void _Ready() {
      this.gravity = -((float) ProjectSettings.GetSetting("physics/3d/default_gravity"));
      this.magneticArea = GetNode<Area>(this.magneticAreaPath);
      this.mesh = GetNode<Spatial>(this.meshPath);
      this.animationPlayer = this.mesh.GetNode<AnimationPlayer>("AnimationPlayer");

      SetAnimationState(AnimationState.Idle);
    }

    public override void _PhysicsProcess(float delta) {
      var movement = Godot.Input.GetVector(
        ActionTypes.MoveLeft,
        ActionTypes.MoveRight,
        ActionTypes.MoveDown,
        ActionTypes.MoveUp);
      yVelocity = Math.Max(yVelocity + delta * gravity, gravity);

      if (this.animationState != AnimationState.Jump) {
        if (movement.x != 0)
          SetAnimationState(AnimationState.Running);
        else
          SetAnimationState(AnimationState.Idle);
      }
      else if (IsOnFloor() && this.yVelocity <= 0) {
        SetAnimationState(AnimationState.Idle);
      }

      if (movement.x > 0)
        this.mesh.LookAt(this.Transform.origin + Vector3.Left, Vector3.Up);
      else if (movement.x < 0)
        this.mesh.LookAt(this.Transform.origin + Vector3.Right, Vector3.Up);

      MoveAndSlide(new Vector3(movement.x * this.playerSpeed, yVelocity, 0f), Vector3.Up);
    }

    public override void _Input(InputEvent @event) {
      if (@event.IsActionPressed(ActionTypes.Jump))
        Jump();
      else if (@event.IsActionPressed(ActionTypes.Interact))
        Magnetize();
    }

    private void Jump() {
      if (IsOnFloor()) {
        this.yVelocity = this.jumpSpeed;
        SetAnimationState(AnimationState.Jump);
      }
    }

    private void SetAnimationState(AnimationState newState) {
      if (newState != this.animationState) {
        this.animationState = newState;

        string animationName;
        switch (this.animationState) {
          case AnimationState.Idle:
            animationName = "default";
            break;
          case AnimationState.Running:
            animationName = "PlayerRun";
            break;
          case AnimationState.Jump:
            animationName = "PlayerJump";
            break;
          default:
            return;
        }
        this.animationPlayer.Play(animationName);
      }
    }

    private void Magnetize() {
      var bodies = this.magneticArea.GetOverlappingBodies();
      foreach (var body in bodies) {
        if (body == this)
          continue;
        var node = (Spatial) body;
        switch (body) {
          case IMagnetic magnetic:
            var forceDirection = this.GlobalTransform.origin - node.GlobalTransform.origin;
            magnetic.ApplyMagneticForce(forceDirection);
            break;
        }
      }
    }
  }
}
