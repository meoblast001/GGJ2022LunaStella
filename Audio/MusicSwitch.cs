using Godot;

namespace Audio {
  public class MusicSwitch : Node {
    [Export] private NodePath bgMusicPath;
    [Export] private NodePath winAudioPath;

    private AudioStreamPlayer bgMusic;
    private AudioStreamPlayer winAudio;

    public override void _Ready() {
      this.bgMusic = GetNode<AudioStreamPlayer>(this.bgMusicPath);
      this.winAudio = GetNode<AudioStreamPlayer>(this.winAudioPath);
    }

    public void OnGoalReached() {
      this.bgMusic.Stop();
      this.winAudio.Play();
    }
  }
}