using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Services.PlaylistService
{
    public interface IPlaylistService
    {
        Task<IEnumerable<string>> GetPlaylistTracksAsync(string genre);
    }
}
