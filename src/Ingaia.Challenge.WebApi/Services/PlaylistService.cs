using Ingaia.Challenge.WebApi.Interfaces;
using System.Threading.Tasks;
using Ingaia.Challenge.WebApi.Config;
using Microsoft.Extensions.Options;
using SpotifyAPI.Web;
using System.Collections.Generic;

namespace Ingaia.Challenge.WebApi.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IOptions<PlaylistConfig> _playlistConfig;
        private readonly SpotifyClient _spotifyClient;

        public PlaylistService(IOptions<PlaylistConfig> playlistConfig)
        {
            _playlistConfig = playlistConfig;

            var config = SpotifyClientConfig
                .CreateDefault()
                .WithAuthenticator(new ClientCredentialsAuthenticator(
                    _playlistConfig.Value.ClientId,
                    _playlistConfig.Value.SecretKey));

            _spotifyClient = new SpotifyClient(config);
        }

        public async Task<IEnumerable<string>> GetPlaylistTracksAsync(string genre)
        {
            var tracks = new List<string>();
            var playlistSearch = await SearchPlaylistByGenre(genre);
            if (playlistSearch == null)
            {
                return tracks;
            }

            var playlist = await GetPlaylistById(playlistSearch.Id);
            if (playlist == null)
            {
                return tracks;
            }
            
            foreach (var item in playlist.Tracks.Items)
            {
                var fullTrack = (FullTrack)item.Track;
                tracks.Add(fullTrack.Name);
            }

            return tracks;
        }

        private async Task<SimplePlaylist> SearchPlaylistByGenre(string genre)
        {
            var searchRequest = new SearchRequest(SearchRequest.Types.Playlist, genre);
            var playlist = await _spotifyClient.Search.Item(searchRequest);

            return playlist.Playlists.Items.Count > 0
                ? playlist.Playlists.Items[0]
                : default;
        }

        private async Task<FullPlaylist> GetPlaylistById(string playlistId)
        {
            return await _spotifyClient.Playlists.Get(playlistId);
        }
    }
}
