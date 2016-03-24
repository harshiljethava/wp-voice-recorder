using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneApp3.Resources;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using System.ComponentModel;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;



namespace PhoneApp3
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        MemoryStream audioStream = null;
        byte[] audioBuffer = null;
        

        public MainPage()
        {
            InitializeComponent();
            Microphone.Default.BufferDuration = TimeSpan.FromSeconds(1);
            Microphone.Default.BufferReady += microphone_BufferReady;
            recordingList.ItemsSource = new ObservableCollection<VoiceRecording>();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
         }
           
        void microphone_BufferReady(object sender, EventArgs e)
        {
            int count = Microphone.Default.GetData(audioBuffer);
            audioStream.Write(audioBuffer, 0, count);
        }
        void record_Click(object sender, EventArgs e)
        {
            if (Microphone.Default.State == MicrophoneState.Stopped)
            {
                Storyboard SBText = (Storyboard)this.FindName("SBText");
                SBText.Begin();

                recordingList.IsEnabled = false;
                recordingMessage.Visibility = Visibility.Visible;
                audioStream = new MemoryStream();
                audioBuffer = new byte[Microphone.Default.
                GetSampleSizeInBytes(TimeSpan.FromSeconds(1))];
                Microphone.Default.Start();
            }
        }
        async void stopRecord_Click(object sender, EventArgs e)
        {
            TextBlockPlay.Text = "" ;
            if (Microphone.Default.State == MicrophoneState.Started)
            {
                
                Microphone.Default.Stop();
                string filename = await WriteFile();
                audioBuffer = null;
                audioStream = null;
                recordingMessage.Visibility = Visibility.Collapsed;
                recordingList.IsEnabled = true;
                recordingList.ItemsSource.Add(new VoiceRecording { Title = filename, Date = DateTime.Now });
            }
        }

        async Task<string> WriteFile()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await localFolder.CreateFileAsync(
            "recorded.wav",
            CreationCollisionOption.GenerateUniqueName);
            using (Stream fileStream = await file.OpenStreamForWriteAsync())
            {
                using (var writer = new BinaryWriter(fileStream))
                {
                    writer.Write(new char[4] { 'R', 'I', 'F', 'F' });
                    writer.Write((Int32)(36 + audioStream.Length));
                    writer.Write(new char[4] { 'W', 'A', 'V', 'E' });
                    writer.Write(new char[4] { 'f', 'm', 't', ' ' });
                    writer.Write((Int32)16);
                    writer.Write((UInt16)1);
                    writer.Write((UInt16)1);
                    writer.Write((UInt32)16000);
                    writer.Write((UInt32)32000);
                    writer.Write((UInt16)2);
                    writer.Write((UInt16)16);
                    writer.Write(new char[4] { 'd', 'a', 't', 'a' });
                    writer.Write((Int32)audioStream.Length);
                    writer.Write(audioStream.GetBuffer(), 0,
                    (int)audioStream.Length);
                    writer.Flush();
                }
            }
            return file.Name;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await DisplayRecordingNames();
        }
        async Task DisplayRecordingNames()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
            foreach(StorageFile file in files)
            {
                recordingList.ItemsSource.Add(new VoiceRecording { Title = file.Name, Date = file.DateCreated });
            }
        }
        SoundEffectInstance audioPlayerInstance = null;
        async void PlayFile(string filename)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            using (Stream fileStream = await
            localFolder.OpenStreamForReadAsync(filename))
            {
                var soundEffect = SoundEffect.FromStream(fileStream);
                audioPlayerInstance = soundEffect.CreateInstance();
                audioPlayerInstance.Play();
            }
        }
        void play_Click(object sender, EventArgs e)
        {
            
            if (audioPlayerInstance != null &&
            audioPlayerInstance.State == SoundState.Playing)
            {
                audioPlayerInstance.Pause();
            }
            else
            {
                
                TextBlockPlay.Text += "Playing";
                var button = (Button)sender;
                string filename = (string)button.Tag;
                PlayFile(filename);
            }
            
        }
        
        public void recordingList_SelectionChanged1(object sender, EventArgs e) 
        {
           
        }
 
     
     private  void recordingList_SelectionChanged(object sender, SelectionChangedEventArgs e)
       {

          
      }
        
     
      
     
      
    }
}