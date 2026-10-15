using Godot;

namespace TitanCraft.UI;

/// <summary>
/// Carries the last frame of the in-world ending across the change to the
/// victory screen, which is a separate scene with no world behind it. The
/// screen consumes it once so a later visit does not show a stale frame.
/// </summary>
public static class EndingSnapshot
{
    private static ImageTexture? _texture;

    /// <summary>
    /// Copies the viewport's current frame. Headless runs have no rendered
    /// frame to copy, so the victory screen keeps its plain backdrop there.
    /// </summary>
    public static void Capture(Viewport viewport)
    {
        _texture = null;
        if (DisplayServer.GetName() == "headless")
            return;

        var image = viewport.GetTexture()?.GetImage();
        if (image is null || image.IsEmpty())
            return;

        _texture = ImageTexture.CreateFromImage(image);
    }

    /// <summary>Test seam: place a frame as if an ending had been captured.</summary>
    public static void Set(ImageTexture? texture) => _texture = texture;

    /// <summary>Returns the captured frame, if any, and clears it.</summary>
    public static ImageTexture? Take()
    {
        var texture = _texture;
        _texture = null;
        return texture;
    }
}
