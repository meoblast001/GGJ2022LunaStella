using Godot;

namespace Door {
  public class Door : KinematicBody {

	[Export] private float transformFactor = 1.5f;
	private bool isOpen = false;
	
	private void _on_Switch_OpenDoor(bool isDoorOpen) {
		if (isDoorOpen == true) {
			this.isOpen = true;
			//TranslateObjectLocal(Transform.basis.y * (this.transformFactor));
			GetNode<AnimationPlayer>("AnimationPlayer").Play("Open");
		} else {
			this.isOpen = false;
			//TranslateObjectLocal(-Transform.basis.y * (this.transformFactor));
			GetNode<AnimationPlayer>("AnimationPlayer").Play("Open (copy)");
		}
	}

  }
}

