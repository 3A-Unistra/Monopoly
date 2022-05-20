# Monopoly

This repository houses the Unity project for the game as well as the original CI
pipeline config imported from the private GitLab instance and a few shell
scripts for management.

## Usage

To use the project, you must first install [Unity](https://unity.com/). The
project was created using Unity LTS 2020.3.26f1. Using any earlier or later
version may or may not be supported.

Once Unity is installed, you may import the Unity project which is housed in the
`Monopoly` subfolder, at which point the project may be opened in the Unity
Editor. There are additional C# solutions available in the project for use in
Visual Studio.

## Platform support

The game was designed to run on Windows, Linux (Mono and IL2CPP), macOS as well
as WebGL. Notional tests were run on the Android compilation but it is known to
have major text rendering issues. iOS compilation has not been tested.

## Security

SSL certificate validity is bypassed when connecting via. HTTPS to obtain a game
token as the original server had used an invalid certificate, and thus the
bypass should be removed if used on another server.

[See here](https://github.com/3A-Unistra/Monopoly/blob/main/Monopoly/Assets/Scripts/UI/MenuLogin.cs#L31)
for the relevant code.

## License

This project is licensed under the GNU Affero General Public License v3.0.
Please see the included `COPYING` file for more information.

