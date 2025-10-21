using Godot;
using System.Collections.Generic;

namespace Dong
{
    public partial class AudioManager : Node
    {
        /// <summary>
        /// Return the instance of AudioManager class for sound playing
        /// </summary>
        public static AudioManager Instance { get; private set; }
        /// <summary>
        /// Dictionary containing list of all child of type AudioStreamPlayer inside AudioManager scene node
        /// </summary>
        private readonly Dictionary<string, AudioStreamPlayer> _sounds = [];

        public override void _Ready()
        {
            if (Instance != null)
            {
                GD.PushWarning("AudioManager already exist. This instance try will be cancelled.");
                QueueFree();
                return;
            }

            Instance = this;

            foreach (Node child in GetChildren())
            {
                if (child is AudioStreamPlayer player)
                {
                    _sounds[child.Name.ToString().ToLower()] = player;
                }
            }
        }

        public void PlaySound(string name)
        {
            name = name.ToLower();

            if (_sounds.TryGetValue(name, out var player))
            {
                player.Play();
            }
            else
            {
                GD.PushWarning($"Doesn't exist audio with name: {name}");
            }
        }

        public void StopSound(string name)
        {
            name = name.ToLower();

            if (_sounds.TryGetValue(name, out var player))
            {
                player.Stop();
            }
        }

        public void StopAll()
        {
            foreach (var player in _sounds.Values)
                player.Stop();
        }
    }
}
