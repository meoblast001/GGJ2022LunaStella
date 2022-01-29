using Godot;

public class Level2 : Spatial {
    private void _on_Goal_GameWin() {
        GD.Print("PLAY ME SOMETHIGN");
        GetNode<AudioStreamPlayer>("AudioBG").Stop();
        GetNode<AudioStreamPlayer>("AudioWin").Play();

    }

}
