using Snap.Hutao.Service.Game.Unlocker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Genshin.Automation
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }
        public static class AppConfig
        {
            public static int Counter = 0;
        }
        //声明WinAPI函数
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        //定义自己的方法
        public static bool WriteSetting(string iniFilePath, string section, string key, string value)
        {
            return 0 == WritePrivateProfileString(section, key, value, iniFilePath) ? false : true;
        }

        public static string ReadSetting(string iniFilePath, string section, string key, string defaultValue)
        {
            StringBuilder result = new StringBuilder(1024);
            GetPrivateProfileString(section, key, defaultValue, result, 1024, iniFilePath);
            return result.ToString();
        }
        private void MainFrm_Load(object sender, EventArgs e)
        {

            //调用方法
            string iniFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\app.ini"; //配置文件路径

            ReadSetting(iniFilePath, "Point", "", "");

            // 实例化新窗口
            RectangleSelectorFrm selectionForm = new RectangleSelectorFrm();

            // 显示新窗口
            this.Hide();
            selectionForm.Show();
            //MessageBox.Show(selectionForm.selectionRectangle.X+","+ selectionForm.selectionRectangle.Y + "," + selectionForm.selectionRectangle.Width + "," + selectionForm.selectionRectangle.Height);
        }
        
        public static int CountWhitePixels(Rectangle rect)
        {
            Bitmap bitmap = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(rect.Location, Point.Empty, rect.Size, CopyPixelOperation.SourceCopy);
            }

            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                              ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            try
            {
                var pixelBytes = new byte[bitmapData.Stride * bitmap.Height];
                Marshal.Copy(bitmapData.Scan0, pixelBytes, 0, pixelBytes.Length);

                int count = 0;
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        var pixelIndex = y * bitmapData.Stride + x * 4;
                        // BGR
                        if (pixelBytes[pixelIndex] == 255 &&
                            pixelBytes[pixelIndex + 1] == 255 &&
                            pixelBytes[pixelIndex + 2] == 255)
                        {
                            count++;
                        }
                    }
                }
                return count;
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
        private async void UnlockStart()
        {
            // Get a game process.
            Process gameProcess = new Process();
            gameProcess.StartInfo.FileName = @"D:\Game\Genshin Impact\Genshin Impact Game\YuanShen.exe";

            // use the process
            GameFpsUnlocker unlocker = new(gameProcess, 144);

            // Start the game process.
            gameProcess.Start();

            // prepare the arguments.
            TimeSpan find = TimeSpan.FromMilliseconds(100);
            TimeSpan limit = TimeSpan.FromMilliseconds(10000);
            TimeSpan adjust = TimeSpan.FromMilliseconds(2000);

            // unlock fps and wait for the process exit.
            // Exception will throw immediately when occurs.

            try
            {
                await unlocker.UnlockAsync(find, limit, adjust);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
