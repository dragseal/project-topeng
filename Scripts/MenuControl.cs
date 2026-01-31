using Godot;
using System;

public partial class MenuControl : Control
{
// Private fields: accessible anywhere in this class
    private int _baseWidth;
    private int _baseHeight;

    public override void _Ready()
    {
        // We set them once
        _baseWidth = (int)ProjectSettings.GetSetting("display/window/size/viewport_width");
        _baseHeight = (int)ProjectSettings.GetSetting("display/window/size/viewport_height");
        // Sets the window to 1280x720 on launch while keeping the game at 360p
        ChangeWindowScale(2); 
    }
    public override void _Process(double delta)
    {
        // Check for alt + Enter key press to toggle fullscreen
        if (Input.IsActionJustPressed("toggleFullscreen"))
        {
            ToggleFullscreen();
        }
    }

    private void ToggleFullscreen()
    {
        var currentMode = DisplayServer.WindowGetMode();
        if (currentMode == DisplayServer.WindowMode.Fullscreen || 
            currentMode == DisplayServer.WindowMode.ExclusiveFullscreen)
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        }
        else
        {
            // 'Exclusive' is better for performance in 4.6
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
        }
    }
    public void ChangeWindowScale(int factor)
    {
        // 1. Calculate the new dimensions
        Vector2I newSize = new Vector2I(_baseWidth * factor, _baseHeight * factor);

        // 2. Apply to the OS window
        DisplayServer.WindowSetSize(newSize);

        // 3. Center the window so it doesn't expand off-screen
        Vector2I screenSize = DisplayServer.ScreenGetSize();
        Vector2I centerPos = (screenSize / 2) - (newSize / 2);
        DisplayServer.WindowSetPosition(centerPos);

    }
}
