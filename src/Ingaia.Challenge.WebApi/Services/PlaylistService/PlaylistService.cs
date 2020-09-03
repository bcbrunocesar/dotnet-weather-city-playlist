using System.Threading.Tasks;
using Ingaia.Challenge.WebApi.Config;
using Microsoft.Extensions.Options;
using SpotifyAPI.Web;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Ingaia.Challenge.WebApi.Constants;
using System;

namespace Ingaia.Challenge.WebApi.Services.PlaylistService
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IOptions<PlaylistConfig> _playlistConfig;
        private readonly ILogger<PlaylistService> _logger;
        private readonly SpotifyClient _spotifyClient;        

        public PlaylistService(IOptions<PlaylistConfig> playlistConfig, ILogger<PlaylistService> logger)
        {
            _playlistConfig = playlistConfig;
            _logger = logger;

            _spotifyClient = CreateSpotifyInstance();
        }

        public async Task<IEnumerable<string>> GetPlaylistTracksAsync(string genre)
        {
            try
            {
                var tracks = new List<string>();
                var playlistSearch = await SearchPlaylistByGenre(genre);
                if (playlistSearch == null)
                {
                    _logger.LogInformation(string.Format(LogMessages.PLAYLIST_GENRE_NOT_FOUND, genre));
                    return tracks;
                }

                var playlist = await GetPlaylistById(playlistSearch.Id);
                if (playlist == null)
                {
                    _logger.LogInformation(string.Format(LogMessages.PLAYLIST_NOT_FOUND, playlistSearch.Id));
                    return tracks;
                }

                foreach (var item in playlist.Tracks.Items)
                {
                    var fullTrack = (FullTrack)item.Track;
                    tracks.Add(fullTrack.Name);
                }

                return tracks;
            }
            catch (Exception error)
            {
                _logger.LogError(error, error.Message);
                return default;
            }
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

        private SpotifyClient CreateSpotifyInstance()
        {
            var config = SpotifyClientConfig
             .CreateDefault()
             .WithAuthenticator(new ClientCredentialsAuthenticator(
                 _playlistConfig.Value.ClientId,
                 _playlistConfig.Value.SecretKey));

            return new SpotifyClient(config);
        }
    }
}
