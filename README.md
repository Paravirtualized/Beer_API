
# sn76489arduino
Project and Supporting files to turn an arduino into a BBC Micro music player.

In this repository, you will find everything you need to create a BBC Micro Music player, using an Arduino, and a PC.

I'll provide

* Circuit Diagrams (To connect a Texas Instruments SN76489 *[The BBC Micro's hardware sound chip]* to your Arduino)
* The code/Arduino IDE project, to run in the Arduino IDE and program into your arduino
* A VGM Streamer application written in C# using dotnet core, to stream the music data from a PC to the Arduino

## A Few Notes

The project was built on and designed to work with an arduino mega.  It may take a little work to adapt it to work on other arduinos.

The main thing you will have to do to use it on different devices will be to remap the pin numbers you use for the connections to the sound chip.  Iv'e followed good arduino practice however, and provided easy to read pin number constants/defines at the top of the file, so it should be trivial with anyone who has a smattering of arduino experience to change it.

MOST diagrams for the SN76489 sound chip that are available online, are taken from the original Texas Instruments data sheet.  This data sheet **IS WRONG** in the original data sheet, data pin d0 was labeled as being on pin 3 of the chip, when in fact it is actually on pin 10.

Once you've set the pin numbers up, build the circuit, then use the Arduino IDE, load the arduino project, and upload it to your chosen arduino device.

The c# program has only been tested on a PC running under windows, however since it's dotnet core, then it should be possible to run it on ANY MACHINE that you have dot net core installed on.  Dot Net core currently runs on Windows, Linux AND MacOS, instructions to install dotnet core are outside the scope of this project, the place to look for instructions for your platform is as follows:

https://www.microsoft.com/net/learn/get-started/windows

Once you have dotnet core installed and running correctly, use the command line of your system, change to the folder called "VGMPlayer" in the place you cloned this project, and type

**dotnet run**

Running the player is simple, but you'll need to change a few things first, the name of the file to play is in the file 'Program.cs' on line 69, you'll need to change this line to the file you want to play, Iv'e deliberately not made any effort to make this windows player user aware, as it's really only a proof of concept, if you know C# and you want to go changing it to ask for the file name or anything like that, then feel free to do so.

In the file "SerialSender.cs", on line 10 is the name of the serial port your arduino is connected too, you will need to change this to whatever port your OS tells you your arduino is using in the arduino IDE tools menu, it will be something like "COMxx" for windows and "/dev/ttyxxxx" or "/dev/....." on linux.  DOn't ask me about MacOS I don't own one, and I ain't got a clue :-)

To hear the sound output, you **WILL NEED** an amplifed speaker.  Personally (As you can see in the YouTube video here: https://www.youtube.com/watch?v=rOjFRLOkblk ) I use a small USB Powered Xmi mobile phone speaker, but connecting a jack plug to the line in on your PC works just as well, and gives you the added bonus that you can record it too.
