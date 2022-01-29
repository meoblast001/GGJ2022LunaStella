using Godot;

namespace Switch {
  public class Switch : Area {
	
	[Signal]
	delegate void OpenDoor(bool isDoorOpen);

	private bool isPressed = false;

	private void _on_Switch_body_shape_entered(RID body_rid, object body, int body_shape_index, int local_shape_index) {
	  this.isPressed = true;
	  EmitSignal("OpenDoor", this.isPressed);
	}
	
	private void _on_Switch_body_shape_exited(RID body_rid, object body, int body_shape_index, int local_shape_index) {
	  this.isPressed = false;
	  EmitSignal("OpenDoor", this.isPressed);
	}
  }
}









