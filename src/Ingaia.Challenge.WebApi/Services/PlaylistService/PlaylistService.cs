using System.Threading.Tasks;
using Ingaia.Challenge.WebApi.Config;
using Microsoft.Extensions.Options;
using SpotifyAPI.Web;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Ingaia.Challenge.WebApi.Constants;
using System;
using Ingaia.Challenge.WebApi.Infrastructure.Notificator;
using Ingaia.Challenge.WebApi.Infrastructure.Enums;

namespace Ingaia.Challenge.WebApi.Services.PlaylistService
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IOptions<PlaylistConfig> _playlistConfig;
        private readonly ILogger<PlaylistService> _logger;
        private readonly INotificator _notificator;
        private readonly SpotifyClient _spotifyClient;

        public PlaylistService
        (
            IOptions<PlaylistConfig> playlistConfig,
            ILogger<PlaylistService> logger,
            INotificator notificator
        )
        {
            _playlistConfig = playlistConfig;
            _logger = logger;
            _notificator = notificator;

            _spotifyClient = CreateSpotifyInstance();
        }

        public async Task<IEnumerable<string>> GetPlaylistTracksAsync(string genre)
        {
            try
            {
                var playlistSearch = await SearchPlaylistByGenre(genre);
                if (playlistSearch is null)
                {
                    _notificator.Handle("Playlist não encontrada.", ENotificationType.NotFound);
                    _logger.LogInformation(LogMessagesConstant.PLAYLIST_GENRE_NOT_FOUND, genre);

                    return default;
                }

                var playlist = await GetPlaylistById(playlistSearch.Id);
                if (playlist is null)
                {
                    _notificator.Handle("Ocorreu um erro ao buscar a playlist no serviço de streaming.", ENotificationType.Failed);
                    _logger.LogInformation(LogMessagesConstant.PLAYLIST_NOT_FOUND, playlistSearch.Id);

                    return default;
                }

                return GetTracksName(playlist.Tracks.Items);
            }
            catch (Exception error)
            {
                _notificator.Handle(error.Message, ENotificationType.Failed);
                _logger.LogError(error, error.Message);

                return default;
            }
        }

        private IEnumerable<string> GetTracksName(List<PlaylistTrack<IPlayableItem>> playlistTracks)
        {
            var tracks = new List<string>();

            foreach (var item in playlistTracks)
            {
                var fullTrack = (FullTrack)item.Track;
                tracks.Add(fullTrack.Name);
            }

            return tracks;
        }

        private async Task<SimplePlaylist> SearchPlaylistByGenre(string genre)
        {
            var searchRequest = new SearchRequest(SearchRequest.Types.Playlist, genre);
            var spotifyClient = await _spotifyClient.Search.Item(searchRequest);

            return spotifyClient.Playlists.Items.Count > 0
                ? spotifyClient.Playlists.Items[0]
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
