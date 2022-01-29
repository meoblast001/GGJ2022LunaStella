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
		var audio = GetNode<AudioStreamPlayer3D>("AudioOn");
		if (!audio.Playing) {
		  audio.Play();
		}
		EmitSignal("OpenDoor", this.isPressed);
	  }
	}

	private void _on_Switch_body_exited(object body) {
	  if (body is KinematicBody) { // should be Player
		this.isPressed = false;
		GetNode<AnimationPlayer>("AnimationPlayer").Play("StepOff");
		var audio = GetNode<AudioStreamPlayer3D>("AudioOff");
		if (!audio.Playing) {
		  audio.Play();
		}
		EmitSignal("OpenDoor", this.isPressed);
	  }
	}
  }
}

