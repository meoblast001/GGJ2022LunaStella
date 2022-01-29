using Godot;

namespace Switch {
  public class Switch : Area {

	[Signal]
	delegate void OpenDoor(bool isDoorOpen);

	private bool isPressed = false;

	private void _on_Switch_body_entered(object body) {
	  if (body is KinematicBody) { // should Player
		this.isPressed = true;
	GetNode<AnimationPlayer>("AnimationPlayer").Play("StepOn");
		EmitSignal("OpenDoor", this.isPressed);
	  }
	}

	private void _on_Switch_body_exited(object body) {
	  if (body is KinematicBody) { // should be Player
		this.isPressed = false;
	GetNode<AnimationPlayer>("AnimationPlayer").Play("StepOff");
		EmitSignal("OpenDoor", this.isPressed);
	  }
	}
  }
}

