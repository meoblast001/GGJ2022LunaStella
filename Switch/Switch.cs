using Godot;

namespace Switch {
  public class Switch : Area {

	[Signal]
	delegate void OpenDoor(bool isDoorOpen);

	private bool isPressed = false;

	private void _on_Switch_body_entered(object body) {
	  if (body is KinematicBody) { // should Player
		this.isPressed = true;
		EmitSignal("OpenDoor", this.isPressed);
	  }
	}

	private void _on_Switch_body_exited(object body) {
	  if (body is KinematicBody) { // should be Player
		this.isPressed = false;
		EmitSignal("OpenDoor", this.isPressed);
	  }
	}
  }
}

