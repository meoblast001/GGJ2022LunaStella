using Godot;

public class Switch : KinematicBody {
	
  [Signal] OpenDoor;

  private bool isPressed = false;

  public override void _Ready() {
	
  }

}
