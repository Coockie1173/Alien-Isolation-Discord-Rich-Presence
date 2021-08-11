using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiscordRPC;
using DiscordRPC.Logging;
using System.Threading;

namespace AIDRP
{
    public partial class Form1 : Form
    {
        public static DiscordRpcClient client;
        public static bool IsWorking = false;
        public static Thread ReadThread;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //init main thread
            client = new DiscordRpcClient("874744622872674304");

            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

            client.OnReady += (sender, e) =>
            {
                Console.WriteLine("Received Ready from user {0}", e.User.Username);
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine("Received Update! {0}", e.Presence);
            };

            client.Initialize();

            client.SetPresence(new RichPresence()
            {
                Details = "Alien: Isolation",
                State = "No state found...",
                Assets = new Assets()
                {
                    LargeImageKey = "alien_logo",
                    LargeImageText = "Alien: Isolation",
                    SmallImageKey = "alien_logo",
                }
            });
        }

        private void StartDRPButton_Click(object sender, EventArgs e)
        {
            try
            {
                MemoryHandler.mem = new VAMemory("AI");
                Process GameProcess = Process.GetProcessesByName("AI").FirstOrDefault();
                if(GameProcess == null)
                {
                    throw new Exception("Process not found");
                }
                MemoryHandler.Base = GameProcess.MainModule.BaseAddress;

                IsWorking = false;
                Thread.Sleep(2000);
                ReadThread = new Thread(new ThreadStart(StartReadThread));
                ReadThread.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Game probably not launched: {ex.Message}");
            }
        }

        public void StartReadThread()
        {
            IsWorking = true;

            while(IsWorking)
            {
                try
                {
                    CurrentData.ReadCurrentData();
                    if(CurrentData.CurrentMission != 9)
                    {
                        client.SetPresence(new RichPresence()
                        {
                            Details = "Alien: Isolation",
                            State = CurrentData.CurrentState,
                            Assets = new Assets()
                            {
                                LargeImageKey = CurrentData.MapName.ToLower(),
                                LargeImageText = "Alien: Isolation",
                                SmallImageKey = "alien_logo",
                            }
                        });
                    }
                    else
                    {
                        client.SetPresence(new RichPresence()
                        {
                            Details = "Alien: Isolation",
                            State = CurrentData.CurrentState,
                            Assets = new Assets()
                            {
                                LargeImageKey = "PRESENCE_LV426".ToLower(),
                                LargeImageText = "Alien: Isolation",
                                SmallImageKey = "alien_logo",
                            }
                        });
                    }

                    Thread.Sleep(1000);
                }
                catch(Exception ex)
                {
                    IsWorking = false;
                    MessageBox.Show($"Error when reading: {ex.Message}");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ExePath = Properties.Settings.Default.AIExeLocation;
            if(ExePath == "")
            {
                OpenFileDialog diag = new OpenFileDialog();
                diag.Filter = "*.exe|*.exe";
                DialogResult res =  diag.ShowDialog();
                if(res == DialogResult.OK)
                {
                    if (diag.FileName.Contains("AI.exe"))
                    {
                        Properties.Settings.Default.AIExeLocation = diag.FileName;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            try
            {
                Process.Start(Properties.Settings.Default.AIExeLocation);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error when starting: {ex.Message}");
                MessageBox.Show($"Please try again");
                Properties.Settings.Default.AIExeLocation = "";
            }
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsWorking = false;
        }
    }
}
