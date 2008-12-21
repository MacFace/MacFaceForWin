using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

/// <summary>
/// IOptimusMini インターフェース
/// </summary>
internal interface IOptimusMini : IDisposable
{
    void Dispose();
    bool IsAlive { get; }
    OptimusMini.BrightnessLevel Brightness { set; }
    void DisplayOn();
    void DisplayOff();
    void ShowPicture(int num, Bitmap image);
}

/// <summary>
/// 何もしない IOptimusMini を実装するクラス
/// </summary>
class OptimusMiniMock : IOptimusMini
{
    void IDisposable.Dispose()
    {
    }

    public bool IsAlive
    {
        get { return false; }
    }

    public OptimusMini.BrightnessLevel Brightness
    {
        set { }
    }

    public void DisplayOn()
    {
    }

    public void DisplayOff()
    {
    }

    public void ShowPicture(int num, Bitmap image)
    {
    }

    void IOptimusMini.Dispose()
    {
    }
}

/// <summary>
/// OptimusMini への操作を実装するクラス
/// </summary>
class OptimusMini : IOptimusMini
{
    protected class Driver
    {
        const string DRIVER = "OptimusMini.dll";

        public const int SCREEN_SIZE = 96;
        public const int SCREEN_BUFFER_SIZE = SCREEN_SIZE * SCREEN_SIZE * 3;

        public delegate void OnKeyDownCallbackDelegate(int key);
        public delegate void OnDeviceStateChangedCallbackDelegate(int state);

        [DllImport(DRIVER)]
        public static extern void Init();

        [DllImport(DRIVER)]
        public static extern void Close();

        [DllImport(DRIVER)]
        public static extern int IsAlive();

        [DllImport(DRIVER)]
        public static extern int SendCommand(byte c1, byte c2);

        [DllImport(DRIVER)]
        public static extern int SendCommandBlocking(byte c1, byte c2);

        [DllImport(DRIVER)]
        public static extern int ShowPicture(int p_nButton, byte[] p_lpData);

        [DllImport(DRIVER)]
        public static extern int ShowPictureBlocking(int p_nButton, byte[] p_lpData);

        [DllImport(DRIVER)]
        public static extern int RegisterEventHandler(OnKeyDownCallbackDelegate dk, OnDeviceStateChangedCallbackDelegate ds);
    }

    public OptimusMini()
    {
        Driver.Init();
    }

    public void Dispose()
    {
        DisplayOff();
        Driver.Close();
    }

    public enum BrightnessLevel
    {
        Normal = 0,
    }

    public bool IsAlive
    {
        get { return (Driver.IsAlive() != 0); }
    }

    public BrightnessLevel Brightness
    {
        set { Driver.SendCommand(9, 60); }
    }

    public void DisplayOn()
    {
        Driver.SendCommand(2, 0);
    }

    public void DisplayOff()
    {
        Driver.SendCommand(3, 0);
    }

    public void ShowPicture(int num, Bitmap image)
    {
        BitmapData bData = image.LockBits(new Rectangle(new Point(0, 0), new Size(Driver.SCREEN_SIZE, Driver.SCREEN_SIZE)), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        byte[] bmpBytes = null;

        try
        {
            // number of bytes in the bitmap
            int byteCount = bData.Stride * image.Height;
            bmpBytes = new byte[byteCount];

            // Copy the locked bytes from memory
            Marshal.Copy(bData.Scan0, bmpBytes, 0, byteCount);
        }
        // don't forget to unlock the bitmap!!
        finally
        {
            image.UnlockBits(bData);
        }
        Driver.ShowPictureBlocking(num, bmpBytes);
    }
}
