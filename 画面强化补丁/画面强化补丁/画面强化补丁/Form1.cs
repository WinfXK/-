using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace 画面强化补丁
{

    public partial class Form1 : Form
    {
        private static readonly String DirName = "C:\\Users\\" + System.Environment.UserName + "\\AppData\\Local\\Frontier Developments\\Elite Dangerous\\Options\\Graphics\\";
        private readonly String BakFileName = DirName+"GraphicsConfigurationOverride.xml.bak";
        private readonly String DataFileName= DirName+"\\GraphicsConfigurationOverride.xml";
        private readonly String MyDataFileName = "v520.xml";
        private readonly String SettingXml = DirName + "Settings.xml";
        private readonly String BakSettingXml = DirName + "Settings.xml.bak";
        private readonly String s= "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><GraphicsOptions><Version>1</Version><PresetName>Custom</PresetName><StereoscopicMode>0</StereoscopicMode><IPDAmount>0.001000</IPDAmount><AMDCrashFix>false</AMDCrashFix><FOV>72.000000</FOV><HighResScreenCapAntiAlias>3</HighResScreenCapAntiAlias><HighResScreenCapScale>4</HighResScreenCapScale><GammaOffset>0.000000</GammaOffset><DisableGuiEffects>false</DisableGuiEffects><StereoFocalDistance>25.000000</StereoFocalDistance><StencilDump>false</StencilDump><ShaderWarming>true</ShaderWarming><VehicleMotionBlackout>false</VehicleMotionBlackout><VehicleMaintainHorizonCamera>false</VehicleMaintainHorizonCamera><DisableCameraShake>false</DisableCameraShake></GraphicsOptions>";
        public Form1()
        {
            InitializeComponent();
            if (!File.Exists(BakFileName)&&!File.Exists(BakSettingXml))
            {
                button1.Hide();
            }
            else
            {
                button1.Show();
            }
            if(!Directory.Exists(DirName))
                Directory.CreateDirectory(DirName);
            ThreadStart childref = new ThreadStart(VUpdate);
            Thread childThread = new Thread(childref);
            childThread.Start();
        }
        private void VUpdate()
        {
            String s = DoGetRequestSendData("http://ed.Winfxk.cn/upv.php?v=2");
            if (s == null || s.Equals(""))
                return;
            if (s.Equals("true"))
            {
                DialogResult dr;
                dr = MessageBox.Show("检查到更新版本！点击确定立即跳转下载。", "提示", MessageBoxButtons.OKCancel,
                         MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if(dr== DialogResult.OK)
                {
                    System.Diagnostics.Process.Start("http://ed.Winfxk.cn/upv.php");
                }
            }
        }
        public static string DoGetRequestSendData(string url)
        {
            HttpWebRequest hwRequest = null;
            string strResult = string.Empty;
            try
            {
                hwRequest = (System.Net.HttpWebRequest)WebRequest.Create(url);
                hwRequest.Method = "GET";
                hwRequest.ContentType = "application/x-www-form-urlencoded";
            }
            catch (Exception)
            {
            }
            try
            {
                HttpWebResponse hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.ASCII);
                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();
            }
            catch (Exception)
            {
            }
            return strResult;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(DirName))
                Directory.CreateDirectory(DirName);
            if (!File.Exists(BakFileName)&&File.Exists(DataFileName))
            {
                FileInfo f1 = new FileInfo(DataFileName);
                f1.CopyTo(BakFileName, true);
            }
            Assembly assm = Assembly.GetExecutingAssembly();
            StreamWriter w = new StreamWriter(DataFileName);
            w.Write(new System.IO.StreamReader(assm.GetManifestResourceStream("画面强化补丁.Resources." + MyDataFileName)).ReadToEnd());
            w.Close();
            if (checkBox1.Checked)
            {
                if (!File.Exists(BakSettingXml) && File.Exists(SettingXml))
                {
                    FileInfo f2 = new FileInfo(SettingXml);
                    f2.CopyTo(BakSettingXml, true);
                }
                StreamWriter xw = new StreamWriter(SettingXml);
                xw.Write(s);
                xw.Close();
            }
            if (!File.Exists(BakFileName) && !File.Exists(BakSettingXml))
            {
                button1.Hide();
            }
            else
            {
                button1.Show();
            }
            MessageBox.Show("配置完成！重启游戏生效！", "提示");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(DirName))
                Directory.CreateDirectory(DirName);
            if (!File.Exists(BakFileName) && !File.Exists(BakSettingXml))
            {
                MessageBox.Show("未找到备份数据！", "提示");
                return;
            }
            else
            {
                if (File.Exists(BakFileName)) { 
                    FileInfo f2 = new FileInfo(BakFileName);
                    f2.CopyTo(DataFileName, true);
                    File.Delete(BakFileName);
                 }
                if (File.Exists(BakSettingXml))
                {
                    FileInfo fw = new FileInfo(BakSettingXml);
                    fw.CopyTo(SettingXml,true);
                    File.Delete(BakSettingXml);
                }
                if (!File.Exists(BakFileName))
                {
                    button1.Hide();
                }
                else
                {
                    button1.Show();
                }
                MessageBox.Show("已恢复到默认状态！", "提示");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://jq.qq.com/?_wv=1027&k=2vs25BbN");
        }
    }
}
