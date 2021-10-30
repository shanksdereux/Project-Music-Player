using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Project_Music_Player
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            trackVolume.Value = 50;
            lblVolume.Text = "50%";
        }

        string[] path, files;

        private void trackList_SelectedIndexChanged(object sender, EventArgs e)
        {
            player.URL = path[trackList.SelectedIndex];
            player.Ctlcontrols.play();

            try
            {
                var file = TagLib.File.Create(path[trackList.SelectedIndex]);
                var bin = (byte[])(file.Tag.Pictures[0].Data.Data);
                pictureBox1.Image = Image.FromStream(new MemoryStream(bin));
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.stop();
            playerBar.Value = 0;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.play();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (trackList.SelectedIndex < trackList.Items.Count-1)
            {
                trackList.SelectedIndex = trackList.SelectedIndex + 1;
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (trackList.SelectedIndex > 0)
            {
                trackList.SelectedIndex = trackList.SelectedIndex - 1;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (player.playState ==WMPLib.WMPPlayState.wmppsPlaying)
            {
                playerBar.Maximum = (int)player.Ctlcontrols.currentItem.duration;
                playerBar.Value = (int)player.Ctlcontrols.currentPosition;

                try
                {
                    lbl_track_start.Text = player.Ctlcontrols.currentPositionString;
                    lbl_track_end.Text = player.Ctlcontrols.currentItem.durationString.ToString();
                }
                catch
                {

                }
            }  
        }

        private void trackVolume_Scroll(object sender, EventArgs e)
        {
            player.settings.volume = trackVolume.Value;
            lblVolume.Text = trackVolume.Value.ToString() + "%";
        }

        private void playerBar_MouseDown(object sender, MouseEventArgs e)
        {
            player.Ctlcontrols.currentPosition = player.currentMedia.duration * e.X / playerBar.Width;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                files = openFileDialog.FileNames;
                path = openFileDialog.FileNames;
                for (int i = 0; i < files.Length; i++)
                {
                    trackList.Items.Add(files[i]);
                }
            }
        }
    }
}
