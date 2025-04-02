# Audiorecordingapp
﻿MicrophoneActivatedRecorder

A C# application that records audio from the microphone only when it detects sound. This app monitors microphone input and starts recording when sound is detected above a specified threshold. It saves the audio in a .wav format and stops recording if no sound is detected.

##Features

	Starts recording when microphone input exceeds a specified threshold (default is 1000).

	Saves the audio to a .wav file with a timestamped filename.

	Stops recording automatically when no sound is detected.

	Supports cancellation with a CancellationToken.

##Prerequisites

	To run this application, you need:

	.NET 5.0 or higher (for .NET Core or .NET Framework projects)

	NAudio library: A library for handling audio input and output. This project uses NAudio for microphone capture.

	To install NAudio, run the following command in your project directory:


##add package NAudio

##Setup and Installation

	Open the project in Visual Studio or your preferred IDE.

	Ensure you have the NAudio NuGet package installed as mentioned above.

	Build and run the project.

##Usage
	When you run the program, it will monitor the microphone for input.

	If sound is detected, it starts recording the audio and saves it to the D:\ drive by default (you can change the path in the StartRecording method).

	If no sound is detected, it will automatically stop recording after a short period.

	The application will print information to the console about when the recording starts and stops, along with the file path where the audio is saved.
