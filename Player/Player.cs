using System;
using Godot;
using Input;
using Magnetism;

namespace Player {
  public class Player : KinematicBody {
	[Export] private float playerSpeed;
	[Export] private float jumpSpeed;
	[Export] private NodePath magneticAreaPath;

	private float gravity;
	private float yVelocity = 0;
	private Area magneticArea;

	public override void _Ready() {
	  this.gravity = -((float) ProjectSettings.GetSetting("physics/3d/default_gravity"));
	  this.magneticArea = GetNode<Area>(this.magneticAreaPath);
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
		Magnetize();
	}

	private void Jump() {
	  if (IsOnFloor())
		this.yVelocity = this.jumpSpeed;
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
