using System;
using Godot;
using HUD;
using Input;
using Magnetism;

namespace Player {
  public class Player : KinematicBody {
    private enum AnimationState {
      Idle,
      Running,
      Jump
    }

    private enum Direction {
      Left,
      Right
    }

    [Export] private float playerSpeed;
    [Export] private float jumpSpeed;
    [Export] private NodePath magneticAreaPath;
    [Export] private NodePath magnetPickupAreaPath;
    [Export] private NodePath meshPath;
    [Export] private NodePath magnetHoldPath;
    [Export] private NodePath jumpAudioPath;
    [Export] private float magnetInnerBound;

    private float gravity;
    private float yVelocity = 0;
    private Area magneticArea;
    private Area magnetPickupArea;
    private Spatial mesh;
    private Spatial magnetHold;
    private AudioStreamPlayer3D jumpAudio;
    private AnimationPlayer animationPlayer;
    private AnimationState animationState = (AnimationState)(-1);
    private bool attract = false;
    private Direction direction = Direction.Left;

    private Magnet.Magnet magnet = null;
    private RigidBody.ModeEnum originalMagnetRigidBodyMode;

    public override void _Ready() {
      this.gravity = -((float)ProjectSettings.GetSetting("physics/3d/default_gravity"));
      this.magneticArea = GetNode<Area>(this.magneticAreaPath);
      this.magnetPickupArea = GetNode<Area>(this.magnetPickupAreaPath);
      this.mesh = GetNode<Spatial>(this.meshPath);
      this.magnetHold = GetNode<Spatial>(this.magnetHoldPath);
      this.jumpAudio = GetNode<AudioStreamPlayer3D>(this.jumpAudioPath);
      this.animationPlayer = this.mesh.GetNode<AnimationPlayer>("AnimationPlayer");

      SetAnimationState(AnimationState.Idle);
      Hud.Singleton.PlayerIsAttracting = false;
    }

    public override void _Process(float delta) {
      base._Process(delta);

      if (this.magnet != null) {
        this.magnet.GlobalTransform
          = new Transform(this.magnet.GlobalTransform.basis, this.magnetHold.GlobalTransform.origin);
        this.magnet.Rotation = Vector3.Zero;
      }
    }

    public override void _PhysicsProcess(float delta) {
      base._PhysicsProcess(delta);

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

      if (movement.x > 0) {
        this.mesh.LookAt(this.Transform.origin + Vector3.Left, Vector3.Up);
        this.direction = Direction.Right;
      } else if (movement.x < 0) {
        this.mesh.LookAt(this.Transform.origin + Vector3.Right, Vector3.Up);
        this.direction = Direction.Left;
      }

      MoveAndSlide(new Vector3(movement.x * this.playerSpeed, yVelocity, 0f), Vector3.Up);
      if (this.attract) {
        Attract(delta);
      } else {
        Repel(delta);
      }
    }

    public override void _Input(InputEvent @event) {
      if (@event.IsActionPressed(ActionTypes.Jump))
        Jump();
      else if (@event.IsActionPressed(ActionTypes.Interact)) {
        if (this.attract) {
          this.attract = false;
          Hud.Singleton.PlayerIsAttracting = false;
        }
        else {
          this.attract = true;
          Hud.Singleton.PlayerIsAttracting = true;
        }
      } else if (@event.IsActionPressed(ActionTypes.Throw)) {
        Throw();
      }
    }

    public void OnBodyEnterMagnetPickupArea(Node body) {
      if (!body.IsInGroup(Groups.Magnet))
        return;

      this.magnet = (Magnet.Magnet) body;
      this.originalMagnetRigidBodyMode = this.magnet.Mode;
      this.magnet.Mode = RigidBody.ModeEnum.Static;
    }

    private void Jump() {
      if (IsOnFloor()) {
        this.yVelocity = this.jumpSpeed;
        SetAnimationState(AnimationState.Jump);
        if (!this.jumpAudio.Playing)
          this.jumpAudio.Play();
      }
    }

    private void Throw() {
      if (this.magnet == null)
        return;

      this.magnet.Mode = this.originalMagnetRigidBodyMode;
      this.magnet.AddCentralForce(this.direction == Direction.Left
        ? new Vector3(-1f, 1f, 0f * 2000f)
        : new Vector3(1f, 1f, 0f) * 2000f);
      this.magnet = null;
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

    private void Repel(float delta) {
      var bodies = this.magneticArea.GetOverlappingBodies();
      foreach (var body in bodies) {
        if (body == this)
          continue;
        var node = (Spatial)body;
        switch (body) {
          case IMagnetic magnetic: {
            var forceDirection = (this.GlobalTransform.origin - node.GlobalTransform.origin).Normalized();
            magnetic.ApplyRepellingForce(-forceDirection, delta);
            break;
          }
          case IPassiveMagnetic passiveMagnetic: {
            var forceDirection = (node.GlobalTransform.origin - this.GlobalTransform.origin).Normalized();
            this.ApplyRepellingForce(-forceDirection, passiveMagnetic.RepelStrength, delta);
            break;
          }
        }
      }
    }

    private void Attract(float delta) {
      var bodies = this.magneticArea.GetOverlappingBodies();
      foreach (var body in bodies) {
        if (body == this)
          continue;
        var node = (Spatial)body;
        switch (body) {
          case IMagnetic magnetic: {
            var distance = this.GlobalTransform.origin.DistanceTo(node.GlobalTransform.origin);
            if (distance >= this.magnetInnerBound) {
              var forceDirection = (this.GlobalTransform.origin - node.GlobalTransform.origin).Normalized();
              magnetic.ApplyAttractionForce(forceDirection, delta);
            }
            break;
          }
          case IPassiveMagnetic passiveMagnetic: {
            var distance = this.GlobalTransform.origin.DistanceTo(node.GlobalTransform.origin);
            if (distance >= this.magnetInnerBound) {
              var forceDirection = (node.GlobalTransform.origin - this.GlobalTransform.origin).Normalized();
              this.ApplyAttractionForce(forceDirection, passiveMagnetic.AttractStrength, delta);
            }
            break;
          }
        }
      }
    }

    private void ApplyRepellingForce(Vector3 direction, float strength, float delta) {
      if (this.magnet != null)
        MoveAndSlide(direction * strength * delta);
    }

    private void ApplyAttractionForce(Vector3 direction, float strength, float delta) {
      if (this.magnet != null)
        MoveAndSlide(direction * strength * delta);
    }
  }
}
