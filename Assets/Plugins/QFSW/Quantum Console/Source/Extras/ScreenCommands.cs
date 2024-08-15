#if !QC_DISABLED && !QC_DISABLE_BUILTIN_ALL && !QC_DISABLE_BUILTIN_EXTRA
using System.Collections.Generic;
using UnityEngine;

namespace QFSW.QC.Extras
{
    public static class ScreenCommands
    {
        [Command("fullscreen", "Get or set the fullscreen state of the application.")]
        private static bool Fullscreen
        {
            get => Screen.fullScreen;
            set => Screen.fullScreen = value;
        }

        [Command("screen-dpi", "Get the DPI of the current device's screen.")]
        private static float DPI => Screen.dpi;

        [Command("screen-orientation", "Get or set the orientation of the screen.")]
        [CommandPlatform(Platform.MobilePlatforms)]
        private static ScreenOrientation Orientation
        {
            get => Screen.orientation;
            set => Screen.orientation = value;
        }

        [Command("current-resolution", "Get the current resolution of the application or window.")]
        private static Resolution GetCurrentResolution()
        {
            return new Resolution
            {
                width = Screen.width,
                height = Screen.height,
                refreshRate = Screen.currentResolution.refreshRate
            };
        }

        [Command("supported-resolutions", "Get all resolutions supported by this device in fullscreen mode.")]
        [CommandPlatform(Platform.AllPlatforms ^ Platform.WebGLPlayer)]
        private static IEnumerable<Resolution> GetSupportedResolutions()
        {
            foreach (Resolution resolution in Screen.resolutions)
            {
                yield return resolution;
            }
        }

        [Command("set-resolution", "Set the resolution of the current application. Optionally, set the fullscreen state as well.")]
        private static void SetResolution(int width, int height, bool fullscreen = false)
        {
            Screen.SetResolution(width, height, fullscreen);
        }

        [Command("capture-screenshot", "Capture a screenshot and save it to the supplied file path as a PNG. If superSize is supplied, the screenshot will be captured at a higher than native resolution.")]
        private static void CaptureScreenshot(
            [CommandParameterDescription("The name of the file to save the screenshot in")] string filename,
            [CommandParameterDescription("Factor by which to increase resolution")] int superSize = 1)
        {
            try
            {
                ScreenCapture.CaptureScreenshot(filename, superSize);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to capture screenshot: {ex.Message}");
            }
        }
    }
}
#endif
