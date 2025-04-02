using NAudio.Wave;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class MicrophoneActivatedRecorder
{
    private const int SilenceThreshold = 1000; // Silence threshold (volume level)
    private WaveInEvent? waveIn;
    private WaveFileWriter? writer;
    private string? filePath;
    private bool isRecording = false;

    public async Task StartRecording(CancellationToken cancellationToken)
    {
        // Initialize the microphone input
        waveIn = new WaveInEvent();
        waveIn.DeviceNumber = 0; // Default microphone
        waveIn.WaveFormat = new WaveFormat(44100, 1); // 44.1kHz, Mono
        waveIn.DataAvailable += OnDataAvailable;
        waveIn.RecordingStopped += OnRecordingStopped;

        // Create a directory for saving recordings (ensure the directory exists)
        string directoryPath = "D:\\";//Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Recordings");
        Directory.CreateDirectory(directoryPath);

        // Generate the file path with a timestamp
        filePath = Path.Combine(directoryPath, $"Recording_{DateTime.Now:yyyyMMdd_HHmmss}.wav");

        Console.WriteLine($"Starting to monitor microphone input. Saving to: {filePath}");

        // Start monitoring and recording
        waveIn.StartRecording();
        await MonitorMicrophoneAsync(cancellationToken);
    }

    private async Task MonitorMicrophoneAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            // If recording and microphone is idle, stop the recording
            if (isRecording && writer != null && writer.Length == 0)
            {
                StopRecording();
            }

            await Task.Delay(500); // Check every 500 ms
        }
    }

    private void OnDataAvailable(object sender, WaveInEventArgs e)
    {
        // Check if audio data is received
        if (e.BytesRecorded > 0)
        {
            if (!isRecording)
            {
                StartRecordingToFile();
            }

            writer?.Write(e.Buffer, 0, e.BytesRecorded); // Write the audio buffer to the file
        }
        else
        {
            // If no sound is detected for a period, stop recording
            if (isRecording && writer?.Length == 0)
            {
                StopRecording();
            }
        }
    }

    private void StartRecordingToFile()
    {
        writer = new WaveFileWriter(filePath, waveIn?.WaveFormat);
        isRecording = true;
        Console.WriteLine("Recording started...");
    }

    private void StopRecording()
    {
        if (isRecording)
        {
            waveIn?.StopRecording();
            writer?.Dispose();
            isRecording = false;
            Console.WriteLine($"Recording stopped. File saved to: {filePath}");
        }
    }

    private void OnRecordingStopped(object sender, StoppedEventArgs e)
    {
        writer?.Dispose();
        waveIn?.Dispose();
    }

    public void Dispose()
    {
        writer?.Dispose();
        waveIn?.Dispose();
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            var recorder = new MicrophoneActivatedRecorder();

            // Start recording with a cancellation token
            await recorder.StartRecording(cancellationTokenSource.Token);

            // Press any key to stop the application
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

            cancellationTokenSource.Cancel();
        }
    }
}